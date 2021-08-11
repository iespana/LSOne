namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class StockCountingItemPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockCountingItemPage));
            this.btnPostLine = new System.Windows.Forms.Button();
            this.btnPostAllLines = new System.Windows.Forms.Button();
            this.lvStockCount = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.column10 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column11 = new LSOne.Controls.Columns.Column();
            this.column12 = new LSOne.Controls.Columns.Column();
            this.column13 = new LSOne.Controls.Columns.Column();
            this.column14 = new LSOne.Controls.Columns.Column();
            this.colPicture = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.stocCountDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProcessingStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPostLine
            // 
            resources.ApplyResources(this.btnPostLine, "btnPostLine");
            this.btnPostLine.Name = "btnPostLine";
            this.btnPostLine.UseVisualStyleBackColor = true;
            this.btnPostLine.Click += new System.EventHandler(this.btnPostLine_Click);
            // 
            // btnPostAllLines
            // 
            resources.ApplyResources(this.btnPostAllLines, "btnPostAllLines");
            this.btnPostAllLines.Name = "btnPostAllLines";
            this.btnPostAllLines.UseVisualStyleBackColor = true;
            this.btnPostAllLines.Click += new System.EventHandler(this.btnPostAllLines_Click);
            // 
            // lvStockCount
            // 
            resources.ApplyResources(this.lvStockCount, "lvStockCount");
            this.lvStockCount.BuddyControl = null;
            this.lvStockCount.Columns.Add(this.column1);
            this.lvStockCount.Columns.Add(this.column2);
            this.lvStockCount.Columns.Add(this.column3);
            this.lvStockCount.Columns.Add(this.column5);
            this.lvStockCount.Columns.Add(this.column4);
            this.lvStockCount.Columns.Add(this.column9);
            this.lvStockCount.Columns.Add(this.column10);
            this.lvStockCount.Columns.Add(this.column6);
            this.lvStockCount.Columns.Add(this.column7);
            this.lvStockCount.Columns.Add(this.column8);
            this.lvStockCount.Columns.Add(this.column11);
            this.lvStockCount.Columns.Add(this.column12);
            this.lvStockCount.Columns.Add(this.column13);
            this.lvStockCount.Columns.Add(this.column14);
            this.lvStockCount.Columns.Add(this.colPicture);
            this.lvStockCount.ContentBackColor = System.Drawing.Color.White;
            this.lvStockCount.DefaultRowHeight = ((short)(22));
            this.lvStockCount.DimSelectionWhenDisabled = true;
            this.lvStockCount.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvStockCount.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvStockCount.HeaderHeight = ((short)(25));
            this.lvStockCount.HorizontalScrollbar = true;
            this.lvStockCount.Name = "lvStockCount";
            this.lvStockCount.OddRowColor = System.Drawing.Color.White;
            this.lvStockCount.RowLineColor = System.Drawing.Color.LightGray;
            this.lvStockCount.SecondarySortColumn = ((short)(-1));
            this.lvStockCount.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvStockCount.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvStockCount.SortSetting = "0:1";
            this.lvStockCount.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvStockCount_HeaderClicked);
            this.lvStockCount.SelectionChanged += new System.EventHandler(this.lvStockCount_SelectionChanged);
            this.lvStockCount.CellAction += new LSOne.Controls.CellActionDelegate(this.LvStockCount_CellAction);
            this.lvStockCount.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvStockCount_RowDoubleClick);
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
            this.column1.Width = ((short)(50));
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
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            this.column3.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.NoTextWhenSmall = true;
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
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
            this.column5.Width = ((short)(50));
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
            this.column4.Width = ((short)(50));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.SecondarySortColumn = ((short)(-1));
            this.column9.Tag = null;
            this.column9.Width = ((short)(50));
            // 
            // column10
            // 
            this.column10.AutoSize = true;
            this.column10.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column10.DefaultStyle = null;
            resources.ApplyResources(this.column10, "column10");
            this.column10.MaximumWidth = ((short)(0));
            this.column10.MinimumWidth = ((short)(10));
            this.column10.SecondarySortColumn = ((short)(-1));
            this.column10.Tag = null;
            this.column10.Width = ((short)(50));
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
            this.column6.Width = ((short)(50));
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
            this.column7.Width = ((short)(50));
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
            this.column8.Width = ((short)(50));
            // 
            // column11
            // 
            this.column11.AutoSize = true;
            this.column11.DefaultStyle = null;
            this.column11.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column11, "column11");
            this.column11.MaximumWidth = ((short)(0));
            this.column11.MinimumWidth = ((short)(10));
            this.column11.NoTextWhenSmall = true;
            this.column11.SecondarySortColumn = ((short)(-1));
            this.column11.Tag = null;
            this.column11.Width = ((short)(50));
            // 
            // column12
            // 
            this.column12.AutoSize = true;
            this.column12.DefaultStyle = null;
            this.column12.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column12, "column12");
            this.column12.MaximumWidth = ((short)(0));
            this.column12.MinimumWidth = ((short)(10));
            this.column12.NoTextWhenSmall = true;
            this.column12.SecondarySortColumn = ((short)(-1));
            this.column12.Tag = null;
            this.column12.Width = ((short)(50));
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
            // column14
            // 
            this.column14.AutoSize = true;
            this.column14.DefaultStyle = null;
            resources.ApplyResources(this.column14, "column14");
            this.column14.MaximumWidth = ((short)(0));
            this.column14.MinimumWidth = ((short)(10));
            this.column14.SecondarySortColumn = ((short)(-1));
            this.column14.Tag = null;
            this.column14.Width = ((short)(50));
            // 
            // colPicture
            // 
            this.colPicture.AutoSize = true;
            this.colPicture.Clickable = false;
            this.colPicture.DefaultStyle = null;
            resources.ApplyResources(this.colPicture, "colPicture");
            this.colPicture.MaximumWidth = ((short)(0));
            this.colPicture.MinimumWidth = ((short)(10));
            this.colPicture.SecondarySortColumn = ((short)(-1));
            this.colPicture.Tag = null;
            this.colPicture.Width = ((short)(50));
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = true;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = true;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnRemoveClicked);
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.MaxNumberOfSections = 15;
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
            // stocCountDataScroll
            // 
            resources.ApplyResources(this.stocCountDataScroll, "stocCountDataScroll");
            this.stocCountDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.stocCountDataScroll.Name = "stocCountDataScroll";
            this.stocCountDataScroll.PageSize = 0;
            this.stocCountDataScroll.PageChanged += new System.EventHandler(this.itemDataScroll_PageChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblProcessingStatus
            // 
            resources.ApplyResources(this.lblProcessingStatus, "lblProcessingStatus");
            this.lblProcessingStatus.Name = "lblProcessingStatus";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.lblProcessingStatus, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // StockCountingItemPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.stocCountDataScroll);
            this.Controls.Add(this.searchBar1);
            this.Controls.Add(this.btnsEditAddRemove);
            this.Controls.Add(this.btnPostLine);
            this.Controls.Add(this.btnPostAllLines);
            this.Controls.Add(this.lvStockCount);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "StockCountingItemPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private Controls.ListView lvStockCount;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column6;
        private Controls.Columns.Column column7;
        private Controls.Columns.Column column8;
        private Controls.ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.Button btnPostLine;
        private System.Windows.Forms.Button btnPostAllLines;
        private Controls.SearchBar searchBar1;
        private Controls.Columns.Column column9;
        private Controls.Columns.Column column10;
        private Controls.Columns.Column column11;
        private Controls.Columns.Column column12;
        private Controls.DatabasePageDisplay stocCountDataScroll;
        private Controls.Columns.Column column13;
        private Controls.Columns.Column column14;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProcessingStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Controls.Columns.Column colPicture;
    }
}
