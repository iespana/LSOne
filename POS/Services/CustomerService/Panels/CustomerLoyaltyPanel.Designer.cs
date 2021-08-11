using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class CustomerLoyaltyPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerLoyaltyPanel));
            this.lvLoyalty = new LSOne.Controls.ListView();
            this.clmCardNumber = new LSOne.Controls.Columns.Column();
            this.clmType = new LSOne.Controls.Columns.Column();
            this.clmScheme = new LSOne.Controls.Columns.Column();
            this.clmCurrentValue = new LSOne.Controls.Columns.Column();
            this.clmIssued = new LSOne.Controls.Columns.Column();
            this.clmUsed = new LSOne.Controls.Columns.Column();
            this.clmExpired = new LSOne.Controls.Columns.Column();
            this.clmStatus = new LSOne.Controls.Columns.Column();
            this.lblBalance = new LSOne.Controls.DoubleLabel();
            this.lblExpired = new LSOne.Controls.DoubleLabel();
            this.lblUsed = new LSOne.Controls.DoubleLabel();
            this.lblIssued = new LSOne.Controls.DoubleLabel();
            this.SuspendLayout();
            // 
            // lvLoyalty
            // 
            resources.ApplyResources(this.lvLoyalty, "lvLoyalty");
            this.lvLoyalty.ApplyVisualStyles = false;
            this.lvLoyalty.BackColor = System.Drawing.Color.White;
            this.lvLoyalty.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvLoyalty.BuddyControl = null;
            this.lvLoyalty.Columns.Add(this.clmCardNumber);
            this.lvLoyalty.Columns.Add(this.clmType);
            this.lvLoyalty.Columns.Add(this.clmScheme);
            this.lvLoyalty.Columns.Add(this.clmCurrentValue);
            this.lvLoyalty.Columns.Add(this.clmIssued);
            this.lvLoyalty.Columns.Add(this.clmUsed);
            this.lvLoyalty.Columns.Add(this.clmExpired);
            this.lvLoyalty.Columns.Add(this.clmStatus);
            this.lvLoyalty.ContentBackColor = System.Drawing.Color.White;
            this.lvLoyalty.DefaultRowHeight = ((short)(50));
            this.lvLoyalty.EvenRowColor = System.Drawing.Color.White;
            this.lvLoyalty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvLoyalty.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvLoyalty.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvLoyalty.HeaderHeight = ((short)(30));
            this.lvLoyalty.HideVerticalScrollbarWhenDisabled = true;
            this.lvLoyalty.Name = "lvLoyalty";
            this.lvLoyalty.OddRowColor = System.Drawing.Color.White;
            this.lvLoyalty.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvLoyalty.RowLines = true;
            this.lvLoyalty.SecondarySortColumn = ((short)(-1));
            this.lvLoyalty.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvLoyalty.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLoyalty.SortSetting = "0:1";
            this.lvLoyalty.TouchScroll = true;
            this.lvLoyalty.UseFocusRectangle = false;
            this.lvLoyalty.VerticalScrollbarValue = 0;
            this.lvLoyalty.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvLoyalty.SelectionChanged += new System.EventHandler(this.lvLoyalty_SelectionChanged);
            // 
            // clmCardNumber
            // 
            this.clmCardNumber.AutoSize = true;
            this.clmCardNumber.Clickable = false;
            this.clmCardNumber.DefaultStyle = null;
            resources.ApplyResources(this.clmCardNumber, "clmCardNumber");
            this.clmCardNumber.MaximumWidth = ((short)(0));
            this.clmCardNumber.MinimumWidth = ((short)(10));
            this.clmCardNumber.SecondarySortColumn = ((short)(-1));
            this.clmCardNumber.Tag = null;
            this.clmCardNumber.Width = ((short)(150));
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
            this.clmType.Width = ((short)(60));
            // 
            // clmScheme
            // 
            this.clmScheme.AutoSize = true;
            this.clmScheme.Clickable = false;
            this.clmScheme.DefaultStyle = null;
            this.clmScheme.FillRemainingWidth = true;
            resources.ApplyResources(this.clmScheme, "clmScheme");
            this.clmScheme.MaximumWidth = ((short)(0));
            this.clmScheme.MinimumWidth = ((short)(10));
            this.clmScheme.SecondarySortColumn = ((short)(-1));
            this.clmScheme.Tag = null;
            this.clmScheme.Width = ((short)(100));
            // 
            // clmCurrentValue
            // 
            this.clmCurrentValue.AutoSize = true;
            this.clmCurrentValue.Clickable = false;
            this.clmCurrentValue.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmCurrentValue.DefaultStyle = null;
            resources.ApplyResources(this.clmCurrentValue, "clmCurrentValue");
            this.clmCurrentValue.MaximumWidth = ((short)(0));
            this.clmCurrentValue.MinimumWidth = ((short)(10));
            this.clmCurrentValue.SecondarySortColumn = ((short)(-1));
            this.clmCurrentValue.Tag = null;
            this.clmCurrentValue.Width = ((short)(150));
            // 
            // clmIssued
            // 
            this.clmIssued.AutoSize = true;
            this.clmIssued.Clickable = false;
            this.clmIssued.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmIssued.DefaultStyle = null;
            resources.ApplyResources(this.clmIssued, "clmIssued");
            this.clmIssued.MaximumWidth = ((short)(0));
            this.clmIssued.MinimumWidth = ((short)(10));
            this.clmIssued.SecondarySortColumn = ((short)(-1));
            this.clmIssued.Tag = null;
            this.clmIssued.Width = ((short)(80));
            // 
            // clmUsed
            // 
            this.clmUsed.AutoSize = true;
            this.clmUsed.Clickable = false;
            this.clmUsed.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmUsed.DefaultStyle = null;
            resources.ApplyResources(this.clmUsed, "clmUsed");
            this.clmUsed.MaximumWidth = ((short)(0));
            this.clmUsed.MinimumWidth = ((short)(10));
            this.clmUsed.SecondarySortColumn = ((short)(-1));
            this.clmUsed.Tag = null;
            this.clmUsed.Width = ((short)(80));
            // 
            // clmExpired
            // 
            this.clmExpired.AutoSize = true;
            this.clmExpired.Clickable = false;
            this.clmExpired.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmExpired.DefaultStyle = null;
            resources.ApplyResources(this.clmExpired, "clmExpired");
            this.clmExpired.MaximumWidth = ((short)(0));
            this.clmExpired.MinimumWidth = ((short)(10));
            this.clmExpired.SecondarySortColumn = ((short)(-1));
            this.clmExpired.Tag = null;
            this.clmExpired.Width = ((short)(80));
            // 
            // clmStatus
            // 
            this.clmStatus.AutoSize = true;
            this.clmStatus.Clickable = false;
            this.clmStatus.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmStatus.DefaultStyle = null;
            resources.ApplyResources(this.clmStatus, "clmStatus");
            this.clmStatus.MaximumWidth = ((short)(0));
            this.clmStatus.MinimumWidth = ((short)(10));
            this.clmStatus.SecondarySortColumn = ((short)(-1));
            this.clmStatus.Tag = null;
            this.clmStatus.Width = ((short)(80));
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.White;
            this.lblBalance.HeaderText = "Points balance";
            resources.ApplyResources(this.lblBalance, "lblBalance");
            this.lblBalance.Name = "lblBalance";
            // 
            // lblExpired
            // 
            this.lblExpired.BackColor = System.Drawing.Color.White;
            this.lblExpired.HeaderText = "Expired points";
            resources.ApplyResources(this.lblExpired, "lblExpired");
            this.lblExpired.Name = "lblExpired";
            // 
            // lblUsed
            // 
            this.lblUsed.BackColor = System.Drawing.Color.White;
            this.lblUsed.HeaderText = "Used points";
            resources.ApplyResources(this.lblUsed, "lblUsed");
            this.lblUsed.Name = "lblUsed";
            // 
            // lblIssued
            // 
            this.lblIssued.BackColor = System.Drawing.Color.White;
            this.lblIssued.HeaderText = "Issued points";
            resources.ApplyResources(this.lblIssued, "lblIssued");
            this.lblIssued.Name = "lblIssued";
            // 
            // CustomerLoyaltyPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvLoyalty);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblExpired);
            this.Controls.Add(this.lblUsed);
            this.Controls.Add(this.lblIssued);
            this.Name = "CustomerLoyaltyPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DoubleLabel lblIssued;
        private Controls.DoubleLabel lblUsed;
        private Controls.DoubleLabel lblExpired;
        private Controls.DoubleLabel lblBalance;
        private Controls.ListView lvLoyalty;
        private Controls.Columns.Column clmCardNumber;
        private Controls.Columns.Column clmType;
        private Controls.Columns.Column clmScheme;
        private Controls.Columns.Column clmCurrentValue;
        private Controls.Columns.Column clmIssued;
        private Controls.Columns.Column clmUsed;
        private Controls.Columns.Column clmExpired;
        private Controls.Columns.Column clmStatus;
    }
}
