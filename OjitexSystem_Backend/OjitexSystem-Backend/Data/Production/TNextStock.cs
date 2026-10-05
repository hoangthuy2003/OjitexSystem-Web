using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TNextStock
{
    public decimal NstProCd { get; set; }

    public decimal? NstOpenStock { get; set; }

    public decimal? NstWarehouseNg { get; set; }

    public decimal? NstDisposal { get; set; }

    public decimal? NstRepair { get; set; }

    public decimal? NstProduction { get; set; }

    public decimal? NstDelivery { get; set; }

    public decimal? NstStock { get; set; }
}
