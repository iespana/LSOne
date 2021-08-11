using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class PurchaseOrderItemsPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseOrderItemsPage));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lvItems = new LSOne.Controls.ListView();
            this.colItemId = new LSOne.Controls.Columns.Column();
            this.colItemName = new LSOne.Controls.Columns.Column();
            this.colVariant = new LSOne.Controls.Columns.Column();
            this.colQuantity = new LSOne.Controls.Columns.Column();
            this.colUnitPriceExclTax = new LSOne.Controls.Columns.Column();
            this.colTaxAmount = new LSOne.Controls.Columns.Column();
            this.colCalculatedDiscount = new LSOne.Controls.Columns.Column();
            this.colFinalUnitPrice = new LSOne.Controls.Columns.Column();
            this.colDiscountAmt = new LSOne.Controls.Columns.Column();
            this.colDiscountPercentage = new LSOne.Controls.Columns.Column();
            this.colTaxMethod = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.colPicture = new LSOne.Controls.Columns.Column();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.colItemId);
            this.lvItems.Columns.Add(this.colItemName);
            this.lvItems.Columns.Add(this.colVariant);
            this.lvItems.Columns.Add(this.colQuantity);
            this.lvItems.Columns.Add(this.colUnitPriceExclTax);
            this.lvItems.Columns.Add(this.colTaxAmount);
            this.lvItems.Columns.Add(this.colCalculatedDiscount);
            this.lvItems.Columns.Add(this.colFinalUnitPrice);
            this.lvItems.Columns.Add(this.colDiscountAmt);
            this.lvItems.Columns.Add(this.colDiscountPercentage);
            this.lvItems.Columns.Add(this.colTaxMethod);
            this.lvItems.Columns.Add(this.colPicture);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.HorizontalScrollbar = true;
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "1:1";
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.CellAction += new LSOne.Controls.CellActionDelegate(this.LvItems_CellAction);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            // 
            // colItemId
            // 
            this.colItemId.AutoSize = true;
            this.colItemId.DefaultStyle = null;
            resources.ApplyResources(this.colItemId, "colItemId");
            this.colItemId.InternalSort = true;
            this.colItemId.MaximumWidth = ((short)(0));
            this.colItemId.MinimumWidth = ((short)(10));
            this.colItemId.SecondarySortColumn = ((short)(-1));
            this.colItemId.Tag = null;
            this.colItemId.Width = ((short)(50));
            // 
            // colItemName
            // 
            this.colItemName.AutoSize = true;
            this.colItemName.DefaultStyle = null;
            resources.ApplyResources(this.colItemName, "colItemName");
            this.colItemName.InternalSort = true;
            this.colItemName.MaximumWidth = ((short)(0));
            this.colItemName.MinimumWidth = ((short)(10));
            this.colItemName.SecondarySortColumn = ((short)(-1));
            this.colItemName.Tag = null;
            this.colItemName.Width = ((short)(50));
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
            this.colVariant.SecondarySortColumn = ((short)(-1));
            this.colVariant.Tag = null;
            this.colVariant.Width = ((short)(50));
            // 
            // colQuantity
            // 
            this.colQuantity.AutoSize = true;
            this.colQuantity.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colQuantity.DefaultStyle = null;
            resources.ApplyResources(this.colQuantity, "colQuantity");
            this.colQuantity.InternalSort = true;
            this.colQuantity.MaximumWidth = ((short)(0));
            this.colQuantity.MinimumWidth = ((short)(10));
            this.colQuantity.SecondarySortColumn = ((short)(-1));
            this.colQuantity.Tag = null;
            this.colQuantity.Width = ((short)(50));
            // 
            // colUnitPriceExclTax
            // 
            this.colUnitPriceExclTax.AutoSize = true;
            this.colUnitPriceExclTax.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colUnitPriceExclTax.DefaultStyle = null;
            resources.ApplyResources(this.colUnitPriceExclTax, "colUnitPriceExclTax");
            this.colUnitPriceExclTax.InternalSort = true;
            this.colUnitPriceExclTax.MaximumWidth = ((short)(0));
            this.colUnitPriceExclTax.MinimumWidth = ((short)(10));
            this.colUnitPriceExclTax.SecondarySortColumn = ((short)(-1));
            this.colUnitPriceExclTax.Tag = null;
            this.colUnitPriceExclTax.Width = ((short)(50));
            // 
            // colTaxAmount
            // 
            this.colTaxAmount.AutoSize = true;
            this.colTaxAmount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colTaxAmount.DefaultStyle = null;
            resources.ApplyResources(this.colTaxAmount, "colTaxAmount");
            this.colTaxAmount.InternalSort = true;
            this.colTaxAmount.MaximumWidth = ((short)(0));
            this.colTaxAmount.MinimumWidth = ((short)(10));
            this.colTaxAmount.SecondarySortColumn = ((short)(-1));
            this.colTaxAmount.Tag = null;
            this.colTaxAmount.Width = ((short)(50));
            // 
            // colCalculatedDiscount
            // 
            this.colCalculatedDiscount.AutoSize = true;
            this.colCalculatedDiscount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colCalculatedDiscount.DefaultStyle = null;
            resources.ApplyResources(this.colCalculatedDiscount, "colCalculatedDiscount");
            this.colCalculatedDiscount.InternalSort = true;
            this.colCalculatedDiscount.MaximumWidth = ((short)(0));
            this.colCalculatedDiscount.MinimumWidth = ((short)(10));
            this.colCalculatedDiscount.SecondarySortColumn = ((short)(-1));
            this.colCalculatedDiscount.Tag = null;
            this.colCalculatedDiscount.Width = ((short)(50));
            // 
            // colFinalUnitPrice
            // 
            this.colFinalUnitPrice.AutoSize = true;
            this.colFinalUnitPrice.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colFinalUnitPrice.DefaultStyle = null;
            resources.ApplyResources(this.colFinalUnitPrice, "colFinalUnitPrice");
            this.colFinalUnitPrice.InternalSort = true;
            this.colFinalUnitPrice.MaximumWidth = ((short)(0));
            this.colFinalUnitPrice.MinimumWidth = ((short)(10));
            this.colFinalUnitPrice.SecondarySortColumn = ((short)(-1));
            this.colFinalUnitPrice.Tag = null;
            this.colFinalUnitPrice.Width = ((short)(50));
            // 
            // colDiscountAmt
            // 
            this.colDiscountAmt.AutoSize = true;
            this.colDiscountAmt.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colDiscountAmt.DefaultStyle = null;
            resources.ApplyResources(this.colDiscountAmt, "colDiscountAmt");
            this.colDiscountAmt.InternalSort = true;
            this.colDiscountAmt.MaximumWidth = ((short)(0));
            this.colDiscountAmt.MinimumWidth = ((short)(10));
            this.colDiscountAmt.SecondarySortColumn = ((short)(-1));
            this.colDiscountAmt.Tag = null;
            this.colDiscountAmt.Width = ((short)(50));
            // 
            // colDiscountPercentage
            // 
            this.colDiscountPercentage.AutoSize = true;
            this.colDiscountPercentage.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colDiscountPercentage.DefaultStyle = null;
            resources.ApplyResources(this.colDiscountPercentage, "colDiscountPercentage");
            this.colDiscountPercentage.InternalSort = true;
            this.colDiscountPercentage.MaximumWidth = ((short)(0));
            this.colDiscountPercentage.MinimumWidth = ((short)(10));
            this.colDiscountPercentage.SecondarySortColumn = ((short)(-1));
            this.colDiscountPercentage.Tag = null;
            this.colDiscountPercentage.Width = ((short)(50));
            // 
            // colTaxMethod
            // 
            this.colTaxMethod.AutoSize = true;
            this.colTaxMethod.DefaultStyle = null;
            resources.ApplyResources(this.colTaxMethod, "colTaxMethod");
            this.colTaxMethod.InternalSort = true;
            this.colTaxMethod.MaximumWidth = ((short)(0));
            this.colTaxMethod.MinimumWidth = ((short)(10));
            this.colTaxMethod.SecondarySortColumn = ((short)(-1));
            this.colTaxMethod.Tag = null;
            this.colTaxMethod.Width = ((short)(50));
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = true;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = true;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SearchOptionEnabled = true;
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.SaveAsDefault += new System.EventHandler(this.searchBar1_SaveAsDefault);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.itemDataScroll_PageChanged);
            // 
            // colPicture
            // 
            this.colPicture.AutoSize = true;
            this.colPicture.Clickable = false;
            this.colPicture.DefaultStyle = null;
            resources.ApplyResources(this.colPicture, "colPicture");
            this.colPicture.MaximumWidth = ((short)(0));
            this.colPicture.MinimumWidth = ((short)(10));
            this.colPicture.SecondarySortColumn = ((short)(-1));
            this.colPicture.Tag = null;
            this.colPicture.Width = ((short)(50));
            // 
            // PurchaseOrderItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.itemDataScroll);
            this.Controls.Add(this.searchBar1);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnsEditAddRemove);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "PurchaseOrderItemsPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ListView lvItems;
        private Controls.Columns.Column colItemName;
        private Controls.Columns.Column colVariant;
        private Controls.Columns.Column colQuantity;
        private Controls.Columns.Column colUnitPriceExclTax;
        private Controls.Columns.Column colDiscountAmt;
        private Controls.Columns.Column colDiscountPercentage;
        private Controls.Columns.Column colTaxAmount;
        private Controls.Columns.Column colFinalUnitPrice;
        private SearchBar searchBar1;
        private Controls.Columns.Column colItemId;
        private DatabasePageDisplay itemDataScroll;
        private Controls.Columns.Column colTaxMethod;
        private Controls.Columns.Column colCalculatedDiscount;
        private Controls.Columns.Column colPicture;
    }
}
