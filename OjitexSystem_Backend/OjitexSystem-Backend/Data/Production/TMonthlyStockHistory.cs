using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMonthlyStockHistory
{
    public int MsthId { get; set; }

    public int? MsthReceiveNo { get; set; }

    public decimal? MsthProCd { get; set; }

    public string? MsthSection { get; set; }

    public decimal? MsthInputStock { get; set; }

    public decimal? MsthOutputStock { get; set; }

    public decimal? MsthCurrentStock { get; set; }

    public string? MsthRemark { get; set; }

    public DateOnly? MsthWorkDate { get; set; }

    public DateTime? MsthDateInput { get; set; }

    public string? MsthUserId { get; set; }

    public DateOnly? MsthDateReport { get; set; }
}
