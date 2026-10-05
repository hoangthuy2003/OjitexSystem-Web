using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TSubconReport
{
    public int ScrId { get; set; }

    public int? ScrReceiveNo { get; set; }

    public decimal? ScrProCd { get; set; }

    public decimal? ScrProductQty { get; set; }

    public DateOnly? ScrDate { get; set; }

    public string? ScrSubconName { get; set; }

    public string? ScrRemark { get; set; }

    public DateTime? ScrDateInput { get; set; }

    public string? ScrUserId { get; set; }
}
