using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TSpecialCustomerPoManage
{
    public int ScpmId { get; set; }

    public decimal? ScpmProCd { get; set; }

    public string? ScpmProName { get; set; }

    public string? ScpmPo { get; set; }

    public decimal? ScpmOrderQty { get; set; }

    public string? ScpmSpecialIndication { get; set; }

    public string? ScpmRemark { get; set; }

    public DateTime? ScpmDateInput { get; set; }

    public DateTime? ScpmDateChange { get; set; }

    public string? ScpmUserId { get; set; }
}
