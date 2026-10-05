using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Conditions Xml
/// </summary>
public partial class IeCondXml
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
    /// Conditions Xml
    /// </summary>
    public string? CondXml { get; set; }
}
