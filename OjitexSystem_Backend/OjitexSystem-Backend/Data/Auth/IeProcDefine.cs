using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Proc Define
/// </summary>
public partial class IeProcDefine
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
    /// Library name
    /// </summary>
    public string? LibName { get; set; }

    /// <summary>
    /// Module name
    /// </summary>
    public string? ModuleName { get; set; }

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
