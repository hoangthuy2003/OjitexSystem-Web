using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Value Define
/// </summary>
public partial class IeValueDefine
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
    /// Value SEQ
    /// </summary>
    public decimal ValueSeq { get; set; }

    /// <summary>
    /// DB value
    /// </summary>
    public string? DbValue { get; set; }

    /// <summary>
    /// Display value
    /// </summary>
    public string? DispValue { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
