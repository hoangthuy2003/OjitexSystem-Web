using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TSubconReceive
{
    public int SrId { get; set; }

    public int? SrReceiveNo { get; set; }

    public decimal? SrProCd { get; set; }

    public decimal? SrQuantity { get; set; }

    public DateOnly? SrDate { get; set; }

    public string? SrSubconName { get; set; }

    public string? SrRemark { get; set; }

    public DateTime? SrDateInput { get; set; }

    public string? SrUserId { get; set; }
}
