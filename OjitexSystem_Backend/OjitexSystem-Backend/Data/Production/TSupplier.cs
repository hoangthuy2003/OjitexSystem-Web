using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Production;

/// <summary>
/// T_Supplier
/// </summary>
public partial class TSupplier
{
    /// <summary>
    /// sup_cus_cd
    /// </summary>
    public string SupCusCd { get; set; } = null!;

    /// <summary>
    /// sup_full_name
    /// </summary>
    public string? SupFullName { get; set; }

    /// <summary>
    /// sup_address
    /// </summary>
    public string? SupAddress { get; set; }

    /// <summary>
    /// sup_tax_cd
    /// </summary>
    public string? SupTaxCd { get; set; }

    /// <summary>
    /// sup_contactName
    /// </summary>
    public string? SupContactName { get; set; }

    /// <summary>
    /// sup_phone
    /// </summary>
    public string? SupPhone { get; set; }

    /// <summary>
    /// sup_fax
    /// </summary>
    public string? SupFax { get; set; }

    /// <summary>
    /// sup_bank_name
    /// </summary>
    public string? SupBankName { get; set; }

    /// <summary>
    /// sup_bank_account
    /// </summary>
    public string? SupBankAccount { get; set; }

    /// <summary>
    /// sup_remark
    /// </summary>
    public string? SupRemark { get; set; }

    /// <summary>
    /// sup_date_input
    /// </summary>
    public DateTime? SupDateInput { get; set; }

    /// <summary>
    /// sup_user_id
    /// </summary>
    public string? SupUserId { get; set; }
}
