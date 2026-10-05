using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Alias Xml
/// </summary>
public partial class IeAliasXml
{
    /// <summary>
    /// Query ID
    /// </summary>
    public string QueryId { get; set; } = null!;

    /// <summary>
    /// Execute SEQ
    /// </summary>
    public decimal ExecSeq { get; set; }

    /// <summary>
    /// Line number
    /// </summary>
    public decimal RowNo { get; set; }

    /// <summary>
    /// Alias Xml
    /// </summary>
    public string? AliasXml { get; set; }
}
