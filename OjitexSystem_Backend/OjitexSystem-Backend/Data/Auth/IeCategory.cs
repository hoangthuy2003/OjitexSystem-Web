using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Category
/// </summary>
public partial class IeCategory
{
    /// <summary>
    /// Category ID
    /// </summary>
    public string CategoryId { get; set; } = null!;

    /// <summary>
    /// Category Name
    /// </summary>
    public string CategoryName { get; set; } = null!;

    /// <summary>
    /// Entry Date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Change Date
    /// </summary>
    public DateTime ChgDate { get; set; }
}
