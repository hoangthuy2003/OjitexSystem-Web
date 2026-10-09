namespace OjitexSystem_Backend.Services;

public interface ILegacyPasswordMigrationService
{
    Task<int> ResetLegacyPasswordsAsync();
}
