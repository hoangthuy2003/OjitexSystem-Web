using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO SelectQuery Define
/// </summary>
public partial class IeSelectQuery
{
    /// <summary>
    /// Query ID
    /// </summary>
    public string QueryId { get; set; } = null!;

    /// <summary>
    /// SelectQuery name
    /// </summary>
    public string QueryName { get; set; } = null!;

    /// <summary>
    /// Catetory ID
    /// </summary>
    public string CategoryId { get; set; } = null!;

    /// <summary>
    /// Select Xml
    /// </summary>
    public string? SelectXml { get; set; }

    /// <summary>
    /// Alias Xml
    /// </summary>
    public string? AliasXml { get; set; }

    /// <summary>
    /// Select statement
    /// </summary>
    public string? SelectText { get; set; }

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
