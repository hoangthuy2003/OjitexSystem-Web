using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class VtImportConvertingFromPlan
{
    public DateOnly? CrConvertingDate { get; set; }

    public int? CrOrderNo { get; set; }

    public int? CrReceiveNo { get; set; }

    public DateOnly? CrDeliveryDate { get; set; }

    public DateOnly? CrFinishDate { get; set; }

    public decimal? CrCusCd { get; set; }

    public decimal? CrProCd { get; set; }

    public decimal? CrCsQty { get; set; }

    public decimal? CrPlanQty { get; set; }

    public string? CrDept { get; set; }

    public decimal? CrSqm { get; set; }

    public decimal? CrPaperCode { get; set; }

    public string? CrFlute { get; set; }
}
