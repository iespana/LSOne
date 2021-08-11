using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    partial class CustomerGroupDiscountedPurchasesPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerGroupDiscountedPurchasesPage));
            this.lblPurchasePeriod = new System.Windows.Forms.Label();
            this.chkLimitDiscount = new System.Windows.Forms.CheckBox();
            this.lblMaxAmount = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbPurchasePeriod = new System.Windows.Forms.ComboBox();
            this.ntbMaxAmount = new LSOne.Controls.NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPurchasePeriod
            // 
            resources.ApplyResources(this.lblPurchasePeriod, "lblPurchasePeriod");
            this.lblPurchasePeriod.Name = "lblPurchasePeriod";
            // 
            // chkLimitDiscount
            // 
            resources.ApplyResources(this.chkLimitDiscount, "chkLimitDiscount");
            this.chkLimitDiscount.Name = "chkLimitDiscount";
            this.chkLimitDiscount.UseVisualStyleBackColor = true;
            this.chkLimitDiscount.CheckedChanged += new System.EventHandler(this.chkLimitDiscount_CheckedChanged);
            // 
            // lblMaxAmount
            // 
            resources.ApplyResources(this.lblMaxAmount, "lblMaxAmount");
            this.lblMaxAmount.Name = "lblMaxAmount";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbPurchasePeriod);
            this.groupBox1.Controls.Add(this.ntbMaxAmount);
            this.groupBox1.Controls.Add(this.lblMaxAmount);
            this.groupBox1.Controls.Add(this.lblPurchasePeriod);
            this.groupBox1.Controls.Add(this.chkLimitDiscount);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbPurchasePeriod
            // 
            this.cmbPurchasePeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurchasePeriod.FormattingEnabled = true;
            this.cmbPurchasePeriod.Items.AddRange(new object[] {
            resources.GetString("cmbPurchasePeriod.Items"),
            resources.GetString("cmbPurchasePeriod.Items1"),
            resources.GetString("cmbPurchasePeriod.Items2"),
            resources.GetString("cmbPurchasePeriod.Items3")});
            resources.ApplyResources(this.cmbPurchasePeriod, "cmbPurchasePeriod");
            this.cmbPurchasePeriod.Name = "cmbPurchasePeriod";
            // 
            // ntbMaxAmount
            // 
            this.ntbMaxAmount.AllowDecimal = true;
            this.ntbMaxAmount.AllowNegative = false;
            this.ntbMaxAmount.CultureInfo = null;
            this.ntbMaxAmount.DecimalLetters = 0;
            this.ntbMaxAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxAmount, "ntbMaxAmount");
            this.ntbMaxAmount.MaxValue = 0D;
            this.ntbMaxAmount.MinValue = 0D;
            this.ntbMaxAmount.Name = "ntbMaxAmount";
            this.ntbMaxAmount.Value = 0D;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CustomerGroupDiscountedPurchasesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.Name = "CustomerGroupDiscountedPurchasesPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPurchasePeriod;
        private System.Windows.Forms.CheckBox chkLimitDiscount;
        private System.Windows.Forms.Label lblMaxAmount;
        private System.Windows.Forms.GroupBox groupBox1;
        private NumericTextBox ntbMaxAmount;
        private System.Windows.Forms.ComboBox cmbPurchasePeriod;
        private System.Windows.Forms.Label label1;
    }
}
