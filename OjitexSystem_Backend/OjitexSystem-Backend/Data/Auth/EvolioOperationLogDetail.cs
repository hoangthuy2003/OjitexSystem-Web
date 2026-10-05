using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Operation Log Detail
/// </summary>
public partial class EvolioOperationLogDetail
{
    /// <summary>
    /// Record No
    /// </summary>
    public long RecNo { get; set; }

    /// <summary>
    /// Parent record No
    /// </summary>
    public long ParentRecNo { get; set; }

    /// <summary>
    /// Sql statement
    /// </summary>
    public string? SqlText { get; set; }

    /// <summary>
    /// Parameters
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// Entry date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Reserve item 1
    /// </summary>
    public string? DataVarchar1 { get; set; }

    /// <summary>
    /// Reserve item 2
    /// </summary>
    public string? DataVarchar2 { get; set; }

    /// <summary>
    /// Reserve item 3
    /// </summary>
    public string? DataVarchar3 { get; set; }

    /// <summary>
    /// Reserve item 4
    /// </summary>
    public string? DataVarchar4 { get; set; }

    /// <summary>
    /// Reserve item 5
    /// </summary>
    public string? DataVarchar5 { get; set; }

    /// <summary>
    /// Reserve item 6
    /// </summary>
    public string? DataVarchar6 { get; set; }

    /// <summary>
    /// Reserve item 7
    /// </summary>
    public string? DataVarchar7 { get; set; }

    /// <summary>
    /// Reserve item 8
    /// </summary>
    public string? DataVarchar8 { get; set; }

    /// <summary>
    /// Reserve item 9
    /// </summary>
    public string? DataVarchar9 { get; set; }

    /// <summary>
    /// Reserve item 10
    /// </summary>
    public string? DataVarchar10 { get; set; }
}
