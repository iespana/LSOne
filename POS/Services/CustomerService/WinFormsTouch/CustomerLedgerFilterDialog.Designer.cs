namespace LSOne.Services.WinFormsTouch
{
    partial class CustomerLedgerFilterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerLedgerFilterDialog));
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.cmbType = new LSOne.Controls.DualDataComboBox();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.cmbTerminal = new LSOne.Controls.DualDataComboBox();
            this.txtDocument = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblTerminal = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblDtTo = new System.Windows.Forms.Label();
            this.lblDocument = new System.Windows.Forms.Label();
            this.lblStore = new System.Windows.Forms.Label();
            this.lblDtFrom = new System.Windows.Forms.Label();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.dtTo = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.dtFrom = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.SuspendLayout();
            // 
            // touchDialogBanner
            // 
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // cmbType
            // 
            this.cmbType.AddList = null;
            this.cmbType.AllowKeyboardSelection = false;
            this.cmbType.EnableTextBox = true;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.IsPOSControl = true;
            this.cmbType.MaxLength = 32767;
            this.cmbType.Name = "cmbType";
            this.cmbType.NoChangeAllowed = false;
            this.cmbType.OnlyDisplayID = false;
            this.cmbType.ReadOnly = true;
            this.cmbType.RemoveList = null;
            this.cmbType.RowHeight = ((short)(50));
            this.cmbType.SecondaryData = null;
            this.cmbType.SelectedData = null;
            this.cmbType.SelectedDataID = null;
            this.cmbType.SelectionList = null;
            this.cmbType.ShowDropDownOnTyping = true;
            this.cmbType.SkipIDColumn = true;
            this.cmbType.Touch = true;
            this.cmbType.RequestData += new System.EventHandler(this.cmbType_RequestData);
            this.cmbType.SelectedDataChanged += new System.EventHandler(this.btnOK_Enabled);
            this.cmbType.RequestClear += new System.EventHandler(this.cmbType_RequestClear);
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
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            this.cmbStore.EnableTextBox = true;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.IsPOSControl = true;
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.ReadOnly = true;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(50));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.ShowDropDownOnTyping = true;
            this.cmbStore.SkipIDColumn = true;
            this.cmbStore.Touch = true;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.cmbStore_SelectedDataChanged);
            this.cmbStore.RequestClear += new System.EventHandler(this.cmbStore_RequestClear);
            // 
            // cmbTerminal
            // 
            this.cmbTerminal.AddList = null;
            this.cmbTerminal.AllowKeyboardSelection = false;
            this.cmbTerminal.EnableTextBox = true;
            resources.ApplyResources(this.cmbTerminal, "cmbTerminal");
            this.cmbTerminal.IsPOSControl = true;
            this.cmbTerminal.MaxLength = 32767;
            this.cmbTerminal.Name = "cmbTerminal";
            this.cmbTerminal.NoChangeAllowed = false;
            this.cmbTerminal.OnlyDisplayID = false;
            this.cmbTerminal.ReadOnly = true;
            this.cmbTerminal.RemoveList = null;
            this.cmbTerminal.RowHeight = ((short)(50));
            this.cmbTerminal.SecondaryData = null;
            this.cmbTerminal.SelectedData = null;
            this.cmbTerminal.SelectedDataID = null;
            this.cmbTerminal.SelectionList = null;
            this.cmbTerminal.ShowDropDownOnTyping = true;
            this.cmbTerminal.SkipIDColumn = true;
            this.cmbTerminal.Touch = true;
            this.cmbTerminal.RequestData += new System.EventHandler(this.cmbTerminal_RequestData);
            this.cmbTerminal.SelectedDataChanged += new System.EventHandler(this.btnOK_Enabled);
            this.cmbTerminal.RequestClear += new System.EventHandler(this.cmbTerminal_RequestClear);
            // 
            // txtDocument
            // 
            this.txtDocument.BackColor = System.Drawing.Color.White;
            this.txtDocument.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.txtDocument, "txtDocument");
            this.txtDocument.MaxLength = 80;
            this.txtDocument.Name = "txtDocument";
            this.txtDocument.TextChanged += new System.EventHandler(this.btnOK_Enabled);
            // 
            // lblTerminal
            // 
            resources.ApplyResources(this.lblTerminal, "lblTerminal");
            this.lblTerminal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTerminal.Name = "lblTerminal";
            // 
            // lblType
            // 
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblType.Name = "lblType";
            // 
            // lblDtTo
            // 
            resources.ApplyResources(this.lblDtTo, "lblDtTo");
            this.lblDtTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDtTo.Name = "lblDtTo";
            // 
            // lblDocument
            // 
            resources.ApplyResources(this.lblDocument, "lblDocument");
            this.lblDocument.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDocument.Name = "lblDocument";
            // 
            // lblStore
            // 
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblStore.Name = "lblStore";
            // 
            // lblDtFrom
            // 
            resources.ApplyResources(this.lblDtFrom, "lblDtFrom");
            this.lblDtFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDtFrom.Name = "lblDtFrom";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancel.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dtTo
            // 
            this.dtTo.BackColor = System.Drawing.Color.White;
            this.dtTo.Checked = false;
            resources.ApplyResources(this.dtTo, "dtTo");
            this.dtTo.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtTo.MinDate = new System.DateTime(((long)(0)));
            this.dtTo.Name = "dtTo";
            this.dtTo.ShowEmbeddedCheckBox = true;
            this.dtTo.CheckedChanged += new System.EventHandler(this.btnOK_Enabled);
            // 
            // dtFrom
            // 
            this.dtFrom.BackColor = System.Drawing.Color.White;
            this.dtFrom.Checked = false;
            resources.ApplyResources(this.dtFrom, "dtFrom");
            this.dtFrom.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtFrom.MinDate = new System.DateTime(((long)(0)));
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.ShowEmbeddedCheckBox = true;
            this.dtFrom.CheckedChanged += new System.EventHandler(this.btnOK_Enabled);
            // 
            // CustomerLedgerFilterDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.cmbTerminal);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.txtDocument);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblDtFrom);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.lblDocument);
            this.Controls.Add(this.lblDtTo);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblTerminal);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "CustomerLedgerFilterDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchDialogBanner touchDialogBanner;
        private LSOne.Controls.Dialogs.DatePickerTouch dtFrom;
        private LSOne.Controls.Dialogs.DatePickerTouch dtTo;
        private Controls.DualDataComboBox cmbType;
        private Controls.TouchKeyboard touchKeyboard;
        private Controls.DualDataComboBox cmbStore;
        private Controls.DualDataComboBox cmbTerminal;
        private Controls.ShadeTextBoxTouch txtDocument;
        private System.Windows.Forms.Label lblTerminal;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblDtTo;
        private System.Windows.Forms.Label lblDocument;
        private System.Windows.Forms.Label lblStore;
        private System.Windows.Forms.Label lblDtFrom;
        private Controls.TouchButton btnOK;
        private Controls.TouchButton btnCancel;
    }
}