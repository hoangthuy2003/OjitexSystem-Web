using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMatExportPaperRoll
{
    public int EprId { get; set; }

    public string? EprName { get; set; }

    public decimal? EprGramage { get; set; }

    public decimal? EprQty { get; set; }

    public decimal? EprUsdPrice { get; set; }

    public decimal? EprUsdAmount { get; set; }

    public decimal? EprVndPrice { get; set; }

    public decimal? EprVndAmount { get; set; }

    public decimal? EprUsdTransportFee { get; set; }

    public decimal? EprVndTransportFee { get; set; }

    public decimal? EprUsdOthersFee { get; set; }

    public decimal? EprVndOthersFee { get; set; }

    public string? EprInvoiceNo { get; set; }

    public DateOnly? EprInvoiceDate { get; set; }

    public string? EprType { get; set; }

    public string? EprSupplierName { get; set; }

    public string? EprContainerNo { get; set; }

    public string? EprLogisticsRemark { get; set; }

    public string? EprMaterialRemark { get; set; }

    public DateTime? EprDateInput { get; set; }

    public DateTime? EprDateChanged { get; set; }

    public DateTime? EprDateDelivered { get; set; }

    public string? EprUserId { get; set; }
}
