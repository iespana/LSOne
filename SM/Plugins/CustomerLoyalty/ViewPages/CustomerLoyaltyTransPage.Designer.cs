using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.CustomerLoyalty.ViewPages
{
	partial class CustomerLoyaltyTransPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerLoyaltyTransPage));
            this.lvTransactions = new ListView();
            this.columnType = new Column();
            this.columnReceipt = new Column();
            this.columnLoyaltyPoints = new Column();
            this.columnRemainingPoints = new Column();
            this.columnStatus = new Column();
            this.columnCreatedDate = new Column();
            this.columnIssuedDate = new Column();
            this.columnStore = new Column();
            this.columnTerminal = new Column();
            this.columnCard = new Column();
            this.columnScheme = new Column();
            this.columnExpDate = new Column();
            this.columnStaff = new Column();
            this.itemDataScroll = new DatabasePageDisplay();
            this.searchBar1 = new SearchBar();
            this.SuspendLayout();
            // 
            // lvTransactions
            // 
            resources.ApplyResources(this.lvTransactions, "lvTransactions");
            this.lvTransactions.BackColor = System.Drawing.Color.White;
            this.lvTransactions.BuddyControl = null;
            this.lvTransactions.Columns.Add(this.columnType);
            this.lvTransactions.Columns.Add(this.columnReceipt);
            this.lvTransactions.Columns.Add(this.columnLoyaltyPoints);
            this.lvTransactions.Columns.Add(this.columnRemainingPoints);
            this.lvTransactions.Columns.Add(this.columnStatus);
            this.lvTransactions.Columns.Add(this.columnCreatedDate);
            this.lvTransactions.Columns.Add(this.columnIssuedDate);
            this.lvTransactions.Columns.Add(this.columnStore);
            this.lvTransactions.Columns.Add(this.columnTerminal);
            this.lvTransactions.Columns.Add(this.columnCard);
            this.lvTransactions.Columns.Add(this.columnScheme);
            this.lvTransactions.Columns.Add(this.columnExpDate);
            this.lvTransactions.Columns.Add(this.columnStaff);
            this.lvTransactions.ContentBackColor = System.Drawing.Color.White;
            this.lvTransactions.DefaultRowHeight = ((short)(22));
            this.lvTransactions.DimSelectionWhenDisabled = true;
            this.lvTransactions.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvTransactions.ForeColor = System.Drawing.Color.Black;
            this.lvTransactions.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lvTransactions.HeaderHeight = ((short)(25));
            this.lvTransactions.HorizontalScrollbar = true;
            this.lvTransactions.Name = "lvTransactions";
            this.lvTransactions.OddRowColor = System.Drawing.Color.Transparent;
            this.lvTransactions.RowLineColor = System.Drawing.Color.LightGray;
            this.lvTransactions.SelectionModel = ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvTransactions.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvTransactions.SortSetting = "0:1";
            // 
            // columnType
            // 
            this.columnType.AutoSize = true;
            this.columnType.Clickable = false;
            this.columnType.DefaultStyle = null;
            resources.ApplyResources(this.columnType, "columnType");
            this.columnType.MaximumWidth = ((short)(0));
            this.columnType.MinimumWidth = ((short)(10));
            this.columnType.Sizable = true;
            this.columnType.Tag = null;
            this.columnType.Width = ((short)(50));
            // 
            // columnReceipt
            // 
            this.columnReceipt.AutoSize = true;
            this.columnReceipt.Clickable = false;
            this.columnReceipt.DefaultStyle = null;
            resources.ApplyResources(this.columnReceipt, "columnReceipt");
            this.columnReceipt.MaximumWidth = ((short)(0));
            this.columnReceipt.MinimumWidth = ((short)(10));
            this.columnReceipt.Sizable = true;
            this.columnReceipt.Tag = null;
            this.columnReceipt.Width = ((short)(100));
            // 
            // columnLoyaltyPoints
            // 
            this.columnLoyaltyPoints.AutoSize = true;
            this.columnLoyaltyPoints.Clickable = false;
            this.columnLoyaltyPoints.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnLoyaltyPoints.DefaultStyle = null;
            resources.ApplyResources(this.columnLoyaltyPoints, "columnLoyaltyPoints");
            this.columnLoyaltyPoints.MaximumWidth = ((short)(0));
            this.columnLoyaltyPoints.MinimumWidth = ((short)(10));
            this.columnLoyaltyPoints.Sizable = true;
            this.columnLoyaltyPoints.Tag = null;
            this.columnLoyaltyPoints.Width = ((short)(80));
            // 
            // columnRemainingPoints
            // 
            this.columnRemainingPoints.AutoSize = true;
            this.columnRemainingPoints.Clickable = false;
            this.columnRemainingPoints.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnRemainingPoints.DefaultStyle = null;
            resources.ApplyResources(this.columnRemainingPoints, "columnRemainingPoints");
            this.columnRemainingPoints.MaximumWidth = ((short)(0));
            this.columnRemainingPoints.MinimumWidth = ((short)(10));
            this.columnRemainingPoints.Sizable = true;
            this.columnRemainingPoints.Tag = null;
            this.columnRemainingPoints.Width = ((short)(100));
            // 
            // columnStatus
            // 
            this.columnStatus.AutoSize = true;
            this.columnStatus.Clickable = false;
            this.columnStatus.DefaultStyle = null;
            resources.ApplyResources(this.columnStatus, "columnStatus");
            this.columnStatus.MaximumWidth = ((short)(0));
            this.columnStatus.MinimumWidth = ((short)(10));
            this.columnStatus.Sizable = true;
            this.columnStatus.Tag = null;
            this.columnStatus.Width = ((short)(50));
            // 
            // columnCreatedDate
            // 
            this.columnCreatedDate.AutoSize = true;
            this.columnCreatedDate.Clickable = false;
            this.columnCreatedDate.DefaultStyle = null;
            resources.ApplyResources(this.columnCreatedDate, "columnCreatedDate");
            this.columnCreatedDate.MaximumWidth = ((short)(0));
            this.columnCreatedDate.MinimumWidth = ((short)(70));
            this.columnCreatedDate.Sizable = true;
            this.columnCreatedDate.Tag = null;
            this.columnCreatedDate.Width = ((short)(70));
            // 
            // columnIssuedDate
            // 
            this.columnIssuedDate.AutoSize = true;
            this.columnIssuedDate.Clickable = false;
            this.columnIssuedDate.DefaultStyle = null;
            resources.ApplyResources(this.columnIssuedDate, "columnIssuedDate");
            this.columnIssuedDate.MaximumWidth = ((short)(0));
            this.columnIssuedDate.MinimumWidth = ((short)(70));
            this.columnIssuedDate.Sizable = true;
            this.columnIssuedDate.Tag = null;
            this.columnIssuedDate.Width = ((short)(70));
            // 
            // columnStore
            // 
            this.columnStore.AutoSize = true;
            this.columnStore.Clickable = false;
            this.columnStore.DefaultStyle = null;
            resources.ApplyResources(this.columnStore, "columnStore");
            this.columnStore.MaximumWidth = ((short)(0));
            this.columnStore.MinimumWidth = ((short)(10));
            this.columnStore.Sizable = true;
            this.columnStore.Tag = null;
            this.columnStore.Width = ((short)(50));
            // 
            // columnTerminal
            // 
            this.columnTerminal.AutoSize = true;
            this.columnTerminal.Clickable = false;
            this.columnTerminal.DefaultStyle = null;
            resources.ApplyResources(this.columnTerminal, "columnTerminal");
            this.columnTerminal.MaximumWidth = ((short)(0));
            this.columnTerminal.MinimumWidth = ((short)(10));
            this.columnTerminal.Sizable = true;
            this.columnTerminal.Tag = null;
            this.columnTerminal.Width = ((short)(50));
            // 
            // columnCard
            // 
            this.columnCard.AutoSize = true;
            this.columnCard.Clickable = false;
            this.columnCard.DefaultStyle = null;
            resources.ApplyResources(this.columnCard, "columnCard");
            this.columnCard.MaximumWidth = ((short)(0));
            this.columnCard.MinimumWidth = ((short)(10));
            this.columnCard.Sizable = true;
            this.columnCard.Tag = null;
            this.columnCard.Width = ((short)(50));
            // 
            // columnScheme
            // 
            this.columnScheme.AutoSize = true;
            this.columnScheme.Clickable = false;
            this.columnScheme.DefaultStyle = null;
            resources.ApplyResources(this.columnScheme, "columnScheme");
            this.columnScheme.MaximumWidth = ((short)(0));
            this.columnScheme.MinimumWidth = ((short)(60));
            this.columnScheme.Sizable = true;
            this.columnScheme.Tag = null;
            this.columnScheme.Width = ((short)(60));
            // 
            // columnExpDate
            // 
            this.columnExpDate.AutoSize = false;
            this.columnExpDate.Clickable = false;
            this.columnExpDate.DefaultStyle = null;
            resources.ApplyResources(this.columnExpDate, "columnExpDate");
            this.columnExpDate.MaximumWidth = ((short)(0));
            this.columnExpDate.MinimumWidth = ((short)(90));
            this.columnExpDate.Sizable = true;
            this.columnExpDate.Tag = null;
            this.columnExpDate.Width = ((short)(90));
            // 
            // columnStaff
            // 
            this.columnStaff.AutoSize = true;
            this.columnStaff.Clickable = false;
            this.columnStaff.DefaultStyle = null;
            resources.ApplyResources(this.columnStaff, "columnStaff");
            this.columnStaff.MaximumWidth = ((short)(0));
            this.columnStaff.MinimumWidth = ((short)(10));
            this.columnStaff.Sizable = true;
            this.columnStaff.Tag = null;
            this.columnStaff.Width = ((short)(50));
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.SaveAsDefault += new System.EventHandler(this.searchBar1_SaveAsDefault);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            this.searchBar1.UnknownControlAdd += new UnknownControlCreatedDelegate(this.searchBar1_UnknownControlAdd);
            this.searchBar1.UnknownControlRemove += new UnknownControlDelegate(this.searchBar1_UnknownControlRemove);
            this.searchBar1.UnknownControlHasSelection += new UnknownControlSelectionDelegate(this.searchBar1_UnknownControlHasSelection);
            this.searchBar1.UnknownControlGetSelection += new UnknownControlGetSelectionDelegate(this.searchBar1_UnknownControlGetSelection);
            this.searchBar1.UnknownControlSetSelection += new UnknownControlSetSelectionDelegate(this.searchBar1_UnknownControlSetSelection);
            // 
            // CustomerLoyaltyTransPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.searchBar1);
            this.Controls.Add(this.itemDataScroll);
            this.Controls.Add(this.lvTransactions);
            this.Name = "CustomerLoyaltyTransPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvTransactions;
		private Column columnType;
		private Column columnReceipt;
		private Column columnLoyaltyPoints;
		private Column columnRemainingPoints;
		private Column columnStatus;
		private Column columnIssuedDate;
		private Column columnStore;
		private Column columnTerminal;
		private Column columnCard;
		private Column columnScheme;
		private Column columnExpDate;
        private Column columnStaff;
		private DatabasePageDisplay itemDataScroll;
        private Column columnCreatedDate;
        private SearchBar searchBar1;


	}
}
