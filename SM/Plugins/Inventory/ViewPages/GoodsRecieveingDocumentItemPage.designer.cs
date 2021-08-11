namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class GoodsRecieveingDocumentItemPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoodsRecieveingDocumentItemPage));
            this.btnPostAllLines = new System.Windows.Forms.Button();
            this.btnPostLine = new System.Windows.Forms.Button();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.goodsReceivingDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.lvItems = new LSOne.Controls.ListView();
            this.colItemID = new LSOne.Controls.Columns.Column();
            this.colItemName = new LSOne.Controls.Columns.Column();
            this.colItemVariant = new LSOne.Controls.Columns.Column();
            this.colRecievedQuantity = new LSOne.Controls.Columns.Column();
            this.colRecievedDate = new LSOne.Controls.Columns.Column();
            this.colOrderedQuantity = new LSOne.Controls.Columns.Column();
            this.colPosted = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // btnPostAllLines
            // 
            resources.ApplyResources(this.btnPostAllLines, "btnPostAllLines");
            this.btnPostAllLines.Name = "btnPostAllLines";
            this.btnPostAllLines.UseVisualStyleBackColor = true;
            this.btnPostAllLines.Click += new System.EventHandler(this.btnPostAllLines_Click);
            // 
            // btnPostLine
            // 
            resources.ApplyResources(this.btnPostLine, "btnPostLine");
            this.btnPostLine.Name = "btnPostLine";
            this.btnPostLine.UseVisualStyleBackColor = true;
            this.btnPostLine.Click += new System.EventHandler(this.btnPostLine_Click);
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
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // goodsReceivingDataScroll
            // 
            resources.ApplyResources(this.goodsReceivingDataScroll, "goodsReceivingDataScroll");
            this.goodsReceivingDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.goodsReceivingDataScroll.Name = "goodsReceivingDataScroll";
            this.goodsReceivingDataScroll.PageSize = 0;
            this.goodsReceivingDataScroll.PageChanged += new System.EventHandler(this.stocCountDataScroll_PageChanged);
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
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.colItemID);
            this.lvItems.Columns.Add(this.colItemName);
            this.lvItems.Columns.Add(this.colItemVariant);
            this.lvItems.Columns.Add(this.colRecievedQuantity);
            this.lvItems.Columns.Add(this.colRecievedDate);
            this.lvItems.Columns.Add(this.colOrderedQuantity);
            this.lvItems.Columns.Add(this.colPosted);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "0:1";
            this.lvItems.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvItems_HeaderClicked);
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            // 
            // colItemID
            // 
            this.colItemID.AutoSize = true;
            this.colItemID.DefaultStyle = null;
            resources.ApplyResources(this.colItemID, "colItemID");
            this.colItemID.MaximumWidth = ((short)(0));
            this.colItemID.MinimumWidth = ((short)(10));
            this.colItemID.SecondarySortColumn = ((short)(-1));
            this.colItemID.Tag = null;
            this.colItemID.Width = ((short)(50));
            // 
            // colItemName
            // 
            this.colItemName.AutoSize = true;
            this.colItemName.DefaultStyle = null;
            resources.ApplyResources(this.colItemName, "colItemName");
            this.colItemName.MaximumWidth = ((short)(0));
            this.colItemName.MinimumWidth = ((short)(10));
            this.colItemName.SecondarySortColumn = ((short)(-1));
            this.colItemName.Tag = null;
            this.colItemName.Width = ((short)(50));
            // 
            // colItemVariant
            // 
            this.colItemVariant.AutoSize = true;
            this.colItemVariant.DefaultStyle = null;
            this.colItemVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colItemVariant, "colItemVariant");
            this.colItemVariant.MaximumWidth = ((short)(0));
            this.colItemVariant.MinimumWidth = ((short)(10));
            this.colItemVariant.NoTextWhenSmall = true;
            this.colItemVariant.SecondarySortColumn = ((short)(-1));
            this.colItemVariant.Tag = null;
            this.colItemVariant.Width = ((short)(50));
            // 
            // colRecievedQuantity
            // 
            this.colRecievedQuantity.AutoSize = true;
            this.colRecievedQuantity.DefaultStyle = null;
            resources.ApplyResources(this.colRecievedQuantity, "colRecievedQuantity");
            this.colRecievedQuantity.MaximumWidth = ((short)(0));
            this.colRecievedQuantity.MinimumWidth = ((short)(10));
            this.colRecievedQuantity.SecondarySortColumn = ((short)(-1));
            this.colRecievedQuantity.Tag = null;
            this.colRecievedQuantity.Width = ((short)(50));
            // 
            // colRecievedDate
            // 
            this.colRecievedDate.AutoSize = true;
            this.colRecievedDate.DefaultStyle = null;
            resources.ApplyResources(this.colRecievedDate, "colRecievedDate");
            this.colRecievedDate.MaximumWidth = ((short)(0));
            this.colRecievedDate.MinimumWidth = ((short)(10));
            this.colRecievedDate.SecondarySortColumn = ((short)(-1));
            this.colRecievedDate.Tag = null;
            this.colRecievedDate.Width = ((short)(50));
            // 
            // colOrderedQuantity
            // 
            this.colOrderedQuantity.AutoSize = true;
            this.colOrderedQuantity.DefaultStyle = null;
            resources.ApplyResources(this.colOrderedQuantity, "colOrderedQuantity");
            this.colOrderedQuantity.MaximumWidth = ((short)(0));
            this.colOrderedQuantity.MinimumWidth = ((short)(10));
            this.colOrderedQuantity.SecondarySortColumn = ((short)(-1));
            this.colOrderedQuantity.Tag = null;
            this.colOrderedQuantity.Width = ((short)(50));
            // 
            // colPosted
            // 
            this.colPosted.AutoSize = true;
            this.colPosted.DefaultStyle = null;
            resources.ApplyResources(this.colPosted, "colPosted");
            this.colPosted.MaximumWidth = ((short)(0));
            this.colPosted.MinimumWidth = ((short)(10));
            this.colPosted.SecondarySortColumn = ((short)(-1));
            this.colPosted.Tag = null;
            this.colPosted.Width = ((short)(50));
            // 
            // GoodsRecieveingDocumentItemPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.searchBar1);
            this.Controls.Add(this.goodsReceivingDataScroll);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnPostAllLines);
            this.Controls.Add(this.btnPostLine);
            this.Controls.Add(this.btnsEditAddRemove);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "GoodsRecieveingDocumentItemPage";
            this.ResumeLayout(false);

        }


        #endregion

        private Controls.ListView lvItems;
        private System.Windows.Forms.Button btnPostAllLines;
        private System.Windows.Forms.Button btnPostLine;
        private Controls.ContextButtons btnsEditAddRemove;
        private Controls.Columns.Column colItemID;
        private Controls.Columns.Column colItemName;
        private Controls.Columns.Column colItemVariant;
        private Controls.Columns.Column colRecievedQuantity;
        private Controls.Columns.Column colRecievedDate;
        private Controls.Columns.Column colOrderedQuantity;
        private Controls.Columns.Column colPosted;
        private Controls.DatabasePageDisplay goodsReceivingDataScroll;
        private Controls.SearchBar searchBar1;
    }
}
