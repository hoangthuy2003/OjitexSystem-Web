using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

public partial class TBkPurchaseOrder
{
    public int PoId { get; set; }

    public string? PoStatus { get; set; }

    public string? PoIssueDate { get; set; }

    public string? PoSellerPartyName { get; set; }

    public string? PoSellerPartyAddress { get; set; }

    public string? PoSellerCityName { get; set; }

    public int? PoSellerCustomerAssignAccountId { get; set; }

    public string? PoSellerIdenCode { get; set; }

    public string? PoBuyerPartyName { get; set; }

    public string? PoBuyerPartyAddress { get; set; }

    public string? PoBuyerCustomerAssignAccountId { get; set; }

    public string? PoBuyerIdenCode { get; set; }

    public string? PoDeliveryPartyName { get; set; }

    public string? PoDeliveryPartyAddress { get; set; }

    public string? PoDeliveryIdenCode { get; set; }

    public string? PoPoNumber { get; set; }

    public string? PoReqNo { get; set; }

    public string? PoBrandName { get; set; }

    public string? PoBuyerItemName { get; set; }

    public string? PoSellerItemName { get; set; }

    public string? PoInventoryTransactionId { get; set; }

    public string? PoProductDescription { get; set; }

    public string? PoReference { get; set; }

    public string? PoDimensions { get; set; }

    public decimal? PoIdl { get; set; }

    public decimal? PoIdw { get; set; }

    public decimal? PoIdh { get; set; }

    public decimal? PoOdl { get; set; }

    public decimal? PoOdw { get; set; }

    public decimal? PoOdh { get; set; }

    public DateOnly? PoRequestDate { get; set; }

    public DateOnly? PoEta { get; set; }

    public decimal? PoQty { get; set; }

    public string? PoUnit { get; set; }

    public decimal? PoPricePerUom { get; set; }

    public decimal? PoAmount { get; set; }

    public decimal? PoOjitexProductCode { get; set; }

    public string? PoOjitexProductName { get; set; }

    public decimal? PoOjitexQty { get; set; }

    public decimal? PoTotalGoodsItemQty { get; set; }

    public decimal? PoDeliveredQty { get; set; }

    public decimal? PoBackorderQty { get; set; }

    public string? PoOrderReferenceId { get; set; }

    public string? PoCustomerReference { get; set; }

    public string? PoRemark { get; set; }

    public DateTime? PoDateInput { get; set; }

    public string? PoUserId { get; set; }
}
