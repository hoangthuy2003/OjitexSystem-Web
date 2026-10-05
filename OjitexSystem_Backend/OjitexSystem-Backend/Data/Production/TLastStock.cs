using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLastStock
{
    public decimal LstProCd { get; set; }

    public decimal? LstOpenStock { get; set; }

    public decimal? LstWarehouseNg { get; set; }

    public decimal? LstDisposal { get; set; }

    public decimal? LstRepair { get; set; }

    public decimal? LstProduction { get; set; }

    public decimal? LstDelivery { get; set; }

    public decimal? LstStock { get; set; }
}
