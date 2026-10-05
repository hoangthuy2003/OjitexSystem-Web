using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO ValueQuery Define
/// </summary>
public partial class IeValueQryDefine
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
    public decimal SeqNo { get; set; }

    /// <summary>
    /// Param SEQ
    /// </summary>
    public decimal ParamSeq { get; set; }

    /// <summary>
    /// Param name
    /// </summary>
    public string? ParamName { get; set; }

    /// <summary>
    /// Param value
    /// </summary>
    public string? ParamValue { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
