using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TWarehousePaperRoll
{
    public int WpKey { get; set; }

    public string? WpLogiCode { get; set; }

    public string? WpGradeName { get; set; }

    public decimal? WpGramage { get; set; }

    public decimal? WpWidth { get; set; }

    public decimal? WpInput { get; set; }

    public decimal? WpUse { get; set; }

    public decimal? WpRemain { get; set; }

    public decimal? WpMetre { get; set; }

    public DateOnly? WpReceiveDate { get; set; }

    public string? WpType { get; set; }

    public string? WpRemark { get; set; }

    public DateTime? WpDateInput { get; set; }

    public string? WpUserId { get; set; }
}
