using LSOne.Controls;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class ReceivedTransferRequestsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private TouchScrollButtonPanel panel;
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceivedTransferRequestsDialog));
            this.banner = new LSOne.Controls.TouchDialogBanner();
            this.lvItemsList = new LSOne.Controls.ListView();
            this.colItem = new LSOne.Controls.Columns.Column();
            this.colQuantity = new LSOne.Controls.Columns.Column();
            this.colUnit = new LSOne.Controls.Columns.Column();
            this.lvTransferRequests = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colFromStore = new LSOne.Controls.Columns.Column();
            this.colDate = new LSOne.Controls.Columns.Column();
            this.colDueDelivery = new LSOne.Controls.Columns.Column();
            this.panel = new LSOne.Controls.TouchScrollButtonPanel();
            this.pnlTransferInfo = new LSOne.Controls.DoubleBufferedPanel();
            this.lblDueDate = new LSOne.Controls.DoubleLabel();
            this.lblDate = new LSOne.Controls.DoubleLabel();
            this.lblStore = new LSOne.Controls.DoubleLabel();
            this.lblDescription = new LSOne.Controls.DoubleLabel();
            this.lblTransferID = new LSOne.Controls.DoubleLabel();
            this.upDownButton = new LSOne.Controls.UpDownButton();
            this.pnlTransferInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // banner
            // 
            resources.ApplyResources(this.banner, "banner");
            this.banner.BackColor = System.Drawing.Color.White;
            this.banner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.banner.Name = "banner";
            this.banner.TabStop = false;
            // 
            // lvItemsList
            // 
            resources.ApplyResources(this.lvItemsList, "lvItemsList");
            this.lvItemsList.ApplyVisualStyles = false;
            this.lvItemsList.BackColor = System.Drawing.Color.White;
            this.lvItemsList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvItemsList.BuddyControl = null;
            this.lvItemsList.Columns.Add(this.colItem);
            this.lvItemsList.Columns.Add(this.colQuantity);
            this.lvItemsList.Columns.Add(this.colUnit);
            this.lvItemsList.ContentBackColor = System.Drawing.Color.White;
            this.lvItemsList.DefaultRowHeight = ((short)(50));
            this.lvItemsList.EvenRowColor = System.Drawing.Color.White;
            this.lvItemsList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvItemsList.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvItemsList.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvItemsList.HeaderHeight = ((short)(30));
            this.lvItemsList.HideHorizontalScrollbarWhenDisabled = true;
            this.lvItemsList.HideVerticalScrollbarWhenDisabled = true;
            this.lvItemsList.Name = "lvItemsList";
            this.lvItemsList.OddRowColor = System.Drawing.Color.White;
            this.lvItemsList.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvItemsList.RowLines = true;
            this.lvItemsList.SecondarySortColumn = ((short)(-1));
            this.lvItemsList.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvItemsList.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItemsList.SortSetting = "0:1";
            this.lvItemsList.TouchScroll = true;
            this.lvItemsList.UseFocusRectangle = false;
            this.lvItemsList.VerticalScrollbarValue = 0;
            this.lvItemsList.VerticalScrollbarYOffset = 0;
            this.lvItemsList.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            // 
            // colItem
            // 
            this.colItem.AutoSize = true;
            this.colItem.Clickable = false;
            this.colItem.DefaultStyle = null;
            this.colItem.HeaderText = global::LSOne.Services.Properties.Resources.Item;
            this.colItem.MaximumWidth = ((short)(0));
            this.colItem.MinimumWidth = ((short)(10));
            this.colItem.RelativeSize = 60;
            this.colItem.SecondarySortColumn = ((short)(-1));
            this.colItem.Tag = null;
            this.colItem.Width = ((short)(50));
            // 
            // colQuantity
            // 
            this.colQuantity.AutoSize = true;
            this.colQuantity.Clickable = false;
            this.colQuantity.DefaultStyle = null;
            this.colQuantity.HeaderText = global::LSOne.Services.Properties.Resources.Qty;
            this.colQuantity.MaximumWidth = ((short)(0));
            this.colQuantity.MinimumWidth = ((short)(10));
            this.colQuantity.SecondarySortColumn = ((short)(-1));
            this.colQuantity.Tag = null;
            this.colQuantity.Width = ((short)(50));
            // 
            // colUnit
            // 
            this.colUnit.AutoSize = true;
            this.colUnit.Clickable = false;
            this.colUnit.DefaultStyle = null;
            this.colUnit.HeaderText = global::LSOne.Services.Properties.Resources.Unit;
            this.colUnit.MaximumWidth = ((short)(0));
            this.colUnit.MinimumWidth = ((short)(10));
            this.colUnit.SecondarySortColumn = ((short)(-1));
            this.colUnit.Tag = null;
            this.colUnit.Width = ((short)(50));
            // 
            // lvTransferRequests
            // 
            resources.ApplyResources(this.lvTransferRequests, "lvTransferRequests");
            this.lvTransferRequests.ApplyVisualStyles = false;
            this.lvTransferRequests.BackColor = System.Drawing.Color.White;
            this.lvTransferRequests.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvTransferRequests.BuddyControl = null;
            this.lvTransferRequests.Columns.Add(this.colID);
            this.lvTransferRequests.Columns.Add(this.colDescription);
            this.lvTransferRequests.Columns.Add(this.colFromStore);
            this.lvTransferRequests.Columns.Add(this.colDate);
            this.lvTransferRequests.Columns.Add(this.colDueDelivery);
            this.lvTransferRequests.ContentBackColor = System.Drawing.Color.White;
            this.lvTransferRequests.DefaultRowHeight = ((short)(50));
            this.lvTransferRequests.EvenRowColor = System.Drawing.Color.White;
            this.lvTransferRequests.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvTransferRequests.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvTransferRequests.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvTransferRequests.HeaderHeight = ((short)(30));
            this.lvTransferRequests.HideHorizontalScrollbarWhenDisabled = true;
            this.lvTransferRequests.HideVerticalScrollbarWhenDisabled = true;
            this.lvTransferRequests.Name = "lvTransferRequests";
            this.lvTransferRequests.OddRowColor = System.Drawing.Color.White;
            this.lvTransferRequests.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvTransferRequests.RowLines = true;
            this.lvTransferRequests.SecondarySortColumn = ((short)(-1));
            this.lvTransferRequests.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvTransferRequests.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvTransferRequests.SortSetting = "0:1";
            this.lvTransferRequests.TouchScroll = true;
            this.lvTransferRequests.UseFocusRectangle = false;
            this.lvTransferRequests.VerticalScrollbarValue = 0;
            this.lvTransferRequests.VerticalScrollbarYOffset = 0;
            this.lvTransferRequests.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvTransferRequests.SelectionChanged += new System.EventHandler(this.LvTransferRequests_SelectionChanged);
            this.lvTransferRequests.Load += new System.EventHandler(this.LvTransferRequests_Load);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.InternalSort = true;
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.RelativeSize = 18;
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(50));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            this.colDescription.HeaderText = global::LSOne.Services.Properties.Resources.Description;
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.RelativeSize = 23;
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colFromStore
            // 
            this.colFromStore.AutoSize = true;
            this.colFromStore.DefaultStyle = null;
            resources.ApplyResources(this.colFromStore, "colFromStore");
            this.colFromStore.InternalSort = true;
            this.colFromStore.MaximumWidth = ((short)(0));
            this.colFromStore.MinimumWidth = ((short)(10));
            this.colFromStore.RelativeSize = 23;
            this.colFromStore.SecondarySortColumn = ((short)(-1));
            this.colFromStore.Tag = null;
            this.colFromStore.Width = ((short)(50));
            // 
            // colDate
            // 
            this.colDate.AutoSize = true;
            this.colDate.DefaultStyle = null;
            resources.ApplyResources(this.colDate, "colDate");
            this.colDate.InternalSort = true;
            this.colDate.MaximumWidth = ((short)(0));
            this.colDate.MinimumWidth = ((short)(10));
            this.colDate.RelativeSize = 18;
            this.colDate.SecondarySortColumn = ((short)(-1));
            this.colDate.Tag = null;
            this.colDate.Width = ((short)(50));
            // 
            // colDueDelivery
            // 
            this.colDueDelivery.AutoSize = true;
            this.colDueDelivery.DefaultStyle = null;
            resources.ApplyResources(this.colDueDelivery, "colDueDelivery");
            this.colDueDelivery.InternalSort = true;
            this.colDueDelivery.MaximumWidth = ((short)(0));
            this.colDueDelivery.MinimumWidth = ((short)(10));
            this.colDueDelivery.RelativeSize = 18;
            this.colDueDelivery.SecondarySortColumn = ((short)(-1));
            this.colDueDelivery.Tag = null;
            this.colDueDelivery.Width = ((short)(50));
            // 
            // panel
            // 
            resources.ApplyResources(this.panel, "panel");
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.ButtonHeight = 50;
            this.panel.HorizontalMaxButtonWidth = 150;
            this.panel.HorizontalMinButtonWidth = 150;
            this.panel.Name = "panel";
            this.panel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panel_Click);
            // 
            // pnlTransferInfo
            // 
            this.pnlTransferInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlTransferInfo.Controls.Add(this.lblDueDate);
            this.pnlTransferInfo.Controls.Add(this.lblDate);
            this.pnlTransferInfo.Controls.Add(this.lblStore);
            this.pnlTransferInfo.Controls.Add(this.lblDescription);
            this.pnlTransferInfo.Controls.Add(this.lblTransferID);
            resources.ApplyResources(this.pnlTransferInfo, "pnlTransferInfo");
            this.pnlTransferInfo.Name = "pnlTransferInfo";
            this.pnlTransferInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTransferInfo_Paint);
            // 
            // lblDueDate
            // 
            this.lblDueDate.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblDueDate, "lblDueDate");
            this.lblDueDate.HeaderText = "Due date";
            this.lblDueDate.Name = "lblDueDate";
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblDate, "lblDate");
            this.lblDate.HeaderText = "Date";
            this.lblDate.Name = "lblDate";
            // 
            // lblStore
            // 
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.BackColor = System.Drawing.Color.White;
            this.lblStore.HeaderText = "Requesting store";
            this.lblStore.Name = "lblStore";
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.BackColor = System.Drawing.Color.White;
            this.lblDescription.HeaderText = "Description";
            this.lblDescription.Name = "lblDescription";
            // 
            // lblTransferID
            // 
            resources.ApplyResources(this.lblTransferID, "lblTransferID");
            this.lblTransferID.BackColor = System.Drawing.Color.White;
            this.lblTransferID.HeaderText = "ID";
            this.lblTransferID.Name = "lblTransferID";
            // 
            // upDownButton
            // 
            resources.ApplyResources(this.upDownButton, "upDownButton");
            this.upDownButton.Name = "upDownButton";
            this.upDownButton.DownButtonClick += new System.EventHandler<System.Windows.Forms.MouseEventArgs>(this.upDownButton_DownButtonClick);
            this.upDownButton.UpButtonClick += new System.EventHandler<System.Windows.Forms.MouseEventArgs>(this.upDownButton_UpButtonClick);
            // 
            // ReceivedTransferRequestsDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.upDownButton);
            this.Controls.Add(this.pnlTransferInfo);
            this.Controls.Add(this.banner);
            this.Controls.Add(this.lvItemsList);
            this.Controls.Add(this.lvTransferRequests);
            this.Controls.Add(this.panel);
            this.Name = "ReceivedTransferRequestsDialog";
            this.Load += new System.EventHandler(this.ReceivedTransferRequestsDialog_Load);
            this.pnlTransferInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.ListView lvTransferRequests;
        private Controls.Columns.Column colID;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colFromStore;
        private Controls.Columns.Column colDueDelivery;
        private Controls.Columns.Column colDate;
        private ListView lvItemsList;
        private TouchDialogBanner banner;
        private Controls.Columns.Column colItem;
        private Controls.Columns.Column colQuantity;
        private Controls.Columns.Column colUnit;
        private DoubleBufferedPanel pnlTransferInfo;
        private DoubleLabel lblTransferID;
        private DoubleLabel lblDescription;
        private DoubleLabel lblStore;
        private DoubleLabel lblDate;
        private DoubleLabel lblDueDate;
        private UpDownButton upDownButton;
    }
}