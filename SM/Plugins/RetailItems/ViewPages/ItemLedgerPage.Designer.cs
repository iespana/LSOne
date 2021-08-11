using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemLedgerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemLedgerPage));
            this.lvLedger = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column12 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column13 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.SuspendLayout();
            // 
            // lvLedger
            // 
            resources.ApplyResources(this.lvLedger, "lvLedger");
            this.lvLedger.BuddyControl = null;
            this.lvLedger.Columns.Add(this.column1);
            this.lvLedger.Columns.Add(this.column2);
            this.lvLedger.Columns.Add(this.column3);
            this.lvLedger.Columns.Add(this.column4);
            this.lvLedger.Columns.Add(this.column12);
            this.lvLedger.Columns.Add(this.column5);
            this.lvLedger.Columns.Add(this.column6);
            this.lvLedger.Columns.Add(this.column13);
            this.lvLedger.Columns.Add(this.column7);
            this.lvLedger.Columns.Add(this.column8);
            this.lvLedger.ContentBackColor = System.Drawing.Color.White;
            this.lvLedger.DefaultRowHeight = ((short)(22));
            this.lvLedger.EvenRowColor = System.Drawing.Color.White;
            this.lvLedger.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLedger.HeaderHeight = ((short)(25));
            this.lvLedger.HorizontalScrollbar = true;
            this.lvLedger.Name = "lvLedger";
            this.lvLedger.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvLedger.RowLineColor = System.Drawing.Color.LightGray;
            this.lvLedger.SecondarySortColumn = ((short)(-1));
            this.lvLedger.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLedger.SortSetting = "0:1";
            this.lvLedger.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvLedger_HeaderClicked);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(40));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.Clickable = false;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(40));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.Clickable = false;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.Clickable = false;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column12
            // 
            this.column12.AutoSize = true;
            this.column12.Clickable = false;
            this.column12.DefaultStyle = null;
            resources.ApplyResources(this.column12, "column12");
            this.column12.MaximumWidth = ((short)(0));
            this.column12.MinimumWidth = ((short)(10));
            this.column12.SecondarySortColumn = ((short)(-1));
            this.column12.Tag = null;
            this.column12.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.Clickable = false;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(90));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.Clickable = false;
            this.column6.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // column13
            // 
            this.column13.AutoSize = true;
            this.column13.DefaultStyle = null;
            resources.ApplyResources(this.column13, "column13");
            this.column13.MaximumWidth = ((short)(0));
            this.column13.MinimumWidth = ((short)(10));
            this.column13.SecondarySortColumn = ((short)(-1));
            this.column13.Tag = null;
            this.column13.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.Clickable = false;
            this.column7.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(60));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.Clickable = false;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(50));
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
            this.searchBar1.DefaultNumberOfSections = 2;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SearchOptionEnabled = true;
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.SaveAsDefault += new System.EventHandler(this.searchBar1_SaveAsDefault);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            this.searchBar1.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar1_UnknownControlAdd);
            this.searchBar1.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar1_UnknownControlRemove);
            this.searchBar1.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar1_UnknownControlHasSelection);
            this.searchBar1.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar1_UnknownControlGetSelection);
            this.searchBar1.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar1_UnknownControlSetSelection);
            // 
            // ItemLedgerPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.searchBar1);
            this.Controls.Add(this.lvLedger);
            this.Controls.Add(this.itemDataScroll);
            this.Name = "ItemLedgerPage";
            this.ResumeLayout(false);

        }

        #endregion

        private DatabasePageDisplay itemDataScroll;
        private ListView lvLedger;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private Column column5;
        private Column column6;
        private Column column7;
        private SearchBar searchBar1;
        private Column column12;
        private Column column13;
        private Column column8;
    }
}
