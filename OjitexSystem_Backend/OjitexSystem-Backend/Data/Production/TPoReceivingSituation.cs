using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TPoReceivingSituation
{
    public int PrsId { get; set; }

    public decimal? PrsCusCd { get; set; }

    public string? PrsFinishedBefore { get; set; }

    public string? PrsNotFinishedBefore { get; set; }

    public string? PrsConfirmationAfter { get; set; }

    public string? PrsFinishedAfter { get; set; }

    public DateOnly? PrsDateApply { get; set; }

    public DateTime? PrsDateInput { get; set; }

    public DateTime? PrsDateChange { get; set; }

    public string? PrsUserId { get; set; }
}
