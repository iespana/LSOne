using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class InformationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformationPanel));
            this.colID = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colReceived = new LSOne.Controls.Columns.Column();
            this.colOrdered = new LSOne.Controls.Columns.Column();
            this.lvOrder = new LSOne.Controls.ListView();
            this.colToPickUp = new LSOne.Controls.Columns.Column();
            this.colRemaining = new LSOne.Controls.Columns.Column();
            this.colEdit = new LSOne.Controls.Columns.Column();
            this.lblPayNow = new LSOne.Controls.DoubleLabel();
            this.lblBalance = new LSOne.Controls.DoubleLabel();
            this.lblTotal = new LSOne.Controls.DoubleLabel();
            this.lblExpire = new LSOne.Controls.DoubleLabel();
            this.lblDelivery = new LSOne.Controls.DoubleLabel();
            this.lblSource = new LSOne.Controls.DoubleLabel();
            this.lblAddress = new LSOne.Controls.DoubleLabel();
            this.lblCustomer = new LSOne.Controls.DoubleLabel();
            this.lblReference = new LSOne.Controls.DoubleLabel();
            this.panelButtons = new LSOne.Controls.TouchScrollButtonPanel();
            this.SuspendLayout();
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            this.colID.DefaultStyle = null;
            this.colID.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            resources.ApplyResources(this.colID, "colID");
            this.colID.MaximumWidth = ((short)(100));
            this.colID.MinimumWidth = ((short)(100));
            this.colID.RelativeSize = 0;
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(100));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            this.colDescription.FillRemainingWidth = true;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(500));
            this.colDescription.MinimumWidth = ((short)(100));
            this.colDescription.RelativeSize = 0;
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(200));
            // 
            // colReceived
            // 
            this.colReceived.AutoSize = true;
            this.colReceived.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            this.colReceived.DefaultStyle = null;
            this.colReceived.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            resources.ApplyResources(this.colReceived, "colReceived");
            this.colReceived.MaximumWidth = ((short)(145));
            this.colReceived.MinimumWidth = ((short)(145));
            this.colReceived.RelativeSize = 0;
            this.colReceived.SecondarySortColumn = ((short)(-1));
            this.colReceived.Tag = null;
            this.colReceived.Width = ((short)(145));
            // 
            // colOrdered
            // 
            this.colOrdered.AutoSize = true;
            this.colOrdered.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            this.colOrdered.DefaultStyle = null;
            this.colOrdered.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            resources.ApplyResources(this.colOrdered, "colOrdered");
            this.colOrdered.MaximumWidth = ((short)(145));
            this.colOrdered.MinimumWidth = ((short)(145));
            this.colOrdered.RelativeSize = 0;
            this.colOrdered.SecondarySortColumn = ((short)(-1));
            this.colOrdered.Tag = null;
            this.colOrdered.Width = ((short)(145));
            // 
            // lvOrder
            // 
            this.lvOrder.ApplyVisualStyles = false;
            this.lvOrder.BackColor = System.Drawing.Color.White;
            this.lvOrder.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvOrder.BuddyControl = null;
            this.lvOrder.Columns.Add(this.colID);
            this.lvOrder.Columns.Add(this.colDescription);
            this.lvOrder.Columns.Add(this.colOrdered);
            this.lvOrder.Columns.Add(this.colReceived);
            this.lvOrder.Columns.Add(this.colToPickUp);
            this.lvOrder.Columns.Add(this.colRemaining);
            this.lvOrder.Columns.Add(this.colEdit);
            this.lvOrder.ContentBackColor = System.Drawing.Color.White;
            this.lvOrder.DefaultRowHeight = ((short)(50));
            this.lvOrder.EvenRowColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lvOrder, "lvOrder");
            this.lvOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvOrder.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvOrder.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvOrder.HeaderHeight = ((short)(30));
            this.lvOrder.HideVerticalScrollbarWhenDisabled = true;
            this.lvOrder.Name = "lvOrder";
            this.lvOrder.OddRowColor = System.Drawing.Color.White;
            this.lvOrder.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvOrder.RowLines = true;
            this.lvOrder.SecondarySortColumn = ((short)(-1));
            this.lvOrder.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvOrder.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvOrder.SortSetting = "0:1";
            this.lvOrder.TouchScroll = true;
            this.lvOrder.UseFocusRectangle = false;
            this.lvOrder.VerticalScrollbarValue = 0;
            this.lvOrder.VerticalScrollbarYOffset = 0;
            this.lvOrder.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvOrder.CellAction += new LSOne.Controls.CellActionDelegate(this.lvOrder_CellAction);
            // 
            // colToPickUp
            // 
            this.colToPickUp.AutoSize = true;
            this.colToPickUp.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            this.colToPickUp.DefaultStyle = null;
            this.colToPickUp.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            resources.ApplyResources(this.colToPickUp, "colToPickUp");
            this.colToPickUp.MaximumWidth = ((short)(145));
            this.colToPickUp.MinimumWidth = ((short)(145));
            this.colToPickUp.RelativeSize = 0;
            this.colToPickUp.SecondarySortColumn = ((short)(-1));
            this.colToPickUp.Tag = null;
            this.colToPickUp.Width = ((short)(145));
            // 
            // colRemaining
            // 
            this.colRemaining.AutoSize = true;
            this.colRemaining.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            this.colRemaining.DefaultStyle = null;
            this.colRemaining.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            resources.ApplyResources(this.colRemaining, "colRemaining");
            this.colRemaining.MaximumWidth = ((short)(145));
            this.colRemaining.MinimumWidth = ((short)(145));
            this.colRemaining.RelativeSize = 0;
            this.colRemaining.SecondarySortColumn = ((short)(-1));
            this.colRemaining.Tag = null;
            this.colRemaining.Width = ((short)(145));
            // 
            // colEdit
            // 
            this.colEdit.AutoSize = true;
            this.colEdit.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            this.colEdit.DefaultStyle = null;
            resources.ApplyResources(this.colEdit, "colEdit");
            this.colEdit.MaximumWidth = ((short)(50));
            this.colEdit.MinimumWidth = ((short)(50));
            this.colEdit.RelativeSize = 0;
            this.colEdit.SecondarySortColumn = ((short)(-1));
            this.colEdit.Tag = null;
            this.colEdit.Width = ((short)(50));
            // 
            // lblPayNow
            // 
            this.lblPayNow.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblPayNow, "lblPayNow");
            this.lblPayNow.HeaderText = "Pay now";
            this.lblPayNow.Name = "lblPayNow";
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblBalance, "lblBalance");
            this.lblBalance.HeaderText = "Balance";
            this.lblBalance.Name = "lblBalance";
            // 
            // lblTotal
            // 
            this.lblTotal.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblTotal, "lblTotal");
            this.lblTotal.HeaderText = "Total";
            this.lblTotal.Name = "lblTotal";
            // 
            // lblExpire
            // 
            this.lblExpire.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblExpire, "lblExpire");
            this.lblExpire.HeaderText = "Expire";
            this.lblExpire.Name = "lblExpire";
            // 
            // lblDelivery
            // 
            this.lblDelivery.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblDelivery, "lblDelivery");
            this.lblDelivery.HeaderText = "Delivery";
            this.lblDelivery.Name = "lblDelivery";
            // 
            // lblSource
            // 
            this.lblSource.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblSource, "lblSource");
            this.lblSource.HeaderText = "Type";
            this.lblSource.Name = "lblSource";
            // 
            // lblAddress
            // 
            this.lblAddress.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.HeaderText = "Address";
            this.lblAddress.Name = "lblAddress";
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.HeaderText = "Customer";
            this.lblCustomer.Name = "lblCustomer";
            // 
            // lblReference
            // 
            this.lblReference.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblReference, "lblReference");
            this.lblReference.HeaderText = "Reference";
            this.lblReference.Name = "lblReference";
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.ButtonHeight = 50;
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.HorizontalMaxButtonWidth = 200;
            this.panelButtons.HorizontalMinButtonWidth = 150;
            this.panelButtons.IsHorizontal = true;
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panelButtons_Click);
            // 
            // InformationPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblPayNow);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblExpire);
            this.Controls.Add(this.lblDelivery);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.lblReference);
            this.Controls.Add(this.lvOrder);
            this.Controls.Add(this.panelButtons);
            this.Name = "InformationPanel";
            this.ResumeLayout(false);

        }

        #endregion
        private LSOne.Controls.TouchScrollButtonPanel panelButtons;
        private LSOne.Controls.Columns.Column colID;
        private LSOne.Controls.Columns.Column colDescription;
        private LSOne.Controls.Columns.Column colReceived;
        private LSOne.Controls.Columns.Column colOrdered;
        private LSOne.Controls.ListView lvOrder;
        private Controls.Columns.Column colToPickUp;
        private Controls.Columns.Column colRemaining;
        private Controls.DoubleLabel lblReference;
        private Controls.DoubleLabel lblCustomer;
        private Controls.DoubleLabel lblAddress;
        private Controls.DoubleLabel lblSource;
        private Controls.DoubleLabel lblDelivery;
        private Controls.DoubleLabel lblExpire;
        private Controls.DoubleLabel lblTotal;
        private Controls.DoubleLabel lblBalance;
        private Controls.DoubleLabel lblPayNow;
        private Controls.Columns.Column colEdit;
    }
}
