using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TOjitexHoliday
{
    public int OjhKey { get; set; }

    public DateOnly? OjhDayoff { get; set; }

    public string? OjiRemark { get; set; }

    public int? OjiColor { get; set; }

    public DateTime? OjhDateInput { get; set; }

    public string? OjhUserId { get; set; }
}
