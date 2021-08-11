namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class StoreTransferSendingItemDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTransferSendingItemDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateAnother = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblQuantitySending = new System.Windows.Forms.Label();
            this.ntbSendingQuantity = new LSOne.Controls.NumericTextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.lblVariant = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbItem = new LSOne.Controls.DualDataComboBox();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.lblInventory = new System.Windows.Forms.Label();
            this.btnInventory = new System.Windows.Forms.Button();
            this.pnlInventory = new System.Windows.Forms.Panel();
            this.lvItemInventory = new LSOne.Controls.ListView();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.clmInventory = new LSOne.Controls.Columns.Column();
            this.clmOrdered = new LSOne.Controls.Columns.Column();
            this.clmReserved = new LSOne.Controls.Columns.Column();
            this.clmParked = new LSOne.Controls.Columns.Column();
            this.lblRegion = new System.Windows.Forms.Label();
            this.cmbRegion = new LSOne.Controls.DualDataComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            this.pnlInventory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.chkCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // chkCreateAnother
            // 
            resources.ApplyResources(this.chkCreateAnother, "chkCreateAnother");
            this.chkCreateAnother.Checked = true;
            this.chkCreateAnother.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateAnother.Name = "chkCreateAnother";
            this.chkCreateAnother.UseVisualStyleBackColor = true;
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
            // lblQuantitySending
            // 
            this.lblQuantitySending.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblQuantitySending, "lblQuantitySending");
            this.lblQuantitySending.Name = "lblQuantitySending";
            // 
            // ntbSendingQuantity
            // 
            this.ntbSendingQuantity.AllowDecimal = true;
            this.ntbSendingQuantity.AllowNegative = false;
            this.ntbSendingQuantity.CultureInfo = null;
            this.ntbSendingQuantity.DecimalLetters = 2;
            this.ntbSendingQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbSendingQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbSendingQuantity, "ntbSendingQuantity");
            this.ntbSendingQuantity.MaxValue = 999999999999D;
            this.ntbSendingQuantity.MinValue = 0D;
            this.ntbSendingQuantity.Name = "ntbSendingQuantity";
            this.ntbSendingQuantity.Value = 0D;
            this.ntbSendingQuantity.TextChanged += new System.EventHandler(this.ntbSendingQuantity_TextChanged);
            this.ntbSendingQuantity.Leave += new System.EventHandler(this.ntbSendingQuantity_Leave);
            // 
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUnit, "lblUnit");
            this.lblUnit.Name = "lblUnit";
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
            this.cmbUnit.ReceiveKeyboardEvents = true;
            this.cmbUnit.RemoveList = null;
            this.cmbUnit.RowHeight = ((short)(22));
            this.cmbUnit.SecondaryData = null;
            this.cmbUnit.SelectedData = null;
            this.cmbUnit.SelectedDataID = null;
            this.cmbUnit.SelectionList = null;
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.RequestData += new System.EventHandler(this.cmbUnit_RequestData);
            this.cmbUnit.SelectedDataChanged += new System.EventHandler(this.cmbUnit_SelectedDataChanged);
            // 
            // lblVariant
            // 
            this.lblVariant.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblVariant, "lblVariant");
            this.lblVariant.Name = "lblVariant";
            // 
            // cmbVariant
            // 
            this.cmbVariant.AddList = null;
            this.cmbVariant.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariant, "cmbVariant");
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
            this.cmbVariant.SkipIDColumn = true;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbItem
            // 
            this.cmbItem.AddList = null;
            this.cmbItem.AllowKeyboardSelection = false;
            this.cmbItem.EnableTextBox = true;
            resources.ApplyResources(this.cmbItem, "cmbItem");
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
            this.cmbItem.SkipIDColumn = false;
            this.cmbItem.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbItem_DropDown);
            this.cmbItem.SelectedDataChanged += new System.EventHandler(this.cmbItem_SelectedDataChanged);
            // 
            // lblBarcode
            // 
            this.lblBarcode.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBarcode, "lblBarcode");
            this.lblBarcode.Name = "lblBarcode";
            // 
            // txtBarcode
            // 
            resources.ApplyResources(this.txtBarcode, "txtBarcode");
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Click += new System.EventHandler(this.txtBarcode_Click);
            this.txtBarcode.Enter += new System.EventHandler(this.txtBarcode_Enter);
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            this.txtBarcode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyUp);
            this.txtBarcode.Leave += new System.EventHandler(this.txtBarcode_Leave);
            // 
            // lblInventory
            // 
            this.lblInventory.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblInventory, "lblInventory");
            this.lblInventory.Name = "lblInventory";
            // 
            // btnInventory
            // 
            resources.ApplyResources(this.btnInventory, "btnInventory");
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.UseVisualStyleBackColor = true;
            this.btnInventory.Click += new System.EventHandler(this.btnInventory_Click);
            // 
            // pnlInventory
            // 
            this.pnlInventory.Controls.Add(this.lvItemInventory);
            this.pnlInventory.Controls.Add(this.lblRegion);
            this.pnlInventory.Controls.Add(this.cmbRegion);
            resources.ApplyResources(this.pnlInventory, "pnlInventory");
            this.pnlInventory.Name = "pnlInventory";
            // 
            // lvItemInventory
            // 
            resources.ApplyResources(this.lvItemInventory, "lvItemInventory");
            this.lvItemInventory.BuddyControl = null;
            this.lvItemInventory.Columns.Add(this.clmStore);
            this.lvItemInventory.Columns.Add(this.clmInventory);
            this.lvItemInventory.Columns.Add(this.clmOrdered);
            this.lvItemInventory.Columns.Add(this.clmReserved);
            this.lvItemInventory.Columns.Add(this.clmParked);
            this.lvItemInventory.ContentBackColor = System.Drawing.Color.White;
            this.lvItemInventory.DefaultRowHeight = ((short)(22));
            this.lvItemInventory.DimSelectionWhenDisabled = true;
            this.lvItemInventory.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItemInventory.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItemInventory.HeaderHeight = ((short)(25));
            this.lvItemInventory.Name = "lvItemInventory";
            this.lvItemInventory.OddRowColor = System.Drawing.Color.White;
            this.lvItemInventory.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItemInventory.SecondarySortColumn = ((short)(-1));
            this.lvItemInventory.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItemInventory.SortSetting = "0:1";
            // 
            // clmStore
            // 
            this.clmStore.AutoSize = true;
            this.clmStore.Clickable = false;
            this.clmStore.DefaultStyle = null;
            resources.ApplyResources(this.clmStore, "clmStore");
            this.clmStore.MaximumWidth = ((short)(0));
            this.clmStore.MinimumWidth = ((short)(10));
            this.clmStore.SecondarySortColumn = ((short)(-1));
            this.clmStore.Tag = null;
            this.clmStore.Width = ((short)(150));
            // 
            // clmInventory
            // 
            this.clmInventory.AutoSize = true;
            this.clmInventory.Clickable = false;
            this.clmInventory.DefaultStyle = null;
            resources.ApplyResources(this.clmInventory, "clmInventory");
            this.clmInventory.MaximumWidth = ((short)(0));
            this.clmInventory.MinimumWidth = ((short)(10));
            this.clmInventory.SecondarySortColumn = ((short)(-1));
            this.clmInventory.Tag = null;
            this.clmInventory.Width = ((short)(100));
            // 
            // clmOrdered
            // 
            this.clmOrdered.AutoSize = true;
            this.clmOrdered.Clickable = false;
            this.clmOrdered.DefaultStyle = null;
            resources.ApplyResources(this.clmOrdered, "clmOrdered");
            this.clmOrdered.MaximumWidth = ((short)(0));
            this.clmOrdered.MinimumWidth = ((short)(10));
            this.clmOrdered.SecondarySortColumn = ((short)(-1));
            this.clmOrdered.Tag = null;
            this.clmOrdered.Width = ((short)(75));
            // 
            // clmReserved
            // 
            this.clmReserved.AutoSize = true;
            this.clmReserved.Clickable = false;
            this.clmReserved.DefaultStyle = null;
            resources.ApplyResources(this.clmReserved, "clmReserved");
            this.clmReserved.MaximumWidth = ((short)(0));
            this.clmReserved.MinimumWidth = ((short)(10));
            this.clmReserved.SecondarySortColumn = ((short)(-1));
            this.clmReserved.Tag = null;
            this.clmReserved.Width = ((short)(75));
            // 
            // clmParked
            // 
            this.clmParked.AutoSize = true;
            this.clmParked.Clickable = false;
            this.clmParked.DefaultStyle = null;
            resources.ApplyResources(this.clmParked, "clmParked");
            this.clmParked.MaximumWidth = ((short)(0));
            this.clmParked.MinimumWidth = ((short)(10));
            this.clmParked.SecondarySortColumn = ((short)(-1));
            this.clmParked.Tag = null;
            this.clmParked.Width = ((short)(75));
            // 
            // lblRegion
            // 
            this.lblRegion.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRegion, "lblRegion");
            this.lblRegion.Name = "lblRegion";
            // 
            // cmbRegion
            // 
            this.cmbRegion.AddList = null;
            this.cmbRegion.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRegion, "cmbRegion");
            this.cmbRegion.MaxLength = 32767;
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.NoChangeAllowed = false;
            this.cmbRegion.OnlyDisplayID = false;
            this.cmbRegion.ReceiveKeyboardEvents = true;
            this.cmbRegion.RemoveList = null;
            this.cmbRegion.RowHeight = ((short)(22));
            this.cmbRegion.SecondaryData = null;
            this.cmbRegion.SelectedData = null;
            this.cmbRegion.SelectedDataID = null;
            this.cmbRegion.SelectionList = null;
            this.cmbRegion.SkipIDColumn = true;
            this.cmbRegion.RequestData += new System.EventHandler(this.cmbRegion_RequestData);
            this.cmbRegion.SelectedDataChanged += new System.EventHandler(this.cmbRegion_SelectedDataChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // StoreTransferSendingItemDialog
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.pnlInventory);
            this.Controls.Add(this.btnInventory);
            this.Controls.Add(this.lblInventory);
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
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.HasHelp = true;
            this.KeyPreview = true;
            this.Name = "StoreTransferSendingItemDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
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
            this.Controls.SetChildIndex(this.lblInventory, 0);
            this.Controls.SetChildIndex(this.btnInventory, 0);
            this.Controls.SetChildIndex(this.pnlInventory, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlInventory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkCreateAnother;
        private System.Windows.Forms.Label lblQuantitySending;
        private Controls.NumericTextBox ntbSendingQuantity;
        private System.Windows.Forms.Label lblUnit;
        private Controls.DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label lblVariant;
        private Controls.DualDataComboBox cmbVariant;
        private System.Windows.Forms.Label label3;
        private Controls.DualDataComboBox cmbItem;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label lblInventory;
        private System.Windows.Forms.Button btnInventory;
        private System.Windows.Forms.Panel pnlInventory;
        private System.Windows.Forms.Label lblRegion;
        private Controls.DualDataComboBox cmbRegion;
        private Controls.ListView lvItemInventory;
        private Controls.Columns.Column clmStore;
        private Controls.Columns.Column clmInventory;
        private Controls.Columns.Column clmOrdered;
        private Controls.Columns.Column clmReserved;
        private Controls.Columns.Column clmParked;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}