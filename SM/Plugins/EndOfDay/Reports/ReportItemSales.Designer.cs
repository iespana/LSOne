using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.Reports
{
    partial class ReportItemSales
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportItemSales));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblVariantID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItemDescr = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItemID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.amount = new DevExpress.XtraReports.UI.XRLabel();
            this.quantity = new DevExpress.XtraReports.UI.XRLabel();
            this.variantID = new DevExpress.XtraReports.UI.XRLabel();
            this.itemDescription = new DevExpress.XtraReports.UI.XRLabel();
            this.itemID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.periodDefinition = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel76 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDateTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.companyInfo = new System.Windows.Forms.BindingSource(this.components);
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLblStoreID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblStoreIDH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.bsTotals = new System.Windows.Forms.BindingSource(this.components);
            this.itemSaleLines = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.companyInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTotals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemSaleLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel10,
            this.xrLabel2,
            this.xrLblAmount,
            this.xrLblVariantID,
            this.xrLblItemDescr,
            this.xrLblItemID,
            this.xrLabel8});
            resources.ApplyResources(this.Detail, "Detail");
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
            // 
            // xrLabel14
            // 
            this.xrLabel14.CanGrow = false;
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormattedWholeDiscountAmountWithTax")});
            resources.ApplyResources(this.xrLabel14, "xrLabel14");
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.WordWrap = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.CanGrow = false;
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormattedPrice")});
            resources.ApplyResources(this.xrLabel13, "xrLabel13");
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.WordWrap = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.CanGrow = false;
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "StyleName")});
            resources.ApplyResources(this.xrLabel10, "xrLabel10");
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.WordWrap = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.CanGrow = false;
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormattedQuantity")});
            resources.ApplyResources(this.xrLabel2, "xrLabel2");
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.WordWrap = false;
            // 
            // xrLblAmount
            // 
            this.xrLblAmount.CanGrow = false;
            this.xrLblAmount.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormattedNetAmountWithTax")});
            resources.ApplyResources(this.xrLblAmount, "xrLblAmount");
            this.xrLblAmount.Name = "xrLblAmount";
            this.xrLblAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblAmount.StylePriority.UseFont = false;
            this.xrLblAmount.StylePriority.UseTextAlignment = false;
            this.xrLblAmount.WordWrap = false;
            // 
            // xrLblVariantID
            // 
            this.xrLblVariantID.CanGrow = false;
            this.xrLblVariantID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ColorName")});
            resources.ApplyResources(this.xrLblVariantID, "xrLblVariantID");
            this.xrLblVariantID.Name = "xrLblVariantID";
            this.xrLblVariantID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblVariantID.StylePriority.UseFont = false;
            this.xrLblVariantID.WordWrap = false;
            // 
            // xrLblItemDescr
            // 
            this.xrLblItemDescr.CanGrow = false;
            this.xrLblItemDescr.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemName")});
            resources.ApplyResources(this.xrLblItemDescr, "xrLblItemDescr");
            this.xrLblItemDescr.Name = "xrLblItemDescr";
            this.xrLblItemDescr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItemDescr.StylePriority.UseFont = false;
            this.xrLblItemDescr.WordWrap = false;
            // 
            // xrLblItemID
            // 
            this.xrLblItemID.CanGrow = false;
            this.xrLblItemID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemID")});
            resources.ApplyResources(this.xrLblItemID, "xrLblItemID");
            this.xrLblItemID.Name = "xrLblItemID";
            this.xrLblItemID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItemID.StylePriority.UseFont = false;
            this.xrLblItemID.WordWrap = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.CanGrow = false;
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SizeName")});
            resources.ApplyResources(this.xrLabel8, "xrLabel8");
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.WordWrap = false;
            // 
            // TopMargin
            // 
            resources.ApplyResources(this.TopMargin, "TopMargin");
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // BottomMargin
            // 
            resources.ApplyResources(this.BottomMargin, "BottomMargin");
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel9,
            this.amount,
            this.quantity,
            this.variantID,
            this.itemDescription,
            this.itemID,
            this.xrLabel7});
            resources.ApplyResources(this.PageHeader, "PageHeader");
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.StylePriority.UseTextAlignment = false;
            // 
            // xrLabel12
            // 
            resources.ApplyResources(this.xrLabel12, "xrLabel12");
            this.xrLabel12.CanGrow = false;
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.StylePriority.UseBackColor = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.WordWrap = false;
            // 
            // xrLabel11
            // 
            resources.ApplyResources(this.xrLabel11, "xrLabel11");
            this.xrLabel11.CanGrow = false;
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.StylePriority.UseBackColor = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.WordWrap = false;
            // 
            // xrLabel9
            // 
            resources.ApplyResources(this.xrLabel9, "xrLabel9");
            this.xrLabel9.CanGrow = false;
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.StylePriority.UseBackColor = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.WordWrap = false;
            // 
            // amount
            // 
            resources.ApplyResources(this.amount, "amount");
            this.amount.CanGrow = false;
            this.amount.Name = "amount";
            this.amount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.amount.StylePriority.UseBackColor = false;
            this.amount.StylePriority.UseFont = false;
            this.amount.StylePriority.UseTextAlignment = false;
            this.amount.WordWrap = false;
            // 
            // quantity
            // 
            resources.ApplyResources(this.quantity, "quantity");
            this.quantity.CanGrow = false;
            this.quantity.Name = "quantity";
            this.quantity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.quantity.StylePriority.UseBackColor = false;
            this.quantity.StylePriority.UseFont = false;
            this.quantity.StylePriority.UseTextAlignment = false;
            this.quantity.WordWrap = false;
            // 
            // variantID
            // 
            resources.ApplyResources(this.variantID, "variantID");
            this.variantID.CanGrow = false;
            this.variantID.Multiline = true;
            this.variantID.Name = "variantID";
            this.variantID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.variantID.StylePriority.UseBackColor = false;
            this.variantID.StylePriority.UseFont = false;
            this.variantID.WordWrap = false;
            // 
            // itemDescription
            // 
            resources.ApplyResources(this.itemDescription, "itemDescription");
            this.itemDescription.CanGrow = false;
            this.itemDescription.Name = "itemDescription";
            this.itemDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.itemDescription.StylePriority.UseBackColor = false;
            this.itemDescription.StylePriority.UseFont = false;
            this.itemDescription.WordWrap = false;
            // 
            // itemID
            // 
            resources.ApplyResources(this.itemID, "itemID");
            this.itemID.CanGrow = false;
            this.itemID.Name = "itemID";
            this.itemID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.itemID.StylePriority.UseBackColor = false;
            this.itemID.StylePriority.UseFont = false;
            this.itemID.WordWrap = false;
            // 
            // xrLabel7
            // 
            resources.ApplyResources(this.xrLabel7, "xrLabel7");
            this.xrLabel7.CanGrow = false;
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.StylePriority.UseBackColor = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.WordWrap = false;
            // 
            // periodDefinition
            // 
            this.periodDefinition.CanGrow = false;
            resources.ApplyResources(this.periodDefinition, "periodDefinition");
            this.periodDefinition.Name = "periodDefinition";
            this.periodDefinition.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.periodDefinition.StylePriority.UseFont = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            resources.ApplyResources(this.xrLabel1, "xrLabel1");
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.StylePriority.UseFont = false;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLabel76});
            resources.ApplyResources(this.PageFooter, "PageFooter");
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo1
            // 
            resources.ApplyResources(this.xrPageInfo1, "xrPageInfo1");
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // xrLabel76
            // 
            resources.ApplyResources(this.xrLabel76, "xrLabel76");
            this.xrLabel76.Name = "xrLabel76";
            this.xrLabel76.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel76.StylePriority.UseTextAlignment = false;
            // 
            // lblDateTime
            // 
            this.lblDateTime.CanGrow = false;
            resources.ApplyResources(this.lblDateTime, "lblDateTime");
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDateTime.StylePriority.UseTextAlignment = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.companyInfo, "AddressFormatted")});
            resources.ApplyResources(this.xrLabel3, "xrLabel3");
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.StylePriority.UseFont = false;
            // 
            // companyInfo
            // 
            this.companyInfo.DataSource = typeof(CompanyInfo);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLblStoreID,
            this.xrLblStoreIDH,
            this.xrLabel21,
            this.xrPictureBox1,
            this.xrLabel4,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel1,
            this.xrLabel3,
            this.periodDefinition,
            this.lblDateTime});
            resources.ApplyResources(this.ReportHeader, "ReportHeader");
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLblStoreID
            // 
            resources.ApplyResources(this.xrLblStoreID, "xrLblStoreID");
            this.xrLblStoreID.Name = "xrLblStoreID";
            this.xrLblStoreID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblStoreID.StylePriority.UseFont = false;
            this.xrLblStoreID.StylePriority.UseTextAlignment = false;
            // 
            // xrLblStoreIDH
            // 
            resources.ApplyResources(this.xrLblStoreIDH, "xrLblStoreIDH");
            this.xrLblStoreIDH.Name = "xrLblStoreIDH";
            this.xrLblStoreIDH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblStoreIDH.StylePriority.UseFont = false;
            this.xrLblStoreIDH.StylePriority.UseTextAlignment = false;
            this.xrLblStoreIDH.WordWrap = false;
            // 
            // xrLabel21
            // 
            this.xrLabel21.CanGrow = false;
            resources.ApplyResources(this.xrLabel21, "xrLabel21");
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.WordWrap = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", this.companyInfo, "CompanyLogo")});
            resources.ApplyResources(this.xrPictureBox1, "xrPictureBox1");
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.companyInfo, "Text")});
            resources.ApplyResources(this.xrLabel4, "xrLabel4");
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.StylePriority.UseFont = false;
            // 
            // xrLabel19
            // 
            resources.ApplyResources(this.xrLabel19, "xrLabel19");
            this.xrLabel19.Multiline = true;
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.WordWrap = false;
            // 
            // xrLabel18
            // 
            resources.ApplyResources(this.xrLabel18, "xrLabel18");
            this.xrLabel18.Multiline = true;
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.WordWrap = false;
            // 
            // xrLabel17
            // 
            this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.companyInfo, "Fax")});
            resources.ApplyResources(this.xrLabel17, "xrLabel17");
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.StylePriority.UseFont = false;
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.companyInfo, "Phone")});
            resources.ApplyResources(this.xrLabel16, "xrLabel16");
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.StylePriority.UseFont = false;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.xrLabel20,
            this.xrLabel6,
            this.xrLabel5});
            resources.ApplyResources(this.ReportFooter, "ReportFooter");
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLine1
            // 
            resources.ApplyResources(this.xrLine1, "xrLine1");
            this.xrLine1.Name = "xrLine1";
            // 
            // xrLabel20
            // 
            resources.ApplyResources(this.xrLabel20, "xrLabel20");
            this.xrLabel20.CanGrow = false;
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.StylePriority.UseBackColor = false;
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.bsTotals, "FormattedWholeDiscountAmountWithTax")});
            resources.ApplyResources(this.xrLabel6, "xrLabel6");
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanGrow = false;
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.bsTotals, "FormattedNetAmountWithTax")});
            resources.ApplyResources(this.xrLabel5, "xrLabel5");
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            // 
            // bsTotals
            // 
            this.bsTotals.DataSource = typeof(SalesTotalAmounts);
            // 
            // itemSaleLines
            // 
            this.itemSaleLines.DataSource = typeof(ItemSaleLine);
            // 
            // ReportItemSales
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter,
            this.ReportHeader,
            this.ReportFooter});
            this.DataSource = this.itemSaleLines;
            resources.ApplyResources(this, "$this");
            this.Version = "10.1";
            ((System.ComponentModel.ISupportInitialize)(this.companyInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTotals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemSaleLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel periodDefinition;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel76;
        private DevExpress.XtraReports.UI.XRLabel lblDateTime;
        private DevExpress.XtraReports.UI.XRLabel xrLblAmount;
        private DevExpress.XtraReports.UI.XRLabel xrLblVariantID;
        private DevExpress.XtraReports.UI.XRLabel xrLblItemDescr;
        private DevExpress.XtraReports.UI.XRLabel xrLblItemID;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel amount;
        private DevExpress.XtraReports.UI.XRLabel quantity;
        private DevExpress.XtraReports.UI.XRLabel variantID;
        private DevExpress.XtraReports.UI.XRLabel itemDescription;
        private DevExpress.XtraReports.UI.XRLabel itemID;
        private System.Windows.Forms.BindingSource itemSaleLines;
        private System.Windows.Forms.BindingSource companyInfo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private System.Windows.Forms.BindingSource bsTotals;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLabel xrLblStoreID;
        private DevExpress.XtraReports.UI.XRLabel xrLblStoreIDH;

        //public DatasetItemSales DatasetItemSales
        //{
        //    get { return datasetItemSales1; }
        //    set { datasetItemSales1 = value; }
        //}
    }
}
