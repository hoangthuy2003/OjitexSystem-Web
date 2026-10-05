using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TSubMaterialExportDetail
{
    public string SmedDeliverySlip { get; set; } = null!;

    public string SmedCode { get; set; } = null!;

    public string? SmedName { get; set; }

    public string? SmedUnit { get; set; }

    public decimal? SmedQuantity { get; set; }

    public string? SmedRemark { get; set; }

    public string? SmedOtherNote { get; set; }
}
