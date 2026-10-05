using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Login History
/// </summary>
public partial class IeLoginHist
{
    /// <summary>
    /// Login date time
    /// </summary>
    public decimal Dttm { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Program Type
    /// </summary>
    public string PgType { get; set; } = null!;

    /// <summary>
    /// Login type
    /// </summary>
    public string LoginType { get; set; } = null!;

    /// <summary>
    /// Remarks
    /// </summary>
    public string? Biko { get; set; }

    /// <summary>
    /// Internal ID
    /// </summary>
    public string LoginId { get; set; } = null!;

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
