using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Value Xml
/// </summary>
public partial class IeValueXml
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
    /// Value Xml
    /// </summary>
    public string? ValueXml { get; set; }
}
