using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLogiDeliveryNote
{
    public int DnKey { get; set; }

    public string DnDeliverySlip { get; set; } = null!;

    public decimal? DnCusCd { get; set; }

    public string? DnCusNa { get; set; }

    public string? DnTransportCompany { get; set; }

    public string? DnTruckNumber { get; set; }

    public string? DnCarType { get; set; }

    public DateOnly? DnDateDelivery { get; set; }

    public string? DnState { get; set; }

    public string? DnSupCd { get; set; }

    public string? DnNote { get; set; }

    public string? DnDeliveryAddress { get; set; }

    public string? DnGoodsType { get; set; }

    public string? DnStatus { get; set; }

    public string? DnStatusRemark { get; set; }

    public string? DnUserConfirm { get; set; }

    public DateTime? DnDateConfirm { get; set; }

    public DateTime? DnDateInput { get; set; }

    public string? DnUserId { get; set; }

    public DateTime? DnDateChange { get; set; }
}
