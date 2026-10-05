using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TPaperRollOpeningStock
{
    public int PosId { get; set; }

    public string? PosLabel { get; set; }

    public string? PosSerialNo { get; set; }

    public string? PosKind { get; set; }

    public decimal? PosGramage { get; set; }

    public decimal? PosWidth { get; set; }

    public decimal? PosInputKg { get; set; }

    public decimal? PosUsedKg { get; set; }

    public decimal? PosRemainKg { get; set; }

    public decimal? PosRemainMetre { get; set; }

    public string? PosPaperType { get; set; }

    public string? PosSupplierName { get; set; }

    public string? PosRemark { get; set; }

    public DateTime? PosDateReceive { get; set; }

    public DateTime? PosDateInput { get; set; }

    public DateTime? PosDateChanged { get; set; }

    public string? PosUserId { get; set; }
}
