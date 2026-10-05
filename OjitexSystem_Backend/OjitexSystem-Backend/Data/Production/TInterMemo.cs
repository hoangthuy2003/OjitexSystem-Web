using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TInterMemo
{
    public int ImId { get; set; }

    public string? ImActNo { get; set; }

    public decimal? ImCusCd { get; set; }

    public DateTime? ImOrderDate { get; set; }

    public int? ImReceiveNo { get; set; }

    public decimal? ImProCd { get; set; }

    public decimal? ImProductQty { get; set; }

    public DateOnly? ImDelivery { get; set; }

    public string? ImOld { get; set; }

    public string? ImNew { get; set; }

    public string? ImDetail { get; set; }

    public string? ImSection { get; set; }

    public string? ImPrintState { get; set; }

    public int? ImEditNo { get; set; }

    public DateTime? ImDatePrinted { get; set; }

    public DateTime? ImDateInput { get; set; }

    public string? ImUserId { get; set; }
}
