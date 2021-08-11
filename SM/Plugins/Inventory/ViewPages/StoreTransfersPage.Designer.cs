namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class StoreTransfersPage
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
                searchTimer.Tick -= SearchTimerOnTick;
                searchTimer.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTransfersPage));
            this.lvItems = new LSOne.Controls.ListView();
            this.clmDot = new LSOne.Controls.Columns.Column();
            this.clmID = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmSendingStore = new LSOne.Controls.Columns.Column();
            this.clmReceivingStore = new LSOne.Controls.Columns.Column();
            this.clmItems = new LSOne.Controls.Columns.Column();
            this.clmStatus = new LSOne.Controls.Columns.Column();
            this.clmDateOne = new LSOne.Controls.Columns.Column();
            this.clmDateTwo = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.contextButtons = new LSOne.Controls.ContextButtons();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.SuspendLayout();
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BackColor = System.Drawing.Color.White;
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.clmDot);
            this.lvItems.Columns.Add(this.clmID);
            this.lvItems.Columns.Add(this.clmDescription);
            this.lvItems.Columns.Add(this.clmSendingStore);
            this.lvItems.Columns.Add(this.clmReceivingStore);
            this.lvItems.Columns.Add(this.clmItems);
            this.lvItems.Columns.Add(this.clmStatus);
            this.lvItems.Columns.Add(this.clmDateOne);
            this.lvItems.Columns.Add(this.clmDateTwo);
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
            this.lvItems.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvItems_HeaderClicked);
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            // 
            // clmDot
            // 
            this.clmDot.AutoSize = true;
            this.clmDot.Clickable = false;
            this.clmDot.DefaultStyle = null;
            resources.ApplyResources(this.clmDot, "clmDot");
            this.clmDot.MaximumWidth = ((short)(25));
            this.clmDot.MinimumWidth = ((short)(25));
            this.clmDot.SecondarySortColumn = ((short)(-1));
            this.clmDot.Tag = null;
            this.clmDot.Width = ((short)(25));
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
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(50));
            // 
            // clmSendingStore
            // 
            this.clmSendingStore.AutoSize = true;
            this.clmSendingStore.DefaultStyle = null;
            resources.ApplyResources(this.clmSendingStore, "clmSendingStore");
            this.clmSendingStore.MaximumWidth = ((short)(0));
            this.clmSendingStore.MinimumWidth = ((short)(10));
            this.clmSendingStore.SecondarySortColumn = ((short)(-1));
            this.clmSendingStore.Tag = null;
            this.clmSendingStore.Width = ((short)(50));
            // 
            // clmReceivingStore
            // 
            this.clmReceivingStore.AutoSize = true;
            this.clmReceivingStore.DefaultStyle = null;
            resources.ApplyResources(this.clmReceivingStore, "clmReceivingStore");
            this.clmReceivingStore.MaximumWidth = ((short)(0));
            this.clmReceivingStore.MinimumWidth = ((short)(10));
            this.clmReceivingStore.SecondarySortColumn = ((short)(-1));
            this.clmReceivingStore.Tag = null;
            this.clmReceivingStore.Width = ((short)(50));
            // 
            // clmItems
            // 
            this.clmItems.AutoSize = true;
            this.clmItems.Clickable = false;
            this.clmItems.DefaultStyle = null;
            resources.ApplyResources(this.clmItems, "clmItems");
            this.clmItems.MaximumWidth = ((short)(0));
            this.clmItems.MinimumWidth = ((short)(10));
            this.clmItems.SecondarySortColumn = ((short)(-1));
            this.clmItems.Tag = null;
            this.clmItems.Width = ((short)(50));
            // 
            // clmStatus
            // 
            this.clmStatus.AutoSize = true;
            this.clmStatus.Clickable = false;
            this.clmStatus.DefaultStyle = null;
            resources.ApplyResources(this.clmStatus, "clmStatus");
            this.clmStatus.MaximumWidth = ((short)(0));
            this.clmStatus.MinimumWidth = ((short)(10));
            this.clmStatus.SecondarySortColumn = ((short)(-1));
            this.clmStatus.Tag = null;
            this.clmStatus.Width = ((short)(50));
            // 
            // clmDateOne
            // 
            this.clmDateOne.AutoSize = true;
            this.clmDateOne.DefaultStyle = null;
            resources.ApplyResources(this.clmDateOne, "clmDateOne");
            this.clmDateOne.MaximumWidth = ((short)(0));
            this.clmDateOne.MinimumWidth = ((short)(10));
            this.clmDateOne.SecondarySortColumn = ((short)(-1));
            this.clmDateOne.Tag = null;
            this.clmDateOne.Width = ((short)(50));
            // 
            // clmDateTwo
            // 
            this.clmDateTwo.AutoSize = true;
            this.clmDateTwo.DefaultStyle = null;
            resources.ApplyResources(this.clmDateTwo, "clmDateTwo");
            this.clmDateTwo.MaximumWidth = ((short)(0));
            this.clmDateTwo.MinimumWidth = ((short)(10));
            this.clmDateTwo.SecondarySortColumn = ((short)(-1));
            this.clmDateTwo.Tag = null;
            this.clmDateTwo.Width = ((short)(50));
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
            // contextButtons
            // 
            this.contextButtons.AddButtonEnabled = false;
            resources.ApplyResources(this.contextButtons, "contextButtons");
            this.contextButtons.BackColor = System.Drawing.Color.Transparent;
            this.contextButtons.Context = LSOne.Controls.ButtonTypes.EditRemove;
            this.contextButtons.EditButtonEnabled = true;
            this.contextButtons.Name = "contextButtons";
            this.contextButtons.RemoveButtonEnabled = true;
            this.contextButtons.EditButtonClicked += new System.EventHandler(this.contextButtons_EditButtonClicked);
            this.contextButtons.AddButtonClicked += new System.EventHandler(this.contextButtons_AddButtonClicked);
            this.contextButtons.RemoveButtonClicked += new System.EventHandler(this.contextButtons_RemoveButtonClicked);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.itemDataScroll_PageChanged);
            // 
            // StoreTransfersPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.itemDataScroll);
            this.Controls.Add(this.contextButtons);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.searchBar);
            this.Name = "StoreTransfersPage";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvItems;
        private Controls.SearchBar searchBar;
        private Controls.Columns.Column clmID;
        private Controls.Columns.Column clmDescription;
        private Controls.Columns.Column clmReceivingStore;
        private Controls.Columns.Column clmSendingStore;
        private Controls.Columns.Column clmItems;
        private Controls.Columns.Column clmStatus;
        private Controls.Columns.Column clmDateOne;
        private Controls.Columns.Column clmDateTwo;
        private Controls.ContextButtons contextButtons;
        private Controls.Columns.Column clmDot;
        private Controls.DatabasePageDisplay itemDataScroll;
    }
}
