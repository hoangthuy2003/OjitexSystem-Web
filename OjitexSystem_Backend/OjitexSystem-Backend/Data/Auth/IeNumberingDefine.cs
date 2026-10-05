using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Numbering
/// </summary>
public partial class IeNumberingDefine
{
    /// <summary>
    /// Sequential number code
    /// </summary>
    public string CtrCd { get; set; } = null!;

    /// <summary>
    /// Sequential number sub code
    /// </summary>
    public string CtrSubCd { get; set; } = null!;

    /// <summary>
    /// Sequential number
    /// </summary>
    public decimal LastCtr { get; set; }

    /// <summary>
    /// Minimum
    /// </summary>
    public decimal BegCtr { get; set; }

    /// <summary>
    /// Maximum
    /// </summary>
    public decimal EndCtr { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
