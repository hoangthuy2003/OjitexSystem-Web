using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TAcceptanceDownloadHistory
{
    public int AdhId { get; set; }

    public string? AdhActNo { get; set; }

    public int? AdhReceiveNo { get; set; }

    public int? AdhClass { get; set; }

    public decimal? AdhCusCd { get; set; }

    public decimal? AdhProCd { get; set; }

    public decimal? AdhOrderQty { get; set; }

    public decimal? AdhStock { get; set; }

    public decimal? AdhProduct { get; set; }

    public DateOnly? AdhDelivery { get; set; }

    public string? AdhPo { get; set; }

    public string? AdhRemark { get; set; }

    public string? AdhNoteDetail { get; set; }

    public string? AdhType { get; set; }

    public DateTime? AdhDateInput { get; set; }

    public string? AdhCsUserId { get; set; }

    public DateTime? AdhDlTime { get; set; }

    public string? AdhDlUser { get; set; }
}
