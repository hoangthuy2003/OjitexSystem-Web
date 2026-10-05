using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMatMakerMaster
{
    public string MmCode { get; set; } = null!;

    public string? MmName { get; set; }

    public string? MmAddress { get; set; }

    public string? MmPhone { get; set; }

    public string? MmNote { get; set; }

    public DateTime? MmDateInput { get; set; }

    public string? MmUserId { get; set; }
}
