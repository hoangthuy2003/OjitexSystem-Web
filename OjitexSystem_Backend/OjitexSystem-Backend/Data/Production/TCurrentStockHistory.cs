using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TCurrentStockHistory
{
    public int CsthId { get; set; }

    public int? CsthReceiveNo { get; set; }

    public decimal? CsthProCd { get; set; }

    public string? CsthSection { get; set; }

    public decimal? CsthInputStock { get; set; }

    public decimal? CsthOutputStock { get; set; }

    public decimal? CsthCurrentStock { get; set; }

    public string? CsthRemark { get; set; }

    public DateOnly? CsthWorkDate { get; set; }

    public DateTime? CsthDateInput { get; set; }

    public string? CsthUserId { get; set; }
}
