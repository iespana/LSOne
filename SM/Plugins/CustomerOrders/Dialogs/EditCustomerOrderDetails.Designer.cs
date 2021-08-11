namespace LSOne.ViewPlugins.CustomerOrders.Dialogs
{
    partial class EditCustomerOrderDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCustomerOrderDetails));
            this.label3 = new System.Windows.Forms.Label();
            this.tbReference = new System.Windows.Forms.TextBox();
            this.cmbDelivery = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbSource = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDeliveryLocation = new LSOne.Controls.DualDataComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbShippingAddress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblNoOfOrders = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbCustomer = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbReference
            // 
            resources.ApplyResources(this.tbReference, "tbReference");
            this.tbReference.Name = "tbReference";
            this.tbReference.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbDelivery
            // 
            this.cmbDelivery.AddList = null;
            this.cmbDelivery.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDelivery, "cmbDelivery");
            this.cmbDelivery.MaxLength = 32767;
            this.cmbDelivery.Name = "cmbDelivery";
            this.cmbDelivery.NoChangeAllowed = false;
            this.cmbDelivery.OnlyDisplayID = false;
            this.cmbDelivery.RemoveList = null;
            this.cmbDelivery.RowHeight = ((short)(22));
            this.cmbDelivery.SecondaryData = null;
            this.cmbDelivery.SelectedData = null;
            this.cmbDelivery.SelectedDataID = null;
            this.cmbDelivery.SelectionList = null;
            this.cmbDelivery.SkipIDColumn = true;
            this.cmbDelivery.RequestData += new System.EventHandler(this.Configurations_RequestData);
            this.cmbDelivery.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbDelivery.RequestClear += new System.EventHandler(this.cmbDelivery_RequestClear);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbSource
            // 
            this.cmbSource.AddList = null;
            this.cmbSource.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSource, "cmbSource");
            this.cmbSource.MaxLength = 32767;
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.NoChangeAllowed = false;
            this.cmbSource.OnlyDisplayID = false;
            this.cmbSource.RemoveList = null;
            this.cmbSource.RowHeight = ((short)(22));
            this.cmbSource.SecondaryData = null;
            this.cmbSource.SelectedData = null;
            this.cmbSource.SelectedDataID = null;
            this.cmbSource.SelectionList = null;
            this.cmbSource.SkipIDColumn = true;
            this.cmbSource.RequestData += new System.EventHandler(this.Configurations_RequestData);
            this.cmbSource.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbSource.RequestClear += new System.EventHandler(this.cmbDelivery_RequestClear);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbDeliveryLocation
            // 
            this.cmbDeliveryLocation.AddList = null;
            this.cmbDeliveryLocation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDeliveryLocation, "cmbDeliveryLocation");
            this.cmbDeliveryLocation.MaxLength = 32767;
            this.cmbDeliveryLocation.Name = "cmbDeliveryLocation";
            this.cmbDeliveryLocation.NoChangeAllowed = false;
            this.cmbDeliveryLocation.OnlyDisplayID = false;
            this.cmbDeliveryLocation.RemoveList = null;
            this.cmbDeliveryLocation.RowHeight = ((short)(22));
            this.cmbDeliveryLocation.SecondaryData = null;
            this.cmbDeliveryLocation.SelectedData = null;
            this.cmbDeliveryLocation.SelectedDataID = null;
            this.cmbDeliveryLocation.SelectionList = null;
            this.cmbDeliveryLocation.SkipIDColumn = true;
            this.cmbDeliveryLocation.RequestData += new System.EventHandler(this.cmbDeliveryLocation_RequestData);
            this.cmbDeliveryLocation.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbDeliveryLocation.RequestClear += new System.EventHandler(this.cmbDelivery_RequestClear);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // dtExpiryDate
            // 
            resources.ApplyResources(this.dtExpiryDate, "dtExpiryDate");
            this.dtExpiryDate.Name = "dtExpiryDate";
            this.dtExpiryDate.ShowCheckBox = true;
            this.dtExpiryDate.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbComment
            // 
            resources.ApplyResources(this.tbComment, "tbComment");
            this.tbComment.Name = "tbComment";
            this.tbComment.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // tbShippingAddress
            // 
            resources.ApplyResources(this.tbShippingAddress, "tbShippingAddress");
            this.tbShippingAddress.Name = "tbShippingAddress";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.lblNoOfOrders);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // lblNoOfOrders
            // 
            resources.ApplyResources(this.lblNoOfOrders, "lblNoOfOrders");
            this.lblNoOfOrders.ForeColor = System.Drawing.Color.Red;
            this.lblNoOfOrders.Name = "lblNoOfOrders";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.AddList = null;
            this.cmbCustomer.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCustomer, "cmbCustomer");
            this.cmbCustomer.MaxLength = 32767;
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.NoChangeAllowed = false;
            this.cmbCustomer.OnlyDisplayID = false;
            this.cmbCustomer.RemoveList = null;
            this.cmbCustomer.RowHeight = ((short)(22));
            this.cmbCustomer.SecondaryData = null;
            this.cmbCustomer.SelectedData = null;
            this.cmbCustomer.SelectedDataID = null;
            this.cmbCustomer.SelectionList = null;
            this.cmbCustomer.SkipIDColumn = true;
            this.cmbCustomer.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbCustomer_DropDown);
            this.cmbCustomer.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // EditCustomerOrderDetails
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbShippingAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.dtExpiryDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbDeliveryLocation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbDelivery);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbReference);
            this.HasHelp = true;
            this.Name = "EditCustomerOrderDetails";
            this.Shown += new System.EventHandler(this.EditCustomerOrderDetails_Shown);
            this.Controls.SetChildIndex(this.tbReference, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbDelivery, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbSource, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbDeliveryLocation, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.dtExpiryDate, 0);
            this.Controls.SetChildIndex(this.tbComment, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.tbShippingAddress, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbCustomer, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbReference;
        private Controls.DualDataComboBox cmbDelivery;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private Controls.DualDataComboBox cmbSource;
        private System.Windows.Forms.Label label5;
        private Controls.DualDataComboBox cmbDeliveryLocation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtExpiryDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbShippingAddress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNoOfOrders;
        private Controls.DualDataComboBox cmbCustomer;
    }
}