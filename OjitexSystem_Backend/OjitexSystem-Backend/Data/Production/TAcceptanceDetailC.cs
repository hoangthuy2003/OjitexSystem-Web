using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TAcceptanceDetailC
{
    public long AcdId { get; set; }

    public string? AcdHeaderId { get; set; }

    public string? AcdActNo { get; set; }

    public int? AcdLineNo { get; set; }

    public string? AcdState { get; set; }

    public int? AcdClass { get; set; }

    public int? AcdReceiveNo { get; set; }

    public decimal? AcdProCd { get; set; }

    public string? AcdProName { get; set; }

    public decimal? AcdOrderQty { get; set; }

    public decimal? AcdStock { get; set; }

    public decimal? AcdProduct { get; set; }

    public DateOnly? AcdDelivery { get; set; }

    public string? AcdPo { get; set; }

    public string? AcdRemark { get; set; }

    public string? AcdNoteDetail { get; set; }
}
