using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.EndOfDay.Reports
{
    partial class ReportFinVAT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportFinVAT));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel180 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel181 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel171 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel185 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel170 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel175 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel176 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel178 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine14 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel183 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLine15 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel186 = new DevExpress.XtraReports.UI.XRLabel();
            this.totalGrossAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.totalTaxAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.totalNetAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.taxGroupLines = new System.Windows.Forms.BindingSource(this.components);
            //((System.ComponentModel.ISupportInitialize)(this.datasetVAT1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.taxGroupLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel180,
            this.xrLabel181,
            this.xrLabel171,
            this.xrLabel185});
            resources.ApplyResources(this.Detail, "Detail");
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrLabel180
            // 
            this.xrLabel180.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormattedGrossAmount", "{0:n0}")});
            resources.ApplyResources(this.xrLabel180, "xrLabel180");
            this.xrLabel180.Multiline = true;
            this.xrLabel180.Name = "xrLabel180";
            this.xrLabel180.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel180.StylePriority.UseFont = false;
            this.xrLabel180.WordWrap = false;
            // 
            // xrLabel181
            // 
            this.xrLabel181.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SalesTaxGroupName")});
            resources.ApplyResources(this.xrLabel181, "xrLabel181");
            this.xrLabel181.Name = "xrLabel181";
            this.xrLabel181.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel181.StylePriority.UseFont = false;
            this.xrLabel181.StylePriority.UseTextAlignment = false;
            this.xrLabel181.WordWrap = false;
            // 
            // xrLabel171
            // 
            this.xrLabel171.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormattedTaxAmount", "{0:n0}")});
            resources.ApplyResources(this.xrLabel171, "xrLabel171");
            this.xrLabel171.Multiline = true;
            this.xrLabel171.Name = "xrLabel171";
            this.xrLabel171.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel171.StylePriority.UseFont = false;
            this.xrLabel171.WordWrap = false;
            // 
            // xrLabel185
            // 
            this.xrLabel185.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FormattedNetAmount", "{0:n0}")});
            resources.ApplyResources(this.xrLabel185, "xrLabel185");
            this.xrLabel185.Multiline = true;
            this.xrLabel185.Name = "xrLabel185";
            this.xrLabel185.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel185.StylePriority.UseFont = false;
            this.xrLabel185.WordWrap = false;
            // 
            // datasetVAT1
            // 
            //this.datasetVAT1.DataSetName = "DatasetVAT";
            //this.datasetVAT1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel170,
            this.xrLabel175,
            this.xrLabel176,
            this.xrLabel178,
            this.xrLine14,
            this.xrLabel183});
            resources.ApplyResources(this.PageHeader, "PageHeader");
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrLabel170
            // 
            resources.ApplyResources(this.xrLabel170, "xrLabel170");
            this.xrLabel170.Name = "xrLabel170";
            this.xrLabel170.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel170.StylePriority.UseTextAlignment = false;
            // 
            // xrLabel175
            // 
            resources.ApplyResources(this.xrLabel175, "xrLabel175");
            this.xrLabel175.Name = "xrLabel175";
            this.xrLabel175.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel175.StylePriority.UseFont = false;
            this.xrLabel175.StylePriority.UseTextAlignment = false;
            this.xrLabel175.WordWrap = false;
            // 
            // xrLabel176
            // 
            resources.ApplyResources(this.xrLabel176, "xrLabel176");
            this.xrLabel176.Multiline = true;
            this.xrLabel176.Name = "xrLabel176";
            this.xrLabel176.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel176.StylePriority.UseFont = false;
            this.xrLabel176.StylePriority.UseTextAlignment = false;
            this.xrLabel176.WordWrap = false;
            // 
            // xrLabel178
            // 
            resources.ApplyResources(this.xrLabel178, "xrLabel178");
            this.xrLabel178.Multiline = true;
            this.xrLabel178.Name = "xrLabel178";
            this.xrLabel178.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel178.StylePriority.UseFont = false;
            this.xrLabel178.StylePriority.UseTextAlignment = false;
            this.xrLabel178.WordWrap = false;
            // 
            // xrLine14
            // 
            resources.ApplyResources(this.xrLine14, "xrLine14");
            this.xrLine14.Name = "xrLine14";
            this.xrLine14.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrLabel183
            // 
            resources.ApplyResources(this.xrLabel183, "xrLabel183");
            this.xrLabel183.Multiline = true;
            this.xrLabel183.Name = "xrLabel183";
            this.xrLabel183.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel183.StylePriority.UseFont = false;
            this.xrLabel183.StylePriority.UseTextAlignment = false;
            this.xrLabel183.WordWrap = false;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine15,
            this.xrLabel186,
            this.totalGrossAmount,
            this.totalTaxAmount,
            this.totalNetAmount});
            resources.ApplyResources(this.PageFooter, "PageFooter");
            this.PageFooter.Name = "PageFooter";
            this.PageFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrLine15
            // 
            resources.ApplyResources(this.xrLine15, "xrLine15");
            this.xrLine15.Name = "xrLine15";
            this.xrLine15.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrLabel186
            // 
            resources.ApplyResources(this.xrLabel186, "xrLabel186");
            this.xrLabel186.Name = "xrLabel186";
            this.xrLabel186.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel186.StylePriority.UseFont = false;
            this.xrLabel186.WordWrap = false;
            // 
            // totalGrossAmount
            // 
            resources.ApplyResources(this.totalGrossAmount, "totalGrossAmount");
            this.totalGrossAmount.Multiline = true;
            this.totalGrossAmount.Name = "totalGrossAmount";
            this.totalGrossAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.totalGrossAmount.StylePriority.UseFont = false;
            // 
            // totalTaxAmount
            // 
            resources.ApplyResources(this.totalTaxAmount, "totalTaxAmount");
            this.totalTaxAmount.Multiline = true;
            this.totalTaxAmount.Name = "totalTaxAmount";
            this.totalTaxAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.totalTaxAmount.StylePriority.UseFont = false;
            // 
            // totalNetAmount
            // 
            resources.ApplyResources(this.totalNetAmount, "totalNetAmount");
            this.totalNetAmount.Multiline = true;
            this.totalNetAmount.Name = "totalNetAmount";
            this.totalNetAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.totalNetAmount.StylePriority.UseFont = false;
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // taxGroupLines
            // 
            this.taxGroupLines.DataSource = typeof(FinancialReportTaxGroupLine);
            // 
            // ReportFinVAT
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageHeader,
            this.PageFooter,
            this.topMarginBand1,
            this.bottomMarginBand1});
            this.DataSource = this.taxGroupLines;
            resources.ApplyResources(this, "$this");
            this.Version = "10.1";
            //((System.ComponentModel.ISupportInitialize)(this.datasetVAT1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.taxGroupLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel170;
        private DevExpress.XtraReports.UI.XRLabel xrLabel175;
        private DevExpress.XtraReports.UI.XRLabel xrLabel176;
        private DevExpress.XtraReports.UI.XRLabel xrLabel178;
        private DevExpress.XtraReports.UI.XRLine xrLine14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel183;
        private DevExpress.XtraReports.UI.XRLabel xrLabel180;
        private DevExpress.XtraReports.UI.XRLabel xrLabel181;
        private DevExpress.XtraReports.UI.XRLabel xrLabel171;
        private DevExpress.XtraReports.UI.XRLabel xrLabel185;
        private DevExpress.XtraReports.UI.XRLine xrLine15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel186;
        private DevExpress.XtraReports.UI.XRLabel totalGrossAmount;
        private DevExpress.XtraReports.UI.XRLabel totalTaxAmount;
        private DevExpress.XtraReports.UI.XRLabel totalNetAmount;
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
        private System.Windows.Forms.BindingSource taxGroupLines;
    

        public string TotalSaleWithVAT
        {
            set { totalGrossAmount.Text = value; }
        }
        public string TotalVAT
        {
            set { totalTaxAmount.Text = value; }
        }
        public string TotalSaleWithoutVAT
        {
            set { totalNetAmount.Text = value; }
        }
    }
}
