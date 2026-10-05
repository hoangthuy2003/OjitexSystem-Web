using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Query Library Define
/// </summary>
public partial class IeQueryLibrary
{
    /// <summary>
    /// Query ID
    /// </summary>
    public string QueryId { get; set; } = null!;

    /// <summary>
    /// Library name
    /// </summary>
    public string Library { get; set; } = null!;

    /// <summary>
    /// Role
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Execute order
    /// </summary>
    public decimal ExecOrder { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
