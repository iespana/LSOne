namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemCostPage
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
            this.lvCosts = new LSOne.Controls.ListView();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.clmCost = new LSOne.Controls.Columns.Column();
            this.clmQuantity = new LSOne.Controls.Columns.Column();
            this.clmTotal = new LSOne.Controls.Columns.Column();
            this.clmDate = new LSOne.Controls.Columns.Column();
            this.clmReason = new LSOne.Controls.Columns.Column();
            this.clmUser = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.itemCostDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.btnEdit = new LSOne.Controls.ContextButton();
            this.clmEmpty = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // lvCosts
            // 
            this.lvCosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCosts.BorderColor = System.Drawing.Color.DarkGray;
            this.lvCosts.BuddyControl = null;
            this.lvCosts.Columns.Add(this.clmStore);
            this.lvCosts.Columns.Add(this.clmCost);
            this.lvCosts.Columns.Add(this.clmQuantity);
            this.lvCosts.Columns.Add(this.clmTotal);
            this.lvCosts.Columns.Add(this.clmDate);
            this.lvCosts.Columns.Add(this.clmReason);
            this.lvCosts.Columns.Add(this.clmUser);
            this.lvCosts.Columns.Add(this.clmEmpty);
            this.lvCosts.ContentBackColor = System.Drawing.Color.White;
            this.lvCosts.DefaultRowHeight = ((short)(22));
            this.lvCosts.DimSelectionWhenDisabled = true;
            this.lvCosts.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCosts.HeaderBackColor = System.Drawing.Color.White;
            this.lvCosts.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCosts.HeaderHeight = ((short)(25));
            this.lvCosts.HorizontalScrollbar = true;
            this.lvCosts.Location = new System.Drawing.Point(3, 69);
            this.lvCosts.Name = "lvCosts";
            this.lvCosts.OddRowColor = System.Drawing.Color.White;
            this.lvCosts.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCosts.SecondarySortColumn = ((short)(-1));
            this.lvCosts.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvCosts.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvCosts.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCosts.Size = new System.Drawing.Size(507, 292);
            this.lvCosts.SortSetting = "0:1";
            this.lvCosts.TabIndex = 1;
            this.lvCosts.VerticalScrollbarValue = 0;
            this.lvCosts.VerticalScrollbarYOffset = 0;
            this.lvCosts.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvCosts_HeaderClicked);
            this.lvCosts.SelectionChanged += new System.EventHandler(this.lvCosts_SelectionChanged);
            this.lvCosts.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvCosts_RowDoubleClick);
            // 
            // clmStore
            // 
            this.clmStore.AutoSize = true;
            this.clmStore.DefaultStyle = null;
            this.clmStore.HeaderText = "Store";
            this.clmStore.MaximumWidth = ((short)(0));
            this.clmStore.MinimumWidth = ((short)(200));
            this.clmStore.SecondarySortColumn = ((short)(-1));
            this.clmStore.Tag = null;
            this.clmStore.Width = ((short)(200));
            // 
            // clmCost
            // 
            this.clmCost.AutoSize = true;
            this.clmCost.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmCost.DefaultStyle = null;
            this.clmCost.HeaderText = "Cost";
            this.clmCost.MaximumWidth = ((short)(0));
            this.clmCost.MinimumWidth = ((short)(100));
            this.clmCost.SecondarySortColumn = ((short)(-1));
            this.clmCost.Tag = null;
            this.clmCost.Width = ((short)(100));
            // 
            // clmQuantity
            // 
            this.clmQuantity.AutoSize = true;
            this.clmQuantity.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmQuantity.DefaultStyle = null;
            this.clmQuantity.HeaderText = "Quantity";
            this.clmQuantity.MaximumWidth = ((short)(0));
            this.clmQuantity.MinimumWidth = ((short)(100));
            this.clmQuantity.SecondarySortColumn = ((short)(-1));
            this.clmQuantity.Sizable = false;
            this.clmQuantity.Tag = null;
            this.clmQuantity.Width = ((short)(100));
            // 
            // clmTotal
            // 
            this.clmTotal.AutoSize = true;
            this.clmTotal.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmTotal.DefaultStyle = null;
            this.clmTotal.HeaderText = "Value";
            this.clmTotal.MaximumWidth = ((short)(0));
            this.clmTotal.MinimumWidth = ((short)(100));
            this.clmTotal.SecondarySortColumn = ((short)(-1));
            this.clmTotal.Tag = null;
            this.clmTotal.Width = ((short)(100));
            // 
            // clmDate
            // 
            this.clmDate.AutoSize = true;
            this.clmDate.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmDate.DefaultStyle = null;
            this.clmDate.HeaderText = "Calculation date";
            this.clmDate.MaximumWidth = ((short)(0));
            this.clmDate.MinimumWidth = ((short)(150));
            this.clmDate.SecondarySortColumn = ((short)(-1));
            this.clmDate.Tag = null;
            this.clmDate.Width = ((short)(150));
            // 
            // clmReason
            // 
            this.clmReason.AutoSize = true;
            this.clmReason.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmReason.DefaultStyle = null;
            this.clmReason.HeaderText = "Reason for recalculation";
            this.clmReason.MaximumWidth = ((short)(0));
            this.clmReason.MinimumWidth = ((short)(150));
            this.clmReason.SecondarySortColumn = ((short)(-1));
            this.clmReason.Tag = null;
            this.clmReason.Width = ((short)(150));
            // 
            // clmUser
            // 
            this.clmUser.AutoSize = true;
            this.clmUser.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmUser.DefaultStyle = null;
            this.clmUser.HeaderText = "Recalculation by";
            this.clmUser.MaximumWidth = ((short)(0));
            this.clmUser.MinimumWidth = ((short)(150));
            this.clmUser.SecondarySortColumn = ((short)(-1));
            this.clmUser.Tag = null;
            this.clmUser.Width = ((short)(150));
            // 
            // searchBar
            // 
            this.searchBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.Location = new System.Drawing.Point(3, 3);
            this.searchBar.Name = "searchBar";
            this.searchBar.SearchOptionEnabled = true;
            this.searchBar.Size = new System.Drawing.Size(507, 60);
            this.searchBar.TabIndex = 0;
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
            // itemCostDataScroll
            // 
            this.itemCostDataScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemCostDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemCostDataScroll.DisplayNotFoundText = null;
            this.itemCostDataScroll.DisplayText = "Costs";
            this.itemCostDataScroll.Location = new System.Drawing.Point(3, 360);
            this.itemCostDataScroll.Name = "itemCostDataScroll";
            this.itemCostDataScroll.PageSize = 0;
            this.itemCostDataScroll.Size = new System.Drawing.Size(506, 30);
            this.itemCostDataScroll.TabIndex = 2;
            this.itemCostDataScroll.PageChanged += new System.EventHandler(this.itemCostDataScroll_PageChanged);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.Context = LSOne.Controls.ButtonType.Edit;
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(485, 394);
            this.btnEdit.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnEdit.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(24, 24);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // clmEmpty
            // 
            this.clmEmpty.AutoSize = true;
            this.clmEmpty.Clickable = false;
            this.clmEmpty.DefaultStyle = null;
            this.clmEmpty.FillRemainingWidth = true;
            this.clmEmpty.HeaderText = "";
            this.clmEmpty.MaximumWidth = ((short)(0));
            this.clmEmpty.MinimumWidth = ((short)(10));
            this.clmEmpty.SecondarySortColumn = ((short)(-1));
            this.clmEmpty.Tag = null;
            this.clmEmpty.Width = ((short)(50));
            // 
            // ItemCostPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.lvCosts);
            this.Controls.Add(this.itemCostDataScroll);
            this.Controls.Add(this.searchBar);
            this.DoubleBuffered = true;
            this.Name = "ItemCostPage";
            this.Size = new System.Drawing.Size(513, 421);
            this.SizeChanged += new System.EventHandler(this.ItemCostPage_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvCosts;
        private Controls.Columns.Column clmStore;
        private Controls.Columns.Column clmCost;
        private Controls.Columns.Column clmDate;
        private Controls.Columns.Column clmReason;
        private Controls.Columns.Column clmUser;
        private Controls.Columns.Column clmQuantity;
        private Controls.SearchBar searchBar;
        private Controls.DatabasePageDisplay itemCostDataScroll;
        private Controls.Columns.Column clmTotal;
        private Controls.ContextButton btnEdit;
        private Controls.Columns.Column clmEmpty;
    }
}
