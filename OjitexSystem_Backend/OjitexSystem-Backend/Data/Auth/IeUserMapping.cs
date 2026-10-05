using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO User Mapping
/// </summary>
public partial class IeUserMapping
{
    /// <summary>
    /// User ID
    /// </summary>
    public decimal? UserId { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public decimal? UserFamilyName { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public decimal? UserFirstName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public decimal? Password { get; set; }

    /// <summary>
    /// Last time password
    /// </summary>
    public decimal? PastPassword { get; set; }

    /// <summary>
    /// Last time password 1
    /// </summary>
    public decimal? PastPassword1 { get; set; }

    /// <summary>
    /// Last time password 2
    /// </summary>
    public decimal? PastPassword2 { get; set; }

    /// <summary>
    /// Default category
    /// </summary>
    public decimal? DefaultCategory { get; set; }
}
