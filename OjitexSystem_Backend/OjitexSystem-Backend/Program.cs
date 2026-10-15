using OjitexSystem_Backend.Infrastructure;
using OjitexSystem_Backend.Services.Auth;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOjitexApplication(builder.Configuration, builder.Environment);

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

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// API không tồn tại trả 404.
app.MapFallback("/api/{**path}", () => Results.NotFound());

// Các URL giao diện được Angular xử lý.
app.MapFallbackToFile("index.html");
app.Run();
