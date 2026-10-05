using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TProcorrugatorInput
{
    public int CorrCd { get; set; }

    public int? CorrOrderNo { get; set; }

    public int? CorrLotNo { get; set; }

    public int? CorrReceiveNo { get; set; }

    public DateOnly? CorrDay { get; set; }

    public string? CorruKind { get; set; }

    public decimal? CorrCusCd { get; set; }

    public decimal? CorrProCd { get; set; }

    public decimal? CorruRealSheetWidth { get; set; }

    public decimal? CorruRealSheetLen { get; set; }

    public decimal? CorrRealCorruWid { get; set; }

    public decimal? CorrRealQtyPerSheet { get; set; }

    public decimal? CorrOutputFgCase { get; set; }

    public decimal? CorrOutputFgSheet { get; set; }

    public decimal? CorrOutputFgScon { get; set; }

    public string? CorrHscLoss { get; set; }

    public DateTime? CorrDateInput { get; set; }

    public string? CorrUserId { get; set; }

    public decimal? CorrRealQty { get; set; }

    public decimal? CorrWidthLoss { get; set; }

    public decimal? CorrUnitAbility { get; set; }

    public decimal? CorrTotalSqm { get; set; }

    public decimal? CorrTotalM { get; set; }

    public decimal? CorrTrimWeight { get; set; }

    public decimal? CorrCorWeight { get; set; }

    public decimal? CorrHscWeight { get; set; }

    public DateOnly? CorrCorDate { get; set; }

    public string? CorrRemark { get; set; }

    public string? CorrFlute { get; set; }

    public string? CorrPaperCode { get; set; }

    public decimal? CorrUnitWeight { get; set; }

    public string? CorrSheetBoard { get; set; }
}
