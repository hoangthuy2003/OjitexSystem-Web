using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TPaperRollUsed
{
    public int PruId { get; set; }

    public string? PruSerialNo { get; set; }

    public string? PruLabel { get; set; }

    public string? PruName { get; set; }

    public decimal? PruGramage { get; set; }

    public decimal? PruWidth { get; set; }

    public decimal? PruCurrentStock { get; set; }

    public decimal? PruAfterUsed { get; set; }

    public decimal? PruUsedKg { get; set; }

    public string? PruPaperType { get; set; }

    public string? PruSection { get; set; }

    public string? PruRemark { get; set; }

    public string? PruStatus { get; set; }

    public DateTime? PruDateUsed { get; set; }

    public DateTime? PruDateInput { get; set; }

    public DateTime? PruDateChanged { get; set; }

    public DateTime? PruDateConfirm { get; set; }

    public string? PruUserId { get; set; }

    public string? PruUserIdChanged { get; set; }

    public string? PruUserIdConfirm { get; set; }
}
