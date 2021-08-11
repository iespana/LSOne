using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class SearchPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchPanel));
            this.pnlCustomerInfo = new System.Windows.Forms.Panel();
            this.lblAddress = new LSOne.Controls.DoubleLabel();
            this.lblComment = new LSOne.Controls.DoubleLabel();
            this.lblCustomer = new LSOne.Controls.DoubleLabel();
            this.pnlNoCustomerInfo = new System.Windows.Forms.Panel();
            this.lblNoCustomerInfo = new System.Windows.Forms.Label();
            this.lvCustomerOrders = new LSOne.Controls.ListView();
            this.colSelection = new LSOne.Controls.Columns.Column();
            this.colReference = new LSOne.Controls.Columns.Column();
            this.colDelivery = new LSOne.Controls.Columns.Column();
            this.colSource = new LSOne.Controls.Columns.Column();
            this.colDeliveryLocation = new LSOne.Controls.Columns.Column();
            this.colComment = new LSOne.Controls.Columns.Column();
            this.colStatus = new LSOne.Controls.Columns.Column();
            this.pnlReceipt = new LSOne.Controls.DoubleBufferedPanel();
            this.pnlCustomerInfo.SuspendLayout();
            this.pnlNoCustomerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCustomerInfo
            // 
            this.pnlCustomerInfo.Controls.Add(this.lblAddress);
            this.pnlCustomerInfo.Controls.Add(this.lblComment);
            this.pnlCustomerInfo.Controls.Add(this.lblCustomer);
            resources.ApplyResources(this.pnlCustomerInfo, "pnlCustomerInfo");
            this.pnlCustomerInfo.Name = "pnlCustomerInfo";
            this.pnlCustomerInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCustomerInfo_Paint);
            // 
            // lblAddress
            // 
            this.lblAddress.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.HeaderText = "Address";
            this.lblAddress.Name = "lblAddress";
            // 
            // lblComment
            // 
            this.lblComment.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblComment, "lblComment");
            this.lblComment.HeaderText = "Comment";
            this.lblComment.Name = "lblComment";
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.HeaderText = "Customer";
            this.lblCustomer.Name = "lblCustomer";
            // 
            // pnlNoCustomerInfo
            // 
            this.pnlNoCustomerInfo.Controls.Add(this.lblNoCustomerInfo);
            resources.ApplyResources(this.pnlNoCustomerInfo, "pnlNoCustomerInfo");
            this.pnlNoCustomerInfo.Name = "pnlNoCustomerInfo";
            // 
            // lblNoCustomerInfo
            // 
            resources.ApplyResources(this.lblNoCustomerInfo, "lblNoCustomerInfo");
            this.lblNoCustomerInfo.ForeColor = System.Drawing.Color.Red;
            this.lblNoCustomerInfo.Name = "lblNoCustomerInfo";
            // 
            // lvCustomerOrders
            // 
            resources.ApplyResources(this.lvCustomerOrders, "lvCustomerOrders");
            this.lvCustomerOrders.ApplyVisualStyles = false;
            this.lvCustomerOrders.BackColor = System.Drawing.Color.White;
            this.lvCustomerOrders.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvCustomerOrders.BuddyControl = null;
            this.lvCustomerOrders.Columns.Add(this.colSelection);
            this.lvCustomerOrders.Columns.Add(this.colReference);
            this.lvCustomerOrders.Columns.Add(this.colDelivery);
            this.lvCustomerOrders.Columns.Add(this.colSource);
            this.lvCustomerOrders.Columns.Add(this.colDeliveryLocation);
            this.lvCustomerOrders.Columns.Add(this.colComment);
            this.lvCustomerOrders.Columns.Add(this.colStatus);
            this.lvCustomerOrders.ContentBackColor = System.Drawing.Color.White;
            this.lvCustomerOrders.DefaultRowHeight = ((short)(50));
            this.lvCustomerOrders.EvenRowColor = System.Drawing.Color.White;
            this.lvCustomerOrders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvCustomerOrders.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvCustomerOrders.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvCustomerOrders.HeaderHeight = ((short)(30));
            this.lvCustomerOrders.HideVerticalScrollbarWhenDisabled = true;
            this.lvCustomerOrders.HorizontalScrollbar = true;
            this.lvCustomerOrders.Name = "lvCustomerOrders";
            this.lvCustomerOrders.OddRowColor = System.Drawing.Color.White;
            this.lvCustomerOrders.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvCustomerOrders.RowLines = true;
            this.lvCustomerOrders.SecondarySortColumn = ((short)(-1));
            this.lvCustomerOrders.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvCustomerOrders.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCustomerOrders.SortSetting = "1:1";
            this.lvCustomerOrders.TouchScroll = true;
            this.lvCustomerOrders.UseFocusRectangle = false;
            this.lvCustomerOrders.VerticalScrollbarValue = 0;
            this.lvCustomerOrders.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvCustomerOrders.SelectionChanged += new System.EventHandler(this.lvCustomerOrders_SelectionChanged);
            this.lvCustomerOrders.CellAction += new LSOne.Controls.CellActionDelegate(this.lvCustomerOrders_CellAction);
            // 
            // colSelection
            // 
            this.colSelection.AutoSize = true;
            this.colSelection.DefaultStyle = null;
            resources.ApplyResources(this.colSelection, "colSelection");
            this.colSelection.MaximumWidth = ((short)(0));
            this.colSelection.MinimumWidth = ((short)(15));
            this.colSelection.RelativeSize = 10;
            this.colSelection.SecondarySortColumn = ((short)(-1));
            this.colSelection.Tag = null;
            this.colSelection.Width = ((short)(15));
            // 
            // colReference
            // 
            this.colReference.AutoSize = true;
            this.colReference.DefaultStyle = null;
            resources.ApplyResources(this.colReference, "colReference");
            this.colReference.InternalSort = true;
            this.colReference.MaximumWidth = ((short)(0));
            this.colReference.MinimumWidth = ((short)(50));
            this.colReference.RelativeSize = 25;
            this.colReference.SecondarySortColumn = ((short)(-1));
            this.colReference.Tag = null;
            this.colReference.Width = ((short)(100));
            // 
            // colDelivery
            // 
            this.colDelivery.AutoSize = true;
            this.colDelivery.DefaultStyle = null;
            resources.ApplyResources(this.colDelivery, "colDelivery");
            this.colDelivery.InternalSort = true;
            this.colDelivery.MaximumWidth = ((short)(0));
            this.colDelivery.MinimumWidth = ((short)(25));
            this.colDelivery.RelativeSize = 25;
            this.colDelivery.SecondarySortColumn = ((short)(-1));
            this.colDelivery.Tag = null;
            this.colDelivery.Width = ((short)(60));
            // 
            // colSource
            // 
            this.colSource.AutoSize = true;
            this.colSource.DefaultStyle = null;
            resources.ApplyResources(this.colSource, "colSource");
            this.colSource.InternalSort = true;
            this.colSource.MaximumWidth = ((short)(0));
            this.colSource.MinimumWidth = ((short)(25));
            this.colSource.RelativeSize = 25;
            this.colSource.SecondarySortColumn = ((short)(-1));
            this.colSource.Tag = null;
            this.colSource.Width = ((short)(60));
            // 
            // colDeliveryLocation
            // 
            this.colDeliveryLocation.AutoSize = true;
            this.colDeliveryLocation.DefaultStyle = null;
            resources.ApplyResources(this.colDeliveryLocation, "colDeliveryLocation");
            this.colDeliveryLocation.InternalSort = true;
            this.colDeliveryLocation.MaximumWidth = ((short)(0));
            this.colDeliveryLocation.MinimumWidth = ((short)(50));
            this.colDeliveryLocation.RelativeSize = 25;
            this.colDeliveryLocation.SecondarySortColumn = ((short)(-1));
            this.colDeliveryLocation.Tag = null;
            this.colDeliveryLocation.Width = ((short)(150));
            // 
            // colComment
            // 
            this.colComment.AutoSize = true;
            this.colComment.DefaultStyle = null;
            resources.ApplyResources(this.colComment, "colComment");
            this.colComment.InternalSort = true;
            this.colComment.MaximumWidth = ((short)(0));
            this.colComment.MinimumWidth = ((short)(10));
            this.colComment.RelativeSize = 10;
            this.colComment.SecondarySortColumn = ((short)(-1));
            this.colComment.Tag = null;
            this.colComment.Width = ((short)(10));
            // 
            // colStatus
            // 
            this.colStatus.AutoSize = true;
            this.colStatus.DefaultStyle = null;
            resources.ApplyResources(this.colStatus, "colStatus");
            this.colStatus.InternalSort = true;
            this.colStatus.MaximumWidth = ((short)(0));
            this.colStatus.MinimumWidth = ((short)(25));
            this.colStatus.RelativeSize = 10;
            this.colStatus.SecondarySortColumn = ((short)(-1));
            this.colStatus.Tag = null;
            this.colStatus.Width = ((short)(50));
            // 
            // pnlReceipt
            // 
            resources.ApplyResources(this.pnlReceipt, "pnlReceipt");
            this.pnlReceipt.Name = "pnlReceipt";
            // 
            // SearchPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlReceipt);
            this.Controls.Add(this.pnlNoCustomerInfo);
            this.Controls.Add(this.pnlCustomerInfo);
            this.Controls.Add(this.lvCustomerOrders);
            this.Name = "SearchPanel";
            this.pnlCustomerInfo.ResumeLayout(false);
            this.pnlNoCustomerInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvCustomerOrders;
        private Controls.Columns.Column colReference;
        private Controls.Columns.Column colDelivery;
        private Controls.Columns.Column colSource;
        private Controls.Columns.Column colDeliveryLocation;
        private System.Windows.Forms.Panel pnlCustomerInfo;
        private Controls.Columns.Column colStatus;
        private Controls.Columns.Column colSelection;
        private Controls.Columns.Column colComment;
        private System.Windows.Forms.Panel pnlNoCustomerInfo;
        private Controls.DoubleLabel lblCustomer;
        private System.Windows.Forms.Label lblNoCustomerInfo;
        private Controls.DoubleLabel lblComment;
        private Controls.DoubleLabel lblAddress;
        private Controls.DoubleBufferedPanel pnlReceipt;
    }
}
