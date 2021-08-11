using LSOne.Controls;

namespace LSOne.Services.WinFormsTouch
{
    partial class GetGiftCardBalanceDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetGiftCardBalanceDialog));
            this.txtGiftCardID = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.tdBannerBalance = new LSOne.Controls.TouchDialogBanner();
            this.btnClose = new LSOne.Controls.TouchButton();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.lblGiftCardID = new System.Windows.Forms.Label();
            this.btnGet = new LSOne.Controls.TouchButton();
            this.lblBalance = new LSOne.Controls.DoubleLabel();
            this.lblIssued = new LSOne.Controls.DoubleLabel();
            this.SuspendLayout();
            // 
            // txtGiftCardID
            // 
            this.txtGiftCardID.BackColor = System.Drawing.Color.White;
            this.txtGiftCardID.EndCharacter = null;
            this.txtGiftCardID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.txtGiftCardID.LastTrack = null;
            resources.ApplyResources(this.txtGiftCardID, "txtGiftCardID");
            this.txtGiftCardID.ManualEntryOfTrack = true;
            this.txtGiftCardID.MaxLength = 20;
            this.txtGiftCardID.Name = "txtGiftCardID";
            this.txtGiftCardID.NumericOnly = false;
            this.txtGiftCardID.Seperator = null;
            this.txtGiftCardID.StartCharacter = null;
            this.txtGiftCardID.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.txtGiftCardID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGiftCardID_KeyDown);
            this.txtGiftCardID.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tdBannerBalance
            // 
            this.tdBannerBalance.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tdBannerBalance, "tdBannerBalance");
            this.tdBannerBalance.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.tdBannerBalance.Name = "tdBannerBalance";
            this.tdBannerBalance.TabStop = false;
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnClose.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = false;
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
            // lblGiftCardID
            // 
            resources.ApplyResources(this.lblGiftCardID, "lblGiftCardID");
            this.lblGiftCardID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblGiftCardID.Name = "lblGiftCardID";
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
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblBalance, "lblBalance");
            this.lblBalance.HeaderText = "Balance";
            this.lblBalance.Name = "lblBalance";
            // 
            // lblIssued
            // 
            this.lblIssued.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblIssued, "lblIssued");
            this.lblIssued.HeaderText = "Issued";
            this.lblIssued.Name = "lblIssued";
            // 
            // GetGiftCardBalanceDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.lblIssued);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.lblGiftCardID);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tdBannerBalance);
            this.Controls.Add(this.txtGiftCardID);
            this.Name = "GetGiftCardBalanceDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private MSRTextBoxTouch txtGiftCardID;
        private TouchDialogBanner tdBannerBalance;
        private TouchButton btnClose;
        private TouchKeyboard touchKeyboard;
        private System.Windows.Forms.Label lblGiftCardID;
        private TouchButton btnGet;
        private DoubleLabel lblBalance;
        private DoubleLabel lblIssued;
    }
}