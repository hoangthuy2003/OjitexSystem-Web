using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TAcceptanceDownloadLogi
{
    public int AdlId { get; set; }

    public string? AdlActNo { get; set; }

    public int? AdlReceiveNo { get; set; }

    public int? AdlClass { get; set; }

    public decimal? AdlCusCd { get; set; }

    public decimal? AdlProCd { get; set; }

    public decimal? AdlOrderQty { get; set; }

    public decimal? AdlStock { get; set; }

    public decimal? AdlProduct { get; set; }

    public DateOnly? AdlDelivery { get; set; }

    public string? AdlPo { get; set; }

    public string? AdlRemark { get; set; }

    public string? AdlNoteDetail { get; set; }

    public string? AdlNoteAll { get; set; }

    public string? AdlType { get; set; }

    public DateTime? AdlDateInput { get; set; }

    public string? AdlCsUserId { get; set; }

    public string? AdlUserId { get; set; }
}
