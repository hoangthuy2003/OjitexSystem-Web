using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TCustomerAddress
{
    public int CaId { get; set; }

    public decimal? CaCusCd { get; set; }

    public int? CaLineNo { get; set; }

    public string? CaAddressName { get; set; }

    public string? CaAddressDetail { get; set; }

    public string? CaCity { get; set; }

    public string? CaCountry { get; set; }

    public string? CaPic { get; set; }

    public string? CaPicPhone { get; set; }

    public string? CaOther { get; set; }

    public DateTime? CaDateInput { get; set; }

    public string? CaUserId { get; set; }
}
