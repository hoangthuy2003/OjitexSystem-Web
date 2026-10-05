using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO JOB Define
/// </summary>
public partial class IeJobDefine
{
    /// <summary>
    /// JOB ID
    /// </summary>
    public string JobId { get; set; } = null!;

    /// <summary>
    /// Procedure Name
    /// </summary>
    public string? ProcedureName { get; set; }

    /// <summary>
    /// External lang
    /// </summary>
    public string ExternalLang { get; set; } = null!;

    /// <summary>
    /// External library
    /// </summary>
    public string? ExternalLib { get; set; }

    /// <summary>
    /// External module name
    /// </summary>
    public string? ExternalModule { get; set; }

    /// <summary>
    /// Result sets count
    /// </summary>
    public int ResultSets { get; set; }

    /// <summary>
    /// Procedure label
    /// </summary>
    public string ProcedureLabel { get; set; } = null!;

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
