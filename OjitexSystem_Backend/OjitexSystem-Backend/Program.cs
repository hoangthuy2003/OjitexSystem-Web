using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OjitexSystem_Backend.Controllers;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ProductionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OJITEXHP")));
builder.Services.AddDbContext<AuthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EVOLIO")));

builder.Services.AddScoped<ICurrentStockRepository, CurrentStockRepository>();
builder.Services.AddScoped<ICurrentStockService, CurrentStockService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<ILegacyPasswordMigrationService, LegacyPasswordMigrationService>();
builder.Services.AddScoped<PasswordHasher<IeUser>>();

var issuer = builder.Configuration["Authentication:Issuer"] ?? "OjitexSystem";
var audience = builder.Configuration["Authentication:Audience"] ?? "OjitexSystem";
var signingKey = builder.Configuration["Authentication:SigningKey"];
if (string.IsNullOrWhiteSpace(signingKey))
{
    if (!builder.Environment.IsDevelopment())
    {
        throw new InvalidOperationException(
            "Set Authentication:SigningKey (at least 32 characters) before starting outside Development.");
    }

    signingKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
}

var signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
if (signingKeyBytes.Length < 32)
{
    throw new InvalidOperationException("Authentication:SigningKey must be at least 32 bytes.");
}

builder.Services.AddSingleton(new JwtTokenService(issuer, audience, signingKeyBytes));
builder.Services
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.AdminOnly, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new AdminAccessRequirement());
    });
    options.AddPolicy(AuthorizationPolicies.LogisticsCategory, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new CategoryAccessRequirement("C000000005"));
    });
});
builder.Services.AddScoped<IAuthorizationHandler, AdminAccessHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CategoryAccessHandler>();

builder.Services.AddCors(options =>
{
    var frontendOrigins = new[]
        {
            "http://localhost:4200",
            "http://127.0.0.1:4200"
        }
        .AsEnumerable();
    var configuredOrigin = builder.Configuration["Frontend:Origin"];
    if (!string.IsNullOrWhiteSpace(configuredOrigin))
    {
        frontendOrigins = frontendOrigins.Append(configuredOrigin);
    }

    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(frontendOrigins.Distinct(StringComparer.OrdinalIgnoreCase).ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (args.Any(argument => string.Equals(argument, "--reset-legacy-passwords", StringComparison.OrdinalIgnoreCase)))
{
    await using var scope = app.Services.CreateAsyncScope();
    var migration = scope.ServiceProvider.GetRequiredService<ILegacyPasswordMigrationService>();
    var updatedUsers = await migration.ResetLegacyPasswordsAsync();
    Console.WriteLine($"Password migration completed. {updatedUsers} active account(s) reset to 123456.");
    return;
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
