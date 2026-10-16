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
    [Theory]
    [InlineData(false, false, HttpStatusCode.OK)]
    [InlineData(true, false, HttpStatusCode.Forbidden)]
    [InlineData(false, true, HttpStatusCode.Forbidden)]
    public async Task CorrugatorEndpointRechecksAccountStateForAuthenticatedUsers(
        bool locked, bool deleted, HttpStatusCode expected)
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser(locked: locked, deleted: deleted);
        var service = new RecordingCorrugatorService();
        await using var app = await StartAsync(services =>
        {
            services.AddScoped<IAccessService>(_ => new AccessService(fixture.Access));
            services.AddSingleton<ICorrugatorOnlineService>(service);
        });
        using var client = app.GetTestClient();
        var token = app.Services.GetRequiredService<JwtTokenService>().CreateToken("alice", []).Token;
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        using var response = await client.GetAsync("/api/factory/corrugator-online");
        Assert.Equal(expected, response.StatusCode);
        Assert.Equal(expected == HttpStatusCode.OK ? 1 : 0, service.Calls);
        if (expected == HttpStatusCode.OK)
        {
            Assert.True(response.Headers.CacheControl?.NoStore);
            using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal("2026-10-16", body.RootElement.GetProperty("reportDate").GetString());
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

    private static async Task<WebApplication> StartAsync(Action<IServiceCollection>? configure = null)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
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

    private sealed class RecordingCorrugatorService : ICorrugatorOnlineService
    {
        public int Calls { get; private set; }
        public Task<CorrugatorOnlineDto> GetTodayAsync(CancellationToken cancellationToken = default)
        {
            Calls++;
            return Task.FromResult(new CorrugatorOnlineDto(new DateOnly(2026, 10, 16),
                new DateTime(2026, 10, 16, 9, 0, 0), []));
        }
    }
}
