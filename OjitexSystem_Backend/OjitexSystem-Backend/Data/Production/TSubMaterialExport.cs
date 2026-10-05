using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

/// <summary>
/// T_SubMaterialExport
/// </summary>
public partial class TSubMaterialExport
{
    /// <summary>
    /// sme_delivery_slip
    /// </summary>
    public string SmeDeliverySlip { get; set; } = null!;

    /// <summary>
    /// sme_delivery_date
    /// </summary>
    public DateTime? SmeDeliveryDate { get; set; }

    /// <summary>
    /// sme_destination_code
    /// </summary>
    public string? SmeDestinationCode { get; set; }

    /// <summary>
    /// sme_destination_name
    /// </summary>
    public string? SmeDestinationName { get; set; }

    /// <summary>
    /// sme_transport_company
    /// </summary>
    public string? SmeTransportCompany { get; set; }

    /// <summary>
    /// sme_truck_number
    /// </summary>
    public string? SmeTruckNumber { get; set; }

    /// <summary>
    /// sme_delivery_address
    /// </summary>
    public string? SmeDeliveryAddress { get; set; }

    /// <summary>
    /// sme_note
    /// </summary>
    public string? SmeNote { get; set; }

    /// <summary>
    /// sme_car_type
    /// </summary>
    public string? SmeCarType { get; set; }

    /// <summary>
    /// sme_date_input
    /// </summary>
    public DateTime? SmeDateInput { get; set; }

    /// <summary>
    /// sme_user_id_input
    /// </summary>
    public string? SmeUserIdInput { get; set; }

    /// <summary>
    /// sme_date_changed
    /// </summary>
    public DateTime? SmeDateChanged { get; set; }

    /// <summary>
    /// sme_user_id_changed
    /// </summary>
    public string? SmeUserIdChanged { get; set; }
}
