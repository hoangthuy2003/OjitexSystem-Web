using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Role
/// </summary>
public partial class IeRole
{
    /// <summary>
    /// Role
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Role description 1
    /// </summary>
    public string? RoleDesc1 { get; set; }

    /// <summary>
    /// Role description 2
    /// </summary>
    public string? RoleDesc2 { get; set; }

    /// <summary>
    /// Role description 3
    /// </summary>
    public string? RoleDesc3 { get; set; }

    /// <summary>
    /// Delete-disable flag
    /// </summary>
    public decimal DelDisableFlag { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
