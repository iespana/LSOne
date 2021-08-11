namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class ItemVendorMultiEditDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemVendorMultiEditDialog));
            this.lblEditPreview = new System.Windows.Forms.Label();
            this.lvlEditPreview = new LSOne.Controls.ListView();
            this.Description = new LSOne.Controls.Columns.Column();
            this.Change = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.cmbItems = new LSOne.Controls.DualDataComboBox();
            this.lblItems = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.colVariant = new LSOne.Controls.Columns.Column();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEditPreview
            // 
            resources.ApplyResources(this.lblEditPreview, "lblEditPreview");
            this.lblEditPreview.BackColor = System.Drawing.Color.Transparent;
            this.lblEditPreview.Name = "lblEditPreview";
            // 
            // lvlEditPreview
            // 
            this.lvlEditPreview.BuddyControl = null;
            this.lvlEditPreview.Columns.Add(this.Description);
            this.lvlEditPreview.Columns.Add(this.colVariant);
            this.lvlEditPreview.Columns.Add(this.Change);
            this.lvlEditPreview.Columns.Add(this.column1);
            this.lvlEditPreview.ContentBackColor = System.Drawing.Color.White;
            this.lvlEditPreview.DefaultRowHeight = ((short)(22));
            this.lvlEditPreview.DimSelectionWhenDisabled = true;
            this.lvlEditPreview.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvlEditPreview.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvlEditPreview.HeaderHeight = ((short)(25));
            resources.ApplyResources(this.lvlEditPreview, "lvlEditPreview");
            this.lvlEditPreview.Name = "lvlEditPreview";
            this.lvlEditPreview.OddRowColor = System.Drawing.Color.White;
            this.lvlEditPreview.RowLineColor = System.Drawing.Color.LightGray;
            this.lvlEditPreview.SecondarySortColumn = ((short)(-1));
            this.lvlEditPreview.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvlEditPreview.SortSetting = "0:1";
            this.lvlEditPreview.CellAction += new LSOne.Controls.CellActionDelegate(this.lvlEditPreview_CellAction);
            // 
            // Description
            // 
            this.Description.AutoSize = true;
            this.Description.Clickable = false;
            this.Description.DefaultStyle = null;
            resources.ApplyResources(this.Description, "Description");
            this.Description.MaximumWidth = ((short)(0));
            this.Description.MinimumWidth = ((short)(10));
            this.Description.RelativeSize = 0;
            this.Description.SecondarySortColumn = ((short)(0));
            this.Description.Tag = null;
            this.Description.Width = ((short)(50));
            // 
            // Change
            // 
            this.Change.AutoSize = true;
            this.Change.Clickable = false;
            this.Change.DefaultStyle = null;
            resources.ApplyResources(this.Change, "Change");
            this.Change.MaximumWidth = ((short)(0));
            this.Change.MinimumWidth = ((short)(10));
            this.Change.RelativeSize = 0;
            this.Change.SecondarySortColumn = ((short)(0));
            this.Change.Tag = null;
            this.Change.Width = ((short)(50));
            // 
            // column1
            // 
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.RelativeSize = 10;
            this.column1.SecondarySortColumn = ((short)(0));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // cmbItems
            // 
            this.cmbItems.AddList = null;
            this.cmbItems.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbItems, "cmbItems");
            this.cmbItems.MaxLength = 32767;
            this.cmbItems.Name = "cmbItems";
            this.cmbItems.NoChangeAllowed = false;
            this.cmbItems.OnlyDisplayID = false;
            this.cmbItems.RemoveList = null;
            this.cmbItems.RowHeight = ((short)(22));
            this.cmbItems.SecondaryData = null;
            this.cmbItems.SelectedData = null;
            this.cmbItems.SelectedDataID = null;
            this.cmbItems.SelectionList = null;
            this.cmbItems.SkipIDColumn = false;
            this.cmbItems.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbItems_DropDown);
            this.cmbItems.SelectedDataChanged += new System.EventHandler(this.cmbItems_SelectedDataChanged);
            // 
            // lblItems
            // 
            resources.ApplyResources(this.lblItems, "lblItems");
            this.lblItems.Name = "lblItems";
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
            // colVariant
            // 
            this.colVariant.AutoSize = true;
            this.colVariant.DefaultStyle = null;
            this.colVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colVariant, "colVariant");
            this.colVariant.InternalSort = true;
            this.colVariant.MaximumWidth = ((short)(0));
            this.colVariant.MinimumWidth = ((short)(10));
            this.colVariant.NoTextWhenSmall = true;
            this.colVariant.SecondarySortColumn = ((short)(0));
            this.colVariant.Tag = null;
            this.colVariant.Width = ((short)(50));
            // 
            // ItemVendorMultiEditDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblEditPreview);
            this.Controls.Add(this.lvlEditPreview);
            this.Controls.Add(this.cmbItems);
            this.Controls.Add(this.lblItems);
            this.HasHelp = true;
            this.Name = "ItemVendorMultiEditDialog";
            this.Load += new System.EventHandler(this.VendorItemMultiEditDialog_Load);
            this.Controls.SetChildIndex(this.lblItems, 0);
            this.Controls.SetChildIndex(this.cmbItems, 0);
            this.Controls.SetChildIndex(this.lvlEditPreview, 0);
            this.Controls.SetChildIndex(this.lblEditPreview, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEditPreview;
        private Controls.ListView lvlEditPreview;
        private Controls.DualDataComboBox cmbItems;
        private System.Windows.Forms.Label lblItems;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.Columns.Column Description;
        private Controls.Columns.Column Change;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column colVariant;
    }
}