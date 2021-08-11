namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class ExistingStoreTransferSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExistingStoreTransferSearch));
            this.lvItems = new LSOne.Controls.ListView();
            this.clmID = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.clmSendingStore = new LSOne.Controls.Columns.Column();
            this.clmReceivingStore = new LSOne.Controls.Columns.Column();
            this.clmSent = new LSOne.Controls.Columns.Column();
            this.clmReceived = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.SuspendLayout();
            // 
            // lvItems
            // 
            this.lvItems.BackColor = System.Drawing.Color.White;
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.clmID);
            this.lvItems.Columns.Add(this.column1);
            this.lvItems.Columns.Add(this.clmSendingStore);
            this.lvItems.Columns.Add(this.clmReceivingStore);
            this.lvItems.Columns.Add(this.clmSent);
            this.lvItems.Columns.Add(this.clmReceived);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.HorizontalScrollbar = true;
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "0:1";
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            // 
            // clmID
            // 
            this.clmID.AutoSize = true;
            this.clmID.DefaultStyle = null;
            resources.ApplyResources(this.clmID, "clmID");
            this.clmID.InternalSort = true;
            this.clmID.MaximumWidth = ((short)(0));
            this.clmID.MinimumWidth = ((short)(10));
            this.clmID.SecondarySortColumn = ((short)(-1));
            this.clmID.Tag = null;
            this.clmID.Width = ((short)(50));
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // clmSendingStore
            // 
            this.clmSendingStore.AutoSize = true;
            this.clmSendingStore.DefaultStyle = null;
            resources.ApplyResources(this.clmSendingStore, "clmSendingStore");
            this.clmSendingStore.InternalSort = true;
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
            this.clmReceivingStore.InternalSort = true;
            this.clmReceivingStore.MaximumWidth = ((short)(0));
            this.clmReceivingStore.MinimumWidth = ((short)(10));
            this.clmReceivingStore.SecondarySortColumn = ((short)(-1));
            this.clmReceivingStore.Tag = null;
            this.clmReceivingStore.Width = ((short)(50));
            // 
            // clmSent
            // 
            this.clmSent.AutoSize = true;
            this.clmSent.DefaultStyle = null;
            resources.ApplyResources(this.clmSent, "clmSent");
            this.clmSent.InternalSort = true;
            this.clmSent.MaximumWidth = ((short)(0));
            this.clmSent.MinimumWidth = ((short)(10));
            this.clmSent.SecondarySortColumn = ((short)(-1));
            this.clmSent.Tag = null;
            this.clmSent.Width = ((short)(50));
            // 
            // clmReceived
            // 
            this.clmReceived.AutoSize = true;
            this.clmReceived.DefaultStyle = null;
            resources.ApplyResources(this.clmReceived, "clmReceived");
            this.clmReceived.InternalSort = true;
            this.clmReceived.MaximumWidth = ((short)(0));
            this.clmReceived.MinimumWidth = ((short)(10));
            this.clmReceived.SecondarySortColumn = ((short)(-1));
            this.clmReceived.Tag = null;
            this.clmReceived.Width = ((short)(50));
            // 
            // searchBar
            // 
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            resources.ApplyResources(this.searchBar, "searchBar");
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
            // ExistingStoreTransferSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.searchBar);
            this.Name = "ExistingStoreTransferSearch";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SearchBar searchBar;
        private Controls.ListView lvItems;
        private Controls.Columns.Column clmID;
        private Controls.Columns.Column clmSendingStore;
        private Controls.Columns.Column clmReceivingStore;
        private Controls.Columns.Column clmSent;
        private Controls.Columns.Column clmReceived;
        private Controls.Columns.Column column1;
    }
}
