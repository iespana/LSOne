using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerLoyalty.Dialogs
{
    partial class LoyaltySchemeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoyaltySchemeDialog));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.laDescription = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbExpTimeUnit = new System.Windows.Forms.ComboBox();
            this.laExpTimeUnit = new System.Windows.Forms.Label();
            this.laPtsUseLimit = new System.Windows.Forms.Label();
            this.laDefMultType = new System.Windows.Forms.Label();
            this.cmbDefMultType = new System.Windows.Forms.ComboBox();
            this.ntbExpTimeValue = new NumericTextBox();
            this.ntbPtsUseLimit = new NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new DualDataComboBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // laDescription
            // 
            resources.ApplyResources(this.laDescription, "laDescription");
            this.laDescription.Name = "laDescription";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
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
            // cmbExpTimeUnit
            // 
            this.cmbExpTimeUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpTimeUnit.FormattingEnabled = true;
            resources.ApplyResources(this.cmbExpTimeUnit, "cmbExpTimeUnit");
            this.cmbExpTimeUnit.Name = "cmbExpTimeUnit";
            this.cmbExpTimeUnit.SelectedIndexChanged += new System.EventHandler(this.cmbExpTimeUnit_SelectedIndexChanged);
            // 
            // laExpTimeUnit
            // 
            resources.ApplyResources(this.laExpTimeUnit, "laExpTimeUnit");
            this.laExpTimeUnit.Name = "laExpTimeUnit";
            // 
            // laPtsUseLimit
            // 
            resources.ApplyResources(this.laPtsUseLimit, "laPtsUseLimit");
            this.laPtsUseLimit.Name = "laPtsUseLimit";
            // 
            // laDefMultType
            // 
            resources.ApplyResources(this.laDefMultType, "laDefMultType");
            this.laDefMultType.Name = "laDefMultType";
            // 
            // cmbDefMultType
            // 
            this.cmbDefMultType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefMultType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDefMultType, "cmbDefMultType");
            this.cmbDefMultType.Name = "cmbDefMultType";
            this.cmbDefMultType.SelectedIndexChanged += new System.EventHandler(this.cmbDefMultType_SelectedIndexChanged);
            // 
            // ntbExpTimeValue
            // 
            this.ntbExpTimeValue.AcceptsTab = true;
            this.ntbExpTimeValue.AllowDecimal = false;
            this.ntbExpTimeValue.AllowNegative = false;
            this.ntbExpTimeValue.BackColor = System.Drawing.SystemColors.Window;
            this.ntbExpTimeValue.CultureInfo = null;
            this.ntbExpTimeValue.DecimalLetters = 0;
            this.ntbExpTimeValue.HasMinValue = true;
            resources.ApplyResources(this.ntbExpTimeValue, "ntbExpTimeValue");
            this.ntbExpTimeValue.MaxValue = 2147483647D;
            this.ntbExpTimeValue.MinValue = 1D;
            this.ntbExpTimeValue.Name = "ntbExpTimeValue";
            this.ntbExpTimeValue.Value = 0D;
            this.ntbExpTimeValue.TextChanged += new System.EventHandler(this.ntbExpTimeValue_TextChanged);
            // 
            // ntbPtsUseLimit
            // 
            this.ntbPtsUseLimit.AcceptsTab = true;
            this.ntbPtsUseLimit.AllowDecimal = false;
            this.ntbPtsUseLimit.AllowNegative = false;
            this.ntbPtsUseLimit.BackColor = System.Drawing.SystemColors.Window;
            this.ntbPtsUseLimit.CultureInfo = null;
            this.ntbPtsUseLimit.DecimalLetters = 0;
            this.ntbPtsUseLimit.HasMinValue = true;
            resources.ApplyResources(this.ntbPtsUseLimit, "ntbPtsUseLimit");
            this.ntbPtsUseLimit.MaxValue = 100D;
            this.ntbPtsUseLimit.MinValue = 0D;
            this.ntbPtsUseLimit.Name = "ntbPtsUseLimit";
            this.ntbPtsUseLimit.Value = 100D;
            this.ntbPtsUseLimit.TextChanged += new System.EventHandler(this.ntbPtsUseLimit_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.OnlyDisplayID = false;
            this.cmbCopyFrom.RemoveList = null;
            this.cmbCopyFrom.RowHeight = ((short)(22));
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SelectedDataID = null;
            this.cmbCopyFrom.SelectionList = null;
            this.cmbCopyFrom.SkipIDColumn = true;
            this.cmbCopyFrom.RequestData += new System.EventHandler(this.cmbCopyFrom_RequestData);
            this.cmbCopyFrom.RequestClear += new System.EventHandler(this.cmbCopyFrom_RequestClear);
            // 
            // LoyaltySchemeDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ntbPtsUseLimit);
            this.Controls.Add(this.ntbExpTimeValue);
            this.Controls.Add(this.laDefMultType);
            this.Controls.Add(this.cmbDefMultType);
            this.Controls.Add(this.laPtsUseLimit);
            this.Controls.Add(this.laExpTimeUnit);
            this.Controls.Add(this.cmbExpTimeUnit);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.laDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "LoyaltySchemeDialog";
            this.Controls.SetChildIndex(this.laDescription, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbExpTimeUnit, 0);
            this.Controls.SetChildIndex(this.laExpTimeUnit, 0);
            this.Controls.SetChildIndex(this.laPtsUseLimit, 0);
            this.Controls.SetChildIndex(this.cmbDefMultType, 0);
            this.Controls.SetChildIndex(this.laDefMultType, 0);
            this.Controls.SetChildIndex(this.ntbExpTimeValue, 0);
            this.Controls.SetChildIndex(this.ntbPtsUseLimit, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label laDescription;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label laDefMultType;
		private System.Windows.Forms.ComboBox cmbDefMultType;
		private System.Windows.Forms.Label laPtsUseLimit;
		private System.Windows.Forms.Label laExpTimeUnit;
		private System.Windows.Forms.ComboBox cmbExpTimeUnit;
		private NumericTextBox ntbExpTimeValue;
		private NumericTextBox ntbPtsUseLimit;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbCopyFrom;
    }
}