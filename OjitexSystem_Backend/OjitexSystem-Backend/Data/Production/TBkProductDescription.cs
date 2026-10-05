using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

/// <summary>
/// T_BK_ProductDescription
/// </summary>
public partial class TBkProductDescription
{
    /// <summary>
    /// pd_pro_cd
    /// </summary>
    public decimal PdProCd { get; set; }

    /// <summary>
    /// pd_pro_name
    /// </summary>
    public string? PdProName { get; set; }

    /// <summary>
    /// pd_pro_description
    /// </summary>
    public string? PdProDescription { get; set; }

    /// <summary>
    /// pd_pro_note
    /// </summary>
    public string? PdProNote { get; set; }

    /// <summary>
    /// pd_date_input
    /// </summary>
    public DateTime? PdDateInput { get; set; }

    /// <summary>
    /// pd_user_id
    /// </summary>
    public string? PdUserId { get; set; }
}
