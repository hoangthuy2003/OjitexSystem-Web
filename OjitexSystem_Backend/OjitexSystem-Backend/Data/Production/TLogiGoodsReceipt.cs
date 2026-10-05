using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLogiGoodsReceipt
{
    public int GrKey { get; set; }

    public DateOnly? GrDateReceipt { get; set; }

    public decimal? GrReceiveNo { get; set; }

    public decimal? GrCusCd { get; set; }

    public decimal? GrProCd { get; set; }

    public string? GrProName { get; set; }

    public decimal? GrQty { get; set; }

    public string? GrProductionSection { get; set; }

    public string? GrNote { get; set; }

    public DateTime? GrDateInput { get; set; }

    public DateTime? GrDateChanged { get; set; }

    public string? GrUserId { get; set; }

    public string? GrUserChanged { get; set; }
}
