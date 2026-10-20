using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Admin;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Repositories.Logistics;
using OjitexSystem_Backend.Repositories.Factory;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Admin;
using OjitexSystem_Backend.Services.Auth;
using OjitexSystem_Backend.Services.Logistics;
using OjitexSystem_Backend.Services.Factory;
namespace OjitexSystem_Backend.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOjitexApplication(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var details = new ValidationProblemDetails(context.ModelState);
                details.Extensions["message"] = "Dữ liệu yêu cầu không hợp lệ.";
                return new BadRequestObjectResult(details);
            };
        });
        services.AddOpenApi();
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.Configure<ExceptionHandlerOptions>(options =>
            options.SuppressDiagnosticsCallback = _ => false);
        services.AddPersistence(configuration);
        services.AddBusinessServices();
        services.AddSecurity(configuration, environment);
        services.AddFrontendCors(configuration);
        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProductionContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OJITEXHP")));
        services.AddDbContext<AuthContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("EVOLIO")));

        services.AddScoped<IStockReportRepository, StockReportRepository>();
        services.AddScoped<ICorrugatorOnlineRepository, CorrugatorOnlineRepository>();
        services.AddScoped<IConvertingOnlineRepository, ConvertingOnlineRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccessRepository, AccessRepository>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
    }

    private static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IStockReportService, StockReportService>();
        services.AddScoped<ICorrugatorOnlineService, CorrugatorOnlineService>();
        services.AddScoped<IConvertingOnlineService, ConvertingOnlineService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();
        services.AddScoped<ILegacyPasswordMigrationService, LegacyPasswordMigrationService>();
        services.AddScoped<IAccessService, AccessService>();
        services.AddScoped<PasswordHasher<IeUser>>();
    }

    private static string GetDevelopmentSigningKey(string contentRootPath)
    {
        var directory = Path.Combine(contentRootPath, ".local");
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, "jwt-signing-key");
        // Keep the development key outside build output so restarts preserve valid tokens.
        using var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        if (stream.Length == 0)
        {
            var key = Encoding.UTF8.GetBytes(Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
            stream.Write(key);
            stream.Flush(flushToDisk: true);
            stream.Position = 0;
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static void AddSecurity(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var issuer = configuration["Authentication:Issuer"] ?? "OjitexSystem";
        var audience = configuration["Authentication:Audience"] ?? "OjitexSystem";
        var signingKey = configuration["Authentication:SigningKey"];
        if (string.IsNullOrWhiteSpace(signingKey))
        {
            if (!environment.IsDevelopment())
            {
                throw new InvalidOperationException(
                    "Set Authentication:SigningKey (at least 32 characters) before starting outside Development.");
            }

            signingKey = GetDevelopmentSigningKey(environment.ContentRootPath);
        }

        var signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
        if (signingKeyBytes.Length < 32)
        {
            throw new InvalidOperationException("Authentication:SigningKey must be at least 32 bytes.");
        }

        services.AddSingleton(provider => new JwtTokenService(
            issuer, audience, signingKeyBytes, provider.GetRequiredService<TimeProvider>()));
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.ActiveUser, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new ActiveUserRequirement());
            });
            options.AddPolicy(AuthorizationPolicies.AdminOnly, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new AdminAccessRequirement());
            });
            options.AddPolicy(AuthorizationPolicies.LogisticsCategory, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new CategoryAccessRequirement(AccessConstants.LogisticsCategoryId));
            });
        });
        services.AddScoped<IAuthorizationHandler, AdminAccessHandler>();
        services.AddScoped<IAuthorizationHandler, ActiveUserHandler>();
        services.AddScoped<IAuthorizationHandler, CategoryAccessHandler>();

        services.AddHttpContextAccessor();
    }

    private static void AddFrontendCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var frontendOrigins = new[]
                {
                    "http://localhost:4200",
                    "http://127.0.0.1:4200"
                }
                .AsEnumerable();
            var configuredOrigin = configuration["Frontend:Origin"];
            if (!string.IsNullOrWhiteSpace(configuredOrigin))
            {
                frontendOrigins = frontendOrigins.Append(configuredOrigin);
            }

            options.AddPolicy("Frontend", policy =>
                policy.WithOrigins(frontendOrigins.Distinct(StringComparer.OrdinalIgnoreCase).ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
    }
}
