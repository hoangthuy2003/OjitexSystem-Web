using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TNextStockHistory
{
    public int NsthId { get; set; }

    public int? NsthReceiveNo { get; set; }

    public decimal? NsthProCd { get; set; }

    public string? NsthSection { get; set; }

    public decimal? NsthInputStock { get; set; }

    public decimal? NsthOutputStock { get; set; }

    public decimal? NsthCurrentStock { get; set; }

    public string? NsthRemark { get; set; }

    public DateOnly? NsthWorkDate { get; set; }

    public DateTime? NsthDateInput { get; set; }

    public string? NsthUserId { get; set; }
}
