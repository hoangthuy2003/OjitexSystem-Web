using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TDestination
{
    public string DeCode { get; set; } = null!;

    public string? DeFullName { get; set; }

    public string? DeAddress { get; set; }

    public string? DePicName { get; set; }

    public string? DePicPhone { get; set; }

    public DateTime? DeDateInput { get; set; }

    public string? DeUserId { get; set; }
}
