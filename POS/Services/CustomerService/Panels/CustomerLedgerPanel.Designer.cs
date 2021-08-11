using LSOne.POS.Processes.WinControls;
using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class CustomerLedgerPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerLedgerPanel));
            this.lvLedger = new LSOne.Controls.ListView();
            this.clmDate = new LSOne.Controls.Columns.Column();
            this.clmType = new LSOne.Controls.Columns.Column();
            this.clmAmount = new LSOne.Controls.Columns.Column();
            this.clmRemainingAmount = new LSOne.Controls.Columns.Column();
            this.clmStatus = new LSOne.Controls.Columns.Column();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.clmReceiptID = new LSOne.Controls.Columns.Column();
            this.lblCreditLimit = new LSOne.Controls.DoubleLabel();
            this.lblBalance = new LSOne.Controls.DoubleLabel();
            this.lblTotal = new LSOne.Controls.DoubleLabel();
            this.panelLedgerValues = new System.Windows.Forms.Panel();
            this.pnlReceipt = new LSOne.Controls.DoubleBufferedPanel();
            this.panelLedgerValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvLedger
            // 
            resources.ApplyResources(this.lvLedger, "lvLedger");
            this.lvLedger.ApplyVisualStyles = false;
            this.lvLedger.BackColor = System.Drawing.Color.White;
            this.lvLedger.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvLedger.BuddyControl = null;
            this.lvLedger.Columns.Add(this.clmDate);
            this.lvLedger.Columns.Add(this.clmType);
            this.lvLedger.Columns.Add(this.clmAmount);
            this.lvLedger.Columns.Add(this.clmRemainingAmount);
            this.lvLedger.Columns.Add(this.clmStatus);
            this.lvLedger.Columns.Add(this.clmStore);
            this.lvLedger.Columns.Add(this.clmReceiptID);
            this.lvLedger.ContentBackColor = System.Drawing.Color.White;
            this.lvLedger.DefaultRowHeight = ((short)(50));
            this.lvLedger.EvenRowColor = System.Drawing.Color.White;
            this.lvLedger.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvLedger.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvLedger.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvLedger.HeaderHeight = ((short)(30));
            this.lvLedger.Name = "lvLedger";
            this.lvLedger.OddRowColor = System.Drawing.Color.White;
            this.lvLedger.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvLedger.RowLines = true;
            this.lvLedger.SecondarySortColumn = ((short)(-1));
            this.lvLedger.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvLedger.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLedger.SortSetting = "0:1";
            this.lvLedger.TouchScroll = true;
            this.lvLedger.UseFocusRectangle = false;
            this.lvLedger.VerticalScrollbar = false;
            this.lvLedger.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvLedger.SelectionChanged += new System.EventHandler(this.lvLedger_SelectionChanged);
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
            this.clmDate.Width = ((short)(80));
            // 
            // clmType
            // 
            this.clmType.AutoSize = true;
            this.clmType.Clickable = false;
            this.clmType.DefaultStyle = null;
            resources.ApplyResources(this.clmType, "clmType");
            this.clmType.MaximumWidth = ((short)(0));
            this.clmType.MinimumWidth = ((short)(10));
            this.clmType.SecondarySortColumn = ((short)(-1));
            this.clmType.Tag = null;
            this.clmType.Width = ((short)(80));
            // 
            // clmAmount
            // 
            this.clmAmount.AutoSize = true;
            this.clmAmount.Clickable = false;
            this.clmAmount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmAmount.DefaultStyle = null;
            resources.ApplyResources(this.clmAmount, "clmAmount");
            this.clmAmount.MaximumWidth = ((short)(0));
            this.clmAmount.MinimumWidth = ((short)(10));
            this.clmAmount.SecondarySortColumn = ((short)(-1));
            this.clmAmount.Tag = null;
            this.clmAmount.Width = ((short)(100));
            // 
            // clmRemainingAmount
            // 
            this.clmRemainingAmount.AutoSize = true;
            this.clmRemainingAmount.Clickable = false;
            this.clmRemainingAmount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmRemainingAmount.DefaultStyle = null;
            resources.ApplyResources(this.clmRemainingAmount, "clmRemainingAmount");
            this.clmRemainingAmount.MaximumWidth = ((short)(0));
            this.clmRemainingAmount.MinimumWidth = ((short)(10));
            this.clmRemainingAmount.SecondarySortColumn = ((short)(-1));
            this.clmRemainingAmount.Tag = null;
            this.clmRemainingAmount.Width = ((short)(100));
            // 
            // clmStatus
            // 
            this.clmStatus.AutoSize = true;
            this.clmStatus.Clickable = false;
            this.clmStatus.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            this.clmStatus.DefaultStyle = null;
            resources.ApplyResources(this.clmStatus, "clmStatus");
            this.clmStatus.MaximumWidth = ((short)(0));
            this.clmStatus.MinimumWidth = ((short)(10));
            this.clmStatus.SecondarySortColumn = ((short)(-1));
            this.clmStatus.Tag = null;
            this.clmStatus.Width = ((short)(70));
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
            this.clmStore.Width = ((short)(110));
            // 
            // clmReceiptID
            // 
            this.clmReceiptID.AutoSize = true;
            this.clmReceiptID.Clickable = false;
            this.clmReceiptID.DefaultStyle = null;
            resources.ApplyResources(this.clmReceiptID, "clmReceiptID");
            this.clmReceiptID.MaximumWidth = ((short)(0));
            this.clmReceiptID.MinimumWidth = ((short)(10));
            this.clmReceiptID.SecondarySortColumn = ((short)(-1));
            this.clmReceiptID.Tag = null;
            this.clmReceiptID.Width = ((short)(135));
            // 
            // lblCreditLimit
            // 
            this.lblCreditLimit.BackColor = System.Drawing.Color.White;
            this.lblCreditLimit.HeaderText = "Credit limit";
            resources.ApplyResources(this.lblCreditLimit, "lblCreditLimit");
            this.lblCreditLimit.Name = "lblCreditLimit";
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.White;
            this.lblBalance.HeaderText = "Balance";
            resources.ApplyResources(this.lblBalance, "lblBalance");
            this.lblBalance.Name = "lblBalance";
            // 
            // lblTotal
            // 
            this.lblTotal.BackColor = System.Drawing.Color.White;
            this.lblTotal.HeaderText = "Total sales";
            resources.ApplyResources(this.lblTotal, "lblTotal");
            this.lblTotal.Name = "lblTotal";
            // 
            // panelLedgerValues
            // 
            this.panelLedgerValues.BackColor = System.Drawing.SystemColors.Window;
            this.panelLedgerValues.Controls.Add(this.lblCreditLimit);
            this.panelLedgerValues.Controls.Add(this.lblBalance);
            this.panelLedgerValues.Controls.Add(this.lblTotal);
            resources.ApplyResources(this.panelLedgerValues, "panelLedgerValues");
            this.panelLedgerValues.Name = "panelLedgerValues";
            this.panelLedgerValues.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLedgerValues_Paint);
            // 
            // pnlReceipt
            // 
            resources.ApplyResources(this.pnlReceipt, "pnlReceipt");
            this.pnlReceipt.Name = "pnlReceipt";
            // 
            // CustomerLedgerPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlReceipt);
            this.Controls.Add(this.panelLedgerValues);
            this.Controls.Add(this.lvLedger);
            this.DoubleBuffered = true;
            this.Name = "CustomerLedgerPanel";
            this.panelLedgerValues.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvLedger;
        private Controls.Columns.Column clmDate;
        private Controls.Columns.Column clmType;
        private Controls.Columns.Column clmAmount;
        private Controls.Columns.Column clmRemainingAmount;
        private Controls.Columns.Column clmStatus;
        private Controls.Columns.Column clmStore;
        private Controls.Columns.Column clmReceiptID;

        private LSOne.Controls.DoubleLabel lblCreditLimit;
        private LSOne.Controls.DoubleLabel lblBalance;
        private LSOne.Controls.DoubleLabel lblTotal;
        private System.Windows.Forms.Panel panelLedgerValues;
        private Controls.DoubleBufferedPanel pnlReceipt;
    }
}
