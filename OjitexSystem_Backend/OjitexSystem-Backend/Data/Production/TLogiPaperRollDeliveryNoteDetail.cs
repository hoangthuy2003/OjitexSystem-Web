using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLogiPaperRollDeliveryNoteDetail
{
    public string PdndDeliverySlip { get; set; } = null!;

    public string PdndSerialNo { get; set; } = null!;

    public string PdndLabel { get; set; } = null!;

    public string? PdndGradeName { get; set; }

    public decimal? PdndGramage { get; set; }

    public decimal? PdndWidth { get; set; }

    public decimal? PdndExportKg { get; set; }

    public decimal? PdndExportMeter { get; set; }

    public string? PdndPaperType { get; set; }

    public string? PdndRemark { get; set; }

    public string? PdndOtherNote { get; set; }
}
