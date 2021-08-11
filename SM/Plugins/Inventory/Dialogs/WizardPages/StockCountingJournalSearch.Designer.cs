namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class StockCountingJournalSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockCountingJournalSearch));
            this.lvStockCount = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.SuspendLayout();
            // 
            // lvStockCount
            // 
            resources.ApplyResources(this.lvStockCount, "lvStockCount");
            this.lvStockCount.BuddyControl = null;
            this.lvStockCount.Columns.Add(this.column1);
            this.lvStockCount.Columns.Add(this.column2);
            this.lvStockCount.Columns.Add(this.column3);
            this.lvStockCount.Columns.Add(this.column4);
            this.lvStockCount.Columns.Add(this.column5);
            this.lvStockCount.Columns.Add(this.column6);
            this.lvStockCount.ContentBackColor = System.Drawing.Color.White;
            this.lvStockCount.DefaultRowHeight = ((short)(22));
            this.lvStockCount.DimSelectionWhenDisabled = true;
            this.lvStockCount.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvStockCount.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvStockCount.HeaderHeight = ((short)(25));
            this.lvStockCount.Name = "lvStockCount";
            this.lvStockCount.OddRowColor = System.Drawing.Color.White;
            this.lvStockCount.RowLineColor = System.Drawing.Color.LightGray;
            this.lvStockCount.SecondarySortColumn = ((short)(-1));
            this.lvStockCount.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvStockCount.SortSetting = "0:1";
            this.lvStockCount.SelectionChanged += new System.EventHandler(this.lvStockCount_SelectionChanged);
            this.lvStockCount.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvStockCount_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(0));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.InternalSort = true;
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
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
            // StockCountingJournalSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.lvStockCount);
            this.Name = "StockCountingJournalSearch";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvStockCount;
        private Controls.SearchBar searchBar;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column6;
    }
}
