using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO SelectConditions Define
/// </summary>
public partial class IeSelectDefine
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
    /// Display select flag
    /// </summary>
    public decimal SelectFlag { get; set; }

    /// <summary>
    /// Select statement
    /// </summary>
    public string? SelectText { get; set; }

    /// <summary>
    /// Table Alias ID
    /// </summary>
    public string TableAliasId { get; set; } = null!;

    /// <summary>
    /// Column ID
    /// </summary>
    public string ColumnId { get; set; } = null!;

    /// <summary>
    /// Column Alias ID
    /// </summary>
    public string ColumnAliasId { get; set; } = null!;

    /// <summary>
    /// Display name
    /// </summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// Sort type
    /// </summary>
    public string SortType { get; set; } = null!;

    /// <summary>
    /// Sort order
    /// </summary>
    public decimal SortOrder { get; set; }

    /// <summary>
    /// Grouping
    /// </summary>
    public string Grouping { get; set; } = null!;

    /// <summary>
    /// Where statement
    /// </summary>
    public string? QueryWhere { get; set; }

    /// <summary>
    /// Or1 statement
    /// </summary>
    public string? QueryOr1 { get; set; }

    /// <summary>
    /// Or2 statement
    /// </summary>
    public string? QueryOr2 { get; set; }

    /// <summary>
    /// Or3 statement
    /// </summary>
    public string? QueryOr3 { get; set; }

    /// <summary>
    /// Or4 statement
    /// </summary>
    public string? QueryOr4 { get; set; }

    /// <summary>
    /// Insert flag
    /// </summary>
    public decimal InsertFlag { get; set; }

    /// <summary>
    /// Update key flag
    /// </summary>
    public decimal UpdateKeyFlag { get; set; }

    /// <summary>
    /// Update flag
    /// </summary>
    public decimal UpdateFlag { get; set; }

    /// <summary>
    /// Delete key flag
    /// </summary>
    public decimal DeleteKeyFlag { get; set; }

    /// <summary>
    /// Not null flag
    /// </summary>
    public decimal NotnullFlag { get; set; }

    /// <summary>
    /// Control type
    /// </summary>
    public string ControlType { get; set; } = null!;

    /// <summary>
    /// Ctl Value type flag
    /// </summary>
    public string? CtlValueType { get; set; }

    /// <summary>
    /// Ctl list type
    /// </summary>
    public decimal CtlListFlag { get; set; }

    /// <summary>
    /// Ctl invalid permission
    /// </summary>
    public decimal CtlInvalidFlag { get; set; }

    /// <summary>
    /// Ctl SelectQuery ID
    /// </summary>
    public string? CtlSelqryId { get; set; }

    /// <summary>
    /// Ctl DB Value
    /// </summary>
    public string? CtlSelqryDb { get; set; }

    /// <summary>
    /// Ctl Display Value
    /// </summary>
    public string? CtlSelqryDisp { get; set; }

    /// <summary>
    /// Ltrim flag
    /// </summary>
    public decimal LtrimFlag { get; set; }

    /// <summary>
    /// Rtrim flag
    /// </summary>
    public decimal RtrimFlag { get; set; }

    /// <summary>
    /// Cast value
    /// </summary>
    public string? CastValue { get; set; }

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
