using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

/// <summary>
/// T_CSDisposal_List
/// </summary>
public partial class TCsdisposalList
{
    /// <summary>
    /// dis_pro_cd
    /// </summary>
    public decimal DisProCd { get; set; }

    /// <summary>
    /// dis_date_input
    /// </summary>
    public DateTime? DisDateInput { get; set; }

    /// <summary>
    /// dis_part_na
    /// </summary>
    public string? DisPartNa { get; set; }

    /// <summary>
    /// dis_unit_price
    /// </summary>
    public string? DisUnitPrice { get; set; }

    /// <summary>
    /// dis_rate
    /// </summary>
    public string? DisRate { get; set; }

    /// <summary>
    /// dis_net_weight
    /// </summary>
    public string? DisNetWeight { get; set; }

    /// <summary>
    /// dis_decla_cd
    /// </summary>
    public string? DisDeclaCd { get; set; }

    /// <summary>
    /// dis_qty_pcs
    /// </summary>
    public int? DisQtyPcs { get; set; }

    /// <summary>
    /// dis_packing
    /// </summary>
    public string? DisPacking { get; set; }

    /// <summary>
    /// dis_pro_vnname
    /// </summary>
    public string? DisProVnname { get; set; }

    /// <summary>
    /// dis_ratio
    /// </summary>
    public string? DisRatio { get; set; }

    /// <summary>
    /// dis_remark
    /// </summary>
    public string? DisRemark { get; set; }

    /// <summary>
    /// dis_class
    /// </summary>
    public int? DisClass { get; set; }

    /// <summary>
    /// dis_user_id
    /// </summary>
    public string? DisUserId { get; set; }
}
