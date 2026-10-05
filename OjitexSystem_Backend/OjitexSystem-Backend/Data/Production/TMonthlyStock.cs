using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TMonthlyStock
{
    public int MstId { get; set; }

    public decimal? MstProCd { get; set; }

    public decimal? MstOpenStock { get; set; }

    public decimal? MstWarehouseNg { get; set; }

    public decimal? MstDisposal { get; set; }

    public decimal? MstRepair { get; set; }

    public decimal? MstProduction { get; set; }

    public decimal? MstDelivery { get; set; }

    public decimal? MstCloseStock { get; set; }

    public DateOnly? MstDateReport { get; set; }
}
