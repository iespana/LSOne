using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class SearchInventoryTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchInventoryTransfer));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.tbIDOrDescription = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblIDOrDescription = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.lblTo1 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblDueDate = new System.Windows.Forms.Label();
            this.lblTo2 = new System.Windows.Forms.Label();
            this.dtDueDateTo = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.dtDueDateFrom = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.dtDateTo = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.dtDateFrom = new LSOne.Controls.Dialogs.DatePickerTouch();
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
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            // 
            // tbIDOrDescription
            // 
            this.tbIDOrDescription.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbIDOrDescription, "tbIDOrDescription");
            this.tbIDOrDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbIDOrDescription.MaxLength = 50;
            this.tbIDOrDescription.Name = "tbIDOrDescription";
            this.tbIDOrDescription.Enter += new System.EventHandler(this.BuddyControlEnter);
            this.tbIDOrDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbIDOrDescription_KeyDown);
            this.tbIDOrDescription.Leave += new System.EventHandler(this.BuddyControlLeave);
            this.tbIDOrDescription.TextChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // lblIDOrDescription
            // 
            resources.ApplyResources(this.lblIDOrDescription, "lblIDOrDescription");
            this.lblIDOrDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblIDOrDescription.Name = "lblIDOrDescription";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.IsPOSControl = true;
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(50));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = true;
            this.cmbStore.Tag = "0";
            this.cmbStore.Touch = true;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbRequestingStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            this.cmbStore.RequestClear += new System.EventHandler(this.cmbRequestingStore_RequestClear);
            // 
            // lblStore
            // 
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblStore.Name = "lblStore";
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
            // lblTo1
            // 
            resources.ApplyResources(this.lblTo1, "lblTo1");
            this.lblTo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTo1.Name = "lblTo1";
            // 
            // lblDate
            // 
            resources.ApplyResources(this.lblDate, "lblDate");
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDate.Name = "lblDate";
            // 
            // lblDueDate
            // 
            resources.ApplyResources(this.lblDueDate, "lblDueDate");
            this.lblDueDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDueDate.Name = "lblDueDate";
            // 
            // lblTo2
            // 
            resources.ApplyResources(this.lblTo2, "lblTo2");
            this.lblTo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTo2.Name = "lblTo2";
            // 
            // dtDueDateTo
            // 
            resources.ApplyResources(this.dtDueDateTo, "dtDueDateTo");
            this.dtDueDateTo.BackColor = System.Drawing.Color.White;
            this.dtDueDateTo.Checked = false;
            this.dtDueDateTo.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtDueDateTo.MinDate = new System.DateTime(((long)(0)));
            this.dtDueDateTo.Name = "dtDueDateTo";
            this.dtDueDateTo.ShowEmbeddedCheckBox = true;
            this.dtDueDateTo.CheckedChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // dtDueDateFrom
            // 
            resources.ApplyResources(this.dtDueDateFrom, "dtDueDateFrom");
            this.dtDueDateFrom.BackColor = System.Drawing.Color.White;
            this.dtDueDateFrom.Checked = false;
            this.dtDueDateFrom.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtDueDateFrom.MinDate = new System.DateTime(((long)(0)));
            this.dtDueDateFrom.Name = "dtDueDateFrom";
            this.dtDueDateFrom.ShowEmbeddedCheckBox = true;
            this.dtDueDateFrom.CheckedChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // dtDateTo
            // 
            this.dtDateTo.BackColor = System.Drawing.Color.White;
            this.dtDateTo.Checked = false;
            resources.ApplyResources(this.dtDateTo, "dtDateTo");
            this.dtDateTo.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtDateTo.MinDate = new System.DateTime(((long)(0)));
            this.dtDateTo.Name = "dtDateTo";
            this.dtDateTo.ShowEmbeddedCheckBox = true;
            this.dtDateTo.CheckedChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // dtDateFrom
            // 
            this.dtDateFrom.BackColor = System.Drawing.Color.White;
            this.dtDateFrom.Checked = false;
            resources.ApplyResources(this.dtDateFrom, "dtDateFrom");
            this.dtDateFrom.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtDateFrom.MinDate = new System.DateTime(((long)(0)));
            this.dtDateFrom.Name = "dtDateFrom";
            this.dtDateFrom.ShowEmbeddedCheckBox = true;
            this.dtDateFrom.CheckedChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // SearchInventoryTransfer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtDueDateTo);
            this.Controls.Add(this.dtDueDateFrom);
            this.Controls.Add(this.dtDateTo);
            this.Controls.Add(this.dtDateFrom);
            this.Controls.Add(this.lblDueDate);
            this.Controls.Add(this.lblTo2);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblTo1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.tbIDOrDescription);
            this.Controls.Add(this.lblIDOrDescription);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "SearchInventoryTransfer";
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.TouchKeyboard touchKeyboard;
        private LSOne.Controls.ShadeTextBoxTouch tbIDOrDescription;
        private System.Windows.Forms.Label lblIDOrDescription;
        private Controls.DualDataComboBox cmbStore;
        private System.Windows.Forms.Label lblStore;
        private TouchButton btnCancel;
        private TouchButton btnOk;
        private System.Windows.Forms.Label lblTo1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblDueDate;
        private System.Windows.Forms.Label lblTo2;
        private Controls.Dialogs.DatePickerTouch dtDateFrom;
        private Controls.Dialogs.DatePickerTouch dtDateTo;
        private Controls.Dialogs.DatePickerTouch dtDueDateFrom;
        private Controls.Dialogs.DatePickerTouch dtDueDateTo;
    }
}