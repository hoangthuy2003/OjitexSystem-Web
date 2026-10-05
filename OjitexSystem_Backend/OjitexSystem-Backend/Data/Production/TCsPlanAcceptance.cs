using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

/// <summary>
/// T_CS_PLAN_Acceptance
/// </summary>
public partial class TCsPlanAcceptance
{
    /// <summary>
    /// acc_id
    /// </summary>
    public int AccId { get; set; }

    /// <summary>
    /// acc_receive_no
    /// </summary>
    public int? AccReceiveNo { get; set; }

    /// <summary>
    /// acc_class
    /// </summary>
    public int? AccClass { get; set; }

    /// <summary>
    /// acc_cus_cd
    /// </summary>
    public decimal? AccCusCd { get; set; }

    /// <summary>
    /// acc_pro_cd
    /// </summary>
    public decimal? AccProCd { get; set; }

    /// <summary>
    /// acc_order_qty
    /// </summary>
    public decimal? AccOrderQty { get; set; }

    /// <summary>
    /// acc_stock
    /// </summary>
    public decimal? AccStock { get; set; }

    /// <summary>
    /// acc_product
    /// </summary>
    public decimal? AccProduct { get; set; }

    /// <summary>
    /// acc_delivery
    /// </summary>
    public DateTime? AccDelivery { get; set; }

    /// <summary>
    /// acc_po
    /// </summary>
    public string? AccPo { get; set; }

    /// <summary>
    /// acc_remark
    /// </summary>
    public string? AccRemark { get; set; }

    /// <summary>
    /// acc_type
    /// </summary>
    public string? AccType { get; set; }

    /// <summary>
    /// acc_date_input
    /// </summary>
    public DateTime? AccDateInput { get; set; }

    /// <summary>
    /// acc_user_name
    /// </summary>
    public string? AccUserName { get; set; }
}
