namespace LSOne.ViewPlugins.LookupValues.Views
{
    partial class EFTMappingView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EFTMappingView));
            this.lblID = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.lblSchemeName = new System.Windows.Forms.Label();
            this.tbSchemeName = new System.Windows.Forms.TextBox();
            this.lblTenderType = new System.Windows.Forms.Label();
            this.cmbTenderType = new System.Windows.Forms.ComboBox();
            this.lblEnabled = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.lblCreated = new System.Windows.Forms.Label();
            this.lblCreatedValue = new System.Windows.Forms.Label();
            this.cmbCardType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLookupOrder = new System.Windows.Forms.Label();
            this.ntbLookupOrder = new LSOne.Controls.NumericTextBox();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Controls.Add(this.lblLookupOrder);
            this.pnlBottom.Controls.Add(this.ntbLookupOrder);
            this.pnlBottom.Controls.Add(this.cmbCardType);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.lblCreatedValue);
            this.pnlBottom.Controls.Add(this.lblCreated);
            this.pnlBottom.Controls.Add(this.chkEnabled);
            this.pnlBottom.Controls.Add(this.lblEnabled);
            this.pnlBottom.Controls.Add(this.cmbTenderType);
            this.pnlBottom.Controls.Add(this.lblTenderType);
            this.pnlBottom.Controls.Add(this.tbSchemeName);
            this.pnlBottom.Controls.Add(this.lblSchemeName);
            this.pnlBottom.Controls.Add(this.tbID);
            this.pnlBottom.Controls.Add(this.lblID);
            // 
            // lblID
            // 
            this.lblID.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblID, "lblID");
            this.lblID.Name = "lblID";
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            // 
            // lblSchemeName
            // 
            this.lblSchemeName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSchemeName, "lblSchemeName");
            this.lblSchemeName.Name = "lblSchemeName";
            // 
            // tbSchemeName
            // 
            resources.ApplyResources(this.tbSchemeName, "tbSchemeName");
            this.tbSchemeName.Name = "tbSchemeName";
            // 
            // lblTenderType
            // 
            this.lblTenderType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTenderType, "lblTenderType");
            this.lblTenderType.Name = "lblTenderType";
            // 
            // cmbTenderType
            // 
            this.cmbTenderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTenderType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTenderType, "cmbTenderType");
            this.cmbTenderType.Name = "cmbTenderType";
            this.cmbTenderType.Sorted = true;
            // 
            // lblEnabled
            // 
            this.lblEnabled.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblEnabled, "lblEnabled");
            this.lblEnabled.Name = "lblEnabled";
            // 
            // chkEnabled
            // 
            resources.ApplyResources(this.chkEnabled, "chkEnabled");
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // lblCreated
            // 
            this.lblCreated.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCreated, "lblCreated");
            this.lblCreated.Name = "lblCreated";
            // 
            // lblCreatedValue
            // 
            this.lblCreatedValue.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCreatedValue, "lblCreatedValue");
            this.lblCreatedValue.Name = "lblCreatedValue";
            // 
            // cmbCardType
            // 
            this.cmbCardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCardType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCardType, "cmbCardType");
            this.cmbCardType.Name = "cmbCardType";
            this.cmbCardType.Sorted = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblLookupOrder
            // 
            this.lblLookupOrder.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLookupOrder, "lblLookupOrder");
            this.lblLookupOrder.Name = "lblLookupOrder";
            // 
            // ntbLookupOrder
            // 
            this.ntbLookupOrder.AllowDecimal = true;
            this.ntbLookupOrder.AllowNegative = false;
            this.ntbLookupOrder.CultureInfo = null;
            this.ntbLookupOrder.DecimalLetters = 0;
            this.ntbLookupOrder.ForeColor = System.Drawing.Color.Black;
            this.ntbLookupOrder.HasMinValue = true;
            resources.ApplyResources(this.ntbLookupOrder, "ntbLookupOrder");
            this.ntbLookupOrder.MaxValue = 10000D;
            this.ntbLookupOrder.MinValue = 0D;
            this.ntbLookupOrder.Name = "ntbLookupOrder";
            this.ntbLookupOrder.Value = 0D;
            // 
            // EFTMappingView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 75;
            this.Name = "EFTMappingView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lblSchemeName;
        private System.Windows.Forms.TextBox tbSchemeName;
        private System.Windows.Forms.Label lblTenderType;
        private System.Windows.Forms.ComboBox cmbTenderType;
        private System.Windows.Forms.Label lblCreatedValue;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label lblEnabled;
        private System.Windows.Forms.ComboBox cmbCardType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLookupOrder;
        private Controls.NumericTextBox ntbLookupOrder;

    }
}
