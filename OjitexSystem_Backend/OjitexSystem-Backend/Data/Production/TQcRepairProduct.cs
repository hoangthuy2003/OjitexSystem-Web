using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TQcRepairProduct
{
    public int QcrpId { get; set; }

    public decimal? QcrpReceiveNo { get; set; }

    public decimal? QcrpProCd { get; set; }

    public string? QcrpProName { get; set; }

    public decimal? QcrpBackloadQty { get; set; }

    public decimal? QcrpRealQtyReceived { get; set; }

    public decimal? QcrpOkQty { get; set; }

    public decimal? QcrpNgQty { get; set; }

    public string? QcrpRemark { get; set; }

    public DateOnly? QcrpReceivedDate { get; set; }

    public DateTime? QcrpDateInput { get; set; }

    public DateTime? QcrpDateChange { get; set; }

    public string? QcrpUserId { get; set; }
}
