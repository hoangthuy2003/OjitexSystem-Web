using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TVolumeEstimate
{
    public long VeId { get; set; }

    public DateOnly? VeDateApply { get; set; }

    public decimal? VeVolume { get; set; }

    public string? VeRemark { get; set; }

    public DateTime? VeDateInput { get; set; }

    public string? VeUserId { get; set; }
}
