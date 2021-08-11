using LSOne.Controls;

namespace LSOne.ViewPlugins.CentralSuspensions.Views
{
    partial class SuspendedTransactionsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuspendedTransactionsView));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lvSuspendedTransAddInfo = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnRemove = new LSOne.Controls.ContextButton();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.lvSuspendedTransactions = new LSOne.Controls.ListView();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvSuspendedTransactions);
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            this.pnlBottom.Controls.Add(this.btnRemove);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.lvSuspendedTransAddInfo);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lvSuspendedTransAddInfo
            // 
            resources.ApplyResources(this.lvSuspendedTransAddInfo, "lvSuspendedTransAddInfo");
            this.lvSuspendedTransAddInfo.BorderColor = System.Drawing.Color.DarkGray;
            this.lvSuspendedTransAddInfo.BuddyControl = null;
            this.lvSuspendedTransAddInfo.Columns.Add(this.column1);
            this.lvSuspendedTransAddInfo.Columns.Add(this.column2);
            this.lvSuspendedTransAddInfo.ContentBackColor = System.Drawing.Color.White;
            this.lvSuspendedTransAddInfo.DefaultRowHeight = ((short)(22));
            this.lvSuspendedTransAddInfo.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvSuspendedTransAddInfo.HeaderBackColor = System.Drawing.Color.White;
            this.lvSuspendedTransAddInfo.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSuspendedTransAddInfo.HeaderHeight = ((short)(25));
            this.lvSuspendedTransAddInfo.Name = "lvSuspendedTransAddInfo";
            this.lvSuspendedTransAddInfo.OddRowColor = System.Drawing.Color.White;
            this.lvSuspendedTransAddInfo.RowLineColor = System.Drawing.Color.LightGray;
            this.lvSuspendedTransAddInfo.SecondarySortColumn = ((short)(-1));
            this.lvSuspendedTransAddInfo.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvSuspendedTransAddInfo.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSuspendedTransAddInfo.SortSetting = "0:1";
            this.lvSuspendedTransAddInfo.VerticalScrollbarValue = 0;
            this.lvSuspendedTransAddInfo.VerticalScrollbarYOffset = 0;
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
            this.column1.Width = ((short)(60));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(60));
            // 
            // lblGroupHeader
            // 
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnRemove
            // 
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnRemove.Context = LSOne.Controls.ButtonType.Remove;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
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
            // lvSuspendedTransactions
            // 
            resources.ApplyResources(this.lvSuspendedTransactions, "lvSuspendedTransactions");
            this.lvSuspendedTransactions.BorderColor = System.Drawing.Color.DarkGray;
            this.lvSuspendedTransactions.BuddyControl = null;
            this.lvSuspendedTransactions.Columns.Add(this.column3);
            this.lvSuspendedTransactions.Columns.Add(this.column4);
            this.lvSuspendedTransactions.Columns.Add(this.column5);
            this.lvSuspendedTransactions.Columns.Add(this.column6);
            this.lvSuspendedTransactions.Columns.Add(this.column7);
            this.lvSuspendedTransactions.Columns.Add(this.column8);
            this.lvSuspendedTransactions.ContentBackColor = System.Drawing.Color.White;
            this.lvSuspendedTransactions.DefaultRowHeight = ((short)(22));
            this.lvSuspendedTransactions.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvSuspendedTransactions.HeaderBackColor = System.Drawing.Color.White;
            this.lvSuspendedTransactions.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSuspendedTransactions.HeaderHeight = ((short)(25));
            this.lvSuspendedTransactions.Name = "lvSuspendedTransactions";
            this.lvSuspendedTransactions.OddRowColor = System.Drawing.Color.White;
            this.lvSuspendedTransactions.RowLineColor = System.Drawing.Color.LightGray;
            this.lvSuspendedTransactions.SecondarySortColumn = ((short)(-1));
            this.lvSuspendedTransactions.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvSuspendedTransactions.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvSuspendedTransactions.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSuspendedTransactions.SortSetting = "0:1";
            this.lvSuspendedTransactions.VerticalScrollbarValue = 0;
            this.lvSuspendedTransactions.VerticalScrollbarYOffset = 0;
            this.lvSuspendedTransactions.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvSuspendedTransactions_HeaderClick);
            this.lvSuspendedTransactions.SelectionChanged += new System.EventHandler(this.lvSuspendedTransactions_SelectionChanged);
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(93));
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
            this.column4.Width = ((short)(120));
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
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(112));
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
            this.column7.Width = ((short)(60));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.SecondarySortColumn = ((short)(-1));
            this.column8.Tag = null;
            this.column8.Width = ((short)(60));
            // 
            // SuspendedTransactionsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 75;
            this.Name = "SuspendedTransactionsView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Label lblNoSelection;
        private ContextButton btnRemove;
        private SearchBar searchBar;
        private ListView lvSuspendedTransAddInfo;
        private ListView lvSuspendedTransactions;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column6;
        private Controls.Columns.Column column7;
        private Controls.Columns.Column column8;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
    }
}
