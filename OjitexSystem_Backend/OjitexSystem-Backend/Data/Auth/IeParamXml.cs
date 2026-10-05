using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Parameter Xml
/// </summary>
public partial class IeParamXml
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
    /// Parameter Xml
    /// </summary>
    public string? ParamXml { get; set; }
}
