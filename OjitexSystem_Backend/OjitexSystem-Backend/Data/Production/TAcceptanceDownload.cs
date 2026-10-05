using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TAcceptanceDownload
{
    public int AdId { get; set; }

    public string? AdActNo { get; set; }

    public int? AdReceiveNo { get; set; }

    public int? AdClass { get; set; }

    public decimal? AdCusCd { get; set; }

    public decimal? AdProCd { get; set; }

    public decimal? AdOrderQty { get; set; }

    public decimal? AdStock { get; set; }

    public decimal? AdProduct { get; set; }

    public DateOnly? AdDelivery { get; set; }

    public string? AdPo { get; set; }

    public string? AdRemark { get; set; }

    public string? AdNoteDetail { get; set; }

    public string? AdType { get; set; }

    public DateTime? AdDateInput { get; set; }

    public string? AdCsUserId { get; set; }

    public string? AdUserId { get; set; }
}
