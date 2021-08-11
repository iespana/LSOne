﻿using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.LoginPanel.WinFormsTouch
{
    partial class PermissionOverrideDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermissionOverrideDialog));
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.tbPassword = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbUser = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnSwitchLoginMethod = new LSOne.Controls.TouchButton();
            this.SuspendLayout();
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
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.btnOK_Click);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.BackColor = System.Drawing.Color.White;
            this.tbPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbPassword.MaxLength = 32767;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '●';
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.Enter += new System.EventHandler(this.tbPassword_Enter);
            this.tbPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPassword_KeyPress);
            this.tbPassword.Leave += new System.EventHandler(this.tbPassword_Leave);
            this.tbPassword.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbUser
            // 
            resources.ApplyResources(this.tbUser, "tbUser");
            this.tbUser.BackColor = System.Drawing.Color.White;
            this.tbUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbUser.MaxLength = 32767;
            this.tbUser.Name = "tbUser";
            this.tbUser.Enter += new System.EventHandler(this.tbUser_Enter);
            this.tbUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUser_KeyDown);
            this.tbUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbUser_KeyPress);
            this.tbUser.Leave += new System.EventHandler(this.tbUser_Leave);
            this.tbUser.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblPassword
            // 
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.Name = "lblPassword";
            // 
            // lblUser
            // 
            resources.ApplyResources(this.lblUser, "lblUser");
            this.lblUser.Name = "lblUser";
            // 
            // btnSwitchLoginMethod
            // 
            resources.ApplyResources(this.btnSwitchLoginMethod, "btnSwitchLoginMethod");
            this.btnSwitchLoginMethod.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSwitchLoginMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnSwitchLoginMethod.BackgroundImage = global::LSOne.Services.LoginPanel.Properties.Resources.Login_48;
            this.btnSwitchLoginMethod.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnSwitchLoginMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnSwitchLoginMethod.Name = "btnSwitchLoginMethod";
            this.btnSwitchLoginMethod.UseVisualStyleBackColor = false;
            this.btnSwitchLoginMethod.Click += new System.EventHandler(this.btnSwitchLoginMethod_Click);
            // 
            // PermissionOverrideDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnSwitchLoginMethod);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.touchKeyboard);
            this.Name = "PermissionOverrideDialog";
            this.Load += new System.EventHandler(this.PermissionOverrideDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.TouchButton btnCancel;
        private LSOne.Controls.TouchButton btnOK;
        private TouchKeyboard touchKeyboard;
        private TouchDialogBanner touchDialogBanner1;
        private LSOne.Controls.ShadeTextBoxTouch tbPassword;
        private LSOne.Controls.ShadeTextBoxTouch tbUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUser;
        private LSOne.Controls.TouchButton btnSwitchLoginMethod;

    }
}