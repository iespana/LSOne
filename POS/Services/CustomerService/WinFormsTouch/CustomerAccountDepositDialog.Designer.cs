using LSOne.Controls;

namespace LSOne.Services.WinFormsTouch
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbCustomerName = new System.Windows.Forms.TextBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.ntbAmount = new LSOne.Controls.NumericTextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblAccountNumber = new System.Windows.Forms.Label();
            this.tbAccountNumber = new LSOne.Controls.MSRTextBox(this.components);
            this.numCurrentBalance = new LSOne.Controls.NumericTextBox();
            this.lblCurrentBalance = new System.Windows.Forms.Label();
            this.scrollBtnPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.SuspendLayout();
            // 
            // dlgBanner
            // 
            resources.ApplyResources(this.dlgBanner, "dlgBanner");
            this.dlgBanner.BackColor = System.Drawing.Color.White;
            this.dlgBanner.Icon = global::LSOne.Services.Properties.Resources.DialogMessage_48;
            this.dlgBanner.Name = "dlgBanner";
            this.dlgBanner.TabStop = false;
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbCustomerName
            // 
            resources.ApplyResources(this.tbCustomerName, "tbCustomerName");
            this.tbCustomerName.Name = "tbCustomerName";
            // 
            // btnGet
            // 
            resources.ApplyResources(this.btnGet, "btnGet");
            this.btnGet.Name = "btnGet";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = false;
            this.ntbAmount.AllowNegative = true;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.ForeColor = System.Drawing.Color.Black;
            this.ntbAmount.HasMinValue = false;
            this.ntbAmount.MaxValue = 0D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ntbAmount_KeyPress);
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.Name = "lblAmount";
            // 
            // lblCustomerName
            // 
            resources.ApplyResources(this.lblCustomerName, "lblCustomerName");
            this.lblCustomerName.Name = "lblCustomerName";
            // 
            // lblAccountNumber
            // 
            resources.ApplyResources(this.lblAccountNumber, "lblAccountNumber");
            this.lblAccountNumber.Name = "lblAccountNumber";
            // 
            // tbAccountNumber
            // 
            resources.ApplyResources(this.tbAccountNumber, "tbAccountNumber");
            this.tbAccountNumber.EndCharacter = null;
            this.tbAccountNumber.LastTrack = null;
            this.tbAccountNumber.ManualEntryOfTrack = true;
            this.tbAccountNumber.Name = "tbAccountNumber";
            this.tbAccountNumber.NumericOnly = false;
            this.tbAccountNumber.Seperator = null;
            this.tbAccountNumber.StartCharacter = null;
            this.tbAccountNumber.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbAccountNumber.TextChanged += new System.EventHandler(this.tbAccountNumber_TextChanged);
            this.tbAccountNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbAccountNumber_KeyDown);
            // 
            // numCurrentBalance
            // 
            this.numCurrentBalance.AllowDecimal = false;
            this.numCurrentBalance.AllowNegative = true;
            resources.ApplyResources(this.numCurrentBalance, "numCurrentBalance");
            this.numCurrentBalance.CultureInfo = null;
            this.numCurrentBalance.DecimalLetters = 2;
            this.numCurrentBalance.ForeColor = System.Drawing.Color.Black;
            this.numCurrentBalance.HasMinValue = false;
            this.numCurrentBalance.MaxValue = 0D;
            this.numCurrentBalance.MinValue = 0D;
            this.numCurrentBalance.Name = "numCurrentBalance";
            this.numCurrentBalance.Value = 0D;
            // 
            // lblCurrentBalance
            // 
            resources.ApplyResources(this.lblCurrentBalance, "lblCurrentBalance");
            this.lblCurrentBalance.Name = "lblCurrentBalance";
            // 
            // scrollBtnPanel
            // 
            resources.ApplyResources(this.scrollBtnPanel, "scrollBtnPanel");
            this.scrollBtnPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.scrollBtnPanel.ButtonHeight = 47;
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
            // CustomerAccountDepositDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numCurrentBalance);
            this.Controls.Add(this.lblCurrentBalance);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbCustomerName);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.lblAccountNumber);
            this.Controls.Add(this.tbAccountNumber);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.scrollBtnPanel);
            this.Controls.Add(this.dlgBanner);
            this.Name = "CustomerAccountDepositDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CustomerAccountDepositDialog_FormClosed);
            this.Load += new System.EventHandler(this.CustomerAccountDepositDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TouchDialogBanner dlgBanner;
        private TouchScrollButtonPanel scrollBtnPanel;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbCustomerName;
        private System.Windows.Forms.Button btnGet;
        private NumericTextBox ntbAmount;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblAccountNumber;
        private MSRTextBox tbAccountNumber;
        private TouchKeyboard touchKeyboard;
        private NumericTextBox numCurrentBalance;
        private System.Windows.Forms.Label lblCurrentBalance;
    }
}