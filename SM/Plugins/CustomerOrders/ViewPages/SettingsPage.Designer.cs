namespace LSOne.ViewPlugins.CustomerOrders.ViewPages
{
    partial class SettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPage));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lblPercent = new System.Windows.Forms.Label();
            this.cmbNumberSeries = new LSOne.Controls.DualDataComboBox();
            this.cmbExpiresIn = new System.Windows.Forms.ComboBox();
            this.ntbExpiresIn = new LSOne.Controls.NumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkSelectDelivery = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkSelectSource = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkDeposits = new LSOne.Controls.LinkFields();
            this.ntbMinDeposit = new LSOne.Controls.NumericTextBox();
            this.chkAcceptsDeposits = new System.Windows.Forms.CheckBox();
            this.lblMinDeposits = new System.Windows.Forms.Label();
            this.lblAcceptsDeposits = new System.Windows.Forms.Label();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.Controls.Add(this.lblPercent);
            this.groupPanel1.Controls.Add(this.cmbNumberSeries);
            this.groupPanel1.Controls.Add(this.cmbExpiresIn);
            this.groupPanel1.Controls.Add(this.ntbExpiresIn);
            this.groupPanel1.Controls.Add(this.label7);
            this.groupPanel1.Controls.Add(this.label6);
            this.groupPanel1.Controls.Add(this.chkSelectDelivery);
            this.groupPanel1.Controls.Add(this.label4);
            this.groupPanel1.Controls.Add(this.chkSelectSource);
            this.groupPanel1.Controls.Add(this.label3);
            this.groupPanel1.Controls.Add(this.lnkDeposits);
            this.groupPanel1.Controls.Add(this.ntbMinDeposit);
            this.groupPanel1.Controls.Add(this.chkAcceptsDeposits);
            this.groupPanel1.Controls.Add(this.lblMinDeposits);
            this.groupPanel1.Controls.Add(this.lblAcceptsDeposits);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lblPercent
            // 
            resources.ApplyResources(this.lblPercent, "lblPercent");
            this.lblPercent.Name = "lblPercent";
            // 
            // cmbNumberSeries
            // 
            this.cmbNumberSeries.AddList = null;
            this.cmbNumberSeries.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbNumberSeries, "cmbNumberSeries");
            this.cmbNumberSeries.MaxLength = 32767;
            this.cmbNumberSeries.Name = "cmbNumberSeries";
            this.cmbNumberSeries.NoChangeAllowed = false;
            this.cmbNumberSeries.OnlyDisplayID = false;
            this.cmbNumberSeries.RemoveList = null;
            this.cmbNumberSeries.RowHeight = ((short)(22));
            this.cmbNumberSeries.SecondaryData = null;
            this.cmbNumberSeries.SelectedData = null;
            this.cmbNumberSeries.SelectedDataID = null;
            this.cmbNumberSeries.SelectionList = null;
            this.cmbNumberSeries.SkipIDColumn = false;
            this.cmbNumberSeries.RequestData += new System.EventHandler(this.cmbNumberSeries_RequestData);
            this.cmbNumberSeries.RequestClear += new System.EventHandler(this.cmbNumberSeries_RequestClear);
            // 
            // cmbExpiresIn
            // 
            this.cmbExpiresIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpiresIn.FormattingEnabled = true;
            this.cmbExpiresIn.Items.AddRange(new object[] {
            resources.GetString("cmbExpiresIn.Items"),
            resources.GetString("cmbExpiresIn.Items1"),
            resources.GetString("cmbExpiresIn.Items2"),
            resources.GetString("cmbExpiresIn.Items3")});
            resources.ApplyResources(this.cmbExpiresIn, "cmbExpiresIn");
            this.cmbExpiresIn.Name = "cmbExpiresIn";
            // 
            // ntbExpiresIn
            // 
            this.ntbExpiresIn.AllowDecimal = false;
            this.ntbExpiresIn.AllowNegative = false;
            this.ntbExpiresIn.CultureInfo = null;
            this.ntbExpiresIn.DecimalLetters = 2;
            this.ntbExpiresIn.ForeColor = System.Drawing.Color.Black;
            this.ntbExpiresIn.HasMinValue = false;
            resources.ApplyResources(this.ntbExpiresIn, "ntbExpiresIn");
            this.ntbExpiresIn.MaxValue = 1000D;
            this.ntbExpiresIn.MinValue = 0D;
            this.ntbExpiresIn.Name = "ntbExpiresIn";
            this.ntbExpiresIn.Value = 1D;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkSelectDelivery
            // 
            resources.ApplyResources(this.chkSelectDelivery, "chkSelectDelivery");
            this.chkSelectDelivery.Name = "chkSelectDelivery";
            this.chkSelectDelivery.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkSelectSource
            // 
            resources.ApplyResources(this.chkSelectSource, "chkSelectSource");
            this.chkSelectSource.Name = "chkSelectSource";
            this.chkSelectSource.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lnkDeposits
            // 
            this.lnkDeposits.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lnkDeposits, "lnkDeposits");
            this.lnkDeposits.Name = "lnkDeposits";
            this.lnkDeposits.TabStop = false;
            // 
            // ntbMinDeposit
            // 
            this.ntbMinDeposit.AllowDecimal = false;
            this.ntbMinDeposit.AllowNegative = false;
            this.ntbMinDeposit.CultureInfo = null;
            this.ntbMinDeposit.DecimalLetters = 2;
            this.ntbMinDeposit.ForeColor = System.Drawing.Color.Black;
            this.ntbMinDeposit.HasMinValue = false;
            resources.ApplyResources(this.ntbMinDeposit, "ntbMinDeposit");
            this.ntbMinDeposit.MaxValue = 100D;
            this.ntbMinDeposit.MinValue = 0D;
            this.ntbMinDeposit.Name = "ntbMinDeposit";
            this.ntbMinDeposit.Value = 0D;
            // 
            // chkAcceptsDeposits
            // 
            resources.ApplyResources(this.chkAcceptsDeposits, "chkAcceptsDeposits");
            this.chkAcceptsDeposits.Name = "chkAcceptsDeposits";
            this.chkAcceptsDeposits.UseVisualStyleBackColor = true;
            this.chkAcceptsDeposits.CheckedChanged += new System.EventHandler(this.chkAcceptsDeposits_CheckedChanged);
            // 
            // lblMinDeposits
            // 
            resources.ApplyResources(this.lblMinDeposits, "lblMinDeposits");
            this.lblMinDeposits.Name = "lblMinDeposits";
            // 
            // lblAcceptsDeposits
            // 
            resources.ApplyResources(this.lblAcceptsDeposits, "lblAcceptsDeposits");
            this.lblAcceptsDeposits.Name = "lblAcceptsDeposits";
            // 
            // SettingsPage
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupPanel1);
            this.Name = "SettingsPage";
            resources.ApplyResources(this, "$this");
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblAcceptsDeposits;
        private System.Windows.Forms.CheckBox chkAcceptsDeposits;
        private System.Windows.Forms.Label lblMinDeposits;
        private Controls.NumericTextBox ntbMinDeposit;
        private Controls.LinkFields lnkDeposits;
        private System.Windows.Forms.CheckBox chkSelectDelivery;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkSelectSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbExpiresIn;
        private Controls.NumericTextBox ntbExpiresIn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private Controls.DualDataComboBox cmbNumberSeries;
        private System.Windows.Forms.Label lblPercent;
    }
}
