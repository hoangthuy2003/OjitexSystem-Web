using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO JoinColumn Define
/// </summary>
public partial class IeJoinColDefine
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
    /// Column SEQ
    /// </summary>
    public decimal ColumnSeq { get; set; }

    /// <summary>
    /// Left column
    /// </summary>
    public string LeftColumn { get; set; } = null!;

    /// <summary>
    /// Right column
    /// </summary>
    public string RightColumn { get; set; } = null!;

    /// <summary>
    /// Join type
    /// </summary>
    public string JoinType { get; set; } = null!;

    /// <summary>
    /// Start X
    /// </summary>
    public decimal StartX { get; set; }

    /// <summary>
    /// Start Y
    /// </summary>
    public decimal StartY { get; set; }

    /// <summary>
    /// End X
    /// </summary>
    public decimal EndX { get; set; }

    /// <summary>
    /// End Y
    /// </summary>
    public decimal EndY { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime? EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
