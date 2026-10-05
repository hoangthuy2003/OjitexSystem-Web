using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMachineSpec
{
    public string MName { get; set; } = null!;

    public int? MMinSettime { get; set; }

    public int? MEfficiency { get; set; }

    public int? MSpeed { get; set; }

    public decimal? MWorkingTime { get; set; }

    public string? MRemark { get; set; }

    public DateTime? MDateInput { get; set; }

    public string? MUserId { get; set; }
}
