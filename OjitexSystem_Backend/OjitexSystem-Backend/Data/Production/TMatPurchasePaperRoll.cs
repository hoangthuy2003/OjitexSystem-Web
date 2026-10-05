using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMatPurchasePaperRoll
{
    public int PprId { get; set; }

    public string? PprName { get; set; }

    public decimal? PprGramage { get; set; }

    public decimal? PprQty { get; set; }

    public decimal? PprUsdPrice { get; set; }

    public decimal? PprUsdAmount { get; set; }

    public decimal? PprVndPrice { get; set; }

    public decimal? PprVndAmount { get; set; }

    public decimal? PprUsdTransportFee { get; set; }

    public decimal? PprVndTransportFee { get; set; }

    public decimal? PprUsdOthersFee { get; set; }

    public decimal? PprVndOthersFee { get; set; }

    public string? PprInvoiceNo { get; set; }

    public DateOnly? PprInvoiceDate { get; set; }

    public string? PprType { get; set; }

    public string? PprSupplierName { get; set; }

    public string? PprContainerNo { get; set; }

    public string? PprLogisticsRemark { get; set; }

    public string? PprMaterialRemark { get; set; }

    public DateTime? PprDateInput { get; set; }

    public DateTime? PprDateChanged { get; set; }

    public DateTime? PprDateReceived { get; set; }

    public string? PprUserId { get; set; }
}
