using System;
using System.Collections.Generic;

namespace OjitexSystem_Backend.Data.Auth;

/// <summary>
/// EVOLIO Operation Log
/// </summary>
public partial class EvolioOperationLog
{
    /// <summary>
    /// Record No
    /// </summary>
    public long RecNo { get; set; }

    /// <summary>
    /// Operation date time
    /// </summary>
    public decimal Dttm { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Last name
    /// </summary>
    public string? UserFamilyName { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public string? UserFirstName { get; set; }

    /// <summary>
    /// Login Date
    /// </summary>
    public decimal UserLoginDate { get; set; }

    /// <summary>
    /// Login Time
    /// </summary>
    public decimal UserLoginTime { get; set; }

    /// <summary>
    /// Program type
    /// </summary>
    public string PgType { get; set; } = null!;

    /// <summary>
    /// Operation type
    /// </summary>
    public string OperationType { get; set; } = null!;

    /// <summary>
    /// Output type
    /// </summary>
    public string? QueryType { get; set; }

    /// <summary>
    /// Query group id
    /// </summary>
    public string? QueryId { get; set; }

    /// <summary>
    /// Query group name
    /// </summary>
    public string? QueryName { get; set; }

    /// <summary>
    /// Execute sequence
    /// </summary>
    public decimal ExecSeq { get; set; }

    /// <summary>
    /// Sub query name
    /// </summary>
    public string? SubQueryName { get; set; }

    /// <summary>
    /// Sub query type
    /// </summary>
    public string? SubQueryType { get; set; }

    /// <summary>
    /// Entry date
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Major version
    /// </summary>
    public decimal GrpQueryVerMajor { get; set; }

    /// <summary>
    /// Minor version
    /// </summary>
    public decimal GrpQueryVerMinor { get; set; }

    /// <summary>
    /// Computer id
    /// </summary>
    public string? ComputerId { get; set; }

    /// <summary>
    /// Error Flag
    /// </summary>
    public string? ErrorFlag { get; set; }

    /// <summary>
    /// Error Message
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Reserve item 1
    /// </summary>
    public string? DataVarchar1 { get; set; }

    /// <summary>
    /// Reserve item 2
    /// </summary>
    public string? DataVarchar2 { get; set; }

    /// <summary>
    /// Reserve item 3
    /// </summary>
    public string? DataVarchar3 { get; set; }

    /// <summary>
    /// Reserve item 4
    /// </summary>
    public string? DataVarchar4 { get; set; }

    /// <summary>
    /// Reserve item 5
    /// </summary>
    public string? DataVarchar5 { get; set; }

    /// <summary>
    /// Reserve item 6
    /// </summary>
    public string? DataVarchar6 { get; set; }

    /// <summary>
    /// Reserve item 7
    /// </summary>
    public string? DataVarchar7 { get; set; }

    /// <summary>
    /// Reserve item 8
    /// </summary>
    public string? DataVarchar8 { get; set; }

    /// <summary>
    /// Reserve item 9
    /// </summary>
    public string? DataVarchar9 { get; set; }

    /// <summary>
    /// Reserve item 10
    /// </summary>
    public string? DataVarchar10 { get; set; }
}
