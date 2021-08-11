using LSOne.Controls;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class ReceiveTransferOrderDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiveTransferOrderDialog));
            this.panel = new LSOne.Controls.TouchScrollButtonPanel();
            this.lvTransferOrders = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colFromStore = new LSOne.Controls.Columns.Column();
            this.colSentDate = new LSOne.Controls.Columns.Column();
            this.colDueDate = new LSOne.Controls.Columns.Column();
            this.lvItemsList = new LSOne.Controls.ListView();
            this.colItem = new LSOne.Controls.Columns.Column();
            this.colQuantity = new LSOne.Controls.Columns.Column();
            this.colUnit = new LSOne.Controls.Columns.Column();
            this.banner = new LSOne.Controls.TouchDialogBanner();
            this.pnlTransferInfo = new LSOne.Controls.DoubleBufferedPanel();
            this.lblDueDate = new LSOne.Controls.DoubleLabel();
            this.lblSentDate = new LSOne.Controls.DoubleLabel();
            this.lblFromStore = new LSOne.Controls.DoubleLabel();
            this.lblDescription = new LSOne.Controls.DoubleLabel();
            this.lblTransferID = new LSOne.Controls.DoubleLabel();
            this.upDownButton = new LSOne.Controls.UpDownButton();
            this.pnlTransferInfo.SuspendLayout();
            this.SuspendLayout();
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
            // lvTransferOrders
            // 
            resources.ApplyResources(this.lvTransferOrders, "lvTransferOrders");
            this.lvTransferOrders.ApplyVisualStyles = false;
            this.lvTransferOrders.BackColor = System.Drawing.Color.White;
            this.lvTransferOrders.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvTransferOrders.BuddyControl = null;
            this.lvTransferOrders.Columns.Add(this.colID);
            this.lvTransferOrders.Columns.Add(this.colDescription);
            this.lvTransferOrders.Columns.Add(this.colFromStore);
            this.lvTransferOrders.Columns.Add(this.colSentDate);
            this.lvTransferOrders.Columns.Add(this.colDueDate);
            this.lvTransferOrders.ContentBackColor = System.Drawing.Color.White;
            this.lvTransferOrders.DefaultRowHeight = ((short)(50));
            this.lvTransferOrders.EvenRowColor = System.Drawing.Color.White;
            this.lvTransferOrders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvTransferOrders.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvTransferOrders.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvTransferOrders.HeaderHeight = ((short)(30));
            this.lvTransferOrders.HideHorizontalScrollbarWhenDisabled = true;
            this.lvTransferOrders.HideVerticalScrollbarWhenDisabled = true;
            this.lvTransferOrders.Name = "lvTransferOrders";
            this.lvTransferOrders.OddRowColor = System.Drawing.Color.White;
            this.lvTransferOrders.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvTransferOrders.RowLines = true;
            this.lvTransferOrders.SecondarySortColumn = ((short)(-1));
            this.lvTransferOrders.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvTransferOrders.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvTransferOrders.SortSetting = "0:1";
            this.lvTransferOrders.TouchScroll = true;
            this.lvTransferOrders.UseFocusRectangle = false;
            this.lvTransferOrders.VerticalScrollbarValue = 0;
            this.lvTransferOrders.VerticalScrollbarYOffset = 0;
            this.lvTransferOrders.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvTransferOrders.SelectionChanged += new System.EventHandler(this.LvTransferOrders_SelectionChanged);
            this.lvTransferOrders.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvTransferOrders_RowDoubleClick);
            this.lvTransferOrders.Load += new System.EventHandler(this.LvTransferOrders_Load);
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
            resources.ApplyResources(this.colDescription, "colDescription");
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
            // colSentDate
            // 
            this.colSentDate.AutoSize = true;
            this.colSentDate.DefaultStyle = null;
            resources.ApplyResources(this.colSentDate, "colSentDate");
            this.colSentDate.InternalSort = true;
            this.colSentDate.MaximumWidth = ((short)(0));
            this.colSentDate.MinimumWidth = ((short)(10));
            this.colSentDate.RelativeSize = 18;
            this.colSentDate.SecondarySortColumn = ((short)(-1));
            this.colSentDate.Tag = null;
            this.colSentDate.Width = ((short)(50));
            // 
            // colDueDate
            // 
            this.colDueDate.AutoSize = true;
            this.colDueDate.DefaultStyle = null;
            resources.ApplyResources(this.colDueDate, "colDueDate");
            this.colDueDate.InternalSort = true;
            this.colDueDate.MaximumWidth = ((short)(0));
            this.colDueDate.MinimumWidth = ((short)(10));
            this.colDueDate.RelativeSize = 18;
            this.colDueDate.SecondarySortColumn = ((short)(-1));
            this.colDueDate.Tag = null;
            this.colDueDate.Width = ((short)(50));
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
            this.colItem.DefaultStyle = null;
            resources.ApplyResources(this.colItem, "colItem");
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
            this.colUnit.DefaultStyle = null;
            this.colUnit.HeaderText = global::LSOne.Services.Properties.Resources.Unit;
            this.colUnit.MaximumWidth = ((short)(0));
            this.colUnit.MinimumWidth = ((short)(10));
            this.colUnit.SecondarySortColumn = ((short)(-1));
            this.colUnit.Tag = null;
            this.colUnit.Width = ((short)(50));
            // 
            // banner
            // 
            resources.ApplyResources(this.banner, "banner");
            this.banner.BackColor = System.Drawing.Color.White;
            this.banner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.banner.Name = "banner";
            this.banner.TabStop = false;
            // 
            // pnlTransferInfo
            // 
            this.pnlTransferInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlTransferInfo.Controls.Add(this.lblDueDate);
            this.pnlTransferInfo.Controls.Add(this.lblSentDate);
            this.pnlTransferInfo.Controls.Add(this.lblFromStore);
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
            // lblSentDate
            // 
            this.lblSentDate.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblSentDate, "lblSentDate");
            this.lblSentDate.HeaderText = "Sent date";
            this.lblSentDate.Name = "lblSentDate";
            // 
            // lblFromStore
            // 
            resources.ApplyResources(this.lblFromStore, "lblFromStore");
            this.lblFromStore.BackColor = System.Drawing.Color.White;
            this.lblFromStore.HeaderText = "From store";
            this.lblFromStore.Name = "lblFromStore";
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
            // ReceiveTransferOrderDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.upDownButton);
            this.Controls.Add(this.pnlTransferInfo);
            this.Controls.Add(this.banner);
            this.Controls.Add(this.lvItemsList);
            this.Controls.Add(this.lvTransferOrders);
            this.Controls.Add(this.panel);
            this.Name = "ReceiveTransferOrderDialog";
            this.Load += new System.EventHandler(this.ReceiveTransferOrderDialog_Load);
            this.pnlTransferInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.ListView lvTransferOrders;
        private Controls.Columns.Column colID;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colFromStore;
        private Controls.Columns.Column colSentDate;
        private Controls.Columns.Column colDueDate;
        private ListView lvItemsList;
        private TouchDialogBanner banner;
        private Controls.Columns.Column colItem;
        private Controls.Columns.Column colQuantity;
        private Controls.Columns.Column colUnit;
        private DoubleBufferedPanel pnlTransferInfo;
        private DoubleLabel lblDueDate;
        private DoubleLabel lblSentDate;
        private DoubleLabel lblFromStore;
        private DoubleLabel lblDescription;
        private DoubleLabel lblTransferID;
        private UpDownButton upDownButton;
    }
}