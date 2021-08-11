using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    partial class CustomerGroupLimitPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerGroupLimitPage));
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ntbPurchaseLimit = new LSOne.Controls.NumericTextBox();
            this.ntbDiscountedPurchases = new LSOne.Controls.NumericTextBox();
            this.tbCurrentPeriod = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblPriceIncludeTax = new System.Windows.Forms.Label();
            this.chkGroupUsesLimit = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ntbPurchaseLimit
            // 
            this.ntbPurchaseLimit.AllowDecimal = true;
            this.ntbPurchaseLimit.AllowNegative = false;
            this.ntbPurchaseLimit.CultureInfo = null;
            this.ntbPurchaseLimit.DecimalLetters = 2;
            resources.ApplyResources(this.ntbPurchaseLimit, "ntbPurchaseLimit");
            this.ntbPurchaseLimit.HasMinValue = true;
            this.ntbPurchaseLimit.MaxValue = 0D;
            this.ntbPurchaseLimit.MinValue = 0D;
            this.ntbPurchaseLimit.Name = "ntbPurchaseLimit";
            this.ntbPurchaseLimit.Value = 0D;
            // 
            // ntbDiscountedPurchases
            // 
            this.ntbDiscountedPurchases.AllowDecimal = true;
            this.ntbDiscountedPurchases.AllowNegative = false;
            this.ntbDiscountedPurchases.CultureInfo = null;
            this.ntbDiscountedPurchases.DecimalLetters = 2;
            resources.ApplyResources(this.ntbDiscountedPurchases, "ntbDiscountedPurchases");
            this.ntbDiscountedPurchases.HasMinValue = true;
            this.ntbDiscountedPurchases.MaxValue = 0D;
            this.ntbDiscountedPurchases.MinValue = 0D;
            this.ntbDiscountedPurchases.Name = "ntbDiscountedPurchases";
            this.ntbDiscountedPurchases.Value = 0D;
            // 
            // tbCurrentPeriod
            // 
            resources.ApplyResources(this.tbCurrentPeriod, "tbCurrentPeriod");
            this.tbCurrentPeriod.Name = "tbCurrentPeriod";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblPriceIncludeTax
            // 
            resources.ApplyResources(this.lblPriceIncludeTax, "lblPriceIncludeTax");
            this.lblPriceIncludeTax.Name = "lblPriceIncludeTax";
            // 
            // chkGroupUsesLimit
            // 
            resources.ApplyResources(this.chkGroupUsesLimit, "chkGroupUsesLimit");
            this.chkGroupUsesLimit.Name = "chkGroupUsesLimit";
            this.chkGroupUsesLimit.UseVisualStyleBackColor = true;
            // 
            // CustomerGroupLimitPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblPriceIncludeTax);
            this.Controls.Add(this.chkGroupUsesLimit);
            this.Controls.Add(this.tbCurrentPeriod);
            this.Controls.Add(this.ntbDiscountedPurchases);
            this.Controls.Add(this.ntbPurchaseLimit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Name = "CustomerGroupLimitPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private NumericTextBox ntbPurchaseLimit;
        private NumericTextBox ntbDiscountedPurchases;
        private System.Windows.Forms.TextBox tbCurrentPeriod;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblPriceIncludeTax;
        private System.Windows.Forms.CheckBox chkGroupUsesLimit;
    }
}
