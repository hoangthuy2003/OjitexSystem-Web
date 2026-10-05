using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO GroupQuery Define
/// </summary>
public partial class IeGroupQuery
{
    /// <summary>
    /// Query ID
    /// </summary>
    public string QueryId { get; set; } = null!;

    /// <summary>
    /// GroupQuery name
    /// </summary>
    public string QueryName { get; set; } = null!;

    /// <summary>
    /// Catetory ID
    /// </summary>
    public string CategoryId { get; set; } = null!;

    /// <summary>
    /// XSD define
    /// </summary>
    public string? XsdDefine { get; set; }

    /// <summary>
    /// Major version
    /// </summary>
    public decimal VerMajor { get; set; }

    /// <summary>
    /// Minor version
    /// </summary>
    public decimal VerMinor { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }

    /// <summary>
    /// Summary
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Remarks
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// Change History
    /// </summary>
    public string? ChangeHist { get; set; }
}
