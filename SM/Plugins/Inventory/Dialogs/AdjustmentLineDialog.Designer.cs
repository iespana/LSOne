using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class AdjustmentLineDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdjustmentLineDialog));
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbVariantNumber = new LSOne.Controls.DualDataComboBox();
            this.cmbRelation = new LSOne.Controls.DualDataComboBox();
            this.lblVariantNumber = new System.Windows.Forms.Label();
            this.lblRelation = new System.Windows.Forms.Label();
            this.cmbReason = new LSOne.Controls.DualDataComboBox();
            this.ntbQuantity = new LSOne.Controls.NumericTextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbCreateAnother = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnEditReasonCode = new LSOne.Controls.ContextButton();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.btnAddReasonCode = new LSOne.Controls.ContextButton();
            this.pnlSwitch = new System.Windows.Forms.Panel();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.lblSwitch = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnlSwitch.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbVariantNumber
            // 
            this.cmbVariantNumber.AddList = null;
            this.cmbVariantNumber.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariantNumber, "cmbVariantNumber");
            this.cmbVariantNumber.MaxLength = 32767;
            this.cmbVariantNumber.Name = "cmbVariantNumber";
            this.cmbVariantNumber.NoChangeAllowed = false;
            this.cmbVariantNumber.OnlyDisplayID = false;
            this.cmbVariantNumber.RemoveList = null;
            this.cmbVariantNumber.RowHeight = ((short)(22));
            this.cmbVariantNumber.SecondaryData = null;
            this.cmbVariantNumber.SelectedData = null;
            this.cmbVariantNumber.SelectedDataID = null;
            this.cmbVariantNumber.SelectionList = null;
            this.cmbVariantNumber.SkipIDColumn = true;
            this.cmbVariantNumber.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariantNumber_DropDown);
            this.cmbVariantNumber.SelectedDataChanged += new System.EventHandler(this.cmbVariantNumber_SelectedDataChanged);
            // 
            // cmbRelation
            // 
            this.cmbRelation.AddList = null;
            this.cmbRelation.AllowKeyboardSelection = false;
            this.cmbRelation.EnableTextBox = true;
            resources.ApplyResources(this.cmbRelation, "cmbRelation");
            this.cmbRelation.MaxLength = 32767;
            this.cmbRelation.Name = "cmbRelation";
            this.cmbRelation.NoChangeAllowed = false;
            this.cmbRelation.OnlyDisplayID = false;
            this.cmbRelation.RemoveList = null;
            this.cmbRelation.RowHeight = ((short)(22));
            this.cmbRelation.SecondaryData = null;
            this.cmbRelation.SelectedData = null;
            this.cmbRelation.SelectedDataID = null;
            this.cmbRelation.SelectionList = null;
            this.cmbRelation.ShowDropDownOnTyping = true;
            this.cmbRelation.SkipIDColumn = false;
            this.cmbRelation.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRelation_DropDown);
            this.cmbRelation.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbRelation_FormatData);
            this.cmbRelation.SelectedDataChanged += new System.EventHandler(this.cmbRelation_SelectedDataChanged);
            this.cmbRelation.Leave += new System.EventHandler(this.cmbRelation_Leave);
            // 
            // lblVariantNumber
            // 
            resources.ApplyResources(this.lblVariantNumber, "lblVariantNumber");
            this.lblVariantNumber.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVariantNumber.Name = "lblVariantNumber";
            // 
            // lblRelation
            // 
            this.lblRelation.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRelation, "lblRelation");
            this.lblRelation.Name = "lblRelation";
            // 
            // cmbReason
            // 
            this.cmbReason.AddList = null;
            this.cmbReason.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbReason, "cmbReason");
            this.cmbReason.MaxLength = 32767;
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.NoChangeAllowed = false;
            this.cmbReason.OnlyDisplayID = false;
            this.cmbReason.RemoveList = null;
            this.cmbReason.RowHeight = ((short)(22));
            this.cmbReason.SecondaryData = null;
            this.cmbReason.SelectedData = null;
            this.cmbReason.SelectedDataID = null;
            this.cmbReason.SelectionList = null;
            this.cmbReason.SkipIDColumn = true;
            this.cmbReason.RequestData += new System.EventHandler(this.cmbReason_RequestData);
            this.cmbReason.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbReason_FormatData);
            this.cmbReason.SelectedDataChanged += new System.EventHandler(this.cmbReason_SelectedDataChanged);
            this.cmbReason.SelectedDataCleared += new System.EventHandler(this.cmbReason_SelectedDataCleared);
            // 
            // ntbQuantity
            // 
            this.ntbQuantity.AllowDecimal = true;
            this.ntbQuantity.AllowNegative = true;
            this.ntbQuantity.CultureInfo = null;
            this.ntbQuantity.DecimalLetters = 0;
            this.ntbQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbQuantity.HasMinValue = true;
            resources.ApplyResources(this.ntbQuantity, "ntbQuantity");
            this.ntbQuantity.MaxValue = 999999999999D;
            this.ntbQuantity.MinValue = 0D;
            this.ntbQuantity.Name = "ntbQuantity";
            this.ntbQuantity.Value = 0D;
            this.ntbQuantity.TextChanged += new System.EventHandler(this.ntbQuantity_TextChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.cbCreateAnother);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Name = "panel2";
            // 
            // cbCreateAnother
            // 
            resources.ApplyResources(this.cbCreateAnother, "cbCreateAnother");
            this.cbCreateAnother.Checked = true;
            this.cbCreateAnother.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCreateAnother.Name = "cbCreateAnother";
            this.cbCreateAnother.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnEditReasonCode
            // 
            this.btnEditReasonCode.BackColor = System.Drawing.Color.Transparent;
            this.btnEditReasonCode.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditReasonCode, "btnEditReasonCode");
            this.btnEditReasonCode.Name = "btnEditReasonCode";
            this.btnEditReasonCode.Click += new System.EventHandler(this.btnAddEditReasonCode_Click);
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.MaxLength = 32767;
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.NoChangeAllowed = false;
            this.cmbUnit.OnlyDisplayID = false;
            this.cmbUnit.RemoveList = null;
            this.cmbUnit.RowHeight = ((short)(22));
            this.cmbUnit.SecondaryData = null;
            this.cmbUnit.SelectedData = null;
            this.cmbUnit.SelectedDataID = null;
            this.cmbUnit.SelectionList = null;
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.RequestData += new System.EventHandler(this.cmbUnit_RequestData);
            this.cmbUnit.SelectedDataChanged += new System.EventHandler(this.cmbUnitId_SelectedDataChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lblBarcode
            // 
            this.lblBarcode.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBarcode, "lblBarcode");
            this.lblBarcode.Name = "lblBarcode";
            // 
            // tbBarcode
            // 
            this.tbBarcode.AcceptsTab = true;
            resources.ApplyResources(this.tbBarcode, "tbBarcode");
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbBarcode_MouseClick);
            this.tbBarcode.Enter += new System.EventHandler(this.tbBarcode_Enter);
            this.tbBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBarcode_KeyDown);
            this.tbBarcode.Leave += new System.EventHandler(this.tbBarcode_Leave);
            // 
            // btnAddReasonCode
            // 
            this.btnAddReasonCode.BackColor = System.Drawing.Color.Transparent;
            this.btnAddReasonCode.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddReasonCode, "btnAddReasonCode");
            this.btnAddReasonCode.Name = "btnAddReasonCode";
            this.btnAddReasonCode.Click += new System.EventHandler(this.btnAddEditReasonCode_Click);
            // 
            // pnlSwitch
            // 
            resources.ApplyResources(this.pnlSwitch, "pnlSwitch");
            this.pnlSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlSwitch.Controls.Add(this.btnSwitch);
            this.pnlSwitch.Controls.Add(this.lblSwitch);
            this.pnlSwitch.Name = "pnlSwitch";
            // 
            // btnSwitch
            // 
            resources.ApplyResources(this.btnSwitch, "btnSwitch");
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.UseVisualStyleBackColor = true;
            this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
            // 
            // lblSwitch
            // 
            this.lblSwitch.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSwitch, "lblSwitch");
            this.lblSwitch.Name = "lblSwitch";
            // 
            // AdjustmentLineDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.pnlSwitch);
            this.Controls.Add(this.btnAddReasonCode);
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.tbBarcode);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.btnEditReasonCode);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ntbQuantity);
            this.Controls.Add(this.cmbReason);
            this.Controls.Add(this.lblVariantNumber);
            this.Controls.Add(this.lblRelation);
            this.Controls.Add(this.cmbVariantNumber);
            this.Controls.Add(this.cmbRelation);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.KeyPreview = true;
            this.Name = "AdjustmentLineDialog";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AdjustmentLineDialog_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AdjustmentLineDialog_KeyUp);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cmbRelation, 0);
            this.Controls.SetChildIndex(this.cmbVariantNumber, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.lblVariantNumber, 0);
            this.Controls.SetChildIndex(this.cmbReason, 0);
            this.Controls.SetChildIndex(this.ntbQuantity, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnEditReasonCode, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.Controls.SetChildIndex(this.tbBarcode, 0);
            this.Controls.SetChildIndex(this.lblBarcode, 0);
            this.Controls.SetChildIndex(this.btnAddReasonCode, 0);
            this.Controls.SetChildIndex(this.pnlSwitch, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlSwitch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DualDataComboBox cmbVariantNumber;
        private DualDataComboBox cmbRelation;
        private System.Windows.Forms.Label lblVariantNumber;
        private System.Windows.Forms.Label lblRelation;
        private DualDataComboBox cmbReason;
        private NumericTextBox ntbQuantity;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private ContextButton btnEditReasonCode;
        private DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label label1;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.CheckBox cbCreateAnother;
        private ContextButton btnAddReasonCode;
        private System.Windows.Forms.Panel pnlSwitch;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.Label lblSwitch;
    }
}