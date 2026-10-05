using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TPaperRollExport
{
    public string PreSn { get; set; } = null!;

    public string PreLabel { get; set; } = null!;

    public string? PreGradeName { get; set; }

    public decimal? PreGramage { get; set; }

    public decimal? PreWidth { get; set; }

    public decimal? PreExportKg { get; set; }

    public string? PrePaperType { get; set; }

    public string? PreContainerNo { get; set; }

    public string? PreDestination { get; set; }

    public DateTime? PreDateDelivery { get; set; }

    public string? PreRemark { get; set; }

    public DateTime? PreDateInput { get; set; }

    public DateTime? PreDateChanged { get; set; }

    public string? PreUserId { get; set; }
}
