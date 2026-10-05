using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Join Define
/// </summary>
public partial class IeJoinDefine
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
    /// Join SEQ
    /// </summary>
    public decimal JoinSeq { get; set; }

    /// <summary>
    /// Left alias id
    /// </summary>
    public string LeftAliasId { get; set; } = null!;

    /// <summary>
    /// Left all select
    /// </summary>
    public decimal LeftAll { get; set; }

    /// <summary>
    /// Right alias id
    /// </summary>
    public string RightAliasId { get; set; } = null!;

    /// <summary>
    /// Right all select
    /// </summary>
    public decimal RightAll { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
