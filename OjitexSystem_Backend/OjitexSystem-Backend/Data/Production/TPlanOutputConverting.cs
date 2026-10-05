using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TPlanOutputConverting
{
    public long PcvtCd { get; set; }

    public int? PcvtOrderNo { get; set; }

    public int? PcvtReceiveNo { get; set; }

    public decimal? PcvtCusCd { get; set; }

    public string? PcvtCusNa { get; set; }

    public decimal? PcvtProCd { get; set; }

    public string? PcvtProNa { get; set; }

    public decimal? PcvtCaseQty { get; set; }

    public decimal? PcvtSheetWidth { get; set; }

    public decimal? PcvtSheetLen { get; set; }

    public decimal? PcvtQtyPerSheet { get; set; }

    public decimal? PcvtQtyPerCorru { get; set; }

    public string? PcvtFlute { get; set; }

    public DateOnly? PcvtCorruDate { get; set; }

    public string? PcvtProcess1 { get; set; }

    public DateOnly? PcvtProcess1Date { get; set; }

    public string? PcvtProcess2 { get; set; }

    public DateOnly? PcvtProcess2Date { get; set; }

    public string? PcvtProcess3 { get; set; }

    public DateOnly? PcvtProcess3Date { get; set; }

    public string? PcvtProcess4 { get; set; }

    public DateOnly? PcvtProcess4Date { get; set; }

    public decimal? PcvtCsOrder { get; set; }

    public DateOnly? PcvtFinishDate { get; set; }

    public DateOnly? PcvtDeliveryDate { get; set; }

    public decimal? PcvtSheetSqm { get; set; }

    public string? PcvtInk1 { get; set; }

    public string? PcvtInk2 { get; set; }

    public string? PcvtInk3 { get; set; }

    public string? PcvtInk4 { get; set; }

    public string? PcvtInk5 { get; set; }

    public string? PcvtPrintPlateNo { get; set; }

    public string? PcvtDiePlateNo { get; set; }

    public string? PcvtPaperCd { get; set; }

    public string? PcvtProcessName { get; set; }

    public DateOnly? PcvtTargetDate { get; set; }

    public string? PcvtTargetTime { get; set; }

    public decimal? PcvtRunTime { get; set; }

    public int? PcvtSetTime { get; set; }

    public DateOnly? PcvtConvertingDay { get; set; }

    public DateTime? PcvtDateInput { get; set; }

    public string? PcvtUserId { get; set; }
}
