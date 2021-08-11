namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class StoreTransferReceivingItemDialog
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
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.lblQuantitySending = new System.Windows.Forms.Label();
            this.ntbSendingQuantity = new LSOne.Controls.NumericTextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.lblVariant = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbItem = new LSOne.Controls.DualDataComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkReceiveAnother = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblReceivingQty = new System.Windows.Forms.Label();
            this.ntbReceivingQuantity = new LSOne.Controls.NumericTextBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBarcode
            // 
            this.txtBarcode.Enabled = false;
            this.txtBarcode.Location = new System.Drawing.Point(203, 75);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(261, 20);
            this.txtBarcode.TabIndex = 1;
            // 
            // lblBarcode
            // 
            this.lblBarcode.BackColor = System.Drawing.Color.Transparent;
            this.lblBarcode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblBarcode.Location = new System.Drawing.Point(4, 78);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(193, 18);
            this.lblBarcode.TabIndex = 0;
            this.lblBarcode.Text = "Barcode:";
            this.lblBarcode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblQuantitySending
            // 
            this.lblQuantitySending.BackColor = System.Drawing.Color.Transparent;
            this.lblQuantitySending.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblQuantitySending.Location = new System.Drawing.Point(4, 185);
            this.lblQuantitySending.Name = "lblQuantitySending";
            this.lblQuantitySending.Size = new System.Drawing.Size(193, 17);
            this.lblQuantitySending.TabIndex = 8;
            this.lblQuantitySending.Text = "Sending quantity:";
            this.lblQuantitySending.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ntbSendingQuantity
            // 
            this.ntbSendingQuantity.AllowDecimal = true;
            this.ntbSendingQuantity.AllowNegative = false;
            this.ntbSendingQuantity.CultureInfo = null;
            this.ntbSendingQuantity.DecimalLetters = 2;
            this.ntbSendingQuantity.Enabled = false;
            this.ntbSendingQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbSendingQuantity.HasMinValue = false;
            this.ntbSendingQuantity.Location = new System.Drawing.Point(203, 182);
            this.ntbSendingQuantity.MaxValue = 999999999999D;
            this.ntbSendingQuantity.MinValue = 0D;
            this.ntbSendingQuantity.Name = "ntbSendingQuantity";
            this.ntbSendingQuantity.Size = new System.Drawing.Size(261, 20);
            this.ntbSendingQuantity.TabIndex = 9;
            this.ntbSendingQuantity.Text = "0.00";
            this.ntbSendingQuantity.Value = 0D;
            // 
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblUnit.Enabled = false;
            this.lblUnit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblUnit.Location = new System.Drawing.Point(4, 158);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(193, 18);
            this.lblUnit.TabIndex = 6;
            this.lblUnit.Text = "Unit:";
            this.lblUnit.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            this.cmbUnit.Enabled = false;
            this.cmbUnit.Location = new System.Drawing.Point(203, 155);
            this.cmbUnit.MaxLength = 32767;
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.NoChangeAllowed = false;
            this.cmbUnit.OnlyDisplayID = false;
            this.cmbUnit.ReceiveKeyboardEvents = true;
            this.cmbUnit.RemoveList = null;
            this.cmbUnit.RowHeight = ((short)(22));
            this.cmbUnit.SecondaryData = null;
            this.cmbUnit.SelectedData = null;
            this.cmbUnit.SelectedDataID = null;
            this.cmbUnit.SelectionList = null;
            this.cmbUnit.Size = new System.Drawing.Size(261, 21);
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.TabIndex = 7;
            // 
            // lblVariant
            // 
            this.lblVariant.BackColor = System.Drawing.Color.Transparent;
            this.lblVariant.Enabled = false;
            this.lblVariant.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblVariant.Location = new System.Drawing.Point(4, 131);
            this.lblVariant.Name = "lblVariant";
            this.lblVariant.Size = new System.Drawing.Size(193, 18);
            this.lblVariant.TabIndex = 4;
            this.lblVariant.Text = "Variant:";
            this.lblVariant.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbVariant
            // 
            this.cmbVariant.AddList = null;
            this.cmbVariant.AllowKeyboardSelection = false;
            this.cmbVariant.Enabled = false;
            this.cmbVariant.Location = new System.Drawing.Point(203, 128);
            this.cmbVariant.MaxLength = 32767;
            this.cmbVariant.Name = "cmbVariant";
            this.cmbVariant.NoChangeAllowed = false;
            this.cmbVariant.OnlyDisplayID = false;
            this.cmbVariant.ReceiveKeyboardEvents = true;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.Size = new System.Drawing.Size(261, 21);
            this.cmbVariant.SkipIDColumn = true;
            this.cmbVariant.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(4, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Item:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbItem
            // 
            this.cmbItem.AddList = null;
            this.cmbItem.AllowKeyboardSelection = false;
            this.cmbItem.Enabled = false;
            this.cmbItem.EnableTextBox = true;
            this.cmbItem.Location = new System.Drawing.Point(203, 101);
            this.cmbItem.MaxLength = 32767;
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.NoChangeAllowed = false;
            this.cmbItem.OnlyDisplayID = false;
            this.cmbItem.ReceiveKeyboardEvents = true;
            this.cmbItem.RemoveList = null;
            this.cmbItem.RowHeight = ((short)(22));
            this.cmbItem.SecondaryData = null;
            this.cmbItem.SelectedData = null;
            this.cmbItem.SelectedDataID = null;
            this.cmbItem.SelectionList = null;
            this.cmbItem.ShowDropDownOnTyping = true;
            this.cmbItem.Size = new System.Drawing.Size(261, 21);
            this.cmbItem.SkipIDColumn = false;
            this.cmbItem.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.chkReceiveAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(-3, 249);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(561, 46);
            this.panel2.TabIndex = 12;
            // 
            // chkReceiveAnother
            // 
            this.chkReceiveAnother.AutoSize = true;
            this.chkReceiveAnother.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkReceiveAnother.Location = new System.Drawing.Point(15, 15);
            this.chkReceiveAnother.Name = "chkReceiveAnother";
            this.chkReceiveAnother.Size = new System.Drawing.Size(105, 17);
            this.chkReceiveAnother.TabIndex = 0;
            this.chkReceiveAnother.Text = "Receive another";
            this.chkReceiveAnother.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(386, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(467, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblReceivingQty
            // 
            this.lblReceivingQty.BackColor = System.Drawing.Color.Transparent;
            this.lblReceivingQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblReceivingQty.Location = new System.Drawing.Point(4, 211);
            this.lblReceivingQty.Name = "lblReceivingQty";
            this.lblReceivingQty.Size = new System.Drawing.Size(193, 17);
            this.lblReceivingQty.TabIndex = 10;
            this.lblReceivingQty.Text = "Receiving quantity:";
            this.lblReceivingQty.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ntbReceivingQuantity
            // 
            this.ntbReceivingQuantity.AllowDecimal = true;
            this.ntbReceivingQuantity.AllowNegative = false;
            this.ntbReceivingQuantity.CultureInfo = null;
            this.ntbReceivingQuantity.DecimalLetters = 2;
            this.ntbReceivingQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbReceivingQuantity.HasMinValue = false;
            this.ntbReceivingQuantity.Location = new System.Drawing.Point(203, 208);
            this.ntbReceivingQuantity.MaxValue = 999999999999D;
            this.ntbReceivingQuantity.MinValue = 0D;
            this.ntbReceivingQuantity.Name = "ntbReceivingQuantity";
            this.ntbReceivingQuantity.Size = new System.Drawing.Size(261, 20);
            this.ntbReceivingQuantity.TabIndex = 11;
            this.ntbReceivingQuantity.Text = "0.00";
            this.ntbReceivingQuantity.Value = 0D;
            this.ntbReceivingQuantity.TextChanged += new System.EventHandler(this.ntbReceivingQuantity_TextChanged);
            // 
            // StoreTransferReceivingItemDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(554, 295);
            this.Controls.Add(this.lblReceivingQty);
            this.Controls.Add(this.ntbReceivingQuantity);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.lblQuantitySending);
            this.Controls.Add(this.ntbSendingQuantity);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbItem);
            this.HasHelp = true;
            this.Header = "Select receiving quantity";
            this.Name = "StoreTransferReceivingItemDialog";
            this.Text = "Transfer order receive line";
            this.Controls.SetChildIndex(this.cmbItem, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.Controls.SetChildIndex(this.lblVariant, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.lblUnit, 0);
            this.Controls.SetChildIndex(this.ntbSendingQuantity, 0);
            this.Controls.SetChildIndex(this.lblQuantitySending, 0);
            this.Controls.SetChildIndex(this.lblBarcode, 0);
            this.Controls.SetChildIndex(this.txtBarcode, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.ntbReceivingQuantity, 0);
            this.Controls.SetChildIndex(this.lblReceivingQty, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.Label lblQuantitySending;
        private Controls.NumericTextBox ntbSendingQuantity;
        private System.Windows.Forms.Label lblUnit;
        private Controls.DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label lblVariant;
        private Controls.DualDataComboBox cmbVariant;
        private System.Windows.Forms.Label label3;
        private Controls.DualDataComboBox cmbItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkReceiveAnother;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblReceivingQty;
        private Controls.NumericTextBox ntbReceivingQuantity;
    }
}