namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class StoreTransferItemsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTransferItemsPage));
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.lvItems = new LSOne.Controls.ListView();
            this.clmID = new LSOne.Controls.Columns.Column();
            this.clmItemName = new LSOne.Controls.Columns.Column();
            this.clmVariant = new LSOne.Controls.Columns.Column();
            this.clmBarcode = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.SuspendLayout();
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.itemDataScroll_PageChanged);
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
            this.searchBar.SearchOptionChanged += new System.EventHandler(this.searchBar_SearchOptionChanged);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.clmID);
            this.lvItems.Columns.Add(this.clmItemName);
            this.lvItems.Columns.Add(this.clmVariant);
            this.lvItems.Columns.Add(this.clmBarcode);
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
            this.lvItems.CellAction += new LSOne.Controls.CellActionDelegate(this.lvItems_CellAction);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            // 
            // clmID
            // 
            this.clmID.AutoSize = true;
            this.clmID.DefaultStyle = null;
            resources.ApplyResources(this.clmID, "clmID");
            this.clmID.MaximumWidth = ((short)(0));
            this.clmID.MinimumWidth = ((short)(10));
            this.clmID.SecondarySortColumn = ((short)(-1));
            this.clmID.Tag = null;
            this.clmID.Width = ((short)(50));
            // 
            // clmItemName
            // 
            this.clmItemName.AutoSize = true;
            this.clmItemName.DefaultStyle = null;
            resources.ApplyResources(this.clmItemName, "clmItemName");
            this.clmItemName.MaximumWidth = ((short)(0));
            this.clmItemName.MinimumWidth = ((short)(10));
            this.clmItemName.SecondarySortColumn = ((short)(-1));
            this.clmItemName.Tag = null;
            this.clmItemName.Width = ((short)(50));
            // 
            // clmVariant
            // 
            this.clmVariant.AutoSize = true;
            this.clmVariant.DefaultStyle = null;
            resources.ApplyResources(this.clmVariant, "clmVariant");
            this.clmVariant.MaximumWidth = ((short)(0));
            this.clmVariant.MinimumWidth = ((short)(10));
            this.clmVariant.SecondarySortColumn = ((short)(-1));
            this.clmVariant.Tag = null;
            this.clmVariant.Width = ((short)(50));
            // 
            // clmBarcode
            // 
            this.clmBarcode.AutoSize = true;
            this.clmBarcode.DefaultStyle = null;
            resources.ApplyResources(this.clmBarcode, "clmBarcode");
            this.clmBarcode.MaximumWidth = ((short)(0));
            this.clmBarcode.MinimumWidth = ((short)(10));
            this.clmBarcode.SecondarySortColumn = ((short)(-1));
            this.clmBarcode.Tag = null;
            this.clmBarcode.Width = ((short)(50));
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
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // StoreTransferItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.itemDataScroll);
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnsEditAddRemove);
            this.Name = "StoreTransferItemsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DatabasePageDisplay itemDataScroll;
        private Controls.SearchBar searchBar;
        private Controls.ListView lvItems;
        private Controls.ContextButtons btnsEditAddRemove;
        private Controls.Columns.Column clmID;
        private Controls.Columns.Column clmItemName;
        private Controls.Columns.Column clmVariant;
        private Controls.Columns.Column clmBarcode;
    }
}
