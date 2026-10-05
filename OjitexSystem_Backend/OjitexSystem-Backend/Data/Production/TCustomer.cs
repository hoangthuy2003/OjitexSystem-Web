using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

/// <summary>
/// T_Customer
/// </summary>
public partial class TCustomer
{
    /// <summary>
    /// cus_cd
    /// </summary>
    public decimal CusCd { get; set; }

    /// <summary>
    /// cus_full_name
    /// </summary>
    public string? CusFullName { get; set; }

    /// <summary>
    /// cus_address
    /// </summary>
    public string? CusAddress { get; set; }

    /// <summary>
    /// cus_tax_cd
    /// </summary>
    public string? CusTaxCd { get; set; }

    /// <summary>
    /// cus_contactName
    /// </summary>
    public string? CusContactName { get; set; }

    /// <summary>
    /// cus_phone
    /// </summary>
    public string? CusPhone { get; set; }

    /// <summary>
    /// cus_fax
    /// </summary>
    public string? CusFax { get; set; }

    /// <summary>
    /// cus_bank_name
    /// </summary>
    public string? CusBankName { get; set; }

    /// <summary>
    /// cus_bank_account
    /// </summary>
    public string? CusBankAccount { get; set; }

    /// <summary>
    /// cus_remark
    /// </summary>
    public string? CusRemark { get; set; }

    /// <summary>
    /// cus_date_input
    /// </summary>
    public DateTime? CusDateInput { get; set; }

    /// <summary>
    /// cus_user_id
    /// </summary>
    public string? CusUserId { get; set; }
}
