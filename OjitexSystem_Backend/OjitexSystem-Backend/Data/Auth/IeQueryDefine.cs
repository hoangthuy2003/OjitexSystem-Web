using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Query Define
/// </summary>
public partial class IeQueryDefine
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
    /// Query name
    /// </summary>
    public string? QueryName { get; set; }

    /// <summary>
    /// Query type
    /// </summary>
    public string QueryType { get; set; } = null!;

    /// <summary>
    /// Execute timing
    /// </summary>
    public string ExecTiming { get; set; } = null!;

    /// <summary>
    /// Library list flag
    /// </summary>
    public decimal LibListFlag { get; set; }

    /// <summary>
    /// Error stop flag
    /// </summary>
    public decimal ErrstopFlag { get; set; }

    /// <summary>
    /// Error message
    /// </summary>
    public string? ErrstopMsg { get; set; }

    /// <summary>
    /// Update type
    /// </summary>
    public string UpdateType { get; set; } = null!;

    /// <summary>
    /// Result occurs flag
    /// </summary>
    public string ResultOccurs { get; set; } = null!;

    /// <summary>
    /// Update target library
    /// </summary>
    public string? TargetLib { get; set; }

    /// <summary>
    /// Update target table
    /// </summary>
    public string? TargetTable { get; set; }

    /// <summary>
    /// Insert flag
    /// </summary>
    public decimal InsertFlag { get; set; }

    /// <summary>
    /// Update flag
    /// </summary>
    public decimal UpdateFlag { get; set; }

    /// <summary>
    /// Delete flag
    /// </summary>
    public decimal DeleteFlag { get; set; }

    /// <summary>
    /// SQL sentence
    /// </summary>
    public string? SqlText { get; set; }

    /// <summary>
    /// Job ID
    /// </summary>
    public string? JobId { get; set; }

    /// <summary>
    /// Reference executable flag
    /// </summary>
    public decimal ExecutableFlag { get; set; }

    /// <summary>
    /// Reserve item 1
    /// </summary>
    public string? DataVarchr1 { get; set; }

    /// <summary>
    /// Reserve item 2
    /// </summary>
    public string? DataVarchr2 { get; set; }

    /// <summary>
    /// Reserve item 3
    /// </summary>
    public string? DataVarchr3 { get; set; }

    /// <summary>
    /// Reserve item 4
    /// </summary>
    public string? DataVarchr4 { get; set; }

    /// <summary>
    /// Reserve item 5
    /// </summary>
    public string? DataVarchr5 { get; set; }

    /// <summary>
    /// Reserve item 6
    /// </summary>
    public string? DataVarchr6 { get; set; }

    /// <summary>
    /// Reserve item 7
    /// </summary>
    public string? DataVarchr7 { get; set; }

    /// <summary>
    /// Reserve item 8
    /// </summary>
    public string? DataVarchr8 { get; set; }

    /// <summary>
    /// Reserve item 9
    /// </summary>
    public string? DataVarchr9 { get; set; }

    /// <summary>
    /// Reserve item 10
    /// </summary>
    public string? DataVarchr10 { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
