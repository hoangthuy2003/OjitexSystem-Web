using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TAcceptanceC
{
    public int ActId { get; set; }

    public string? ActHeaderId { get; set; }

    public string? ActNo { get; set; }

    public decimal? ActCusCd { get; set; }

    public string? ActCusName { get; set; }

    public decimal? ActSumOfOrder { get; set; }

    public decimal? ActSumOfStock { get; set; }

    public decimal? ActSumOfProduct { get; set; }

    public string? ActNoteAll { get; set; }

    public DateTime? ActDateInput { get; set; }

    public DateTime? ActDateChange { get; set; }

    public string? ActUserId { get; set; }
}
