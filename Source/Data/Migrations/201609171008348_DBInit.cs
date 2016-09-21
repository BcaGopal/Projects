namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBInit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Web.StockHeaders", "MachineId", "Web.Machines");
            DropForeignKey("Web.JobOrderHeaders", "MachineId", "Web.Machines");
            DropIndex("Web.Machines", "IX_Machine_MachineName");
            DropPrimaryKey("Web.Machines");
            CreateTable(
                "Web.JobOrderHeaderExtendeds",
                c => new
                    {
                        JobOrderHeaderId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobOrderHeaderId)
                .ForeignKey("Web.JobOrderHeaders", t => t.JobOrderHeaderId)
                .ForeignKey("Web.People", t => t.PersonId)
                .Index(t => t.JobOrderHeaderId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "Web.MaterialPlanCancelForProdOrders",
                c => new
                    {
                        MaterialPlanCancelForProdOrderId = c.Int(nullable: false, identity: true),
                        MaterialPlanCancelHeaderId = c.Int(nullable: false),
                        MaterialPlanLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OMSId = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Sr = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanCancelForProdOrderId)
                .ForeignKey("Web.MaterialPlanCancelHeaders", t => t.MaterialPlanCancelHeaderId)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .Index(t => t.MaterialPlanCancelHeaderId)
                .Index(t => t.MaterialPlanLineId);
            
            CreateTable(
                "Web.MaterialPlanCancelHeaders",
                c => new
                    {
                        MaterialPlanCancelHeaderId = c.Int(nullable: false, identity: true),
                        DocTypeId = c.Int(nullable: false),
                        DocDate = c.DateTime(nullable: false),
                        DocNo = c.String(maxLength: 20),
                        BuyerId = c.Int(),
                        DivisionId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        GodownId = c.Int(),
                        Status = c.Int(nullable: false),
                        Remark = c.String(),
                        LockReason = c.String(),
                        ReviewCount = c.Int(),
                        ReviewBy = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaterialPlanCancelHeaderId)
                .ForeignKey("Web.Buyers", t => t.BuyerId)
                .ForeignKey("Web.Divisions", t => t.DivisionId)
                .ForeignKey("Web.DocumentTypes", t => t.DocTypeId)
                .ForeignKey("Web.Godowns", t => t.GodownId)
                .ForeignKey("Web.Sites", t => t.SiteId)
                .Index(t => t.DocTypeId)
                .Index(t => t.BuyerId)
                .Index(t => t.DivisionId)
                .Index(t => t.SiteId)
                .Index(t => t.GodownId);
            
            CreateTable(
                "Web.MaterialPlanCancelForProdOrderLines",
                c => new
                    {
                        MaterialPlanCancelForProdOrderLineId = c.Int(nullable: false, identity: true),
                        MaterialPlanCancelForProdOrderId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        MaterialPlanCancelLineId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                        Sr = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanCancelForProdOrderLineId)
                .ForeignKey("Web.MaterialPlanCancelForProdOrders", t => t.MaterialPlanCancelForProdOrderId)
                .ForeignKey("Web.MaterialPlanCancelLines", t => t.MaterialPlanCancelLineId)
                .Index(t => t.MaterialPlanCancelForProdOrderId)
                .Index(t => t.MaterialPlanCancelLineId);
            
            CreateTable(
                "Web.MaterialPlanCancelLines",
                c => new
                    {
                        MaterialPlanCancelLineId = c.Int(nullable: false, identity: true),
                        MaterialPlanCancelHeaderId = c.Int(nullable: false),
                        MaterialPlanLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Remark = c.String(),
                        LockReason = c.String(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        OMSId = c.String(maxLength: 50),
                        Sr = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanCancelLineId)
                .ForeignKey("Web.MaterialPlanCancelHeaders", t => t.MaterialPlanCancelHeaderId)
                .ForeignKey("Web.MaterialPlanLines", t => t.MaterialPlanLineId)
                .Index(t => t.MaterialPlanCancelHeaderId)
                .Index(t => t.MaterialPlanLineId);
            
            CreateTable(
                "Web.MaterialPlanCancelForSaleOrders",
                c => new
                    {
                        MaterialPlanCancelForSaleOrderId = c.Int(nullable: false, identity: true),
                        MaterialPlanCancelHeaderId = c.Int(nullable: false),
                        MaterialPlanForSaleOrderLineId = c.Int(nullable: false),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 4),
                        MaterialPlanCancelLineId = c.Int(),
                        OMSId = c.String(maxLength: 50),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Sr = c.Int(),
                    })
                .PrimaryKey(t => t.MaterialPlanCancelForSaleOrderId)
                .ForeignKey("Web.MaterialPlanCancelHeaders", t => t.MaterialPlanCancelHeaderId)
                .ForeignKey("Web.MaterialPlanCancelLines", t => t.MaterialPlanCancelLineId)
                .ForeignKey("Web.MaterialPlanForSaleOrderLines", t => t.MaterialPlanForSaleOrderLineId)
                .Index(t => t.MaterialPlanCancelHeaderId)
                .Index(t => t.MaterialPlanForSaleOrderLineId)
                .Index(t => t.MaterialPlanCancelLineId);
            
            AddColumn("Web.ProductGroups", "Sr", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AddColumn("Web.PackingLines", "LossQty", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.PackingLines", "PassQty", c => c.Decimal(precision: 18, scale: 4));
            AddColumn("Web.Machines", "ProductID", c => c.Int(nullable: false));
            AddColumn("Web.ProductInvoiceGroups", "Code", c => c.String(maxLength: 50));
            AddColumn("Web.JobInvoiceSettings", "NoOfPrintCopies", c => c.Int());
            AddColumn("Web.JobOrderHeaderStatus", "JobCancelCount", c => c.Int());
            AddColumn("Web.ReportColumns", "Serial", c => c.Int(nullable: false));
            AddColumn("Web.ReportColumns", "IsDocNo", c => c.Boolean(nullable: false));
            AddColumn("Web.ReportHeaders", "IsGridReport", c => c.Boolean());
            AddColumn("Web.ReportHeaders", "IsPDFReport", c => c.Boolean());
            AddColumn("Web.ViewSaleInvoiceBalance", "BaleNo", c => c.String());
            AddPrimaryKey("Web.Machines", "ProductID");
            CreateIndex("Web.Machines", "ProductID");
            AddForeignKey("Web.Machines", "ProductID", "Web.Products", "ProductId");
            AddForeignKey("Web.StockHeaders", "MachineId", "Web.Machines", "ProductID");
            AddForeignKey("Web.JobOrderHeaders", "MachineId", "Web.Machines", "ProductID");
            DropColumn("Web.Machines", "MachineId");
            DropColumn("Web.Machines", "MachineName");
            DropColumn("Web.Machines", "IsActive");
            DropColumn("Web.Machines", "CreatedBy");
            DropColumn("Web.Machines", "ModifiedBy");
            DropColumn("Web.Machines", "CreatedDate");
            DropColumn("Web.Machines", "ModifiedDate");
        }
        
        public override void Down()
        {
            AddColumn("Web.Machines", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("Web.Machines", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("Web.Machines", "ModifiedBy", c => c.String());
            AddColumn("Web.Machines", "CreatedBy", c => c.String());
            AddColumn("Web.Machines", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("Web.Machines", "MachineName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("Web.Machines", "MachineId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("Web.JobOrderHeaders", "MachineId", "Web.Machines");
            DropForeignKey("Web.StockHeaders", "MachineId", "Web.Machines");
            DropForeignKey("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanForSaleOrderLineId", "Web.MaterialPlanForSaleOrderLines");
            DropForeignKey("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanCancelLineId", "Web.MaterialPlanCancelLines");
            DropForeignKey("Web.MaterialPlanCancelForSaleOrders", "MaterialPlanCancelHeaderId", "Web.MaterialPlanCancelHeaders");
            DropForeignKey("Web.MaterialPlanCancelForProdOrderLines", "MaterialPlanCancelLineId", "Web.MaterialPlanCancelLines");
            DropForeignKey("Web.MaterialPlanCancelLines", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.MaterialPlanCancelLines", "MaterialPlanCancelHeaderId", "Web.MaterialPlanCancelHeaders");
            DropForeignKey("Web.MaterialPlanCancelForProdOrderLines", "MaterialPlanCancelForProdOrderId", "Web.MaterialPlanCancelForProdOrders");
            DropForeignKey("Web.MaterialPlanCancelForProdOrders", "MaterialPlanLineId", "Web.MaterialPlanLines");
            DropForeignKey("Web.MaterialPlanCancelForProdOrders", "MaterialPlanCancelHeaderId", "Web.MaterialPlanCancelHeaders");
            DropForeignKey("Web.MaterialPlanCancelHeaders", "SiteId", "Web.Sites");
            DropForeignKey("Web.MaterialPlanCancelHeaders", "GodownId", "Web.Godowns");
            DropForeignKey("Web.MaterialPlanCancelHeaders", "DocTypeId", "Web.DocumentTypes");
            DropForeignKey("Web.MaterialPlanCancelHeaders", "DivisionId", "Web.Divisions");
            DropForeignKey("Web.MaterialPlanCancelHeaders", "BuyerId", "Web.Buyers");
            DropForeignKey("Web.JobOrderHeaderExtendeds", "PersonId", "Web.People");
            DropForeignKey("Web.JobOrderHeaderExtendeds", "JobOrderHeaderId", "Web.JobOrderHeaders");
            DropForeignKey("Web.Machines", "ProductID", "Web.Products");
            DropIndex("Web.MaterialPlanCancelForSaleOrders", new[] { "MaterialPlanCancelLineId" });
            DropIndex("Web.MaterialPlanCancelForSaleOrders", new[] { "MaterialPlanForSaleOrderLineId" });
            DropIndex("Web.MaterialPlanCancelForSaleOrders", new[] { "MaterialPlanCancelHeaderId" });
            DropIndex("Web.MaterialPlanCancelLines", new[] { "MaterialPlanLineId" });
            DropIndex("Web.MaterialPlanCancelLines", new[] { "MaterialPlanCancelHeaderId" });
            DropIndex("Web.MaterialPlanCancelForProdOrderLines", new[] { "MaterialPlanCancelLineId" });
            DropIndex("Web.MaterialPlanCancelForProdOrderLines", new[] { "MaterialPlanCancelForProdOrderId" });
            DropIndex("Web.MaterialPlanCancelHeaders", new[] { "GodownId" });
            DropIndex("Web.MaterialPlanCancelHeaders", new[] { "SiteId" });
            DropIndex("Web.MaterialPlanCancelHeaders", new[] { "DivisionId" });
            DropIndex("Web.MaterialPlanCancelHeaders", new[] { "BuyerId" });
            DropIndex("Web.MaterialPlanCancelHeaders", new[] { "DocTypeId" });
            DropIndex("Web.MaterialPlanCancelForProdOrders", new[] { "MaterialPlanLineId" });
            DropIndex("Web.MaterialPlanCancelForProdOrders", new[] { "MaterialPlanCancelHeaderId" });
            DropIndex("Web.JobOrderHeaderExtendeds", new[] { "PersonId" });
            DropIndex("Web.JobOrderHeaderExtendeds", new[] { "JobOrderHeaderId" });
            DropIndex("Web.Machines", new[] { "ProductID" });
            DropPrimaryKey("Web.Machines");
            DropColumn("Web.ViewSaleInvoiceBalance", "BaleNo");
            DropColumn("Web.ReportHeaders", "IsPDFReport");
            DropColumn("Web.ReportHeaders", "IsGridReport");
            DropColumn("Web.ReportColumns", "IsDocNo");
            DropColumn("Web.ReportColumns", "Serial");
            DropColumn("Web.JobOrderHeaderStatus", "JobCancelCount");
            DropColumn("Web.JobInvoiceSettings", "NoOfPrintCopies");
            DropColumn("Web.ProductInvoiceGroups", "Code");
            DropColumn("Web.Machines", "ProductID");
            DropColumn("Web.PackingLines", "PassQty");
            DropColumn("Web.PackingLines", "LossQty");
            DropColumn("Web.ProductGroups", "Sr");
            DropTable("Web.MaterialPlanCancelForSaleOrders");
            DropTable("Web.MaterialPlanCancelLines");
            DropTable("Web.MaterialPlanCancelForProdOrderLines");
            DropTable("Web.MaterialPlanCancelHeaders");
            DropTable("Web.MaterialPlanCancelForProdOrders");
            DropTable("Web.JobOrderHeaderExtendeds");
            AddPrimaryKey("Web.Machines", "MachineId");
            CreateIndex("Web.Machines", "MachineName", unique: true, name: "IX_Machine_MachineName");
            AddForeignKey("Web.JobOrderHeaders", "MachineId", "Web.Machines", "MachineId");
            AddForeignKey("Web.StockHeaders", "MachineId", "Web.Machines", "MachineId");
        }
    }
}
