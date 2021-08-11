using LSOne.Utilities.ColorPalette;

namespace LSOne.Controls.Dialogs
{
    partial class CashManagementDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashManagementDialog));
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.ntbAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.ntbAddedAmount = new LSOne.Controls.ShadeNumericTextBox();
            this.lblAddedAmount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // touchKeyboard
            // 
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard1_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard1_ObtainCultureName);
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            this.tbDescription.BackColor = System.Drawing.Color.White;
            this.tbDescription.EndCharacter = null;
            this.tbDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbDescription.LastTrack = null;
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.ManualEntryOfTrack = true;
            this.tbDescription.MaxLength = 150;
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.NumericOnly = false;
            this.tbDescription.Seperator = null;
            this.tbDescription.StartCharacter = null;
            this.tbDescription.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            // 
            // ntbAmount
            // 
            this.ntbAmount.AllowDecimal = false;
            this.ntbAmount.AllowNegative = true;
            this.ntbAmount.BackColor = System.Drawing.Color.White;
            this.ntbAmount.CultureInfo = null;
            this.ntbAmount.DecimalLetters = 2;
            resources.ApplyResources(this.ntbAmount, "ntbAmount");
            this.ntbAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.ntbAmount.HasMinValue = false;
            this.ntbAmount.MaxLength = 32767;
            this.ntbAmount.MaxValue = 0D;
            this.ntbAmount.MinValue = 0D;
            this.ntbAmount.Name = "ntbAmount";
            this.ntbAmount.Value = 0D;
            this.ntbAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ntbAmount_KeyPress);
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblAmount.Name = "lblAmount";
            // 
            // btnOk
            // 
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancel.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ntbAddedAmount
            // 
            this.ntbAddedAmount.AllowDecimal = true;
            this.ntbAddedAmount.AllowNegative = true;
            this.ntbAddedAmount.BackColor = System.Drawing.Color.White;
            this.ntbAddedAmount.CultureInfo = null;
            this.ntbAddedAmount.DecimalLetters = 2;
            this.ntbAddedAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.ntbAddedAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbAddedAmount, "ntbAddedAmount");
            this.ntbAddedAmount.MaxLength = 16;
            this.ntbAddedAmount.MaxValue = 2100000000D;
            this.ntbAddedAmount.MinValue = 0D;
            this.ntbAddedAmount.Name = "ntbAddedAmount";
            this.ntbAddedAmount.Value = 0D;
            // 
            // lblAddedAmount
            // 
            resources.ApplyResources(this.lblAddedAmount, "lblAddedAmount");
            this.lblAddedAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblAddedAmount.Name = "lblAddedAmount";
            // 
            // CashManagementDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ntbAddedAmount);
            this.Controls.Add(this.lblAddedAmount);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.ntbAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.touchDialogBanner);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "CashManagementDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchKeyboard touchKeyboard;
        private TouchDialogBanner touchDialogBanner;
        private System.Windows.Forms.Label lblDescription;
        private MSRTextBoxTouch tbDescription;
        private ShadeNumericTextBox ntbAmount;
        private System.Windows.Forms.Label lblAmount;
        private TouchButton btnOk;
        private TouchButton btnCancel;
        private ShadeNumericTextBox ntbAddedAmount;
        private System.Windows.Forms.Label lblAddedAmount;
    }
}