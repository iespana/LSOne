using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class SearchCustomerOrders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchCustomerOrders));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.tbReference = new LSOne.Controls.ShadeTextBoxTouch();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCustomerName = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.tbComment = new LSOne.Controls.ShadeTextBoxTouch();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDeliveryLocation = new LSOne.Controls.DualDataComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbSource = new LSOne.Controls.DualDataComboBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.cmbDelivery = new LSOne.Controls.DualDataComboBox();
            this.lblDelivery = new System.Windows.Forms.Label();
            this.chkExpired = new LSOne.Controls.TouchCheckBox();
            this.btnSearchCustomer = new LSOne.Controls.TouchButton();
            this.btnMyLocation = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.SuspendLayout();
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            // 
            // tbReference
            // 
            resources.ApplyResources(this.tbReference, "tbReference");
            this.tbReference.BackColor = System.Drawing.Color.White;
            this.tbReference.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbReference.MaxLength = 80;
            this.tbReference.Name = "tbReference";
            this.tbReference.Enter += new System.EventHandler(this.BuddyControlEnter);
            this.tbReference.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbReference_KeyDown);
            this.tbReference.Leave += new System.EventHandler(this.BuddyControlLeave);
            this.tbReference.TextChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbCustomerName
            // 
            resources.ApplyResources(this.tbCustomerName, "tbCustomerName");
            this.tbCustomerName.BackColor = System.Drawing.Color.White;
            this.tbCustomerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCustomerName.MaxLength = 250;
            this.tbCustomerName.Name = "tbCustomerName";
            this.tbCustomerName.TextChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // lblCustomerName
            // 
            resources.ApplyResources(this.lblCustomerName, "lblCustomerName");
            this.lblCustomerName.Name = "lblCustomerName";
            // 
            // tbComment
            // 
            resources.ApplyResources(this.tbComment, "tbComment");
            this.tbComment.BackColor = System.Drawing.Color.White;
            this.tbComment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbComment.MaxLength = 80;
            this.tbComment.Name = "tbComment";
            this.tbComment.Enter += new System.EventHandler(this.BuddyControlEnter);
            this.tbComment.Leave += new System.EventHandler(this.BuddyControlLeave);
            this.tbComment.TextChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbDeliveryLocation
            // 
            this.cmbDeliveryLocation.AddList = null;
            this.cmbDeliveryLocation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDeliveryLocation, "cmbDeliveryLocation");
            this.cmbDeliveryLocation.IsPOSControl = true;
            this.cmbDeliveryLocation.MaxLength = 32767;
            this.cmbDeliveryLocation.Name = "cmbDeliveryLocation";
            this.cmbDeliveryLocation.NoChangeAllowed = false;
            this.cmbDeliveryLocation.OnlyDisplayID = false;
            this.cmbDeliveryLocation.ReadOnly = true;
            this.cmbDeliveryLocation.RemoveList = null;
            this.cmbDeliveryLocation.RowHeight = ((short)(50));
            this.cmbDeliveryLocation.SecondaryData = null;
            this.cmbDeliveryLocation.SelectedData = null;
            this.cmbDeliveryLocation.SelectedDataID = null;
            this.cmbDeliveryLocation.SelectionList = null;
            this.cmbDeliveryLocation.SkipIDColumn = true;
            this.cmbDeliveryLocation.Touch = true;
            this.cmbDeliveryLocation.RequestData += new System.EventHandler(this.cmbDeliveryLocation_RequestData);
            this.cmbDeliveryLocation.SelectedDataChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            this.cmbDeliveryLocation.RequestClear += new System.EventHandler(this.cmbDeliveryLocation_RequestClear);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // cmbSource
            // 
            this.cmbSource.AddList = null;
            this.cmbSource.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSource, "cmbSource");
            this.cmbSource.IsPOSControl = true;
            this.cmbSource.MaxLength = 32767;
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.NoChangeAllowed = false;
            this.cmbSource.OnlyDisplayID = false;
            this.cmbSource.ReadOnly = true;
            this.cmbSource.RemoveList = null;
            this.cmbSource.RowHeight = ((short)(50));
            this.cmbSource.SecondaryData = null;
            this.cmbSource.SelectedData = null;
            this.cmbSource.SelectedDataID = null;
            this.cmbSource.SelectionList = null;
            this.cmbSource.SkipIDColumn = true;
            this.cmbSource.Tag = "1";
            this.cmbSource.Touch = true;
            this.cmbSource.RequestData += new System.EventHandler(this.cmbDelivery_RequestData);
            this.cmbSource.SelectedDataChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            this.cmbSource.RequestClear += new System.EventHandler(this.cmbSource_RequestClear);
            // 
            // lblSource
            // 
            resources.ApplyResources(this.lblSource, "lblSource");
            this.lblSource.Name = "lblSource";
            // 
            // cmbDelivery
            // 
            this.cmbDelivery.AddList = null;
            this.cmbDelivery.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDelivery, "cmbDelivery");
            this.cmbDelivery.IsPOSControl = true;
            this.cmbDelivery.MaxLength = 32767;
            this.cmbDelivery.Name = "cmbDelivery";
            this.cmbDelivery.NoChangeAllowed = false;
            this.cmbDelivery.OnlyDisplayID = false;
            this.cmbDelivery.ReadOnly = true;
            this.cmbDelivery.RemoveList = null;
            this.cmbDelivery.RowHeight = ((short)(50));
            this.cmbDelivery.SecondaryData = null;
            this.cmbDelivery.SelectedData = null;
            this.cmbDelivery.SelectedDataID = null;
            this.cmbDelivery.SelectionList = null;
            this.cmbDelivery.SkipIDColumn = true;
            this.cmbDelivery.Tag = "0";
            this.cmbDelivery.Touch = true;
            this.cmbDelivery.RequestData += new System.EventHandler(this.cmbDelivery_RequestData);
            this.cmbDelivery.SelectedDataChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            this.cmbDelivery.RequestClear += new System.EventHandler(this.cmbDelivery_RequestClear);
            // 
            // lblDelivery
            // 
            resources.ApplyResources(this.lblDelivery, "lblDelivery");
            this.lblDelivery.Name = "lblDelivery";
            // 
            // chkExpired
            // 
            resources.ApplyResources(this.chkExpired, "chkExpired");
            this.chkExpired.Name = "chkExpired";
            this.chkExpired.CheckedChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
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
            // btnMyLocation
            // 
            resources.ApplyResources(this.btnMyLocation, "btnMyLocation");
            this.btnMyLocation.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnMyLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnMyLocation.BackgroundImage = global::LSOne.Services.Properties.Resources.current_location_32px;
            this.btnMyLocation.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnMyLocation.DrawBorder = false;
            this.btnMyLocation.ForeColor = System.Drawing.Color.White;
            this.btnMyLocation.Name = "btnMyLocation";
            this.btnMyLocation.TabStop = false;
            this.btnMyLocation.UseVisualStyleBackColor = false;
            this.btnMyLocation.Click += new System.EventHandler(this.btnMyLocation_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancel.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Tag = "BtnLong";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.Tag = "BtnLong";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // SearchCustomerOrders
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnMyLocation);
            this.Controls.Add(this.btnSearchCustomer);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.chkExpired);
            this.Controls.Add(this.tbCustomerName);
            this.Controls.Add(this.cmbSource);
            this.Controls.Add(this.tbReference);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbDeliveryLocation);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.cmbDelivery);
            this.Controls.Add(this.lblDelivery);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.label1);
            this.Name = "SearchCustomerOrders";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.TouchKeyboard touchKeyboard;
        private Controls.ShadeTextBoxTouch tbReference;
        private System.Windows.Forms.Label label1;
        private Controls.ShadeTextBoxTouch tbCustomerName;
        private System.Windows.Forms.Label lblCustomerName;
        private Controls.ShadeTextBoxTouch tbComment;
        private System.Windows.Forms.Label label3;
        private Controls.DualDataComboBox cmbDeliveryLocation;
        private System.Windows.Forms.Label label11;
        private Controls.DualDataComboBox cmbSource;
        private System.Windows.Forms.Label lblSource;
        private Controls.DualDataComboBox cmbDelivery;
        private System.Windows.Forms.Label lblDelivery;
        private Controls.TouchCheckBox chkExpired;
        private Controls.TouchButton btnSearchCustomer;
        private Controls.TouchButton btnMyLocation;
        private Controls.TouchButton btnCancel;
        private Controls.TouchButton btnOk;
    }
}