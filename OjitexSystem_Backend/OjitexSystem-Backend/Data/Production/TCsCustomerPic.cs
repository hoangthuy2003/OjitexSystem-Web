using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TCsCustomerPic
{
    public decimal PicCusCd { get; set; }

    public string? PicCusName { get; set; }

    public string? PicCsId { get; set; }

    public string? PicCsName { get; set; }

    public DateOnly? PicDateTransfer { get; set; }

    public string? PicUserTransfer { get; set; }

    public string? PicRemark { get; set; }

    public DateTime? PicDateInput { get; set; }

    public string? PicUserId { get; set; }
}
