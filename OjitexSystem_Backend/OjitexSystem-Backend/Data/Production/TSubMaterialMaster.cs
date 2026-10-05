using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TSubMaterialMaster
{
    public string SmmCode { get; set; } = null!;

    public string? SmmName { get; set; }

    public string? SmmUnit { get; set; }

    public string? SmmPicName { get; set; }

    public string? SmmPicPhone { get; set; }

    public string? SmmNote { get; set; }

    public string? SmmRemark { get; set; }

    public DateTime? SmmDateInput { get; set; }

    public string? SmmUserIdInput { get; set; }

    public DateTime? SmmDateChanged { get; set; }

    public string? SmmUserIdChanged { get; set; }
}
