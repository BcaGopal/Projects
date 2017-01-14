namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonSettings : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Web.JobReceiveQALineDetails", newName: "JobReceiveQALineExtendeds");
            DropIndex("Web.MaterialPlanHeaders", new[] { "DocTypeId" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "DivisionId" });
            DropIndex("Web.MaterialPlanHeaders", new[] { "SiteId" });
            DropIndex("Web.SaleOrderCancelHeaders", new[] { "DocTypeId" });
            DropIndex("Web.SaleOrderCancelHeaders", new[] { "DivisionId" });
            DropIndex("Web.SaleOrderCancelHeaders", new[] { "SiteId" });
            CreateTable(
                "Web.Dimension3",
                c => new
                    {
                        Dimension3Id = c.Int(nullable: false, identity: true),
                        Dimension3Name = c.String(nullable: false, maxLength: 50),
                        ProductTypeId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 50),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Dimension3Id)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.Dimension3Name, unique: true, name: "IX_Dimension3_Dimension3Name")
                .Index(t => t.ProductTypeId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.Dimension4",
                c => new
                    {
                        Dimension4Id = c.Int(nullable: false, identity: true),
                        Dimension4Name = c.String(nullable: false, maxLength: 50),
                        ProductTypeId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 50),
                        ReferenceDocTypeId = c.Int(),
                        ReferenceDocId = c.Int(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Dimension4Id)
                .ForeignKey("Web.ProductTypes", t => t.ProductTypeId)
                .ForeignKey("Web.DocumentTypes", t => t.ReferenceDocTypeId)
                .Index(t => t.Dimension4Name, unique: true, name: "IX_Dimension4_Dimension4Name")
                .Index(t => t.ProductTypeId)
                .Index(t => t.ReferenceDocTypeId);
            
            CreateTable(
                "Web.Dimension3Types",
                c => new
                    {
                        Dimension3TypeId = c.Int(nullable: false, identity: true),
                        Dimension3TypeName = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Dimension3TypeId)
                .Index(t => t.Dimension3TypeName, unique: true, name: "IX_Dimension3Type_Dimension3TypeName");
            
            CreateTable(
                "Web.Dimension4Types",
                c => new
                    {
                        Dimension4TypeId = c.Int(nullable: false, identity: true),
                        Dimension4TypeName = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Dimension4TypeId)
                .Index(t => t.Dimension4TypeName, unique: true, name: "IX_Dimension4Type_Dimension4TypeName");
            
            CreateTable(
                "Web.PackingLineExtendeds",
                c => new
                    {
                        PackingLineId = c.Int(nullable: false),
                        Length = c.Decimal(precision: 18, scale: 4),
                        Width = c.Decimal(precision: 18, scale: 4),
                        Height = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.PackingLineId)
                .ForeignKey("Web.PackingLines", t => t.PackingLineId)
                .Index(t => t.PackingLineId);
            
            CreateTable(
                "Web.PersonSettings",
                c => new
                    {
                        PersonSettingsId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        isVisibleAddress = c.Boolean(),
                        isVisibleCity = c.Boolean(),
                        isVisibleZipCode = c.Boolean(),
                        isVisiblePhone = c.Boolean(),
                        isVisibleMobile = c.Boolean(),
                        isVisibleEMail = c.Boolean(),
                        isVisibleCstNo = c.Boolean(),
                        isVisibleTinNo = c.Boolean(),
                        isVisiblePanNo = c.Boolean(),
                        isVisibleSalesTaxGroup = c.Boolean(),
                        isVisibleGuarantor = c.Boolean(),
                        isVisibleParent = c.Boolean(),
                        isVisibleTdsCategory = c.Boolean(),
                        isVisibleTdsGroup = c.Boolean(),
                        isVisibleWorkInDivision = c.Boolean(),
                        isVisibleWorkInBranch = c.Boolean(),
                        isVisibleTags = c.Boolean(),
                        isVisibleIsSisterConcern = c.Boolean(),
                        isVisibleContactPersonDetail = c.Boolean(),
                        isVisibleBankAccountDetail = c.Boolean(),
                        isVisiblePersonProcessDetail = c.Boolean(),
                        AccountGroupId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PersonSettingsId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .Index(t => t.DocTypeId);
            
            AddColumn("Web.People", "DocTypeId", c => c.Int(nullable: false));
            AddColumn("Web.GatePassHeaders", "ReviewCount", c => c.Int());
            AddColumn("Web.GatePassHeaders", "ReviewBy", c => c.String());
            AddColumn("Web.GatePassHeaders", "OrderById", c => c.Int());
            AddColumn("Web.ProdOrderLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.ProdOrderLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.MaterialPlanLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.MaterialPlanLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.StockLines", "StockInId", c => c.Int());
            AddColumn("Web.SaleOrderLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.SaleOrderLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.SaleOrderLines", "StockId", c => c.Int());
            AddColumn("Web.SaleOrderHeaders", "GodownId", c => c.Int());
            AddColumn("Web.PurchaseIndentLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.PurchaseIndentLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.BomDetails", "Dimension3Id", c => c.Int());
            AddColumn("Web.BomDetails", "Dimension4Id", c => c.Int());
            AddColumn("Web.BomDetails", "MBQ", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.BomDetails", "StdCost", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.BomDetails", "StdTime", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.CalculationFooters", "IncludedCharges", c => c.String());
            AddColumn("Web.CalculationFooters", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.CalculationProducts", "IncludedCharges", c => c.String());
            AddColumn("Web.CalculationProducts", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.CarpetSkuSettings", "isVisibleTraceType", c => c.Boolean());
            AddColumn("Web.CarpetSkuSettings", "isVisibleMapType", c => c.Boolean());
            AddColumn("Web.CarpetSkuSettings", "isVisibleStencilSize", c => c.Boolean());
            AddColumn("Web.CarpetSkuSettings", "PerimeterSizeTypeId", c => c.Int());
            AddColumn("Web.DocumentTypeSettings", "ProductUidCaption", c => c.String(maxLength: 50));
            AddColumn("Web.DocumentTypeSettings", "PartyCaption", c => c.String(maxLength: 50));
            AddColumn("Web.DocumentTypeSettings", "ContraDocTypeCaption", c => c.String(maxLength: 50));
            AddColumn("Web.ExcessMaterialSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.ExcessMaterialSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.JobConsumptionSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.JobConsumptionSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.JobInvoiceAmendmentHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.JobInvoiceAmendmentHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.JobInvoiceHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.JobInvoiceHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.JobInvoiceLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.JobInvoiceLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.JobInvoiceRateAmendmentLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.JobInvoiceRateAmendmentLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.JobInvoiceReturnHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.JobInvoiceReturnHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.JobInvoiceReturnLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.JobInvoiceReturnLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.JobInvoiceSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.JobInvoiceSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.JobOrderHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.JobOrderHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.JobOrderInspectionRequestSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.JobOrderInspectionRequestSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.JobOrderInspectionSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.JobOrderInspectionSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.JobOrderLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.JobOrderLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.JobOrderSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.JobOrderSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.QAGroups", "ReviewCount", c => c.Int());
            AddColumn("Web.QAGroups", "ReviewBy", c => c.String());
            AddColumn("Web.JobReceiveQASettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "IsVisibleLength", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "IsVisibleWidth", c => c.Boolean());
            AddColumn("Web.JobReceiveQASettings", "IsVisibleHeight", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleCostCenter", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleConsumptionDetail", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleByProductDetail", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "LossPer", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.JobReceiveSettings", "ConsumptionProductCaption", c => c.String(maxLength: 50));
            AddColumn("Web.JobReceiveSettings", "ConsumptionDimension1Caption", c => c.String(maxLength: 50));
            AddColumn("Web.JobReceiveSettings", "ConsumptionDimension2Caption", c => c.String(maxLength: 50));
            AddColumn("Web.JobReceiveSettings", "ByProductCaption", c => c.String(maxLength: 50));
            AddColumn("Web.JobReceiveSettings", "ByProductDimension1Caption", c => c.String(maxLength: 50));
            AddColumn("Web.JobReceiveSettings", "ByProductDimension2Caption", c => c.String(maxLength: 50));
            AddColumn("Web.JobReceiveSettings", "isVisibleConsumptionDimension1", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleConsumptionDimension2", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleByProductDimension1", c => c.Boolean());
            AddColumn("Web.JobReceiveSettings", "isVisibleByProductDimension2", c => c.Boolean());
            AddColumn("Web.StockHeaderSettings", "isVisibleProductCode", c => c.Boolean());
            AddColumn("Web.StockHeaderSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.StockHeaderSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.StockHeaderSettings", "isVisibleStockIn", c => c.Boolean());
            AddColumn("Web.MaterialPlanForProdOrderLines", "Dimension3Id", c => c.Int());
            AddColumn("Web.MaterialPlanForProdOrderLines", "Dimension4Id", c => c.Int());
            AddColumn("Web.MaterialPlanSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.MaterialPlanSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.MaterialReceiveSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.MaterialReceiveSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.MaterialRequestSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.MaterialRequestSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.PackingSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.PackingSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.PersonBankAccounts", "AccountHolderName", c => c.String(maxLength: 100));
            AddColumn("Web.PersonRegistrations", "RegistrationDate", c => c.DateTime());
            AddColumn("Web.PersonRegistrations", "ExpiryDate", c => c.DateTime());
            AddColumn("Web.ProdOrderSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.ProdOrderSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.ProductionOrderSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.ProductionOrderSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.ProductProcesses", "Dimension3Id", c => c.Int());
            AddColumn("Web.ProductProcesses", "Dimension4Id", c => c.Int());
            AddColumn("Web.PurchaseGoodsReceiptSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.PurchaseGoodsReceiptSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.PurchaseIndentSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.PurchaseIndentSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.PurchaseInvoiceHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseInvoiceHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseInvoiceLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseInvoiceLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseInvoiceReturnHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseInvoiceReturnHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseInvoiceReturnLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseInvoiceReturnLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseInvoiceSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.PurchaseInvoiceSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.PurchaseOrderAmendmentHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseOrderAmendmentHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseOrderHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseOrderHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseOrderInspectionRequestSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.PurchaseOrderInspectionRequestSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.PurchaseOrderInspectionSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.PurchaseOrderInspectionSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.PurchaseOrderLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseOrderLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseOrderRateAmendmentLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseOrderRateAmendmentLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseOrderSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.PurchaseOrderSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.PurchaseQuotationHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseQuotationHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseQuotationLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.PurchaseQuotationLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.PurchaseQuotationSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.PurchaseQuotationSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.RateConversionSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.RateConversionSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.RequisitionSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.RequisitionSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.SaleDeliveryOrderSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.SaleDeliveryOrderSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.SaleDispatchSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.SaleDispatchSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.SaleEnquiryLineExtendeds", "BuyerSpecification", c => c.String());
            AddColumn("Web.SaleEnquiryLineExtendeds", "BuyerSpecification1", c => c.String());
            AddColumn("Web.SaleEnquiryLineExtendeds", "BuyerSpecification2", c => c.String());
            AddColumn("Web.SaleEnquiryLineExtendeds", "BuyerSpecification3", c => c.String());
            AddColumn("Web.SaleEnquirySettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.SaleEnquirySettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.SaleEnquirySettings", "isVisibleRate", c => c.Boolean());
            AddColumn("Web.SaleEnquirySettings", "isVisibleBillToParty", c => c.Boolean());
            AddColumn("Web.SaleInvoiceHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.SaleInvoiceHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.SaleInvoiceLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.SaleInvoiceLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.SaleInvoiceReturnHeaderCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.SaleInvoiceReturnHeaderCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.SaleInvoiceReturnLineCharges", "IncludedCharges", c => c.String());
            AddColumn("Web.SaleInvoiceReturnLineCharges", "IncludedChargesCalculation", c => c.String());
            AddColumn("Web.SaleInvoiceSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.SaleInvoiceSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.SaleOrderSettings", "isVisibleDimension3", c => c.Boolean());
            AddColumn("Web.SaleOrderSettings", "isVisibleDimension4", c => c.Boolean());
            AddColumn("Web.SaleOrderSettings", "isVisibleBillToParty", c => c.Boolean());
            AddColumn("Web.SaleOrderSettings", "isVisibleGodown", c => c.Boolean());
            AddColumn("Web.SaleOrderSettings", "isPostedInStock", c => c.Boolean());
            AddColumn("Web.ViewProductBuyer", "BuyerSku", c => c.String());
            AddColumn("Web.ViewProductBuyer", "BuyerSpecification", c => c.String());
            AddColumn("Web.ViewProductBuyer", "BuyerSpecification1", c => c.String());
            AddColumn("Web.ViewProductBuyer", "BuyerSpecification2", c => c.String());
            AddColumn("Web.ViewProductBuyer", "BuyerSpecification3", c => c.String());
            AlterColumn("Web.CalculationFooters", "AddDeduct", c => c.Byte());
            AlterColumn("Web.CalculationProducts", "AddDeduct", c => c.Byte());
            AlterColumn("Web.JobInvoiceAmendmentHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.JobInvoiceHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.JobInvoiceLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.JobInvoiceRateAmendmentLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.JobInvoiceReturnHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.JobInvoiceReturnLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.JobOrderHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.JobOrderLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseInvoiceHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseInvoiceLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseInvoiceReturnHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseInvoiceReturnLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseOrderAmendmentHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseOrderHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseOrderLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseOrderRateAmendmentLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseQuotationHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.PurchaseQuotationLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.SaleInvoiceHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.SaleInvoiceLineCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.SaleInvoiceReturnHeaderCharges", "AddDeduct", c => c.Byte());
            AlterColumn("Web.SaleInvoiceReturnLineCharges", "AddDeduct", c => c.Byte());
            CreateIndex("Web.People", "DocTypeId");
            CreateIndex("Web.GatePassHeaders", "OrderById");
            CreateIndex("Web.ProdOrderLines", "Dimension3Id");
            CreateIndex("Web.ProdOrderLines", "Dimension4Id");
            CreateIndex("Web.MaterialPlanLines", "Dimension3Id");
            CreateIndex("Web.MaterialPlanLines", "Dimension4Id");
            CreateIndex("Web.MaterialPlanHeaders", new[] { "DocTypeId", "DocDate", "DivisionId", "SiteId" }, unique: true, name: "IX_MaterialPlanHeader_DocID");
            CreateIndex("Web.StockLines", "StockInId");
            CreateIndex("Web.SaleOrderLines", "Dimension3Id");
            CreateIndex("Web.SaleOrderLines", "Dimension4Id");
            CreateIndex("Web.SaleOrderLines", "StockId");
            CreateIndex("Web.SaleOrderHeaders", "GodownId");
            CreateIndex("Web.PurchaseIndentLines", "Dimension3Id");
            CreateIndex("Web.PurchaseIndentLines", "Dimension4Id");
            CreateIndex("Web.BomDetails", "Dimension3Id");
            CreateIndex("Web.BomDetails", "Dimension4Id");
            CreateIndex("Web.CarpetSkuSettings", "PerimeterSizeTypeId");
            CreateIndex("Web.MaterialPlanForProdOrderLines", "Dimension3Id");
            CreateIndex("Web.MaterialPlanForProdOrderLines", "Dimension4Id");
            CreateIndex("Web.ProductProcesses", "Dimension3Id");
            CreateIndex("Web.ProductProcesses", "Dimension4Id");
            CreateIndex("Web.SaleOrderCancelHeaders", new[] { "DocTypeId", "DocNo", "DivisionId", "SiteId" }, unique: true, name: "IX_SaleOrderCancelHeader_DocID");
            AddForeignKey("Web.People", "DocTypeId", "Web.DocumentTypes", "DocumentTypeId");
            AddForeignKey("Web.GatePassHeaders", "OrderById", "Web.Employees", "PersonID");
            AddForeignKey("Web.ProdOrderLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.ProdOrderLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.MaterialPlanLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.MaterialPlanLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.StockLines", "StockInId", "Web.Stocks", "StockId");
            AddForeignKey("Web.SaleOrderLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.SaleOrderLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.SaleOrderHeaders", "GodownId", "Web.Godowns", "GodownId");
            AddForeignKey("Web.SaleOrderLines", "StockId", "Web.Stocks", "StockId");
            AddForeignKey("Web.PurchaseIndentLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.PurchaseIndentLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.BomDetails", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.BomDetails", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.CarpetSkuSettings", "PerimeterSizeTypeId", "Web.ProductSizeTypes", "ProductSizeTypeId");
            AddForeignKey("Web.MaterialPlanForProdOrderLines", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.MaterialPlanForProdOrderLines", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            AddForeignKey("Web.ProductProcesses", "Dimension3Id", "Web.Dimension3", "Dimension3Id");
            AddForeignKey("Web.ProductProcesses", "Dimension4Id", "Web.Dimension4", "Dimension4Id");
            DropColumn("Web.SaleEnquiryLineExtendeds", "ProductGroup");
            DropColumn("Web.SaleEnquiryLineExtendeds", "Size");
            DropColumn("Web.SaleEnquiryLineExtendeds", "ProductQuality");
            DropColumn("Web.SaleEnquiryLineExtendeds", "Colour");
            DropColumn("Web.ViewProductBuyer", "ProductGroup");
            DropColumn("Web.ViewProductBuyer", "Size");
            DropColumn("Web.ViewProductBuyer", "Colour");
            DropColumn("Web.ViewProductBuyer", "ProductQuality");
        }
        
        public override void Down()
        {
            AddColumn("Web.ViewProductBuyer", "ProductQuality", c => c.String());
            AddColumn("Web.ViewProductBuyer", "Colour", c => c.String());
            AddColumn("Web.ViewProductBuyer", "Size", c => c.String());
            AddColumn("Web.ViewProductBuyer", "ProductGroup", c => c.String());
            AddColumn("Web.SaleEnquiryLineExtendeds", "Colour", c => c.String());
            AddColumn("Web.SaleEnquiryLineExtendeds", "ProductQuality", c => c.String());
            AddColumn("Web.SaleEnquiryLineExtendeds", "Size", c => c.String());
            AddColumn("Web.SaleEnquiryLineExtendeds", "ProductGroup", c => c.String());
            DropForeignKey("Web.ProductProcesses", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.ProductProcesses", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.PersonSettings", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.PackingLineExtendeds", "PackingLineId", "Web.PackingLines");
            DropForeignKey("Web.MaterialPlanForProdOrderLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.MaterialPlanForProdOrderLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.CarpetSkuSettings", "PerimeterSizeTypeId", "Web.ProductSizeTypes");
            DropForeignKey("Web.BomDetails", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.BomDetails", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.PurchaseIndentLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.PurchaseIndentLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.SaleOrderLines", "StockId", "Web.Stocks");
            DropForeignKey("Web.SaleOrderHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.SaleOrderLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.SaleOrderLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.StockLines", "StockInId", "Web.Stocks");
            DropForeignKey("Web.MaterialPlanLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.MaterialPlanLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.ProdOrderLines", "Dimension4Id", "Web.Dimension4");
            DropForeignKey("Web.Dimension4", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.Dimension4", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.ProdOrderLines", "Dimension3Id", "Web.Dimension3");
            DropForeignKey("Web.Dimension3", "ReferenceDocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.Dimension3", "ProductTypeId", "Web.ProductTypes");
            DropForeignKey("Web.GatePassHeaders", "OrderById", "Web.Employees");
            DropForeignKey("Web.People", "DocTypeId", "Web.DocumentTypes");
            DropIndex("Web.SaleOrderCancelHeaders", "IX_SaleOrderCancelHeader_DocID");
            DropIndex("Web.ProductProcesses", new[] { "Dimension4Id" });
            DropIndex("Web.ProductProcesses", new[] { "Dimension3Id" });
            DropIndex("Web.PersonSettings", new[] { "DocTypeId" });
            DropIndex("Web.PackingLineExtendeds", new[] { "PackingLineId" });
            DropIndex("Web.MaterialPlanForProdOrderLines", new[] { "Dimension4Id" });
            DropIndex("Web.MaterialPlanForProdOrderLines", new[] { "Dimension3Id" });
            DropIndex("Web.Dimension4Types", "IX_Dimension4Type_Dimension4TypeName");
            DropIndex("Web.Dimension3Types", "IX_Dimension3Type_Dimension3TypeName");
            DropIndex("Web.CarpetSkuSettings", new[] { "PerimeterSizeTypeId" });
            DropIndex("Web.BomDetails", new[] { "Dimension4Id" });
            DropIndex("Web.BomDetails", new[] { "Dimension3Id" });
            DropIndex("Web.PurchaseIndentLines", new[] { "Dimension4Id" });
            DropIndex("Web.PurchaseIndentLines", new[] { "Dimension3Id" });
            DropIndex("Web.SaleOrderHeaders", new[] { "GodownId" });
            DropIndex("Web.SaleOrderLines", new[] { "StockId" });
            DropIndex("Web.SaleOrderLines", new[] { "Dimension4Id" });
            DropIndex("Web.SaleOrderLines", new[] { "Dimension3Id" });
            DropIndex("Web.StockLines", new[] { "StockInId" });
            DropIndex("Web.MaterialPlanHeaders", "IX_MaterialPlanHeader_DocID");
            DropIndex("Web.MaterialPlanLines", new[] { "Dimension4Id" });
            DropIndex("Web.MaterialPlanLines", new[] { "Dimension3Id" });
            DropIndex("Web.Dimension4", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.Dimension4", new[] { "ProductTypeId" });
            DropIndex("Web.Dimension4", "IX_Dimension4_Dimension4Name");
            DropIndex("Web.Dimension3", new[] { "ReferenceDocTypeId" });
            DropIndex("Web.Dimension3", new[] { "ProductTypeId" });
            DropIndex("Web.Dimension3", "IX_Dimension3_Dimension3Name");
            DropIndex("Web.ProdOrderLines", new[] { "Dimension4Id" });
            DropIndex("Web.ProdOrderLines", new[] { "Dimension3Id" });
            DropIndex("Web.GatePassHeaders", new[] { "OrderById" });
            DropIndex("Web.People", new[] { "DocTypeId" });
            AlterColumn("Web.SaleInvoiceReturnLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.SaleInvoiceReturnHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.SaleInvoiceLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.SaleInvoiceHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseQuotationLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseQuotationHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseOrderRateAmendmentLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseOrderLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseOrderHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseOrderAmendmentHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseInvoiceReturnLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseInvoiceReturnHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseInvoiceLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.PurchaseInvoiceHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.JobOrderLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.JobOrderHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.JobInvoiceReturnLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.JobInvoiceReturnHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.JobInvoiceRateAmendmentLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.JobInvoiceLineCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.JobInvoiceHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.JobInvoiceAmendmentHeaderCharges", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.CalculationProducts", "AddDeduct", c => c.Boolean());
            AlterColumn("Web.CalculationFooters", "AddDeduct", c => c.Boolean());
            DropColumn("Web.ViewProductBuyer", "BuyerSpecification3");
            DropColumn("Web.ViewProductBuyer", "BuyerSpecification2");
            DropColumn("Web.ViewProductBuyer", "BuyerSpecification1");
            DropColumn("Web.ViewProductBuyer", "BuyerSpecification");
            DropColumn("Web.ViewProductBuyer", "BuyerSku");
            DropColumn("Web.SaleOrderSettings", "isPostedInStock");
            DropColumn("Web.SaleOrderSettings", "isVisibleGodown");
            DropColumn("Web.SaleOrderSettings", "isVisibleBillToParty");
            DropColumn("Web.SaleOrderSettings", "isVisibleDimension4");
            DropColumn("Web.SaleOrderSettings", "isVisibleDimension3");
            DropColumn("Web.SaleInvoiceSettings", "isVisibleDimension4");
            DropColumn("Web.SaleInvoiceSettings", "isVisibleDimension3");
            DropColumn("Web.SaleInvoiceReturnLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.SaleInvoiceReturnLineCharges", "IncludedCharges");
            DropColumn("Web.SaleInvoiceReturnHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.SaleInvoiceReturnHeaderCharges", "IncludedCharges");
            DropColumn("Web.SaleInvoiceLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.SaleInvoiceLineCharges", "IncludedCharges");
            DropColumn("Web.SaleInvoiceHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.SaleInvoiceHeaderCharges", "IncludedCharges");
            DropColumn("Web.SaleEnquirySettings", "isVisibleBillToParty");
            DropColumn("Web.SaleEnquirySettings", "isVisibleRate");
            DropColumn("Web.SaleEnquirySettings", "isVisibleDimension4");
            DropColumn("Web.SaleEnquirySettings", "isVisibleDimension3");
            DropColumn("Web.SaleEnquiryLineExtendeds", "BuyerSpecification3");
            DropColumn("Web.SaleEnquiryLineExtendeds", "BuyerSpecification2");
            DropColumn("Web.SaleEnquiryLineExtendeds", "BuyerSpecification1");
            DropColumn("Web.SaleEnquiryLineExtendeds", "BuyerSpecification");
            DropColumn("Web.SaleDispatchSettings", "isVisibleDimension4");
            DropColumn("Web.SaleDispatchSettings", "isVisibleDimension3");
            DropColumn("Web.SaleDeliveryOrderSettings", "isVisibleDimension4");
            DropColumn("Web.SaleDeliveryOrderSettings", "isVisibleDimension3");
            DropColumn("Web.RequisitionSettings", "isVisibleDimension4");
            DropColumn("Web.RequisitionSettings", "isVisibleDimension3");
            DropColumn("Web.RateConversionSettings", "isVisibleDimension4");
            DropColumn("Web.RateConversionSettings", "isVisibleDimension3");
            DropColumn("Web.PurchaseQuotationSettings", "isVisibleDimension4");
            DropColumn("Web.PurchaseQuotationSettings", "isVisibleDimension3");
            DropColumn("Web.PurchaseQuotationLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseQuotationLineCharges", "IncludedCharges");
            DropColumn("Web.PurchaseQuotationHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseQuotationHeaderCharges", "IncludedCharges");
            DropColumn("Web.PurchaseOrderSettings", "isVisibleDimension4");
            DropColumn("Web.PurchaseOrderSettings", "isVisibleDimension3");
            DropColumn("Web.PurchaseOrderRateAmendmentLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseOrderRateAmendmentLineCharges", "IncludedCharges");
            DropColumn("Web.PurchaseOrderLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseOrderLineCharges", "IncludedCharges");
            DropColumn("Web.PurchaseOrderInspectionSettings", "isVisibleDimension4");
            DropColumn("Web.PurchaseOrderInspectionSettings", "isVisibleDimension3");
            DropColumn("Web.PurchaseOrderInspectionRequestSettings", "isVisibleDimension4");
            DropColumn("Web.PurchaseOrderInspectionRequestSettings", "isVisibleDimension3");
            DropColumn("Web.PurchaseOrderHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseOrderHeaderCharges", "IncludedCharges");
            DropColumn("Web.PurchaseOrderAmendmentHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseOrderAmendmentHeaderCharges", "IncludedCharges");
            DropColumn("Web.PurchaseInvoiceSettings", "isVisibleDimension4");
            DropColumn("Web.PurchaseInvoiceSettings", "isVisibleDimension3");
            DropColumn("Web.PurchaseInvoiceReturnLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseInvoiceReturnLineCharges", "IncludedCharges");
            DropColumn("Web.PurchaseInvoiceReturnHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseInvoiceReturnHeaderCharges", "IncludedCharges");
            DropColumn("Web.PurchaseInvoiceLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseInvoiceLineCharges", "IncludedCharges");
            DropColumn("Web.PurchaseInvoiceHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.PurchaseInvoiceHeaderCharges", "IncludedCharges");
            DropColumn("Web.PurchaseIndentSettings", "isVisibleDimension4");
            DropColumn("Web.PurchaseIndentSettings", "isVisibleDimension3");
            DropColumn("Web.PurchaseGoodsReceiptSettings", "isVisibleDimension4");
            DropColumn("Web.PurchaseGoodsReceiptSettings", "isVisibleDimension3");
            DropColumn("Web.ProductProcesses", "Dimension4Id");
            DropColumn("Web.ProductProcesses", "Dimension3Id");
            DropColumn("Web.ProductionOrderSettings", "isVisibleDimension4");
            DropColumn("Web.ProductionOrderSettings", "isVisibleDimension3");
            DropColumn("Web.ProdOrderSettings", "isVisibleDimension4");
            DropColumn("Web.ProdOrderSettings", "isVisibleDimension3");
            DropColumn("Web.PersonRegistrations", "ExpiryDate");
            DropColumn("Web.PersonRegistrations", "RegistrationDate");
            DropColumn("Web.PersonBankAccounts", "AccountHolderName");
            DropColumn("Web.PackingSettings", "isVisibleDimension4");
            DropColumn("Web.PackingSettings", "isVisibleDimension3");
            DropColumn("Web.MaterialRequestSettings", "isVisibleDimension4");
            DropColumn("Web.MaterialRequestSettings", "isVisibleDimension3");
            DropColumn("Web.MaterialReceiveSettings", "isVisibleDimension4");
            DropColumn("Web.MaterialReceiveSettings", "isVisibleDimension3");
            DropColumn("Web.MaterialPlanSettings", "isVisibleDimension4");
            DropColumn("Web.MaterialPlanSettings", "isVisibleDimension3");
            DropColumn("Web.MaterialPlanForProdOrderLines", "Dimension4Id");
            DropColumn("Web.MaterialPlanForProdOrderLines", "Dimension3Id");
            DropColumn("Web.StockHeaderSettings", "isVisibleStockIn");
            DropColumn("Web.StockHeaderSettings", "isVisibleDimension4");
            DropColumn("Web.StockHeaderSettings", "isVisibleDimension3");
            DropColumn("Web.StockHeaderSettings", "isVisibleProductCode");
            DropColumn("Web.JobReceiveSettings", "isVisibleByProductDimension2");
            DropColumn("Web.JobReceiveSettings", "isVisibleByProductDimension1");
            DropColumn("Web.JobReceiveSettings", "isVisibleConsumptionDimension2");
            DropColumn("Web.JobReceiveSettings", "isVisibleConsumptionDimension1");
            DropColumn("Web.JobReceiveSettings", "ByProductDimension2Caption");
            DropColumn("Web.JobReceiveSettings", "ByProductDimension1Caption");
            DropColumn("Web.JobReceiveSettings", "ByProductCaption");
            DropColumn("Web.JobReceiveSettings", "ConsumptionDimension2Caption");
            DropColumn("Web.JobReceiveSettings", "ConsumptionDimension1Caption");
            DropColumn("Web.JobReceiveSettings", "ConsumptionProductCaption");
            DropColumn("Web.JobReceiveSettings", "LossPer");
            DropColumn("Web.JobReceiveSettings", "isVisibleByProductDetail");
            DropColumn("Web.JobReceiveSettings", "isVisibleConsumptionDetail");
            DropColumn("Web.JobReceiveSettings", "isVisibleCostCenter");
            DropColumn("Web.JobReceiveSettings", "isVisibleDimension4");
            DropColumn("Web.JobReceiveSettings", "isVisibleDimension3");
            DropColumn("Web.JobReceiveQASettings", "IsVisibleHeight");
            DropColumn("Web.JobReceiveQASettings", "IsVisibleWidth");
            DropColumn("Web.JobReceiveQASettings", "IsVisibleLength");
            DropColumn("Web.JobReceiveQASettings", "isVisibleDimension4");
            DropColumn("Web.JobReceiveQASettings", "isVisibleDimension3");
            DropColumn("Web.QAGroups", "ReviewBy");
            DropColumn("Web.QAGroups", "ReviewCount");
            DropColumn("Web.JobOrderSettings", "isVisibleDimension4");
            DropColumn("Web.JobOrderSettings", "isVisibleDimension3");
            DropColumn("Web.JobOrderLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.JobOrderLineCharges", "IncludedCharges");
            DropColumn("Web.JobOrderInspectionSettings", "isVisibleDimension4");
            DropColumn("Web.JobOrderInspectionSettings", "isVisibleDimension3");
            DropColumn("Web.JobOrderInspectionRequestSettings", "isVisibleDimension4");
            DropColumn("Web.JobOrderInspectionRequestSettings", "isVisibleDimension3");
            DropColumn("Web.JobOrderHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.JobOrderHeaderCharges", "IncludedCharges");
            DropColumn("Web.JobInvoiceSettings", "isVisibleDimension4");
            DropColumn("Web.JobInvoiceSettings", "isVisibleDimension3");
            DropColumn("Web.JobInvoiceReturnLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.JobInvoiceReturnLineCharges", "IncludedCharges");
            DropColumn("Web.JobInvoiceReturnHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.JobInvoiceReturnHeaderCharges", "IncludedCharges");
            DropColumn("Web.JobInvoiceRateAmendmentLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.JobInvoiceRateAmendmentLineCharges", "IncludedCharges");
            DropColumn("Web.JobInvoiceLineCharges", "IncludedChargesCalculation");
            DropColumn("Web.JobInvoiceLineCharges", "IncludedCharges");
            DropColumn("Web.JobInvoiceHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.JobInvoiceHeaderCharges", "IncludedCharges");
            DropColumn("Web.JobInvoiceAmendmentHeaderCharges", "IncludedChargesCalculation");
            DropColumn("Web.JobInvoiceAmendmentHeaderCharges", "IncludedCharges");
            DropColumn("Web.JobConsumptionSettings", "isVisibleDimension4");
            DropColumn("Web.JobConsumptionSettings", "isVisibleDimension3");
            DropColumn("Web.ExcessMaterialSettings", "isVisibleDimension4");
            DropColumn("Web.ExcessMaterialSettings", "isVisibleDimension3");
            DropColumn("Web.DocumentTypeSettings", "ContraDocTypeCaption");
            DropColumn("Web.DocumentTypeSettings", "PartyCaption");
            DropColumn("Web.DocumentTypeSettings", "ProductUidCaption");
            DropColumn("Web.CarpetSkuSettings", "PerimeterSizeTypeId");
            DropColumn("Web.CarpetSkuSettings", "isVisibleStencilSize");
            DropColumn("Web.CarpetSkuSettings", "isVisibleMapType");
            DropColumn("Web.CarpetSkuSettings", "isVisibleTraceType");
            DropColumn("Web.CalculationProducts", "IncludedChargesCalculation");
            DropColumn("Web.CalculationProducts", "IncludedCharges");
            DropColumn("Web.CalculationFooters", "IncludedChargesCalculation");
            DropColumn("Web.CalculationFooters", "IncludedCharges");
            DropColumn("Web.BomDetails", "StdTime");
            DropColumn("Web.BomDetails", "StdCost");
            DropColumn("Web.BomDetails", "MBQ");
            DropColumn("Web.BomDetails", "Dimension4Id");
            DropColumn("Web.BomDetails", "Dimension3Id");
            DropColumn("Web.PurchaseIndentLines", "Dimension4Id");
            DropColumn("Web.PurchaseIndentLines", "Dimension3Id");
            DropColumn("Web.SaleOrderHeaders", "GodownId");
            DropColumn("Web.SaleOrderLines", "StockId");
            DropColumn("Web.SaleOrderLines", "Dimension4Id");
            DropColumn("Web.SaleOrderLines", "Dimension3Id");
            DropColumn("Web.StockLines", "StockInId");
            DropColumn("Web.MaterialPlanLines", "Dimension4Id");
            DropColumn("Web.MaterialPlanLines", "Dimension3Id");
            DropColumn("Web.ProdOrderLines", "Dimension4Id");
            DropColumn("Web.ProdOrderLines", "Dimension3Id");
            DropColumn("Web.GatePassHeaders", "OrderById");
            DropColumn("Web.GatePassHeaders", "ReviewBy");
            DropColumn("Web.GatePassHeaders", "ReviewCount");
            DropColumn("Web.People", "DocTypeId");
            DropTable("Web.PersonSettings");
            DropTable("Web.PackingLineExtendeds");
            DropTable("Web.Dimension4Types");
            DropTable("Web.Dimension3Types");
            DropTable("Web.Dimension4");
            DropTable("Web.Dimension3");
            CreateIndex("Web.SaleOrderCancelHeaders", "SiteId");
            CreateIndex("Web.SaleOrderCancelHeaders", "DivisionId");
            CreateIndex("Web.SaleOrderCancelHeaders", "DocTypeId");
            CreateIndex("Web.MaterialPlanHeaders", "SiteId");
            CreateIndex("Web.MaterialPlanHeaders", "DivisionId");
            CreateIndex("Web.MaterialPlanHeaders", "DocTypeId");
            RenameTable(name: "Web.JobReceiveQALineExtendeds", newName: "JobReceiveQALineDetails");
        }
    }
}
