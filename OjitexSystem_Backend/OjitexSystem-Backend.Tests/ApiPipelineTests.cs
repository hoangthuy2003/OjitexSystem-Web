using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OjitexSystem_Backend.Controllers.Auth;
using OjitexSystem_Backend.Infrastructure;
using OjitexSystem_Backend.Dtos.Factory;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Auth;
using OjitexSystem_Backend.Services.Factory;

namespace OjitexSystem_Backend.Tests;

public sealed class ApiPipelineTests
{
    [Fact]
    public async Task DevelopmentRestartPreservesExistingTokensWithoutConfiguredSigningKey()
    {
        var contentRoot = Path.Combine(Path.GetTempPath(), "ojitex-auth-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(contentRoot);
        try
        {
            using var fixture = new AuthTestFixture();
            fixture.AddUser();
            void Configure(IServiceCollection services)
            {
                services.AddScoped<IAccessService>(_ => new AccessService(fixture.Access));
                services.AddSingleton<ICorrugatorOnlineService>(new RecordingCorrugatorService());
            }

            string token;
            await using (var first = await StartAsync(Configure, contentRoot))
            {
                token = first.Services.GetRequiredService<JwtTokenService>().CreateToken("alice", []).Token;
            }

            await using var restarted = await StartAsync(Configure, contentRoot);
            using var client = restarted.GetTestClient();
            client.DefaultRequestHeaders.Authorization = new("Bearer", token);
            using var response = await client.GetAsync("/api/factory/corrugator-online");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        finally
        {
            Directory.Delete(contentRoot, recursive: true);
        }
    }

    [Theory]
    [InlineData("corrugator-online", false, false, HttpStatusCode.OK)]
    [InlineData("corrugator-online", true, false, HttpStatusCode.Forbidden)]
    [InlineData("corrugator-online", false, true, HttpStatusCode.Forbidden)]
    [InlineData("corrugator-history", false, false, HttpStatusCode.OK)]
    [InlineData("corrugator-history", true, false, HttpStatusCode.Forbidden)]
    [InlineData("corrugator-history", false, true, HttpStatusCode.Forbidden)]
    [InlineData("converting-history", false, false, HttpStatusCode.OK)]
    [InlineData("converting-history", true, false, HttpStatusCode.Forbidden)]
    [InlineData("converting-history", false, true, HttpStatusCode.Forbidden)]
    [InlineData("fdc-online", false, false, HttpStatusCode.OK)]
    [InlineData("fdc-online", true, false, HttpStatusCode.Forbidden)]
    [InlineData("fdc-online", false, true, HttpStatusCode.Forbidden)]
    [InlineData("evol-online", false, false, HttpStatusCode.OK)]
    [InlineData("evol-online", true, false, HttpStatusCode.Forbidden)]
    [InlineData("evol-online", false, true, HttpStatusCode.Forbidden)]
    [InlineData("fg-online", false, false, HttpStatusCode.OK)]
    [InlineData("fg-online", true, false, HttpStatusCode.Forbidden)]
    [InlineData("fg-online", false, true, HttpStatusCode.Forbidden)]
    [InlineData("fap-online", false, false, HttpStatusCode.OK)]
    [InlineData("fap-online", true, false, HttpStatusCode.Forbidden)]
    [InlineData("fap-online", false, true, HttpStatusCode.Forbidden)]
    [InlineData("fps-online", false, false, HttpStatusCode.OK)]
    [InlineData("fps-online", true, false, HttpStatusCode.Forbidden)]
    [InlineData("fps-online", false, true, HttpStatusCode.Forbidden)]
    public async Task FactoryEndpointsRecheckAccountStateForAuthenticatedUsers(
        string report, bool locked, bool deleted, HttpStatusCode expected)
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser(locked: locked, deleted: deleted);
        var service = new RecordingCorrugatorService();
        var convertingService = new RecordingConvertingService();
        await using var app = await StartAsync(services =>
        {
            services.AddScoped<IAccessService>(_ => new AccessService(fixture.Access));
            services.AddSingleton<ICorrugatorOnlineService>(service);
            services.AddSingleton<IConvertingOnlineService>(convertingService);
        });
        using var client = app.GetTestClient();
        var token = app.Services.GetRequiredService<JwtTokenService>().CreateToken("alice", []).Token;
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        using var response = await client.GetAsync($"/api/factory/{report}?machineName=OTHER&date=10%2F16%2F2026");
        Assert.Equal(expected, response.StatusCode);
        Assert.Equal(expected == HttpStatusCode.OK ? 1 : 0,
            report.StartsWith("corrugator-") ? service.Calls : convertingService.Calls);
        if (expected == HttpStatusCode.OK)
        {
            Assert.True(response.Headers.CacheControl?.NoStore);
            using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal("2026-10-16", body.RootElement.GetProperty("reportDate").GetString());
            if (!report.StartsWith("corrugator-") && report != "converting-history")
            {
                var machineName = report switch
                {
                    "fdc-online" => "FDC",
                    "evol-online" => "EVOL",
                    "fg-online" => "FG",
                    "fap-online" => "FAP",
                    "fps-online" => "FPS",
                    _ => throw new InvalidOperationException("Unknown converting report")
                };
                Assert.Equal(machineName, convertingService.MachineName);
            }
        }
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("19/10/2026", null)]
    [InlineData("2026-10-19", null)]
    [InlineData("1/10/2026", null)]
    [InlineData("02/31/2026", null)]
    [InlineData("02/29/2026", null)]
    [InlineData("10/19/2026", "2026-10-19")]
    [InlineData("03/04/2026", "2026-03-04")]
    [InlineData("02/29/2024", "2024-02-29")]
    public async Task CorrugatorHistoryRequiresAnExactMonthDayYearDate(string? date, string? expectedDate)
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser();
        var service = new RecordingCorrugatorService();
        await using var app = await StartAsync(services =>
        {
            services.AddScoped<IAccessService>(_ => new AccessService(fixture.Access));
            services.AddSingleton<ICorrugatorOnlineService>(service);
        });
        using var client = app.GetTestClient();
        var token = app.Services.GetRequiredService<JwtTokenService>().CreateToken("alice", []).Token;
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        var query = date is null ? "" : "?date=" + Uri.EscapeDataString(date);
        using var response = await client.GetAsync("/api/factory/corrugator-history" + query);
        Assert.Equal(expectedDate is null ? HttpStatusCode.BadRequest : HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedDate is null ? 0 : 1, service.Calls);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        if (expectedDate is null)
        {
            Assert.Contains("mm/dd/yyyy", body.RootElement.GetProperty("message").GetString());
        }
        else
        {
            Assert.Equal(DateOnly.Parse(expectedDate), service.HistoryDate);
            Assert.Equal(expectedDate, body.RootElement.GetProperty("reportDate").GetString());
            Assert.True(response.Headers.CacheControl?.NoStore);
        }
    }

    [Theory]
    [InlineData("", HttpStatusCode.OK, null, null, null)]
    [InlineData("?date=&process=%20&productCode=", HttpStatusCode.OK, null, null, null)]
    [InlineData("?date=10%2F19%2F2026&process=fdc&productCode=115", HttpStatusCode.OK, "2026-10-19", "fdc", "115")]
    [InlineData("?date=03%2F04%2F2026", HttpStatusCode.OK, "2026-03-04", null, null)]
    [InlineData("?process=FG", HttpStatusCode.OK, null, "FG", null)]
    [InlineData("?productCode=0", HttpStatusCode.OK, null, null, "0")]
    [InlineData("?date=19%2F10%2F2026", HttpStatusCode.BadRequest, null, null, null)]
    [InlineData("?date=02%2F29%2F2026", HttpStatusCode.BadRequest, null, null, null)]
    [InlineData("?productCode=11%25", HttpStatusCode.BadRequest, null, null, null)]
    [InlineData("?productCode=-1", HttpStatusCode.BadRequest, null, null, null)]
    [InlineData("?productCode=12345678", HttpStatusCode.BadRequest, null, null, null)]
    [InlineData("?process=TOOLONG", HttpStatusCode.BadRequest, null, null, null)]
    public async Task ConvertingHistoryAcceptsOptionalCombinedFiltersAndRejectsInvalidInput(
        string query, HttpStatusCode expected, string? date, string? process, string? productCode)
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser();
        var service = new RecordingConvertingService();
        await using var app = await StartAsync(services =>
        {
            services.AddScoped<IAccessService>(_ => new AccessService(fixture.Access));
            services.AddSingleton<IConvertingOnlineService>(service);
        });
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer",
            app.Services.GetRequiredService<JwtTokenService>().CreateToken("alice", []).Token);
        using var response = await client.GetAsync("/api/factory/converting-history" + query);
        Assert.Equal(expected, response.StatusCode);
        Assert.Equal(expected == HttpStatusCode.OK ? 1 : 0, service.Calls);
        if (expected == HttpStatusCode.OK)
        {
            Assert.Equal(date is null ? null : (DateOnly?)DateOnly.Parse(date), service.HistoryDate);
            Assert.Equal(process, service.Process);
            Assert.Equal(productCode, service.ProductCode);
            Assert.True(response.Headers.CacheControl?.NoStore);
        }
        else
        {
            using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.False(string.IsNullOrWhiteSpace(body.RootElement.GetProperty("message").GetString()));
        }
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{\"username\":null,\"password\":null}")]
    [InlineData("{\"username\":\"alice\",\"password\":\"\"}")]
    public async Task InvalidLoginReturnsValidationResponseBeforeDatabaseAccess(string payload)
    {
        await using var app = await StartAsync();
        using var client = app.GetTestClient();
        using var response = await client.PostAsync("/api/auth/login", new StringContent(payload, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(body.RootElement.TryGetProperty("message", out _));
        Assert.True(body.RootElement.TryGetProperty("errors", out _));
    }

    [Theory]
    [InlineData("/api/auth/me")]
    [InlineData("/api/admin/dashboard")]
    [InlineData("/api/StockReport/current")]
    [InlineData("/api/factory/corrugator-online")]
    [InlineData("/api/factory/corrugator-history?date=10%2F19%2F2026")]
    [InlineData("/api/factory/fdc-online")]
    [InlineData("/api/factory/evol-online")]
    [InlineData("/api/factory/fg-online")]
    [InlineData("/api/factory/fap-online")]
    [InlineData("/api/factory/fps-online")]
    [InlineData("/api/factory/converting-history")]
    public async Task ProtectedEndpointsRejectAnonymousRequestsWithoutQueryingDatabase(string endpoint)
    {
        await using var app = await StartAsync();
        using var client = app.GetTestClient();
        using var response = await client.GetAsync(endpoint);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData("application/json")]
    [InlineData("text/plain")]
    public async Task UnexpectedErrorsReturnProblemDetailsWithoutExposingException(string accept)
    {
        await using var app = await StartAsync();
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Accept.ParseAdd(accept);
        using var response = await client.GetAsync("/test-error");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("sensitive-internal-detail", content);
        using var body = JsonDocument.Parse(content);
        Assert.True(body.RootElement.TryGetProperty("message", out _));
        Assert.True(body.RootElement.TryGetProperty("traceId", out _));
    }

    private static async Task<WebApplication> StartAsync(
        Action<IServiceCollection>? configure = null, string? contentRoot = null)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Development",
            ContentRootPath = contentRoot
        });
        // Only the restart regression exercises key persistence; other tests need no key files.
        builder.Configuration["Authentication:SigningKey"] = contentRoot is null
            ? "ojitex-tests-only-signing-key-at-least-32-bytes"
            : null;
        builder.WebHost.UseTestServer();
        builder.Logging.ClearProviders();
        builder.Services.AddOjitexApplication(builder.Configuration, builder.Environment);
        builder.Services.AddControllers().AddApplicationPart(typeof(AuthController).Assembly);
        builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        configure?.Invoke(builder.Services);
        var app = builder.Build();
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/test-error", () => Task.FromException(new InvalidOperationException("sensitive-internal-detail")));
        await app.StartAsync();
        return app;
    }

    private sealed class RecordingConvertingService : IConvertingOnlineService
    {
        public int Calls { get; private set; }
        public string? MachineName { get; private set; }
        public DateOnly? HistoryDate { get; private set; }
        public string? Process { get; private set; }
        public string? ProductCode { get; private set; }

        public Task<ConvertingHistoryDto> GetHistoryAsync(
            DateOnly? date, string? process, string? productCode, CancellationToken cancellationToken = default)
        {
            Calls++;
            HistoryDate = date;
            Process = process;
            ProductCode = productCode;
            return Task.FromResult(new ConvertingHistoryDto(date, new DateTime(2026, 10, 20, 9, 0, 0), []));
        }
        public Task<ConvertingOnlineDto> GetTodayAsync(string machineName, CancellationToken cancellationToken = default)
        {
            Calls++;
            MachineName = machineName;
            return Task.FromResult(new ConvertingOnlineDto(new DateOnly(2026, 10, 16),
                new DateTime(2026, 10, 16, 9, 0, 0), []));
        }
    }

    private sealed class RecordingCorrugatorService : ICorrugatorOnlineService
    {
        public int Calls { get; private set; }
        public DateOnly? HistoryDate { get; private set; }
        public Task<CorrugatorOnlineDto> GetTodayAsync(CancellationToken cancellationToken = default)
        {
            Calls++;
            return Task.FromResult(new CorrugatorOnlineDto(new DateOnly(2026, 10, 16),
                new DateTime(2026, 10, 16, 9, 0, 0), []));
        }

        public Task<CorrugatorOnlineDto> GetHistoryAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            Calls++;
            HistoryDate = date;
            return Task.FromResult(new CorrugatorOnlineDto(date,
                new DateTime(2026, 10, 20, 9, 0, 0), []));
        }
    }
}
