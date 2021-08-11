using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Views
{
    partial class GoodsReceivingDocumentsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoodsReceivingDocumentsView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.lvItems = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colVendor = new LSOne.Controls.Columns.Column();
            this.colStore = new LSOne.Controls.Columns.Column();
            this.colStatus = new LSOne.Controls.Columns.Column();
            this.colOrdered = new LSOne.Controls.Columns.Column();
            this.colReceived = new LSOne.Controls.Columns.Column();
            this.colCreated = new LSOne.Controls.Columns.Column();
            this.colPosted = new LSOne.Controls.Columns.Column();
            this.lblMsg = new System.Windows.Forms.Label();
            this.colStatusImage = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblMsg);
            this.pnlBottom.Controls.Add(this.lvItems);
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
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
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
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
            this.searchBar1.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar_UnknownControlHasSelection);
            this.searchBar1.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar_UnknownControlGetSelection);
            this.searchBar1.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar_UnknownControlSetSelection);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.colStatusImage);
            this.lvItems.Columns.Add(this.colID);
            this.lvItems.Columns.Add(this.colDescription);
            this.lvItems.Columns.Add(this.colVendor);
            this.lvItems.Columns.Add(this.colStore);
            this.lvItems.Columns.Add(this.colStatus);
            this.lvItems.Columns.Add(this.colOrdered);
            this.lvItems.Columns.Add(this.colReceived);
            this.lvItems.Columns.Add(this.colCreated);
            this.lvItems.Columns.Add(this.colPosted);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.HorizontalScrollbar = true;
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "1:1";
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            this.lvItems.DoubleClick += new System.EventHandler(this.lvItems_DoubleClick);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.InternalSort = true;
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(50));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            this.colDescription.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.NoTextWhenSmall = true;
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colVendor
            // 
            this.colVendor.AutoSize = true;
            this.colVendor.DefaultStyle = null;
            resources.ApplyResources(this.colVendor, "colVendor");
            this.colVendor.InternalSort = true;
            this.colVendor.MaximumWidth = ((short)(0));
            this.colVendor.MinimumWidth = ((short)(10));
            this.colVendor.SecondarySortColumn = ((short)(-1));
            this.colVendor.Tag = null;
            this.colVendor.Width = ((short)(50));
            // 
            // colStore
            // 
            this.colStore.AutoSize = true;
            this.colStore.DefaultStyle = null;
            resources.ApplyResources(this.colStore, "colStore");
            this.colStore.InternalSort = true;
            this.colStore.MaximumWidth = ((short)(0));
            this.colStore.MinimumWidth = ((short)(10));
            this.colStore.SecondarySortColumn = ((short)(-1));
            this.colStore.Tag = null;
            this.colStore.Width = ((short)(50));
            // 
            // colStatus
            // 
            this.colStatus.AutoSize = true;
            this.colStatus.DefaultStyle = null;
            resources.ApplyResources(this.colStatus, "colStatus");
            this.colStatus.InternalSort = true;
            this.colStatus.MaximumWidth = ((short)(0));
            this.colStatus.MinimumWidth = ((short)(10));
            this.colStatus.SecondarySortColumn = ((short)(-1));
            this.colStatus.Tag = null;
            this.colStatus.Width = ((short)(50));
            // 
            // colOrdered
            // 
            this.colOrdered.AutoSize = true;
            this.colOrdered.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colOrdered.DefaultStyle = null;
            resources.ApplyResources(this.colOrdered, "colOrdered");
            this.colOrdered.InternalSort = true;
            this.colOrdered.MaximumWidth = ((short)(0));
            this.colOrdered.MinimumWidth = ((short)(10));
            this.colOrdered.SecondarySortColumn = ((short)(-1));
            this.colOrdered.Tag = null;
            this.colOrdered.Width = ((short)(50));
            // 
            // colReceived
            // 
            this.colReceived.AutoSize = true;
            this.colReceived.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colReceived.DefaultStyle = null;
            resources.ApplyResources(this.colReceived, "colReceived");
            this.colReceived.InternalSort = true;
            this.colReceived.MaximumWidth = ((short)(0));
            this.colReceived.MinimumWidth = ((short)(10));
            this.colReceived.SecondarySortColumn = ((short)(-1));
            this.colReceived.Tag = null;
            this.colReceived.Width = ((short)(50));
            // 
            // colCreated
            // 
            this.colCreated.AutoSize = true;
            this.colCreated.DefaultStyle = null;
            resources.ApplyResources(this.colCreated, "colCreated");
            this.colCreated.InternalSort = true;
            this.colCreated.MaximumWidth = ((short)(0));
            this.colCreated.MinimumWidth = ((short)(10));
            this.colCreated.SecondarySortColumn = ((short)(-1));
            this.colCreated.Tag = null;
            this.colCreated.Width = ((short)(50));
            // 
            // colPosted
            // 
            this.colPosted.AutoSize = true;
            this.colPosted.DefaultStyle = null;
            resources.ApplyResources(this.colPosted, "colPosted");
            this.colPosted.InternalSort = true;
            this.colPosted.MaximumWidth = ((short)(0));
            this.colPosted.MinimumWidth = ((short)(10));
            this.colPosted.SecondarySortColumn = ((short)(-1));
            this.colPosted.Tag = null;
            this.colPosted.Width = ((short)(50));
            // 
            // lblMsg
            // 
            resources.ApplyResources(this.lblMsg, "lblMsg");
            this.lblMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Name = "lblMsg";
            // 
            // colStatusImage
            // 
            this.colStatusImage.AutoSize = true;
            this.colStatusImage.Clickable = false;
            this.colStatusImage.DefaultStyle = null;
            resources.ApplyResources(this.colStatusImage, "colStatusImage");
            this.colStatusImage.MaximumWidth = ((short)(0));
            this.colStatusImage.MinimumWidth = ((short)(10));
            this.colStatusImage.SecondarySortColumn = ((short)(-1));
            this.colStatusImage.Tag = null;
            this.colStatusImage.Width = ((short)(50));
            // 
            // GoodsReceivingDocumentsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "GoodsReceivingDocumentsView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsEditAddRemove;
        private SearchBar searchBar1;
        private ListView lvItems;
        private Controls.Columns.Column colID;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colVendor;
        private Controls.Columns.Column colStore;
        private Controls.Columns.Column colStatus;
        private Controls.Columns.Column colOrdered;
        private Controls.Columns.Column colReceived;
        private Controls.Columns.Column colCreated;
        private Controls.Columns.Column colPosted;
        private System.Windows.Forms.Label lblMsg;
        private Controls.Columns.Column colStatusImage;
    }
}
