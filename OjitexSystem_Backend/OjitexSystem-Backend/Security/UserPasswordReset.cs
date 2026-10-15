using Microsoft.AspNetCore.Identity;
using OjitexSystem_Backend.Data.Auth;

namespace OjitexSystem_Backend.Security;

public static class UserPasswordReset
{
    public const string DefaultPassword = "123456";

    public static void Apply(IeUser user, PasswordHasher<IeUser> passwordHasher, DateTime now)
    {
        user.Password = passwordHasher.HashPassword(user, DefaultPassword);
        user.PastPassword = null;
        user.PastPassword1 = null;
        user.PastPassword2 = null;
        user.PasswordUpdateDate = now;
        user.PasswordMissDate = null;
        user.PasswordMissCount = 0;
        user.UserLockFlag = 0;
        user.ChgDate = now;
    }
}
