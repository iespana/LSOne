using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.LoginPanel.WinFormsTouch
{
    partial class ChangePasswordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePasswordDialog));
            this.lblPrompt = new System.Windows.Forms.Label();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.btnChange = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.tbConfirmPassword = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbNewPassword = new LSOne.Controls.ShadeTextBoxTouch();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.tbOldPassword = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblOldPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            resources.ApplyResources(this.lblPrompt, "lblPrompt");
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Tag = "H2";
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // btnChange
            // 
            resources.ApplyResources(this.btnChange, "btnChange");
            this.btnChange.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnChange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnChange.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnChange.ForeColor = System.Drawing.Color.White;
            this.btnChange.Name = "btnChange";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
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
            // tbConfirmPassword
            // 
            this.tbConfirmPassword.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbConfirmPassword, "tbConfirmPassword");
            this.tbConfirmPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbConfirmPassword.MaxLength = 32767;
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.PasswordChar = '●';
            this.tbConfirmPassword.UseSystemPasswordChar = true;
            this.tbConfirmPassword.Enter += new System.EventHandler(this.tbConfirmPassword_Enter);
            this.tbConfirmPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChangePasswordDialog_KeyPress);
            this.tbConfirmPassword.Leave += new System.EventHandler(this.tbConfirmPassword_Leave);
            this.tbConfirmPassword.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbNewPassword
            // 
            this.tbNewPassword.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbNewPassword, "tbNewPassword");
            this.tbNewPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbNewPassword.MaxLength = 32767;
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.PasswordChar = '●';
            this.tbNewPassword.UseSystemPasswordChar = true;
            this.tbNewPassword.Enter += new System.EventHandler(this.tbNewPassword_Enter);
            this.tbNewPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChangePasswordDialog_KeyPress);
            this.tbNewPassword.Leave += new System.EventHandler(this.tbNewPassword_Leave);
            this.tbNewPassword.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblNewPassword
            // 
            resources.ApplyResources(this.lblNewPassword, "lblNewPassword");
            this.lblNewPassword.Name = "lblNewPassword";
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // tbOldPassword
            // 
            this.tbOldPassword.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbOldPassword, "tbOldPassword");
            this.tbOldPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbOldPassword.MaxLength = 32767;
            this.tbOldPassword.Name = "tbOldPassword";
            this.tbOldPassword.PasswordChar = '●';
            this.tbOldPassword.UseSystemPasswordChar = true;
            this.tbOldPassword.Enter += new System.EventHandler(this.tbOldPassword_Enter);
            this.tbOldPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChangePasswordDialog_KeyPress);
            this.tbOldPassword.Leave += new System.EventHandler(this.tbOldPassword_Leave);
            // 
            // lblOldPassword
            // 
            resources.ApplyResources(this.lblOldPassword, "lblOldPassword");
            this.lblOldPassword.Name = "lblOldPassword";
            // 
            // ChangePasswordDialog
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.tbOldPassword);
            this.Controls.Add(this.lblOldPassword);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.tbConfirmPassword);
            this.Controls.Add(this.tbNewPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "ChangePasswordDialog";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChangePasswordDialog_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPrompt;
        private TouchKeyboard touchKeyboard;
        private LSOne.Controls.TouchButton btnChange;
        private LSOne.Controls.TouchButton btnCancel;
        private LSOne.Controls.ShadeTextBoxTouch tbConfirmPassword;
        private LSOne.Controls.ShadeTextBoxTouch tbNewPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNewPassword;
        private TouchDialogBanner touchDialogBanner1;
        private LSOne.Controls.ShadeTextBoxTouch tbOldPassword;
        private System.Windows.Forms.Label lblOldPassword;

    }
}