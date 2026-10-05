using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TKamPaperMaster
{
    public string KamPaperCd { get; set; } = null!;

    public string? KamPaperName { get; set; }

    public decimal? KamUnitPrice { get; set; }

    public decimal? KamWeight { get; set; }

    public int? KamGlueGradeCd { get; set; }

    public int? KamGlueWeight { get; set; }

    public int? KamBMediumGradeCd { get; set; }

    public int? KamBMediumWeight { get; set; }

    public int? KamBLinerGradeCd { get; set; }

    public int? KamBLinerWeight { get; set; }

    public int? KamAMediumGradeCd { get; set; }

    public int? KamAMediumWeight { get; set; }

    public int? KamALinerGradeCd { get; set; }

    public int? KamALinerWeight { get; set; }

    public string? KamFlute { get; set; }

    public DateTime? KamDateInput { get; set; }

    public DateTime? KamDateChanged { get; set; }

    public string? KamUserId { get; set; }
}
