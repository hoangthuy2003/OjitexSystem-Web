namespace OjitexSystem_Backend.Services.Auth;

public interface ILegacyPasswordMigrationService
{
    Task<int> ResetLegacyPasswordsAsync(CancellationToken cancellationToken = default);
}
