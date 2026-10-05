using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TOpeningStock
{
    public int OsId { get; set; }

    public int? OsReceiveNo { get; set; }

    public string? OsLabel { get; set; }

    public decimal? OsCusCd { get; set; }

    public decimal? OsProCd { get; set; }

    public decimal? OsRealStockQty { get; set; }

    public string? OsArea { get; set; }

    public string? OsPic { get; set; }

    public string? OsRemark { get; set; }

    public DateOnly? OsDateReport { get; set; }

    public DateTime? OsDateInput { get; set; }

    public DateTime? OsDateChange { get; set; }

    public string? OsUserId { get; set; }
}
