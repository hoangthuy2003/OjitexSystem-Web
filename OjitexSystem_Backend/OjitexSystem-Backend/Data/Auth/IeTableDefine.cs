using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO QueryTables Define
/// </summary>
public partial class IeTableDefine
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
    /// Alias ID
    /// </summary>
    public string AliasId { get; set; } = null!;

    /// <summary>
    /// Library ID
    /// </summary>
    public string LibId { get; set; } = null!;

    /// <summary>
    /// Table ID
    /// </summary>
    public string TableId { get; set; } = null!;

    /// <summary>
    /// Library List flag
    /// </summary>
    public decimal LibListFlag { get; set; }

    /// <summary>
    /// Point X
    /// </summary>
    public decimal PointX { get; set; }

    /// <summary>
    /// Point Y
    /// </summary>
    public decimal PointY { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
