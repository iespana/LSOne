using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class TransferItemsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferItemsPanel));
            this.tnpNumpad = new LSOne.Controls.TouchNumPad();
            this.pnlItemInfo = new LSOne.Controls.DoubleBufferedPanel();
            this.tlpInventory = new LSOne.Controls.DoubleBufferedTableLayoutPanel();
            this.lblFromStoreParked = new System.Windows.Forms.Label();
            this.lblMyStore = new System.Windows.Forms.Label();
            this.lblFromStore = new System.Windows.Forms.Label();
            this.lblFromStoreReserved = new System.Windows.Forms.Label();
            this.lblInventOnHand = new System.Windows.Forms.Label();
            this.lblMyStoreParked = new System.Windows.Forms.Label();
            this.lblMyStoreInventOnHand = new System.Windows.Forms.Label();
            this.lblFromStoreOrdered = new System.Windows.Forms.Label();
            this.lblFromStoreInventOnHnad = new System.Windows.Forms.Label();
            this.lblMyStoreReserved = new System.Windows.Forms.Label();
            this.lblMyStoreOrdered = new System.Windows.Forms.Label();
            this.lblParked = new System.Windows.Forms.Label();
            this.lblOrdered = new System.Windows.Forms.Label();
            this.lblReserved = new System.Windows.Forms.Label();
            this.pbItemImage = new System.Windows.Forms.PictureBox();
            this.pnlItems = new LSOne.Controls.DoubleBufferedPanel();
            this.btnItemInfo = new LSOne.Controls.TouchButton();
            this.ntbBarCode = new LSOne.Controls.ShadeNumericTextBox();
            this.lvItems = new LSOne.Controls.ListView();
            this.clmItemID = new LSOne.Controls.Columns.Column();
            this.clmItemName = new LSOne.Controls.Columns.Column();
            this.clmVariant = new LSOne.Controls.Columns.Column();
            this.clmQty = new LSOne.Controls.Columns.Column();
            this.FocusTimer = new System.Windows.Forms.Timer(this.components);
            this.btnPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.pnlItemInfo.SuspendLayout();
            this.tlpInventory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbItemImage)).BeginInit();
            this.pnlItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // tnpNumpad
            // 
            this.tnpNumpad.BackColor = System.Drawing.Color.Transparent;
            this.tnpNumpad.KeystrokeMode = true;
            resources.ApplyResources(this.tnpNumpad, "tnpNumpad");
            this.tnpNumpad.MultiplyButtonIsZeroZero = true;
            this.tnpNumpad.Name = "tnpNumpad";
            this.tnpNumpad.TabStop = false;
            this.tnpNumpad.EnterPressed += new System.EventHandler(this.tnpNumpad_EnterPressed);
            this.tnpNumpad.ClearPressed += new System.EventHandler(this.tnpNumpad_ClearPressed);
            // 
            // pnlItemInfo
            // 
            resources.ApplyResources(this.pnlItemInfo, "pnlItemInfo");
            this.pnlItemInfo.Controls.Add(this.tlpInventory);
            this.pnlItemInfo.Controls.Add(this.pbItemImage);
            this.pnlItemInfo.Controls.Add(this.tnpNumpad);
            this.pnlItemInfo.Name = "pnlItemInfo";
            // 
            // tlpInventory
            // 
            resources.ApplyResources(this.tlpInventory, "tlpInventory");
            this.tlpInventory.Controls.Add(this.lblFromStoreParked, 2, 4);
            this.tlpInventory.Controls.Add(this.lblMyStore, 0, 0);
            this.tlpInventory.Controls.Add(this.lblFromStore, 2, 0);
            this.tlpInventory.Controls.Add(this.lblFromStoreReserved, 2, 3);
            this.tlpInventory.Controls.Add(this.lblInventOnHand, 1, 1);
            this.tlpInventory.Controls.Add(this.lblMyStoreParked, 0, 4);
            this.tlpInventory.Controls.Add(this.lblMyStoreInventOnHand, 0, 1);
            this.tlpInventory.Controls.Add(this.lblFromStoreOrdered, 2, 2);
            this.tlpInventory.Controls.Add(this.lblFromStoreInventOnHnad, 2, 1);
            this.tlpInventory.Controls.Add(this.lblMyStoreReserved, 0, 3);
            this.tlpInventory.Controls.Add(this.lblMyStoreOrdered, 0, 2);
            this.tlpInventory.Controls.Add(this.lblParked, 1, 4);
            this.tlpInventory.Controls.Add(this.lblOrdered, 1, 2);
            this.tlpInventory.Controls.Add(this.lblReserved, 1, 3);
            this.tlpInventory.Name = "tlpInventory";
            this.tlpInventory.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.tlpInventory_CellPaint);
            this.tlpInventory.Paint += new System.Windows.Forms.PaintEventHandler(this.tlpInventory_Paint);
            // 
            // lblFromStoreParked
            // 
            resources.ApplyResources(this.lblFromStoreParked, "lblFromStoreParked");
            this.lblFromStoreParked.Name = "lblFromStoreParked";
            // 
            // lblMyStore
            // 
            resources.ApplyResources(this.lblMyStore, "lblMyStore");
            this.lblMyStore.Name = "lblMyStore";
            // 
            // lblFromStore
            // 
            resources.ApplyResources(this.lblFromStore, "lblFromStore");
            this.lblFromStore.Name = "lblFromStore";
            // 
            // lblFromStoreReserved
            // 
            resources.ApplyResources(this.lblFromStoreReserved, "lblFromStoreReserved");
            this.lblFromStoreReserved.Name = "lblFromStoreReserved";
            // 
            // lblInventOnHand
            // 
            resources.ApplyResources(this.lblInventOnHand, "lblInventOnHand");
            this.lblInventOnHand.Name = "lblInventOnHand";
            // 
            // lblMyStoreParked
            // 
            resources.ApplyResources(this.lblMyStoreParked, "lblMyStoreParked");
            this.lblMyStoreParked.Name = "lblMyStoreParked";
            // 
            // lblMyStoreInventOnHand
            // 
            resources.ApplyResources(this.lblMyStoreInventOnHand, "lblMyStoreInventOnHand");
            this.lblMyStoreInventOnHand.Name = "lblMyStoreInventOnHand";
            // 
            // lblFromStoreOrdered
            // 
            resources.ApplyResources(this.lblFromStoreOrdered, "lblFromStoreOrdered");
            this.lblFromStoreOrdered.Name = "lblFromStoreOrdered";
            // 
            // lblFromStoreInventOnHnad
            // 
            resources.ApplyResources(this.lblFromStoreInventOnHnad, "lblFromStoreInventOnHnad");
            this.lblFromStoreInventOnHnad.Name = "lblFromStoreInventOnHnad";
            // 
            // lblMyStoreReserved
            // 
            resources.ApplyResources(this.lblMyStoreReserved, "lblMyStoreReserved");
            this.lblMyStoreReserved.Name = "lblMyStoreReserved";
            // 
            // lblMyStoreOrdered
            // 
            resources.ApplyResources(this.lblMyStoreOrdered, "lblMyStoreOrdered");
            this.lblMyStoreOrdered.Name = "lblMyStoreOrdered";
            // 
            // lblParked
            // 
            resources.ApplyResources(this.lblParked, "lblParked");
            this.lblParked.Name = "lblParked";
            // 
            // lblOrdered
            // 
            resources.ApplyResources(this.lblOrdered, "lblOrdered");
            this.lblOrdered.Name = "lblOrdered";
            // 
            // lblReserved
            // 
            resources.ApplyResources(this.lblReserved, "lblReserved");
            this.lblReserved.Name = "lblReserved";
            // 
            // pbItemImage
            // 
            resources.ApplyResources(this.pbItemImage, "pbItemImage");
            this.pbItemImage.Name = "pbItemImage";
            this.pbItemImage.TabStop = false;
            this.pbItemImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pbItemImage_Paint);
            // 
            // pnlItems
            // 
            resources.ApplyResources(this.pnlItems, "pnlItems");
            this.pnlItems.Controls.Add(this.btnItemInfo);
            this.pnlItems.Controls.Add(this.ntbBarCode);
            this.pnlItems.Controls.Add(this.lvItems);
            this.pnlItems.Name = "pnlItems";
            // 
            // btnItemInfo
            // 
            resources.ApplyResources(this.btnItemInfo, "btnItemInfo");
            this.btnItemInfo.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnItemInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnItemInfo.BackgroundImage = global::LSOne.Services.Properties.Resources.Arrowdownthin_32px;
            this.btnItemInfo.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnItemInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnItemInfo.Name = "btnItemInfo";
            this.btnItemInfo.UseVisualStyleBackColor = false;
            this.btnItemInfo.Click += new System.EventHandler(this.btnItemInfo_Click);
            // 
            // ntbBarCode
            // 
            this.ntbBarCode.AllowDecimal = false;
            this.ntbBarCode.AllowNegative = false;
            resources.ApplyResources(this.ntbBarCode, "ntbBarCode");
            this.ntbBarCode.BackColor = System.Drawing.Color.White;
            this.ntbBarCode.CultureInfo = null;
            this.ntbBarCode.DecimalLetters = 0;
            this.ntbBarCode.ForeColor = System.Drawing.Color.Black;
            this.ntbBarCode.HasMinValue = false;
            this.ntbBarCode.MaxLength = 32767;
            this.ntbBarCode.MaxValue = 0D;
            this.ntbBarCode.MinValue = 0D;
            this.ntbBarCode.Name = "ntbBarCode";
            this.ntbBarCode.ShowToolTip = false;
            this.ntbBarCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntbBarCode.Value = 0D;
            this.ntbBarCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ntbBarCode_KeyDown);
            this.ntbBarCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ntbBarCode_KeyPress);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.ApplyVisualStyles = false;
            this.lvItems.BackColor = System.Drawing.Color.White;
            this.lvItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.clmItemID);
            this.lvItems.Columns.Add(this.clmItemName);
            this.lvItems.Columns.Add(this.clmVariant);
            this.lvItems.Columns.Add(this.clmQty);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(50));
            this.lvItems.EvenRowColor = System.Drawing.Color.White;
            this.lvItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvItems.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvItems.HeaderHeight = ((short)(30));
            this.lvItems.HideHorizontalScrollbarWhenDisabled = true;
            this.lvItems.HideVerticalScrollbarWhenDisabled = true;
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvItems.RowLines = true;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "0:1";
            this.lvItems.TouchScroll = true;
            this.lvItems.UseFocusRectangle = false;
            this.lvItems.VerticalScrollbarValue = 0;
            this.lvItems.VerticalScrollbarYOffset = 0;
            this.lvItems.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            // 
            // clmItemID
            // 
            this.clmItemID.AutoSize = true;
            this.clmItemID.DefaultStyle = null;
            resources.ApplyResources(this.clmItemID, "clmItemID");
            this.clmItemID.InternalSort = true;
            this.clmItemID.MaximumWidth = ((short)(0));
            this.clmItemID.MinimumWidth = ((short)(100));
            this.clmItemID.SecondarySortColumn = ((short)(-1));
            this.clmItemID.Tag = null;
            this.clmItemID.Width = ((short)(100));
            // 
            // clmItemName
            // 
            this.clmItemName.AutoSize = true;
            this.clmItemName.DefaultStyle = null;
            this.clmItemName.FillRemainingWidth = true;
            resources.ApplyResources(this.clmItemName, "clmItemName");
            this.clmItemName.InternalSort = true;
            this.clmItemName.MaximumWidth = ((short)(0));
            this.clmItemName.MinimumWidth = ((short)(300));
            this.clmItemName.SecondarySortColumn = ((short)(-1));
            this.clmItemName.Tag = null;
            this.clmItemName.Width = ((short)(400));
            // 
            // clmVariant
            // 
            this.clmVariant.AutoSize = true;
            this.clmVariant.DefaultStyle = null;
            resources.ApplyResources(this.clmVariant, "clmVariant");
            this.clmVariant.InternalSort = true;
            this.clmVariant.MaximumWidth = ((short)(0));
            this.clmVariant.MinimumWidth = ((short)(100));
            this.clmVariant.SecondarySortColumn = ((short)(-1));
            this.clmVariant.Tag = null;
            this.clmVariant.Width = ((short)(100));
            // 
            // clmQty
            // 
            this.clmQty.AutoSize = true;
            this.clmQty.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmQty.DefaultStyle = null;
            resources.ApplyResources(this.clmQty, "clmQty");
            this.clmQty.InternalSort = true;
            this.clmQty.MaximumWidth = ((short)(0));
            this.clmQty.MinimumWidth = ((short)(150));
            this.clmQty.SecondarySortColumn = ((short)(-1));
            this.clmQty.Tag = null;
            this.clmQty.Width = ((short)(150));
            // 
            // FocusTimer
            // 
            this.FocusTimer.Interval = 1000;
            this.FocusTimer.Tick += new System.EventHandler(this.FocusTimer_Tick);
            // 
            // btnPanel
            // 
            resources.ApplyResources(this.btnPanel, "btnPanel");
            this.btnPanel.BackColor = System.Drawing.Color.Transparent;
            this.btnPanel.ButtonHeight = 50;
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.TabStop = false;
            this.btnPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.btnPanel_Click);
            // 
            // TransferItemsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnPanel);
            this.Controls.Add(this.pnlItems);
            this.Controls.Add(this.pnlItemInfo);
            this.Name = "TransferItemsPanel";
            this.Load += new System.EventHandler(this.TransferItemsPanel_Load);
            this.pnlItemInfo.ResumeLayout(false);
            this.tlpInventory.ResumeLayout(false);
            this.tlpInventory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbItemImage)).EndInit();
            this.pnlItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchNumPad tnpNumpad;
        private LSOne.Controls.DoubleBufferedPanel pnlItemInfo;
        private LSOne.Controls.DoubleBufferedPanel pnlItems;
        private Controls.ListView lvItems;
        private LSOne.Controls.ShadeNumericTextBox ntbBarCode;
        private Controls.Columns.Column clmItemID;
        private Controls.Columns.Column clmItemName;
        private Controls.Columns.Column clmVariant;
        private Controls.Columns.Column clmQty;
        private System.Windows.Forms.PictureBox pbItemImage;
        private LSOne.Controls.DoubleBufferedTableLayoutPanel tlpInventory;
        private System.Windows.Forms.Label lblInventOnHand;
        private System.Windows.Forms.Label lblOrdered;
        private System.Windows.Forms.Label lblParked;
        private System.Windows.Forms.Label lblReserved;
        private System.Windows.Forms.Label lblMyStore;
        private System.Windows.Forms.Label lblFromStore;
        private System.Windows.Forms.Label lblMyStoreInventOnHand;
        private System.Windows.Forms.Label lblMyStoreReserved;
        private System.Windows.Forms.Label lblMyStoreOrdered;
        private System.Windows.Forms.Label lblMyStoreParked;
        private System.Windows.Forms.Label lblFromStoreOrdered;
        private System.Windows.Forms.Label lblFromStoreInventOnHnad;
        private System.Windows.Forms.Label lblFromStoreParked;
        private System.Windows.Forms.Label lblFromStoreReserved;
        private LSOne.Controls.TouchButton btnItemInfo;
        private System.Windows.Forms.Timer FocusTimer;
        private Controls.TouchScrollButtonPanel btnPanel;
    }
}
