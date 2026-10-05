using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TDisposalProduct
{
    public int DpId { get; set; }

    public int? DpReceiveNo { get; set; }

    public decimal? DpCusCd { get; set; }

    public decimal? DpProCd { get; set; }

    public decimal? DpQuantity { get; set; }

    public string? DpRemark { get; set; }

    public string? DpPic { get; set; }

    public DateOnly? DpDateReport { get; set; }

    public DateTime? DpDateInput { get; set; }

    public DateTime? DpDateChange { get; set; }

    public string? DpUserId { get; set; }
}
