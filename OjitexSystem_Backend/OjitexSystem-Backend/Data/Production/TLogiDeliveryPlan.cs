using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLogiDeliveryPlan
{
    public int DpId { get; set; }

    public DateOnly? DpDateDelivery { get; set; }

    public string? DpCusNa { get; set; }

    public decimal? DpReceiveNo { get; set; }

    public decimal? DpProCd { get; set; }

    public string? DpProName { get; set; }

    public decimal? DpQty { get; set; }

    public decimal? DpStock { get; set; }

    public decimal? DpAutualQty { get; set; }

    public string? DpRemark { get; set; }

    public decimal? DpSqm { get; set; }

    public decimal? DpTotalSqm { get; set; }

    public string? DpFlute { get; set; }

    public string? DpPoNo { get; set; }

    public string? DpLastProcess { get; set; }

    public string? DpAcceptanceNote { get; set; }

    public string? DpActNo { get; set; }

    public DateTime? DpDateInput { get; set; }

    public DateTime? DpDateChanged { get; set; }

    public string? DpUserId { get; set; }

    public string? DpUserIdChanged { get; set; }
}
