using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMatSalesForecast
{
    public int SfSerialNo { get; set; }

    public DateOnly? SfDateForecast { get; set; }

    public decimal? SfCusCd { get; set; }

    public decimal? SfProCd { get; set; }

    public decimal? SfQuantity { get; set; }

    public string? SfRemark { get; set; }

    public DateTime? SfDateInput { get; set; }

    public string? SfUserId { get; set; }
}
