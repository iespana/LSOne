namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class MoveToMainInventoryDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoveToMainInventoryDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.btnEditReasonCode = new LSOne.Controls.ContextButton();
            this.ntbQuantity = new LSOne.Controls.NumericTextBox();
            this.cmbReason = new LSOne.Controls.DualDataComboBox();
            this.lblVariantNumber = new System.Windows.Forms.Label();
            this.lblRelation = new System.Windows.Forms.Label();
            this.cmbVariantNumber = new LSOne.Controls.DualDataComboBox();
            this.cmbItem = new LSOne.Controls.DualDataComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAddReasonCode = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // 
            // btnEditReasonCode
            // 
            this.btnEditReasonCode.BackColor = System.Drawing.Color.Transparent;
            this.btnEditReasonCode.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditReasonCode, "btnEditReasonCode");
            this.btnEditReasonCode.Name = "btnEditReasonCode";
            this.btnEditReasonCode.Click += new System.EventHandler(this.btnAddEditReasonCode_Click);
            // 
            // ntbQuantity
            // 
            this.ntbQuantity.AllowDecimal = false;
            this.ntbQuantity.AllowNegative = false;
            this.ntbQuantity.CultureInfo = null;
            this.ntbQuantity.DecimalLetters = 2;
            this.ntbQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbQuantity, "ntbQuantity");
            this.ntbQuantity.MaxValue = 100000D;
            this.ntbQuantity.MinValue = 0D;
            this.ntbQuantity.Name = "ntbQuantity";
            this.ntbQuantity.Value = 0D;
            this.ntbQuantity.TextChanged += new System.EventHandler(this.ntbQuantity_TextChanged);
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
            // lblVariantNumber
            // 
            this.lblVariantNumber.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.lblVariantNumber, "lblVariantNumber");
            this.lblVariantNumber.Name = "lblVariantNumber";
            // 
            // lblRelation
            // 
            this.lblRelation.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRelation, "lblRelation");
            this.lblRelation.Name = "lblRelation";
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
            // 
            // cmbItem
            // 
            this.cmbItem.AddList = null;
            this.cmbItem.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbItem, "cmbItem");
            this.cmbItem.EnableTextBox = true;
            this.cmbItem.MaxLength = 32767;
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.NoChangeAllowed = false;
            this.cmbItem.OnlyDisplayID = false;
            this.cmbItem.RemoveList = null;
            this.cmbItem.RowHeight = ((short)(22));
            this.cmbItem.SecondaryData = null;
            this.cmbItem.SelectedData = null;
            this.cmbItem.SelectedDataID = null;
            this.cmbItem.SelectionList = null;
            this.cmbItem.ShowDropDownOnTyping = true;
            this.cmbItem.SkipIDColumn = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Name = "panel2";
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
            // btnAddReasonCode
            // 
            this.btnAddReasonCode.BackColor = System.Drawing.Color.Transparent;
            this.btnAddReasonCode.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddReasonCode, "btnAddReasonCode");
            this.btnAddReasonCode.Name = "btnAddReasonCode";
            this.btnAddReasonCode.Click += new System.EventHandler(this.btnAddEditReasonCode_Click);
            // 
            // MoveToMainInventoryDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddReasonCode);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.btnEditReasonCode);
            this.Controls.Add(this.ntbQuantity);
            this.Controls.Add(this.cmbReason);
            this.Controls.Add(this.lblVariantNumber);
            this.Controls.Add(this.lblRelation);
            this.Controls.Add(this.cmbVariantNumber);
            this.Controls.Add(this.cmbItem);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.HasHelp = true;
            this.Name = "MoveToMainInventoryDialog";
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cmbItem, 0);
            this.Controls.SetChildIndex(this.cmbVariantNumber, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.lblVariantNumber, 0);
            this.Controls.SetChildIndex(this.cmbReason, 0);
            this.Controls.SetChildIndex(this.ntbQuantity, 0);
            this.Controls.SetChildIndex(this.btnEditReasonCode, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnAddReasonCode, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private Controls.DualDataComboBox cmbUnit;
        private Controls.ContextButton btnEditReasonCode;
        private Controls.NumericTextBox ntbQuantity;
        private Controls.DualDataComboBox cmbReason;
        private System.Windows.Forms.Label lblVariantNumber;
        private System.Windows.Forms.Label lblRelation;
        private Controls.DualDataComboBox cmbVariantNumber;
        private Controls.DualDataComboBox cmbItem;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Controls.ContextButton btnAddReasonCode;
    }
}