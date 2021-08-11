using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerLoyalty.Dialogs
{
    partial class LoyaltySchemeRuleDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoyaltySchemeRuleDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCreateAnother = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.laType = new System.Windows.Forms.Label();
            this.cmbMultType = new System.Windows.Forms.ComboBox();
            this.laMultType = new System.Windows.Forms.Label();
            this.laRelation = new System.Windows.Forms.Label();
            this.cmbRelation = new DualDataComboBox();
            this.dtpEndingDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartingDate = new System.Windows.Forms.DateTimePicker();
            this.laEndingDate = new System.Windows.Forms.Label();
            this.laStartingDate = new System.Windows.Forms.Label();
            this.laQtyAmt = new System.Windows.Forms.Label();
            this.laLoyPts = new System.Windows.Forms.Label();
            this.ntbQtyAmt = new NumericTextBox();
            this.ntbLoyPts = new NumericTextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnCreateAnother
            // 
            resources.ApplyResources(this.btnCreateAnother, "btnCreateAnother");
            this.btnCreateAnother.Name = "btnCreateAnother";
            this.btnCreateAnother.UseVisualStyleBackColor = true;
            this.btnCreateAnother.Click += new System.EventHandler(this.btnCreateAnother_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // laType
            // 
            this.laType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laType, "laType");
            this.laType.Name = "laType";
            // 
            // cmbMultType
            // 
            this.cmbMultType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMultType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbMultType, "cmbMultType");
            this.cmbMultType.Name = "cmbMultType";
            this.cmbMultType.SelectedIndexChanged += new System.EventHandler(this.cmbMultType_SelectedIndexChanged);
            // 
            // laMultType
            // 
            this.laMultType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laMultType, "laMultType");
            this.laMultType.Name = "laMultType";
            // 
            // laRelation
            // 
            this.laRelation.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laRelation, "laRelation");
            this.laRelation.Name = "laRelation";
            // 
            // cmbRelation
            // 
            this.cmbRelation.AddList = null;
            this.cmbRelation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRelation, "cmbRelation");
            this.cmbRelation.MaxLength = 32767;
            this.cmbRelation.Name = "cmbRelation";
            this.cmbRelation.RemoveList = null;
            this.cmbRelation.RowHeight = ((short)(22));
            this.cmbRelation.SelectedData = null;
            this.cmbRelation.SelectedDataID = null;
            this.cmbRelation.SelectionList = null;
            this.cmbRelation.SkipIDColumn = true;
            this.cmbRelation.RequestData += new System.EventHandler(this.cmbRelation_RequestData);
            this.cmbRelation.DropDown += new DropDownEventHandler(this.cmbRelation_DropDown);
            this.cmbRelation.SelectedDataChanged += new System.EventHandler(this.cmbRelation_SelectedDataChanged);
            // 
            // dtpEndingDate
            // 
            this.dtpEndingDate.Checked = false;
            this.dtpEndingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpEndingDate, "dtpEndingDate");
            this.dtpEndingDate.Name = "dtpEndingDate";
            this.dtpEndingDate.ShowCheckBox = true;
            this.dtpEndingDate.ValueChanged += new System.EventHandler(this.dtpEndingDate_ValueChanged);
            // 
            // dtpStartingDate
            // 
            this.dtpStartingDate.Checked = false;
            this.dtpStartingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpStartingDate, "dtpStartingDate");
            this.dtpStartingDate.Name = "dtpStartingDate";
            this.dtpStartingDate.ShowCheckBox = true;
            this.dtpStartingDate.ValueChanged += new System.EventHandler(this.dtpStartingDate_ValueChanged);
            // 
            // laEndingDate
            // 
            this.laEndingDate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laEndingDate, "laEndingDate");
            this.laEndingDate.Name = "laEndingDate";
            // 
            // laStartingDate
            // 
            this.laStartingDate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laStartingDate, "laStartingDate");
            this.laStartingDate.Name = "laStartingDate";
            // 
            // laQtyAmt
            // 
            this.laQtyAmt.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laQtyAmt, "laQtyAmt");
            this.laQtyAmt.Name = "laQtyAmt";
            // 
            // laLoyPts
            // 
            this.laLoyPts.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laLoyPts, "laLoyPts");
            this.laLoyPts.Name = "laLoyPts";
            // 
            // ntbQtyAmt
            // 
            this.ntbQtyAmt.AcceptsTab = true;
            this.ntbQtyAmt.AllowDecimal = false;
            this.ntbQtyAmt.AllowNegative = false;
            this.ntbQtyAmt.BackColor = System.Drawing.SystemColors.Window;
            this.ntbQtyAmt.CultureInfo = null;
            this.ntbQtyAmt.DecimalLetters = 2;
            this.ntbQtyAmt.HasMinValue = true;
            resources.ApplyResources(this.ntbQtyAmt, "ntbQtyAmt");
            this.ntbQtyAmt.MaxValue = 0D;
            this.ntbQtyAmt.MinValue = 0D;
            this.ntbQtyAmt.Name = "ntbQtyAmt";
            this.ntbQtyAmt.Value = 0D;
            this.ntbQtyAmt.TextChanged += new System.EventHandler(this.ntbQtyAmt_TextChanged);
            // 
            // ntbLoyPts
            // 
            this.ntbLoyPts.AcceptsTab = true;
            this.ntbLoyPts.AllowDecimal = true;
            this.ntbLoyPts.AllowNegative = false;
            this.ntbLoyPts.BackColor = System.Drawing.SystemColors.Window;
            this.ntbLoyPts.CultureInfo = null;
            this.ntbLoyPts.DecimalLetters = 2;
            this.ntbLoyPts.HasMinValue = false;
            resources.ApplyResources(this.ntbLoyPts, "ntbLoyPts");
            this.ntbLoyPts.MaxValue = 0D;
            this.ntbLoyPts.MinValue = 0D;
            this.ntbLoyPts.Name = "ntbLoyPts";
            this.ntbLoyPts.Value = 0D;
            this.ntbLoyPts.TextChanged += new System.EventHandler(this.ntbLoyPts_TextChanged);
            // 
            // LoyaltySchemeRuleDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ntbLoyPts);
            this.Controls.Add(this.ntbQtyAmt);
            this.Controls.Add(this.laLoyPts);
            this.Controls.Add(this.laQtyAmt);
            this.Controls.Add(this.dtpEndingDate);
            this.Controls.Add(this.dtpStartingDate);
            this.Controls.Add(this.laEndingDate);
            this.Controls.Add(this.laStartingDate);
            this.Controls.Add(this.laRelation);
            this.Controls.Add(this.cmbRelation);
            this.Controls.Add(this.cmbMultType);
            this.Controls.Add(this.laMultType);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.laType);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "LoyaltySchemeRuleDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.laType, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.laMultType, 0);
            this.Controls.SetChildIndex(this.cmbMultType, 0);
            this.Controls.SetChildIndex(this.cmbRelation, 0);
            this.Controls.SetChildIndex(this.laRelation, 0);
            this.Controls.SetChildIndex(this.laStartingDate, 0);
            this.Controls.SetChildIndex(this.laEndingDate, 0);
            this.Controls.SetChildIndex(this.dtpStartingDate, 0);
            this.Controls.SetChildIndex(this.dtpEndingDate, 0);
            this.Controls.SetChildIndex(this.laQtyAmt, 0);
            this.Controls.SetChildIndex(this.laLoyPts, 0);
            this.Controls.SetChildIndex(this.ntbQtyAmt, 0);
            this.Controls.SetChildIndex(this.ntbLoyPts, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.ComboBox cmbType;
		private System.Windows.Forms.Label laType;
		private System.Windows.Forms.ComboBox cmbMultType;
		private System.Windows.Forms.Label laMultType;
		private System.Windows.Forms.Label laRelation;
		private DualDataComboBox cmbRelation;
		private System.Windows.Forms.Label laLoyPts;
		private System.Windows.Forms.Label laQtyAmt;
		private System.Windows.Forms.DateTimePicker dtpEndingDate;
		private System.Windows.Forms.DateTimePicker dtpStartingDate;
		private System.Windows.Forms.Label laEndingDate;
		private System.Windows.Forms.Label laStartingDate;
		private System.Windows.Forms.Button btnCreateAnother;
		private NumericTextBox ntbQtyAmt;
		private NumericTextBox ntbLoyPts;
    }
}