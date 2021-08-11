using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class frmJournalSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJournalSearch));
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.lblReceiptID = new System.Windows.Forms.Label();
            this.txtReceiptId = new LSOne.Controls.ShadeTextBoxTouch();
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.lblType = new System.Windows.Forms.Label();
            this.lblStaff = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.ntbFromAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.lblFromAmount = new System.Windows.Forms.Label();
            this.lblToAmount = new System.Windows.Forms.Label();
            this.ntbToAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.cmbStaff = new LSOne.Controls.DualDataComboBox();
            this.cmbType = new LSOne.Controls.DualDataComboBox();
            this.dpToDate = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.dpFromDate = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.SuspendLayout();
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
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.Tag = "BtnLong";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblReceiptID
            // 
            resources.ApplyResources(this.lblReceiptID, "lblReceiptID");
            this.lblReceiptID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblReceiptID.Name = "lblReceiptID";
            // 
            // txtReceiptId
            // 
            this.txtReceiptId.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txtReceiptId, "txtReceiptId");
            this.txtReceiptId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.txtReceiptId.MaxLength = 61;
            this.txtReceiptId.Name = "txtReceiptId";
            this.txtReceiptId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransactionId_KeyDown);
            this.txtReceiptId.TextChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // lblType
            // 
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblType.Name = "lblType";
            // 
            // lblStaff
            // 
            resources.ApplyResources(this.lblStaff, "lblStaff");
            this.lblStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblStaff.Name = "lblStaff";
            // 
            // lblFromDate
            // 
            resources.ApplyResources(this.lblFromDate, "lblFromDate");
            this.lblFromDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblFromDate.Name = "lblFromDate";
            // 
            // lblToDate
            // 
            resources.ApplyResources(this.lblToDate, "lblToDate");
            this.lblToDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblToDate.Name = "lblToDate";
            // 
            // ntbFromAmount
            // 
            this.ntbFromAmount.AllowDecimal = true;
            this.ntbFromAmount.AllowNegative = true;
            this.ntbFromAmount.BackColor = System.Drawing.Color.White;
            this.ntbFromAmount.CultureInfo = null;
            this.ntbFromAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbFromAmount, "ntbFromAmount");
            this.ntbFromAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbFromAmount.HasMinValue = false;
            this.ntbFromAmount.MaxLength = 15;
            this.ntbFromAmount.MaxValue = 0D;
            this.ntbFromAmount.MinValue = 0D;
            this.ntbFromAmount.Name = "ntbFromAmount";
            this.ntbFromAmount.Value = 0D;
            this.ntbFromAmount.TextChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // lblFromAmount
            // 
            resources.ApplyResources(this.lblFromAmount, "lblFromAmount");
            this.lblFromAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblFromAmount.Name = "lblFromAmount";
            // 
            // lblToAmount
            // 
            resources.ApplyResources(this.lblToAmount, "lblToAmount");
            this.lblToAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblToAmount.Name = "lblToAmount";
            // 
            // ntbToAmount
            // 
            this.ntbToAmount.AllowDecimal = true;
            this.ntbToAmount.AllowNegative = true;
            this.ntbToAmount.BackColor = System.Drawing.Color.White;
            this.ntbToAmount.CultureInfo = null;
            this.ntbToAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbToAmount, "ntbToAmount");
            this.ntbToAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbToAmount.HasMinValue = false;
            this.ntbToAmount.MaxLength = 15;
            this.ntbToAmount.MaxValue = 0D;
            this.ntbToAmount.MinValue = 0D;
            this.ntbToAmount.Name = "ntbToAmount";
            this.ntbToAmount.Value = 0D;
            this.ntbToAmount.TextChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
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
            // cmbStaff
            // 
            this.cmbStaff.AddList = null;
            this.cmbStaff.AllowKeyboardSelection = false;
            this.cmbStaff.EnableTextBox = true;
            resources.ApplyResources(this.cmbStaff, "cmbStaff");
            this.cmbStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbStaff.IsPOSControl = true;
            this.cmbStaff.MaxLength = 32767;
            this.cmbStaff.Name = "cmbStaff";
            this.cmbStaff.NoChangeAllowed = false;
            this.cmbStaff.OnlyDisplayID = false;
            this.cmbStaff.ReadOnly = true;
            this.cmbStaff.RemoveList = null;
            this.cmbStaff.RowHeight = ((short)(50));
            this.cmbStaff.SecondaryData = null;
            this.cmbStaff.SelectedData = null;
            this.cmbStaff.SelectedDataID = null;
            this.cmbStaff.SelectionList = null;
            this.cmbStaff.ShowDropDownOnTyping = true;
            this.cmbStaff.SkipIDColumn = true;
            this.cmbStaff.Touch = true;
            this.cmbStaff.RequestData += new System.EventHandler(this.cmbStaff_RequestData);
            this.cmbStaff.SelectedDataChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            this.cmbStaff.RequestClear += new System.EventHandler(this.cmbStaff_RequestClear);
            // 
            // cmbType
            // 
            this.cmbType.AddList = null;
            this.cmbType.AllowKeyboardSelection = false;
            this.cmbType.EnableTextBox = true;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
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
            this.cmbType.SelectedDataChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            this.cmbType.RequestClear += new System.EventHandler(this.cmbType_RequestClear);
            // 
            // dpToDate
            // 
            this.dpToDate.BackColor = System.Drawing.Color.White;
            this.dpToDate.Checked = false;
            resources.ApplyResources(this.dpToDate, "dpToDate");
            this.dpToDate.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dpToDate.MinDate = new System.DateTime(((long)(0)));
            this.dpToDate.Name = "dpToDate";
            this.dpToDate.ShowEmbeddedCheckBox = true;
            this.dpToDate.CheckedChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // dpFromDate
            // 
            this.dpFromDate.BackColor = System.Drawing.Color.White;
            this.dpFromDate.Checked = false;
            resources.ApplyResources(this.dpFromDate, "dpFromDate");
            this.dpFromDate.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dpFromDate.MinDate = new System.DateTime(((long)(0)));
            this.dpFromDate.Name = "dpFromDate";
            this.dpFromDate.ShowEmbeddedCheckBox = true;
            this.dpFromDate.CheckedChanged += new System.EventHandler(this.CheckSearchCriteriaEntered);
            // 
            // frmJournalSearch
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ntbToAmount);
            this.Controls.Add(this.lblToAmount);
            this.Controls.Add(this.lblFromAmount);
            this.Controls.Add(this.dpToDate);
            this.Controls.Add(this.dpFromDate);
            this.Controls.Add(this.ntbFromAmount);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.cmbStaff);
            this.Controls.Add(this.lblStaff);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblReceiptID);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.txtReceiptId);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "frmJournalSearch";
            this.Load += new System.EventHandler(this.frmJournalSearch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TouchButton btnOk;
        private ShadeTextBoxTouch txtReceiptId;
        private TouchButton btnCancel;
        private System.Windows.Forms.Label lblReceiptID;
        private LSOne.Controls.TouchDialogBanner touchDialogBanner;
        private System.Windows.Forms.Label lblType;
        private DualDataComboBox cmbType;
        private System.Windows.Forms.Label lblStaff;
        private DualDataComboBox cmbStaff;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblToDate;
        private ShadeNumericTextBox ntbFromAmount;
        private LSOne.Controls.Dialogs.DatePickerTouch dpFromDate;
        private LSOne.Controls.Dialogs.DatePickerTouch dpToDate;
        private System.Windows.Forms.Label lblFromAmount;
        private System.Windows.Forms.Label lblToAmount;
        private ShadeNumericTextBox ntbToAmount;
        private TouchKeyboard touchKeyboard;
    }
}