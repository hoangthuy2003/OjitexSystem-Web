using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TLogiDeliveryNoteDetail
{
    public long DndKey { get; set; }

    public string? DndDeliverySlip { get; set; }

    public int? DndLineNo { get; set; }

    public decimal? DndReceiveNo { get; set; }

    public decimal? DndProCd { get; set; }

    public string? DndProName { get; set; }

    public decimal? DndQty { get; set; }

    public string? DndCsPo { get; set; }

    public string? DndRemark { get; set; }

    public string? DndOtherNote { get; set; }

    public DateTime? DndDateInput { get; set; }
}
