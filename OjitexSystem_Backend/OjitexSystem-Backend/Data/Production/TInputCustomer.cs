using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

/// <summary>
/// T_Input_Customer
/// </summary>
public partial class TInputCustomer
{
    /// <summary>
    /// cus_cd
    /// </summary>
    public decimal CusCd { get; set; }

    /// <summary>
    /// cus_name
    /// </summary>
    public string? CusName { get; set; }

    /// <summary>
    /// cus_full_name
    /// </summary>
    public string? CusFullName { get; set; }

    /// <summary>
    /// cus_nation
    /// </summary>
    public string? CusNation { get; set; }

    /// <summary>
    /// cus_address
    /// </summary>
    public string? CusAddress { get; set; }

    /// <summary>
    /// cus_tax_cd
    /// </summary>
    public string? CusTaxCd { get; set; }

    /// <summary>
    /// cus_bank_account
    /// </summary>
    public string? CusBankAccount { get; set; }

    /// <summary>
    /// cus_bank_name
    /// </summary>
    public string? CusBankName { get; set; }

    /// <summary>
    /// cus_phone
    /// </summary>
    public string? CusPhone { get; set; }

    /// <summary>
    /// cus_fax
    /// </summary>
    public string? CusFax { get; set; }

    /// <summary>
    /// cus_email
    /// </summary>
    public string? CusEmail { get; set; }

    /// <summary>
    /// cus_vat
    /// </summary>
    public decimal? CusVat { get; set; }

    /// <summary>
    /// cus_person_in_chart
    /// </summary>
    public string? CusPersonInChart { get; set; }

    /// <summary>
    /// cus_date_input
    /// </summary>
    public DateTime? CusDateInput { get; set; }

    /// <summary>
    /// cus_user_id
    /// </summary>
    public string? CusUserId { get; set; }
}
