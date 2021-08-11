using LSOne.Controls;

namespace LSOne.ViewPlugins.GiftCards.Views
{
    partial class GiftCardsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GiftCardsView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.giftCardDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.btnActivate = new System.Windows.Forms.Button();
            this.lvGiftCards = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvGiftCards);
            this.pnlBottom.Controls.Add(this.btnActivate);
            this.pnlBottom.Controls.Add(this.giftCardDataScroll);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.searchBar);
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
            // giftCardDataScroll
            // 
            resources.ApplyResources(this.giftCardDataScroll, "giftCardDataScroll");
            this.giftCardDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.giftCardDataScroll.Name = "giftCardDataScroll";
            this.giftCardDataScroll.PageSize = 0;
            this.giftCardDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // btnActivate
            // 
            resources.ApplyResources(this.btnActivate, "btnActivate");
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // lvGiftCards
            // 
            resources.ApplyResources(this.lvGiftCards, "lvGiftCards");
            this.lvGiftCards.BuddyControl = null;
            this.lvGiftCards.Columns.Add(this.column1);
            this.lvGiftCards.Columns.Add(this.column2);
            this.lvGiftCards.Columns.Add(this.column3);
            this.lvGiftCards.Columns.Add(this.column4);
            this.lvGiftCards.Columns.Add(this.column5);
            this.lvGiftCards.Columns.Add(this.column6);
            this.lvGiftCards.Columns.Add(this.column7);
            this.lvGiftCards.ContentBackColor = System.Drawing.Color.White;
            this.lvGiftCards.DefaultRowHeight = ((short)(18));
            this.lvGiftCards.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvGiftCards.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvGiftCards.HeaderHeight = ((short)(20));
            this.lvGiftCards.Name = "lvGiftCards";
            this.lvGiftCards.OddRowColor = System.Drawing.Color.White;
            this.lvGiftCards.RowLineColor = System.Drawing.Color.LightGray;
            this.lvGiftCards.SecondarySortColumn = ((short)(-1));
            this.lvGiftCards.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvGiftCards.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvGiftCards.SortSetting = "0:1";
            this.lvGiftCards.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvGiftCards_HeaderClicked);
            this.lvGiftCards.SelectionChanged += new System.EventHandler(this.lvGiftCards_SelectionChanged);
            this.lvGiftCards.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvGiftCards_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(120));
            this.column1.MinimumWidth = ((short)(120));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Sizable = false;
            this.column1.Tag = null;
            this.column1.Width = ((short)(120));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column2.DefaultStyle = null;
            this.column2.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(112));
            this.column2.MinimumWidth = ((short)(112));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Sizable = false;
            this.column2.Tag = null;
            this.column2.Width = ((short)(112));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(60));
            this.column3.MinimumWidth = ((short)(60));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Sizable = false;
            this.column3.Tag = null;
            this.column3.Width = ((short)(60));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(60));
            this.column4.MinimumWidth = ((short)(60));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Sizable = false;
            this.column4.Tag = null;
            this.column4.Width = ((short)(60));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(60));
            this.column5.MinimumWidth = ((short)(60));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Sizable = false;
            this.column5.Tag = null;
            this.column5.Width = ((short)(60));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(100));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(100));
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
            this.searchBar.SearchOptionChanged += new System.EventHandler(this.searchBar_SearchOptionChanged);
            // 
            // GiftCardsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 98;
            this.Name = "GiftCardsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private SearchBar searchBar;
        private ContextButtons btnsEditAddRemove;
        private DatabasePageDisplay giftCardDataScroll;
        private System.Windows.Forms.Button btnActivate;
        private ListView lvGiftCards;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column6;
        private Controls.Columns.Column column7;
    }
}
