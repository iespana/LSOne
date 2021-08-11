using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.Inventory.Reports
{
    partial class GRDPickingListReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GRDPickingListReport));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblQuantity = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblPrice = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblVariant = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItem = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItemName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.winControlContainer2 = new DevExpress.XtraReports.UI.WinControlContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.goodsReceivingDocument = new System.Windows.Forms.BindingSource(this.components);
            this.xrLblQuantityH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblVariantIDH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItemIDH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblItemNameH = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblPriceH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.goodsReceivingDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel14,
            this.xrLabel13,
            this.xrLblQuantity,
            this.xrLblPrice,
            this.xrLblVariant,
            this.xrLblItem,
            this.xrLblItemName,
            this.xrLabel18});
            resources.ApplyResources(this.Detail, "Detail");
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrLabel14
            // 
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "purchaseOrderLine.StyleName")});
            resources.ApplyResources(this.xrLabel14, "xrLabel14");
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.WordWrap = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "purchaseOrderLine.SizeName")});
            resources.ApplyResources(this.xrLabel13, "xrLabel13");
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.WordWrap = false;
            // 
            // xrLblQuantity
            // 
            this.xrLblQuantity.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "purchaseOrderLine.FormatedQuantity")});
            resources.ApplyResources(this.xrLblQuantity, "xrLblQuantity");
            this.xrLblQuantity.Name = "xrLblQuantity";
            this.xrLblQuantity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblQuantity.StylePriority.UseFont = false;
            this.xrLblQuantity.StylePriority.UseTextAlignment = false;
            this.xrLblQuantity.WordWrap = false;
            // 
            // xrLblPrice
            // 
            this.xrLblPrice.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            resources.ApplyResources(this.xrLblPrice, "xrLblPrice");
            this.xrLblPrice.Name = "xrLblPrice";
            this.xrLblPrice.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblPrice.StylePriority.UseBorders = false;
            this.xrLblPrice.StylePriority.UseFont = false;
            this.xrLblPrice.StylePriority.UseTextAlignment = false;
            this.xrLblPrice.WordWrap = false;
            // 
            // xrLblVariant
            // 
            this.xrLblVariant.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "purchaseOrderLine.ColorName")});
            resources.ApplyResources(this.xrLblVariant, "xrLblVariant");
            this.xrLblVariant.Name = "xrLblVariant";
            this.xrLblVariant.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblVariant.StylePriority.UseFont = false;
            this.xrLblVariant.WordWrap = false;
            // 
            // xrLblItem
            // 
            this.xrLblItem.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "purchaseOrderLine.ItemID")});
            resources.ApplyResources(this.xrLblItem, "xrLblItem");
            this.xrLblItem.Name = "xrLblItem";
            this.xrLblItem.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItem.StylePriority.UseFont = false;
            this.xrLblItem.WordWrap = false;
            // 
            // xrLblItemName
            // 
            this.xrLblItemName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "purchaseOrderLine.ItemName")});
            resources.ApplyResources(this.xrLblItemName, "xrLblItemName");
            this.xrLblItemName.Name = "xrLblItemName";
            this.xrLblItemName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItemName.StylePriority.UseFont = false;
            this.xrLblItemName.WordWrap = false;
            // 
            // xrLabel18
            // 
            this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "purchaseOrderLine.UnitName")});
            resources.ApplyResources(this.xrLabel18, "xrLabel18");
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.WordWrap = false;
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
            this.xrLabel4,
            this.winControlContainer2,
            this.xrLabel8,
            this.xrLabel1});
            resources.ApplyResources(this.ReportHeader, "ReportHeader");
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel4
            // 
            resources.ApplyResources(this.xrLabel4, "xrLabel4");
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            // 
            // winControlContainer2
            // 
            resources.ApplyResources(this.winControlContainer2, "winControlContainer2");
            this.winControlContainer2.Name = "winControlContainer2";
            this.winControlContainer2.WinControl = this.label3;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.goodsReceivingDocument, "GoodsReceivingID.StringValue")});
            resources.ApplyResources(this.xrLabel8, "xrLabel8");
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.WordWrap = false;
            // 
            // goodsReceivingDocument
            // 
            this.goodsReceivingDocument.DataSource = typeof(GoodsReceivingDocument);
            // 
            // xrLblQuantityH
            // 
            resources.ApplyResources(this.xrLblQuantityH, "xrLblQuantityH");
            this.xrLblQuantityH.Name = "xrLblQuantityH";
            this.xrLblQuantityH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblQuantityH.StylePriority.UseFont = false;
            this.xrLblQuantityH.StylePriority.UseTextAlignment = false;
            // 
            // xrLblVariantIDH
            // 
            resources.ApplyResources(this.xrLblVariantIDH, "xrLblVariantIDH");
            this.xrLblVariantIDH.Name = "xrLblVariantIDH";
            this.xrLblVariantIDH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblVariantIDH.StylePriority.UseFont = false;
            // 
            // xrLblItemIDH
            // 
            resources.ApplyResources(this.xrLblItemIDH, "xrLblItemIDH");
            this.xrLblItemIDH.Name = "xrLblItemIDH";
            this.xrLblItemIDH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItemIDH.StylePriority.UseFont = false;
            // 
            // xrLblItemNameH
            // 
            resources.ApplyResources(this.xrLblItemNameH, "xrLblItemNameH");
            this.xrLblItemNameH.Name = "xrLblItemNameH";
            this.xrLblItemNameH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblItemNameH.StylePriority.UseFont = false;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel3,
            this.xrLabel2,
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
            // xrLabel12
            // 
            resources.ApplyResources(this.xrLabel12, "xrLabel12");
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.StylePriority.UseFont = false;
            // 
            // xrLabel11
            // 
            resources.ApplyResources(this.xrLabel11, "xrLabel11");
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.StylePriority.UseFont = false;
            // 
            // xrLblPriceH
            // 
            resources.ApplyResources(this.xrLblPriceH, "xrLblPriceH");
            this.xrLblPriceH.Name = "xrLblPriceH";
            this.xrLblPriceH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblPriceH.StylePriority.UseFont = false;
            this.xrLblPriceH.StylePriority.UseTextAlignment = false;
            this.xrLblPriceH.WordWrap = false;
            // 
            // xrLabel17
            // 
            resources.ApplyResources(this.xrLabel17, "xrLabel17");
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.StylePriority.UseFont = false;
            // 
            // xrLabel2
            // 
            resources.ApplyResources(this.xrLabel2, "xrLabel2");
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.WordWrap = false;
            // 
            // xrLabel3
            // 
            resources.ApplyResources(this.xrLabel3, "xrLabel3");
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.WordWrap = false;
            // 
            // xrLabel5
            // 
            resources.ApplyResources(this.xrLabel5, "xrLabel5");
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.WordWrap = false;
            // 
            // xrLabel6
            // 
            resources.ApplyResources(this.xrLabel6, "xrLabel6");
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.WordWrap = false;
            // 
            // xrLabel7
            // 
            resources.ApplyResources(this.xrLabel7, "xrLabel7");
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.WordWrap = false;
            // 
            // xrLabel9
            // 
            resources.ApplyResources(this.xrLabel9, "xrLabel9");
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.WordWrap = false;
            // 
            // xrLabel10
            // 
            resources.ApplyResources(this.xrLabel10, "xrLabel10");
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.WordWrap = false;
            // 
            // GRDPickingListReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1});
            resources.ApplyResources(this, "$this");
            this.Version = "10.1";
            ((System.ComponentModel.ISupportInitialize)(this.goodsReceivingDocument)).EndInit();
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
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.WinControlContainer winControlContainer2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLblPrice;
        private DevExpress.XtraReports.UI.XRLabel xrLblPriceH;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private System.Windows.Forms.BindingSource goodsReceivingDocument;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        //private System.Windows.Forms.BindingSource goodsReceivingDocumentLines;
    }
}
