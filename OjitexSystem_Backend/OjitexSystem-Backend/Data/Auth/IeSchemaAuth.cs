using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Schema Authority
/// </summary>
public partial class IeSchemaAuth
{
    /// <summary>
    /// Schema ID
    /// </summary>
    public string SchemaId { get; set; } = null!;

    /// <summary>
    /// Specified flag
    /// </summary>
    public decimal AuthType { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Role
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Executable flag
    /// </summary>
    public decimal ExecutableType { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
