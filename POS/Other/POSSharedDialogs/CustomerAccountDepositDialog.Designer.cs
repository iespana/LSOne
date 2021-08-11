using LSOne.Controls;

namespace LSOne.Controls.Dialogs
{
    partial class CustomerAccountDepositDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerAccountDepositDialog));
            this.dlgBanner = new LSOne.Controls.TouchDialogBanner();
            this.scrollBtnPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.btnSearch = new LSOne.Controls.TouchButton();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.ntbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblCreditLimit = new LSOne.Controls.DoubleLabel();
            this.lblBalance = new LSOne.Controls.DoubleLabel();
            this.tbCustomer = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.btnOk = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.errorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgBanner
            // 
            resources.ApplyResources(this.dlgBanner, "dlgBanner");
            this.dlgBanner.BackColor = System.Drawing.Color.White;
            this.dlgBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.dlgBanner.Name = "dlgBanner";
            this.dlgBanner.TabStop = false;
            // 
            // scrollBtnPanel
            // 
            resources.ApplyResources(this.scrollBtnPanel, "scrollBtnPanel");
            this.scrollBtnPanel.BackColor = System.Drawing.Color.White;
            this.scrollBtnPanel.ButtonHeight = 50;
            this.scrollBtnPanel.HorizontalMaxButtonWidth = 200;
            this.scrollBtnPanel.IsHorizontal = true;
            this.scrollBtnPanel.Name = "scrollBtnPanel";
            this.scrollBtnPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.scrollBtnPanel_Click);
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnSearch.BackgroundImage = global::LSOne.Controls.Dialogs.Properties.Resources.Whitesearch32px;
            this.btnSearch.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnSearch.DrawBorder = false;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblAmount.Name = "lblAmount";
            // 
            // lblCustomer
            // 
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCustomer.Name = "lblCustomer";
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = false;
            this.ntbAmount.AllowNegative = false;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.BackColor = System.Drawing.Color.White;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbAmount.HasMinValue = false;
            this.ntbAmount.MaxLength = 32767;
            this.ntbAmount.MaxValue = 0D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ntbAmount_KeyPress);
            // 
            // pnlInfo
            // 
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
            this.pnlInfo.Controls.Add(this.lblCreditLimit);
            this.pnlInfo.Controls.Add(this.lblBalance);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInfo_Paint);
            // 
            // lblCreditLimit
            // 
            this.lblCreditLimit.BackColor = System.Drawing.Color.White;
            this.lblCreditLimit.HeaderText = "Credit limit";
            resources.ApplyResources(this.lblCreditLimit, "lblCreditLimit");
            this.lblCreditLimit.Name = "lblCreditLimit";
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.White;
            this.lblBalance.HeaderText = "Current balance";
            resources.ApplyResources(this.lblBalance, "lblBalance");
            this.lblBalance.Name = "lblBalance";
            // 
            // tbCustomer
            // 
            resources.ApplyResources(this.tbCustomer, "tbCustomer");
            this.tbCustomer.BackColor = System.Drawing.Color.White;
            this.tbCustomer.EndCharacter = null;
            this.tbCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCustomer.LastTrack = null;
            this.tbCustomer.ManualEntryOfTrack = true;
            this.tbCustomer.MaxLength = 32767;
            this.tbCustomer.Name = "tbCustomer";
            this.tbCustomer.NumericOnly = false;
            this.tbCustomer.Seperator = null;
            this.tbCustomer.StartCharacter = null;
            this.tbCustomer.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
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
            // errorProvider
            // 
            this.errorProvider.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            this.errorProvider.Name = "errorProvider";
            // 
            // CustomerAccountDepositDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.tbCustomer);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.scrollBtnPanel);
            this.Controls.Add(this.dlgBanner);
            this.Name = "CustomerAccountDepositDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CustomerAccountDepositDialog_FormClosed);
            this.Load += new System.EventHandler(this.CustomerAccountDepositDialog_Load);
            this.pnlInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner dlgBanner;
        private TouchScrollButtonPanel scrollBtnPanel;
        private TouchKeyboard touchKeyboard;
        private TouchButton btnSearch;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblCustomer;
        private ShadeNumericTextBox ntbAmount;
        private System.Windows.Forms.Panel pnlInfo;
        private DoubleLabel lblCreditLimit;
        private DoubleLabel lblBalance;
        private MSRTextBoxTouch tbCustomer;
        private TouchButton btnOk;
        private TouchButton btnCancel;
        private TouchErrorProvider errorProvider;
    }
}