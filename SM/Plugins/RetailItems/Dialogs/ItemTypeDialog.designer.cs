namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    partial class ItemTypeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemTypeDialog));
            this.lblItemType = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblLedgerLines = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grbIoH = new System.Windows.Forms.GroupBox();
            this.lvItemInventory = new LSOne.Controls.ListView();
            this.colStore = new LSOne.Controls.Columns.Column();
            this.colIoH = new LSOne.Controls.Columns.Column();
            this.colOrdered = new LSOne.Controls.Columns.Column();
            this.colReserved = new LSOne.Controls.Columns.Column();
            this.grpNotification = new LSOne.Controls.GroupPanel();
            this.lblNotification = new System.Windows.Forms.Label();
            this.cmbItemType = new System.Windows.Forms.ComboBox();
            this.ntbLedgerLines = new LSOne.Controls.NumericTextBox();
            this.colParked = new LSOne.Controls.Columns.Column();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel3.SuspendLayout();
            this.grbIoH.SuspendLayout();
            this.grpNotification.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblItemType
            // 
            resources.ApplyResources(this.lblItemType, "lblItemType");
            this.lblItemType.Name = "lblItemType";
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
            // lblLedgerLines
            // 
            resources.ApplyResources(this.lblLedgerLines, "lblLedgerLines");
            this.lblLedgerLines.Name = "lblLedgerLines";
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.grbIoH);
            this.panel3.Controls.Add(this.grpNotification);
            this.panel3.Name = "panel3";
            // 
            // grbIoH
            // 
            resources.ApplyResources(this.grbIoH, "grbIoH");
            this.grbIoH.Controls.Add(this.lvItemInventory);
            this.grbIoH.Name = "grbIoH";
            this.grbIoH.TabStop = false;
            // 
            // lvItemInventory
            // 
            resources.ApplyResources(this.lvItemInventory, "lvItemInventory");
            this.lvItemInventory.BuddyControl = null;
            this.lvItemInventory.Columns.Add(this.colStore);
            this.lvItemInventory.Columns.Add(this.colIoH);
            this.lvItemInventory.Columns.Add(this.colOrdered);
            this.lvItemInventory.Columns.Add(this.colReserved);
            this.lvItemInventory.Columns.Add(this.colParked);
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
            // colStore
            // 
            this.colStore.AutoSize = true;
            this.colStore.DefaultStyle = null;
            resources.ApplyResources(this.colStore, "colStore");
            this.colStore.MaximumWidth = ((short)(0));
            this.colStore.MinimumWidth = ((short)(10));
            this.colStore.SecondarySortColumn = ((short)(-1));
            this.colStore.Tag = null;
            this.colStore.Width = ((short)(50));
            // 
            // colIoH
            // 
            this.colIoH.AutoSize = true;
            this.colIoH.DefaultStyle = null;
            resources.ApplyResources(this.colIoH, "colIoH");
            this.colIoH.MaximumWidth = ((short)(0));
            this.colIoH.MinimumWidth = ((short)(10));
            this.colIoH.SecondarySortColumn = ((short)(-1));
            this.colIoH.Tag = null;
            this.colIoH.Width = ((short)(50));
            // 
            // colOrdered
            // 
            this.colOrdered.AutoSize = true;
            this.colOrdered.DefaultStyle = null;
            resources.ApplyResources(this.colOrdered, "colOrdered");
            this.colOrdered.MaximumWidth = ((short)(0));
            this.colOrdered.MinimumWidth = ((short)(10));
            this.colOrdered.SecondarySortColumn = ((short)(-1));
            this.colOrdered.Tag = null;
            this.colOrdered.Width = ((short)(50));
            // 
            // colReserved
            // 
            this.colReserved.AutoSize = true;
            this.colReserved.DefaultStyle = null;
            resources.ApplyResources(this.colReserved, "colReserved");
            this.colReserved.MaximumWidth = ((short)(0));
            this.colReserved.MinimumWidth = ((short)(10));
            this.colReserved.SecondarySortColumn = ((short)(-1));
            this.colReserved.Tag = null;
            this.colReserved.Width = ((short)(50));
            // 
            // grpNotification
            // 
            resources.ApplyResources(this.grpNotification, "grpNotification");
            this.grpNotification.Controls.Add(this.lblNotification);
            this.grpNotification.Name = "grpNotification";
            // 
            // lblNotification
            // 
            resources.ApplyResources(this.lblNotification, "lblNotification");
            this.lblNotification.BackColor = System.Drawing.Color.Transparent;
            this.lblNotification.Name = "lblNotification";
            // 
            // cmbItemType
            // 
            this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbItemType, "cmbItemType");
            this.cmbItemType.Name = "cmbItemType";
            // 
            // ntbLedgerLines
            // 
            this.ntbLedgerLines.AllowDecimal = false;
            this.ntbLedgerLines.AllowNegative = false;
            this.ntbLedgerLines.CultureInfo = null;
            this.ntbLedgerLines.DecimalLetters = 2;
            this.ntbLedgerLines.ForeColor = System.Drawing.Color.Black;
            this.ntbLedgerLines.HasMinValue = false;
            resources.ApplyResources(this.ntbLedgerLines, "ntbLedgerLines");
            this.ntbLedgerLines.MaxValue = 0D;
            this.ntbLedgerLines.MinValue = 0D;
            this.ntbLedgerLines.Name = "ntbLedgerLines";
            this.ntbLedgerLines.ReadOnly = true;
            this.ntbLedgerLines.Value = 0D;
            // 
            // colParked
            // 
            this.colParked.AutoSize = true;
            this.colParked.DefaultStyle = null;
            resources.ApplyResources(this.colParked, "colParked");
            this.colParked.MaximumWidth = ((short)(0));
            this.colParked.MinimumWidth = ((short)(10));
            this.colParked.SecondarySortColumn = ((short)(-1));
            this.colParked.Tag = null;
            this.colParked.Width = ((short)(50));
            // 
            // ItemTypeDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ntbLedgerLines);
            this.Controls.Add(this.cmbItemType);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblLedgerLines);
            this.Controls.Add(this.lblItemType);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "ItemTypeDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblItemType, 0);
            this.Controls.SetChildIndex(this.lblLedgerLines, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.cmbItemType, 0);
            this.Controls.SetChildIndex(this.ntbLedgerLines, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.grbIoH.ResumeLayout(false);
            this.grpNotification.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblItemType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblLedgerLines;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox grbIoH;
        private Controls.ListView lvItemInventory;
        private Controls.Columns.Column colStore;
        private Controls.Columns.Column colIoH;
        private Controls.Columns.Column colOrdered;
        private Controls.Columns.Column colReserved;
        private Controls.GroupPanel grpNotification;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.ComboBox cmbItemType;
        private Controls.NumericTextBox ntbLedgerLines;
        private Controls.Columns.Column colParked;
    }
}