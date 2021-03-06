using LSOne.Controls;

namespace LSOne.ViewPlugins.CreditVouchers.Views
{
    partial class CreditVouchersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreditVouchersView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.dataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.lvCreditVouchers = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.lvCreditVouchers);
            this.pnlBottom.Controls.Add(this.dataScroll);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditRemove;
            this.btnsEditAddRemove.EditButtonEnabled = true;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = true;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // dataScroll
            // 
            resources.ApplyResources(this.dataScroll, "dataScroll");
            this.dataScroll.BackColor = System.Drawing.Color.Transparent;
            this.dataScroll.Name = "dataScroll";
            this.dataScroll.PageSize = 0;
            this.dataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // lvCreditVouchers
            // 
            resources.ApplyResources(this.lvCreditVouchers, "lvCreditVouchers");
            this.lvCreditVouchers.BuddyControl = null;
            this.lvCreditVouchers.Columns.Add(this.column1);
            this.lvCreditVouchers.Columns.Add(this.column2);
            this.lvCreditVouchers.Columns.Add(this.column3);
            this.lvCreditVouchers.Columns.Add(this.column4);
            this.lvCreditVouchers.Columns.Add(this.column5);
            this.lvCreditVouchers.ContentBackColor = System.Drawing.Color.White;
            this.lvCreditVouchers.DefaultRowHeight = ((short)(18));
            this.lvCreditVouchers.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCreditVouchers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCreditVouchers.HeaderHeight = ((short)(20));
            this.lvCreditVouchers.Name = "lvCreditVouchers";
            this.lvCreditVouchers.OddRowColor = System.Drawing.Color.White;
            this.lvCreditVouchers.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCreditVouchers.SecondarySortColumn = ((short)(-1));
            this.lvCreditVouchers.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvCreditVouchers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCreditVouchers.SortSetting = "0:1";
            this.lvCreditVouchers.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvCreditVouchers_HeaderClick);
            this.lvCreditVouchers.SelectionChanged += new System.EventHandler(this.lvCreditVouchers_SelectionChanged);
            this.lvCreditVouchers.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvCreditVouchers_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(120));
            this.column1.MinimumWidth = ((short)(120));
            this.column1.SecondarySortColumn = ((short)(-1));
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
            this.column3.Tag = null;
            this.column3.Width = ((short)(60));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(100));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(100));
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
            // CreditVouchersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 98;
            this.Name = "CreditVouchersView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsEditAddRemove;
        private DatabasePageDisplay dataScroll;
        private ListView lvCreditVouchers;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private SearchBar searchBar;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
    }
}
