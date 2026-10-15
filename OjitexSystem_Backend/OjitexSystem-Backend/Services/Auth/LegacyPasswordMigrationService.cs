using System.Buffers.Binary;
using Microsoft.AspNetCore.Identity;
using OjitexSystem_Backend.Common;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Data.Auth;

namespace OjitexSystem_Backend.Services.Auth;

public sealed class LegacyPasswordMigrationService(
    IUserRepository users,
    IAuthUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    PasswordHasher<IeUser> passwordHasher) : ILegacyPasswordMigrationService
{
    public async Task<int> ResetLegacyPasswordsAsync(CancellationToken cancellationToken = default)
    {
        var activeUsers = await users.GetActiveAsync(cancellationToken);
        var now = timeProvider.GetBusinessNow();
        var updatedCount = 0;

        foreach (var user in activeUsers)
        {
            if (IsIdentityPasswordHash(user.Password))
            {
                continue;
            }

            UserPasswordReset.Apply(user, passwordHasher, now);
            updatedCount++;
        }

        if (updatedCount > 0)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return updatedCount;
    }

    private static bool IsIdentityPasswordHash(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        try
        {
            var bytes = Convert.FromBase64String(password);
            if (bytes.Length < 61 || bytes[0] != 0x01)
            {
                return false;
            }

            var prf = BinaryPrimitives.ReadInt32BigEndian(bytes.AsSpan(1, 4));
            var iterationCount = BinaryPrimitives.ReadInt32BigEndian(bytes.AsSpan(5, 4));
            var saltLength = BinaryPrimitives.ReadInt32BigEndian(bytes.AsSpan(9, 4));
            return prf is >= 0 and <= 2
                && iterationCount > 0
                && saltLength >= 16
                && bytes.Length > 13 + saltLength;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
