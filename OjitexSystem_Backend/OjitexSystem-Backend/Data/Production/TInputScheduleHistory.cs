using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TInputScheduleHistory
{
    public int InCd { get; set; }

    public decimal? InCusCd { get; set; }

    public decimal? InProCd { get; set; }

    public decimal? InOrderQty { get; set; }

    public decimal? InLotQty { get; set; }

    public decimal? InQuantity { get; set; }

    public DateOnly? InFinishDay { get; set; }

    public DateOnly? InCorruDay { get; set; }

    public DateOnly? InProcess1Date { get; set; }

    public DateOnly? InProcess2Date { get; set; }

    public DateOnly? InProcess3Date { get; set; }

    public DateOnly? InProcess4Date { get; set; }

    public DateOnly? InProcess5Date { get; set; }

    public string? InRemark { get; set; }

    public int? InReceiveNo { get; set; }

    public DateTime? InCsOrderDate { get; set; }

    public DateOnly? InCsDelivery { get; set; }

    public int? InCsClass { get; set; }

    public DateTime? InTimeDelete { get; set; }

    public string? InUserId { get; set; }
}
