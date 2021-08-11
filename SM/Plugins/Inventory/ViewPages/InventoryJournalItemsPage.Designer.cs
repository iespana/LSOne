namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class InventoryJournalItemsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryJournalItemsPage));
            this.searchBar = new LSOne.Controls.SearchBar();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.lvItems = new LSOne.Controls.ListView();
            this.colStatus = new LSOne.Controls.Columns.Column();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colItem = new LSOne.Controls.Columns.Column();
            this.colVariant = new LSOne.Controls.Columns.Column();
            this.colQuantity = new LSOne.Controls.Columns.Column();
            this.colUnit = new LSOne.Controls.Columns.Column();
            this.colReason = new LSOne.Controls.Columns.Column();
            this.colPosted = new LSOne.Controls.Columns.Column();
            this.colStaff = new LSOne.Controls.Columns.Column();
            this.colArea = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.btnMoveToInventory = new System.Windows.Forms.Button();
            this.colBarcode = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // searchBar
            // 
            resources.ApplyResources(this.searchBar, "searchBar");
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.Name = "searchBar";
            this.searchBar.SearchOptionEnabled = true;
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            this.searchBar.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar_UnknownControlAdd);
            this.searchBar.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar_UnknownControlHasSelection);
            this.searchBar.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar_UnknownControlGetSelection);
            this.searchBar.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar_UnknownControlSetSelection);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.itemDataScroll_PageChanged);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.colStatus);
            this.lvItems.Columns.Add(this.colID);
            this.lvItems.Columns.Add(this.colItem);
            this.lvItems.Columns.Add(this.colVariant);
            this.lvItems.Columns.Add(this.colQuantity);
            this.lvItems.Columns.Add(this.colUnit);
            this.lvItems.Columns.Add(this.colBarcode);
            this.lvItems.Columns.Add(this.colReason);
            this.lvItems.Columns.Add(this.colPosted);
            this.lvItems.Columns.Add(this.colStaff);
            this.lvItems.Columns.Add(this.colArea);
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
            this.lvItems.SortSetting = "0:1";
            this.lvItems.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvItems_HeaderClicked);
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.RowExpanded += new LSOne.Controls.RowClickDelegate(this.lvItems_RowExpanded);
            this.lvItems.RowCollapsed += new LSOne.Controls.RowClickDelegate(this.lvItems_RowCollapsed);
            // 
            // colStatus
            // 
            this.colStatus.AutoSize = true;
            this.colStatus.DefaultStyle = null;
            resources.ApplyResources(this.colStatus, "colStatus");
            this.colStatus.MaximumWidth = ((short)(0));
            this.colStatus.MinimumWidth = ((short)(10));
            this.colStatus.SecondarySortColumn = ((short)(-1));
            this.colStatus.Tag = null;
            this.colStatus.Width = ((short)(50));
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(50));
            // 
            // colItem
            // 
            this.colItem.AutoSize = true;
            this.colItem.DefaultStyle = null;
            resources.ApplyResources(this.colItem, "colItem");
            this.colItem.MaximumWidth = ((short)(0));
            this.colItem.MinimumWidth = ((short)(10));
            this.colItem.SecondarySortColumn = ((short)(-1));
            this.colItem.Tag = null;
            this.colItem.Width = ((short)(50));
            // 
            // colVariant
            // 
            this.colVariant.AutoSize = true;
            this.colVariant.DefaultStyle = null;
            this.colVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colVariant, "colVariant");
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
            this.colQuantity.DefaultStyle = null;
            resources.ApplyResources(this.colQuantity, "colQuantity");
            this.colQuantity.MaximumWidth = ((short)(0));
            this.colQuantity.MinimumWidth = ((short)(10));
            this.colQuantity.SecondarySortColumn = ((short)(-1));
            this.colQuantity.Tag = null;
            this.colQuantity.Width = ((short)(50));
            // 
            // colUnit
            // 
            this.colUnit.AutoSize = true;
            this.colUnit.DefaultStyle = null;
            resources.ApplyResources(this.colUnit, "colUnit");
            this.colUnit.MaximumWidth = ((short)(0));
            this.colUnit.MinimumWidth = ((short)(10));
            this.colUnit.SecondarySortColumn = ((short)(-1));
            this.colUnit.Tag = null;
            this.colUnit.Width = ((short)(50));
            // 
            // colReason
            // 
            this.colReason.AutoSize = true;
            this.colReason.DefaultStyle = null;
            resources.ApplyResources(this.colReason, "colReason");
            this.colReason.MaximumWidth = ((short)(0));
            this.colReason.MinimumWidth = ((short)(10));
            this.colReason.SecondarySortColumn = ((short)(-1));
            this.colReason.Tag = null;
            this.colReason.Width = ((short)(50));
            // 
            // colPosted
            // 
            this.colPosted.AutoSize = true;
            this.colPosted.DefaultStyle = null;
            resources.ApplyResources(this.colPosted, "colPosted");
            this.colPosted.MaximumWidth = ((short)(0));
            this.colPosted.MinimumWidth = ((short)(10));
            this.colPosted.SecondarySortColumn = ((short)(-1));
            this.colPosted.Tag = null;
            this.colPosted.Width = ((short)(50));
            // 
            // colStaff
            // 
            this.colStaff.AutoSize = true;
            this.colStaff.DefaultStyle = null;
            resources.ApplyResources(this.colStaff, "colStaff");
            this.colStaff.MaximumWidth = ((short)(0));
            this.colStaff.MinimumWidth = ((short)(10));
            this.colStaff.SecondarySortColumn = ((short)(-1));
            this.colStaff.Tag = null;
            this.colStaff.Width = ((short)(50));
            // 
            // colArea
            // 
            this.colArea.AutoSize = true;
            this.colArea.DefaultStyle = null;
            resources.ApplyResources(this.colArea, "colArea");
            this.colArea.MaximumWidth = ((short)(0));
            this.colArea.MinimumWidth = ((short)(10));
            this.colArea.SecondarySortColumn = ((short)(-1));
            this.colArea.Tag = null;
            this.colArea.Width = ((short)(50));
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            // 
            // btnMoveToInventory
            // 
            resources.ApplyResources(this.btnMoveToInventory, "btnMoveToInventory");
            this.btnMoveToInventory.Name = "btnMoveToInventory";
            this.btnMoveToInventory.UseVisualStyleBackColor = true;
            this.btnMoveToInventory.Click += new System.EventHandler(this.btnMoveToInventory_Click);
            // 
            // colBarcode
            // 
            this.colBarcode.AutoSize = true;
            this.colBarcode.DefaultStyle = null;
            resources.ApplyResources(this.colBarcode, "colBarcode");
            this.colBarcode.MaximumWidth = ((short)(0));
            this.colBarcode.MinimumWidth = ((short)(10));
            this.colBarcode.SecondarySortColumn = ((short)(-1));
            this.colBarcode.Tag = null;
            this.colBarcode.Width = ((short)(50));
            // 
            // InventoryJournalItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnMoveToInventory);
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.itemDataScroll);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnsEditAddRemove);
            this.Name = "InventoryJournalItemsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.SearchBar searchBar;
        private Controls.DatabasePageDisplay itemDataScroll;
        private Controls.ListView lvItems;
        private Controls.ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.Button btnMoveToInventory;
        private Controls.Columns.Column colStatus;
        private Controls.Columns.Column colID;
        private Controls.Columns.Column colItem;
        private Controls.Columns.Column colVariant;
        private Controls.Columns.Column colQuantity;
        private Controls.Columns.Column colUnit;
        private Controls.Columns.Column colReason;
        private Controls.Columns.Column colPosted;
        private Controls.Columns.Column colStaff;
        private Controls.Columns.Column colArea;
        private Controls.Columns.Column colBarcode;
    }
}
