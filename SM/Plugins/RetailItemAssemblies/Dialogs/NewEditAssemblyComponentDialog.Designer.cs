namespace LSOne.ViewPlugins.RetailItemAssemblies.Dialogs
{
    partial class NewEditAssemblyComponentDialog
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbCreateAnother = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbItem = new LSOne.Controls.DualDataComboBox();
            this.lblItem = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.ntbCostPerUnit = new LSOne.Controls.NumericTextBox();
            this.ntbQuantity = new LSOne.Controls.NumericTextBox();
            this.ntbTotalCost = new LSOne.Controls.NumericTextBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblCostPerUnit = new System.Windows.Forms.Label();
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.lblVariant = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.cbCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(-4, 253);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(650, 46);
            this.panel2.TabIndex = 13;
            // 
            // cbCreateAnother
            // 
            this.cbCreateAnother.AutoSize = true;
            this.cbCreateAnother.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbCreateAnother.Location = new System.Drawing.Point(16, 15);
            this.cbCreateAnother.Name = "cbCreateAnother";
            this.cbCreateAnother.Size = new System.Drawing.Size(84, 17);
            this.cbCreateAnother.TabIndex = 0;
            this.cbCreateAnother.Text = "Add another";
            this.cbCreateAnother.UseVisualStyleBackColor = true;
            this.cbCreateAnother.CheckedChanged += new System.EventHandler(this.cbCreateAnother_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(452, 11);
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
            this.btnCancel.Location = new System.Drawing.Point(533, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbItem
            // 
            this.cmbItem.AddList = null;
            this.cmbItem.AllowKeyboardSelection = false;
            this.cmbItem.EnableTextBox = true;
            this.cmbItem.Location = new System.Drawing.Point(200, 81);
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
            this.cmbItem.Size = new System.Drawing.Size(256, 21);
            this.cmbItem.SkipIDColumn = true;
            this.cmbItem.TabIndex = 2;
            this.cmbItem.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbItem_DropDown);
            this.cmbItem.SelectedDataChanged += new System.EventHandler(this.cmbItem_SelectedDataChanged);
            // 
            // lblItem
            // 
            this.lblItem.BackColor = System.Drawing.Color.Transparent;
            this.lblItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblItem.Location = new System.Drawing.Point(2, 78);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(192, 25);
            this.lblItem.TabIndex = 1;
            this.lblItem.Text = "Item:";
            this.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            this.cmbUnit.EnableTextBox = true;
            this.cmbUnit.Location = new System.Drawing.Point(200, 161);
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
            this.cmbUnit.ShowDropDownOnTyping = true;
            this.cmbUnit.Size = new System.Drawing.Size(256, 21);
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.TabIndex = 8;
            this.cmbUnit.RequestData += new System.EventHandler(this.cmbUnit_RequestData);
            this.cmbUnit.SelectedDataChanged += new System.EventHandler(this.cmbUnit_SelectedDataChanged);
            // 
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblUnit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblUnit.Location = new System.Drawing.Point(2, 158);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(192, 25);
            this.lblUnit.TabIndex = 7;
            this.lblUnit.Text = "Unit:";
            this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ntbCostPerUnit
            // 
            this.ntbCostPerUnit.AllowDecimal = true;
            this.ntbCostPerUnit.AllowNegative = false;
            this.ntbCostPerUnit.CultureInfo = null;
            this.ntbCostPerUnit.DecimalLetters = 2;
            this.ntbCostPerUnit.Enabled = false;
            this.ntbCostPerUnit.ForeColor = System.Drawing.Color.Black;
            this.ntbCostPerUnit.HasMinValue = false;
            this.ntbCostPerUnit.Location = new System.Drawing.Point(200, 188);
            this.ntbCostPerUnit.MaxValue = 0D;
            this.ntbCostPerUnit.MinValue = 0D;
            this.ntbCostPerUnit.Name = "ntbCostPerUnit";
            this.ntbCostPerUnit.Size = new System.Drawing.Size(256, 20);
            this.ntbCostPerUnit.TabIndex = 10;
            this.ntbCostPerUnit.Text = "0.00";
            this.ntbCostPerUnit.Value = 0D;
            // 
            // ntbQuantity
            // 
            this.ntbQuantity.AllowDecimal = true;
            this.ntbQuantity.AllowNegative = false;
            this.ntbQuantity.CultureInfo = null;
            this.ntbQuantity.DecimalLetters = 2;
            this.ntbQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbQuantity.HasMinValue = false;
            this.ntbQuantity.Location = new System.Drawing.Point(200, 135);
            this.ntbQuantity.MaxValue = 999999D;
            this.ntbQuantity.MinValue = 0D;
            this.ntbQuantity.Name = "ntbQuantity";
            this.ntbQuantity.Size = new System.Drawing.Size(256, 20);
            this.ntbQuantity.TabIndex = 6;
            this.ntbQuantity.Text = "0.00";
            this.ntbQuantity.Value = 0D;
            this.ntbQuantity.TextChanged += new System.EventHandler(this.ntbQuantity_TextChanged);
            // 
            // ntbTotalCost
            // 
            this.ntbTotalCost.AllowDecimal = true;
            this.ntbTotalCost.AllowNegative = false;
            this.ntbTotalCost.CultureInfo = null;
            this.ntbTotalCost.DecimalLetters = 2;
            this.ntbTotalCost.Enabled = false;
            this.ntbTotalCost.ForeColor = System.Drawing.Color.Black;
            this.ntbTotalCost.HasMinValue = false;
            this.ntbTotalCost.Location = new System.Drawing.Point(200, 214);
            this.ntbTotalCost.MaxValue = 0D;
            this.ntbTotalCost.MinValue = 0D;
            this.ntbTotalCost.Name = "ntbTotalCost";
            this.ntbTotalCost.Size = new System.Drawing.Size(256, 20);
            this.ntbTotalCost.TabIndex = 12;
            this.ntbTotalCost.Text = "0.00";
            this.ntbTotalCost.Value = 0D;
            // 
            // lblQuantity
            // 
            this.lblQuantity.BackColor = System.Drawing.Color.Transparent;
            this.lblQuantity.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblQuantity.Location = new System.Drawing.Point(2, 135);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(192, 19);
            this.lblQuantity.TabIndex = 5;
            this.lblQuantity.Text = "Quantity:";
            this.lblQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCostPerUnit
            // 
            this.lblCostPerUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblCostPerUnit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCostPerUnit.Location = new System.Drawing.Point(2, 188);
            this.lblCostPerUnit.Name = "lblCostPerUnit";
            this.lblCostPerUnit.Size = new System.Drawing.Size(192, 19);
            this.lblCostPerUnit.TabIndex = 9;
            this.lblCostPerUnit.Text = "Cost per unit:";
            this.lblCostPerUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalCost.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTotalCost.Location = new System.Drawing.Point(2, 214);
            this.lblTotalCost.Name = "lblTotalCost";
            this.lblTotalCost.Size = new System.Drawing.Size(192, 19);
            this.lblTotalCost.TabIndex = 11;
            this.lblTotalCost.Text = "Total cost:";
            this.lblTotalCost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbVariant
            // 
            this.cmbVariant.AddList = null;
            this.cmbVariant.AllowKeyboardSelection = false;
            this.cmbVariant.Enabled = false;
            this.cmbVariant.EnableTextBox = true;
            this.cmbVariant.Location = new System.Drawing.Point(200, 108);
            this.cmbVariant.MaxLength = 32767;
            this.cmbVariant.Name = "cmbVariant";
            this.cmbVariant.NoChangeAllowed = false;
            this.cmbVariant.OnlyDisplayID = false;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.ShowDropDownOnTyping = true;
            this.cmbVariant.Size = new System.Drawing.Size(256, 21);
            this.cmbVariant.SkipIDColumn = true;
            this.cmbVariant.TabIndex = 4;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
            this.cmbVariant.RequestClear += new System.EventHandler(this.cmbVariant_RequestClear);
            // 
            // lblVariant
            // 
            this.lblVariant.BackColor = System.Drawing.Color.Transparent;
            this.lblVariant.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblVariant.Location = new System.Drawing.Point(2, 105);
            this.lblVariant.Name = "lblVariant";
            this.lblVariant.Size = new System.Drawing.Size(192, 25);
            this.lblVariant.TabIndex = 3;
            this.lblVariant.Text = "Variant:";
            this.lblVariant.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // NewEditAssemblyComponentDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(614, 295);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.ntbCostPerUnit);
            this.Controls.Add(this.ntbQuantity);
            this.Controls.Add(this.ntbTotalCost);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblCostPerUnit);
            this.Controls.Add(this.lblTotalCost);
            this.Controls.Add(this.cmbItem);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Header = "Select an item to add to the assembly";
            this.Name = "NewEditAssemblyComponentDialog";
            this.Text = "New assembly component";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblUnit, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.lblItem, 0);
            this.Controls.SetChildIndex(this.cmbItem, 0);
            this.Controls.SetChildIndex(this.lblTotalCost, 0);
            this.Controls.SetChildIndex(this.lblCostPerUnit, 0);
            this.Controls.SetChildIndex(this.lblQuantity, 0);
            this.Controls.SetChildIndex(this.ntbTotalCost, 0);
            this.Controls.SetChildIndex(this.ntbQuantity, 0);
            this.Controls.SetChildIndex(this.ntbCostPerUnit, 0);
            this.Controls.SetChildIndex(this.lblVariant, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox cbCreateAnother;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.DualDataComboBox cmbItem;
        private System.Windows.Forms.Label lblItem;
        private Controls.DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label lblUnit;
        private Controls.NumericTextBox ntbCostPerUnit;
        private Controls.NumericTextBox ntbQuantity;
        private Controls.NumericTextBox ntbTotalCost;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblCostPerUnit;
        private System.Windows.Forms.Label lblTotalCost;
        private Controls.DualDataComboBox cmbVariant;
        private System.Windows.Forms.Label lblVariant;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}