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

namespace OjitexSystem_Backend.Tests;

public sealed class ApiPipelineTests
{
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

    private static async Task<WebApplication> StartAsync()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
        builder.WebHost.UseTestServer();
        builder.Logging.ClearProviders();
        builder.Services.AddOjitexApplication(builder.Configuration, builder.Environment);
        builder.Services.AddControllers().AddApplicationPart(typeof(AuthController).Assembly);
        builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        var app = builder.Build();
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/test-error", () => Task.FromException(new InvalidOperationException("sensitive-internal-detail")));
        await app.StartAsync();
        return app;
    }
}
