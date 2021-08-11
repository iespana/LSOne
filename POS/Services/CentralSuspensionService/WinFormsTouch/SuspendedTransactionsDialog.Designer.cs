using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class SuspendedTransactionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuspendedTransactionsDialog));
            this.banner = new LSOne.Controls.TouchDialogBanner();
            this.panelButtons = new LSOne.Controls.TouchScrollButtonPanel();
            this.lvSuspendedTransactions = new LSOne.Controls.ListView();
            this.clmExpanded = new LSOne.Controls.Columns.Column();
            this.clmTransactionID = new LSOne.Controls.Columns.Column();
            this.clmDate = new LSOne.Controls.Columns.Column();
            this.clmTerminal = new LSOne.Controls.Columns.Column();
            this.clmStaff = new LSOne.Controls.Columns.Column();
            this.clmAmount = new LSOne.Controls.Columns.Column();
            this.clmLocation = new LSOne.Controls.Columns.Column();
            this.pnlReceipt = new LSOne.Controls.DoubleBufferedPanel();
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
            // panelButtons
            // 
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.ButtonHeight = 50;
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panelButtons_Click);
            // 
            // lvSuspendedTransactions
            // 
            resources.ApplyResources(this.lvSuspendedTransactions, "lvSuspendedTransactions");
            this.lvSuspendedTransactions.ApplyVisualStyles = false;
            this.lvSuspendedTransactions.BackColor = System.Drawing.Color.White;
            this.lvSuspendedTransactions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSuspendedTransactions.BuddyControl = null;
            this.lvSuspendedTransactions.Columns.Add(this.clmExpanded);
            this.lvSuspendedTransactions.Columns.Add(this.clmTransactionID);
            this.lvSuspendedTransactions.Columns.Add(this.clmDate);
            this.lvSuspendedTransactions.Columns.Add(this.clmTerminal);
            this.lvSuspendedTransactions.Columns.Add(this.clmStaff);
            this.lvSuspendedTransactions.Columns.Add(this.clmAmount);
            this.lvSuspendedTransactions.Columns.Add(this.clmLocation);
            this.lvSuspendedTransactions.ContentBackColor = System.Drawing.Color.White;
            this.lvSuspendedTransactions.DefaultRowHeight = ((short)(50));
            this.lvSuspendedTransactions.EvenRowColor = System.Drawing.Color.White;
            this.lvSuspendedTransactions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvSuspendedTransactions.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvSuspendedTransactions.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvSuspendedTransactions.HeaderHeight = ((short)(30));
            this.lvSuspendedTransactions.Name = "lvSuspendedTransactions";
            this.lvSuspendedTransactions.OddRowColor = System.Drawing.Color.White;
            this.lvSuspendedTransactions.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSuspendedTransactions.RowLines = true;
            this.lvSuspendedTransactions.SecondarySortColumn = ((short)(-1));
            this.lvSuspendedTransactions.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvSuspendedTransactions.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSuspendedTransactions.SortSetting = "0:1";
            this.lvSuspendedTransactions.TouchScroll = true;
            this.lvSuspendedTransactions.UseFocusRectangle = false;
            this.lvSuspendedTransactions.VerticalScrollbar = false;
            this.lvSuspendedTransactions.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvSuspendedTransactions.SelectionChanged += new System.EventHandler(this.lvSuspendedTransactions_SelectionChanged);
            this.lvSuspendedTransactions.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvSuspendedTransactions_RowDoubleClick);
            this.lvSuspendedTransactions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvSuspendedTransactions_KeyDown);
            // 
            // clmExpanded
            // 
            this.clmExpanded.AutoSize = true;
            this.clmExpanded.Clickable = false;
            this.clmExpanded.DefaultStyle = null;
            resources.ApplyResources(this.clmExpanded, "clmExpanded");
            this.clmExpanded.MaximumWidth = ((short)(0));
            this.clmExpanded.MinimumWidth = ((short)(50));
            this.clmExpanded.SecondarySortColumn = ((short)(-1));
            this.clmExpanded.Tag = null;
            this.clmExpanded.Width = ((short)(50));
            // 
            // clmTransactionID
            // 
            this.clmTransactionID.AutoSize = true;
            this.clmTransactionID.Clickable = false;
            this.clmTransactionID.DefaultStyle = null;
            resources.ApplyResources(this.clmTransactionID, "clmTransactionID");
            this.clmTransactionID.MaximumWidth = ((short)(0));
            this.clmTransactionID.MinimumWidth = ((short)(10));
            this.clmTransactionID.SecondarySortColumn = ((short)(-1));
            this.clmTransactionID.Tag = null;
            this.clmTransactionID.Width = ((short)(50));
            // 
            // clmDate
            // 
            this.clmDate.AutoSize = true;
            this.clmDate.Clickable = false;
            this.clmDate.DefaultStyle = null;
            resources.ApplyResources(this.clmDate, "clmDate");
            this.clmDate.MaximumWidth = ((short)(0));
            this.clmDate.MinimumWidth = ((short)(10));
            this.clmDate.SecondarySortColumn = ((short)(-1));
            this.clmDate.Tag = null;
            this.clmDate.Width = ((short)(50));
            // 
            // clmTerminal
            // 
            this.clmTerminal.AutoSize = true;
            this.clmTerminal.Clickable = false;
            this.clmTerminal.DefaultStyle = null;
            resources.ApplyResources(this.clmTerminal, "clmTerminal");
            this.clmTerminal.MaximumWidth = ((short)(0));
            this.clmTerminal.MinimumWidth = ((short)(10));
            this.clmTerminal.SecondarySortColumn = ((short)(-1));
            this.clmTerminal.Tag = null;
            this.clmTerminal.Width = ((short)(50));
            // 
            // clmStaff
            // 
            this.clmStaff.AutoSize = true;
            this.clmStaff.Clickable = false;
            this.clmStaff.DefaultStyle = null;
            resources.ApplyResources(this.clmStaff, "clmStaff");
            this.clmStaff.MaximumWidth = ((short)(0));
            this.clmStaff.MinimumWidth = ((short)(10));
            this.clmStaff.SecondarySortColumn = ((short)(-1));
            this.clmStaff.Tag = null;
            this.clmStaff.Width = ((short)(50));
            // 
            // clmAmount
            // 
            this.clmAmount.AutoSize = true;
            this.clmAmount.Clickable = false;
            this.clmAmount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmAmount.DefaultStyle = null;
            this.clmAmount.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            resources.ApplyResources(this.clmAmount, "clmAmount");
            this.clmAmount.MaximumWidth = ((short)(0));
            this.clmAmount.MinimumWidth = ((short)(10));
            this.clmAmount.SecondarySortColumn = ((short)(-1));
            this.clmAmount.Tag = null;
            this.clmAmount.Width = ((short)(50));
            // 
            // clmLocation
            // 
            this.clmLocation.AutoSize = true;
            this.clmLocation.DefaultStyle = null;
            this.clmLocation.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            resources.ApplyResources(this.clmLocation, "clmLocation");
            this.clmLocation.MaximumWidth = ((short)(0));
            this.clmLocation.MinimumWidth = ((short)(10));
            this.clmLocation.SecondarySortColumn = ((short)(-1));
            this.clmLocation.Tag = null;
            this.clmLocation.Width = ((short)(50));
            // 
            // pnlReceipt
            // 
            resources.ApplyResources(this.pnlReceipt, "pnlReceipt");
            this.pnlReceipt.Name = "pnlReceipt";
            // 
            // SuspendedTransactionsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlReceipt);
            this.Controls.Add(this.lvSuspendedTransactions);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.banner);
            this.Name = "SuspendedTransactionsDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.SuspendedTransactionsDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchDialogBanner banner;
        private Controls.TouchScrollButtonPanel panelButtons;
        private Controls.ListView lvSuspendedTransactions;
        private Controls.DoubleBufferedPanel pnlReceipt;
        private Controls.Columns.Column clmExpanded;
        private Controls.Columns.Column clmTransactionID;
        private Controls.Columns.Column clmDate;
        private Controls.Columns.Column clmTerminal;
        private Controls.Columns.Column clmStaff;
        private Controls.Columns.Column clmAmount;
        private Controls.Columns.Column clmLocation;
    }
}