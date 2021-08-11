using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.Inventory.Reports
{
    partial class PurchaseOrderReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseOrderReport));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblQuantity = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblPrice = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblVariant = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItem = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItemName = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.companyInfo = new System.Windows.Forms.BindingSource(this.components);
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.purchaseOrder = new System.Windows.Forms.BindingSource(this.components);
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblQuantityH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblVariantIDH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItemIDH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItemNameH = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblPriceH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.poMiscChargesSubReport1 = new POMiscChargesSubReport();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblTotalPrice = new DevExpress.XtraReports.UI.XRLabel();
            this.winControlContainer6 = new DevExpress.XtraReports.UI.WinControlContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.purchaseOrderLines = new System.Windows.Forms.BindingSource(this.components);
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.companyInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.poMiscChargesSubReport1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseOrderLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel31,
            this.xrLabel29,
            this.xrLabel27,
            this.xrLabel16,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLblQuantity,
            this.xrLblPrice,
            this.xrLblVariant,
            this.xrLblItem,
            this.xrLblItemName,
            this.xrLabel32});
            resources.ApplyResources(this.Detail, "Detail");
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseBorders = false;
            // 
            // xrLabel31
            // 
            this.xrLabel31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormatedTaxAmount")});
            resources.ApplyResources(this.xrLabel31, "xrLabel31");
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.WordWrap = false;
            // 
            // xrLabel29
            // 
            this.xrLabel29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormatedDiscountPercentage")});
            resources.ApplyResources(this.xrLabel29, "xrLabel29");
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.WordWrap = false;
            // 
            // xrLabel27
            // 
            this.xrLabel27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormatedDiscountAmount")});
            resources.ApplyResources(this.xrLabel27, "xrLabel27");
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.WordWrap = false;
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormatedTotalPrice")});
            resources.ApplyResources(this.xrLabel16, "xrLabel16");
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.WordWrap = false;
            // 
            // xrLabel14
            // 
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "StyleName")});
            resources.ApplyResources(this.xrLabel14, "xrLabel14");
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.WordWrap = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SizeName")});
            resources.ApplyResources(this.xrLabel13, "xrLabel13");
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.WordWrap = false;
            // 
            // xrLblQuantity
            // 
            this.xrLblQuantity.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormatedQuantity")});
            resources.ApplyResources(this.xrLblQuantity, "xrLblQuantity");
            this.xrLblQuantity.Name = "xrLblQuantity";
            this.xrLblQuantity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblQuantity.StylePriority.UseFont = false;
            this.xrLblQuantity.StylePriority.UseTextAlignment = false;
            this.xrLblQuantity.WordWrap = false;
            // 
            // xrLblPrice
            // 
            this.xrLblPrice.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormatedUnitPrice")});
            resources.ApplyResources(this.xrLblPrice, "xrLblPrice");
            this.xrLblPrice.Name = "xrLblPrice";
            this.xrLblPrice.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblPrice.StylePriority.UseFont = false;
            this.xrLblPrice.StylePriority.UseTextAlignment = false;
            this.xrLblPrice.WordWrap = false;
            // 
            // xrLblVariant
            // 
            this.xrLblVariant.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ColorName")});
            resources.ApplyResources(this.xrLblVariant, "xrLblVariant");
            this.xrLblVariant.Name = "xrLblVariant";
            this.xrLblVariant.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblVariant.StylePriority.UseFont = false;
            this.xrLblVariant.WordWrap = false;
            // 
            // xrLblItem
            // 
            this.xrLblItem.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "VendorItemID")});
            resources.ApplyResources(this.xrLblItem, "xrLblItem");
            this.xrLblItem.Name = "xrLblItem";
            this.xrLblItem.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItem.StylePriority.UseFont = false;
            this.xrLblItem.WordWrap = false;
            // 
            // xrLblItemName
            // 
            this.xrLblItemName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemName")});
            resources.ApplyResources(this.xrLblItemName, "xrLblItemName");
            this.xrLblItemName.Name = "xrLblItemName";
            this.xrLblItemName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItemName.StylePriority.UseFont = false;
            this.xrLblItemName.WordWrap = false;
            // 
            // TopMargin
            // 
            resources.ApplyResources(this.TopMargin, "TopMargin");
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrLabel1
            // 
            resources.ApplyResources(this.xrLabel1, "xrLabel1");
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            // 
            // BottomMargin
            // 
            resources.ApplyResources(this.BottomMargin, "BottomMargin");
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel10,
            this.xrPictureBox1,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1});
            resources.ApplyResources(this.ReportHeader, "ReportHeader");
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel25
            // 
            resources.ApplyResources(this.xrLabel25, "xrLabel25");
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.WordWrap = false;
            // 
            // xrLabel24
            // 
            resources.ApplyResources(this.xrLabel24, "xrLabel24");
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.WordWrap = false;
            // 
            // xrLabel23
            // 
            resources.ApplyResources(this.xrLabel23, "xrLabel23");
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.WordWrap = false;
            // 
            // xrLabel22
            // 
            resources.ApplyResources(this.xrLabel22, "xrLabel22");
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.WordWrap = false;
            // 
            // xrLabel21
            // 
            this.xrLabel21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.companyInfo, "Fax")});
            resources.ApplyResources(this.xrLabel21, "xrLabel21");
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // companyInfo
            // 
            this.companyInfo.DataSource = typeof(CompanyInfo);
            // 
            // xrLabel20
            // 
            this.xrLabel20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.companyInfo, "Phone")});
            resources.ApplyResources(this.xrLabel20, "xrLabel20");
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // xrLabel19
            // 
            resources.ApplyResources(this.xrLabel19, "xrLabel19");
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.WordWrap = false;
            // 
            // xrLabel10
            // 
            resources.ApplyResources(this.xrLabel10, "xrLabel10");
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.WordWrap = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", this.companyInfo, "CompanyLogo")});
            resources.ApplyResources(this.xrPictureBox1, "xrPictureBox1");
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabel9
            // 
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.purchaseOrder, "OrderingDateShortFormat")});
            resources.ApplyResources(this.xrLabel9, "xrLabel9");
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.StylePriority.UseFont = false;
            // 
            // purchaseOrder
            // 
            this.purchaseOrder.DataSource = typeof(PurchaseOrder);
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.purchaseOrder, "ID.StringValue")});
            resources.ApplyResources(this.xrLabel8, "xrLabel8");
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.StylePriority.UseFont = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.purchaseOrder, "DeliveryAddressFormatted")});
            resources.ApplyResources(this.xrLabel7, "xrLabel7");
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.StylePriority.UseFont = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.purchaseOrder, "StoreName")});
            resources.ApplyResources(this.xrLabel6, "xrLabel6");
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.StylePriority.UseFont = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.purchaseOrder, "VendorAddressFormatted")});
            resources.ApplyResources(this.xrLabel5, "xrLabel5");
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.StylePriority.UseFont = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.purchaseOrder, "VendorName")});
            resources.ApplyResources(this.xrLabel4, "xrLabel4");
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.StylePriority.UseFont = false;
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
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.companyInfo, "Text")});
            resources.ApplyResources(this.xrLabel2, "xrLabel2");
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.StylePriority.UseFont = false;
            // 
            // xrLblQuantityH
            // 
            resources.ApplyResources(this.xrLblQuantityH, "xrLblQuantityH");
            this.xrLblQuantityH.Name = "xrLblQuantityH";
            this.xrLblQuantityH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblQuantityH.StylePriority.UseBackColor = false;
            this.xrLblQuantityH.StylePriority.UseFont = false;
            this.xrLblQuantityH.StylePriority.UseTextAlignment = false;
            this.xrLblQuantityH.WordWrap = false;
            // 
            // xrLblVariantIDH
            // 
            resources.ApplyResources(this.xrLblVariantIDH, "xrLblVariantIDH");
            this.xrLblVariantIDH.Name = "xrLblVariantIDH";
            this.xrLblVariantIDH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblVariantIDH.StylePriority.UseBackColor = false;
            this.xrLblVariantIDH.StylePriority.UseFont = false;
            this.xrLblVariantIDH.WordWrap = false;
            // 
            // xrLblItemIDH
            // 
            resources.ApplyResources(this.xrLblItemIDH, "xrLblItemIDH");
            this.xrLblItemIDH.Name = "xrLblItemIDH";
            this.xrLblItemIDH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItemIDH.StylePriority.UseBackColor = false;
            this.xrLblItemIDH.StylePriority.UseFont = false;
            this.xrLblItemIDH.WordWrap = false;
            // 
            // xrLblItemNameH
            // 
            resources.ApplyResources(this.xrLblItemNameH, "xrLblItemNameH");
            this.xrLblItemNameH.Name = "xrLblItemNameH";
            this.xrLblItemNameH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItemNameH.StylePriority.UseBackColor = false;
            this.xrLblItemNameH.StylePriority.UseFont = false;
            this.xrLblItemNameH.WordWrap = false;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel30,
            this.xrLabel28,
            this.xrLabel26,
            this.xrLabel15,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLblItemIDH,
            this.xrLblItemNameH,
            this.xrLblVariantIDH,
            this.xrLblPriceH,
            this.xrLblQuantityH,
            this.xrLabel17});
            resources.ApplyResources(this.GroupHeader1, "GroupHeader1");
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel30
            // 
            resources.ApplyResources(this.xrLabel30, "xrLabel30");
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.StylePriority.UseBackColor = false;
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.WordWrap = false;
            // 
            // xrLabel28
            // 
            resources.ApplyResources(this.xrLabel28, "xrLabel28");
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.StylePriority.UseBackColor = false;
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.WordWrap = false;
            // 
            // xrLabel26
            // 
            resources.ApplyResources(this.xrLabel26, "xrLabel26");
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.StylePriority.UseBackColor = false;
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.WordWrap = false;
            // 
            // xrLabel15
            // 
            resources.ApplyResources(this.xrLabel15, "xrLabel15");
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.StylePriority.UseBackColor = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.WordWrap = false;
            // 
            // xrLabel12
            // 
            resources.ApplyResources(this.xrLabel12, "xrLabel12");
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.StylePriority.UseBackColor = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.WordWrap = false;
            // 
            // xrLabel11
            // 
            resources.ApplyResources(this.xrLabel11, "xrLabel11");
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.StylePriority.UseBackColor = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.WordWrap = false;
            // 
            // xrLblPriceH
            // 
            resources.ApplyResources(this.xrLblPriceH, "xrLblPriceH");
            this.xrLblPriceH.Name = "xrLblPriceH";
            this.xrLblPriceH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblPriceH.StylePriority.UseBackColor = false;
            this.xrLblPriceH.StylePriority.UseFont = false;
            this.xrLblPriceH.StylePriority.UseTextAlignment = false;
            this.xrLblPriceH.WordWrap = false;
            // 
            // xrLabel17
            // 
            resources.ApplyResources(this.xrLabel17, "xrLabel17");
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.StylePriority.UseBackColor = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.WordWrap = false;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreport1,
            this.xrLine2,
            this.lblTotalPrice,
            this.winControlContainer6});
            resources.ApplyResources(this.ReportFooter, "ReportFooter");
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.StylePriority.UseBorders = false;
            // 
            // xrSubreport1
            // 
            resources.ApplyResources(this.xrSubreport1, "xrSubreport1");
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.ReportSource = this.poMiscChargesSubReport1;
            // 
            // xrLine2
            // 
            resources.ApplyResources(this.xrLine2, "xrLine2");
            this.xrLine2.Name = "xrLine2";
            // 
            // lblTotalPrice
            // 
            resources.ApplyResources(this.lblTotalPrice, "lblTotalPrice");
            this.lblTotalPrice.Name = "lblTotalPrice";
            this.lblTotalPrice.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalPrice.StylePriority.UseFont = false;
            this.lblTotalPrice.StylePriority.UseTextAlignment = false;
            // 
            // winControlContainer6
            // 
            resources.ApplyResources(this.winControlContainer6, "winControlContainer6");
            this.winControlContainer6.Name = "winControlContainer6";
            this.winControlContainer6.WinControl = this.label6;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // purchaseOrderLines
            // 
            this.purchaseOrderLines.DataSource = typeof(PurchaseOrderLine);
            // 
            // xrLabel32
            // 
            this.xrLabel32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "UnitName")});
            resources.ApplyResources(this.xrLabel32, "xrLabel32");
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.WordWrap = false;
            // 
            // PurchaseOrderReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1,
            this.ReportFooter});
            this.DataSource = this.purchaseOrderLines;
            this.Landscape = true;
            resources.ApplyResources(this, "$this");
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "10.1";
            ((System.ComponentModel.ISupportInitialize)(this.companyInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.poMiscChargesSubReport1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseOrderLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLblQuantityH;
        private DevExpress.XtraReports.UI.XRLabel xrLblVariantIDH;
        private DevExpress.XtraReports.UI.XRLabel xrLblItemIDH;
        private DevExpress.XtraReports.UI.XRLabel xrLblItemNameH;
        private DevExpress.XtraReports.UI.XRLabel xrLblQuantity;
        private DevExpress.XtraReports.UI.XRLabel xrLblVariant;
        private DevExpress.XtraReports.UI.XRLabel xrLblItem;
        private DevExpress.XtraReports.UI.XRLabel xrLblItemName;
        private System.Windows.Forms.BindingSource purchaseOrder;
        private System.Windows.Forms.BindingSource purchaseOrderLines;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.WinControlContainer winControlContainer6;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraReports.UI.XRLabel xrLblPrice;
        private DevExpress.XtraReports.UI.XRLabel xrLblPriceH;
        private DevExpress.XtraReports.UI.XRLabel lblTotalPrice;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private System.Windows.Forms.BindingSource companyInfo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel24;
        private DevExpress.XtraReports.UI.XRLabel xrLabel23;
        private DevExpress.XtraReports.UI.XRLabel xrLabel22;
        private DevExpress.XtraReports.UI.XRLabel xrLabel25;
        private DevExpress.XtraReports.UI.XRLabel xrLabel31;
        private DevExpress.XtraReports.UI.XRLabel xrLabel29;
        private DevExpress.XtraReports.UI.XRLabel xrLabel27;
        private DevExpress.XtraReports.UI.XRLabel xrLabel30;
        private DevExpress.XtraReports.UI.XRLabel xrLabel28;
        private DevExpress.XtraReports.UI.XRLabel xrLabel26;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRSubreport xrSubreport1;
        private POMiscChargesSubReport poMiscChargesSubReport1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel32;
    }
}
