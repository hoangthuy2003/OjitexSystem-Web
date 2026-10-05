using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TSystemState
{
    public int StStateId { get; set; }

    public string? StSection { get; set; }

    public string? StFunctionId { get; set; }

    public string? StFunctionName { get; set; }

    public string? StFunctionState { get; set; }

    public string? StRemark { get; set; }

    public DateTime? StDateInput { get; set; }

    public string? StUserId { get; set; }
}
