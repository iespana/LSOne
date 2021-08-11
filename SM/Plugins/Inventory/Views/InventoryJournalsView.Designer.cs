namespace LSOne.ViewPlugins.Inventory.Views
{
    partial class InventoryJournalsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryJournalsView));
            this.searchBar = new LSOne.Controls.SearchBar();
            this.lvJournals = new LSOne.Controls.ListView();
            this.colState = new LSOne.Controls.Columns.Column();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colCreated = new LSOne.Controls.Columns.Column();
            this.colStore = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.btnMoveToInventory = new System.Windows.Forms.Button();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.itemDataScroll);
            this.pnlBottom.Controls.Add(this.btnMoveToInventory);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.lvJournals);
            this.pnlBottom.Controls.Add(this.searchBar);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
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
            this.searchBar.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar_UnknownControlRemove);
            this.searchBar.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar_UnknownControlHasSelection);
            this.searchBar.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar_UnknownControlGetSelection);
            this.searchBar.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar_UnknownControlSetSelection);
            // 
            // lvJournals
            // 
            resources.ApplyResources(this.lvJournals, "lvJournals");
            this.lvJournals.BuddyControl = null;
            this.lvJournals.Columns.Add(this.colState);
            this.lvJournals.Columns.Add(this.colID);
            this.lvJournals.Columns.Add(this.colDescription);
            this.lvJournals.Columns.Add(this.colCreated);
            this.lvJournals.Columns.Add(this.colStore);
            this.lvJournals.ContentBackColor = System.Drawing.Color.White;
            this.lvJournals.DefaultRowHeight = ((short)(22));
            this.lvJournals.DimSelectionWhenDisabled = true;
            this.lvJournals.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvJournals.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvJournals.HeaderHeight = ((short)(25));
            this.lvJournals.HorizontalScrollbar = true;
            this.lvJournals.Name = "lvJournals";
            this.lvJournals.OddRowColor = System.Drawing.Color.White;
            this.lvJournals.RowLineColor = System.Drawing.Color.LightGray;
            this.lvJournals.SecondarySortColumn = ((short)(-1));
            this.lvJournals.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvJournals.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvJournals.SortSetting = "0:1";
            this.lvJournals.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvJournals_HeaderClicked);
            this.lvJournals.SelectionChanged += new System.EventHandler(this.lvJournals_SelectionChanged);
            this.lvJournals.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvJournals_RowDoubleClick);
            // 
            // colState
            // 
            this.colState.AutoSize = true;
            this.colState.Clickable = false;
            this.colState.DefaultStyle = null;
            resources.ApplyResources(this.colState, "colState");
            this.colState.MaximumWidth = ((short)(0));
            this.colState.MinimumWidth = ((short)(10));
            this.colState.SecondarySortColumn = ((short)(-1));
            this.colState.Tag = null;
            this.colState.Width = ((short)(50));
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
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colCreated
            // 
            this.colCreated.AutoSize = true;
            this.colCreated.DefaultStyle = null;
            resources.ApplyResources(this.colCreated, "colCreated");
            this.colCreated.MaximumWidth = ((short)(0));
            this.colCreated.MinimumWidth = ((short)(10));
            this.colCreated.SecondarySortColumn = ((short)(-1));
            this.colCreated.Tag = null;
            this.colCreated.Width = ((short)(50));
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
            // btnMoveToInventory
            // 
            resources.ApplyResources(this.btnMoveToInventory, "btnMoveToInventory");
            this.btnMoveToInventory.Name = "btnMoveToInventory";
            this.btnMoveToInventory.UseVisualStyleBackColor = true;
            this.btnMoveToInventory.Click += new System.EventHandler(this.btnMoveToInventory_Click);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.itemDataScroll_PageChanged);
            // 
            // InventoryJournalsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "InventoryJournalsView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SearchBar searchBar;
        private Controls.ListView lvJournals;
        private Controls.ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.Button btnMoveToInventory;
        private Controls.Columns.Column colState;
        private Controls.Columns.Column colID;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colCreated;
        private Controls.Columns.Column colStore;
        private Controls.DatabasePageDisplay itemDataScroll;
    }
}
