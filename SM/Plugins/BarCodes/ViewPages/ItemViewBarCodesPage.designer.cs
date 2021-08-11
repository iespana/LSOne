using LSOne.Controls;

namespace LSOne.ViewPlugins.BarCodes.ViewPages
{
    partial class ItemViewBarCodesPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemViewBarCodesPage));
            this.cmbBarCodeSetup = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEditBarCodeSetup = new LSOne.Controls.ContextButton();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvBarCodes = new LSOne.Controls.ListView();
            this.colBarcode = new LSOne.Controls.Columns.Column();
            this.colBarcodeSetup = new LSOne.Controls.Columns.Column();
            this.colItemID = new LSOne.Controls.Columns.Column();
            this.colItemDescription = new LSOne.Controls.Columns.Column();
            this.colVariantDescription = new LSOne.Controls.Columns.Column();
            this.colQty = new LSOne.Controls.Columns.Column();
            this.colUnit = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // cmbBarCodeSetup
            // 
            this.cmbBarCodeSetup.AddList = null;
            this.cmbBarCodeSetup.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbBarCodeSetup, "cmbBarCodeSetup");
            this.cmbBarCodeSetup.MaxLength = 32767;
            this.cmbBarCodeSetup.Name = "cmbBarCodeSetup";
            this.cmbBarCodeSetup.NoChangeAllowed = false;
            this.cmbBarCodeSetup.OnlyDisplayID = false;
            this.cmbBarCodeSetup.RemoveList = null;
            this.cmbBarCodeSetup.RowHeight = ((short)(22));
            this.cmbBarCodeSetup.SecondaryData = null;
            this.cmbBarCodeSetup.SelectedData = null;
            this.cmbBarCodeSetup.SelectedDataID = null;
            this.cmbBarCodeSetup.SelectionList = null;
            this.cmbBarCodeSetup.SkipIDColumn = true;
            this.cmbBarCodeSetup.RequestData += new System.EventHandler(this.cmbBarCodeSetup_RequestData);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnEditBarCodeSetup
            // 
            this.btnEditBarCodeSetup.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditBarCodeSetup, "btnEditBarCodeSetup");
            this.btnEditBarCodeSetup.Name = "btnEditBarCodeSetup";
            this.btnEditBarCodeSetup.Click += new System.EventHandler(this.btnEditBarCodeSetup_Click);
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvBarCodes
            // 
            resources.ApplyResources(this.lvBarCodes, "lvBarCodes");
            this.lvBarCodes.BuddyControl = null;
            this.lvBarCodes.Columns.Add(this.colBarcode);
            this.lvBarCodes.Columns.Add(this.colBarcodeSetup);
            this.lvBarCodes.Columns.Add(this.colItemID);
            this.lvBarCodes.Columns.Add(this.colItemDescription);
            this.lvBarCodes.Columns.Add(this.colVariantDescription);
            this.lvBarCodes.Columns.Add(this.colQty);
            this.lvBarCodes.Columns.Add(this.colUnit);
            this.lvBarCodes.ContentBackColor = System.Drawing.Color.White;
            this.lvBarCodes.DefaultRowHeight = ((short)(22));
            this.lvBarCodes.DimSelectionWhenDisabled = true;
            this.lvBarCodes.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvBarCodes.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvBarCodes.HeaderHeight = ((short)(25));
            this.lvBarCodes.Name = "lvBarCodes";
            this.lvBarCodes.OddRowColor = System.Drawing.Color.White;
            this.lvBarCodes.RowLineColor = System.Drawing.Color.LightGray;
            this.lvBarCodes.SecondarySortColumn = ((short)(-1));
            this.lvBarCodes.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvBarCodes.SortSetting = "0:1";
            this.lvBarCodes.SelectionChanged += new System.EventHandler(this.lvBarCodes_SelectionChanged);
            this.lvBarCodes.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvBarCodes_RowDoubleClick);
            // 
            // colBarcode
            // 
            this.colBarcode.AutoSize = true;
            this.colBarcode.DefaultStyle = null;
            resources.ApplyResources(this.colBarcode, "colBarcode");
            this.colBarcode.InternalSort = true;
            this.colBarcode.MaximumWidth = ((short)(0));
            this.colBarcode.MinimumWidth = ((short)(10));
            this.colBarcode.Tag = null;
            this.colBarcode.Width = ((short)(50));
            // 
            // colBarcodeSetup
            // 
            this.colBarcodeSetup.AutoSize = true;
            this.colBarcodeSetup.DefaultStyle = null;
            resources.ApplyResources(this.colBarcodeSetup, "colBarcodeSetup");
            this.colBarcodeSetup.InternalSort = true;
            this.colBarcodeSetup.MaximumWidth = ((short)(0));
            this.colBarcodeSetup.MinimumWidth = ((short)(10));
            this.colBarcodeSetup.Tag = null;
            this.colBarcodeSetup.Width = ((short)(50));
            // 
            // colItemID
            // 
            this.colItemID.AutoSize = true;
            this.colItemID.DefaultStyle = null;
            resources.ApplyResources(this.colItemID, "colItemID");
            this.colItemID.InternalSort = true;
            this.colItemID.MaximumWidth = ((short)(0));
            this.colItemID.MinimumWidth = ((short)(10));
            this.colItemID.Tag = null;
            this.colItemID.Width = ((short)(50));
            // 
            // colItemDescription
            // 
            this.colItemDescription.AutoSize = true;
            this.colItemDescription.DefaultStyle = null;
            resources.ApplyResources(this.colItemDescription, "colItemDescription");
            this.colItemDescription.InternalSort = true;
            this.colItemDescription.MaximumWidth = ((short)(0));
            this.colItemDescription.MinimumWidth = ((short)(10));
            this.colItemDescription.Tag = null;
            this.colItemDescription.Width = ((short)(50));
            // 
            // colVariantDescription
            // 
            this.colVariantDescription.AutoSize = true;
            this.colVariantDescription.DefaultStyle = null;
            resources.ApplyResources(this.colVariantDescription, "colVariantDescription");
            this.colVariantDescription.InternalSort = true;
            this.colVariantDescription.MaximumWidth = ((short)(0));
            this.colVariantDescription.MinimumWidth = ((short)(10));
            this.colVariantDescription.Tag = null;
            this.colVariantDescription.Width = ((short)(50));
            // 
            // colQty
            // 
            this.colQty.AutoSize = true;
            this.colQty.DefaultStyle = null;
            resources.ApplyResources(this.colQty, "colQty");
            this.colQty.InternalSort = true;
            this.colQty.MaximumWidth = ((short)(0));
            this.colQty.MinimumWidth = ((short)(10));
            this.colQty.Tag = null;
            this.colQty.Width = ((short)(50));
            // 
            // colUnit
            // 
            this.colUnit.AutoSize = true;
            this.colUnit.DefaultStyle = null;
            resources.ApplyResources(this.colUnit, "colUnit");
            this.colUnit.InternalSort = true;
            this.colUnit.MaximumWidth = ((short)(0));
            this.colUnit.MinimumWidth = ((short)(10));
            this.colUnit.Tag = null;
            this.colUnit.Width = ((short)(50));
            // 
            // ItemViewBarCodesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvBarCodes);
            this.Controls.Add(this.btnEditBarCodeSetup);
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.cmbBarCodeSetup);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.Name = "ItemViewBarCodesPage";
            this.ResumeLayout(false);

        }

        #endregion
        private DualDataComboBox cmbBarCodeSetup;
        private System.Windows.Forms.Label label3;
        private ContextButtons btnsContextButtons;
        private ContextButton btnEditBarCodeSetup;
        private ListView lvBarCodes;
        private Controls.Columns.Column colBarcode;
        private Controls.Columns.Column colBarcodeSetup;
        private Controls.Columns.Column colItemID;
        private Controls.Columns.Column colItemDescription;
        private Controls.Columns.Column colVariantDescription;
        private Controls.Columns.Column colQty;
        private Controls.Columns.Column colUnit;
    }
}
