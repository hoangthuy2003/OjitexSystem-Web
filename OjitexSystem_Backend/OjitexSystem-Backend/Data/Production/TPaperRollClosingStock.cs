using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TPaperRollClosingStock
{
    public int PcsId { get; set; }

    public string? PcsLabel { get; set; }

    public string? PcsSerialNo { get; set; }

    public string? PcsKind { get; set; }

    public decimal? PcsGramage { get; set; }

    public decimal? PcsWidth { get; set; }

    public decimal? PcsInputKg { get; set; }

    public decimal? PcsUsedKg { get; set; }

    public decimal? PcsRemainKg { get; set; }

    public decimal? PcsRemainMetre { get; set; }

    public string? PcsPaperType { get; set; }

    public string? PcsSupplierName { get; set; }

    public string? PcsRemark { get; set; }

    public DateTime? PcsDateReceive { get; set; }

    public DateTime? PcsDateInput { get; set; }

    public DateTime? PcsDateChanged { get; set; }

    public string? PcsUserId { get; set; }
}
