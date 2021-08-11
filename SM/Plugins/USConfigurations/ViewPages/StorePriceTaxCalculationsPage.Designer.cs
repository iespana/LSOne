namespace LSOne.ViewPlugins.USConfigurations.ViewPages
{
    partial class StorePriceTaxCalculationsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StorePriceTaxCalculationsPage));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkKeyedPriceInclTax = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbCalculateDiscountsFrom = new System.Windows.Forms.ComboBox();
            this.chkDisplayAmountsWithTax = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbUseTaxGroupFrom = new System.Windows.Forms.ComboBox();
            this.chkUseTaxRounding = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDisplayBalanceWithTax = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // chkKeyedPriceInclTax
            // 
            resources.ApplyResources(this.chkKeyedPriceInclTax, "chkKeyedPriceInclTax");
            this.chkKeyedPriceInclTax.Name = "chkKeyedPriceInclTax";
            this.chkKeyedPriceInclTax.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmbCalculateDiscountsFrom
            // 
            this.cmbCalculateDiscountsFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCalculateDiscountsFrom.FormattingEnabled = true;
            this.cmbCalculateDiscountsFrom.Items.AddRange(new object[] {
            resources.GetString("cmbCalculateDiscountsFrom.Items"),
            resources.GetString("cmbCalculateDiscountsFrom.Items1")});
            resources.ApplyResources(this.cmbCalculateDiscountsFrom, "cmbCalculateDiscountsFrom");
            this.cmbCalculateDiscountsFrom.Name = "cmbCalculateDiscountsFrom";
            this.cmbCalculateDiscountsFrom.SelectedIndexChanged += new System.EventHandler(this.cmbCalculateDiscountsFrom_SelectedIndexChanged);
            // 
            // chkDisplayAmountsWithTax
            // 
            resources.ApplyResources(this.chkDisplayAmountsWithTax, "chkDisplayAmountsWithTax");
            this.chkDisplayAmountsWithTax.Name = "chkDisplayAmountsWithTax";
            this.chkDisplayAmountsWithTax.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbUseTaxGroupFrom
            // 
            this.cmbUseTaxGroupFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUseTaxGroupFrom.FormattingEnabled = true;
            this.cmbUseTaxGroupFrom.Items.AddRange(new object[] {
            resources.GetString("cmbUseTaxGroupFrom.Items"),
            resources.GetString("cmbUseTaxGroupFrom.Items1")});
            resources.ApplyResources(this.cmbUseTaxGroupFrom, "cmbUseTaxGroupFrom");
            this.cmbUseTaxGroupFrom.Name = "cmbUseTaxGroupFrom";
            // 
            // chkUseTaxRounding
            // 
            resources.ApplyResources(this.chkUseTaxRounding, "chkUseTaxRounding");
            this.chkUseTaxRounding.Name = "chkUseTaxRounding";
            this.chkUseTaxRounding.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkDisplayBalanceWithTax
            // 
            resources.ApplyResources(this.chkDisplayBalanceWithTax, "chkDisplayBalanceWithTax");
            this.chkDisplayBalanceWithTax.Name = "chkDisplayBalanceWithTax";
            this.chkDisplayBalanceWithTax.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // StorePriceTaxCalculationsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkDisplayBalanceWithTax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkUseTaxRounding);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbUseTaxGroupFrom);
            this.Controls.Add(this.chkDisplayAmountsWithTax);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbCalculateDiscountsFrom);
            this.Controls.Add(this.chkKeyedPriceInclTax);
            this.Controls.Add(this.label8);
            this.DoubleBuffered = true;
            this.Name = "StorePriceTaxCalculationsPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckBox chkKeyedPriceInclTax;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkDisplayAmountsWithTax;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbCalculateDiscountsFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbUseTaxGroupFrom;
        private System.Windows.Forms.CheckBox chkUseTaxRounding;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkDisplayBalanceWithTax;
        private System.Windows.Forms.Label label3;
    }
}
