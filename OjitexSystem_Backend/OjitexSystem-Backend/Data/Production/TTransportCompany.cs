using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TTransportCompany
{
    public int TcId { get; set; }

    public string? TcName { get; set; }

    public string? TcFullName { get; set; }

    public string? TcPhone { get; set; }

    public string? TcAddress { get; set; }

    public DateTime? TcDateInput { get; set; }

    public string? TcUserId { get; set; }
}
