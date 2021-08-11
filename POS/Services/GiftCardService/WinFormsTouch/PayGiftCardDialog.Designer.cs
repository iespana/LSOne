using LSOne.Controls;

namespace LSOne.Services.WinFormsTouch
{
    partial class PayGiftCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayGiftCardDialog));
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.lblGiftCard = new System.Windows.Forms.Label();
            this.tbGiftCardID = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.ntbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.btnGet = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblIssued = new LSOne.Controls.DoubleLabel();
            this.lblBalance = new LSOne.Controls.DoubleLabel();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // lblGiftCard
            // 
            resources.ApplyResources(this.lblGiftCard, "lblGiftCard");
            this.lblGiftCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblGiftCard.Name = "lblGiftCard";
            // 
            // tbGiftCardID
            // 
            resources.ApplyResources(this.tbGiftCardID, "tbGiftCardID");
            this.tbGiftCardID.BackColor = System.Drawing.Color.White;
            this.tbGiftCardID.EndCharacter = null;
            this.tbGiftCardID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbGiftCardID.LastTrack = null;
            this.tbGiftCardID.ManualEntryOfTrack = true;
            this.tbGiftCardID.MaxLength = 60;
            this.tbGiftCardID.Name = "tbGiftCardID";
            this.tbGiftCardID.NumericOnly = false;
            this.tbGiftCardID.Seperator = null;
            this.tbGiftCardID.StartCharacter = null;
            this.tbGiftCardID.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbGiftCardID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbGiftCardID_KeyDown);
            this.tbGiftCardID.TextChanged += new System.EventHandler(this.tbGiftCardID_TextChanged);
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = false;
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
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblAmount.Name = "lblAmount";
            // 
            // btnGet
            // 
            resources.ApplyResources(this.btnGet, "btnGet");
            this.btnGet.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnGet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnGet.BackgroundImage = global::LSOne.Services.Properties.Resources.Checkmark_white_32px;
            this.btnGet.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnGet.DrawBorder = false;
            this.btnGet.ForeColor = System.Drawing.Color.White;
            this.btnGet.Name = "btnGet";
            this.btnGet.UseVisualStyleBackColor = false;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
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
            // 
            // pnlInfo
            // 
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
            this.pnlInfo.Controls.Add(this.lblIssued);
            this.pnlInfo.Controls.Add(this.lblBalance);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInfo_Paint);
            // 
            // lblIssued
            // 
            this.lblIssued.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblIssued, "lblIssued");
            this.lblIssued.HeaderText = "Issued";
            this.lblIssued.Name = "lblIssued";
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblBalance, "lblBalance");
            this.lblBalance.HeaderText = "Balance";
            this.lblBalance.Name = "lblBalance";
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
            // PayGiftCardDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblGiftCard);
            this.Controls.Add(this.tbGiftCardID);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "PayGiftCardDialog";
            this.pnlInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TouchKeyboard touchKeyboard;
        private TouchDialogBanner touchDialogBanner;
        private System.Windows.Forms.Label lblGiftCard;
        private MSRTextBoxTouch tbGiftCardID;
        private ShadeNumericTextBox ntbAmount;
        private System.Windows.Forms.Label lblAmount;
        private TouchButton btnGet;
        private TouchButton btnOk;
        private TouchButton btnCancel;
        private System.Windows.Forms.Panel pnlInfo;
        private DoubleLabel lblIssued;
        private DoubleLabel lblBalance;
    }
}