using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TGenPaperMaster
{
    public int GenGradeCd { get; set; }

    public string? GenGradeName { get; set; }

    public decimal? GenActual { get; set; }

    public decimal? GenBudget { get; set; }

    public DateTime? GenDateInput { get; set; }

    public DateTime? GenDateChange { get; set; }

    public string? GenUserId { get; set; }
}
