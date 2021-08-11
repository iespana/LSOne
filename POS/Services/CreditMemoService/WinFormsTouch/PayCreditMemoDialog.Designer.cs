using LSOne.Controls;

namespace LSOne.Services.WinFormsTouch
{
    partial class PayCreditMemoDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PayCreditMemoDialog));
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.lblCreditMemoAmount = new System.Windows.Forms.Label();
            this.lblCreditMemo = new System.Windows.Forms.Label();
            this.tbCreditMemoID = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.btnGet = new LSOne.Controls.TouchButton();
            this.tbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblIssued = new LSOne.Controls.DoubleLabel();
            this.lblBalance = new LSOne.Controls.DoubleLabel();
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
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // lblCreditMemoAmount
            // 
            resources.ApplyResources(this.lblCreditMemoAmount, "lblCreditMemoAmount");
            this.lblCreditMemoAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCreditMemoAmount.Name = "lblCreditMemoAmount";
            // 
            // lblCreditMemo
            // 
            resources.ApplyResources(this.lblCreditMemo, "lblCreditMemo");
            this.lblCreditMemo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblCreditMemo.Name = "lblCreditMemo";
            // 
            // tbCreditMemoID
            // 
            resources.ApplyResources(this.tbCreditMemoID, "tbCreditMemoID");
            this.tbCreditMemoID.BackColor = System.Drawing.Color.White;
            this.tbCreditMemoID.EndCharacter = null;
            this.tbCreditMemoID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbCreditMemoID.LastTrack = null;
            this.tbCreditMemoID.ManualEntryOfTrack = true;
            this.tbCreditMemoID.MaxLength = 60;
            this.tbCreditMemoID.Name = "tbCreditMemoID";
            this.tbCreditMemoID.NumericOnly = false;
            this.tbCreditMemoID.Seperator = null;
            this.tbCreditMemoID.StartCharacter = null;
            this.tbCreditMemoID.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbCreditMemoID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCreditMemoID_KeyDown);
            this.tbCreditMemoID.TextChanged += new System.EventHandler(this.tbCreditMemo_TextChanged);
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
            // tbAmount
            // 
            resources.ApplyResources(this.tbAmount, "tbAmount");
            this.tbAmount.BackColor = System.Drawing.Color.White;
            this.tbAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbAmount.MaxLength = 60;
            this.tbAmount.Name = "tbAmount";
            this.tbAmount.SelectionStart = 1;
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
            this.pnlInfo.Controls.Add(this.lblIssued);
            this.pnlInfo.Controls.Add(this.lblBalance);
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
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
            // PayCreditMemoDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbAmount);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.lblCreditMemoAmount);
            this.Controls.Add(this.lblCreditMemo);
            this.Controls.Add(this.tbCreditMemoID);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "PayCreditMemoDialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PayCreditMemoDialog_FormClosed);
            this.Load += new System.EventHandler(this.PayCreditMemoDialog_Load);
            this.pnlInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TouchKeyboard touchKeyboard;
        private TouchDialogBanner touchDialogBanner;
        private System.Windows.Forms.Label lblCreditMemoAmount;
        private System.Windows.Forms.Label lblCreditMemo;
        private MSRTextBoxTouch tbCreditMemoID;
        private TouchButton btnGet;
        private ShadeNumericTextBox tbAmount;
        private TouchButton btnOk;
        private TouchButton btnCancel;
        private System.Windows.Forms.Panel pnlInfo;
        private DoubleLabel lblIssued;
        private DoubleLabel lblBalance;
    }
}