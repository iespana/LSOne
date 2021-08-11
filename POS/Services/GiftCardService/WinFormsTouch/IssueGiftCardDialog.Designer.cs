using LSOne.Controls;

namespace LSOne.Services.WinFormsTouch
{
    partial class IssueGiftCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueGiftCardDialog));
            this.txtGiftCardID = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.lblGiftCardID = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.ntbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.touchScrollButtonPanel = new LSOne.Controls.TouchScrollButtonPanel();
            this.SuspendLayout();
            // 
            // txtGiftCardID
            // 
            resources.ApplyResources(this.txtGiftCardID, "txtGiftCardID");
            this.txtGiftCardID.BackColor = System.Drawing.Color.White;
            this.txtGiftCardID.EndCharacter = null;
            this.txtGiftCardID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.txtGiftCardID.LastTrack = null;
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
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
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
            this.ntbAmount.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // touchScrollButtonPanel
            // 
            resources.ApplyResources(this.touchScrollButtonPanel, "touchScrollButtonPanel");
            this.touchScrollButtonPanel.BackColor = System.Drawing.Color.White;
            this.touchScrollButtonPanel.ButtonHeight = 50;
            this.touchScrollButtonPanel.IsHorizontal = true;
            this.touchScrollButtonPanel.Name = "touchScrollButtonPanel";
            this.touchScrollButtonPanel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchScrollButtonPanel_Click);
            // 
            // IssueGiftCardDialog
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.touchScrollButtonPanel);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblGiftCardID);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.txtGiftCardID);
            this.Name = "IssueGiftCardDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private MSRTextBoxTouch txtGiftCardID;
        private TouchDialogBanner touchDialogBanner;
        private TouchButton btnCancel;
        private TouchButton btnOk;
        private TouchKeyboard touchKeyboard;
        private System.Windows.Forms.Label lblGiftCardID;
        private System.Windows.Forms.Label lblAmount;
        private ShadeNumericTextBox ntbAmount;
        private TouchScrollButtonPanel touchScrollButtonPanel;

    }
}