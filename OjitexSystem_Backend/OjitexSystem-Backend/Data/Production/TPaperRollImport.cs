using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TPaperRollImport
{
    public string PrimSn { get; set; } = null!;

    public string PrimLabel { get; set; } = null!;

    public string? PrimGradeName { get; set; }

    public decimal? PrimGramage { get; set; }

    public decimal? PrimWidth { get; set; }

    public decimal? PrimImportKg { get; set; }

    public string? PrimPaperType { get; set; }

    public string? PrimContainerNo { get; set; }

    public string? PrimSupplierName { get; set; }

    public DateTime? PrimDateReceived { get; set; }

    public string? PrimRemark { get; set; }

    public DateTime? PrimDateInput { get; set; }

    public DateTime? PrimDateChanged { get; set; }

    public string? PrimUserId { get; set; }

    public string? PrimUserIdChanged { get; set; }
}
