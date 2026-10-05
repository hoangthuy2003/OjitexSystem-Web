using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLogiPaperRollDeliveryNote
{
    public string PdnDeliverySlip { get; set; } = null!;

    public string? PdnDesCd { get; set; }

    public string? PdnDesNa { get; set; }

    public string? PdnTransportCompany { get; set; }

    public string? PdnContainerNo { get; set; }

    public string? PdnCarType { get; set; }

    public DateOnly? PdnDateDelivery { get; set; }

    public string? PdnStatus { get; set; }

    public string? PdnNote { get; set; }

    public string? PdnDeliveryAddress { get; set; }

    public int? PdnChangedCount { get; set; }

    public DateTime? PdnDateInput { get; set; }

    public DateTime? PdnDateChanged { get; set; }

    public DateTime? PdnDateConfirm { get; set; }

    public string? PdnUserIdInput { get; set; }

    public string? PdnUserIdChanged { get; set; }

    public string? PdnUserIdConfirm { get; set; }
}
