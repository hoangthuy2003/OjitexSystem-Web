using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TOtherProduct
{
    public int OpId { get; set; }

    public int? OpReceiveNo { get; set; }

    public decimal? OpProCd { get; set; }

    public decimal? OpQuantity { get; set; }

    public decimal? OpSqm { get; set; }

    public decimal? OpTotalSqm { get; set; }

    public string? OpSection { get; set; }

    public string? OpRemark { get; set; }

    public DateOnly? OpDate { get; set; }

    public DateTime? OpDateInput { get; set; }

    public string? OpUserId { get; set; }
}
