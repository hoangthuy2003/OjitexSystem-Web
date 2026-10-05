using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMatSupplierMaster
{
    public string SmCode { get; set; } = null!;

    public string? SmName { get; set; }

    public string? SmAddress { get; set; }

    public string? SmPhone { get; set; }

    public string? SmNote { get; set; }

    public DateTime? SmDateInput { get; set; }

    public string? SmUserId { get; set; }
}
