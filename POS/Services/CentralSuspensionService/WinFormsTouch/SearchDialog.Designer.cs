namespace LSOne.Services.WinFormsTouch
{
    partial class SearchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchDialog));
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.ntbToAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.lblToAmount = new System.Windows.Forms.Label();
            this.lblFromAmount = new System.Windows.Forms.Label();
            this.ntbFromAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.lblToDate = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblStaff = new System.Windows.Forms.Label();
            this.lblTerminal = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new LSOne.Controls.ShadeTextBoxTouch();
            this.dpToDate = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.dpFromDate = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.cmbStaff = new LSOne.Controls.DualDataComboBox();
            this.cmbTerminal = new LSOne.Controls.DualDataComboBox();
            this.SuspendLayout();
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
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // ntbToAmount
            // 
            this.ntbToAmount.AllowDecimal = true;
            this.ntbToAmount.AllowNegative = true;
            this.ntbToAmount.BackColor = System.Drawing.Color.White;
            this.ntbToAmount.CultureInfo = null;
            this.ntbToAmount.DecimalLetters = 2;
            this.ntbToAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbToAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbToAmount, "ntbToAmount");
            this.ntbToAmount.MaxLength = 15;
            this.ntbToAmount.MaxValue = 0D;
            this.ntbToAmount.MinValue = 0D;
            this.ntbToAmount.Name = "ntbToAmount";
            this.ntbToAmount.Value = 0D;
            this.ntbToAmount.Leave += new System.EventHandler(this.ntbToAmount_Leave);
            // 
            // lblToAmount
            // 
            resources.ApplyResources(this.lblToAmount, "lblToAmount");
            this.lblToAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblToAmount.Name = "lblToAmount";
            // 
            // lblFromAmount
            // 
            resources.ApplyResources(this.lblFromAmount, "lblFromAmount");
            this.lblFromAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblFromAmount.Name = "lblFromAmount";
            // 
            // ntbFromAmount
            // 
            this.ntbFromAmount.AllowDecimal = true;
            this.ntbFromAmount.AllowNegative = true;
            this.ntbFromAmount.BackColor = System.Drawing.Color.White;
            this.ntbFromAmount.CultureInfo = null;
            this.ntbFromAmount.DecimalLetters = 2;
            this.ntbFromAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbFromAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbFromAmount, "ntbFromAmount");
            this.ntbFromAmount.MaxLength = 15;
            this.ntbFromAmount.MaxValue = 0D;
            this.ntbFromAmount.MinValue = 0D;
            this.ntbFromAmount.Name = "ntbFromAmount";
            this.ntbFromAmount.Value = 0D;
            this.ntbFromAmount.Leave += new System.EventHandler(this.ntbFromAmount_Leave);
            // 
            // lblToDate
            // 
            resources.ApplyResources(this.lblToDate, "lblToDate");
            this.lblToDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblToDate.Name = "lblToDate";
            // 
            // lblFromDate
            // 
            resources.ApplyResources(this.lblFromDate, "lblFromDate");
            this.lblFromDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblFromDate.Name = "lblFromDate";
            // 
            // lblStaff
            // 
            resources.ApplyResources(this.lblStaff, "lblStaff");
            this.lblStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblStaff.Name = "lblStaff";
            // 
            // lblTerminal
            // 
            resources.ApplyResources(this.lblTerminal, "lblTerminal");
            this.lblTerminal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblTerminal.Name = "lblTerminal";
            // 
            // lblSearch
            // 
            resources.ApplyResources(this.lblSearch, "lblSearch");
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblSearch.Name = "lblSearch";
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.MaxLength = 61;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
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
            // 
            // cmbStaff
            // 
            this.cmbStaff.AddList = null;
            this.cmbStaff.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStaff, "cmbStaff");
            this.cmbStaff.EnableTextBox = true;
            this.cmbStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbStaff.IsPOSControl = true;
            this.cmbStaff.MaxLength = 32767;
            this.cmbStaff.Name = "cmbStaff";
            this.cmbStaff.NoChangeAllowed = false;
            this.cmbStaff.OnlyDisplayID = false;
            this.cmbStaff.ReadOnly = true;
            this.cmbStaff.RemoveList = null;
            this.cmbStaff.RowHeight = ((short)(22));
            this.cmbStaff.SecondaryData = null;
            this.cmbStaff.SelectedData = null;
            this.cmbStaff.SelectedDataID = null;
            this.cmbStaff.SelectionList = null;
            this.cmbStaff.ShowDropDownOnTyping = true;
            this.cmbStaff.SkipIDColumn = true;
            this.cmbStaff.Touch = true;
            this.cmbStaff.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbStaff_DropDown);
            this.cmbStaff.RequestClear += new System.EventHandler(this.cmbStaff_RequestClear);
            // 
            // cmbTerminal
            // 
            this.cmbTerminal.AddList = null;
            this.cmbTerminal.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTerminal, "cmbTerminal");
            this.cmbTerminal.EnableTextBox = true;
            this.cmbTerminal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbTerminal.IsPOSControl = true;
            this.cmbTerminal.MaxLength = 32767;
            this.cmbTerminal.Name = "cmbTerminal";
            this.cmbTerminal.NoChangeAllowed = false;
            this.cmbTerminal.OnlyDisplayID = false;
            this.cmbTerminal.ReadOnly = true;
            this.cmbTerminal.RemoveList = null;
            this.cmbTerminal.RowHeight = ((short)(22));
            this.cmbTerminal.SecondaryData = null;
            this.cmbTerminal.SelectedData = null;
            this.cmbTerminal.SelectedDataID = null;
            this.cmbTerminal.SelectionList = null;
            this.cmbTerminal.ShowDropDownOnTyping = true;
            this.cmbTerminal.SkipIDColumn = true;
            this.cmbTerminal.Touch = true;
            this.cmbTerminal.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbTerminal_DropDown);
            this.cmbTerminal.RequestClear += new System.EventHandler(this.cmbTerminal_RequestClear);
            // 
            // SearchDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbStaff);
            this.Controls.Add(this.cmbTerminal);
            this.Controls.Add(this.ntbToAmount);
            this.Controls.Add(this.lblToAmount);
            this.Controls.Add(this.lblFromAmount);
            this.Controls.Add(this.dpToDate);
            this.Controls.Add(this.dpFromDate);
            this.Controls.Add(this.ntbFromAmount);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.lblStaff);
            this.Controls.Add(this.lblTerminal);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "SearchDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.SearchDialog_Load);
            this.Shown += new System.EventHandler(this.SearchDialog_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchKeyboard touchKeyboard;
        private Controls.TouchButton btnCancel;
        private Controls.TouchButton btnOK;
        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.ShadeNumericTextBox ntbToAmount;
        private System.Windows.Forms.Label lblToAmount;
        private System.Windows.Forms.Label lblFromAmount;
        private Controls.Dialogs.DatePickerTouch dpToDate;
        private Controls.Dialogs.DatePickerTouch dpFromDate;
        private Controls.ShadeNumericTextBox ntbFromAmount;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblStaff;
        private System.Windows.Forms.Label lblTerminal;
        private System.Windows.Forms.Label lblSearch;
        private Controls.ShadeTextBoxTouch txtSearch;
        private Controls.DualDataComboBox cmbTerminal;
        private Controls.DualDataComboBox cmbStaff;
    }
}