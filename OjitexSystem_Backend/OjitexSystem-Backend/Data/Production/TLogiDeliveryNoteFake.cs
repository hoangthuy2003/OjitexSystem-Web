using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLogiDeliveryNoteFake
{
    public string DnkDeliverySlip { get; set; } = null!;

    public decimal? DnkCusCd { get; set; }

    public string? DnkCusNa { get; set; }

    public string? DnkTransportCompany { get; set; }

    public string? DnkTruckNumber { get; set; }

    public string? DnkCarType { get; set; }

    public DateOnly? DnkDateDelivery { get; set; }

    public string? DnkStatus { get; set; }

    public string? DnkSupCd { get; set; }

    public string? DnkNote { get; set; }

    public string? DnkDeliveryAddress { get; set; }

    public string? DnkGoodsType { get; set; }

    public DateTime? DnkDateInput { get; set; }

    public DateTime? DnkDateChanged { get; set; }

    public DateTime? DnkDateConfirm { get; set; }

    public string? DnkUserIdInput { get; set; }

    public string? DnkUserIdChanged { get; set; }

    public string? DnkUserIdConfirm { get; set; }
}
