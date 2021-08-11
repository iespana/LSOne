using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.CustomerLoyalty.Views
{
    partial class LoyaltyCardsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoyaltyCardsView));
            this.btnsContextButtons = new ContextButtons();
            this.lvCards = new ListView();
            this.columnCardNumber = new Column();
            this.columnTenderType = new Column();
            this.columnCustomer = new Column();
            this.columnScheme = new Column();
            this.columnStartingPts = new Column();
            this.columnIssued = new Column();
            this.columnUsed = new Column();
            this.columnExpired = new Column();
            this.columnCurrentValue = new Column();
            this.columnBalance = new Column();
            this.itemDataScroll = new DatabasePageDisplay();
            this.searchBar1 = new SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.lvCards);
            this.pnlBottom.Controls.Add(this.itemDataScroll);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnsContextButtons_RemoveButtonClicked);
            // 
            // lvCards
            // 
            resources.ApplyResources(this.lvCards, "lvCards");
            this.lvCards.BackColor = System.Drawing.Color.White;
            this.lvCards.BuddyControl = null;
            this.lvCards.Columns.Add(this.columnCardNumber);
            this.lvCards.Columns.Add(this.columnTenderType);
            this.lvCards.Columns.Add(this.columnCustomer);
            this.lvCards.Columns.Add(this.columnScheme);
            this.lvCards.Columns.Add(this.columnStartingPts);
            this.lvCards.Columns.Add(this.columnIssued);
            this.lvCards.Columns.Add(this.columnUsed);
            this.lvCards.Columns.Add(this.columnExpired);
            this.lvCards.Columns.Add(this.columnCurrentValue);
            this.lvCards.Columns.Add(this.columnBalance);
            this.lvCards.ContentBackColor = System.Drawing.Color.White;
            this.lvCards.DefaultRowHeight = ((short)(22));
            this.lvCards.DimSelectionWhenDisabled = true;
            this.lvCards.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCards.ForeColor = System.Drawing.Color.Black;
            this.lvCards.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lvCards.HeaderHeight = ((short)(25));
            this.lvCards.HorizontalScrollbar = true;
            this.lvCards.Name = "lvCards";
            this.lvCards.OddRowColor = System.Drawing.Color.Transparent;
            this.lvCards.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCards.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvCards.SortSetting = "0:1";
            this.lvCards.HeaderClicked += new HeaderDelegate(this.lvCards_HeaderClicked);
            this.lvCards.SelectionChanged += new System.EventHandler(this.lvCards_SelectionChanged);
            this.lvCards.RowDoubleClick += new RowClickDelegate(this.gridControl1_RowDoubleClick);
            // 
            // columnCardNumber
            // 
            this.columnCardNumber.AutoSize = true;
            this.columnCardNumber.Clickable = true;
            this.columnCardNumber.DefaultStyle = null;
            resources.ApplyResources(this.columnCardNumber, "columnCardNumber");
            this.columnCardNumber.MaximumWidth = ((short)(0));
            this.columnCardNumber.MinimumWidth = ((short)(90));
            this.columnCardNumber.Sizable = true;
            this.columnCardNumber.Tag = null;
            this.columnCardNumber.Width = ((short)(90));
            // 
            // columnTenderType
            // 
            this.columnTenderType.AutoSize = true;
            this.columnTenderType.Clickable = true;
            this.columnTenderType.DefaultStyle = null;
            resources.ApplyResources(this.columnTenderType, "columnTenderType");
            this.columnTenderType.MaximumWidth = ((short)(0));
            this.columnTenderType.MinimumWidth = ((short)(60));
            this.columnTenderType.Sizable = true;
            this.columnTenderType.Tag = null;
            this.columnTenderType.Width = ((short)(60));
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
            this.columnCustomer.Width = ((short)(60));
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
            // columnStartingPts
            // 
            this.columnStartingPts.AutoSize = true;
            this.columnStartingPts.Clickable = false;
            this.columnStartingPts.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnStartingPts.DefaultStyle = null;
            resources.ApplyResources(this.columnStartingPts, "columnStartingPts");
            this.columnStartingPts.MaximumWidth = ((short)(0));
            this.columnStartingPts.MinimumWidth = ((short)(10));
            this.columnStartingPts.Sizable = true;
            this.columnStartingPts.Tag = null;
            this.columnStartingPts.Width = ((short)(80));
            // 
            // columnIssued
            // 
            this.columnIssued.AutoSize = true;
            this.columnIssued.Clickable = false;
            this.columnIssued.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnIssued.DefaultStyle = null;
            resources.ApplyResources(this.columnIssued, "columnIssued");
            this.columnIssued.MaximumWidth = ((short)(0));
            this.columnIssued.MinimumWidth = ((short)(10));
            this.columnIssued.Sizable = true;
            this.columnIssued.Tag = null;
            this.columnIssued.Width = ((short)(50));
            // 
            // columnUsed
            // 
            this.columnUsed.AutoSize = true;
            this.columnUsed.Clickable = false;
            this.columnUsed.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnUsed.DefaultStyle = null;
            resources.ApplyResources(this.columnUsed, "columnUsed");
            this.columnUsed.MaximumWidth = ((short)(0));
            this.columnUsed.MinimumWidth = ((short)(10));
            this.columnUsed.Sizable = true;
            this.columnUsed.Tag = null;
            this.columnUsed.Width = ((short)(50));
            // 
            // columnExpired
            // 
            this.columnExpired.AutoSize = true;
            this.columnExpired.Clickable = false;
            this.columnExpired.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnExpired.DefaultStyle = null;
            resources.ApplyResources(this.columnExpired, "columnExpired");
            this.columnExpired.MaximumWidth = ((short)(0));
            this.columnExpired.MinimumWidth = ((short)(10));
            this.columnExpired.Sizable = true;
            this.columnExpired.Tag = null;
            this.columnExpired.Width = ((short)(50));
            // 
            // columnCurrentValue
            // 
            this.columnCurrentValue.AutoSize = true;
            this.columnCurrentValue.Clickable = false;
            this.columnCurrentValue.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnCurrentValue.DefaultStyle = null;
            resources.ApplyResources(this.columnCurrentValue, "columnCurrentValue");
            this.columnCurrentValue.MaximumWidth = ((short)(0));
            this.columnCurrentValue.MinimumWidth = ((short)(10));
            this.columnCurrentValue.Sizable = true;
            this.columnCurrentValue.Tag = null;
            this.columnCurrentValue.Width = ((short)(80));
            // 
            // columnBalance
            // 
            this.columnBalance.AutoSize = true;
            this.columnBalance.Clickable = false;
            this.columnBalance.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnBalance.DefaultStyle = null;
            resources.ApplyResources(this.columnBalance, "columnBalance");
            this.columnBalance.MaximumWidth = ((short)(0));
            this.columnBalance.MinimumWidth = ((short)(10));
            this.columnBalance.Sizable = true;
            this.columnBalance.Tag = null;
            this.columnBalance.Width = ((short)(50));
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
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
            this.searchBar1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.searchBar1_ControlRemoved);
            // 
            // LoyaltyCardsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "LoyaltyCardsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		private DatabasePageDisplay itemDataScroll;
        private ListView lvCards;
		private Column columnCardNumber;
		private Column columnTenderType;
		private Column columnCustomer;
		private Column columnScheme;
		private Column columnCurrentValue;
		private Column columnIssued;
		private Column columnUsed;
		private Column columnExpired;
		private Column columnBalance;
		private ContextButtons btnsContextButtons;
		private Column columnStartingPts;
        private SearchBar searchBar1;

	}
}
