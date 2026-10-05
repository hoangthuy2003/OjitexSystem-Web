using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TAdjustProduct
{
    public int ApId { get; set; }

    public int? ApReceiveNo { get; set; }

    public decimal? ApProCd { get; set; }

    public decimal? ApQuantity { get; set; }

    public decimal? ApSqm { get; set; }

    public decimal? ApTotalSqm { get; set; }

    public string? ApRemark { get; set; }

    public DateOnly? ApDate { get; set; }

    public DateTime? ApDateInput { get; set; }

    public string? ApUserId { get; set; }
}
