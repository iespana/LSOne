using LSOne.Controls;

namespace LSOne.Services.Panels
{
    partial class EditDetailsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDetailsPanel));
            this.lblShippingAddress = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblComment = new System.Windows.Forms.Label();
            this.lblDueDate = new System.Windows.Forms.Label();
            this.cmbDeliveryLocation = new LSOne.Controls.DualDataComboBox();
            this.lblDeliveryLocation = new System.Windows.Forms.Label();
            this.cmbType = new LSOne.Controls.DualDataComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbDelivery = new LSOne.Controls.DualDataComboBox();
            this.lblDelivery = new System.Windows.Forms.Label();
            this.lblReference = new System.Windows.Forms.Label();
            this.tbReference = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbComment = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbCustomerName = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.tbAddress = new LSOne.Controls.ShadeTextBoxTouch();
            this.chkUpdateStock = new LSOne.Controls.TouchCheckBox();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.dtExpires = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.panelButtons = new LSOne.Controls.TouchScrollButtonPanel();
            this.btnSearchCustomer = new LSOne.Controls.TouchButton();
            this.SuspendLayout();
            // 
            // lblShippingAddress
            // 
            resources.ApplyResources(this.lblShippingAddress, "lblShippingAddress");
            this.lblShippingAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblShippingAddress.Name = "lblShippingAddress";
            // 
            // lblCustomer
            // 
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCustomer.Name = "lblCustomer";
            // 
            // lblComment
            // 
            resources.ApplyResources(this.lblComment, "lblComment");
            this.lblComment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblComment.Name = "lblComment";
            // 
            // lblDueDate
            // 
            resources.ApplyResources(this.lblDueDate, "lblDueDate");
            this.lblDueDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDueDate.Name = "lblDueDate";
            // 
            // cmbDeliveryLocation
            // 
            this.cmbDeliveryLocation.AddList = null;
            this.cmbDeliveryLocation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDeliveryLocation, "cmbDeliveryLocation");
            this.cmbDeliveryLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbDeliveryLocation.IsPOSControl = true;
            this.cmbDeliveryLocation.MaxLength = 32767;
            this.cmbDeliveryLocation.Name = "cmbDeliveryLocation";
            this.cmbDeliveryLocation.NoChangeAllowed = false;
            this.cmbDeliveryLocation.OnlyDisplayID = false;
            this.cmbDeliveryLocation.RemoveList = null;
            this.cmbDeliveryLocation.RowHeight = ((short)(50));
            this.cmbDeliveryLocation.SecondaryData = null;
            this.cmbDeliveryLocation.SelectedData = null;
            this.cmbDeliveryLocation.SelectedDataID = null;
            this.cmbDeliveryLocation.SelectionList = null;
            this.cmbDeliveryLocation.SkipIDColumn = true;
            this.cmbDeliveryLocation.Touch = true;
            this.cmbDeliveryLocation.RequestData += new System.EventHandler(this.cmbPickUp_RequestData);
            // 
            // lblDeliveryLocation
            // 
            resources.ApplyResources(this.lblDeliveryLocation, "lblDeliveryLocation");
            this.lblDeliveryLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDeliveryLocation.Name = "lblDeliveryLocation";
            // 
            // cmbType
            // 
            this.cmbType.AddList = null;
            this.cmbType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbType.IsPOSControl = true;
            this.cmbType.MaxLength = 32767;
            this.cmbType.Name = "cmbType";
            this.cmbType.NoChangeAllowed = false;
            this.cmbType.OnlyDisplayID = false;
            this.cmbType.RemoveList = null;
            this.cmbType.RowHeight = ((short)(50));
            this.cmbType.SecondaryData = null;
            this.cmbType.SelectedData = null;
            this.cmbType.SelectedDataID = null;
            this.cmbType.SelectionList = null;
            this.cmbType.SkipIDColumn = true;
            this.cmbType.Tag = "1";
            this.cmbType.Touch = true;
            this.cmbType.RequestData += new System.EventHandler(this.Configurations_RequestData);
            // 
            // lblType
            // 
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblType.Name = "lblType";
            // 
            // cmbDelivery
            // 
            this.cmbDelivery.AddList = null;
            this.cmbDelivery.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDelivery, "cmbDelivery");
            this.cmbDelivery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbDelivery.IsPOSControl = true;
            this.cmbDelivery.MaxLength = 32767;
            this.cmbDelivery.Name = "cmbDelivery";
            this.cmbDelivery.NoChangeAllowed = false;
            this.cmbDelivery.OnlyDisplayID = false;
            this.cmbDelivery.RemoveList = null;
            this.cmbDelivery.RowHeight = ((short)(50));
            this.cmbDelivery.SecondaryData = null;
            this.cmbDelivery.SelectedData = null;
            this.cmbDelivery.SelectedDataID = null;
            this.cmbDelivery.SelectionList = null;
            this.cmbDelivery.SkipIDColumn = true;
            this.cmbDelivery.Tag = "0";
            this.cmbDelivery.Touch = true;
            this.cmbDelivery.RequestData += new System.EventHandler(this.Configurations_RequestData);
            // 
            // lblDelivery
            // 
            resources.ApplyResources(this.lblDelivery, "lblDelivery");
            this.lblDelivery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDelivery.Name = "lblDelivery";
            // 
            // lblReference
            // 
            resources.ApplyResources(this.lblReference, "lblReference");
            this.lblReference.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblReference.Name = "lblReference";
            // 
            // tbReference
            // 
            this.tbReference.BackColor = System.Drawing.Color.White;
            this.tbReference.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbReference, "tbReference");
            this.tbReference.MaxLength = 50;
            this.tbReference.Name = "tbReference";
            this.tbReference.Enter += new System.EventHandler(this.BuddyControlEnter);
            this.tbReference.Leave += new System.EventHandler(this.BuddyControlLeave);
            // 
            // tbComment
            // 
            this.tbComment.BackColor = System.Drawing.Color.White;
            this.tbComment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbComment, "tbComment");
            this.tbComment.MaxLength = 250;
            this.tbComment.Name = "tbComment";
            this.tbComment.Enter += new System.EventHandler(this.BuddyControlEnter);
            this.tbComment.Leave += new System.EventHandler(this.BuddyControlLeave);
            // 
            // tbCustomerName
            // 
            this.tbCustomerName.BackColor = System.Drawing.Color.White;
            this.tbCustomerName.EndCharacter = null;
            this.tbCustomerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCustomerName.LastTrack = null;
            resources.ApplyResources(this.tbCustomerName, "tbCustomerName");
            this.tbCustomerName.ManualEntryOfTrack = true;
            this.tbCustomerName.MaxLength = 60;
            this.tbCustomerName.Name = "tbCustomerName";
            this.tbCustomerName.NumericOnly = false;
            this.tbCustomerName.Seperator = null;
            this.tbCustomerName.StartCharacter = null;
            this.tbCustomerName.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbCustomerName.Enter += new System.EventHandler(this.BuddyControlEnter);
            this.tbCustomerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCustomerName_KeyDown);
            this.tbCustomerName.Leave += new System.EventHandler(this.BuddyControlLeave);
            this.tbCustomerName.TextChanged += new System.EventHandler(this.tbCustomerName_TextChanged);
            // 
            // tbAddress
            // 
            this.tbAddress.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbAddress, "tbAddress");
            this.tbAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbAddress.MaxLength = 60;
            this.tbAddress.Multiline = true;
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.TabStop = false;
            // 
            // chkUpdateStock
            // 
            resources.ApplyResources(this.chkUpdateStock, "chkUpdateStock");
            this.chkUpdateStock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.chkUpdateStock.Name = "chkUpdateStock";
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            // 
            // dtExpires
            // 
            this.dtExpires.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dtExpires, "dtExpires");
            this.dtExpires.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.dtExpires.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtExpires.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtExpires.Name = "dtExpires";
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.ButtonHeight = 50;
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.HorizontalMaxButtonWidth = 150;
            this.panelButtons.HorizontalMinButtonWidth = 150;
            this.panelButtons.IsHorizontal = true;
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panelButtons_Click);
            // 
            // btnSearchCustomer
            // 
            resources.ApplyResources(this.btnSearchCustomer, "btnSearchCustomer");
            this.btnSearchCustomer.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSearchCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnSearchCustomer.BackgroundImage = global::LSOne.Services.Properties.Resources.Whitesearch32px;
            this.btnSearchCustomer.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnSearchCustomer.DrawBorder = false;
            this.btnSearchCustomer.ForeColor = System.Drawing.Color.White;
            this.btnSearchCustomer.Name = "btnSearchCustomer";
            this.btnSearchCustomer.TabStop = false;
            this.btnSearchCustomer.UseVisualStyleBackColor = false;
            this.btnSearchCustomer.Click += new System.EventHandler(this.btnSearchCustomer_Click);
            // 
            // EditDetailsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnSearchCustomer);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lblReference);
            this.Controls.Add(this.tbReference);
            this.Controls.Add(this.lblDelivery);
            this.Controls.Add(this.cmbDelivery);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblDeliveryLocation);
            this.Controls.Add(this.cmbDeliveryLocation);
            this.Controls.Add(this.lblDueDate);
            this.Controls.Add(this.dtExpires);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.tbCustomerName);
            this.Controls.Add(this.lblShippingAddress);
            this.Controls.Add(this.tbAddress);
            this.Controls.Add(this.chkUpdateStock);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "EditDetailsPanel";
            this.Load += new System.EventHandler(this.EditDetailsPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblReference;
        private LSOne.Controls.DualDataComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private LSOne.Controls.DualDataComboBox cmbDelivery;
        private System.Windows.Forms.Label lblDelivery;
        private LSOne.Controls.ShadeTextBoxTouch tbReference;
        private System.Windows.Forms.Label lblDueDate;
        private LSOne.Controls.DualDataComboBox cmbDeliveryLocation;
        private System.Windows.Forms.Label lblDeliveryLocation;
        private LSOne.Controls.ShadeTextBoxTouch tbComment;
        private System.Windows.Forms.Label lblComment;
        private LSOne.Controls.Dialogs.DatePickerTouch dtExpires;
        private LSOne.Controls.TouchKeyboard touchKeyboard;
        private LSOne.Controls.ShadeTextBoxTouch tbAddress;
        private System.Windows.Forms.Label lblShippingAddress;
        private LSOne.Controls.MSRTextBoxTouch tbCustomerName;
        private System.Windows.Forms.Label lblCustomer;
        private Controls.TouchCheckBox chkUpdateStock;
        private TouchScrollButtonPanel panelButtons;
        private TouchButton btnSearchCustomer;
    }
}
