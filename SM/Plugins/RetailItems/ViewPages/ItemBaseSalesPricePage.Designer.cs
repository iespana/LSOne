using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemBaseSalesPricePage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemBaseSalesPricePage));
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDiscountPercent = new System.Windows.Forms.Label();
            this.ntbProfitMargin = new LSOne.Controls.NumericTextBox();
            this.ntbCostPrice = new LSOne.Controls.NumericTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ntbSalesPriceMiscCharges = new LSOne.Controls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTruncate = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider3 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider4 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label19 = new System.Windows.Forms.Label();
            this.ntbSalesPriceWithVAT = new LSOne.Controls.NumericTextBox();
            this.ntbSalesPrice = new LSOne.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlTaxGroupChanged = new LSOne.Controls.GroupPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).BeginInit();
            this.pnlTaxGroupChanged.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Mode = LSOne.Controls.LinkFields.ModeEnum.Quad;
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lblDiscountPercent
            // 
            this.lblDiscountPercent.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDiscountPercent, "lblDiscountPercent");
            this.lblDiscountPercent.Name = "lblDiscountPercent";
            // 
            // ntbProfitMargin
            // 
            this.ntbProfitMargin.AcceptsTab = true;
            this.ntbProfitMargin.AllowDecimal = true;
            this.ntbProfitMargin.AllowNegative = false;
            this.ntbProfitMargin.BackColor = System.Drawing.SystemColors.Window;
            this.ntbProfitMargin.CultureInfo = null;
            this.ntbProfitMargin.DecimalLetters = 2;
            this.ntbProfitMargin.ForeColor = System.Drawing.Color.Black;
            this.ntbProfitMargin.HasMinValue = false;
            resources.ApplyResources(this.ntbProfitMargin, "ntbProfitMargin");
            this.ntbProfitMargin.MaxValue = 100D;
            this.ntbProfitMargin.MinValue = 0D;
            this.ntbProfitMargin.Name = "ntbProfitMargin";
            this.ntbProfitMargin.Value = 0D;
            this.ntbProfitMargin.TextChanged += new System.EventHandler(this.ntbProfitMargin_TextChanged);
            this.ntbProfitMargin.Leave += new System.EventHandler(this.ntbProfitMargin_Leave);
            // 
            // ntbCostPrice
            // 
            this.ntbCostPrice.AllowDecimal = true;
            this.ntbCostPrice.AllowNegative = false;
            this.ntbCostPrice.CultureInfo = null;
            this.ntbCostPrice.DecimalLetters = 2;
            this.ntbCostPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbCostPrice.HasMinValue = false;
            resources.ApplyResources(this.ntbCostPrice, "ntbCostPrice");
            this.ntbCostPrice.MaxValue = 9999999999999D;
            this.ntbCostPrice.MinValue = 0D;
            this.ntbCostPrice.Name = "ntbCostPrice";
            this.ntbCostPrice.Value = 0D;
            this.ntbCostPrice.Leave += new System.EventHandler(this.ntbCostPrice_Leave);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // ntbSalesPriceMiscCharges
            // 
            this.ntbSalesPriceMiscCharges.AllowDecimal = true;
            this.ntbSalesPriceMiscCharges.AllowNegative = false;
            this.ntbSalesPriceMiscCharges.CultureInfo = null;
            this.ntbSalesPriceMiscCharges.DecimalLetters = 2;
            this.ntbSalesPriceMiscCharges.ForeColor = System.Drawing.Color.Black;
            this.ntbSalesPriceMiscCharges.HasMinValue = false;
            resources.ApplyResources(this.ntbSalesPriceMiscCharges, "ntbSalesPriceMiscCharges");
            this.ntbSalesPriceMiscCharges.MaxValue = 9999999999999D;
            this.ntbSalesPriceMiscCharges.MinValue = 0D;
            this.ntbSalesPriceMiscCharges.Name = "ntbSalesPriceMiscCharges";
            this.ntbSalesPriceMiscCharges.Value = 0D;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnTruncate
            // 
            resources.ApplyResources(this.btnTruncate, "btnTruncate");
            this.btnTruncate.Name = "btnTruncate";
            this.btnTruncate.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // errorProvider3
            // 
            this.errorProvider3.ContainerControl = this;
            // 
            // errorProvider4
            // 
            this.errorProvider4.ContainerControl = this;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // ntbSalesPriceWithVAT
            // 
            this.ntbSalesPriceWithVAT.AllowDecimal = true;
            this.ntbSalesPriceWithVAT.AllowNegative = false;
            this.ntbSalesPriceWithVAT.CultureInfo = null;
            this.ntbSalesPriceWithVAT.DecimalLetters = 2;
            this.ntbSalesPriceWithVAT.ForeColor = System.Drawing.Color.Black;
            this.ntbSalesPriceWithVAT.HasMinValue = false;
            resources.ApplyResources(this.ntbSalesPriceWithVAT, "ntbSalesPriceWithVAT");
            this.ntbSalesPriceWithVAT.MaxValue = 9999999999999D;
            this.ntbSalesPriceWithVAT.MinValue = 0D;
            this.ntbSalesPriceWithVAT.Name = "ntbSalesPriceWithVAT";
            this.ntbSalesPriceWithVAT.Value = 0D;
            this.ntbSalesPriceWithVAT.TextChanged += new System.EventHandler(this.ntbSalesPriceWithVAT_TextChanged);
            this.ntbSalesPriceWithVAT.Leave += new System.EventHandler(this.ntbSalesPriceWithVAT_CalculateAndSetPriceWithoutTax);
            // 
            // ntbSalesPrice
            // 
            this.ntbSalesPrice.AllowDecimal = true;
            this.ntbSalesPrice.AllowNegative = false;
            this.ntbSalesPrice.CultureInfo = null;
            this.ntbSalesPrice.DecimalLetters = 2;
            this.ntbSalesPrice.ForeColor = System.Drawing.Color.Black;
            this.ntbSalesPrice.HasMinValue = false;
            resources.ApplyResources(this.ntbSalesPrice, "ntbSalesPrice");
            this.ntbSalesPrice.MaxValue = 9999999999999D;
            this.ntbSalesPrice.MinValue = 0D;
            this.ntbSalesPrice.Name = "ntbSalesPrice";
            this.ntbSalesPrice.Value = 0D;
            this.ntbSalesPrice.ValueChanged += new System.EventHandler(this.ntbSalesPrice_ValueChanged);
            this.ntbSalesPrice.TextChanged += new System.EventHandler(this.ntbSalesPrice_TextChanged);
            this.ntbSalesPrice.Leave += new System.EventHandler(this.ntbSalesPrice_CalculateAndSetPriceWithTax);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // pnlTaxGroupChanged
            // 
            resources.ApplyResources(this.pnlTaxGroupChanged, "pnlTaxGroupChanged");
            this.pnlTaxGroupChanged.Controls.Add(this.label2);
            this.pnlTaxGroupChanged.Controls.Add(this.label1);
            this.pnlTaxGroupChanged.Name = "pnlTaxGroupChanged";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ItemBaseSalesPricePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlTaxGroupChanged);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblDiscountPercent);
            this.Controls.Add(this.ntbProfitMargin);
            this.Controls.Add(this.ntbCostPrice);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ntbSalesPriceMiscCharges);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTruncate);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.ntbSalesPriceWithVAT);
            this.Controls.Add(this.ntbSalesPrice);
            this.Controls.Add(this.label6);
            this.DoubleBuffered = true;
            this.Name = "ItemBaseSalesPricePage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).EndInit();
            this.pnlTaxGroupChanged.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LinkFields linkFields1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDiscountPercent;
        private NumericTextBox ntbProfitMargin;
        private NumericTextBox ntbCostPrice;
        private System.Windows.Forms.Label label8;
        private NumericTextBox ntbSalesPriceMiscCharges;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTruncate;
        private System.Windows.Forms.Label label19;
        private NumericTextBox ntbSalesPriceWithVAT;
        private NumericTextBox ntbSalesPrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ErrorProvider errorProvider3;
        private System.Windows.Forms.ErrorProvider errorProvider4;
        private GroupPanel pnlTaxGroupChanged;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
