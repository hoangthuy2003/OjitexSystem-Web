using System.Buffers.Binary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Auth;

namespace OjitexSystem_Backend.Services;

public sealed class LegacyPasswordMigrationService(
    AuthContext context,
    PasswordHasher<IeUser> passwordHasher)
{
    public async Task<int> ResetLegacyPasswordsAsync()
    {
        var users = await context.IeUsers
            .Where(user => user.LogicalDelFlag == 0)
            .ToListAsync();
        var now = DateTime.Now;
        var updatedCount = 0;

        foreach (var user in users)
        {
            if (IsIdentityPasswordHash(user.Password))
            {
                continue;
            }

            user.Password = passwordHasher.HashPassword(user, "123456");
            user.PastPassword = null;
            user.PastPassword1 = null;
            user.PastPassword2 = null;
            user.PasswordUpdateDate = now;
            user.PasswordMissDate = null;
            user.PasswordMissCount = 0;
            user.UserLockFlag = 0;
            user.ChgDate = now;
            updatedCount++;
        }

        if (updatedCount > 0)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
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
