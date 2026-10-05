using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// ｉＥ端末管理
/// </summary>
public partial class IeTerminal
{
    /// <summary>
    /// 端末ＩＤ
    /// </summary>
    public string TerminalId { get; set; } = null!;

    /// <summary>
    /// 端末名
    /// </summary>
    public string? TerminalName { get; set; }

    /// <summary>
    /// 備考
    /// </summary>
    public string? Biko { get; set; }

    /// <summary>
    /// 登録日時
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTime ChgDate { get; set; }
}
