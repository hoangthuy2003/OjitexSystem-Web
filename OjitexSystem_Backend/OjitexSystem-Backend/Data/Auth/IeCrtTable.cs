using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO CreateTable Define
/// </summary>
public partial class IeCrtTable
{
    /// <summary>
    /// Query ID
    /// </summary>
    public string QueryId { get; set; } = null!;

    /// <summary>
    /// Query name
    /// </summary>
    public string QueryName { get; set; } = null!;

    /// <summary>
    /// Catetory ID
    /// </summary>
    public string CategoryId { get; set; } = null!;

    /// <summary>
    /// Library Name
    /// </summary>
    public string? LibId { get; set; }

    /// <summary>
    /// Table ID
    /// </summary>
    public string? TableId { get; set; }

    /// <summary>
    /// Table name label
    /// </summary>
    public string? TableLabel { get; set; }

    /// <summary>
    /// PrimaryKey CST name
    /// </summary>
    public string? PrimaryKeyId { get; set; }

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

    /// <summary>
    /// Summary
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Remarks
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// Change History
    /// </summary>
    public string? ChangeHist { get; set; }
}
