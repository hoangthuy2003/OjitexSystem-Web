using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMatPaperRollComming
{
    public int PrcSerialNo { get; set; }

    public DateOnly? PrcDateComming { get; set; }

    public string? PrcMakerCd { get; set; }

    public string? PrcMakerNa { get; set; }

    public string? PrcSupCd { get; set; }

    public string? PrcSupNa { get; set; }

    public int? PrcGraceCd { get; set; }

    public string? PrcGraceNa { get; set; }

    public decimal? PrcGramage { get; set; }

    public decimal? PrcWidth { get; set; }

    public decimal? PrcLength { get; set; }

    public decimal? PrcUnitPrice { get; set; }

    public decimal? PrcWeight { get; set; }

    public decimal? PrcTotalAmount { get; set; }

    public string? PrcInvoiceNo { get; set; }

    public string? PrcLogisticsCode { get; set; }

    public string? PrcPaperRollNo { get; set; }

    public string? PrcContainerNo { get; set; }

    public string? PrcLogisticsRemark { get; set; }

    public string? PrcMaterialRemark { get; set; }

    public DateTime? PrcDateInput { get; set; }

    public DateTime? PrcDateChanged { get; set; }

    public string? PrcUserId { get; set; }
}
