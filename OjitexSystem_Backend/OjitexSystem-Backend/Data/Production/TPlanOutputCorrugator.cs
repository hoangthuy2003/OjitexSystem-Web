using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TPlanOutputCorrugator
{
    public int PcorCd { get; set; }

    public int? PcorLotNo { get; set; }

    public int? PcorOrderNo { get; set; }

    public int? PcorReceiveNo { get; set; }

    public decimal? PcorCusCd { get; set; }

    public string? PcorCusNa { get; set; }

    public decimal? PcorProCd { get; set; }

    public string? PcorProNa { get; set; }

    public decimal? PcorCaseQty { get; set; }

    public decimal? PcorSheetQty { get; set; }

    public decimal? PcorCaseWidth { get; set; }

    public decimal? PcorCaseLen { get; set; }

    public decimal? PcorSheetWidth { get; set; }

    public decimal? PcorSheetLen { get; set; }

    public decimal? PcorQtyPerSheet { get; set; }

    public decimal? PcorCorruWidth { get; set; }

    public decimal? PcorCorruLength { get; set; }

    public decimal? PcorQtyPerCorru { get; set; }

    public decimal? PcorTotalLen { get; set; }

    public decimal? PcorTrimWidth { get; set; }

    public string? PcorFlute { get; set; }

    public string? PcorPaperCd { get; set; }

    public string? PcorGlNa { get; set; }

    public int? PcorGlWeight { get; set; }

    public string? PcorBmNa { get; set; }

    public int? PcorBmWeight { get; set; }

    public string? PcorBlNa { get; set; }

    public int? PcorBlWeight { get; set; }

    public string? PcorAmNa { get; set; }

    public int? PcorAmWeight { get; set; }

    public string? PcorAlNa { get; set; }

    public int? PcorAlWeight { get; set; }

    public decimal? PcorClassSlitter { get; set; }

    public decimal? PcorScore1 { get; set; }

    public decimal? PcorScore2 { get; set; }

    public decimal? PcorScore3 { get; set; }

    public decimal? PcorScore4 { get; set; }

    public decimal? PcorScore5 { get; set; }

    public DateTime? PcorOrderDate { get; set; }

    public DateOnly? PcorCorruDate { get; set; }

    public string? PcorProcess1 { get; set; }

    public DateOnly? PcorProcess1Date { get; set; }

    public string? PcorProcess2 { get; set; }

    public DateOnly? PcorProcess2Date { get; set; }

    public string? PcorProcess3 { get; set; }

    public DateOnly? PcorProcess3Date { get; set; }

    public string? PcorProcess4 { get; set; }

    public DateOnly? PcorProcess4Date { get; set; }

    public string? PcorProcess5 { get; set; }

    public DateOnly? PcorProcess5Date { get; set; }

    public DateOnly? PcorDeliveryDate { get; set; }

    public decimal? PcorRegularSpeed { get; set; }

    public string? PcorSpecialCd { get; set; }

    public string? PcorTecmoBar { get; set; }

    public decimal? PcorSheetSqm { get; set; }

    public string? PcorInk1 { get; set; }

    public string? PcorInk2 { get; set; }

    public string? PcorInk3 { get; set; }

    public string? PcorInk4 { get; set; }

    public string? PcorInk5 { get; set; }

    public string? PcorPrintPlateNo { get; set; }

    public string? PcorDiePlateNo { get; set; }

    public string? PcorRemark { get; set; }

    public string? PcorRemark2 { get; set; }

    public DateOnly? PcorTargetDate { get; set; }

    public string? PcorTargetTime { get; set; }

    public DateOnly? PcorCorruDay { get; set; }

    public DateTime? PcorDateInput { get; set; }

    public string? PcorUserId { get; set; }
}
