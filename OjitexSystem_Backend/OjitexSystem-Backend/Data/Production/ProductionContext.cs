using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OjitexSystem_Backend.Data.Production;

public partial class ProductionContext : DbContext
{
    public ProductionContext(DbContextOptions<ProductionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TAcceptanceC> TAcceptanceCs { get; set; }

    public virtual DbSet<TAcceptanceDetailC> TAcceptanceDetailCs { get; set; }

    public virtual DbSet<TAcceptanceDownload> TAcceptanceDownloads { get; set; }

    public virtual DbSet<TAcceptanceDownloadHistory> TAcceptanceDownloadHistories { get; set; }

    public virtual DbSet<TAcceptanceDownloadLogi> TAcceptanceDownloadLogis { get; set; }

    public virtual DbSet<TAdjustProduct> TAdjustProducts { get; set; }

    public virtual DbSet<TBkProductDescription> TBkProductDescriptions { get; set; }

    public virtual DbSet<TBkPurchaseOrder> TBkPurchaseOrders { get; set; }

    public virtual DbSet<TConvertingOnlineReport> TConvertingOnlineReports { get; set; }

    public virtual DbSet<TConvertingResult> TConvertingResults { get; set; }

    public virtual DbSet<TCorrugatorOnlineReport> TCorrugatorOnlineReports { get; set; }

    public virtual DbSet<TCsCustomerPic> TCsCustomerPics { get; set; }

    public virtual DbSet<TCsPlanAcceptance> TCsPlanAcceptances { get; set; }

    public virtual DbSet<TCsSalesVolume> TCsSalesVolumes { get; set; }

    public virtual DbSet<TCsdisposalList> TCsdisposalLists { get; set; }

    public virtual DbSet<TCurrentStock> TCurrentStocks { get; set; }

    public virtual DbSet<TCurrentStockHistory> TCurrentStockHistories { get; set; }

    public virtual DbSet<TCustomer> TCustomers { get; set; }

    public virtual DbSet<TCustomer2Ojitex> TCustomer2Ojitexes { get; set; }

    public virtual DbSet<TCustomerAddress> TCustomerAddresses { get; set; }

    public virtual DbSet<TDestination> TDestinations { get; set; }

    public virtual DbSet<TDisposalProduct> TDisposalProducts { get; set; }

    public virtual DbSet<TGenPaperMaster> TGenPaperMasters { get; set; }

    public virtual DbSet<TInputCustomer> TInputCustomers { get; set; }

    public virtual DbSet<TInputProduct> TInputProducts { get; set; }

    public virtual DbSet<TInputSchedule> TInputSchedules { get; set; }

    public virtual DbSet<TInputScheduleHistory> TInputScheduleHistories { get; set; }

    public virtual DbSet<TInterMemo> TInterMemos { get; set; }

    public virtual DbSet<TKamPaperMaster> TKamPaperMasters { get; set; }

    public virtual DbSet<TLastStock> TLastStocks { get; set; }

    public virtual DbSet<TLastStockHistory> TLastStockHistories { get; set; }

    public virtual DbSet<TLogiDeliveryNote> TLogiDeliveryNotes { get; set; }

    public virtual DbSet<TLogiDeliveryNoteDetail> TLogiDeliveryNoteDetails { get; set; }

    public virtual DbSet<TLogiDeliveryNoteDetailFake> TLogiDeliveryNoteDetailFakes { get; set; }

    public virtual DbSet<TLogiDeliveryNoteFake> TLogiDeliveryNoteFakes { get; set; }

    public virtual DbSet<TLogiDeliveryPlan> TLogiDeliveryPlans { get; set; }

    public virtual DbSet<TLogiGoodsReceipt> TLogiGoodsReceipts { get; set; }

    public virtual DbSet<TLogiPaperRollDeliveryNote> TLogiPaperRollDeliveryNotes { get; set; }

    public virtual DbSet<TLogiPaperRollDeliveryNoteDetail> TLogiPaperRollDeliveryNoteDetails { get; set; }

    public virtual DbSet<TMachineSpec> TMachineSpecs { get; set; }

    public virtual DbSet<TMachineSpecification> TMachineSpecifications { get; set; }

    public virtual DbSet<TMatExportPaperRoll> TMatExportPaperRolls { get; set; }

    public virtual DbSet<TMatMakerMaster> TMatMakerMasters { get; set; }

    public virtual DbSet<TMatPaperRollComming> TMatPaperRollCommings { get; set; }

    public virtual DbSet<TMatPurchasePaperRoll> TMatPurchasePaperRolls { get; set; }

    public virtual DbSet<TMatSalesForecast> TMatSalesForecasts { get; set; }

    public virtual DbSet<TMatSupplierMaster> TMatSupplierMasters { get; set; }

    public virtual DbSet<TMonthlyStock> TMonthlyStocks { get; set; }

    public virtual DbSet<TMonthlyStockHistory> TMonthlyStockHistories { get; set; }

    public virtual DbSet<TNextStock> TNextStocks { get; set; }

    public virtual DbSet<TNextStockHistory> TNextStockHistories { get; set; }

    public virtual DbSet<TOjitexHoliday> TOjitexHolidays { get; set; }

    public virtual DbSet<TOpeningStock> TOpeningStocks { get; set; }

    public virtual DbSet<TOtherProduct> TOtherProducts { get; set; }

    public virtual DbSet<TPaperRollClosingStock> TPaperRollClosingStocks { get; set; }

    public virtual DbSet<TPaperRollExport> TPaperRollExports { get; set; }

    public virtual DbSet<TPaperRollImport> TPaperRollImports { get; set; }

    public virtual DbSet<TPaperRollOpeningStock> TPaperRollOpeningStocks { get; set; }

    public virtual DbSet<TPaperRollUsed> TPaperRollUseds { get; set; }

    public virtual DbSet<TPlanOutputConverting> TPlanOutputConvertings { get; set; }

    public virtual DbSet<TPlanOutputCorrugator> TPlanOutputCorrugators { get; set; }

    public virtual DbSet<TPoReceivingSituation> TPoReceivingSituations { get; set; }

    public virtual DbSet<TProconvertingInput> TProconvertingInputs { get; set; }

    public virtual DbSet<TProcorrugatorInput> TProcorrugatorInputs { get; set; }

    public virtual DbSet<TQcRepairProduct> TQcRepairProducts { get; set; }

    public virtual DbSet<TSpecialCustomerPoManage> TSpecialCustomerPoManages { get; set; }

    public virtual DbSet<TSubMaterialExport> TSubMaterialExports { get; set; }

    public virtual DbSet<TSubMaterialExportDetail> TSubMaterialExportDetails { get; set; }

    public virtual DbSet<TSubMaterialMaster> TSubMaterialMasters { get; set; }

    public virtual DbSet<TSubconReceive> TSubconReceives { get; set; }

    public virtual DbSet<TSubconReport> TSubconReports { get; set; }

    public virtual DbSet<TSupplier> TSuppliers { get; set; }

    public virtual DbSet<TSystemState> TSystemStates { get; set; }

    public virtual DbSet<TTransportCompany> TTransportCompanies { get; set; }

    public virtual DbSet<TVolumeEstimate> TVolumeEstimates { get; set; }

    public virtual DbSet<TWarehouseNg> TWarehouseNgs { get; set; }

    public virtual DbSet<TWarehousePaperRoll> TWarehousePaperRolls { get; set; }

    public virtual DbSet<VtImportConvertingFromPlan> VtImportConvertingFromPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAcceptanceC>(entity =>
        {
            entity.HasKey(e => e.ActId).HasName("PK__T_Accept__EBC830955BA6C130");

            entity.ToTable("T_Acceptance_CS", tb =>
                {
                    tb.HasTrigger("AcceptanceAfterDeleteTrigger");
                    tb.HasTrigger("AcceptanceUpdateTrigger");
                });

            entity.Property(e => e.ActId).HasColumnName("act_id");
            entity.Property(e => e.ActCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("act_cus_cd");
            entity.Property(e => e.ActCusName)
                .HasMaxLength(50)
                .HasColumnName("act_cus_name");
            entity.Property(e => e.ActDateChange)
                .HasColumnType("datetime")
                .HasColumnName("act_date_change");
            entity.Property(e => e.ActDateInput)
                .HasColumnType("datetime")
                .HasColumnName("act_date_input");
            entity.Property(e => e.ActHeaderId)
                .HasMaxLength(19)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("act_header_id");
            entity.Property(e => e.ActNo)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("act_no");
            entity.Property(e => e.ActNoteAll)
                .HasMaxLength(200)
                .HasColumnName("act_note_all");
            entity.Property(e => e.ActSumOfOrder)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("act_sum_of_order");
            entity.Property(e => e.ActSumOfProduct)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("act_sum_of_product");
            entity.Property(e => e.ActSumOfStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("act_sum_of_stock");
            entity.Property(e => e.ActUserId)
                .HasMaxLength(20)
                .HasColumnName("act_user_id");
        });

        modelBuilder.Entity<TAcceptanceDetailC>(entity =>
        {
            entity.HasKey(e => e.AcdId).HasName("PK__T_Accept__60B939B9E94539E8");

            entity.ToTable("T_Acceptance_Detail_CS", tb =>
                {
                    tb.HasTrigger("AcceptanceDetailAfterDeleteTrigger");
                    tb.HasTrigger("AcceptanceDetailUpdateTrigger");
                    tb.HasTrigger("AcceptanceDetail_Update_ActNo_ReceiveNo");
                });

            entity.Property(e => e.AcdId).HasColumnName("acd_id");
            entity.Property(e => e.AcdActNo)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("acd_act_no");
            entity.Property(e => e.AcdClass).HasColumnName("acd_class");
            entity.Property(e => e.AcdDelivery).HasColumnName("acd_delivery");
            entity.Property(e => e.AcdHeaderId)
                .HasMaxLength(19)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("acd_header_id");
            entity.Property(e => e.AcdLineNo).HasColumnName("acd_line_no");
            entity.Property(e => e.AcdNoteDetail)
                .HasMaxLength(200)
                .HasColumnName("acd_note_detail");
            entity.Property(e => e.AcdOrderQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("acd_order_qty");
            entity.Property(e => e.AcdPo)
                .HasMaxLength(200)
                .HasColumnName("acd_po");
            entity.Property(e => e.AcdProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("acd_pro_cd");
            entity.Property(e => e.AcdProName)
                .HasMaxLength(200)
                .HasColumnName("acd_pro_name");
            entity.Property(e => e.AcdProduct)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("acd_product");
            entity.Property(e => e.AcdReceiveNo).HasColumnName("acd_receive_no");
            entity.Property(e => e.AcdRemark)
                .HasMaxLength(300)
                .HasColumnName("acd_remark");
            entity.Property(e => e.AcdState)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("acd_state");
            entity.Property(e => e.AcdStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("acd_stock");
        });

        modelBuilder.Entity<TAcceptanceDownload>(entity =>
        {
            entity.HasKey(e => e.AdId).HasName("PK__T_Accept__CAA4A62710F38C3F");

            entity.ToTable("T_Acceptance_Download", tb => tb.HasTrigger("AcceptanceDownloadAfterDeleteTrigger"));

            entity.Property(e => e.AdId).HasColumnName("ad_id");
            entity.Property(e => e.AdActNo)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ad_act_no");
            entity.Property(e => e.AdClass).HasColumnName("ad_class");
            entity.Property(e => e.AdCsUserId)
                .HasMaxLength(20)
                .HasColumnName("ad_cs_user_id");
            entity.Property(e => e.AdCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("ad_cus_cd");
            entity.Property(e => e.AdDateInput)
                .HasColumnType("datetime")
                .HasColumnName("ad_date_input");
            entity.Property(e => e.AdDelivery).HasColumnName("ad_delivery");
            entity.Property(e => e.AdNoteDetail)
                .HasMaxLength(200)
                .HasColumnName("ad_note_detail");
            entity.Property(e => e.AdOrderQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("ad_order_qty");
            entity.Property(e => e.AdPo)
                .HasMaxLength(200)
                .HasColumnName("ad_po");
            entity.Property(e => e.AdProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("ad_pro_cd");
            entity.Property(e => e.AdProduct)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("ad_product");
            entity.Property(e => e.AdReceiveNo).HasColumnName("ad_receive_no");
            entity.Property(e => e.AdRemark)
                .HasMaxLength(300)
                .HasColumnName("ad_remark");
            entity.Property(e => e.AdStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("ad_stock");
            entity.Property(e => e.AdType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ad_type");
            entity.Property(e => e.AdUserId)
                .HasMaxLength(20)
                .HasColumnName("ad_user_id");
        });

        modelBuilder.Entity<TAcceptanceDownloadHistory>(entity =>
        {
            entity.HasKey(e => e.AdhId).HasName("PK__T_Accept__B84332F5C9E99214");

            entity.ToTable("T_Acceptance_Download_History");

            entity.Property(e => e.AdhId).HasColumnName("adh_id");
            entity.Property(e => e.AdhActNo)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("adh_act_no");
            entity.Property(e => e.AdhClass).HasColumnName("adh_class");
            entity.Property(e => e.AdhCsUserId)
                .HasMaxLength(20)
                .HasColumnName("adh_cs_user_id");
            entity.Property(e => e.AdhCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("adh_cus_cd");
            entity.Property(e => e.AdhDateInput)
                .HasColumnType("datetime")
                .HasColumnName("adh_date_input");
            entity.Property(e => e.AdhDelivery).HasColumnName("adh_delivery");
            entity.Property(e => e.AdhDlTime)
                .HasColumnType("datetime")
                .HasColumnName("adh_dl_time");
            entity.Property(e => e.AdhDlUser)
                .HasMaxLength(20)
                .HasColumnName("adh_dl_user");
            entity.Property(e => e.AdhNoteDetail)
                .HasMaxLength(200)
                .HasColumnName("adh_note_detail");
            entity.Property(e => e.AdhOrderQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("adh_order_qty");
            entity.Property(e => e.AdhPo)
                .HasMaxLength(200)
                .HasColumnName("adh_po");
            entity.Property(e => e.AdhProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("adh_pro_cd");
            entity.Property(e => e.AdhProduct)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("adh_product");
            entity.Property(e => e.AdhReceiveNo).HasColumnName("adh_receive_no");
            entity.Property(e => e.AdhRemark)
                .HasMaxLength(300)
                .HasColumnName("adh_remark");
            entity.Property(e => e.AdhStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("adh_stock");
            entity.Property(e => e.AdhType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("adh_type");
        });

        modelBuilder.Entity<TAcceptanceDownloadLogi>(entity =>
        {
            entity.HasKey(e => e.AdlId).HasName("PK__T_Accept__546196FB8A93291C");

            entity.ToTable("T_Acceptance_Download_Logi");

            entity.Property(e => e.AdlId).HasColumnName("adl_id");
            entity.Property(e => e.AdlActNo)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("adl_act_no");
            entity.Property(e => e.AdlClass).HasColumnName("adl_class");
            entity.Property(e => e.AdlCsUserId)
                .HasMaxLength(20)
                .HasColumnName("adl_cs_user_id");
            entity.Property(e => e.AdlCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("adl_cus_cd");
            entity.Property(e => e.AdlDateInput)
                .HasColumnType("datetime")
                .HasColumnName("adl_date_input");
            entity.Property(e => e.AdlDelivery).HasColumnName("adl_delivery");
            entity.Property(e => e.AdlNoteAll)
                .HasMaxLength(200)
                .HasColumnName("adl_note_all");
            entity.Property(e => e.AdlNoteDetail)
                .HasMaxLength(200)
                .HasColumnName("adl_note_detail");
            entity.Property(e => e.AdlOrderQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("adl_order_qty");
            entity.Property(e => e.AdlPo)
                .HasMaxLength(200)
                .HasColumnName("adl_po");
            entity.Property(e => e.AdlProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("adl_pro_cd");
            entity.Property(e => e.AdlProduct)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("adl_product");
            entity.Property(e => e.AdlReceiveNo).HasColumnName("adl_receive_no");
            entity.Property(e => e.AdlRemark)
                .HasMaxLength(300)
                .HasColumnName("adl_remark");
            entity.Property(e => e.AdlStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("adl_stock");
            entity.Property(e => e.AdlType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("adl_type");
            entity.Property(e => e.AdlUserId)
                .HasMaxLength(20)
                .HasColumnName("adl_user_id");
        });

        modelBuilder.Entity<TAdjustProduct>(entity =>
        {
            entity.HasKey(e => e.ApId).HasName("PK__T_Adjust__C4000E9D2250067B");

            entity.ToTable("T_AdjustProduct");

            entity.Property(e => e.ApId).HasColumnName("ap_id");
            entity.Property(e => e.ApDate).HasColumnName("ap_date");
            entity.Property(e => e.ApDateInput)
                .HasColumnType("datetime")
                .HasColumnName("ap_date_input");
            entity.Property(e => e.ApProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("ap_pro_cd");
            entity.Property(e => e.ApQuantity)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("ap_quantity");
            entity.Property(e => e.ApReceiveNo).HasColumnName("ap_receive_no");
            entity.Property(e => e.ApRemark)
                .HasMaxLength(200)
                .HasColumnName("ap_remark");
            entity.Property(e => e.ApSqm)
                .HasColumnType("decimal(7, 3)")
                .HasColumnName("ap_sqm");
            entity.Property(e => e.ApTotalSqm)
                .HasColumnType("decimal(7, 3)")
                .HasColumnName("ap_totalSQM");
            entity.Property(e => e.ApUserId)
                .HasMaxLength(20)
                .HasColumnName("ap_user_id");
        });

        modelBuilder.Entity<TBkProductDescription>(entity =>
        {
            entity.HasKey(e => e.PdProCd);

            entity.ToTable("T_BK_ProductDescription", tb => tb.HasComment("T_BK_ProductDescription"));

            entity.Property(e => e.PdProCd)
                .HasComment("pd_pro_cd")
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pd_pro_cd");
            entity.Property(e => e.PdDateInput)
                .HasComment("pd_date_input")
                .HasColumnType("datetime")
                .HasColumnName("pd_date_input");
            entity.Property(e => e.PdProDescription)
                .HasMaxLength(200)
                .HasComment("pd_pro_description")
                .HasColumnName("pd_pro_description");
            entity.Property(e => e.PdProName)
                .HasMaxLength(150)
                .HasComment("pd_pro_name")
                .HasColumnName("pd_pro_name");
            entity.Property(e => e.PdProNote)
                .HasMaxLength(300)
                .HasComment("pd_pro_note")
                .HasColumnName("pd_pro_note");
            entity.Property(e => e.PdUserId)
                .HasMaxLength(20)
                .HasComment("pd_user_id")
                .HasColumnName("pd_user_id");
        });

        modelBuilder.Entity<TBkPurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PoId).HasName("PK__T_BK_Pur__368DA7F0B4A32861");

            entity.ToTable("T_BK_PurchaseOrder");

            entity.Property(e => e.PoId).HasColumnName("po_id");
            entity.Property(e => e.PoAmount)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("po_amount");
            entity.Property(e => e.PoBackorderQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("po_backorder_qty");
            entity.Property(e => e.PoBrandName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_brand_name");
            entity.Property(e => e.PoBuyerCustomerAssignAccountId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_buyer_customer_assign_account_id");
            entity.Property(e => e.PoBuyerIdenCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_buyer_iden_code");
            entity.Property(e => e.PoBuyerItemName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_buyer_item_name");
            entity.Property(e => e.PoBuyerPartyAddress)
                .HasMaxLength(200)
                .HasColumnName("po_buyer_party_address");
            entity.Property(e => e.PoBuyerPartyName)
                .HasMaxLength(50)
                .HasColumnName("po_buyer_party_name");
            entity.Property(e => e.PoCustomerReference)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_customer_reference");
            entity.Property(e => e.PoDateInput)
                .HasColumnType("datetime")
                .HasColumnName("po_date_input");
            entity.Property(e => e.PoDeliveredQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("po_delivered_qty");
            entity.Property(e => e.PoDeliveryIdenCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_delivery_iden_code");
            entity.Property(e => e.PoDeliveryPartyAddress)
                .HasMaxLength(200)
                .HasColumnName("po_delivery_party_address");
            entity.Property(e => e.PoDeliveryPartyName)
                .HasMaxLength(100)
                .HasColumnName("po_delivery_party_name");
            entity.Property(e => e.PoDimensions)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_dimensions");
            entity.Property(e => e.PoEta).HasColumnName("po_eta");
            entity.Property(e => e.PoIdh)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("po_idh");
            entity.Property(e => e.PoIdl)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("po_idl");
            entity.Property(e => e.PoIdw)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("po_idw");
            entity.Property(e => e.PoInventoryTransactionId)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_inventory_transaction_id");
            entity.Property(e => e.PoIssueDate)
                .HasMaxLength(19)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_issue_date");
            entity.Property(e => e.PoOdh)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("po_odh");
            entity.Property(e => e.PoOdl)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("po_odl");
            entity.Property(e => e.PoOdw)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("po_odw");
            entity.Property(e => e.PoOjitexProductCode)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("po_ojitex_product_code");
            entity.Property(e => e.PoOjitexProductName)
                .HasMaxLength(200)
                .HasColumnName("po_ojitex_product_name");
            entity.Property(e => e.PoOjitexQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("po_ojitex_qty");
            entity.Property(e => e.PoOrderReferenceId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_order_reference_id");
            entity.Property(e => e.PoPoNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_po_number");
            entity.Property(e => e.PoPricePerUom)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("po_price_per_uom");
            entity.Property(e => e.PoProductDescription)
                .HasMaxLength(100)
                .HasColumnName("po_product_description");
            entity.Property(e => e.PoQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("po_qty");
            entity.Property(e => e.PoReference)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_reference");
            entity.Property(e => e.PoRemark)
                .HasMaxLength(200)
                .HasColumnName("po_remark");
            entity.Property(e => e.PoReqNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_req_no");
            entity.Property(e => e.PoRequestDate).HasColumnName("po_request_date");
            entity.Property(e => e.PoSellerCityName)
                .HasMaxLength(50)
                .HasColumnName("po_seller_city_name");
            entity.Property(e => e.PoSellerCustomerAssignAccountId).HasColumnName("po_seller_customer_assign_account_id");
            entity.Property(e => e.PoSellerIdenCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_seller_iden_code");
            entity.Property(e => e.PoSellerItemName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_seller_item_name");
            entity.Property(e => e.PoSellerPartyAddress)
                .HasMaxLength(200)
                .HasColumnName("po_seller_party_address");
            entity.Property(e => e.PoSellerPartyName)
                .HasMaxLength(50)
                .HasColumnName("po_seller_party_name");
            entity.Property(e => e.PoStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_status");
            entity.Property(e => e.PoTotalGoodsItemQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("po_total_goods_item_qty");
            entity.Property(e => e.PoUnit)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("po_unit");
            entity.Property(e => e.PoUserId)
                .HasMaxLength(20)
                .HasColumnName("po_user_id");
        });

        modelBuilder.Entity<TConvertingOnlineReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T_Converting_Online_Report");

            entity.Property(e => e.CorNo10)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cor_no10");
            entity.Property(e => e.CorNo11)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cor_no11");
            entity.Property(e => e.CorNo12CaseQty)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no12_case_qty");
            entity.Property(e => e.CorNo13ReceiveQty)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no13_receive_qty");
            entity.Property(e => e.CorNo14FeedQty)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no14_feed_qty");
            entity.Property(e => e.CorNo15FinishQty)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no15_finish_qty");
            entity.Property(e => e.CorNo16SurplusQty)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no16_surplus_qty");
            entity.Property(e => e.CorNo17LossQtyOwn)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no17_loss_qty_own");
            entity.Property(e => e.CorNo18LossQtyOwn)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no18_loss_qty_own");
            entity.Property(e => e.CorNo19LossQtyOwn)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no19_loss_qty_own");
            entity.Property(e => e.CorNo1Date)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cor_no1_date");
            entity.Property(e => e.CorNo2)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no2");
            entity.Property(e => e.CorNo20LossQtyOwn)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no20_loss_qty_own");
            entity.Property(e => e.CorNo21LossQtyOwn)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no21_loss_qty_own");
            entity.Property(e => e.CorNo22LossQtyOther)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no22_loss_qty_other");
            entity.Property(e => e.CorNo23LossQtyOther)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no23_loss_qty_other");
            entity.Property(e => e.CorNo24LossQtyOther)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no24_loss_qty_other");
            entity.Property(e => e.CorNo25LossQtyOther)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no25_loss_qty_other");
            entity.Property(e => e.CorNo26LossQtyOther)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no26_loss_qty_other");
            entity.Property(e => e.CorNo27)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no27");
            entity.Property(e => e.CorNo28)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no28");
            entity.Property(e => e.CorNo29)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("cor_no29");
            entity.Property(e => e.CorNo30StartTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cor_no30_start_time");
            entity.Property(e => e.CorNo31FinishTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cor_no31_finish_time");
            entity.Property(e => e.CorNo32DrivingTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cor_no32_driving_time");
            entity.Property(e => e.CorNo33SetTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cor_no33_set_time");
            entity.Property(e => e.CorNo34StopTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cor_no34_stop_time");
            entity.Property(e => e.CorNo35RestTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cor_no35_rest_time");
            entity.Property(e => e.CorNo36BreakTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cor_no36_break_time");
            entity.Property(e => e.CorNo37)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no37");
            entity.Property(e => e.CorNo38)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no38");
            entity.Property(e => e.CorNo39)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no39");
            entity.Property(e => e.CorNo3Shift)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no3_shift");
            entity.Property(e => e.CorNo4)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no4");
            entity.Property(e => e.CorNo40)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("cor_no40");
            entity.Property(e => e.CorNo41)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no41");
            entity.Property(e => e.CorNo42)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("cor_no42");
            entity.Property(e => e.CorNo43MachineName)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cor_no43_machine_name");
            entity.Property(e => e.CorNo44)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("cor_no44");
            entity.Property(e => e.CorNo45)
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("cor_no45");
            entity.Property(e => e.CorNo46)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("cor_no46");
            entity.Property(e => e.CorNo47)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cor_no47");
            entity.Property(e => e.CorNo48)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no48");
            entity.Property(e => e.CorNo49)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no49");
            entity.Property(e => e.CorNo50)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no50");
            entity.Property(e => e.CorNo51)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no51");
            entity.Property(e => e.CorNo52)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no52");
            entity.Property(e => e.CorNo53)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no53");
            entity.Property(e => e.CorNo54)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no54");
            entity.Property(e => e.CorNo55)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no55");
            entity.Property(e => e.CorNo56)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no56");
            entity.Property(e => e.CorNo57)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("cor_no57");
            entity.Property(e => e.CorNo58)
                .HasMaxLength(207)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cor_no58");
            entity.Property(e => e.CorNo5MachineCode)
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("cor_no5_machine_code");
            entity.Property(e => e.CorNo6LotNo)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("cor_no6_lot_no");
            entity.Property(e => e.CorNo7OrderNo)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("cor_no7_order_no");
            entity.Property(e => e.CorNo8CustomerCode)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("cor_no8_customer_code");
            entity.Property(e => e.CorNo9ProductCode)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cor_no9_product_code");
        });

        modelBuilder.Entity<TConvertingResult>(entity =>
        {
            entity.HasKey(e => e.CrCd).HasName("PK__T_Conver__AB6B91E33CFB4FE3");

            entity.ToTable("T_Converting_Results");

            entity.Property(e => e.CrCd).HasColumnName("cr_cd");
            entity.Property(e => e.CrBreakTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cr_break_time");
            entity.Property(e => e.CrConvertingDate).HasColumnName("cr_converting_date");
            entity.Property(e => e.CrCsQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_cs_qty");
            entity.Property(e => e.CrCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("cr_cus_cd");
            entity.Property(e => e.CrDateInput)
                .HasDefaultValueSql("(getdate())", "DF_T_Converting_Results_cr_date_input")
                .HasColumnType("datetime")
                .HasColumnName("cr_date_input");
            entity.Property(e => e.CrDeliveryDate).HasColumnName("cr_delivery_date");
            entity.Property(e => e.CrDept)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cr_dept");
            entity.Property(e => e.CrDrivingTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cr_driving_time");
            entity.Property(e => e.CrFgInput)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_fg_input");
            entity.Property(e => e.CrFgOutput)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_fg_output");
            entity.Property(e => e.CrFinishDate).HasColumnName("cr_finish_date");
            entity.Property(e => e.CrFinishGood)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cr_finish_good");
            entity.Property(e => e.CrFinishTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cr_finish_time");
            entity.Property(e => e.CrFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cr_flute");
            entity.Property(e => e.CrOrderNo).HasColumnName("cr_order_no");
            entity.Property(e => e.CrPaperCode)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_paper_code");
            entity.Property(e => e.CrPlanQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_plan_qty");
            entity.Property(e => e.CrProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_pro_cd");
            entity.Property(e => e.CrReceiveNo).HasColumnName("cr_receive_no");
            entity.Property(e => e.CrRemark)
                .HasMaxLength(100)
                .HasColumnName("cr_remark");
            entity.Property(e => e.CrRestTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cr_rest_time");
            entity.Property(e => e.CrSetTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cr_set_time");
            entity.Property(e => e.CrShift).HasColumnName("cr_shift");
            entity.Property(e => e.CrSqm)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("cr_sqm");
            entity.Property(e => e.CrStartTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cr_start_time");
            entity.Property(e => e.CrStopTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("cr_stop_time");
            entity.Property(e => e.CrUserId)
                .HasMaxLength(20)
                .HasDefaultValue("com", "DF_T_Converting_Results_cr_user_id")
                .HasColumnName("cr_user_id");
        });

        modelBuilder.Entity<TCorrugatorOnlineReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T_Corrugator_Online_Report");

            entity.Property(e => e.CoorNo10Sl1)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("coor_no10_SL1");
            entity.Property(e => e.CoorNo11SheetWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no11_sheet_width");
            entity.Property(e => e.CoorNo12)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no12");
            entity.Property(e => e.CoorNo13)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no13");
            entity.Property(e => e.CoorNo14)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no14");
            entity.Property(e => e.CoorNo15)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no15");
            entity.Property(e => e.CoorNo16)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no16");
            entity.Property(e => e.CoorNo17)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no17");
            entity.Property(e => e.CoorNo18)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("coor_no18");
            entity.Property(e => e.CoorNo19)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no19");
            entity.Property(e => e.CoorNo1OrderNo)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("coor_no1_order_no");
            entity.Property(e => e.CoorNo20)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no20");
            entity.Property(e => e.CoorNo21)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no21");
            entity.Property(e => e.CoorNo22)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no22");
            entity.Property(e => e.CoorNo23)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no23");
            entity.Property(e => e.CoorNo24)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no24");
            entity.Property(e => e.CoorNo25)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no25");
            entity.Property(e => e.CoorNo26)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no26");
            entity.Property(e => e.CoorNo27)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no27");
            entity.Property(e => e.CoorNo28Sf1l)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coor_no28_SF1L");
            entity.Property(e => e.CoorNo29Sf1m)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coor_no29_SF1M");
            entity.Property(e => e.CoorNo2JobOrderNo)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("coor_no2_job_order_no");
            entity.Property(e => e.CoorNo30Sf2l)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coor_no30_SF2L");
            entity.Property(e => e.CoorNo31Sf2m)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coor_no31_SF2M");
            entity.Property(e => e.CoorNo32Dfl)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coor_no32_DFL");
            entity.Property(e => e.CoorNo33)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("coor_no33");
            entity.Property(e => e.CoorNo34QualityItem)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("coor_no34_quality_item");
            entity.Property(e => e.CoorNo35Lot2lowspeed)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("coor_no35_lot2lowspeed");
            entity.Property(e => e.CoorNo36)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no36");
            entity.Property(e => e.CoorNo37)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("coor_no37");
            entity.Property(e => e.CoorNo38DriveTime)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("coor_no38_drive_time");
            entity.Property(e => e.CoorNo39StopTime)
                .HasColumnType("decimal(6, 0)")
                .HasColumnName("coor_no39_stop_time");
            entity.Property(e => e.CoorNo3CorruLength)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no3_corru_length");
            entity.Property(e => e.CoorNo40CorrugatedDatetime)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coor_no40_corrugated_datetime");
            entity.Property(e => e.CoorNo41PlanDate)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coor_no41_plan_date");
            entity.Property(e => e.CoorNo42DateInput)
                .HasDefaultValueSql("(getdate())", "DF_T_Corrugator_Online_Report_coor_no42_date_input")
                .HasColumnType("datetime")
                .HasColumnName("coor_no42_date_input");
            entity.Property(e => e.CoorNo4TargetCut)
                .HasColumnType("decimal(5, 0)")
                .HasColumnName("coor_no4_target_cut");
            entity.Property(e => e.CoorNo5Flute)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("coor_no5_flute");
            entity.Property(e => e.CoorNo6)
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("coor_no6");
            entity.Property(e => e.CoorNo7)
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("coor_no7");
            entity.Property(e => e.CoorNo8)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("coor_no8");
            entity.Property(e => e.CoorNo9PaperWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("coor_no9_paper_width");
        });

        modelBuilder.Entity<TCsCustomerPic>(entity =>
        {
            entity.HasKey(e => e.PicCusCd).HasName("PK__T_CS_Cus__5A9FA90D2774C15F");

            entity.ToTable("T_CS_CustomerPIC");

            entity.Property(e => e.PicCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pic_cus_cd");
            entity.Property(e => e.PicCsId)
                .HasMaxLength(20)
                .HasColumnName("pic_cs_id");
            entity.Property(e => e.PicCsName)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("pic_cs_name");
            entity.Property(e => e.PicCusName)
                .HasMaxLength(50)
                .HasColumnName("pic_cus_name");
            entity.Property(e => e.PicDateInput)
                .HasColumnType("datetime")
                .HasColumnName("pic_date_input");
            entity.Property(e => e.PicDateTransfer).HasColumnName("pic_dateTransfer");
            entity.Property(e => e.PicRemark)
                .HasMaxLength(200)
                .HasColumnName("pic_remark");
            entity.Property(e => e.PicUserId)
                .HasMaxLength(20)
                .HasColumnName("pic_user_id");
            entity.Property(e => e.PicUserTransfer)
                .HasMaxLength(20)
                .HasColumnName("pic_userTransfer");
        });

        modelBuilder.Entity<TCsPlanAcceptance>(entity =>
        {
            entity.HasKey(e => e.AccId);

            entity.ToTable("T_CS_PLAN_Acceptance", tb => tb.HasComment("T_CS_PLAN_Acceptance"));

            entity.Property(e => e.AccId)
                .ValueGeneratedNever()
                .HasComment("acc_id")
                .HasColumnName("acc_id");
            entity.Property(e => e.AccClass)
                .HasComment("acc_class")
                .HasColumnName("acc_class");
            entity.Property(e => e.AccCusCd)
                .HasComment("acc_cus_cd")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("acc_cus_cd");
            entity.Property(e => e.AccDateInput)
                .HasComment("acc_date_input")
                .HasColumnType("datetime")
                .HasColumnName("acc_date_input");
            entity.Property(e => e.AccDelivery)
                .HasComment("acc_delivery")
                .HasColumnType("datetime")
                .HasColumnName("acc_delivery");
            entity.Property(e => e.AccOrderQty)
                .HasComment("acc_order_qty")
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("acc_order_qty");
            entity.Property(e => e.AccPo)
                .HasMaxLength(100)
                .HasComment("acc_po")
                .HasColumnName("acc_po");
            entity.Property(e => e.AccProCd)
                .HasComment("acc_pro_cd")
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("acc_pro_cd");
            entity.Property(e => e.AccProduct)
                .HasComment("acc_product")
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("acc_product");
            entity.Property(e => e.AccReceiveNo)
                .HasComment("acc_receive_no")
                .HasColumnName("acc_receive_no");
            entity.Property(e => e.AccRemark)
                .HasMaxLength(100)
                .HasComment("acc_remark")
                .HasColumnName("acc_remark");
            entity.Property(e => e.AccStock)
                .HasComment("acc_stock")
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("acc_stock");
            entity.Property(e => e.AccType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("acc_type")
                .HasColumnName("acc_type");
            entity.Property(e => e.AccUserName)
                .HasMaxLength(20)
                .HasComment("acc_user_name")
                .HasColumnName("acc_user_name");
        });

        modelBuilder.Entity<TCsSalesVolume>(entity =>
        {
            entity.HasKey(e => e.SvId).HasName("PK__T_CS_Sal__48C1D87DD98AC335");

            entity.ToTable("T_CS_Sales_Volume");

            entity.Property(e => e.SvId).HasColumnName("sv_id");
            entity.Property(e => e.SvCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("sv_cus_cd");
            entity.Property(e => e.SvDateEdit)
                .HasColumnType("datetime")
                .HasColumnName("sv_date_edit");
            entity.Property(e => e.SvDateInput)
                .HasColumnType("datetime")
                .HasColumnName("sv_date_input");
            entity.Property(e => e.SvDeliveryDate).HasColumnName("sv_delivery_date");
            entity.Property(e => e.SvProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("sv_pro_cd");
            entity.Property(e => e.SvQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("sv_qty");
            entity.Property(e => e.SvRemark)
                .HasMaxLength(200)
                .HasColumnName("sv_remark");
            entity.Property(e => e.SvUserEdit)
                .HasMaxLength(20)
                .HasColumnName("sv_user_edit");
            entity.Property(e => e.SvUserInput)
                .HasMaxLength(20)
                .HasColumnName("sv_user_input");
        });

        modelBuilder.Entity<TCsdisposalList>(entity =>
        {
            entity.HasKey(e => e.DisProCd);

            entity.ToTable("T_CSDisposal_List", tb => tb.HasComment("T_CSDisposal_List"));

            entity.Property(e => e.DisProCd)
                .HasComment("dis_pro_cd")
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dis_pro_cd");
            entity.Property(e => e.DisClass)
                .HasComment("dis_class")
                .HasColumnName("dis_class");
            entity.Property(e => e.DisDateInput)
                .HasComment("dis_date_input")
                .HasColumnType("datetime")
                .HasColumnName("dis_date_input");
            entity.Property(e => e.DisDeclaCd)
                .HasMaxLength(20)
                .HasComment("dis_decla_cd")
                .HasColumnName("dis_decla_cd");
            entity.Property(e => e.DisNetWeight)
                .HasMaxLength(20)
                .HasComment("dis_net_weight")
                .HasColumnName("dis_net_weight");
            entity.Property(e => e.DisPacking)
                .HasMaxLength(20)
                .HasComment("dis_packing")
                .HasColumnName("dis_packing");
            entity.Property(e => e.DisPartNa)
                .HasMaxLength(100)
                .HasComment("dis_part_na")
                .HasColumnName("dis_part_na");
            entity.Property(e => e.DisProVnname)
                .HasMaxLength(100)
                .HasComment("dis_pro_vnname")
                .HasColumnName("dis_pro_vnname");
            entity.Property(e => e.DisQtyPcs)
                .HasComment("dis_qty_pcs")
                .HasColumnName("dis_qty_pcs");
            entity.Property(e => e.DisRate)
                .HasMaxLength(20)
                .HasComment("dis_rate")
                .HasColumnName("dis_rate");
            entity.Property(e => e.DisRatio)
                .HasMaxLength(150)
                .HasComment("dis_ratio")
                .HasColumnName("dis_ratio");
            entity.Property(e => e.DisRemark)
                .HasMaxLength(150)
                .HasComment("dis_remark")
                .HasColumnName("dis_remark");
            entity.Property(e => e.DisUnitPrice)
                .HasMaxLength(20)
                .HasComment("dis_unit_price")
                .HasColumnName("dis_unit_price");
            entity.Property(e => e.DisUserId)
                .HasMaxLength(20)
                .HasComment("dis_user_id")
                .HasColumnName("dis_user_id");
        });

        modelBuilder.Entity<TCurrentStock>(entity =>
        {
            entity.HasKey(e => e.CstProCd).HasName("PK__T_Curren__3560FDE19985721E");

            entity.ToTable("T_CurrentStock");

            entity.Property(e => e.CstProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cst_pro_cd");
            entity.Property(e => e.CstDelivery)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cst_delivery");
            entity.Property(e => e.CstDisposal)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cst_disposal");
            entity.Property(e => e.CstOpenStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cst_open_stock");
            entity.Property(e => e.CstProduction)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cst_production");
            entity.Property(e => e.CstRepair)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cst_repair");
            entity.Property(e => e.CstStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cst_stock");
            entity.Property(e => e.CstWarehouseNg)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cst_warehouse_ng");
        });

        modelBuilder.Entity<TCurrentStockHistory>(entity =>
        {
            entity.HasKey(e => e.CsthId).HasName("PK__T_Curren__698E3CC05971245A");

            entity.ToTable("T_CurrentStock_History");

            entity.Property(e => e.CsthId).HasColumnName("csth_id");
            entity.Property(e => e.CsthCurrentStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("csth_currentStock");
            entity.Property(e => e.CsthDateInput)
                .HasColumnType("datetime")
                .HasColumnName("csth_date_input");
            entity.Property(e => e.CsthInputStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("csth_inputStock");
            entity.Property(e => e.CsthOutputStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("csth_outputStock");
            entity.Property(e => e.CsthProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("csth_pro_cd");
            entity.Property(e => e.CsthReceiveNo).HasColumnName("csth_receive_no");
            entity.Property(e => e.CsthRemark)
                .HasMaxLength(200)
                .HasColumnName("csth_remark");
            entity.Property(e => e.CsthSection)
                .HasMaxLength(20)
                .HasColumnName("csth_section");
            entity.Property(e => e.CsthUserId)
                .HasMaxLength(20)
                .HasColumnName("csth_user_id");
            entity.Property(e => e.CsthWorkDate).HasColumnName("csth_work_date");
        });

        modelBuilder.Entity<TCustomer>(entity =>
        {
            entity.HasKey(e => e.CusCd);

            entity.ToTable("T_Customer", tb => tb.HasComment("T_Customer"));

            entity.Property(e => e.CusCd)
                .HasComment("cus_cd")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("cus_cd");
            entity.Property(e => e.CusAddress)
                .HasMaxLength(300)
                .HasComment("cus_address")
                .HasColumnName("cus_address");
            entity.Property(e => e.CusBankAccount)
                .HasMaxLength(100)
                .HasComment("cus_bank_account")
                .HasColumnName("cus_bank_account");
            entity.Property(e => e.CusBankName)
                .HasMaxLength(100)
                .HasComment("cus_bank_name")
                .HasColumnName("cus_bank_name");
            entity.Property(e => e.CusContactName)
                .HasMaxLength(100)
                .HasComment("cus_contactName")
                .HasColumnName("cus_contactName");
            entity.Property(e => e.CusDateInput)
                .HasComment("cus_date_input")
                .HasColumnType("datetime")
                .HasColumnName("cus_date_input");
            entity.Property(e => e.CusFax)
                .HasMaxLength(50)
                .HasComment("cus_fax")
                .HasColumnName("cus_fax");
            entity.Property(e => e.CusFullName)
                .HasMaxLength(200)
                .HasComment("cus_full_name")
                .HasColumnName("cus_full_name");
            entity.Property(e => e.CusPhone)
                .HasMaxLength(50)
                .HasComment("cus_phone")
                .HasColumnName("cus_phone");
            entity.Property(e => e.CusRemark)
                .HasMaxLength(200)
                .HasComment("cus_remark")
                .HasColumnName("cus_remark");
            entity.Property(e => e.CusTaxCd)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("cus_tax_cd")
                .HasColumnName("cus_tax_cd");
            entity.Property(e => e.CusUserId)
                .HasMaxLength(20)
                .HasComment("cus_user_id")
                .HasColumnName("cus_user_id");
        });

        modelBuilder.Entity<TCustomer2Ojitex>(entity =>
        {
            entity.HasKey(e => e.C2oId).HasName("PK__T_Custom__EABD3420547A7703");

            entity.ToTable("T_Customer2Ojitex");

            entity.HasIndex(e => e.C2oProCd, "UQ__T_Custom__91088266A997B68B").IsUnique();

            entity.Property(e => e.C2oId).HasColumnName("c2o_id");
            entity.Property(e => e.C2oClass).HasColumnName("c2o_class");
            entity.Property(e => e.C2oDateInput)
                .HasColumnType("datetime")
                .HasColumnName("c2o_date_input");
            entity.Property(e => e.C2oDeclaCd)
                .HasMaxLength(20)
                .HasColumnName("c2o_decla_cd");
            entity.Property(e => e.C2oNetWeight)
                .HasMaxLength(20)
                .HasColumnName("c2o_net_weight");
            entity.Property(e => e.C2oPacking)
                .HasMaxLength(20)
                .HasColumnName("c2o_packing");
            entity.Property(e => e.C2oPartNa)
                .HasMaxLength(100)
                .HasColumnName("c2o_part_na");
            entity.Property(e => e.C2oProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("c2o_pro_cd");
            entity.Property(e => e.C2oProVnname)
                .HasMaxLength(100)
                .HasColumnName("c2o_pro_vnname");
            entity.Property(e => e.C2oQtyPcs).HasColumnName("c2o_qty_pcs");
            entity.Property(e => e.C2oRate)
                .HasMaxLength(20)
                .HasColumnName("c2o_rate");
            entity.Property(e => e.C2oRatio)
                .HasMaxLength(150)
                .HasColumnName("c2o_ratio");
            entity.Property(e => e.C2oRemark)
                .HasMaxLength(150)
                .HasColumnName("c2o_remark");
            entity.Property(e => e.C2oUnitPrice)
                .HasMaxLength(20)
                .HasColumnName("c2o_unit_price");
            entity.Property(e => e.C2oUserId)
                .HasMaxLength(20)
                .HasColumnName("c2o_user_id");
        });

        modelBuilder.Entity<TCustomerAddress>(entity =>
        {
            entity.HasKey(e => e.CaId).HasName("PK__T_Custom__0875B1F8B273D698");

            entity.ToTable("T_CustomerAddresses");

            entity.Property(e => e.CaId).HasColumnName("ca_id");
            entity.Property(e => e.CaAddressDetail)
                .HasMaxLength(200)
                .HasColumnName("ca_address_detail");
            entity.Property(e => e.CaAddressName)
                .HasMaxLength(100)
                .HasColumnName("ca_address_name");
            entity.Property(e => e.CaCity)
                .HasMaxLength(20)
                .HasColumnName("ca_city");
            entity.Property(e => e.CaCountry)
                .HasMaxLength(20)
                .HasColumnName("ca_country");
            entity.Property(e => e.CaCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("ca_cus_cd");
            entity.Property(e => e.CaDateInput)
                .HasColumnType("datetime")
                .HasColumnName("ca_date_input");
            entity.Property(e => e.CaLineNo).HasColumnName("ca_line_no");
            entity.Property(e => e.CaOther)
                .HasMaxLength(300)
                .HasColumnName("ca_other");
            entity.Property(e => e.CaPic)
                .HasMaxLength(100)
                .HasColumnName("ca_pic");
            entity.Property(e => e.CaPicPhone)
                .HasMaxLength(50)
                .HasColumnName("ca_pic_phone");
            entity.Property(e => e.CaUserId)
                .HasMaxLength(20)
                .HasColumnName("ca_user_id");
        });

        modelBuilder.Entity<TDestination>(entity =>
        {
            entity.HasKey(e => e.DeCode).HasName("PK__T_Destin__20BBF0959F3F67EF");

            entity.ToTable("T_Destination");

            entity.Property(e => e.DeCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("de_code");
            entity.Property(e => e.DeAddress)
                .HasMaxLength(200)
                .HasColumnName("de_address");
            entity.Property(e => e.DeDateInput)
                .HasColumnType("datetime")
                .HasColumnName("de_date_input");
            entity.Property(e => e.DeFullName)
                .HasMaxLength(100)
                .HasColumnName("de_full_name");
            entity.Property(e => e.DePicName)
                .HasMaxLength(20)
                .HasColumnName("de_pic_name");
            entity.Property(e => e.DePicPhone)
                .HasMaxLength(20)
                .HasColumnName("de_pic_phone");
            entity.Property(e => e.DeUserId)
                .HasMaxLength(20)
                .HasColumnName("de_user_id");
        });

        modelBuilder.Entity<TDisposalProduct>(entity =>
        {
            entity.HasKey(e => e.DpId).HasName("PK__T_Dispos__B5B1AAFC319B3C56");

            entity.ToTable("T_DisposalProduct", tb => tb.HasTrigger("DisposalProductUpdateTrigger"));

            entity.Property(e => e.DpId).HasColumnName("dp_id");
            entity.Property(e => e.DpCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("dp_cus_cd");
            entity.Property(e => e.DpDateChange)
                .HasColumnType("datetime")
                .HasColumnName("dp_date_change");
            entity.Property(e => e.DpDateInput)
                .HasColumnType("datetime")
                .HasColumnName("dp_date_input");
            entity.Property(e => e.DpDateReport).HasColumnName("dp_date_report");
            entity.Property(e => e.DpPic)
                .HasMaxLength(20)
                .HasColumnName("dp_pic");
            entity.Property(e => e.DpProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dp_pro_cd");
            entity.Property(e => e.DpQuantity)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dp_quantity");
            entity.Property(e => e.DpReceiveNo).HasColumnName("dp_receive_no");
            entity.Property(e => e.DpRemark)
                .HasMaxLength(200)
                .HasColumnName("dp_remark");
            entity.Property(e => e.DpUserId)
                .HasMaxLength(20)
                .HasColumnName("dp_user_id");
        });

        modelBuilder.Entity<TGenPaperMaster>(entity =>
        {
            entity.HasKey(e => e.GenGradeCd).HasName("PK__T_GEN_Pa__F51FEE9CFCB6D13F");

            entity.ToTable("T_GEN_PaperMaster");

            entity.Property(e => e.GenGradeCd)
                .ValueGeneratedNever()
                .HasColumnName("gen_grade_cd");
            entity.Property(e => e.GenActual)
                .HasColumnType("decimal(9, 0)")
                .HasColumnName("gen_actual");
            entity.Property(e => e.GenBudget)
                .HasColumnType("decimal(9, 0)")
                .HasColumnName("gen_budget");
            entity.Property(e => e.GenDateChange)
                .HasColumnType("datetime")
                .HasColumnName("gen_date_change");
            entity.Property(e => e.GenDateInput)
                .HasColumnType("datetime")
                .HasColumnName("gen_date_input");
            entity.Property(e => e.GenGradeName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gen_grade_name");
            entity.Property(e => e.GenUserId)
                .HasMaxLength(20)
                .HasColumnName("gen_user_id");
        });

        modelBuilder.Entity<TInputCustomer>(entity =>
        {
            entity.HasKey(e => e.CusCd);

            entity.ToTable("T_Input_Customer", tb => tb.HasComment("T_Input_Customer"));

            entity.Property(e => e.CusCd)
                .HasComment("cus_cd")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("cus_cd");
            entity.Property(e => e.CusAddress)
                .HasMaxLength(200)
                .HasComment("cus_address")
                .HasColumnName("cus_address");
            entity.Property(e => e.CusBankAccount)
                .HasMaxLength(50)
                .HasComment("cus_bank_account")
                .HasColumnName("cus_bank_account");
            entity.Property(e => e.CusBankName)
                .HasMaxLength(100)
                .HasComment("cus_bank_name")
                .HasColumnName("cus_bank_name");
            entity.Property(e => e.CusDateInput)
                .HasComment("cus_date_input")
                .HasColumnType("datetime")
                .HasColumnName("cus_date_input");
            entity.Property(e => e.CusEmail)
                .HasMaxLength(20)
                .HasComment("cus_email")
                .HasColumnName("cus_email");
            entity.Property(e => e.CusFax)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("cus_fax")
                .HasColumnName("cus_fax");
            entity.Property(e => e.CusFullName)
                .HasMaxLength(100)
                .HasComment("cus_full_name")
                .HasColumnName("cus_full_name");
            entity.Property(e => e.CusName)
                .HasMaxLength(50)
                .HasComment("cus_name")
                .HasColumnName("cus_name");
            entity.Property(e => e.CusNation)
                .HasMaxLength(10)
                .HasComment("cus_nation")
                .HasColumnName("cus_nation");
            entity.Property(e => e.CusPersonInChart)
                .HasMaxLength(50)
                .HasComment("cus_person_in_chart")
                .HasColumnName("cus_person_in_chart");
            entity.Property(e => e.CusPhone)
                .HasMaxLength(30)
                .HasComment("cus_phone")
                .HasColumnName("cus_phone");
            entity.Property(e => e.CusTaxCd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("cus_tax_cd")
                .HasColumnName("cus_tax_cd");
            entity.Property(e => e.CusUserId)
                .HasMaxLength(20)
                .HasComment("cus_user_id")
                .HasColumnName("cus_user_id");
            entity.Property(e => e.CusVat)
                .HasComment("cus_vat")
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("cus_vat");
        });

        modelBuilder.Entity<TInputProduct>(entity =>
        {
            entity.HasKey(e => e.ProCd);

            entity.ToTable("T_Input_Product", tb => tb.HasComment("T_Input_Product"));

            entity.Property(e => e.ProCd)
                .HasComment("pro_cd")
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pro_cd");
            entity.Property(e => e.ProBundleQty)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_bundle_qty");
            entity.Property(e => e.ProCaseLen)
                .HasComment("pro_case_len")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_case_len");
            entity.Property(e => e.ProCaseWid)
                .HasComment("pro_case_wid")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_case_wid");
            entity.Property(e => e.ProCorruLen)
                .HasComment("pro_corru_len")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_corru_len");
            entity.Property(e => e.ProCorruWid)
                .HasComment("pro_corru_wid")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_corru_wid");
            entity.Property(e => e.ProCusCd)
                .HasComment("pro_cus_cd")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pro_cus_cd");
            entity.Property(e => e.ProDateInput)
                .HasComment("pro_date_input")
                .HasColumnType("datetime")
                .HasColumnName("pro_date_input");
            entity.Property(e => e.ProDiePlate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_die_plate")
                .HasColumnName("pro_die_plate");
            entity.Property(e => e.ProFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_flute")
                .HasColumnName("pro_flute");
            entity.Property(e => e.ProFontOverJoint)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_font_over_joint");
            entity.Property(e => e.ProHandRemark)
                .HasMaxLength(10)
                .HasComment("pro_hand_remark")
                .HasColumnName("pro_hand_remark");
            entity.Property(e => e.ProInOutGlueJoint)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_in_out_glue_joint");
            entity.Property(e => e.ProInk1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_ink_1")
                .HasColumnName("pro_ink_1");
            entity.Property(e => e.ProInk2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_ink_2")
                .HasColumnName("pro_ink_2");
            entity.Property(e => e.ProInk3)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_ink_3")
                .HasColumnName("pro_ink_3");
            entity.Property(e => e.ProInk4)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_ink_4")
                .HasColumnName("pro_ink_4");
            entity.Property(e => e.ProInk5)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_ink_5")
                .HasColumnName("pro_ink_5");
            entity.Property(e => e.ProInk6)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pro_ink_6");
            entity.Property(e => e.ProInk7)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pro_ink_7");
            entity.Property(e => e.ProInk8)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pro_ink_8");
            entity.Property(e => e.ProJoint)
                .HasColumnType("decimal(2, 0)")
                .HasColumnName("pro_joint");
            entity.Property(e => e.ProName)
                .HasMaxLength(200)
                .HasComment("pro_name")
                .HasColumnName("pro_name");
            entity.Property(e => e.ProPanel1)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_panel1");
            entity.Property(e => e.ProPanel2)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_panel2");
            entity.Property(e => e.ProPanel3)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_panel3");
            entity.Property(e => e.ProPanel4)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_panel4");
            entity.Property(e => e.ProPaperCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_paper_cd")
                .HasColumnName("pro_paper_cd");
            entity.Property(e => e.ProPaperRequest)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("pro_paper_request");
            entity.Property(e => e.ProPerSheet)
                .HasComment("pro_per_sheet")
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pro_per_sheet");
            entity.Property(e => e.ProPrintPlate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_print_plate")
                .HasColumnName("pro_print_plate");
            entity.Property(e => e.ProProcess1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_process_1")
                .HasColumnName("pro_process_1");
            entity.Property(e => e.ProProcess2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_process_2")
                .HasColumnName("pro_process_2");
            entity.Property(e => e.ProProcess3)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_process_3")
                .HasColumnName("pro_process_3");
            entity.Property(e => e.ProProcess4)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_process_4")
                .HasColumnName("pro_process_4");
            entity.Property(e => e.ProProcess5)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("pro_process_5")
                .HasColumnName("pro_process_5");
            entity.Property(e => e.ProRearOverJoint)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_rear_over_joint");
            entity.Property(e => e.ProReguSpd)
                .HasComment("pro_regu_spd")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_regu_spd");
            entity.Property(e => e.ProRemark)
                .HasMaxLength(200)
                .HasComment("pro_remark")
                .HasColumnName("pro_remark");
            entity.Property(e => e.ProRemark2)
                .HasMaxLength(200)
                .HasComment("pro_remark_2")
                .HasColumnName("pro_remark_2");
            entity.Property(e => e.ProScore1)
                .HasComment("pro_score_1")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_score_1");
            entity.Property(e => e.ProScore2)
                .HasComment("pro_score_2")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_score_2");
            entity.Property(e => e.ProScore3)
                .HasComment("pro_score_3")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_score_3");
            entity.Property(e => e.ProScore4)
                .HasComment("pro_score_4")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_score_4");
            entity.Property(e => e.ProScore5)
                .HasComment("pro_score_5")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_score_5");
            entity.Property(e => e.ProSheetLen)
                .HasComment("pro_sheet_len")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_sheet_len");
            entity.Property(e => e.ProSheetSqu)
                .HasComment("pro_sheet_squ")
                .HasColumnType("decimal(5, 3)")
                .HasColumnName("pro_sheet_squ");
            entity.Property(e => e.ProSheetWid)
                .HasComment("pro_sheet_wid")
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_sheet_wid");
            entity.Property(e => e.ProSideTrim)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pro_side_trim");
            entity.Property(e => e.ProSpecCd)
                .HasComment("pro_spec_cd")
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("pro_spec_cd");
            entity.Property(e => e.ProSs1Up)
                .HasComment("pro_ss1_up")
                .HasColumnName("pro_ss1_up");
            entity.Property(e => e.ProSs2Up)
                .HasComment("pro_ss2_up")
                .HasColumnName("pro_ss2_up");
            entity.Property(e => e.ProTomUp)
                .HasComment("pro_tom_up")
                .HasColumnName("pro_tom_up");
            entity.Property(e => e.ProTotalProcess)
                .HasComment("pro_total_process")
                .HasColumnType("decimal(1, 0)")
                .HasColumnName("pro_total_process");
            entity.Property(e => e.ProUserId)
                .HasMaxLength(20)
                .HasComment("pro_user_id")
                .HasColumnName("pro_user_id");
        });

        modelBuilder.Entity<TInputSchedule>(entity =>
        {
            entity.HasKey(e => e.InCd).HasName("PK__T_Input___1CD0DAA1BB870906");

            entity.ToTable("T_Input_Schedule", tb =>
                {
                    tb.HasTrigger("PlanGetReceiveNo");
                    tb.HasTrigger("TRG_INPUT_SCHEDULE_HISTORY");
                });

            entity.Property(e => e.InCd).HasColumnName("in_cd");
            entity.Property(e => e.InCorruDay).HasColumnName("in_corru_day");
            entity.Property(e => e.InCsClass).HasColumnName("in_cs_class");
            entity.Property(e => e.InCsDelivery).HasColumnName("in_cs_delivery");
            entity.Property(e => e.InCsOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("in_cs_order_date");
            entity.Property(e => e.InCsUserid)
                .HasMaxLength(20)
                .HasColumnName("in_cs_userid");
            entity.Property(e => e.InCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("in_cus_cd");
            entity.Property(e => e.InDateInput)
                .HasColumnType("datetime")
                .HasColumnName("in_date_input");
            entity.Property(e => e.InFinishDay).HasColumnName("in_finish_day");
            entity.Property(e => e.InLotQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("in_lot_qty");
            entity.Property(e => e.InOrderQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("in_order_qty");
            entity.Property(e => e.InProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("in_pro_cd");
            entity.Property(e => e.InProcess1Date).HasColumnName("in_process1_date");
            entity.Property(e => e.InProcess2Date).HasColumnName("in_process2_date");
            entity.Property(e => e.InProcess3Date).HasColumnName("in_process3_date");
            entity.Property(e => e.InProcess4Date).HasColumnName("in_process4_date");
            entity.Property(e => e.InProcess5Date).HasColumnName("in_process5_date");
            entity.Property(e => e.InQuantity)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("in_quantity");
            entity.Property(e => e.InReceiveNo).HasColumnName("in_receive_no");
            entity.Property(e => e.InRemark)
                .HasMaxLength(200)
                .HasColumnName("in_remark");
            entity.Property(e => e.InUserId)
                .HasMaxLength(20)
                .HasColumnName("in_user_id");
        });

        modelBuilder.Entity<TInputScheduleHistory>(entity =>
        {
            entity.HasKey(e => e.InCd).HasName("PK__T_Input___1CD0DAA1732F069E");

            entity.ToTable("T_Input_Schedule_History");

            entity.Property(e => e.InCd).HasColumnName("in_cd");
            entity.Property(e => e.InCorruDay).HasColumnName("in_corru_day");
            entity.Property(e => e.InCsClass).HasColumnName("in_cs_class");
            entity.Property(e => e.InCsDelivery).HasColumnName("in_cs_delivery");
            entity.Property(e => e.InCsOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("in_cs_order_date");
            entity.Property(e => e.InCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("in_cus_cd");
            entity.Property(e => e.InFinishDay).HasColumnName("in_finish_day");
            entity.Property(e => e.InLotQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("in_lot_qty");
            entity.Property(e => e.InOrderQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("in_order_qty");
            entity.Property(e => e.InProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("in_pro_cd");
            entity.Property(e => e.InProcess1Date).HasColumnName("in_process1_date");
            entity.Property(e => e.InProcess2Date).HasColumnName("in_process2_date");
            entity.Property(e => e.InProcess3Date).HasColumnName("in_process3_date");
            entity.Property(e => e.InProcess4Date).HasColumnName("in_process4_date");
            entity.Property(e => e.InProcess5Date).HasColumnName("in_process5_date");
            entity.Property(e => e.InQuantity)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("in_quantity");
            entity.Property(e => e.InReceiveNo).HasColumnName("in_receive_no");
            entity.Property(e => e.InRemark)
                .HasMaxLength(200)
                .HasColumnName("in_remark");
            entity.Property(e => e.InTimeDelete)
                .HasDefaultValueSql("(getdate())", "DF_T_Input_Schedule_History_in_time_delete")
                .HasColumnType("datetime")
                .HasColumnName("in_time_delete");
            entity.Property(e => e.InUserId)
                .HasMaxLength(20)
                .HasColumnName("in_user_id");
        });

        modelBuilder.Entity<TInterMemo>(entity =>
        {
            entity.HasKey(e => e.ImId).HasName("PK__T_Inter___FBA835BA37E658D7");

            entity.ToTable("T_Inter_Memo");

            entity.Property(e => e.ImId).HasColumnName("im_id");
            entity.Property(e => e.ImActNo)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("im_act_no");
            entity.Property(e => e.ImCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("im_cus_cd");
            entity.Property(e => e.ImDateInput)
                .HasColumnType("datetime")
                .HasColumnName("im_date_input");
            entity.Property(e => e.ImDatePrinted)
                .HasColumnType("datetime")
                .HasColumnName("im_date_printed");
            entity.Property(e => e.ImDelivery).HasColumnName("im_delivery");
            entity.Property(e => e.ImDetail)
                .HasMaxLength(300)
                .HasColumnName("im_detail");
            entity.Property(e => e.ImEditNo).HasColumnName("im_edit_no");
            entity.Property(e => e.ImNew)
                .HasMaxLength(50)
                .HasColumnName("im_new");
            entity.Property(e => e.ImOld)
                .HasMaxLength(50)
                .HasColumnName("im_old");
            entity.Property(e => e.ImOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("im_order_date");
            entity.Property(e => e.ImPrintState)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("im_print_state");
            entity.Property(e => e.ImProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("im_pro_cd");
            entity.Property(e => e.ImProductQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("im_product_qty");
            entity.Property(e => e.ImReceiveNo).HasColumnName("im_receive_no");
            entity.Property(e => e.ImSection)
                .HasMaxLength(10)
                .HasColumnName("im_section");
            entity.Property(e => e.ImUserId)
                .HasMaxLength(20)
                .HasColumnName("im_user_id");
        });

        modelBuilder.Entity<TKamPaperMaster>(entity =>
        {
            entity.HasKey(e => e.KamPaperCd).HasName("PK__T_KAM_Pa__0493CDD843CC751D");

            entity.ToTable("T_KAM_PaperMaster");

            entity.Property(e => e.KamPaperCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("kam_paper_cd");
            entity.Property(e => e.KamALinerGradeCd).HasColumnName("kam_a_liner_grade_cd");
            entity.Property(e => e.KamALinerWeight).HasColumnName("kam_a_liner_weight");
            entity.Property(e => e.KamAMediumGradeCd).HasColumnName("kam_a_medium_grade_cd");
            entity.Property(e => e.KamAMediumWeight).HasColumnName("kam_a_medium_weight");
            entity.Property(e => e.KamBLinerGradeCd).HasColumnName("kam_b_liner_grade_cd");
            entity.Property(e => e.KamBLinerWeight).HasColumnName("kam_b_liner_weight");
            entity.Property(e => e.KamBMediumGradeCd).HasColumnName("kam_b_medium_grade_cd");
            entity.Property(e => e.KamBMediumWeight).HasColumnName("kam_b_medium_weight");
            entity.Property(e => e.KamDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("kam_date_changed");
            entity.Property(e => e.KamDateInput)
                .HasColumnType("datetime")
                .HasColumnName("kam_date_input");
            entity.Property(e => e.KamFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("kam_flute");
            entity.Property(e => e.KamGlueGradeCd).HasColumnName("kam_glue_grade_cd");
            entity.Property(e => e.KamGlueWeight).HasColumnName("kam_glue_weight");
            entity.Property(e => e.KamPaperName)
                .HasMaxLength(150)
                .HasColumnName("kam_paper_name");
            entity.Property(e => e.KamUnitPrice)
                .HasColumnType("decimal(9, 3)")
                .HasColumnName("kam_unit_price");
            entity.Property(e => e.KamUserId)
                .HasMaxLength(20)
                .HasColumnName("kam_user_id");
            entity.Property(e => e.KamWeight)
                .HasColumnType("decimal(9, 3)")
                .HasColumnName("kam_weight");
        });

        modelBuilder.Entity<TLastStock>(entity =>
        {
            entity.HasKey(e => e.LstProCd).HasName("PK__T_LastSt__50D8118DBAE286D1");

            entity.ToTable("T_LastStock");

            entity.Property(e => e.LstProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lst_pro_cd");
            entity.Property(e => e.LstDelivery)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lst_delivery");
            entity.Property(e => e.LstDisposal)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lst_disposal");
            entity.Property(e => e.LstOpenStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lst_open_stock");
            entity.Property(e => e.LstProduction)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lst_production");
            entity.Property(e => e.LstRepair)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lst_repair");
            entity.Property(e => e.LstStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lst_stock");
            entity.Property(e => e.LstWarehouseNg)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lst_warehouse_ng");
        });

        modelBuilder.Entity<TLastStockHistory>(entity =>
        {
            entity.HasKey(e => e.LsthId).HasName("PK__T_LastSt__96DAA6AB2BECBE07");

            entity.ToTable("T_LastStock_History");

            entity.Property(e => e.LsthId).HasColumnName("lsth_id");
            entity.Property(e => e.LsthCurrentStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lsth_currentStock");
            entity.Property(e => e.LsthDateInput)
                .HasColumnType("datetime")
                .HasColumnName("lsth_date_input");
            entity.Property(e => e.LsthInputStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lsth_inputStock");
            entity.Property(e => e.LsthOutputStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lsth_outputStock");
            entity.Property(e => e.LsthProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("lsth_pro_cd");
            entity.Property(e => e.LsthReceiveNo).HasColumnName("lsth_receive_no");
            entity.Property(e => e.LsthRemark)
                .HasMaxLength(200)
                .HasColumnName("lsth_remark");
            entity.Property(e => e.LsthUserId)
                .HasMaxLength(20)
                .HasColumnName("lsth_user_id");
            entity.Property(e => e.LsthWorkDate).HasColumnName("lsth_work_date");
            entity.Property(e => e.LthSection)
                .HasMaxLength(20)
                .HasColumnName("lth_section");
        });

        modelBuilder.Entity<TLogiDeliveryNote>(entity =>
        {
            entity.HasKey(e => e.DnKey).HasName("PK__T_Logi_D__06D222A4AF02E0C9");

            entity.ToTable("T_Logi_DeliveryNote", tb => tb.HasTrigger("DeliveryNoteUpdateTrigger"));

            entity.HasIndex(e => e.DnDeliverySlip, "UQ__T_Logi_D__505295168CEED35A").IsUnique();

            entity.Property(e => e.DnKey).HasColumnName("dn_key");
            entity.Property(e => e.DnCarType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dn_car_type");
            entity.Property(e => e.DnCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("dn_cus_cd");
            entity.Property(e => e.DnCusNa)
                .HasMaxLength(100)
                .HasColumnName("dn_cus_na");
            entity.Property(e => e.DnDateChange)
                .HasColumnType("datetime")
                .HasColumnName("dn_date_change");
            entity.Property(e => e.DnDateConfirm)
                .HasColumnType("datetime")
                .HasColumnName("dn_date_confirm");
            entity.Property(e => e.DnDateDelivery).HasColumnName("dn_date_delivery");
            entity.Property(e => e.DnDateInput)
                .HasColumnType("datetime")
                .HasColumnName("dn_date_input");
            entity.Property(e => e.DnDeliveryAddress)
                .HasMaxLength(200)
                .HasColumnName("dn_delivery_address");
            entity.Property(e => e.DnDeliverySlip)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dn_delivery_slip");
            entity.Property(e => e.DnGoodsType)
                .HasMaxLength(30)
                .HasColumnName("dn_goods_type");
            entity.Property(e => e.DnNote)
                .HasMaxLength(200)
                .HasColumnName("dn_note");
            entity.Property(e => e.DnState)
                .HasMaxLength(50)
                .HasColumnName("dn_state");
            entity.Property(e => e.DnStatus)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dn_status");
            entity.Property(e => e.DnStatusRemark)
                .HasMaxLength(200)
                .HasColumnName("dn_status_remark");
            entity.Property(e => e.DnSupCd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dn_sup_cd");
            entity.Property(e => e.DnTransportCompany)
                .HasMaxLength(50)
                .HasColumnName("dn_transport_company");
            entity.Property(e => e.DnTruckNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dn_truck_number");
            entity.Property(e => e.DnUserConfirm)
                .HasMaxLength(20)
                .HasColumnName("dn_user_confirm");
            entity.Property(e => e.DnUserId)
                .HasMaxLength(20)
                .HasColumnName("dn_user_id");
        });

        modelBuilder.Entity<TLogiDeliveryNoteDetail>(entity =>
        {
            entity.HasKey(e => e.DndKey).HasName("PK__T_Logi_D__A81E5F436E975F0F");

            entity.ToTable("T_Logi_DeliveryNoteDetail", tb => tb.HasTrigger("Delivery_Note_Detail_UpdateTrigger"));

            entity.Property(e => e.DndKey).HasColumnName("dnd_key");
            entity.Property(e => e.DndCsPo)
                .HasMaxLength(200)
                .HasColumnName("dnd_cs_po");
            entity.Property(e => e.DndDateInput)
                .HasColumnType("datetime")
                .HasColumnName("dnd_date_input");
            entity.Property(e => e.DndDeliverySlip)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dnd_delivery_slip");
            entity.Property(e => e.DndLineNo).HasColumnName("dnd_line_no");
            entity.Property(e => e.DndOtherNote)
                .HasMaxLength(200)
                .HasColumnName("dnd_other_note");
            entity.Property(e => e.DndProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dnd_pro_cd");
            entity.Property(e => e.DndProName)
                .HasMaxLength(200)
                .HasColumnName("dnd_pro_name");
            entity.Property(e => e.DndQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dnd_qty");
            entity.Property(e => e.DndReceiveNo)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dnd_receive_no");
            entity.Property(e => e.DndRemark)
                .HasMaxLength(200)
                .HasColumnName("dnd_remark");
        });

        modelBuilder.Entity<TLogiDeliveryNoteDetailFake>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T_Logi_DeliveryNoteDetailFake");

            entity.Property(e => e.DndfCsPo)
                .HasMaxLength(200)
                .HasColumnName("dndf_cs_po");
            entity.Property(e => e.DndfDeliverySlip)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dndf_delivery_slip");
            entity.Property(e => e.DndfLineNo).HasColumnName("dndf_line_no");
            entity.Property(e => e.DndfOtherNote)
                .HasMaxLength(200)
                .HasColumnName("dndf_other_note");
            entity.Property(e => e.DndfProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dndf_pro_cd");
            entity.Property(e => e.DndfProName)
                .HasMaxLength(200)
                .HasColumnName("dndf_pro_name");
            entity.Property(e => e.DndfQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dndf_qty");
            entity.Property(e => e.DndfReceiveNo)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dndf_receive_no");
            entity.Property(e => e.DndfRemark)
                .HasMaxLength(200)
                .HasColumnName("dndf_remark");
        });

        modelBuilder.Entity<TLogiDeliveryNoteFake>(entity =>
        {
            entity.HasKey(e => e.DnkDeliverySlip).HasName("PK__T_Logi_D__CC069AD0523FAA74");

            entity.ToTable("T_Logi_DeliveryNoteFake");

            entity.Property(e => e.DnkDeliverySlip)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dnk_delivery_slip");
            entity.Property(e => e.DnkCarType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dnk_car_type");
            entity.Property(e => e.DnkCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("dnk_cus_cd");
            entity.Property(e => e.DnkCusNa)
                .HasMaxLength(100)
                .HasColumnName("dnk_cus_na");
            entity.Property(e => e.DnkDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("dnk_date_changed");
            entity.Property(e => e.DnkDateConfirm)
                .HasColumnType("datetime")
                .HasColumnName("dnk_date_confirm");
            entity.Property(e => e.DnkDateDelivery).HasColumnName("dnk_date_delivery");
            entity.Property(e => e.DnkDateInput)
                .HasColumnType("datetime")
                .HasColumnName("dnk_date_input");
            entity.Property(e => e.DnkDeliveryAddress)
                .HasMaxLength(200)
                .HasColumnName("dnk_delivery_address");
            entity.Property(e => e.DnkGoodsType)
                .HasMaxLength(30)
                .HasColumnName("dnk_goods_type");
            entity.Property(e => e.DnkNote)
                .HasMaxLength(200)
                .HasColumnName("dnk_note");
            entity.Property(e => e.DnkStatus)
                .HasMaxLength(11)
                .HasColumnName("dnk_status");
            entity.Property(e => e.DnkSupCd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dnk_sup_cd");
            entity.Property(e => e.DnkTransportCompany)
                .HasMaxLength(50)
                .HasColumnName("dnk_transport_company");
            entity.Property(e => e.DnkTruckNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dnk_truck_number");
            entity.Property(e => e.DnkUserIdChanged)
                .HasMaxLength(20)
                .HasColumnName("dnk_user_id_changed");
            entity.Property(e => e.DnkUserIdConfirm)
                .HasMaxLength(20)
                .HasColumnName("dnk_user_id_confirm");
            entity.Property(e => e.DnkUserIdInput)
                .HasMaxLength(20)
                .HasColumnName("dnk_user_id_input");
        });

        modelBuilder.Entity<TLogiDeliveryPlan>(entity =>
        {
            entity.HasKey(e => e.DpId).HasName("PK__T_Logi_D__B5B1AAFC8C074FC4");

            entity.ToTable("T_Logi_DeliveryPlan");

            entity.Property(e => e.DpId).HasColumnName("dp_id");
            entity.Property(e => e.DpAcceptanceNote)
                .HasMaxLength(200)
                .HasColumnName("dp_acceptance_note");
            entity.Property(e => e.DpActNo)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dp_act_no");
            entity.Property(e => e.DpAutualQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dp_autual_qty");
            entity.Property(e => e.DpCusNa)
                .HasMaxLength(100)
                .HasColumnName("dp_cus_na");
            entity.Property(e => e.DpDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("dp_date_changed");
            entity.Property(e => e.DpDateDelivery).HasColumnName("dp_date_delivery");
            entity.Property(e => e.DpDateInput)
                .HasColumnType("datetime")
                .HasColumnName("dp_date_input");
            entity.Property(e => e.DpFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dp_flute");
            entity.Property(e => e.DpLastProcess)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("dp_last_process");
            entity.Property(e => e.DpPoNo)
                .HasMaxLength(200)
                .HasColumnName("dp_po_no");
            entity.Property(e => e.DpProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dp_pro_cd");
            entity.Property(e => e.DpProName)
                .HasMaxLength(200)
                .HasColumnName("dp_pro_name");
            entity.Property(e => e.DpQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dp_qty");
            entity.Property(e => e.DpReceiveNo)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dp_receive_no");
            entity.Property(e => e.DpRemark)
                .HasMaxLength(200)
                .HasColumnName("dp_remark");
            entity.Property(e => e.DpSqm)
                .HasColumnType("decimal(5, 3)")
                .HasColumnName("dp_sqm");
            entity.Property(e => e.DpStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("dp_stock");
            entity.Property(e => e.DpTotalSqm)
                .HasColumnType("decimal(9, 3)")
                .HasColumnName("dp_total_sqm");
            entity.Property(e => e.DpUserId)
                .HasMaxLength(20)
                .HasColumnName("dp_user_id");
            entity.Property(e => e.DpUserIdChanged)
                .HasMaxLength(20)
                .HasColumnName("dp_user_id_changed");
        });

        modelBuilder.Entity<TLogiGoodsReceipt>(entity =>
        {
            entity.HasKey(e => e.GrKey).HasName("PK__T_Logi_G__A3A424CCCEA4E46A");

            entity.ToTable("T_Logi_GoodsReceipt", tb => tb.HasTrigger("GoodsReceiptUpdateTrigger"));

            entity.Property(e => e.GrKey).HasColumnName("gr_key");
            entity.Property(e => e.GrCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("gr_cus_cd");
            entity.Property(e => e.GrDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("gr_date_changed");
            entity.Property(e => e.GrDateInput)
                .HasColumnType("datetime")
                .HasColumnName("gr_date_input");
            entity.Property(e => e.GrDateReceipt).HasColumnName("gr_date_receipt");
            entity.Property(e => e.GrNote)
                .HasMaxLength(200)
                .HasColumnName("gr_note");
            entity.Property(e => e.GrProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("gr_pro_cd");
            entity.Property(e => e.GrProName)
                .HasMaxLength(300)
                .HasColumnName("gr_pro_name");
            entity.Property(e => e.GrProductionSection)
                .HasMaxLength(10)
                .HasColumnName("gr_production_section");
            entity.Property(e => e.GrQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("gr_qty");
            entity.Property(e => e.GrReceiveNo)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("gr_receive_no");
            entity.Property(e => e.GrUserChanged)
                .HasMaxLength(20)
                .HasColumnName("gr_user_changed");
            entity.Property(e => e.GrUserId)
                .HasMaxLength(20)
                .HasColumnName("gr_user_id");
        });

        modelBuilder.Entity<TLogiPaperRollDeliveryNote>(entity =>
        {
            entity.HasKey(e => e.PdnDeliverySlip).HasName("PK__T_Logi_P__E672B42BB7FD5582");

            entity.ToTable("T_Logi_PaperRoll_DeliveryNote");

            entity.Property(e => e.PdnDeliverySlip)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdn_delivery_slip");
            entity.Property(e => e.PdnCarType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdn_car_type");
            entity.Property(e => e.PdnChangedCount).HasColumnName("pdn_changed_count");
            entity.Property(e => e.PdnContainerNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdn_container_no");
            entity.Property(e => e.PdnDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("pdn_date_changed");
            entity.Property(e => e.PdnDateConfirm)
                .HasColumnType("datetime")
                .HasColumnName("pdn_date_confirm");
            entity.Property(e => e.PdnDateDelivery).HasColumnName("pdn_date_delivery");
            entity.Property(e => e.PdnDateInput)
                .HasColumnType("datetime")
                .HasColumnName("pdn_date_input");
            entity.Property(e => e.PdnDeliveryAddress)
                .HasMaxLength(200)
                .HasColumnName("pdn_delivery_address");
            entity.Property(e => e.PdnDesCd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdn_des_cd");
            entity.Property(e => e.PdnDesNa)
                .HasMaxLength(100)
                .HasColumnName("pdn_des_na");
            entity.Property(e => e.PdnNote)
                .HasMaxLength(200)
                .HasColumnName("pdn_note");
            entity.Property(e => e.PdnStatus)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdn_status");
            entity.Property(e => e.PdnTransportCompany)
                .HasMaxLength(50)
                .HasColumnName("pdn_transport_company");
            entity.Property(e => e.PdnUserIdChanged)
                .HasMaxLength(20)
                .HasColumnName("pdn_user_id_changed");
            entity.Property(e => e.PdnUserIdConfirm)
                .HasMaxLength(20)
                .HasColumnName("pdn_user_id_confirm");
            entity.Property(e => e.PdnUserIdInput)
                .HasMaxLength(20)
                .HasColumnName("pdn_user_id_input");
        });

        modelBuilder.Entity<TLogiPaperRollDeliveryNoteDetail>(entity =>
        {
            entity.HasKey(e => new { e.PdndDeliverySlip, e.PdndLabel });

            entity.ToTable("T_Logi_PaperRoll_DeliveryNote_Detail");

            entity.Property(e => e.PdndDeliverySlip)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdnd_delivery_slip");
            entity.Property(e => e.PdndLabel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdnd_label");
            entity.Property(e => e.PdndExportKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pdnd_export_kg");
            entity.Property(e => e.PdndExportMeter)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pdnd_export_meter");
            entity.Property(e => e.PdndGradeName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdnd_grade_name");
            entity.Property(e => e.PdndGramage)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pdnd_gramage");
            entity.Property(e => e.PdndOtherNote)
                .HasMaxLength(200)
                .HasColumnName("pdnd_other_note");
            entity.Property(e => e.PdndPaperType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdnd_paper_type");
            entity.Property(e => e.PdndRemark)
                .HasMaxLength(200)
                .HasColumnName("pdnd_remark");
            entity.Property(e => e.PdndSerialNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pdnd_serial_no");
            entity.Property(e => e.PdndWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pdnd_width");
        });

        modelBuilder.Entity<TMachineSpec>(entity =>
        {
            entity.HasKey(e => e.MName).HasName("PK__T_Machin__F4DF136D92DE09FB");

            entity.ToTable("T_Machine_Spec");

            entity.Property(e => e.MName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("m_name");
            entity.Property(e => e.MDateInput)
                .HasColumnType("datetime")
                .HasColumnName("m_date_input");
            entity.Property(e => e.MEfficiency).HasColumnName("m_efficiency");
            entity.Property(e => e.MMinSettime).HasColumnName("m_min_settime");
            entity.Property(e => e.MRemark)
                .HasMaxLength(200)
                .HasColumnName("m_remark");
            entity.Property(e => e.MSpeed).HasColumnName("m_speed");
            entity.Property(e => e.MUserId)
                .HasMaxLength(20)
                .HasColumnName("m_user_id");
            entity.Property(e => e.MWorkingTime)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("m_working_time");
        });

        modelBuilder.Entity<TMachineSpecification>(entity =>
        {
            entity.HasKey(e => e.MsMachineName).HasName("PK__T_Machin__B3042ACAD6F4E572");

            entity.ToTable("T_Machine_Specification");

            entity.Property(e => e.MsMachineName)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ms_machine_name");
            entity.Property(e => e.MsDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("ms_date_changed");
            entity.Property(e => e.MsDateInput)
                .HasColumnType("datetime")
                .HasColumnName("ms_date_input");
            entity.Property(e => e.MsDeepOfSlotMax).HasColumnName("ms_deep_of_slot_max");
            entity.Property(e => e.MsDeepOfSlotMin).HasColumnName("ms_deep_of_slot_min");
            entity.Property(e => e.MsHighMax).HasColumnName("ms_high_max");
            entity.Property(e => e.MsHighMin).HasColumnName("ms_high_min");
            entity.Property(e => e.MsLengthMax).HasColumnName("ms_length_max");
            entity.Property(e => e.MsLengthMin).HasColumnName("ms_length_min");
            entity.Property(e => e.MsLengthOfFirstFlapMax).HasColumnName("ms_length_of_first_flap_max");
            entity.Property(e => e.MsLengthOfFirstFlapMin).HasColumnName("ms_length_of_first_flap_min");
            entity.Property(e => e.MsLengthOfSecondFlapMax).HasColumnName("ms_length_of_second_flap_max");
            entity.Property(e => e.MsLengthOfSecondFlapMin).HasColumnName("ms_length_of_second_flap_min");
            entity.Property(e => e.MsRemark)
                .HasMaxLength(200)
                .HasColumnName("ms_remark");
            entity.Property(e => e.MsUserChanged)
                .HasMaxLength(20)
                .HasColumnName("ms_user_changed");
            entity.Property(e => e.MsUserInput)
                .HasMaxLength(20)
                .HasColumnName("ms_user_input");
            entity.Property(e => e.MsWidthMax).HasColumnName("ms_width_max");
            entity.Property(e => e.MsWidthMin).HasColumnName("ms_width_min");
        });

        modelBuilder.Entity<TMatExportPaperRoll>(entity =>
        {
            entity.HasKey(e => e.EprId).HasName("PK__T_MAT_Ex__130091CDCA3E30C5");

            entity.ToTable("T_MAT_ExportPaperRoll");

            entity.Property(e => e.EprId).HasColumnName("epr_id");
            entity.Property(e => e.EprContainerNo)
                .HasMaxLength(30)
                .HasColumnName("epr_container_no");
            entity.Property(e => e.EprDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("epr_date_changed");
            entity.Property(e => e.EprDateDelivered)
                .HasColumnType("datetime")
                .HasColumnName("epr_date_delivered");
            entity.Property(e => e.EprDateInput)
                .HasColumnType("datetime")
                .HasColumnName("epr_date_input");
            entity.Property(e => e.EprGramage)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("epr_gramage");
            entity.Property(e => e.EprInvoiceDate).HasColumnName("epr_invoice_date");
            entity.Property(e => e.EprInvoiceNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("epr_invoice_no");
            entity.Property(e => e.EprLogisticsRemark)
                .HasMaxLength(300)
                .HasColumnName("epr_logistics_remark");
            entity.Property(e => e.EprMaterialRemark)
                .HasMaxLength(300)
                .HasColumnName("epr_material_remark");
            entity.Property(e => e.EprName)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("epr_name");
            entity.Property(e => e.EprQty)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("epr_qty");
            entity.Property(e => e.EprSupplierName)
                .HasMaxLength(100)
                .HasColumnName("epr_supplier_name");
            entity.Property(e => e.EprType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("epr_type");
            entity.Property(e => e.EprUsdAmount)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("epr_usd_amount");
            entity.Property(e => e.EprUsdOthersFee)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("epr_usd_others_fee");
            entity.Property(e => e.EprUsdPrice)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("epr_usd_price");
            entity.Property(e => e.EprUsdTransportFee)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("epr_usd_transport_fee");
            entity.Property(e => e.EprUserId)
                .HasMaxLength(20)
                .HasColumnName("epr_user_id");
            entity.Property(e => e.EprVndAmount)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("epr_vnd_amount");
            entity.Property(e => e.EprVndOthersFee)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("epr_vnd_others_fee");
            entity.Property(e => e.EprVndPrice)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("epr_vnd_price");
            entity.Property(e => e.EprVndTransportFee)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("epr_vnd_transport_fee");
        });

        modelBuilder.Entity<TMatMakerMaster>(entity =>
        {
            entity.HasKey(e => e.MmCode).HasName("PK__T_MAT_Ma__8EEFF1AA64A518E6");

            entity.ToTable("T_MAT_Maker_Master");

            entity.Property(e => e.MmCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mm_code");
            entity.Property(e => e.MmAddress)
                .HasMaxLength(200)
                .HasColumnName("mm_address");
            entity.Property(e => e.MmDateInput)
                .HasColumnType("datetime")
                .HasColumnName("mm_date_input");
            entity.Property(e => e.MmName)
                .HasMaxLength(20)
                .HasColumnName("mm_name");
            entity.Property(e => e.MmNote)
                .HasMaxLength(300)
                .HasColumnName("mm_note");
            entity.Property(e => e.MmPhone)
                .HasMaxLength(20)
                .HasColumnName("mm_phone");
            entity.Property(e => e.MmUserId)
                .HasMaxLength(20)
                .HasColumnName("mm_user_id");
        });

        modelBuilder.Entity<TMatPaperRollComming>(entity =>
        {
            entity.HasKey(e => e.PrcSerialNo).HasName("PK__T_MAT_Pa__D776159DAB2AE7EB");

            entity.ToTable("T_MAT_PaperRollComming");

            entity.Property(e => e.PrcSerialNo).HasColumnName("prc_serial_no");
            entity.Property(e => e.PrcContainerNo)
                .HasMaxLength(30)
                .HasColumnName("prc_container_no");
            entity.Property(e => e.PrcDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("prc_date_changed");
            entity.Property(e => e.PrcDateComming).HasColumnName("prc_date_comming");
            entity.Property(e => e.PrcDateInput)
                .HasColumnType("datetime")
                .HasColumnName("prc_date_input");
            entity.Property(e => e.PrcGraceCd).HasColumnName("prc_grace_cd");
            entity.Property(e => e.PrcGraceNa)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prc_grace_na");
            entity.Property(e => e.PrcGramage)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("prc_gramage");
            entity.Property(e => e.PrcInvoiceNo)
                .HasMaxLength(50)
                .HasColumnName("prc_invoice_no");
            entity.Property(e => e.PrcLength)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("prc_length");
            entity.Property(e => e.PrcLogisticsCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prc_logistics_code");
            entity.Property(e => e.PrcLogisticsRemark)
                .HasMaxLength(300)
                .HasColumnName("prc_logistics_remark");
            entity.Property(e => e.PrcMakerCd)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prc_maker_cd");
            entity.Property(e => e.PrcMakerNa)
                .HasMaxLength(20)
                .HasColumnName("prc_maker_na");
            entity.Property(e => e.PrcMaterialRemark)
                .HasMaxLength(300)
                .HasColumnName("prc_material_remark");
            entity.Property(e => e.PrcPaperRollNo)
                .HasMaxLength(30)
                .HasColumnName("prc_paper_roll_no");
            entity.Property(e => e.PrcSupCd)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prc_sup_cd");
            entity.Property(e => e.PrcSupNa)
                .HasMaxLength(20)
                .HasColumnName("prc_sup_na");
            entity.Property(e => e.PrcTotalAmount)
                .HasColumnType("decimal(7, 3)")
                .HasColumnName("prc_total_amount");
            entity.Property(e => e.PrcUnitPrice)
                .HasColumnType("decimal(7, 3)")
                .HasColumnName("prc_unit_price");
            entity.Property(e => e.PrcUserId)
                .HasMaxLength(20)
                .HasColumnName("prc_user_id");
            entity.Property(e => e.PrcWeight)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("prc_weight");
            entity.Property(e => e.PrcWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("prc_width");
        });

        modelBuilder.Entity<TMatPurchasePaperRoll>(entity =>
        {
            entity.HasKey(e => e.PprId).HasName("PK__T_MAT_Pu__47075E7C09288734");

            entity.ToTable("T_MAT_PurchasePaperRoll");

            entity.Property(e => e.PprId).HasColumnName("ppr_id");
            entity.Property(e => e.PprContainerNo)
                .HasMaxLength(30)
                .HasColumnName("ppr_container_no");
            entity.Property(e => e.PprDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("ppr_date_changed");
            entity.Property(e => e.PprDateInput)
                .HasColumnType("datetime")
                .HasColumnName("ppr_date_input");
            entity.Property(e => e.PprDateReceived)
                .HasColumnType("datetime")
                .HasColumnName("ppr_date_received");
            entity.Property(e => e.PprGramage)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("ppr_gramage");
            entity.Property(e => e.PprInvoiceDate).HasColumnName("ppr_invoice_date");
            entity.Property(e => e.PprInvoiceNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ppr_invoice_no");
            entity.Property(e => e.PprLogisticsRemark)
                .HasMaxLength(300)
                .HasColumnName("ppr_logistics_remark");
            entity.Property(e => e.PprMaterialRemark)
                .HasMaxLength(300)
                .HasColumnName("ppr_material_remark");
            entity.Property(e => e.PprName)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ppr_name");
            entity.Property(e => e.PprQty)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("ppr_qty");
            entity.Property(e => e.PprSupplierName)
                .HasMaxLength(100)
                .HasColumnName("ppr_supplier_name");
            entity.Property(e => e.PprType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ppr_type");
            entity.Property(e => e.PprUsdAmount)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("ppr_usd_amount");
            entity.Property(e => e.PprUsdOthersFee)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("ppr_usd_others_fee");
            entity.Property(e => e.PprUsdPrice)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("ppr_usd_price");
            entity.Property(e => e.PprUsdTransportFee)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("ppr_usd_transport_fee");
            entity.Property(e => e.PprUserId)
                .HasMaxLength(20)
                .HasColumnName("ppr_user_id");
            entity.Property(e => e.PprVndAmount)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("ppr_vnd_amount");
            entity.Property(e => e.PprVndOthersFee)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("ppr_vnd_others_fee");
            entity.Property(e => e.PprVndPrice)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("ppr_vnd_price");
            entity.Property(e => e.PprVndTransportFee)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("ppr_vnd_transport_fee");
        });

        modelBuilder.Entity<TMatSalesForecast>(entity =>
        {
            entity.HasKey(e => e.SfSerialNo).HasName("PK__T_MAT_Sa__61674E14934B8B92");

            entity.ToTable("T_MAT_Sales_Forecast");

            entity.Property(e => e.SfSerialNo).HasColumnName("sf_serial_no");
            entity.Property(e => e.SfCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("sf_cus_cd");
            entity.Property(e => e.SfDateForecast).HasColumnName("sf_date_forecast");
            entity.Property(e => e.SfDateInput)
                .HasColumnType("datetime")
                .HasColumnName("sf_date_input");
            entity.Property(e => e.SfProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("sf_pro_cd");
            entity.Property(e => e.SfQuantity)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("sf_quantity");
            entity.Property(e => e.SfRemark)
                .HasMaxLength(300)
                .HasColumnName("sf_remark");
            entity.Property(e => e.SfUserId)
                .HasMaxLength(20)
                .HasColumnName("sf_user_id");
        });

        modelBuilder.Entity<TMatSupplierMaster>(entity =>
        {
            entity.HasKey(e => e.SmCode).HasName("PK__T_MAT_Su__4A8397C37BA75EA8");

            entity.ToTable("T_MAT_Supplier_Master");

            entity.Property(e => e.SmCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("sm_code");
            entity.Property(e => e.SmAddress)
                .HasMaxLength(200)
                .HasColumnName("sm_address");
            entity.Property(e => e.SmDateInput)
                .HasColumnType("datetime")
                .HasColumnName("sm_date_input");
            entity.Property(e => e.SmName)
                .HasMaxLength(20)
                .HasColumnName("sm_name");
            entity.Property(e => e.SmNote)
                .HasMaxLength(300)
                .HasColumnName("sm_note");
            entity.Property(e => e.SmPhone)
                .HasMaxLength(20)
                .HasColumnName("sm_phone");
            entity.Property(e => e.SmUserId)
                .HasMaxLength(20)
                .HasColumnName("sm_user_id");
        });

        modelBuilder.Entity<TMonthlyStock>(entity =>
        {
            entity.HasKey(e => e.MstId).HasName("PK__T_Monthl__F7547A1D77098854");

            entity.ToTable("T_MonthlyStock");

            entity.Property(e => e.MstId).HasColumnName("mst_id");
            entity.Property(e => e.MstCloseStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("mst_close_stock");
            entity.Property(e => e.MstDateReport).HasColumnName("mst_date_report");
            entity.Property(e => e.MstDelivery)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("mst_delivery");
            entity.Property(e => e.MstDisposal)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("mst_disposal");
            entity.Property(e => e.MstOpenStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("mst_open_stock");
            entity.Property(e => e.MstProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("mst_pro_cd");
            entity.Property(e => e.MstProduction)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("mst_production");
            entity.Property(e => e.MstRepair)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("mst_repair");
            entity.Property(e => e.MstWarehouseNg)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("mst_warehouse_ng");
        });

        modelBuilder.Entity<TMonthlyStockHistory>(entity =>
        {
            entity.HasKey(e => e.MsthId).HasName("PK__T_Monthl__CBC9E691B3F8EA59");

            entity.ToTable("T_MonthlyStock_History");

            entity.Property(e => e.MsthId).HasColumnName("msth_id");
            entity.Property(e => e.MsthCurrentStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("msth_currentStock");
            entity.Property(e => e.MsthDateInput)
                .HasColumnType("datetime")
                .HasColumnName("msth_date_input");
            entity.Property(e => e.MsthDateReport).HasColumnName("msth_date_report");
            entity.Property(e => e.MsthInputStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("msth_inputStock");
            entity.Property(e => e.MsthOutputStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("msth_outputStock");
            entity.Property(e => e.MsthProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("msth_pro_cd");
            entity.Property(e => e.MsthReceiveNo).HasColumnName("msth_receive_no");
            entity.Property(e => e.MsthRemark)
                .HasMaxLength(200)
                .HasColumnName("msth_remark");
            entity.Property(e => e.MsthSection)
                .HasMaxLength(20)
                .HasColumnName("msth_section");
            entity.Property(e => e.MsthUserId)
                .HasMaxLength(20)
                .HasColumnName("msth_user_id");
            entity.Property(e => e.MsthWorkDate).HasColumnName("msth_work_date");
        });

        modelBuilder.Entity<TNextStock>(entity =>
        {
            entity.HasKey(e => e.NstProCd).HasName("PK__T_NextSt__B1C939929F8F3B08");

            entity.ToTable("T_NextStock");

            entity.Property(e => e.NstProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nst_pro_cd");
            entity.Property(e => e.NstDelivery)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nst_delivery");
            entity.Property(e => e.NstDisposal)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nst_disposal");
            entity.Property(e => e.NstOpenStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nst_open_stock");
            entity.Property(e => e.NstProduction)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nst_production");
            entity.Property(e => e.NstRepair)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nst_repair");
            entity.Property(e => e.NstStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nst_stock");
            entity.Property(e => e.NstWarehouseNg)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nst_warehouse_ng");
        });

        modelBuilder.Entity<TNextStockHistory>(entity =>
        {
            entity.HasKey(e => e.NsthId).HasName("PK__T_NextSt__82C3CDAAB09DC6CA");

            entity.ToTable("T_NextStock_History");

            entity.Property(e => e.NsthId).HasColumnName("nsth_id");
            entity.Property(e => e.NsthCurrentStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nsth_currentStock");
            entity.Property(e => e.NsthDateInput)
                .HasColumnType("datetime")
                .HasColumnName("nsth_date_input");
            entity.Property(e => e.NsthInputStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nsth_inputStock");
            entity.Property(e => e.NsthOutputStock)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nsth_outputStock");
            entity.Property(e => e.NsthProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("nsth_pro_cd");
            entity.Property(e => e.NsthReceiveNo).HasColumnName("nsth_receive_no");
            entity.Property(e => e.NsthRemark)
                .HasMaxLength(200)
                .HasColumnName("nsth_remark");
            entity.Property(e => e.NsthSection)
                .HasMaxLength(20)
                .HasColumnName("nsth_section");
            entity.Property(e => e.NsthUserId)
                .HasMaxLength(20)
                .HasColumnName("nsth_user_id");
            entity.Property(e => e.NsthWorkDate).HasColumnName("nsth_work_date");
        });

        modelBuilder.Entity<TOjitexHoliday>(entity =>
        {
            entity.HasKey(e => e.OjhKey).HasName("PK__T_Ojitex__5D965E31CDF8CD5B");

            entity.ToTable("T_Ojitex_Holiday");

            entity.Property(e => e.OjhKey).HasColumnName("ojh_key");
            entity.Property(e => e.OjhDateInput)
                .HasColumnType("datetime")
                .HasColumnName("ojh_date_input");
            entity.Property(e => e.OjhDayoff).HasColumnName("ojh_dayoff");
            entity.Property(e => e.OjhUserId)
                .HasMaxLength(20)
                .HasColumnName("ojh_user_id");
            entity.Property(e => e.OjiColor).HasColumnName("oji_color");
            entity.Property(e => e.OjiRemark)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("oji_remark");
        });

        modelBuilder.Entity<TOpeningStock>(entity =>
        {
            entity.HasKey(e => e.OsId).HasName("PK__T_Openin__374FA4B5DE090271");

            entity.ToTable("T_OpeningStock", tb => tb.HasTrigger("OpeningStockUpdateTrigger"));

            entity.Property(e => e.OsId).HasColumnName("os_id");
            entity.Property(e => e.OsArea)
                .HasMaxLength(100)
                .HasColumnName("os_area");
            entity.Property(e => e.OsCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("os_cus_cd");
            entity.Property(e => e.OsDateChange)
                .HasColumnType("datetime")
                .HasColumnName("os_date_change");
            entity.Property(e => e.OsDateInput)
                .HasColumnType("datetime")
                .HasColumnName("os_date_input");
            entity.Property(e => e.OsDateReport).HasColumnName("os_date_report");
            entity.Property(e => e.OsLabel)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("os_label");
            entity.Property(e => e.OsPic)
                .HasMaxLength(20)
                .HasColumnName("os_PIC");
            entity.Property(e => e.OsProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("os_pro_cd");
            entity.Property(e => e.OsRealStockQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("os_real_stock_qty");
            entity.Property(e => e.OsReceiveNo).HasColumnName("os_receive_no");
            entity.Property(e => e.OsRemark)
                .HasMaxLength(200)
                .HasColumnName("os_remark");
            entity.Property(e => e.OsUserId)
                .HasMaxLength(20)
                .HasColumnName("os_user_id");
        });

        modelBuilder.Entity<TOtherProduct>(entity =>
        {
            entity.HasKey(e => e.OpId).HasName("PK__T_OtherP__A26AE2CE3D97D960");

            entity.ToTable("T_OtherProduct");

            entity.Property(e => e.OpId).HasColumnName("op_id");
            entity.Property(e => e.OpDate).HasColumnName("op_date");
            entity.Property(e => e.OpDateInput)
                .HasColumnType("datetime")
                .HasColumnName("op_date_input");
            entity.Property(e => e.OpProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("op_pro_cd");
            entity.Property(e => e.OpQuantity)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("op_quantity");
            entity.Property(e => e.OpReceiveNo).HasColumnName("op_receive_no");
            entity.Property(e => e.OpRemark)
                .HasMaxLength(200)
                .HasColumnName("op_remark");
            entity.Property(e => e.OpSection)
                .HasMaxLength(20)
                .HasColumnName("op_section");
            entity.Property(e => e.OpSqm)
                .HasColumnType("decimal(7, 3)")
                .HasColumnName("op_sqm");
            entity.Property(e => e.OpTotalSqm)
                .HasColumnType("decimal(7, 3)")
                .HasColumnName("op_totalSQM");
            entity.Property(e => e.OpUserId)
                .HasMaxLength(20)
                .HasColumnName("op_user_id");
        });

        modelBuilder.Entity<TPaperRollClosingStock>(entity =>
        {
            entity.HasKey(e => e.PcsId).HasName("PK__T_PaperR__B91443A46C5BC7EC");

            entity.ToTable("T_PaperRoll_Closing_Stock");

            entity.Property(e => e.PcsId).HasColumnName("pcs_id");
            entity.Property(e => e.PcsDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("pcs_date_changed");
            entity.Property(e => e.PcsDateInput)
                .HasColumnType("datetime")
                .HasColumnName("pcs_date_input");
            entity.Property(e => e.PcsDateReceive)
                .HasColumnType("datetime")
                .HasColumnName("pcs_date_receive");
            entity.Property(e => e.PcsGramage)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcs_gramage");
            entity.Property(e => e.PcsInputKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pcs_input_kg");
            entity.Property(e => e.PcsKind)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcs_kind");
            entity.Property(e => e.PcsLabel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcs_label");
            entity.Property(e => e.PcsPaperType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcs_paper_type");
            entity.Property(e => e.PcsRemainKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pcs_remain_kg");
            entity.Property(e => e.PcsRemainMetre)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pcs_remain_metre");
            entity.Property(e => e.PcsRemark)
                .HasMaxLength(300)
                .HasColumnName("pcs_remark");
            entity.Property(e => e.PcsSerialNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcs_serial_no");
            entity.Property(e => e.PcsSupplierName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcs_supplier_name");
            entity.Property(e => e.PcsUsedKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pcs_used_kg");
            entity.Property(e => e.PcsUserId)
                .HasMaxLength(20)
                .HasColumnName("pcs_user_id");
            entity.Property(e => e.PcsWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcs_width");
        });

        modelBuilder.Entity<TPaperRollExport>(entity =>
        {
            entity.HasKey(e => e.PreLabel);

            entity.ToTable("T_PaperRoll_Export");

            entity.Property(e => e.PreLabel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pre_label");
            entity.Property(e => e.PreContainerNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pre_container_no");
            entity.Property(e => e.PreDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("pre_date_changed");
            entity.Property(e => e.PreDateDelivery)
                .HasColumnType("datetime")
                .HasColumnName("pre_date_delivery");
            entity.Property(e => e.PreDateInput)
                .HasColumnType("datetime")
                .HasColumnName("pre_date_input");
            entity.Property(e => e.PreDestination)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pre_destination");
            entity.Property(e => e.PreExportKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pre_export_kg");
            entity.Property(e => e.PreGradeName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pre_grade_name");
            entity.Property(e => e.PreGramage)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pre_gramage");
            entity.Property(e => e.PrePaperType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pre_paper_type");
            entity.Property(e => e.PreRemark)
                .HasMaxLength(300)
                .HasColumnName("pre_remark");
            entity.Property(e => e.PreSn)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pre_sn");
            entity.Property(e => e.PreUserId)
                .HasMaxLength(20)
                .HasColumnName("pre_user_id");
            entity.Property(e => e.PreWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pre_width");
        });

        modelBuilder.Entity<TPaperRollImport>(entity =>
        {
            entity.HasKey(e => e.PrimLabel);

            entity.ToTable("T_PaperRoll_Import");

            entity.Property(e => e.PrimLabel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prim_label");
            entity.Property(e => e.PrimContainerNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prim_container_no");
            entity.Property(e => e.PrimDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("prim_date_changed");
            entity.Property(e => e.PrimDateInput)
                .HasColumnType("datetime")
                .HasColumnName("prim_date_input");
            entity.Property(e => e.PrimDateReceived)
                .HasColumnType("datetime")
                .HasColumnName("prim_date_received");
            entity.Property(e => e.PrimGradeName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prim_grade_name");
            entity.Property(e => e.PrimGramage)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("prim_gramage");
            entity.Property(e => e.PrimImportKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("prim_import_kg");
            entity.Property(e => e.PrimPaperType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prim_paper_type");
            entity.Property(e => e.PrimRemark)
                .HasMaxLength(300)
                .HasColumnName("prim_remark");
            entity.Property(e => e.PrimSn)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prim_sn");
            entity.Property(e => e.PrimSupplierName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prim_supplier_name");
            entity.Property(e => e.PrimUserId)
                .HasMaxLength(20)
                .HasColumnName("prim_user_id");
            entity.Property(e => e.PrimUserIdChanged)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("prim_user_id_changed");
            entity.Property(e => e.PrimWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("prim_width");
        });

        modelBuilder.Entity<TPaperRollOpeningStock>(entity =>
        {
            entity.HasKey(e => e.PosId).HasName("PK__T_PaperR__D1A4EB12876D6884");

            entity.ToTable("T_PaperRoll_Opening_Stock");

            entity.Property(e => e.PosId).HasColumnName("pos_id");
            entity.Property(e => e.PosDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("pos_date_changed");
            entity.Property(e => e.PosDateInput)
                .HasColumnType("datetime")
                .HasColumnName("pos_date_input");
            entity.Property(e => e.PosDateReceive)
                .HasColumnType("datetime")
                .HasColumnName("pos_date_receive");
            entity.Property(e => e.PosGramage)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pos_gramage");
            entity.Property(e => e.PosInputKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pos_input_kg");
            entity.Property(e => e.PosKind)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pos_kind");
            entity.Property(e => e.PosLabel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pos_label");
            entity.Property(e => e.PosPaperType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pos_paper_type");
            entity.Property(e => e.PosRemainKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pos_remain_kg");
            entity.Property(e => e.PosRemainMetre)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pos_remain_metre");
            entity.Property(e => e.PosRemark)
                .HasMaxLength(300)
                .HasColumnName("pos_remark");
            entity.Property(e => e.PosSerialNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pos_serial_no");
            entity.Property(e => e.PosSupplierName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pos_supplier_name");
            entity.Property(e => e.PosUsedKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pos_used_kg");
            entity.Property(e => e.PosUserId)
                .HasMaxLength(20)
                .HasColumnName("pos_user_id");
            entity.Property(e => e.PosWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pos_width");
        });

        modelBuilder.Entity<TPaperRollUsed>(entity =>
        {
            entity.HasKey(e => e.PruId).HasName("PK__T_PaperR__0081245FF4D2EC8D");

            entity.ToTable("T_PaperRoll_Used");

            entity.Property(e => e.PruId).HasColumnName("pru_id");
            entity.Property(e => e.PruAfterUsed)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pru_after_used");
            entity.Property(e => e.PruCurrentStock)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pru_current_stock");
            entity.Property(e => e.PruDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("pru_date_changed");
            entity.Property(e => e.PruDateConfirm)
                .HasColumnType("datetime")
                .HasColumnName("pru_date_confirm");
            entity.Property(e => e.PruDateInput)
                .HasColumnType("datetime")
                .HasColumnName("pru_date_input");
            entity.Property(e => e.PruDateUsed)
                .HasColumnType("datetime")
                .HasColumnName("pru_date_used");
            entity.Property(e => e.PruGramage)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pru_gramage");
            entity.Property(e => e.PruLabel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pru_label");
            entity.Property(e => e.PruName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pru_name");
            entity.Property(e => e.PruPaperType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pru_paper_type");
            entity.Property(e => e.PruRemark)
                .HasMaxLength(300)
                .HasColumnName("pru_remark");
            entity.Property(e => e.PruSection)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pru_section");
            entity.Property(e => e.PruSerialNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pru_serial_no");
            entity.Property(e => e.PruStatus)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pru_status");
            entity.Property(e => e.PruUsedKg)
                .HasColumnType("decimal(8, 0)")
                .HasColumnName("pru_used_kg");
            entity.Property(e => e.PruUserId)
                .HasMaxLength(20)
                .HasColumnName("pru_user_id");
            entity.Property(e => e.PruUserIdChanged)
                .HasMaxLength(20)
                .HasColumnName("pru_user_id_changed");
            entity.Property(e => e.PruUserIdConfirm)
                .HasMaxLength(20)
                .HasColumnName("pru_user_id_confirm");
            entity.Property(e => e.PruWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pru_width");
        });

        modelBuilder.Entity<TPlanOutputConverting>(entity =>
        {
            entity.HasKey(e => e.PcvtCd).HasName("PK__T_PLAN_O__9FFADEAC200D7F2C");

            entity.ToTable("T_PLAN_Output_Converting");

            entity.Property(e => e.PcvtCd).HasColumnName("pcvt_cd");
            entity.Property(e => e.PcvtCaseQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pcvt_case_qty");
            entity.Property(e => e.PcvtConvertingDay).HasColumnName("pcvt_converting_day");
            entity.Property(e => e.PcvtCorruDate).HasColumnName("pcvt_corru_date");
            entity.Property(e => e.PcvtCsOrder)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pcvt_cs_order");
            entity.Property(e => e.PcvtCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pcvt_cus_cd");
            entity.Property(e => e.PcvtCusNa)
                .HasMaxLength(50)
                .HasColumnName("pcvt_cus_na");
            entity.Property(e => e.PcvtDateInput)
                .HasColumnType("datetime")
                .HasColumnName("pcvt_date_input");
            entity.Property(e => e.PcvtDeliveryDate).HasColumnName("pcvt_delivery_date");
            entity.Property(e => e.PcvtDiePlateNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_die_plate_no");
            entity.Property(e => e.PcvtFinishDate).HasColumnName("pcvt_finish_date");
            entity.Property(e => e.PcvtFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_flute");
            entity.Property(e => e.PcvtInk1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_ink_1");
            entity.Property(e => e.PcvtInk2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_ink_2");
            entity.Property(e => e.PcvtInk3)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_ink_3");
            entity.Property(e => e.PcvtInk4)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_ink_4");
            entity.Property(e => e.PcvtInk5)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_ink_5");
            entity.Property(e => e.PcvtOrderNo).HasColumnName("pcvt_order_no");
            entity.Property(e => e.PcvtPaperCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_paper_cd");
            entity.Property(e => e.PcvtPrintPlateNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_print_plate_no");
            entity.Property(e => e.PcvtProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pcvt_pro_cd");
            entity.Property(e => e.PcvtProNa)
                .HasMaxLength(150)
                .HasColumnName("pcvt_pro_na");
            entity.Property(e => e.PcvtProcess1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_process_1");
            entity.Property(e => e.PcvtProcess1Date).HasColumnName("pcvt_process_1_date");
            entity.Property(e => e.PcvtProcess2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_process_2");
            entity.Property(e => e.PcvtProcess2Date).HasColumnName("pcvt_process_2_date");
            entity.Property(e => e.PcvtProcess3)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_process_3");
            entity.Property(e => e.PcvtProcess3Date).HasColumnName("pcvt_process_3_date");
            entity.Property(e => e.PcvtProcess4)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_process_4");
            entity.Property(e => e.PcvtProcess4Date).HasColumnName("pcvt_process_4_date");
            entity.Property(e => e.PcvtProcessName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_process_name");
            entity.Property(e => e.PcvtQtyPerCorru)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pcvt_qty_per_corru");
            entity.Property(e => e.PcvtQtyPerSheet)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pcvt_qty_per_sheet");
            entity.Property(e => e.PcvtReceiveNo).HasColumnName("pcvt_receive_no");
            entity.Property(e => e.PcvtRunTime)
                .HasColumnType("decimal(6, 1)")
                .HasColumnName("pcvt_run_time");
            entity.Property(e => e.PcvtSetTime).HasColumnName("pcvt_set_time");
            entity.Property(e => e.PcvtSheetLen)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcvt_sheet_len");
            entity.Property(e => e.PcvtSheetSqm)
                .HasColumnType("decimal(5, 3)")
                .HasColumnName("pcvt_sheet_sqm");
            entity.Property(e => e.PcvtSheetWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcvt_sheet_width");
            entity.Property(e => e.PcvtTargetDate).HasColumnName("pcvt_target_date");
            entity.Property(e => e.PcvtTargetTime)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcvt_target_time");
            entity.Property(e => e.PcvtUserId)
                .HasMaxLength(20)
                .HasColumnName("pcvt_user_id");
        });

        modelBuilder.Entity<TPlanOutputCorrugator>(entity =>
        {
            entity.HasKey(e => e.PcorCd).HasName("PK__T_PLAN_O__465DBEAF40422189");

            entity.ToTable("T_PLAN_Output_Corrugator");

            entity.Property(e => e.PcorCd).HasColumnName("pcor_cd");
            entity.Property(e => e.PcorAlNa)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_al_na");
            entity.Property(e => e.PcorAlWeight).HasColumnName("pcor_al_weight");
            entity.Property(e => e.PcorAmNa)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_am_na");
            entity.Property(e => e.PcorAmWeight).HasColumnName("pcor_am_weight");
            entity.Property(e => e.PcorBlNa)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_bl_na");
            entity.Property(e => e.PcorBlWeight).HasColumnName("pcor_bl_weight");
            entity.Property(e => e.PcorBmNa)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_bm_na");
            entity.Property(e => e.PcorBmWeight).HasColumnName("pcor_bm_weight");
            entity.Property(e => e.PcorCaseLen)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_case_len");
            entity.Property(e => e.PcorCaseQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pcor_case_qty");
            entity.Property(e => e.PcorCaseWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_case_width");
            entity.Property(e => e.PcorClassSlitter)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pcor_class_slitter");
            entity.Property(e => e.PcorCorruDate).HasColumnName("pcor_corru_date");
            entity.Property(e => e.PcorCorruDay).HasColumnName("pcor_corru_day");
            entity.Property(e => e.PcorCorruLength)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_corru_length");
            entity.Property(e => e.PcorCorruWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_corru_width");
            entity.Property(e => e.PcorCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pcor_cus_cd");
            entity.Property(e => e.PcorCusNa)
                .HasMaxLength(50)
                .HasColumnName("pcor_cus_na");
            entity.Property(e => e.PcorDateInput)
                .HasColumnType("datetime")
                .HasColumnName("pcor_date_input");
            entity.Property(e => e.PcorDeliveryDate).HasColumnName("pcor_delivery_date");
            entity.Property(e => e.PcorDiePlateNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_die_plate_no");
            entity.Property(e => e.PcorFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_flute");
            entity.Property(e => e.PcorGlNa)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_gl_na");
            entity.Property(e => e.PcorGlWeight).HasColumnName("pcor_gl_weight");
            entity.Property(e => e.PcorInk1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_ink_1");
            entity.Property(e => e.PcorInk2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_ink_2");
            entity.Property(e => e.PcorInk3)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_ink_3");
            entity.Property(e => e.PcorInk4)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_ink_4");
            entity.Property(e => e.PcorInk5)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_ink_5");
            entity.Property(e => e.PcorLotNo).HasColumnName("pcor_lot_no");
            entity.Property(e => e.PcorOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("pcor_order_date");
            entity.Property(e => e.PcorOrderNo).HasColumnName("pcor_order_no");
            entity.Property(e => e.PcorPaperCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_paper_cd");
            entity.Property(e => e.PcorPrintPlateNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_print_plate_no");
            entity.Property(e => e.PcorProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pcor_pro_cd");
            entity.Property(e => e.PcorProNa)
                .HasMaxLength(300)
                .HasColumnName("pcor_pro_na");
            entity.Property(e => e.PcorProcess1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_process_1");
            entity.Property(e => e.PcorProcess1Date).HasColumnName("pcor_process_1_date");
            entity.Property(e => e.PcorProcess2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_process_2");
            entity.Property(e => e.PcorProcess2Date).HasColumnName("pcor_process_2_date");
            entity.Property(e => e.PcorProcess3)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_process_3");
            entity.Property(e => e.PcorProcess3Date).HasColumnName("pcor_process_3_date");
            entity.Property(e => e.PcorProcess4)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_process_4");
            entity.Property(e => e.PcorProcess4Date).HasColumnName("pcor_process_4_date");
            entity.Property(e => e.PcorProcess5)
                .HasMaxLength(200)
                .HasColumnName("pcor_process_5");
            entity.Property(e => e.PcorProcess5Date).HasColumnName("pcor_process_5_date");
            entity.Property(e => e.PcorQtyPerCorru)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pcor_qty_per_corru");
            entity.Property(e => e.PcorQtyPerSheet)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("pcor_qty_per_sheet");
            entity.Property(e => e.PcorReceiveNo).HasColumnName("pcor_receive_no");
            entity.Property(e => e.PcorRegularSpeed)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_regular_speed");
            entity.Property(e => e.PcorRemark)
                .HasMaxLength(200)
                .HasColumnName("pcor_remark");
            entity.Property(e => e.PcorRemark2)
                .HasMaxLength(200)
                .HasColumnName("pcor_remark2");
            entity.Property(e => e.PcorScore1)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_score_1");
            entity.Property(e => e.PcorScore2)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_score_2");
            entity.Property(e => e.PcorScore3)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_score_3");
            entity.Property(e => e.PcorScore4)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_score_4");
            entity.Property(e => e.PcorScore5)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_score_5");
            entity.Property(e => e.PcorSheetLen)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_sheet_len");
            entity.Property(e => e.PcorSheetQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pcor_sheet_qty");
            entity.Property(e => e.PcorSheetSqm)
                .HasColumnType("decimal(5, 3)")
                .HasColumnName("pcor_sheet_sqm");
            entity.Property(e => e.PcorSheetWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_sheet_width");
            entity.Property(e => e.PcorSpecialCd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_special_cd");
            entity.Property(e => e.PcorTargetDate).HasColumnName("pcor_target_date");
            entity.Property(e => e.PcorTargetTime)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_target_time");
            entity.Property(e => e.PcorTecmoBar)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pcor_tecmo_bar");
            entity.Property(e => e.PcorTotalLen)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("pcor_total_len");
            entity.Property(e => e.PcorTrimWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("pcor_trim_width");
            entity.Property(e => e.PcorUserId)
                .HasMaxLength(20)
                .HasColumnName("pcor_user_id");
        });

        modelBuilder.Entity<TPoReceivingSituation>(entity =>
        {
            entity.HasKey(e => e.PrsId).HasName("PK__T_PO_Rec__118606A182841FA0");

            entity.ToTable("T_PO_Receiving_Situation");

            entity.Property(e => e.PrsId).HasColumnName("prs_id");
            entity.Property(e => e.PrsConfirmationAfter)
                .HasMaxLength(300)
                .HasColumnName("prs_confirmation_after");
            entity.Property(e => e.PrsCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("prs_cus_cd");
            entity.Property(e => e.PrsDateApply).HasColumnName("prs_date_apply");
            entity.Property(e => e.PrsDateChange)
                .HasColumnType("datetime")
                .HasColumnName("prs_date_change");
            entity.Property(e => e.PrsDateInput)
                .HasColumnType("datetime")
                .HasColumnName("prs_date_input");
            entity.Property(e => e.PrsFinishedAfter)
                .HasMaxLength(300)
                .HasColumnName("prs_finished_after");
            entity.Property(e => e.PrsFinishedBefore)
                .HasMaxLength(300)
                .HasColumnName("prs_finished_before");
            entity.Property(e => e.PrsNotFinishedBefore)
                .HasMaxLength(300)
                .HasColumnName("prs_not_finished_before");
            entity.Property(e => e.PrsUserId)
                .HasMaxLength(20)
                .HasColumnName("prs_user_id");
        });

        modelBuilder.Entity<TProconvertingInput>(entity =>
        {
            entity.HasKey(e => e.ConvCd).HasName("PK__T_PROCon__E990A16EA24570B7");

            entity.ToTable("T_PROConverting_Input");

            entity.Property(e => e.ConvCd).HasColumnName("conv_cd");
            entity.Property(e => e.ConvConvertingDay).HasColumnName("conv_converting_day");
            entity.Property(e => e.ConvCsQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("conv_cs_qty");
            entity.Property(e => e.ConvCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("conv_cus_cd");
            entity.Property(e => e.ConvDateInput)
                .HasColumnType("datetime")
                .HasColumnName("conv_date_input");
            entity.Property(e => e.ConvDeliveryDay).HasColumnName("conv_delivery_day");
            entity.Property(e => e.ConvDept)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_dept");
            entity.Property(e => e.ConvDiePlateNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_die_plate_no");
            entity.Property(e => e.ConvFgInput)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("conv_fg_input");
            entity.Property(e => e.ConvFgOutput)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("conv_fg_output");
            entity.Property(e => e.ConvFinishDay).HasColumnName("conv_finish_day");
            entity.Property(e => e.ConvFinishGood)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_finish_good");
            entity.Property(e => e.ConvFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_flute");
            entity.Property(e => e.ConvInk1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_ink_1");
            entity.Property(e => e.ConvInk2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_ink_2");
            entity.Property(e => e.ConvInk3)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_ink_3");
            entity.Property(e => e.ConvInk4)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_ink_4");
            entity.Property(e => e.ConvInk5)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_ink_5");
            entity.Property(e => e.ConvNotWorkingTime).HasColumnName("conv_not_working_time");
            entity.Property(e => e.ConvOrderNo).HasColumnName("conv_orderNo");
            entity.Property(e => e.ConvOthersTime).HasColumnName("conv_others_time");
            entity.Property(e => e.ConvPaperCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_paper_code");
            entity.Property(e => e.ConvPlanQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("conv_plan_qty");
            entity.Property(e => e.ConvPrintPlateNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("conv_print_plate_no");
            entity.Property(e => e.ConvProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("conv_pro_cd");
            entity.Property(e => e.ConvReceiveNo).HasColumnName("conv_receive_no");
            entity.Property(e => e.ConvRemark)
                .HasMaxLength(100)
                .HasColumnName("conv_remark");
            entity.Property(e => e.ConvRepairTime).HasColumnName("conv_repair_time");
            entity.Property(e => e.ConvSampleTime).HasColumnName("conv_sample_time");
            entity.Property(e => e.ConvSetTime).HasColumnName("conv_set_time");
            entity.Property(e => e.ConvShift).HasColumnName("conv_shift");
            entity.Property(e => e.ConvSqm)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("conv_sqm");
            entity.Property(e => e.ConvUserId)
                .HasMaxLength(20)
                .HasColumnName("conv_user_id");
            entity.Property(e => e.ConvWorkingTime).HasColumnName("conv_working_time");
        });

        modelBuilder.Entity<TProcorrugatorInput>(entity =>
        {
            entity.HasKey(e => e.CorrCd).HasName("PK__T_PROCor__33CFC909B7A07CC4");

            entity.ToTable("T_PROCorrugator_Input");

            entity.Property(e => e.CorrCd).HasColumnName("corr_cd");
            entity.Property(e => e.CorrCorDate).HasColumnName("corr_cor_date");
            entity.Property(e => e.CorrCorWeight)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_cor_weight");
            entity.Property(e => e.CorrCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("corr_cus_cd");
            entity.Property(e => e.CorrDateInput)
                .HasColumnType("datetime")
                .HasColumnName("corr_date_input");
            entity.Property(e => e.CorrDay).HasColumnName("corr_day");
            entity.Property(e => e.CorrFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("corr_flute");
            entity.Property(e => e.CorrHscLoss)
                .HasMaxLength(10)
                .HasColumnName("corr_hsc_loss");
            entity.Property(e => e.CorrHscWeight)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_hsc_weight");
            entity.Property(e => e.CorrLotNo).HasColumnName("corr_lotNo");
            entity.Property(e => e.CorrOrderNo).HasColumnName("corr_orderNo");
            entity.Property(e => e.CorrOutputFgCase)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_output_fg_case");
            entity.Property(e => e.CorrOutputFgScon)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_output_fg_scon");
            entity.Property(e => e.CorrOutputFgSheet)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_output_fg_sheet");
            entity.Property(e => e.CorrPaperCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("corr_paper_code");
            entity.Property(e => e.CorrProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_pro_cd");
            entity.Property(e => e.CorrRealCorruWid)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_real_corru_wid");
            entity.Property(e => e.CorrRealQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_real_qty");
            entity.Property(e => e.CorrRealQtyPerSheet)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("corr_real_qty_per_sheet");
            entity.Property(e => e.CorrReceiveNo).HasColumnName("corr_receive_no");
            entity.Property(e => e.CorrRemark)
                .HasMaxLength(100)
                .HasColumnName("corr_remark");
            entity.Property(e => e.CorrSheetBoard)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("corr_sheet_board");
            entity.Property(e => e.CorrTotalM)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_total_m");
            entity.Property(e => e.CorrTotalSqm)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_total_sqm");
            entity.Property(e => e.CorrTrimWeight)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_trim_weight");
            entity.Property(e => e.CorrUnitAbility)
                .HasColumnType("decimal(5, 4)")
                .HasColumnName("corr_unit_ability");
            entity.Property(e => e.CorrUnitWeight)
                .HasColumnType("decimal(4, 3)")
                .HasColumnName("corr_unit_weight");
            entity.Property(e => e.CorrUserId)
                .HasMaxLength(20)
                .HasColumnName("corr_user_id");
            entity.Property(e => e.CorrWidthLoss)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corr_width_loss");
            entity.Property(e => e.CorruKind)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("corru_kind");
            entity.Property(e => e.CorruRealSheetLen)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corru_real_sheet_len");
            entity.Property(e => e.CorruRealSheetWidth)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("corru_real_sheet_width");
        });

        modelBuilder.Entity<TQcRepairProduct>(entity =>
        {
            entity.HasKey(e => e.QcrpId).HasName("PK__T_QC_Rep__074B1F37589D70E8");

            entity.ToTable("T_QC_RepairProduct", tb => tb.HasTrigger("RepairProductUpdateTrigger"));

            entity.Property(e => e.QcrpId).HasColumnName("qcrp_id");
            entity.Property(e => e.QcrpBackloadQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("qcrp_backload_qty");
            entity.Property(e => e.QcrpDateChange)
                .HasColumnType("datetime")
                .HasColumnName("qcrp_date_change");
            entity.Property(e => e.QcrpDateInput)
                .HasColumnType("datetime")
                .HasColumnName("qcrp_date_input");
            entity.Property(e => e.QcrpNgQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("qcrp_ng_qty");
            entity.Property(e => e.QcrpOkQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("qcrp_ok_qty");
            entity.Property(e => e.QcrpProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("qcrp_pro_cd");
            entity.Property(e => e.QcrpProName)
                .HasMaxLength(150)
                .HasColumnName("qcrp_pro_name");
            entity.Property(e => e.QcrpRealQtyReceived)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("qcrp_real_qty_received");
            entity.Property(e => e.QcrpReceiveNo)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("qcrp_receive_no");
            entity.Property(e => e.QcrpReceivedDate).HasColumnName("qcrp_received_date");
            entity.Property(e => e.QcrpRemark)
                .HasMaxLength(200)
                .HasColumnName("qcrp_remark");
            entity.Property(e => e.QcrpUserId)
                .HasMaxLength(20)
                .HasColumnName("qcrp_user_id");
        });

        modelBuilder.Entity<TSpecialCustomerPoManage>(entity =>
        {
            entity.HasKey(e => e.ScpmId).HasName("PK__T_Specia__594109E53F4F9A17");

            entity.ToTable("T_Special_Customer_PO_Manage");

            entity.Property(e => e.ScpmId).HasColumnName("scpm_id");
            entity.Property(e => e.ScpmDateChange)
                .HasColumnType("datetime")
                .HasColumnName("scpm_date_change");
            entity.Property(e => e.ScpmDateInput)
                .HasColumnType("datetime")
                .HasColumnName("scpm_date_input");
            entity.Property(e => e.ScpmOrderQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("scpm_order_qty");
            entity.Property(e => e.ScpmPo)
                .HasMaxLength(200)
                .HasColumnName("scpm_po");
            entity.Property(e => e.ScpmProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("scpm_pro_cd");
            entity.Property(e => e.ScpmProName)
                .HasMaxLength(150)
                .HasColumnName("scpm_pro_name");
            entity.Property(e => e.ScpmRemark)
                .HasMaxLength(300)
                .HasColumnName("scpm_remark");
            entity.Property(e => e.ScpmSpecialIndication)
                .HasMaxLength(5)
                .HasColumnName("scpm_special_indication");
            entity.Property(e => e.ScpmUserId)
                .HasMaxLength(20)
                .HasColumnName("scpm_user_id");
        });

        modelBuilder.Entity<TSubMaterialExport>(entity =>
        {
            entity.HasKey(e => e.SmeDeliverySlip);

            entity.ToTable("T_SubMaterialExport", tb => tb.HasComment("T_SubMaterialExport"));

            entity.Property(e => e.SmeDeliverySlip)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("sme_delivery_slip")
                .HasColumnName("sme_delivery_slip");
            entity.Property(e => e.SmeCarType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("sme_car_type")
                .HasColumnName("sme_car_type");
            entity.Property(e => e.SmeDateChanged)
                .HasComment("sme_date_changed")
                .HasColumnType("datetime")
                .HasColumnName("sme_date_changed");
            entity.Property(e => e.SmeDateInput)
                .HasComment("sme_date_input")
                .HasColumnType("datetime")
                .HasColumnName("sme_date_input");
            entity.Property(e => e.SmeDeliveryAddress)
                .HasMaxLength(200)
                .HasComment("sme_delivery_address")
                .HasColumnName("sme_delivery_address");
            entity.Property(e => e.SmeDeliveryDate)
                .HasComment("sme_delivery_date")
                .HasColumnType("datetime")
                .HasColumnName("sme_delivery_date");
            entity.Property(e => e.SmeDestinationCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("sme_destination_code")
                .HasColumnName("sme_destination_code");
            entity.Property(e => e.SmeDestinationName)
                .HasMaxLength(100)
                .HasComment("sme_destination_name")
                .HasColumnName("sme_destination_name");
            entity.Property(e => e.SmeNote)
                .HasMaxLength(200)
                .HasComment("sme_note")
                .HasColumnName("sme_note");
            entity.Property(e => e.SmeTransportCompany)
                .HasMaxLength(50)
                .HasComment("sme_transport_company")
                .HasColumnName("sme_transport_company");
            entity.Property(e => e.SmeTruckNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("sme_truck_number")
                .HasColumnName("sme_truck_number");
            entity.Property(e => e.SmeUserIdChanged)
                .HasMaxLength(20)
                .HasComment("sme_user_id_changed")
                .HasColumnName("sme_user_id_changed");
            entity.Property(e => e.SmeUserIdInput)
                .HasMaxLength(20)
                .HasComment("sme_user_id_input")
                .HasColumnName("sme_user_id_input");
        });

        modelBuilder.Entity<TSubMaterialExportDetail>(entity =>
        {
            entity.HasKey(e => new { e.SmedDeliverySlip, e.SmedCode }).HasName("PK__T_SubMat__10260519AF79D916");

            entity.ToTable("T_SubMaterialExportDetail");

            entity.Property(e => e.SmedDeliverySlip)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("smed_delivery_slip");
            entity.Property(e => e.SmedCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("smed_code");
            entity.Property(e => e.SmedName)
                .HasMaxLength(100)
                .HasColumnName("smed_name");
            entity.Property(e => e.SmedOtherNote)
                .HasMaxLength(200)
                .HasColumnName("smed_other_note");
            entity.Property(e => e.SmedQuantity)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("smed_quantity");
            entity.Property(e => e.SmedRemark)
                .HasMaxLength(200)
                .HasColumnName("smed_remark");
            entity.Property(e => e.SmedUnit)
                .HasMaxLength(20)
                .HasColumnName("smed_unit");
        });

        modelBuilder.Entity<TSubMaterialMaster>(entity =>
        {
            entity.HasKey(e => e.SmmCode).HasName("PK__T_SubMat__8E21A97C7BBFA789");

            entity.ToTable("T_SubMaterialMaster");

            entity.Property(e => e.SmmCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("smm_code");
            entity.Property(e => e.SmmDateChanged)
                .HasColumnType("datetime")
                .HasColumnName("smm_date_changed");
            entity.Property(e => e.SmmDateInput)
                .HasColumnType("datetime")
                .HasColumnName("smm_date_input");
            entity.Property(e => e.SmmName)
                .HasMaxLength(100)
                .HasColumnName("smm_name");
            entity.Property(e => e.SmmNote)
                .HasMaxLength(200)
                .HasColumnName("smm_note");
            entity.Property(e => e.SmmPicName)
                .HasMaxLength(20)
                .HasColumnName("smm_pic_name");
            entity.Property(e => e.SmmPicPhone)
                .HasMaxLength(20)
                .HasColumnName("smm_pic_phone");
            entity.Property(e => e.SmmRemark)
                .HasMaxLength(200)
                .HasColumnName("smm_remark");
            entity.Property(e => e.SmmUnit)
                .HasMaxLength(20)
                .HasColumnName("smm_unit");
            entity.Property(e => e.SmmUserIdChanged)
                .HasMaxLength(20)
                .HasColumnName("smm_user_id_changed");
            entity.Property(e => e.SmmUserIdInput)
                .HasMaxLength(20)
                .HasColumnName("smm_user_id_input");
        });

        modelBuilder.Entity<TSubconReceive>(entity =>
        {
            entity.HasKey(e => e.SrId).HasName("PK__T_Subcon__5C9E98B9C6EDBF39");

            entity.ToTable("T_SubconReceive");

            entity.Property(e => e.SrId).HasColumnName("sr_id");
            entity.Property(e => e.SrDate).HasColumnName("sr_date");
            entity.Property(e => e.SrDateInput)
                .HasColumnType("datetime")
                .HasColumnName("sr_date_input");
            entity.Property(e => e.SrProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("sr_pro_cd");
            entity.Property(e => e.SrQuantity)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("sr_quantity");
            entity.Property(e => e.SrReceiveNo).HasColumnName("sr_receive_no");
            entity.Property(e => e.SrRemark)
                .HasMaxLength(200)
                .HasColumnName("sr_remark");
            entity.Property(e => e.SrSubconName)
                .HasMaxLength(20)
                .HasColumnName("sr_subcon_name");
            entity.Property(e => e.SrUserId)
                .HasMaxLength(20)
                .HasColumnName("sr_user_id");
        });

        modelBuilder.Entity<TSubconReport>(entity =>
        {
            entity.HasKey(e => e.ScrId).HasName("PK__T_Subcon__C92AD270531F4F82");

            entity.ToTable("T_SubconReport", tb => tb.HasTrigger("SubconReportUpdateTrigger"));

            entity.Property(e => e.ScrId).HasColumnName("scr_id");
            entity.Property(e => e.ScrDate).HasColumnName("scr_date");
            entity.Property(e => e.ScrDateInput)
                .HasColumnType("datetime")
                .HasColumnName("scr_date_input");
            entity.Property(e => e.ScrProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("scr_pro_cd");
            entity.Property(e => e.ScrProductQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("scr_product_qty");
            entity.Property(e => e.ScrReceiveNo).HasColumnName("scr_receive_no");
            entity.Property(e => e.ScrRemark)
                .HasMaxLength(200)
                .HasColumnName("scr_remark");
            entity.Property(e => e.ScrSubconName)
                .HasMaxLength(20)
                .HasColumnName("scr_subcon_name");
            entity.Property(e => e.ScrUserId)
                .HasMaxLength(20)
                .HasColumnName("scr_user_id");
        });

        modelBuilder.Entity<TSupplier>(entity =>
        {
            entity.HasKey(e => e.SupCusCd);

            entity.ToTable("T_Supplier", tb => tb.HasComment("T_Supplier"));

            entity.Property(e => e.SupCusCd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("sup_cus_cd")
                .HasColumnName("sup_cus_cd");
            entity.Property(e => e.SupAddress)
                .HasMaxLength(300)
                .HasComment("sup_address")
                .HasColumnName("sup_address");
            entity.Property(e => e.SupBankAccount)
                .HasMaxLength(50)
                .HasComment("sup_bank_account")
                .HasColumnName("sup_bank_account");
            entity.Property(e => e.SupBankName)
                .HasMaxLength(100)
                .HasComment("sup_bank_name")
                .HasColumnName("sup_bank_name");
            entity.Property(e => e.SupContactName)
                .HasMaxLength(50)
                .HasComment("sup_contactName")
                .HasColumnName("sup_contactName");
            entity.Property(e => e.SupDateInput)
                .HasComment("sup_date_input")
                .HasColumnType("datetime")
                .HasColumnName("sup_date_input");
            entity.Property(e => e.SupFax)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("sup_fax")
                .HasColumnName("sup_fax");
            entity.Property(e => e.SupFullName)
                .HasMaxLength(100)
                .HasComment("sup_full_name")
                .HasColumnName("sup_full_name");
            entity.Property(e => e.SupPhone)
                .HasMaxLength(15)
                .HasComment("sup_phone")
                .HasColumnName("sup_phone");
            entity.Property(e => e.SupRemark)
                .HasMaxLength(200)
                .HasComment("sup_remark")
                .HasColumnName("sup_remark");
            entity.Property(e => e.SupTaxCd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("sup_tax_cd")
                .HasColumnName("sup_tax_cd");
            entity.Property(e => e.SupUserId)
                .HasMaxLength(20)
                .HasComment("sup_user_id")
                .HasColumnName("sup_user_id");
        });

        modelBuilder.Entity<TSystemState>(entity =>
        {
            entity.HasKey(e => e.StStateId).HasName("PK__T_System__DC96949885A14AAF");

            entity.ToTable("T_System_State");

            entity.Property(e => e.StStateId).HasColumnName("st_state_id");
            entity.Property(e => e.StDateInput)
                .HasColumnType("datetime")
                .HasColumnName("st_date_input");
            entity.Property(e => e.StFunctionId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("st_functionID");
            entity.Property(e => e.StFunctionName)
                .HasMaxLength(50)
                .HasColumnName("st_functionName");
            entity.Property(e => e.StFunctionState)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("st_functionState");
            entity.Property(e => e.StRemark)
                .HasMaxLength(200)
                .HasColumnName("st_remark");
            entity.Property(e => e.StSection)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("st_section");
            entity.Property(e => e.StUserId)
                .HasMaxLength(20)
                .HasColumnName("st_user_id");
        });

        modelBuilder.Entity<TTransportCompany>(entity =>
        {
            entity.HasKey(e => e.TcId).HasName("PK__T_Transp__E61FEFB0A14CBF58");

            entity.ToTable("T_TransportCompany");

            entity.Property(e => e.TcId).HasColumnName("tc_id");
            entity.Property(e => e.TcAddress)
                .HasMaxLength(200)
                .HasColumnName("tc_address");
            entity.Property(e => e.TcDateInput)
                .HasColumnType("datetime")
                .HasColumnName("tc_date_input");
            entity.Property(e => e.TcFullName)
                .HasMaxLength(100)
                .HasColumnName("tc_fullName");
            entity.Property(e => e.TcName)
                .HasMaxLength(20)
                .HasColumnName("tc_name");
            entity.Property(e => e.TcPhone)
                .HasMaxLength(20)
                .HasColumnName("tc_phone");
            entity.Property(e => e.TcUserId)
                .HasMaxLength(20)
                .HasColumnName("tc_user_id");
        });

        modelBuilder.Entity<TVolumeEstimate>(entity =>
        {
            entity.HasKey(e => e.VeId).HasName("PK__T_Volume__3EBE499F70EC42D8");

            entity.ToTable("T_Volume_Estimate");

            entity.Property(e => e.VeId).HasColumnName("ve_id");
            entity.Property(e => e.VeDateApply).HasColumnName("ve_date_apply");
            entity.Property(e => e.VeDateInput)
                .HasColumnType("datetime")
                .HasColumnName("ve_date_input");
            entity.Property(e => e.VeRemark)
                .HasMaxLength(300)
                .HasColumnName("ve_remark");
            entity.Property(e => e.VeUserId)
                .HasMaxLength(20)
                .HasColumnName("ve_user_id");
            entity.Property(e => e.VeVolume)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("ve_volume");
        });

        modelBuilder.Entity<TWarehouseNg>(entity =>
        {
            entity.HasKey(e => e.WnId).HasName("PK__T_Wareho__26FAE7EC92BA23BA");

            entity.ToTable("T_Warehouse_NG", tb => tb.HasTrigger("WarehouseNGUpdateTrigger"));

            entity.Property(e => e.WnId).HasColumnName("wn_id");
            entity.Property(e => e.WnCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("wn_cus_cd");
            entity.Property(e => e.WnDateInput)
                .HasColumnType("datetime")
                .HasColumnName("wn_date_input");
            entity.Property(e => e.WnDateReport).HasColumnName("wn_date_report");
            entity.Property(e => e.WnPic)
                .HasMaxLength(20)
                .HasColumnName("wn_pic");
            entity.Property(e => e.WnProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("wn_pro_cd");
            entity.Property(e => e.WnQuantity)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("wn_quantity");
            entity.Property(e => e.WnReceiveNo).HasColumnName("wn_receive_no");
            entity.Property(e => e.WnRemark)
                .HasMaxLength(200)
                .HasColumnName("wn_remark");
            entity.Property(e => e.WnUserId)
                .HasMaxLength(20)
                .HasColumnName("wn_user_id");
        });

        modelBuilder.Entity<TWarehousePaperRoll>(entity =>
        {
            entity.HasKey(e => e.WpKey).HasName("PK__T_Wareho__0A312A42BEC22B0D");

            entity.ToTable("T_Warehouse_PaperRoll");

            entity.Property(e => e.WpKey).HasColumnName("wp_key");
            entity.Property(e => e.WpDateInput)
                .HasColumnType("datetime")
                .HasColumnName("wp_date_input");
            entity.Property(e => e.WpGradeName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("wp_grade_name");
            entity.Property(e => e.WpGramage)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("wp_gramage");
            entity.Property(e => e.WpInput)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("wp_input");
            entity.Property(e => e.WpLogiCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("wp_logi_code");
            entity.Property(e => e.WpMetre)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("wp_metre");
            entity.Property(e => e.WpReceiveDate).HasColumnName("wp_receive_date");
            entity.Property(e => e.WpRemain)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("wp_remain");
            entity.Property(e => e.WpRemark)
                .HasMaxLength(200)
                .HasColumnName("wp_remark");
            entity.Property(e => e.WpType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("wp_type");
            entity.Property(e => e.WpUse)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("wp_use");
            entity.Property(e => e.WpUserId)
                .HasMaxLength(20)
                .HasColumnName("wp_user_id");
            entity.Property(e => e.WpWidth)
                .HasColumnType("decimal(4, 0)")
                .HasColumnName("wp_width");
        });

        modelBuilder.Entity<VtImportConvertingFromPlan>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VT_importConvertingFromPlan");

            entity.Property(e => e.CrConvertingDate).HasColumnName("cr_converting_date");
            entity.Property(e => e.CrCsQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_cs_qty");
            entity.Property(e => e.CrCusCd)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("cr_cus_cd");
            entity.Property(e => e.CrDeliveryDate).HasColumnName("cr_delivery_date");
            entity.Property(e => e.CrDept)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cr_dept");
            entity.Property(e => e.CrFinishDate).HasColumnName("cr_finish_date");
            entity.Property(e => e.CrFlute)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("cr_flute");
            entity.Property(e => e.CrOrderNo).HasColumnName("cr_order_no");
            entity.Property(e => e.CrPaperCode)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_paper_code");
            entity.Property(e => e.CrPlanQty)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_plan_qty");
            entity.Property(e => e.CrProCd)
                .HasColumnType("decimal(7, 0)")
                .HasColumnName("cr_pro_cd");
            entity.Property(e => e.CrReceiveNo).HasColumnName("cr_receive_no");
            entity.Property(e => e.CrSqm)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("cr_sqm");
        });
        modelBuilder.HasSequence<int>("AcceptanceMonthCounter")
            .HasMin(1L)
            .HasMax(9999L)
            .IsCyclic();
        modelBuilder.HasSequence("BkDeliveryNoteID")
            .StartsAt(0L)
            .HasMax(999999999999999999L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("DeliveryNoteAutoID")
            .StartsAt(50001L)
            .HasMin(1L)
            .IsCyclic();
        modelBuilder.HasSequence<int>("ReceiveNoCounter")
            .HasMin(1L)
            .HasMax(999998L)
            .IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
