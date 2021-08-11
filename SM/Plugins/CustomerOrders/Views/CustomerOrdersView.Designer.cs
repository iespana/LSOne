

namespace LSOne.ViewPlugins.CustomerOrders.Views
{
    partial class CustomerOrdersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerOrdersView));
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.lvCustomerOrders = new LSOne.Controls.ListView();
            this.colReference = new LSOne.Controls.Columns.Column();
            this.colCustomer = new LSOne.Controls.Columns.Column();
            this.colDeliveryLocation = new LSOne.Controls.Columns.Column();
            this.colSource = new LSOne.Controls.Columns.Column();
            this.colDelivery = new LSOne.Controls.Columns.Column();
            this.colExpires = new LSOne.Controls.Columns.Column();
            this.colComment = new LSOne.Controls.Columns.Column();
            this.colStatus = new LSOne.Controls.Columns.Column();
            this.colCreatedAt = new LSOne.Controls.Columns.Column();
            this.colCreatedDate = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lblMsg = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblMsg);
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.lvCustomerOrders);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
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
            // 
            // lvCustomerOrders
            // 
            resources.ApplyResources(this.lvCustomerOrders, "lvCustomerOrders");
            this.lvCustomerOrders.BuddyControl = null;
            this.lvCustomerOrders.Columns.Add(this.colReference);
            this.lvCustomerOrders.Columns.Add(this.colCustomer);
            this.lvCustomerOrders.Columns.Add(this.colDeliveryLocation);
            this.lvCustomerOrders.Columns.Add(this.colSource);
            this.lvCustomerOrders.Columns.Add(this.colDelivery);
            this.lvCustomerOrders.Columns.Add(this.colExpires);
            this.lvCustomerOrders.Columns.Add(this.colComment);
            this.lvCustomerOrders.Columns.Add(this.colStatus);
            this.lvCustomerOrders.Columns.Add(this.colCreatedAt);
            this.lvCustomerOrders.Columns.Add(this.colCreatedDate);
            this.lvCustomerOrders.ContentBackColor = System.Drawing.Color.White;
            this.lvCustomerOrders.DefaultRowHeight = ((short)(22));
            this.lvCustomerOrders.DimSelectionWhenDisabled = true;
            this.lvCustomerOrders.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCustomerOrders.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCustomerOrders.HeaderHeight = ((short)(25));
            this.lvCustomerOrders.HorizontalScrollbar = true;
            this.lvCustomerOrders.Name = "lvCustomerOrders";
            this.lvCustomerOrders.OddRowColor = System.Drawing.Color.White;
            this.lvCustomerOrders.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCustomerOrders.SecondarySortColumn = ((short)(-1));
            this.lvCustomerOrders.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvCustomerOrders.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCustomerOrders.SortSetting = "0:1";
            this.lvCustomerOrders.SelectionChanged += new System.EventHandler(this.lvCustomerOrders_SelectionChanged);
            this.lvCustomerOrders.DoubleClick += new System.EventHandler(this.lvCustomerOrders_DoubleClick);
            // 
            // colReference
            // 
            this.colReference.AutoSize = true;
            this.colReference.DefaultStyle = null;
            resources.ApplyResources(this.colReference, "colReference");
            this.colReference.InternalSort = true;
            this.colReference.MaximumWidth = ((short)(0));
            this.colReference.MinimumWidth = ((short)(40));
            this.colReference.SecondarySortColumn = ((short)(-1));
            this.colReference.Tag = null;
            this.colReference.Width = ((short)(100));
            // 
            // colCustomer
            // 
            this.colCustomer.AutoSize = true;
            this.colCustomer.DefaultStyle = null;
            resources.ApplyResources(this.colCustomer, "colCustomer");
            this.colCustomer.InternalSort = true;
            this.colCustomer.MaximumWidth = ((short)(0));
            this.colCustomer.MinimumWidth = ((short)(50));
            this.colCustomer.SecondarySortColumn = ((short)(-1));
            this.colCustomer.Tag = null;
            this.colCustomer.Width = ((short)(200));
            // 
            // colDeliveryLocation
            // 
            this.colDeliveryLocation.AutoSize = true;
            this.colDeliveryLocation.DefaultStyle = null;
            resources.ApplyResources(this.colDeliveryLocation, "colDeliveryLocation");
            this.colDeliveryLocation.InternalSort = true;
            this.colDeliveryLocation.MaximumWidth = ((short)(0));
            this.colDeliveryLocation.MinimumWidth = ((short)(100));
            this.colDeliveryLocation.SecondarySortColumn = ((short)(-1));
            this.colDeliveryLocation.Tag = null;
            this.colDeliveryLocation.Width = ((short)(200));
            // 
            // colSource
            // 
            this.colSource.AutoSize = true;
            this.colSource.DefaultStyle = null;
            resources.ApplyResources(this.colSource, "colSource");
            this.colSource.InternalSort = true;
            this.colSource.MaximumWidth = ((short)(0));
            this.colSource.MinimumWidth = ((short)(20));
            this.colSource.SecondarySortColumn = ((short)(-1));
            this.colSource.Tag = null;
            this.colSource.Width = ((short)(100));
            // 
            // colDelivery
            // 
            this.colDelivery.AutoSize = true;
            this.colDelivery.DefaultStyle = null;
            resources.ApplyResources(this.colDelivery, "colDelivery");
            this.colDelivery.InternalSort = true;
            this.colDelivery.MaximumWidth = ((short)(0));
            this.colDelivery.MinimumWidth = ((short)(20));
            this.colDelivery.SecondarySortColumn = ((short)(-1));
            this.colDelivery.Tag = null;
            this.colDelivery.Width = ((short)(100));
            // 
            // colExpires
            // 
            this.colExpires.AutoSize = true;
            this.colExpires.DefaultStyle = null;
            resources.ApplyResources(this.colExpires, "colExpires");
            this.colExpires.InternalSort = true;
            this.colExpires.MaximumWidth = ((short)(0));
            this.colExpires.MinimumWidth = ((short)(25));
            this.colExpires.SecondarySortColumn = ((short)(-1));
            this.colExpires.Tag = null;
            this.colExpires.Width = ((short)(100));
            // 
            // colComment
            // 
            this.colComment.AutoSize = true;
            this.colComment.DefaultStyle = null;
            resources.ApplyResources(this.colComment, "colComment");
            this.colComment.InternalSort = true;
            this.colComment.MaximumWidth = ((short)(0));
            this.colComment.MinimumWidth = ((short)(100));
            this.colComment.SecondarySortColumn = ((short)(-1));
            this.colComment.Tag = null;
            this.colComment.Width = ((short)(150));
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
            // colCreatedAt
            // 
            this.colCreatedAt.AutoSize = true;
            this.colCreatedAt.DefaultStyle = null;
            resources.ApplyResources(this.colCreatedAt, "colCreatedAt");
            this.colCreatedAt.InternalSort = true;
            this.colCreatedAt.MaximumWidth = ((short)(0));
            this.colCreatedAt.MinimumWidth = ((short)(20));
            this.colCreatedAt.SecondarySortColumn = ((short)(-1));
            this.colCreatedAt.Tag = null;
            this.colCreatedAt.Width = ((short)(100));
            // 
            // colCreatedDate
            // 
            this.colCreatedDate.AutoSize = true;
            this.colCreatedDate.DefaultStyle = null;
            resources.ApplyResources(this.colCreatedDate, "colCreatedDate");
            this.colCreatedDate.InternalSort = true;
            this.colCreatedDate.MaximumWidth = ((short)(0));
            this.colCreatedDate.MinimumWidth = ((short)(30));
            this.colCreatedDate.SecondarySortColumn = ((short)(-1));
            this.colCreatedDate.Tag = null;
            this.colCreatedDate.Width = ((short)(100));
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
            // lblMsg
            // 
            resources.ApplyResources(this.lblMsg, "lblMsg");
            this.lblMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Name = "lblMsg";
            // 
            // CustomerOrdersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CustomerOrdersView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SearchBar searchBar1;
        private Controls.ListView lvCustomerOrders;
        private Controls.ContextButtons btnsEditAddRemove;
        private Controls.Columns.Column colReference;
        private Controls.Columns.Column colDeliveryLocation;
        private Controls.Columns.Column colSource;
        private Controls.Columns.Column colDelivery;
        private Controls.Columns.Column colExpires;
        private Controls.Columns.Column colComment;
        private Controls.Columns.Column colCreatedAt;
        private Controls.Columns.Column colCreatedDate;
        private System.Windows.Forms.Label lblMsg;
        private Controls.Columns.Column colCustomer;
        private Controls.Columns.Column colStatus;
    }
}
