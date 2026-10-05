using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OjitexSystem_Backend.Data.Auth;

public partial class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EvolioOperationLog> EvolioOperationLogs { get; set; }

    public virtual DbSet<EvolioOperationLogDetail> EvolioOperationLogDetails { get; set; }

    public virtual DbSet<IeAliasXml> IeAliasXmls { get; set; }

    public virtual DbSet<IeCategory> IeCategories { get; set; }

    public virtual DbSet<IeCategoryAuth> IeCategoryAuths { get; set; }

    public virtual DbSet<IeCondXml> IeCondXmls { get; set; }

    public virtual DbSet<IeCrtColumn> IeCrtColumns { get; set; }

    public virtual DbSet<IeCrtTable> IeCrtTables { get; set; }

    public virtual DbSet<IeGroupQuery> IeGroupQueries { get; set; }

    public virtual DbSet<IeJobDefine> IeJobDefines { get; set; }

    public virtual DbSet<IeJobParamDefine> IeJobParamDefines { get; set; }

    public virtual DbSet<IeJobValueDefine> IeJobValueDefines { get; set; }

    public virtual DbSet<IeJoinColDefine> IeJoinColDefines { get; set; }

    public virtual DbSet<IeJoinDefine> IeJoinDefines { get; set; }

    public virtual DbSet<IeLoginHist> IeLoginHists { get; set; }

    public virtual DbSet<IeNumberingDefine> IeNumberingDefines { get; set; }

    public virtual DbSet<IeParamXml> IeParamXmls { get; set; }

    public virtual DbSet<IeProcDefine> IeProcDefines { get; set; }

    public virtual DbSet<IeProcParamDefine> IeProcParamDefines { get; set; }

    public virtual DbSet<IeQueryDefine> IeQueryDefines { get; set; }

    public virtual DbSet<IeQueryLibrary> IeQueryLibraries { get; set; }

    public virtual DbSet<IeRole> IeRoles { get; set; }

    public virtual DbSet<IeSchemaAuth> IeSchemaAuths { get; set; }

    public virtual DbSet<IeSelectDefine> IeSelectDefines { get; set; }

    public virtual DbSet<IeSelectQuery> IeSelectQueries { get; set; }

    public virtual DbSet<IeTableDefine> IeTableDefines { get; set; }

    public virtual DbSet<IeTerminal> IeTerminals { get; set; }

    public virtual DbSet<IeUpdateXml> IeUpdateXmls { get; set; }

    public virtual DbSet<IeUser> IeUsers { get; set; }

    public virtual DbSet<IeUserMapping> IeUserMappings { get; set; }

    public virtual DbSet<IeUserRole> IeUserRoles { get; set; }

    public virtual DbSet<IeValueDefine> IeValueDefines { get; set; }

    public virtual DbSet<IeValueQryDefine> IeValueQryDefines { get; set; }

    public virtual DbSet<IeValueXml> IeValueXmls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EvolioOperationLog>(entity =>
        {
            entity.HasKey(e => e.RecNo);

            entity.ToTable("EVOLIO_OPERATION_LOG", tb => tb.HasComment("EVOLIO Operation Log"));

            entity.HasIndex(e => new { e.Dttm, e.RecNo }, "EVOLIO_OPERATION_LOG_ID1");

            entity.HasIndex(e => new { e.UserId, e.Dttm, e.RecNo }, "EVOLIO_OPERATION_LOG_ID2");

            entity.HasIndex(e => new { e.QueryId, e.Dttm, e.RecNo }, "EVOLIO_OPERATION_LOG_ID3");

            entity.Property(e => e.RecNo)
                .HasComment("Record No")
                .HasColumnName("REC_NO");
            entity.Property(e => e.ComputerId)
                .HasMaxLength(100)
                .HasComment("Computer id")
                .HasColumnName("COMPUTER_ID");
            entity.Property(e => e.DataVarchar1)
                .HasMaxLength(300)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHAR1");
            entity.Property(e => e.DataVarchar10)
                .HasMaxLength(300)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHAR10");
            entity.Property(e => e.DataVarchar2)
                .HasMaxLength(300)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHAR2");
            entity.Property(e => e.DataVarchar3)
                .HasMaxLength(300)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHAR3");
            entity.Property(e => e.DataVarchar4)
                .HasMaxLength(300)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHAR4");
            entity.Property(e => e.DataVarchar5)
                .HasMaxLength(300)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHAR5");
            entity.Property(e => e.DataVarchar6)
                .HasMaxLength(300)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHAR6");
            entity.Property(e => e.DataVarchar7)
                .HasMaxLength(300)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHAR7");
            entity.Property(e => e.DataVarchar8)
                .HasMaxLength(300)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHAR8");
            entity.Property(e => e.DataVarchar9)
                .HasMaxLength(300)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHAR9");
            entity.Property(e => e.Dttm)
                .HasComment("Operation date time")
                .HasColumnType("decimal(14, 0)")
                .HasColumnName("DTTM");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.ErrorFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Error Flag")
                .HasColumnName("ERROR_FLAG");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(300)
                .HasComment("Error Message")
                .HasColumnName("ERROR_MESSAGE");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute sequence")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.GrpQueryVerMajor)
                .HasComment("Major version")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("GRP_QUERY_VER_MAJOR");
            entity.Property(e => e.GrpQueryVerMinor)
                .HasComment("Minor version")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("GRP_QUERY_VER_MINOR");
            entity.Property(e => e.OperationType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Operation type")
                .HasColumnName("OPERATION_TYPE");
            entity.Property(e => e.PgType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Program type")
                .HasColumnName("PG_TYPE");
            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query group id")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.QueryName)
                .HasMaxLength(128)
                .HasComment("Query group name")
                .HasColumnName("QUERY_NAME");
            entity.Property(e => e.QueryType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Output type")
                .HasColumnName("QUERY_TYPE");
            entity.Property(e => e.SubQueryName)
                .HasMaxLength(128)
                .HasComment("Sub query name")
                .HasColumnName("SUB_QUERY_NAME");
            entity.Property(e => e.SubQueryType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Sub query type")
                .HasColumnName("SUB_QUERY_TYPE");
            entity.Property(e => e.UserFamilyName)
                .HasMaxLength(40)
                .HasComment("Last name")
                .HasColumnName("USER_FAMILY_NAME");
            entity.Property(e => e.UserFirstName)
                .HasMaxLength(40)
                .HasComment("First name")
                .HasColumnName("USER_FIRST_NAME");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasComment("User ID")
                .HasColumnName("USER_ID");
            entity.Property(e => e.UserLoginDate)
                .HasComment("Login Date")
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("USER_LOGIN_DATE");
            entity.Property(e => e.UserLoginTime)
                .HasComment("Login Time")
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("USER_LOGIN_TIME");
        });

        modelBuilder.Entity<EvolioOperationLogDetail>(entity =>
        {
            entity.HasKey(e => e.RecNo);

            entity.ToTable("EVOLIO_OPERATION_LOG_DETAIL", tb => tb.HasComment("EVOLIO Operation Log Detail"));

            entity.HasIndex(e => e.ParentRecNo, "EVOLIO_OPERATION_LOG_DETAIL_ID1");

            entity.Property(e => e.RecNo)
                .HasComment("Record No")
                .HasColumnName("REC_NO");
            entity.Property(e => e.DataVarchar1)
                .HasMaxLength(300)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHAR1");
            entity.Property(e => e.DataVarchar10)
                .HasMaxLength(300)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHAR10");
            entity.Property(e => e.DataVarchar2)
                .HasMaxLength(300)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHAR2");
            entity.Property(e => e.DataVarchar3)
                .HasMaxLength(300)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHAR3");
            entity.Property(e => e.DataVarchar4)
                .HasMaxLength(300)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHAR4");
            entity.Property(e => e.DataVarchar5)
                .HasMaxLength(300)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHAR5");
            entity.Property(e => e.DataVarchar6)
                .HasMaxLength(300)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHAR6");
            entity.Property(e => e.DataVarchar7)
                .HasMaxLength(300)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHAR7");
            entity.Property(e => e.DataVarchar8)
                .HasMaxLength(300)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHAR8");
            entity.Property(e => e.DataVarchar9)
                .HasMaxLength(300)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHAR9");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.Parameters)
                .HasMaxLength(2000)
                .HasComment("Parameters")
                .HasColumnName("PARAMETERS");
            entity.Property(e => e.ParentRecNo)
                .HasComment("Parent record No")
                .HasColumnName("PARENT_REC_NO");
            entity.Property(e => e.SqlText)
                .HasComment("Sql statement")
                .HasColumnName("SQL_TEXT");
        });

        modelBuilder.Entity<IeAliasXml>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.RowNo });

            entity.ToTable("IE_ALIAS_XML", tb => tb.HasComment("EVOLIO Alias Xml"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.RowNo)
                .HasComment("Line number")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("ROW_NO");
            entity.Property(e => e.AliasXml)
                .HasComment("Alias Xml")
                .HasColumnName("ALIAS_XML");
        });

        modelBuilder.Entity<IeCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("IE_CATEGORY", tb => tb.HasComment("EVOLIO Category"));

            entity.Property(e => e.CategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Category ID")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(128)
                .HasComment("Category Name")
                .HasColumnName("CATEGORY_NAME");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
        });

        modelBuilder.Entity<IeCategoryAuth>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.AuthType, e.UserId, e.Role });

            entity.ToTable("IE_CATEGORY_AUTH", tb => tb.HasComment("EVOLIO Category Authority"));

            entity.Property(e => e.CategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Category ID")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.AuthType)
                .HasComment("Specified division")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("AUTH_TYPE");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasComment("User ID")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasComment("Role")
                .HasColumnName("ROLE");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.ExecutableType)
                .HasComment("Executable flag")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXECUTABLE_TYPE");
        });

        modelBuilder.Entity<IeCondXml>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.RowNo });

            entity.ToTable("IE_COND_XML", tb => tb.HasComment("EVOLIO Conditions Xml"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.RowNo)
                .HasComment("Line number")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("ROW_NO");
            entity.Property(e => e.CondXml)
                .HasComment("Conditions Xml")
                .HasColumnName("COND_XML");
        });

        modelBuilder.Entity<IeCrtColumn>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ColumnSeq });

            entity.ToTable("IE_CRT_COLUMN", tb => tb.HasComment("EVOLIO CreateTableColumn Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ColumnSeq)
                .HasComment("Column SEQ")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("COLUMN_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.ColumnDefault)
                .HasMaxLength(2000)
                .HasComment("Default Value")
                .HasColumnName("COLUMN_DEFAULT");
            entity.Property(e => e.ColumnId)
                .HasMaxLength(128)
                .HasComment("Column name")
                .HasColumnName("COLUMN_ID");
            entity.Property(e => e.ColumnLabel)
                .HasMaxLength(50)
                .HasComment("Column Label")
                .HasColumnName("COLUMN_LABEL");
            entity.Property(e => e.DataScale)
                .HasComment("Data scale")
                .HasColumnName("DATA_SCALE");
            entity.Property(e => e.DataSize)
                .HasComment("Data size")
                .HasColumnName("DATA_SIZE");
            entity.Property(e => e.DataType)
                .HasMaxLength(128)
                .HasComment("Data type")
                .HasColumnName("DATA_TYPE");
            entity.Property(e => e.DataVarchr1)
                .HasMaxLength(100)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHR1");
            entity.Property(e => e.DataVarchr10)
                .HasMaxLength(100)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHR10");
            entity.Property(e => e.DataVarchr2)
                .HasMaxLength(100)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHR2");
            entity.Property(e => e.DataVarchr3)
                .HasMaxLength(100)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHR3");
            entity.Property(e => e.DataVarchr4)
                .HasMaxLength(100)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHR4");
            entity.Property(e => e.DataVarchr5)
                .HasMaxLength(100)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHR5");
            entity.Property(e => e.DataVarchr6)
                .HasMaxLength(100)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHR6");
            entity.Property(e => e.DataVarchr7)
                .HasMaxLength(100)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHR7");
            entity.Property(e => e.DataVarchr8)
                .HasMaxLength(100)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHR8");
            entity.Property(e => e.DataVarchr9)
                .HasMaxLength(100)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHR9");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.NotnullFlag)
                .HasComment("Not null flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("NOTNULL_FLAG");
            entity.Property(e => e.PrimaryKeyOrd)
                .HasComment("Primary key order")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("PRIMARY_KEY_ORD");
        });

        modelBuilder.Entity<IeCrtTable>(entity =>
        {
            entity.HasKey(e => e.QueryId);

            entity.ToTable("IE_CRT_TABLE", tb => tb.HasComment("EVOLIO CreateTable Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.CategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Catetory ID")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.ChangeHist)
                .HasComment("Change History")
                .HasColumnName("CHANGE_HIST");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.DataVarchr1)
                .HasMaxLength(100)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHR1");
            entity.Property(e => e.DataVarchr10)
                .HasMaxLength(100)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHR10");
            entity.Property(e => e.DataVarchr2)
                .HasMaxLength(100)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHR2");
            entity.Property(e => e.DataVarchr3)
                .HasMaxLength(100)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHR3");
            entity.Property(e => e.DataVarchr4)
                .HasMaxLength(100)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHR4");
            entity.Property(e => e.DataVarchr5)
                .HasMaxLength(100)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHR5");
            entity.Property(e => e.DataVarchr6)
                .HasMaxLength(100)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHR6");
            entity.Property(e => e.DataVarchr7)
                .HasMaxLength(100)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHR7");
            entity.Property(e => e.DataVarchr8)
                .HasMaxLength(100)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHR8");
            entity.Property(e => e.DataVarchr9)
                .HasMaxLength(100)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHR9");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.LibId)
                .HasMaxLength(128)
                .HasComment("Library Name")
                .HasColumnName("LIB_ID");
            entity.Property(e => e.PrimaryKeyId)
                .HasMaxLength(128)
                .HasComment("PrimaryKey CST name")
                .HasColumnName("PRIMARY_KEY_ID");
            entity.Property(e => e.QueryName)
                .HasMaxLength(128)
                .HasComment("Query name")
                .HasColumnName("QUERY_NAME");
            entity.Property(e => e.Remarks)
                .HasMaxLength(2000)
                .HasComment("Remarks")
                .HasColumnName("REMARKS");
            entity.Property(e => e.Summary)
                .HasMaxLength(2000)
                .HasComment("Summary")
                .HasColumnName("SUMMARY");
            entity.Property(e => e.TableId)
                .HasMaxLength(128)
                .HasComment("Table ID")
                .HasColumnName("TABLE_ID");
            entity.Property(e => e.TableLabel)
                .HasMaxLength(50)
                .HasComment("Table name label")
                .HasColumnName("TABLE_LABEL");
        });

        modelBuilder.Entity<IeGroupQuery>(entity =>
        {
            entity.HasKey(e => e.QueryId);

            entity.ToTable("IE_GROUP_QUERY", tb => tb.HasComment("EVOLIO GroupQuery Define"));

            entity.HasIndex(e => new { e.CategoryId, e.QueryId }, "IDX_IE_GROUP_QUERY_01");

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.CategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Catetory ID")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.ChangeHist)
                .HasComment("Change History")
                .HasColumnName("CHANGE_HIST");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.QueryName)
                .HasMaxLength(128)
                .HasComment("GroupQuery name")
                .HasColumnName("QUERY_NAME");
            entity.Property(e => e.Remarks)
                .HasMaxLength(2000)
                .HasComment("Remarks")
                .HasColumnName("REMARKS");
            entity.Property(e => e.Summary)
                .HasMaxLength(2000)
                .HasComment("Summary")
                .HasColumnName("SUMMARY");
            entity.Property(e => e.VerMajor)
                .HasComment("Major version")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("VER_MAJOR");
            entity.Property(e => e.VerMinor)
                .HasComment("Minor version")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("VER_MINOR");
            entity.Property(e => e.XsdDefine)
                .HasComment("XSD define")
                .HasColumnName("XSD_DEFINE");
        });

        modelBuilder.Entity<IeJobDefine>(entity =>
        {
            entity.HasKey(e => e.JobId);

            entity.ToTable("IE_JOB_DEFINE", tb => tb.HasComment("EVOLIO JOB Define"));

            entity.Property(e => e.JobId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("JOB ID")
                .HasColumnName("JOB_ID");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.DataVarchr1)
                .HasMaxLength(100)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHR1");
            entity.Property(e => e.DataVarchr10)
                .HasMaxLength(100)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHR10");
            entity.Property(e => e.DataVarchr2)
                .HasMaxLength(100)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHR2");
            entity.Property(e => e.DataVarchr3)
                .HasMaxLength(100)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHR3");
            entity.Property(e => e.DataVarchr4)
                .HasMaxLength(100)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHR4");
            entity.Property(e => e.DataVarchr5)
                .HasMaxLength(100)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHR5");
            entity.Property(e => e.DataVarchr6)
                .HasMaxLength(100)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHR6");
            entity.Property(e => e.DataVarchr7)
                .HasMaxLength(100)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHR7");
            entity.Property(e => e.DataVarchr8)
                .HasMaxLength(100)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHR8");
            entity.Property(e => e.DataVarchr9)
                .HasMaxLength(100)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHR9");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.ExternalLang)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("External lang")
                .HasColumnName("EXTERNAL_LANG");
            entity.Property(e => e.ExternalLib)
                .HasMaxLength(128)
                .HasComment("External library")
                .HasColumnName("EXTERNAL_LIB");
            entity.Property(e => e.ExternalModule)
                .HasMaxLength(128)
                .HasComment("External module name")
                .HasColumnName("EXTERNAL_MODULE");
            entity.Property(e => e.ProcedureLabel)
                .HasMaxLength(50)
                .HasComment("Procedure label")
                .HasColumnName("PROCEDURE_LABEL");
            entity.Property(e => e.ProcedureName)
                .HasMaxLength(42)
                .HasComment("Procedure Name")
                .HasColumnName("PROCEDURE_NAME");
            entity.Property(e => e.ResultSets)
                .HasComment("Result sets count")
                .HasColumnName("RESULT_SETS");
        });

        modelBuilder.Entity<IeJobParamDefine>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.ParamSeq });

            entity.ToTable("IE_JOB_PARAM_DEFINE", tb => tb.HasComment("EVOLIO JOB Params Define"));

            entity.Property(e => e.JobId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("JOB ID")
                .HasColumnName("JOB_ID");
            entity.Property(e => e.ParamSeq)
                .HasComment("Parameter SEQ")
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("PARAM_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.DataInout)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("InOut")
                .HasColumnName("DATA_INOUT");
            entity.Property(e => e.DataScale)
                .HasComment("Data scale")
                .HasColumnName("DATA_SCALE");
            entity.Property(e => e.DataSize)
                .HasComment("Data size")
                .HasColumnName("DATA_SIZE");
            entity.Property(e => e.DataType)
                .HasMaxLength(128)
                .HasComment("Data type")
                .HasColumnName("DATA_TYPE");
            entity.Property(e => e.DataVarchr1)
                .HasMaxLength(100)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHR1");
            entity.Property(e => e.DataVarchr10)
                .HasMaxLength(100)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHR10");
            entity.Property(e => e.DataVarchr2)
                .HasMaxLength(100)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHR2");
            entity.Property(e => e.DataVarchr3)
                .HasMaxLength(100)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHR3");
            entity.Property(e => e.DataVarchr4)
                .HasMaxLength(100)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHR4");
            entity.Property(e => e.DataVarchr5)
                .HasMaxLength(100)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHR5");
            entity.Property(e => e.DataVarchr6)
                .HasMaxLength(100)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHR6");
            entity.Property(e => e.DataVarchr7)
                .HasMaxLength(100)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHR7");
            entity.Property(e => e.DataVarchr8)
                .HasMaxLength(100)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHR8");
            entity.Property(e => e.DataVarchr9)
                .HasMaxLength(100)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHR9");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.ParamName)
                .HasMaxLength(128)
                .HasComment("Parameter name")
                .HasColumnName("PARAM_NAME");
        });

        modelBuilder.Entity<IeJobValueDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.ParamSeq });

            entity.ToTable("IE_JOB_VALUE_DEFINE", tb => tb.HasComment("EVOLIO JOB ParamsValue Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.ParamSeq)
                .HasComment("Parameter SEQ")
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("PARAM_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.DataValue)
                .HasMaxLength(2000)
                .HasComment("Data value")
                .HasColumnName("DATA_VALUE");
            entity.Property(e => e.DataVarchr1)
                .HasMaxLength(100)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHR1");
            entity.Property(e => e.DataVarchr10)
                .HasMaxLength(100)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHR10");
            entity.Property(e => e.DataVarchr2)
                .HasMaxLength(100)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHR2");
            entity.Property(e => e.DataVarchr3)
                .HasMaxLength(100)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHR3");
            entity.Property(e => e.DataVarchr4)
                .HasMaxLength(100)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHR4");
            entity.Property(e => e.DataVarchr5)
                .HasMaxLength(100)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHR5");
            entity.Property(e => e.DataVarchr6)
                .HasMaxLength(100)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHR6");
            entity.Property(e => e.DataVarchr7)
                .HasMaxLength(100)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHR7");
            entity.Property(e => e.DataVarchr8)
                .HasMaxLength(100)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHR8");
            entity.Property(e => e.DataVarchr9)
                .HasMaxLength(100)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHR9");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.NotnullFlag)
                .HasComment("Not null flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("NOTNULL_FLAG");
            entity.Property(e => e.ReturnFlag)
                .HasComment("Return flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("RETURN_FLAG");
            entity.Property(e => e.ReturnNormalValue)
                .HasMaxLength(100)
                .HasComment("Return normal value")
                .HasColumnName("RETURN_NORMAL_VALUE");
        });

        modelBuilder.Entity<IeJoinColDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.JoinSeq, e.ColumnSeq });

            entity.ToTable("IE_JOIN_COL_DEFINE", tb => tb.HasComment("EVOLIO JoinColumn Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.JoinSeq)
                .HasComment("Join SEQ")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("JOIN_SEQ");
            entity.Property(e => e.ColumnSeq)
                .HasComment("Column SEQ")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("COLUMN_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EndX)
                .HasComment("End X")
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("END_X");
            entity.Property(e => e.EndY)
                .HasComment("End Y")
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("END_Y");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.JoinType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Join type")
                .HasColumnName("JOIN_TYPE");
            entity.Property(e => e.LeftColumn)
                .HasMaxLength(128)
                .HasComment("Left column")
                .HasColumnName("LEFT_COLUMN");
            entity.Property(e => e.RightColumn)
                .HasMaxLength(128)
                .HasComment("Right column")
                .HasColumnName("RIGHT_COLUMN");
            entity.Property(e => e.StartX)
                .HasComment("Start X")
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("START_X");
            entity.Property(e => e.StartY)
                .HasComment("Start Y")
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("START_Y");
        });

        modelBuilder.Entity<IeJoinDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.JoinSeq });

            entity.ToTable("IE_JOIN_DEFINE", tb => tb.HasComment("EVOLIO Join Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.JoinSeq)
                .HasComment("Join SEQ")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("JOIN_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.LeftAliasId)
                .HasMaxLength(128)
                .HasComment("Left alias id")
                .HasColumnName("LEFT_ALIAS_ID");
            entity.Property(e => e.LeftAll)
                .HasComment("Left all select")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("LEFT_ALL");
            entity.Property(e => e.RightAliasId)
                .HasMaxLength(128)
                .HasComment("Right alias id")
                .HasColumnName("RIGHT_ALIAS_ID");
            entity.Property(e => e.RightAll)
                .HasComment("Right all select")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("RIGHT_ALL");
        });

        modelBuilder.Entity<IeLoginHist>(entity =>
        {
            entity.HasKey(e => new { e.Dttm, e.UserId, e.PgType, e.LoginType });

            entity.ToTable("IE_LOGIN_HIST", tb => tb.HasComment("EVOLIO Login History"));

            entity.Property(e => e.Dttm)
                .HasComment("Login date time")
                .HasColumnType("decimal(14, 0)")
                .HasColumnName("DTTM");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasComment("User ID")
                .HasColumnName("USER_ID");
            entity.Property(e => e.PgType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Program Type")
                .HasColumnName("PG_TYPE");
            entity.Property(e => e.LoginType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Login type")
                .HasColumnName("LOGIN_TYPE");
            entity.Property(e => e.Biko)
                .HasMaxLength(500)
                .HasComment("Remarks")
                .HasColumnName("BIKO");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.LoginId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("Internal ID")
                .HasColumnName("LOGIN_ID");
        });

        modelBuilder.Entity<IeNumberingDefine>(entity =>
        {
            entity.HasKey(e => new { e.CtrCd, e.CtrSubCd });

            entity.ToTable("IE_NUMBERING_DEFINE", tb => tb.HasComment("EVOLIO Numbering"));

            entity.Property(e => e.CtrCd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Sequential number code")
                .HasColumnName("CTR_CD");
            entity.Property(e => e.CtrSubCd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Sequential number sub code")
                .HasColumnName("CTR_SUB_CD");
            entity.Property(e => e.BegCtr)
                .HasComment("Minimum")
                .HasColumnType("decimal(11, 0)")
                .HasColumnName("BEG_CTR");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EndCtr)
                .HasComment("Maximum")
                .HasColumnType("decimal(11, 0)")
                .HasColumnName("END_CTR");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.LastCtr)
                .HasComment("Sequential number")
                .HasColumnType("decimal(11, 0)")
                .HasColumnName("LAST_CTR");
        });

        modelBuilder.Entity<IeParamXml>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.RowNo });

            entity.ToTable("IE_PARAM_XML", tb => tb.HasComment("EVOLIO Parameter Xml"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.RowNo)
                .HasComment("Line number")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("ROW_NO");
            entity.Property(e => e.ParamXml)
                .HasComment("Parameter Xml")
                .HasColumnName("PARAM_XML");
        });

        modelBuilder.Entity<IeProcDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq });

            entity.ToTable("IE_PROC_DEFINE", tb => tb.HasComment("EVOLIO Proc Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.LibName)
                .HasMaxLength(128)
                .HasComment("Library name")
                .HasColumnName("LIB_NAME");
            entity.Property(e => e.ModuleName)
                .HasMaxLength(128)
                .HasComment("Module name")
                .HasColumnName("MODULE_NAME");
        });

        modelBuilder.Entity<IeProcParamDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.ParamSeq });

            entity.ToTable("IE_PROC_PARAM_DEFINE", tb => tb.HasComment("EVOLIO Proc Params Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.ParamSeq)
                .HasComment("Parameter SEQ")
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("PARAM_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.DataInout)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasComment("InOut")
                .HasColumnName("DATA_INOUT");
            entity.Property(e => e.DataScale)
                .HasComment("Data scale")
                .HasColumnName("DATA_SCALE");
            entity.Property(e => e.DataSize)
                .HasComment("Data size")
                .HasColumnName("DATA_SIZE");
            entity.Property(e => e.DataType)
                .HasMaxLength(128)
                .HasComment("Data type")
                .HasColumnName("DATA_TYPE");
            entity.Property(e => e.DataValue)
                .HasMaxLength(2000)
                .HasComment("Data value")
                .HasColumnName("DATA_VALUE");
            entity.Property(e => e.DataVarchr1)
                .HasMaxLength(100)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHR1");
            entity.Property(e => e.DataVarchr10)
                .HasMaxLength(100)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHR10");
            entity.Property(e => e.DataVarchr2)
                .HasMaxLength(100)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHR2");
            entity.Property(e => e.DataVarchr3)
                .HasMaxLength(100)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHR3");
            entity.Property(e => e.DataVarchr4)
                .HasMaxLength(100)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHR4");
            entity.Property(e => e.DataVarchr5)
                .HasMaxLength(100)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHR5");
            entity.Property(e => e.DataVarchr6)
                .HasMaxLength(100)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHR6");
            entity.Property(e => e.DataVarchr7)
                .HasMaxLength(100)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHR7");
            entity.Property(e => e.DataVarchr8)
                .HasMaxLength(100)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHR8");
            entity.Property(e => e.DataVarchr9)
                .HasMaxLength(100)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHR9");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.NotnullFlag)
                .HasComment("Not null flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("NOTNULL_FLAG");
            entity.Property(e => e.ParamName)
                .HasMaxLength(128)
                .HasComment("Parameter name")
                .HasColumnName("PARAM_NAME");
            entity.Property(e => e.ReturnFlag)
                .HasComment("Return flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("RETURN_FLAG");
            entity.Property(e => e.ReturnNormalValue)
                .HasMaxLength(100)
                .HasComment("Return normal value")
                .HasColumnName("RETURN_NORMAL_VALUE");
        });

        modelBuilder.Entity<IeQueryDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq });

            entity.ToTable("IE_QUERY_DEFINE", tb => tb.HasComment("EVOLIO Query Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.DataVarchr1)
                .HasMaxLength(100)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHR1");
            entity.Property(e => e.DataVarchr10)
                .HasMaxLength(100)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHR10");
            entity.Property(e => e.DataVarchr2)
                .HasMaxLength(100)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHR2");
            entity.Property(e => e.DataVarchr3)
                .HasMaxLength(100)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHR3");
            entity.Property(e => e.DataVarchr4)
                .HasMaxLength(100)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHR4");
            entity.Property(e => e.DataVarchr5)
                .HasMaxLength(100)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHR5");
            entity.Property(e => e.DataVarchr6)
                .HasMaxLength(100)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHR6");
            entity.Property(e => e.DataVarchr7)
                .HasMaxLength(100)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHR7");
            entity.Property(e => e.DataVarchr8)
                .HasMaxLength(100)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHR8");
            entity.Property(e => e.DataVarchr9)
                .HasMaxLength(100)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHR9");
            entity.Property(e => e.DeleteFlag)
                .HasComment("Delete flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("DELETE_FLAG");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.ErrstopFlag)
                .HasComment("Error stop flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("ERRSTOP_FLAG");
            entity.Property(e => e.ErrstopMsg)
                .HasMaxLength(2000)
                .HasComment("Error message")
                .HasColumnName("ERRSTOP_MSG");
            entity.Property(e => e.ExecTiming)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Execute timing")
                .HasColumnName("EXEC_TIMING");
            entity.Property(e => e.ExecutableFlag)
                .HasComment("Reference executable flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("EXECUTABLE_FLAG");
            entity.Property(e => e.InsertFlag)
                .HasComment("Insert flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("INSERT_FLAG");
            entity.Property(e => e.JobId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Job ID")
                .HasColumnName("JOB_ID");
            entity.Property(e => e.LibListFlag)
                .HasComment("Library list flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("LIB_LIST_FLAG");
            entity.Property(e => e.QueryName)
                .HasMaxLength(128)
                .HasComment("Query name")
                .HasColumnName("QUERY_NAME");
            entity.Property(e => e.QueryType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Query type")
                .HasColumnName("QUERY_TYPE");
            entity.Property(e => e.ResultOccurs)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Result occurs flag")
                .HasColumnName("RESULT_OCCURS");
            entity.Property(e => e.SqlText)
                .HasComment("SQL sentence")
                .HasColumnName("SQL_TEXT");
            entity.Property(e => e.TargetLib)
                .HasMaxLength(128)
                .HasComment("Update target library")
                .HasColumnName("TARGET_LIB");
            entity.Property(e => e.TargetTable)
                .HasMaxLength(128)
                .HasComment("Update target table")
                .HasColumnName("TARGET_TABLE");
            entity.Property(e => e.UpdateFlag)
                .HasComment("Update flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("UPDATE_FLAG");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Update type")
                .HasColumnName("UPDATE_TYPE");
        });

        modelBuilder.Entity<IeQueryLibrary>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.Library, e.Role });

            entity.ToTable("IE_QUERY_LIBRARY", tb => tb.HasComment("EVOLIO Query Library Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.Library)
                .HasMaxLength(128)
                .HasComment("Library name")
                .HasColumnName("LIBRARY");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasComment("Role")
                .HasColumnName("ROLE");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.ExecOrder)
                .HasComment("Execute order")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("EXEC_ORDER");
        });

        modelBuilder.Entity<IeRole>(entity =>
        {
            entity.HasKey(e => e.Role);

            entity.ToTable("IE_ROLE", tb => tb.HasComment("EVOLIO Role"));

            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasComment("Role")
                .HasColumnName("ROLE");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.DelDisableFlag)
                .HasComment("Delete-disable flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("DEL_DISABLE_FLAG");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.RoleDesc1)
                .HasMaxLength(400)
                .HasComment("Role description 1")
                .HasColumnName("ROLE_DESC1");
            entity.Property(e => e.RoleDesc2)
                .HasMaxLength(400)
                .HasComment("Role description 2")
                .HasColumnName("ROLE_DESC2");
            entity.Property(e => e.RoleDesc3)
                .HasMaxLength(400)
                .HasComment("Role description 3")
                .HasColumnName("ROLE_DESC3");
        });

        modelBuilder.Entity<IeSchemaAuth>(entity =>
        {
            entity.HasKey(e => new { e.SchemaId, e.AuthType, e.UserId, e.Role });

            entity.ToTable("IE_SCHEMA_AUTH", tb => tb.HasComment("EVOLIO Schema Authority"));

            entity.Property(e => e.SchemaId)
                .HasMaxLength(128)
                .HasComment("Schema ID")
                .HasColumnName("SCHEMA_ID");
            entity.Property(e => e.AuthType)
                .HasComment("Specified flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("AUTH_TYPE");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasComment("User ID")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasComment("Role")
                .HasColumnName("ROLE");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.ExecutableType)
                .HasComment("Executable flag")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXECUTABLE_TYPE");
        });

        modelBuilder.Entity<IeSelectDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.RowNo });

            entity.ToTable("IE_SELECT_DEFINE", tb => tb.HasComment("EVOLIO SelectConditions Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.RowNo)
                .HasComment("Line number")
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("ROW_NO");
            entity.Property(e => e.CastValue)
                .HasMaxLength(100)
                .HasComment("Cast value")
                .HasColumnName("CAST_VALUE");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.ColumnAliasId)
                .HasMaxLength(128)
                .HasComment("Column Alias ID")
                .HasColumnName("COLUMN_ALIAS_ID");
            entity.Property(e => e.ColumnId)
                .HasMaxLength(128)
                .HasComment("Column ID")
                .HasColumnName("COLUMN_ID");
            entity.Property(e => e.ControlType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasComment("Control type")
                .HasColumnName("CONTROL_TYPE");
            entity.Property(e => e.CtlInvalidFlag)
                .HasComment("Ctl invalid permission")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("CTL_INVALID_FLAG");
            entity.Property(e => e.CtlListFlag)
                .HasComment("Ctl list type")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("CTL_LIST_FLAG");
            entity.Property(e => e.CtlSelqryDb)
                .HasMaxLength(128)
                .HasComment("Ctl DB Value")
                .HasColumnName("CTL_SELQRY_DB");
            entity.Property(e => e.CtlSelqryDisp)
                .HasMaxLength(128)
                .HasComment("Ctl Display Value")
                .HasColumnName("CTL_SELQRY_DISP");
            entity.Property(e => e.CtlSelqryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Ctl SelectQuery ID")
                .HasColumnName("CTL_SELQRY_ID");
            entity.Property(e => e.CtlValueType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Ctl Value type flag")
                .HasColumnName("CTL_VALUE_TYPE");
            entity.Property(e => e.DataVarchr1)
                .HasMaxLength(100)
                .HasComment("Reserve item 1")
                .HasColumnName("DATA_VARCHR1");
            entity.Property(e => e.DataVarchr10)
                .HasMaxLength(100)
                .HasComment("Reserve item 10")
                .HasColumnName("DATA_VARCHR10");
            entity.Property(e => e.DataVarchr2)
                .HasMaxLength(100)
                .HasComment("Reserve item 2")
                .HasColumnName("DATA_VARCHR2");
            entity.Property(e => e.DataVarchr3)
                .HasMaxLength(100)
                .HasComment("Reserve item 3")
                .HasColumnName("DATA_VARCHR3");
            entity.Property(e => e.DataVarchr4)
                .HasMaxLength(100)
                .HasComment("Reserve item 4")
                .HasColumnName("DATA_VARCHR4");
            entity.Property(e => e.DataVarchr5)
                .HasMaxLength(100)
                .HasComment("Reserve item 5")
                .HasColumnName("DATA_VARCHR5");
            entity.Property(e => e.DataVarchr6)
                .HasMaxLength(100)
                .HasComment("Reserve item 6")
                .HasColumnName("DATA_VARCHR6");
            entity.Property(e => e.DataVarchr7)
                .HasMaxLength(100)
                .HasComment("Reserve item 7")
                .HasColumnName("DATA_VARCHR7");
            entity.Property(e => e.DataVarchr8)
                .HasMaxLength(100)
                .HasComment("Reserve item 8")
                .HasColumnName("DATA_VARCHR8");
            entity.Property(e => e.DataVarchr9)
                .HasMaxLength(100)
                .HasComment("Reserve item 9")
                .HasColumnName("DATA_VARCHR9");
            entity.Property(e => e.DeleteKeyFlag)
                .HasComment("Delete key flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("DELETE_KEY_FLAG");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(128)
                .HasComment("Display name")
                .HasColumnName("DISPLAY_NAME");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.Grouping)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("Grouping")
                .HasColumnName("GROUPING");
            entity.Property(e => e.InsertFlag)
                .HasComment("Insert flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("INSERT_FLAG");
            entity.Property(e => e.LtrimFlag)
                .HasComment("Ltrim flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("LTRIM_FLAG");
            entity.Property(e => e.NotnullFlag)
                .HasComment("Not null flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("NOTNULL_FLAG");
            entity.Property(e => e.QueryOr1)
                .HasMaxLength(2000)
                .HasComment("Or1 statement")
                .HasColumnName("QUERY_OR1");
            entity.Property(e => e.QueryOr2)
                .HasMaxLength(2000)
                .HasComment("Or2 statement")
                .HasColumnName("QUERY_OR2");
            entity.Property(e => e.QueryOr3)
                .HasMaxLength(2000)
                .HasComment("Or3 statement")
                .HasColumnName("QUERY_OR3");
            entity.Property(e => e.QueryOr4)
                .HasMaxLength(2000)
                .HasComment("Or4 statement")
                .HasColumnName("QUERY_OR4");
            entity.Property(e => e.QueryWhere)
                .HasMaxLength(2000)
                .HasComment("Where statement")
                .HasColumnName("QUERY_WHERE");
            entity.Property(e => e.RtrimFlag)
                .HasComment("Rtrim flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("RTRIM_FLAG");
            entity.Property(e => e.SelectFlag)
                .HasComment("Display select flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("SELECT_FLAG");
            entity.Property(e => e.SelectText)
                .HasMaxLength(2000)
                .HasComment("Select statement")
                .HasColumnName("SELECT_TEXT");
            entity.Property(e => e.SortOrder)
                .HasComment("Sort order")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("SORT_ORDER");
            entity.Property(e => e.SortType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("Sort type")
                .HasColumnName("SORT_TYPE");
            entity.Property(e => e.TableAliasId)
                .HasMaxLength(128)
                .HasComment("Table Alias ID")
                .HasColumnName("TABLE_ALIAS_ID");
            entity.Property(e => e.UpdateFlag)
                .HasComment("Update flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("UPDATE_FLAG");
            entity.Property(e => e.UpdateKeyFlag)
                .HasComment("Update key flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("UPDATE_KEY_FLAG");
        });

        modelBuilder.Entity<IeSelectQuery>(entity =>
        {
            entity.HasKey(e => e.QueryId);

            entity.ToTable("IE_SELECT_QUERY", tb => tb.HasComment("EVOLIO SelectQuery Define"));

            entity.HasIndex(e => new { e.CategoryId, e.QueryId }, "IDX_IE_SELECT_QUERY_01");

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.AliasXml)
                .HasMaxLength(2000)
                .HasComment("Alias Xml")
                .HasColumnName("ALIAS_XML");
            entity.Property(e => e.CategoryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Catetory ID")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.ChangeHist)
                .HasComment("Change History")
                .HasColumnName("CHANGE_HIST");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.QueryName)
                .HasMaxLength(128)
                .HasComment("SelectQuery name")
                .HasColumnName("QUERY_NAME");
            entity.Property(e => e.Remarks)
                .HasMaxLength(2000)
                .HasComment("Remarks")
                .HasColumnName("REMARKS");
            entity.Property(e => e.SelectText)
                .HasComment("Select statement")
                .HasColumnName("SELECT_TEXT");
            entity.Property(e => e.SelectXml)
                .HasMaxLength(2000)
                .HasComment("Select Xml")
                .HasColumnName("SELECT_XML");
            entity.Property(e => e.Summary)
                .HasMaxLength(2000)
                .HasComment("Summary")
                .HasColumnName("SUMMARY");
        });

        modelBuilder.Entity<IeTableDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.AliasId });

            entity.ToTable("IE_TABLE_DEFINE", tb => tb.HasComment("EVOLIO QueryTables Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.AliasId)
                .HasMaxLength(128)
                .HasComment("Alias ID")
                .HasColumnName("ALIAS_ID");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.LibId)
                .HasMaxLength(128)
                .HasComment("Library ID")
                .HasColumnName("LIB_ID");
            entity.Property(e => e.LibListFlag)
                .HasComment("Library List flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("LIB_LIST_FLAG");
            entity.Property(e => e.PointX)
                .HasComment("Point X")
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("POINT_X");
            entity.Property(e => e.PointY)
                .HasComment("Point Y")
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("POINT_Y");
            entity.Property(e => e.TableId)
                .HasMaxLength(128)
                .HasComment("Table ID")
                .HasColumnName("TABLE_ID");
        });

        modelBuilder.Entity<IeTerminal>(entity =>
        {
            entity.HasKey(e => e.TerminalId);

            entity.ToTable("IE_TERMINAL", tb => tb.HasComment("ｉＥ端末管理"));

            entity.Property(e => e.TerminalId)
                .HasMaxLength(100)
                .HasComment("端末ＩＤ")
                .HasColumnName("TERMINAL_ID");
            entity.Property(e => e.Biko)
                .HasMaxLength(500)
                .HasComment("備考")
                .HasColumnName("BIKO");
            entity.Property(e => e.ChgDate)
                .HasComment("更新日時")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("登録日時")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.TerminalName)
                .HasMaxLength(500)
                .HasComment("端末名")
                .HasColumnName("TERMINAL_NAME");
        });

        modelBuilder.Entity<IeUpdateXml>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.RowNo });

            entity.ToTable("IE_UPDATE_XML", tb => tb.HasComment("EVOLIO Update Xml"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.RowNo)
                .HasComment("Line number")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("ROW_NO");
            entity.Property(e => e.UpdateXml)
                .HasComment("Update Xml")
                .HasColumnName("UPDATE_XML");
        });

        modelBuilder.Entity<IeUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("IE_USER", tb => tb.HasComment("EVOLIO User"));

            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasComment("User ID")
                .HasColumnName("USER_ID");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.CommandHideFlag)
                .HasComment("Hide command flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("COMMAND_HIDE_FLAG");
            entity.Property(e => e.DefaultCategory)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("Default category")
                .HasColumnName("DEFAULT_CATEGORY");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.LastLoginDate)
                .HasComment("Last login date")
                .HasColumnType("datetime")
                .HasColumnName("LAST_LOGIN_DATE");
            entity.Property(e => e.LogicalDelFlag)
                .HasComment("Logical deleted flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("LOGICAL_DEL_FLAG");
            entity.Property(e => e.Password)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasComment("Password")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.PasswordMissCount)
                .HasComment("Password input mistake count")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("PASSWORD_MISS_COUNT");
            entity.Property(e => e.PasswordMissDate)
                .HasComment("Password input mistake date")
                .HasColumnType("datetime")
                .HasColumnName("PASSWORD_MISS_DATE");
            entity.Property(e => e.PasswordUpdateDate)
                .HasComment("Password update date")
                .HasColumnType("datetime")
                .HasColumnName("PASSWORD_UPDATE_DATE");
            entity.Property(e => e.PastPassword)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasComment("Last time password")
                .HasColumnName("PAST_PASSWORD");
            entity.Property(e => e.PastPassword1)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasComment("Last time password 1")
                .HasColumnName("PAST_PASSWORD1");
            entity.Property(e => e.PastPassword2)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasComment("Last time password 2")
                .HasColumnName("PAST_PASSWORD2");
            entity.Property(e => e.UserFamilyName)
                .HasMaxLength(40)
                .HasComment("Last name")
                .HasColumnName("USER_FAMILY_NAME");
            entity.Property(e => e.UserFirstName)
                .HasMaxLength(40)
                .HasComment("First name")
                .HasColumnName("USER_FIRST_NAME");
            entity.Property(e => e.UserLockFlag)
                .HasComment("User lock flag")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("USER_LOCK_FLAG");
        });

        modelBuilder.Entity<IeUserMapping>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IE_USER_MAPPING", tb => tb.HasComment("EVOLIO User Mapping"));

            entity.Property(e => e.DefaultCategory)
                .HasDefaultValue(0m)
                .HasComment("Default category")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("DEFAULT_CATEGORY");
            entity.Property(e => e.Password)
                .HasDefaultValue(0m)
                .HasComment("Password")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.PastPassword)
                .HasDefaultValue(0m)
                .HasComment("Last time password")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("PAST_PASSWORD");
            entity.Property(e => e.PastPassword1)
                .HasDefaultValue(0m)
                .HasComment("Last time password 1")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("PAST_PASSWORD1");
            entity.Property(e => e.PastPassword2)
                .HasDefaultValue(0m)
                .HasComment("Last time password 2")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("PAST_PASSWORD2");
            entity.Property(e => e.UserFamilyName)
                .HasDefaultValue(0m)
                .HasComment("Last name")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("USER_FAMILY_NAME");
            entity.Property(e => e.UserFirstName)
                .HasDefaultValue(0m)
                .HasComment("First name")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("USER_FIRST_NAME");
            entity.Property(e => e.UserId)
                .HasDefaultValue(0m)
                .HasComment("User ID")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<IeUserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Role });

            entity.ToTable("IE_USER_ROLE", tb => tb.HasComment("EVOLIO UserRole"));

            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasComment("User ID")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasComment("Role")
                .HasColumnName("ROLE");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
        });

        modelBuilder.Entity<IeValueDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.SeqNo, e.ValueSeq });

            entity.ToTable("IE_VALUE_DEFINE", tb => tb.HasComment("EVOLIO Value Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.SeqNo)
                .HasComment("Line number")
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("SEQ_NO");
            entity.Property(e => e.ValueSeq)
                .HasComment("Value SEQ")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("VALUE_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.DbValue)
                .HasMaxLength(2000)
                .HasComment("DB value")
                .HasColumnName("DB_VALUE");
            entity.Property(e => e.DispValue)
                .HasMaxLength(2000)
                .HasComment("Display value")
                .HasColumnName("DISP_VALUE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
        });

        modelBuilder.Entity<IeValueQryDefine>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.SeqNo, e.ParamSeq });

            entity.ToTable("IE_VALUE_QRY_DEFINE", tb => tb.HasComment("EVOLIO ValueQuery Define"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.SeqNo)
                .HasComment("Line number")
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("SEQ_NO");
            entity.Property(e => e.ParamSeq)
                .HasComment("Param SEQ")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("PARAM_SEQ");
            entity.Property(e => e.ChgDate)
                .HasComment("Change Date")
                .HasColumnType("datetime")
                .HasColumnName("CHG_DATE");
            entity.Property(e => e.EntryDate)
                .HasComment("Entry Date")
                .HasColumnType("datetime")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.ParamName)
                .HasMaxLength(2000)
                .HasComment("Param name")
                .HasColumnName("PARAM_NAME");
            entity.Property(e => e.ParamValue)
                .HasMaxLength(2000)
                .HasComment("Param value")
                .HasColumnName("PARAM_VALUE");
        });

        modelBuilder.Entity<IeValueXml>(entity =>
        {
            entity.HasKey(e => new { e.QueryId, e.ExecSeq, e.RowNo });

            entity.ToTable("IE_VALUE_XML", tb => tb.HasComment("EVOLIO Value Xml"));

            entity.Property(e => e.QueryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Query ID")
                .HasColumnName("QUERY_ID");
            entity.Property(e => e.ExecSeq)
                .HasComment("Execute SEQ")
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("EXEC_SEQ");
            entity.Property(e => e.RowNo)
                .HasComment("Line number")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("ROW_NO");
            entity.Property(e => e.ValueXml)
                .HasComment("Value Xml")
                .HasColumnName("VALUE_XML");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
