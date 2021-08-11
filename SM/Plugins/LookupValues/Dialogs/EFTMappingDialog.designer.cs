using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class EFTMappingDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EFTMappingDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblLookupOrder = new System.Windows.Forms.Label();
            this.ntbLookupOrder = new LSOne.Controls.NumericTextBox();
            this.cmbCardType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCreatedValue = new System.Windows.Forms.Label();
            this.lblCreated = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.lblEnabled = new System.Windows.Forms.Label();
            this.cmbTenderType = new System.Windows.Forms.ComboBox();
            this.lblTenderType = new System.Windows.Forms.Label();
            this.tbSchemeName = new System.Windows.Forms.TextBox();
            this.lblSchemeName = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
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
            this.ntbLookupOrder.TextChanged += new System.EventHandler(CheckEnabled);
            // 
            // cmbCardType
            // 
            this.cmbCardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCardType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCardType, "cmbCardType");
            this.cmbCardType.Name = "cmbCardType";
            this.cmbCardType.Sorted = true;
            this.cmbCardType.SelectedIndexChanged += new System.EventHandler(CheckEnabled);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblCreatedValue
            // 
            this.lblCreatedValue.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCreatedValue, "lblCreatedValue");
            this.lblCreatedValue.Name = "lblCreatedValue";
            // 
            // lblCreated
            // 
            this.lblCreated.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCreated, "lblCreated");
            this.lblCreated.Name = "lblCreated";
            // 
            // chkEnabled
            // 
            resources.ApplyResources(this.chkEnabled, "chkEnabled");
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(CheckEnabled);
            // 
            // lblEnabled
            // 
            this.lblEnabled.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblEnabled, "lblEnabled");
            this.lblEnabled.Name = "lblEnabled";
            // 
            // cmbTenderType
            // 
            this.cmbTenderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTenderType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTenderType, "cmbTenderType");
            this.cmbTenderType.Name = "cmbTenderType";
            this.cmbTenderType.Sorted = true;
            this.cmbTenderType.SelectedIndexChanged += new System.EventHandler(CheckEnabled);
            // 
            // lblTenderType
            // 
            this.lblTenderType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTenderType, "lblTenderType");
            this.lblTenderType.Name = "lblTenderType";
            // 
            // tbSchemeName
            // 
            resources.ApplyResources(this.tbSchemeName, "tbSchemeName");
            this.tbSchemeName.Name = "tbSchemeName";
            this.tbSchemeName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblSchemeName
            // 
            this.lblSchemeName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSchemeName, "lblSchemeName");
            this.lblSchemeName.Name = "lblSchemeName";
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            this.tbID.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblID
            // 
            this.lblID.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblID, "lblID");
            this.lblID.Name = "lblID";
            // 
            // EFTMappingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblLookupOrder);
            this.Controls.Add(this.ntbLookupOrder);
            this.Controls.Add(this.cmbCardType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCreatedValue);
            this.Controls.Add(this.lblCreated);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.lblEnabled);
            this.Controls.Add(this.cmbTenderType);
            this.Controls.Add(this.lblTenderType);
            this.Controls.Add(this.tbSchemeName);
            this.Controls.Add(this.lblSchemeName);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "EFTMappingDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblID, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.Controls.SetChildIndex(this.lblSchemeName, 0);
            this.Controls.SetChildIndex(this.tbSchemeName, 0);
            this.Controls.SetChildIndex(this.lblTenderType, 0);
            this.Controls.SetChildIndex(this.cmbTenderType, 0);
            this.Controls.SetChildIndex(this.lblEnabled, 0);
            this.Controls.SetChildIndex(this.chkEnabled, 0);
            this.Controls.SetChildIndex(this.lblCreated, 0);
            this.Controls.SetChildIndex(this.lblCreatedValue, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbCardType, 0);
            this.Controls.SetChildIndex(this.ntbLookupOrder, 0);
            this.Controls.SetChildIndex(this.lblLookupOrder, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblLookupOrder;
        private NumericTextBox ntbLookupOrder;
        private System.Windows.Forms.ComboBox cmbCardType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCreatedValue;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label lblEnabled;
        private System.Windows.Forms.ComboBox cmbTenderType;
        private System.Windows.Forms.Label lblTenderType;
        private System.Windows.Forms.TextBox tbSchemeName;
        private System.Windows.Forms.Label lblSchemeName;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lblID;
    }
}