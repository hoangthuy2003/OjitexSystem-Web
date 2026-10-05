using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TConvertingResult
{
    public int CrCd { get; set; }

    public DateOnly? CrConvertingDate { get; set; }

    public int? CrOrderNo { get; set; }

    public int? CrReceiveNo { get; set; }

    public DateOnly? CrDeliveryDate { get; set; }

    public DateOnly? CrFinishDate { get; set; }

    public decimal? CrCusCd { get; set; }

    public decimal? CrProCd { get; set; }

    public decimal? CrCsQty { get; set; }

    public decimal? CrPlanQty { get; set; }

    public decimal? CrFgInput { get; set; }

    public decimal? CrFgOutput { get; set; }

    public string? CrStartTime { get; set; }

    public string? CrFinishTime { get; set; }

    public decimal? CrDrivingTime { get; set; }

    public decimal? CrSetTime { get; set; }

    public decimal? CrStopTime { get; set; }

    public decimal? CrRestTime { get; set; }

    public decimal? CrBreakTime { get; set; }

    public string? CrDept { get; set; }

    public int? CrShift { get; set; }

    public string? CrFinishGood { get; set; }

    public decimal? CrSqm { get; set; }

    public decimal? CrPaperCode { get; set; }

    public string? CrFlute { get; set; }

    public string? CrRemark { get; set; }

    public DateTime? CrDateInput { get; set; }

    public string? CrUserId { get; set; }
}
