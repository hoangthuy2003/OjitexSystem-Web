using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TProconvertingInput
{
    public long ConvCd { get; set; }

    public DateOnly? ConvConvertingDay { get; set; }

    public int? ConvOrderNo { get; set; }

    public int? ConvReceiveNo { get; set; }

    public DateOnly? ConvDeliveryDay { get; set; }

    public DateOnly? ConvFinishDay { get; set; }

    public decimal? ConvCusCd { get; set; }

    public decimal? ConvProCd { get; set; }

    public decimal? ConvCsQty { get; set; }

    public decimal? ConvPlanQty { get; set; }

    public decimal? ConvFgInput { get; set; }

    public decimal? ConvFgOutput { get; set; }

    public string? ConvDept { get; set; }

    public string? ConvFinishGood { get; set; }

    public decimal? ConvSqm { get; set; }

    public string? ConvPaperCode { get; set; }

    public string? ConvFlute { get; set; }

    public string? ConvRemark { get; set; }

    public string? ConvInk1 { get; set; }

    public string? ConvInk2 { get; set; }

    public string? ConvInk3 { get; set; }

    public string? ConvInk4 { get; set; }

    public string? ConvInk5 { get; set; }

    public string? ConvPrintPlateNo { get; set; }

    public string? ConvDiePlateNo { get; set; }

    public int? ConvSetTime { get; set; }

    public int? ConvWorkingTime { get; set; }

    public int? ConvNotWorkingTime { get; set; }

    public int? ConvRepairTime { get; set; }

    public int? ConvSampleTime { get; set; }

    public int? ConvOthersTime { get; set; }

    public DateTime? ConvDateInput { get; set; }

    public string? ConvUserId { get; set; }

    public int? ConvShift { get; set; }
}
