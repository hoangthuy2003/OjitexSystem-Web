using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMachineSpecification
{
    public string MsMachineName { get; set; } = null!;

    public int? MsLengthMin { get; set; }

    public int? MsLengthMax { get; set; }

    public int? MsWidthMin { get; set; }

    public int? MsWidthMax { get; set; }

    public int? MsHighMin { get; set; }

    public int? MsHighMax { get; set; }

    public int? MsDeepOfSlotMin { get; set; }

    public int? MsDeepOfSlotMax { get; set; }

    public int? MsLengthOfFirstFlapMin { get; set; }

    public int? MsLengthOfFirstFlapMax { get; set; }

    public int? MsLengthOfSecondFlapMin { get; set; }

    public int? MsLengthOfSecondFlapMax { get; set; }

    public string? MsRemark { get; set; }

    public DateTime? MsDateInput { get; set; }

    public string? MsUserInput { get; set; }

    public DateTime? MsDateChanged { get; set; }

    public string? MsUserChanged { get; set; }
}
