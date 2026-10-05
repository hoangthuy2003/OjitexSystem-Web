using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TCurrentStock
{
    public decimal CstProCd { get; set; }

    public decimal? CstOpenStock { get; set; }

    public decimal? CstWarehouseNg { get; set; }

    public decimal? CstDisposal { get; set; }

    public decimal? CstRepair { get; set; }

    public decimal? CstProduction { get; set; }

    public decimal? CstDelivery { get; set; }

    public decimal? CstStock { get; set; }
}
