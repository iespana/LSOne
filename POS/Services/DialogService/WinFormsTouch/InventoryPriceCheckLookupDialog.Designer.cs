using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class InventoryPriceCheckLookupDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryPriceCheckLookupDialog));
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.btnPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.lvInventoryStatuses = new LSOne.Controls.ListView();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.clmPrice = new LSOne.Controls.Columns.Column();
            this.clmInventory = new LSOne.Controls.Columns.Column();
            this.tnpNumpad = new LSOne.Controls.TouchNumPad();
            this.tbBarCode = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblItem = new System.Windows.Forms.Label();
            this.lblRegion = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.tbItem = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbRegion = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbCustomer = new LSOne.Controls.ShadeTextBoxTouch();
            this.SuspendLayout();
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // btnPanel
            // 
            resources.ApplyResources(this.btnPanel, "btnPanel");
            this.btnPanel.BackColor = System.Drawing.Color.Transparent;
            this.btnPanel.ButtonHeight = 50;
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.btnPanel_Click);
            // 
            // lvInventoryStatuses
            // 
            resources.ApplyResources(this.lvInventoryStatuses, "lvInventoryStatuses");
            this.lvInventoryStatuses.ApplyVisualStyles = false;
            this.lvInventoryStatuses.BackColor = System.Drawing.Color.White;
            this.lvInventoryStatuses.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvInventoryStatuses.BuddyControl = null;
            this.lvInventoryStatuses.Columns.Add(this.clmStore);
            this.lvInventoryStatuses.Columns.Add(this.clmPrice);
            this.lvInventoryStatuses.Columns.Add(this.clmInventory);
            this.lvInventoryStatuses.ContentBackColor = System.Drawing.Color.White;
            this.lvInventoryStatuses.DefaultRowHeight = ((short)(50));
            this.lvInventoryStatuses.EvenRowColor = System.Drawing.Color.White;
            this.lvInventoryStatuses.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvInventoryStatuses.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvInventoryStatuses.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvInventoryStatuses.HeaderHeight = ((short)(30));
            this.lvInventoryStatuses.Name = "lvInventoryStatuses";
            this.lvInventoryStatuses.OddRowColor = System.Drawing.Color.White;
            this.lvInventoryStatuses.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvInventoryStatuses.RowLines = true;
            this.lvInventoryStatuses.SecondarySortColumn = ((short)(-1));
            this.lvInventoryStatuses.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvInventoryStatuses.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvInventoryStatuses.SortSetting = "1:1";
            this.lvInventoryStatuses.TouchScroll = true;
            this.lvInventoryStatuses.UseFocusRectangle = false;
            this.lvInventoryStatuses.VerticalScrollbar = false;
            this.lvInventoryStatuses.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            // 
            // clmStore
            // 
            this.clmStore.AutoSize = true;
            this.clmStore.Clickable = false;
            this.clmStore.DefaultStyle = null;
            resources.ApplyResources(this.clmStore, "clmStore");
            this.clmStore.MaximumWidth = ((short)(0));
            this.clmStore.MinimumWidth = ((short)(10));
            this.clmStore.SecondarySortColumn = ((short)(-1));
            this.clmStore.Tag = null;
            this.clmStore.Width = ((short)(125));
            // 
            // clmPrice
            // 
            this.clmPrice.AutoSize = true;
            this.clmPrice.Clickable = false;
            this.clmPrice.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmPrice.DefaultStyle = null;
            resources.ApplyResources(this.clmPrice, "clmPrice");
            this.clmPrice.MaximumWidth = ((short)(0));
            this.clmPrice.MinimumWidth = ((short)(10));
            this.clmPrice.SecondarySortColumn = ((short)(-1));
            this.clmPrice.Tag = null;
            this.clmPrice.Width = ((short)(70));
            // 
            // clmInventory
            // 
            this.clmInventory.AutoSize = true;
            this.clmInventory.Clickable = false;
            this.clmInventory.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmInventory.DefaultStyle = null;
            resources.ApplyResources(this.clmInventory, "clmInventory");
            this.clmInventory.MaximumWidth = ((short)(0));
            this.clmInventory.MinimumWidth = ((short)(10));
            this.clmInventory.SecondarySortColumn = ((short)(-1));
            this.clmInventory.Tag = null;
            this.clmInventory.Width = ((short)(100));
            // 
            // tnpNumpad
            // 
            resources.ApplyResources(this.tnpNumpad, "tnpNumpad");
            this.tnpNumpad.BackColor = System.Drawing.Color.Transparent;
            this.tnpNumpad.KeystrokeMode = true;
            this.tnpNumpad.MultiplyButtonIsZeroZero = true;
            this.tnpNumpad.Name = "tnpNumpad";
            this.tnpNumpad.TabStop = false;
            this.tnpNumpad.EnterPressed += new System.EventHandler(this.tnpNumpad_EnterPressed);
            this.tnpNumpad.ClearPressed += new System.EventHandler(this.tnpNumpad_ClearPressed);
            // 
            // tbBarCode
            // 
            resources.ApplyResources(this.tbBarCode, "tbBarCode");
            this.tbBarCode.BackColor = System.Drawing.Color.White;
            this.tbBarCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbBarCode.MaxLength = 32767;
            this.tbBarCode.Name = "tbBarCode";
            this.tbBarCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBarCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBarCode_KeyDown);
            this.tbBarCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbBarCode_KeyPress);
            // 
            // lblItem
            // 
            resources.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblItem.Name = "lblItem";
            // 
            // lblRegion
            // 
            resources.ApplyResources(this.lblRegion, "lblRegion");
            this.lblRegion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblRegion.Name = "lblRegion";
            // 
            // lblCustomer
            // 
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCustomer.Name = "lblCustomer";
            // 
            // tbItem
            // 
            this.tbItem.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbItem, "tbItem");
            this.tbItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbItem.MaxLength = 32767;
            this.tbItem.Name = "tbItem";
            this.tbItem.ReadOnly = true;
            // 
            // tbRegion
            // 
            this.tbRegion.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbRegion, "tbRegion");
            this.tbRegion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbRegion.MaxLength = 32767;
            this.tbRegion.Name = "tbRegion";
            this.tbRegion.ReadOnly = true;
            // 
            // tbCustomer
            // 
            this.tbCustomer.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbCustomer, "tbCustomer");
            this.tbCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCustomer.MaxLength = 32767;
            this.tbCustomer.Name = "tbCustomer";
            this.tbCustomer.ReadOnly = true;
            // 
            // InventoryPriceCheckLookupDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tbCustomer);
            this.Controls.Add(this.tbRegion);
            this.Controls.Add(this.tbItem);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.tbBarCode);
            this.Controls.Add(this.tnpNumpad);
            this.Controls.Add(this.lvInventoryStatuses);
            this.Controls.Add(this.btnPanel);
            this.Controls.Add(this.touchDialogBanner);
            this.Name = "InventoryPriceCheckLookupDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InventoryPriceCheckLookupDialog_FormClosed);
            this.Load += new System.EventHandler(this.InventoryPriceCheckLookupDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.TouchDialogBanner touchDialogBanner;
        private LSOne.Controls.TouchScrollButtonPanel btnPanel;
        private LSOne.Controls.ListView lvInventoryStatuses;
        private LSOne.Controls.TouchNumPad tnpNumpad;
        private LSOne.Controls.ShadeTextBoxTouch tbBarCode;
        private LSOne.Controls.Columns.Column clmPrice;
        private LSOne.Controls.Columns.Column clmStore;
        private LSOne.Controls.Columns.Column clmInventory;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.Label lblCustomer;
        private LSOne.Controls.ShadeTextBoxTouch tbItem;
        private LSOne.Controls.ShadeTextBoxTouch tbRegion;
        private LSOne.Controls.ShadeTextBoxTouch tbCustomer;
    }
}