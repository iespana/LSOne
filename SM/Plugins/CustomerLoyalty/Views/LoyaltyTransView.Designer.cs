using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.CustomerLoyalty.Views
{
    partial class LoyaltyTransView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoyaltyTransView));
            this.itemDataScroll = new DatabasePageDisplay();
            this.lvTransactions = new ListView();
            this.columnType = new Column();
            this.columnReceiptNo = new Column();
            this.columnLoyPts = new Column();
            this.columnRemainPts = new Column();
            this.columnStatus = new Column();
            this.columnDate = new Column();
            this.columnIssueDate = new Column();
            this.columnStore = new Column();
            this.columnTerminal = new Column();
            this.columnCardNumber = new Column();
            this.columnScheme = new Column();
            this.columnExpDate = new Column();
            this.columnCustomer = new Column();
            this.columnStaff = new Column();
            this.searchBar1 = new SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.lvTransactions);
            this.pnlBottom.Controls.Add(this.itemDataScroll);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // lvTransactions
            // 
            resources.ApplyResources(this.lvTransactions, "lvTransactions");
            this.lvTransactions.BackColor = System.Drawing.Color.White;
            this.lvTransactions.BuddyControl = null;
            this.lvTransactions.Columns.Add(this.columnType);
            this.lvTransactions.Columns.Add(this.columnReceiptNo);
            this.lvTransactions.Columns.Add(this.columnLoyPts);
            this.lvTransactions.Columns.Add(this.columnRemainPts);
            this.lvTransactions.Columns.Add(this.columnStatus);
            this.lvTransactions.Columns.Add(this.columnDate);
            this.lvTransactions.Columns.Add(this.columnIssueDate);
            this.lvTransactions.Columns.Add(this.columnStore);
            this.lvTransactions.Columns.Add(this.columnTerminal);
            this.lvTransactions.Columns.Add(this.columnCardNumber);
            this.lvTransactions.Columns.Add(this.columnScheme);
            this.lvTransactions.Columns.Add(this.columnExpDate);
            this.lvTransactions.Columns.Add(this.columnCustomer);
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
            this.columnType.RelativeSize = 70;
            this.columnType.Sizable = true;
            this.columnType.Tag = null;
            this.columnType.Width = ((short)(70));
            // 
            // columnReceiptNo
            // 
            this.columnReceiptNo.AutoSize = true;
            this.columnReceiptNo.Clickable = false;
            this.columnReceiptNo.DefaultStyle = null;
            resources.ApplyResources(this.columnReceiptNo, "columnReceiptNo");
            this.columnReceiptNo.MaximumWidth = ((short)(0));
            this.columnReceiptNo.MinimumWidth = ((short)(10));
            this.columnReceiptNo.Sizable = true;
            this.columnReceiptNo.Tag = null;
            this.columnReceiptNo.Width = ((short)(90));
            // 
            // columnLoyPts
            // 
            this.columnLoyPts.AutoSize = true;
            this.columnLoyPts.Clickable = false;
            this.columnLoyPts.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnLoyPts.DefaultStyle = null;
            resources.ApplyResources(this.columnLoyPts, "columnLoyPts");
            this.columnLoyPts.MaximumWidth = ((short)(0));
            this.columnLoyPts.MinimumWidth = ((short)(10));
            this.columnLoyPts.Sizable = true;
            this.columnLoyPts.Tag = null;
            this.columnLoyPts.Width = ((short)(80));
            // 
            // columnRemainPts
            // 
            this.columnRemainPts.AutoSize = true;
            this.columnRemainPts.Clickable = false;
            this.columnRemainPts.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnRemainPts.DefaultStyle = null;
            resources.ApplyResources(this.columnRemainPts, "columnRemainPts");
            this.columnRemainPts.MaximumWidth = ((short)(0));
            this.columnRemainPts.MinimumWidth = ((short)(10));
            this.columnRemainPts.Sizable = true;
            this.columnRemainPts.Tag = null;
            this.columnRemainPts.Width = ((short)(90));
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
            // columnDate
            // 
            this.columnDate.AutoSize = false;
            this.columnDate.Clickable = false;
            this.columnDate.DefaultStyle = null;
            resources.ApplyResources(this.columnDate, "columnDate");
            this.columnDate.MaximumWidth = ((short)(0));
            this.columnDate.MinimumWidth = ((short)(10));
            this.columnDate.RelativeSize = 80;
            this.columnDate.Sizable = true;
            this.columnDate.Tag = null;
            this.columnDate.Width = ((short)(80));
            // 
            // columnIssueDate
            // 
            this.columnIssueDate.AutoSize = false;
            this.columnIssueDate.Clickable = false;
            this.columnIssueDate.DefaultStyle = null;
            resources.ApplyResources(this.columnIssueDate, "columnIssueDate");
            this.columnIssueDate.MaximumWidth = ((short)(0));
            this.columnIssueDate.MinimumWidth = ((short)(10));
            this.columnIssueDate.RelativeSize = 80;
            this.columnIssueDate.Sizable = true;
            this.columnIssueDate.Tag = null;
            this.columnIssueDate.Width = ((short)(80));
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
            // columnCardNumber
            // 
            this.columnCardNumber.AutoSize = true;
            this.columnCardNumber.Clickable = false;
            this.columnCardNumber.DefaultStyle = null;
            resources.ApplyResources(this.columnCardNumber, "columnCardNumber");
            this.columnCardNumber.MaximumWidth = ((short)(0));
            this.columnCardNumber.MinimumWidth = ((short)(10));
            this.columnCardNumber.Sizable = true;
            this.columnCardNumber.Tag = null;
            this.columnCardNumber.Width = ((short)(50));
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
            this.columnExpDate.AutoSize = true;
            this.columnExpDate.Clickable = false;
            this.columnExpDate.DefaultStyle = null;
            resources.ApplyResources(this.columnExpDate, "columnExpDate");
            this.columnExpDate.MaximumWidth = ((short)(0));
            this.columnExpDate.MinimumWidth = ((short)(10));
            this.columnExpDate.RelativeSize = 80;
            this.columnExpDate.Sizable = true;
            this.columnExpDate.Tag = null;
            this.columnExpDate.Width = ((short)(80));
            // 
            // columnCustomer
            // 
            this.columnCustomer.AutoSize = true;
            this.columnCustomer.Clickable = false;
            this.columnCustomer.DefaultStyle = null;
            resources.ApplyResources(this.columnCustomer, "columnCustomer");
            this.columnCustomer.MaximumWidth = ((short)(0));
            this.columnCustomer.MinimumWidth = ((short)(10));
            this.columnCustomer.Sizable = true;
            this.columnCustomer.Tag = null;
            this.columnCustomer.Width = ((short)(50));
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
            // LoyaltyTransView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "LoyaltyTransView";
            this.Load += new System.EventHandler(this.LoyaltyTransView_Load);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private DatabasePageDisplay itemDataScroll;
        private ListView lvTransactions;
		private Column columnType;
		private Column columnReceiptNo;
		private Column columnLoyPts;
		private Column columnRemainPts;
		private Column columnStatus;
		private Column columnIssueDate;
		private Column columnStore;
		private Column columnTerminal;
		private Column columnCardNumber;
		private Column columnScheme;
		private Column columnExpDate;
		private Column columnCustomer;
		private Column columnStaff;
        private Column columnDate;
        private SearchBar searchBar1;

	}
}
