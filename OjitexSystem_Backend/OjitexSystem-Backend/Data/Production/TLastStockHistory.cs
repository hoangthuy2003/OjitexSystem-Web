using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLastStockHistory
{
    public int LsthId { get; set; }

    public int? LsthReceiveNo { get; set; }

    public decimal? LsthProCd { get; set; }

    public string? LthSection { get; set; }

    public decimal? LsthInputStock { get; set; }

    public decimal? LsthOutputStock { get; set; }

    public decimal? LsthCurrentStock { get; set; }

    public string? LsthRemark { get; set; }

    public DateOnly? LsthWorkDate { get; set; }

    public DateTime? LsthDateInput { get; set; }

    public string? LsthUserId { get; set; }
}
