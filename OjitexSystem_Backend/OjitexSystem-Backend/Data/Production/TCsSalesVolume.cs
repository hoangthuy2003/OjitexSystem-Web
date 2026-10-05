using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TCsSalesVolume
{
    public int SvId { get; set; }

    public DateOnly? SvDeliveryDate { get; set; }

    public decimal? SvCusCd { get; set; }

    public decimal? SvProCd { get; set; }

    public decimal? SvQty { get; set; }

    public string? SvRemark { get; set; }

    public DateTime? SvDateInput { get; set; }

    public string? SvUserInput { get; set; }

    public DateTime? SvDateEdit { get; set; }

    public string? SvUserEdit { get; set; }
}
