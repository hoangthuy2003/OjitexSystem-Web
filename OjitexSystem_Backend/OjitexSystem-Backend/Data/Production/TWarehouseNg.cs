using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TWarehouseNg
{
    public int WnId { get; set; }

    public int? WnReceiveNo { get; set; }

    public decimal? WnCusCd { get; set; }

    public decimal? WnProCd { get; set; }

    public decimal? WnQuantity { get; set; }

    public string? WnRemark { get; set; }

    public string? WnPic { get; set; }

    public DateOnly? WnDateReport { get; set; }

    public DateTime? WnDateInput { get; set; }

    public string? WnUserId { get; set; }
}
