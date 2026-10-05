using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO User
/// </summary>
public partial class IeUser
{
    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Last name
    /// </summary>
    public string? UserFamilyName { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public string? UserFirstName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Last time password
    /// </summary>
    public string? PastPassword { get; set; }

    /// <summary>
    /// Last time password 1
    /// </summary>
    public string? PastPassword1 { get; set; }

    /// <summary>
    /// Last time password 2
    /// </summary>
    public string? PastPassword2 { get; set; }

    /// <summary>
    /// Password input mistake date
    /// </summary>
    public DateTime? PasswordMissDate { get; set; }

    /// <summary>
    /// Password input mistake count
    /// </summary>
    public decimal PasswordMissCount { get; set; }

    /// <summary>
    /// Password update date
    /// </summary>
    public DateTime? PasswordUpdateDate { get; set; }

    /// <summary>
    /// User lock flag
    /// </summary>
    public decimal UserLockFlag { get; set; }

    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// Default category
    /// </summary>
    public string? DefaultCategory { get; set; }

    /// <summary>
    /// Logical deleted flag
    /// </summary>
    public decimal LogicalDelFlag { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }

    /// <summary>
    /// Hide command flag
    /// </summary>
    public decimal CommandHideFlag { get; set; }
}
