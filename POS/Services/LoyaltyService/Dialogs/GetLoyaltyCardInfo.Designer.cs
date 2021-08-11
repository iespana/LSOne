using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class GetLoyaltyCardInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetLoyaltyCardInfo));
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.tbLoyaltyCardNumber = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.lblLoyaltyCardNumber = new System.Windows.Forms.Label();
            this.btnGet = new LSOne.Controls.TouchButton();
            this.lblAmount = new System.Windows.Forms.Label();
            this.ntbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.btnClearDiscount = new LSOne.Controls.TouchButton();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblCardScheme = new LSOne.Controls.DoubleLabel();
            this.lblPointUseLimit = new LSOne.Controls.DoubleLabel();
            this.lblCurrentValue = new LSOne.Controls.DoubleLabel();
            this.lblPointBalance = new LSOne.Controls.DoubleLabel();
            this.lblCustomer = new LSOne.Controls.DoubleLabel();
            this.buttonPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.ntbPoints = new LSOne.Controls.ShadeNumericTextBox();
            this.lblPoints = new System.Windows.Forms.Label();
            this.errorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.pnlInfo.SuspendLayout();
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
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
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
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // tbLoyaltyCardNumber
            // 
            resources.ApplyResources(this.tbLoyaltyCardNumber, "tbLoyaltyCardNumber");
            this.tbLoyaltyCardNumber.BackColor = System.Drawing.Color.White;
            this.tbLoyaltyCardNumber.EndCharacter = null;
            this.tbLoyaltyCardNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbLoyaltyCardNumber.LastTrack = null;
            this.tbLoyaltyCardNumber.ManualEntryOfTrack = true;
            this.tbLoyaltyCardNumber.MaxLength = 32767;
            this.tbLoyaltyCardNumber.Name = "tbLoyaltyCardNumber";
            this.tbLoyaltyCardNumber.NumericOnly = false;
            this.tbLoyaltyCardNumber.Seperator = null;
            this.tbLoyaltyCardNumber.StartCharacter = null;
            this.tbLoyaltyCardNumber.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbLoyaltyCardNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLoyaltyCardNumber_KeyDown);
            this.tbLoyaltyCardNumber.TextChanged += new System.EventHandler(this.tbLoyaltyCardNumber_TextChanged);
            // 
            // lblLoyaltyCardNumber
            // 
            resources.ApplyResources(this.lblLoyaltyCardNumber, "lblLoyaltyCardNumber");
            this.lblLoyaltyCardNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblLoyaltyCardNumber.Name = "lblLoyaltyCardNumber";
            // 
            // btnGet
            // 
            resources.ApplyResources(this.btnGet, "btnGet");
            this.btnGet.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnGet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnGet.BackgroundImage = global::LSOne.Services.Properties.Resources.Checkmark_white_32px;
            this.btnGet.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnGet.DrawBorder = false;
            this.btnGet.ForeColor = System.Drawing.Color.White;
            this.btnGet.Name = "btnGet";
            this.btnGet.UseVisualStyleBackColor = false;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblAmount.Name = "lblAmount";
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = true;
            this.ntbAmount.AllowNegative = false;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.BackColor = System.Drawing.Color.White;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            this.ntbAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.ntbAmount.HasMinValue = false;
            this.ntbAmount.MaxLength = 12;
            this.ntbAmount.MaxValue = 0D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.Leave += new System.EventHandler(this.ntbAmount_Leave);
            this.ntbAmount.TextChanged += new System.EventHandler(this.ntbAmount_TextChanged);
            // 
            // btnClearDiscount
            // 
            resources.ApplyResources(this.btnClearDiscount, "btnClearDiscount");
            this.btnClearDiscount.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClearDiscount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnClearDiscount.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnClearDiscount.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnClearDiscount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnClearDiscount.Name = "btnClearDiscount";
            this.btnClearDiscount.UseVisualStyleBackColor = false;
            // 
            // pnlInfo
            // 
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
            this.pnlInfo.Controls.Add(this.lblCardScheme);
            this.pnlInfo.Controls.Add(this.lblPointUseLimit);
            this.pnlInfo.Controls.Add(this.lblCurrentValue);
            this.pnlInfo.Controls.Add(this.lblPointBalance);
            this.pnlInfo.Controls.Add(this.lblCustomer);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInfo_Paint);
            // 
            // lblCardScheme
            // 
            this.lblCardScheme.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCardScheme, "lblCardScheme");
            this.lblCardScheme.HeaderText = "Card scheme";
            this.lblCardScheme.Name = "lblCardScheme";
            // 
            // lblPointUseLimit
            // 
            this.lblPointUseLimit.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblPointUseLimit, "lblPointUseLimit");
            this.lblPointUseLimit.HeaderText = "Point use limit %";
            this.lblPointUseLimit.Name = "lblPointUseLimit";
            // 
            // lblCurrentValue
            // 
            this.lblCurrentValue.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblCurrentValue, "lblCurrentValue");
            this.lblCurrentValue.HeaderText = "Current value";
            this.lblCurrentValue.Name = "lblCurrentValue";
            // 
            // lblPointBalance
            // 
            this.lblPointBalance.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblPointBalance, "lblPointBalance");
            this.lblPointBalance.HeaderText = "Point balance";
            this.lblPointBalance.Name = "lblPointBalance";
            // 
            // lblCustomer
            // 
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.BackColor = System.Drawing.Color.White;
            this.lblCustomer.HeaderText = "Customer";
            this.lblCustomer.Name = "lblCustomer";
            // 
            // buttonPanel
            // 
            resources.ApplyResources(this.buttonPanel, "buttonPanel");
            this.buttonPanel.BackColor = System.Drawing.Color.White;
            this.buttonPanel.ButtonHeight = 50;
            this.buttonPanel.IsHorizontal = true;
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchScrollButtonPanel_Click);
            // 
            // ntbPoints
            // 
            this.ntbPoints.AllowDecimal = true;
            this.ntbPoints.AllowNegative = false;
            resources.ApplyResources(this.ntbPoints, "ntbPoints");
            this.ntbPoints.BackColor = System.Drawing.Color.White;
            this.ntbPoints.CultureInfo = null;
            this.ntbPoints.DecimalLetters = 0;
            this.ntbPoints.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.ntbPoints.HasMinValue = false;
            this.ntbPoints.MaxLength = 12;
            this.ntbPoints.MaxValue = 0D;
            this.ntbPoints.MinValue = 0D;
            this.ntbPoints.Name = "ntbPoints";
            this.ntbPoints.Value = 0D;
            // 
            // lblPoints
            // 
            resources.ApplyResources(this.lblPoints, "lblPoints");
            this.lblPoints.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPoints.Name = "lblPoints";
            // 
            // errorProvider
            // 
            resources.ApplyResources(this.errorProvider, "errorProvider");
            this.errorProvider.BackColor = System.Drawing.Color.White;
            this.errorProvider.Name = "errorProvider";
            // 
            // GetLoyaltyCardInfo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.lblPoints);
            this.Controls.Add(this.ntbPoints);
            this.Controls.Add(this.btnClearDiscount);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.tbLoyaltyCardNumber);
            this.Controls.Add(this.lblLoyaltyCardNumber);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "GetLoyaltyCardInfo";
            this.pnlInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TouchKeyboard touchKeyboard;
        private TouchButton btnCancel;
        private TouchButton btnOk;
        private TouchDialogBanner touchDialogBanner;
        private MSRTextBoxTouch tbLoyaltyCardNumber;
        private System.Windows.Forms.Label lblLoyaltyCardNumber;
        private TouchButton btnGet;
        private TouchScrollButtonPanel buttonPanel;
        private System.Windows.Forms.Label lblAmount;
        private ShadeNumericTextBox ntbAmount;
        private TouchButton btnClearDiscount;
        private System.Windows.Forms.Panel pnlInfo;
        private DoubleLabel lblPointBalance;
        private DoubleLabel lblCustomer;
        private DoubleLabel lblCardScheme;
        private DoubleLabel lblPointUseLimit;
        private DoubleLabel lblCurrentValue;
        private ShadeNumericTextBox ntbPoints;
        private System.Windows.Forms.Label lblPoints;
        private Controls.Dialogs.TouchErrorProvider errorProvider;
    }
}