using System.Windows.Forms;
using LSOne.Controls;

namespace LSOne.ViewPlugins.ReceiptBrowser.Views
{
    partial class ReceiptBrowserView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiptBrowserView));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnView = new System.Windows.Forms.Button();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.lvReceipts = new LSOne.Controls.ListView();
            this.colTime = new LSOne.Controls.Columns.Column();
            this.colReceipt = new LSOne.Controls.Columns.Column();
            this.colEmployee = new LSOne.Controls.Columns.Column();
            this.colTerminal = new LSOne.Controls.Columns.Column();
            this.colStore = new LSOne.Controls.Columns.Column();
            this.colAmount = new LSOne.Controls.Columns.Column();
            this.receiptsDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.lvReceipts);
            this.pnlBottom.Controls.Add(this.btnView);
            this.pnlBottom.Controls.Add(this.receiptsDataScroll);
            this.pnlBottom.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBottom_Paint);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnView
            // 
            resources.ApplyResources(this.btnView, "btnView");
            this.btnView.Name = "btnView";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
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
            this.searchBar1.Load += new System.EventHandler(this.searchBar1_Load);
            // 
            // lvReceipts
            // 
            resources.ApplyResources(this.lvReceipts, "lvReceipts");
            this.lvReceipts.BorderColor = System.Drawing.Color.DarkGray;
            this.lvReceipts.BuddyControl = null;
            this.lvReceipts.Columns.Add(this.colTime);
            this.lvReceipts.Columns.Add(this.colReceipt);
            this.lvReceipts.Columns.Add(this.colEmployee);
            this.lvReceipts.Columns.Add(this.colTerminal);
            this.lvReceipts.Columns.Add(this.colStore);
            this.lvReceipts.Columns.Add(this.colAmount);
            this.lvReceipts.ContentBackColor = System.Drawing.Color.White;
            this.lvReceipts.DefaultRowHeight = ((short)(22));
            this.lvReceipts.EvenRowColor = System.Drawing.Color.White;
            this.lvReceipts.HeaderBackColor = System.Drawing.Color.White;
            this.lvReceipts.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvReceipts.HeaderHeight = ((short)(25));
            this.lvReceipts.HorizontalScrollbar = true;
            this.lvReceipts.Name = "lvReceipts";
            this.lvReceipts.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvReceipts.RowLineColor = System.Drawing.Color.LightGray;
            this.lvReceipts.SecondarySortColumn = ((short)(-1));
            this.lvReceipts.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvReceipts.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvReceipts.SortSetting = "0:1";
            this.lvReceipts.VerticalScrollbarValue = 0;
            this.lvReceipts.VerticalScrollbarYOffset = 0;
            this.lvReceipts.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvReceipt_HeaderClicked);
            this.lvReceipts.SelectionChanged += new System.EventHandler(this.lvReceipt_SelectionChanged);
            this.lvReceipts.CellAction += new LSOne.Controls.CellActionDelegate(this.lvReceipt_CellAction);
            this.lvReceipts.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvReceipt_RowDoubleClick);
            this.lvReceipts.Load += new System.EventHandler(this.lvReceipts_Load);
            this.lvReceipts.DoubleClick += new System.EventHandler(this.lvReceipts_DoubleClick);
            // 
            // colTime
            // 
            this.colTime.AutoSize = true;
            this.colTime.DefaultStyle = null;
            resources.ApplyResources(this.colTime, "colTime");
            this.colTime.MaximumWidth = ((short)(0));
            this.colTime.MinimumWidth = ((short)(10));
            this.colTime.SecondarySortColumn = ((short)(-1));
            this.colTime.Tag = null;
            this.colTime.Width = ((short)(100));
            // 
            // colReceipt
            // 
            this.colReceipt.AutoSize = true;
            this.colReceipt.DefaultStyle = null;
            resources.ApplyResources(this.colReceipt, "colReceipt");
            this.colReceipt.MaximumWidth = ((short)(0));
            this.colReceipt.MinimumWidth = ((short)(10));
            this.colReceipt.SecondarySortColumn = ((short)(-1));
            this.colReceipt.Tag = null;
            this.colReceipt.Width = ((short)(100));
            // 
            // colEmployee
            // 
            this.colEmployee.AutoSize = true;
            this.colEmployee.DefaultStyle = null;
            resources.ApplyResources(this.colEmployee, "colEmployee");
            this.colEmployee.MaximumWidth = ((short)(0));
            this.colEmployee.MinimumWidth = ((short)(10));
            this.colEmployee.SecondarySortColumn = ((short)(-1));
            this.colEmployee.Tag = null;
            this.colEmployee.Width = ((short)(100));
            // 
            // colTerminal
            // 
            this.colTerminal.AutoSize = true;
            this.colTerminal.DefaultStyle = null;
            resources.ApplyResources(this.colTerminal, "colTerminal");
            this.colTerminal.MaximumWidth = ((short)(0));
            this.colTerminal.MinimumWidth = ((short)(10));
            this.colTerminal.SecondarySortColumn = ((short)(-1));
            this.colTerminal.Tag = null;
            this.colTerminal.Width = ((short)(100));
            // 
            // colStore
            // 
            this.colStore.AutoSize = true;
            this.colStore.DefaultStyle = null;
            resources.ApplyResources(this.colStore, "colStore");
            this.colStore.MaximumWidth = ((short)(0));
            this.colStore.MinimumWidth = ((short)(10));
            this.colStore.SecondarySortColumn = ((short)(-1));
            this.colStore.Tag = null;
            this.colStore.Width = ((short)(100));
            // 
            // colAmount
            // 
            this.colAmount.AutoSize = true;
            this.colAmount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colAmount.DefaultStyle = null;
            resources.ApplyResources(this.colAmount, "colAmount");
            this.colAmount.MaximumWidth = ((short)(0));
            this.colAmount.MinimumWidth = ((short)(10));
            this.colAmount.SecondarySortColumn = ((short)(-1));
            this.colAmount.Tag = null;
            this.colAmount.Width = ((short)(100));
            // 
            // receiptsDataScroll
            // 
            resources.ApplyResources(this.receiptsDataScroll, "receiptsDataScroll");
            this.receiptsDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.receiptsDataScroll.Name = "receiptsDataScroll";
            this.receiptsDataScroll.PageSize = 0;
            this.receiptsDataScroll.PageChanged += new System.EventHandler(this.receiptsDataScroll_PageChanged);
            this.receiptsDataScroll.Load += new System.EventHandler(this.receiptsDataScroll_Load);
            // 
            // ReceiptBrowserView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ReceiptBrowserView";
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnView;
        private LSOne.Controls.SearchBar searchBar1;
        private LSOne.Controls.ListView lvReceipts;
        private Controls.Columns.Column colTime;
        private Controls.Columns.Column colReceipt;
        private Controls.Columns.Column colEmployee;
        private Controls.Columns.Column colTerminal;
        private Controls.Columns.Column colStore;
        private Controls.Columns.Column colAmount;
        private DatabasePageDisplay receiptsDataScroll;

    }
}
