using LSOne.Controls;

namespace LSOne.Services.LoginPanel.Pages
{
    partial class AuthorizeOperationPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthorizeOperationPage));
            this.btnOK = new LSOne.Controls.TouchButton();
            this.tbPassword = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbUser = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnSwitchLoginMethod = new LSOne.Controls.TouchButton();
            this.pnlAuthorize = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.pnlAuthorize.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            resources.ApplyResources(this.tbPassword, "tbPassword");
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
            this.tbUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            resources.ApplyResources(this.tbUser, "tbUser");
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
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPassword.Name = "lblPassword";
            // 
            // lblUser
            // 
            resources.ApplyResources(this.lblUser, "lblUser");
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblUser.Name = "lblUser";
            // 
            // btnSwitchLoginMethod
            // 
            this.btnSwitchLoginMethod.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSwitchLoginMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnSwitchLoginMethod.BackgroundImage = global::LSOne.Services.LoginPanel.Properties.Resources.Login_48;
            this.btnSwitchLoginMethod.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            resources.ApplyResources(this.btnSwitchLoginMethod, "btnSwitchLoginMethod");
            this.btnSwitchLoginMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnSwitchLoginMethod.Name = "btnSwitchLoginMethod";
            this.btnSwitchLoginMethod.TabStop = false;
            this.btnSwitchLoginMethod.UseVisualStyleBackColor = false;
            this.btnSwitchLoginMethod.Click += new System.EventHandler(this.btnSwitchLoginMethod_Click);
            // 
            // pnlAuthorize
            // 
            this.pnlAuthorize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.pnlAuthorize.Controls.Add(this.lblHeader);
            this.pnlAuthorize.Controls.Add(this.lblNote);
            this.pnlAuthorize.Controls.Add(this.btnSwitchLoginMethod);
            this.pnlAuthorize.Controls.Add(this.btnOK);
            this.pnlAuthorize.Controls.Add(this.lblUser);
            this.pnlAuthorize.Controls.Add(this.tbPassword);
            this.pnlAuthorize.Controls.Add(this.lblPassword);
            this.pnlAuthorize.Controls.Add(this.tbUser);
            resources.ApplyResources(this.pnlAuthorize, "pnlAuthorize");
            this.pnlAuthorize.Name = "pnlAuthorize";
            this.pnlAuthorize.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlAuthorize_Paint);
            // 
            // lblHeader
            // 
            resources.ApplyResources(this.lblHeader, "lblHeader");
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblHeader.Name = "lblHeader";
            // 
            // lblNote
            // 
            resources.ApplyResources(this.lblNote, "lblNote");
            this.lblNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblNote.Name = "lblNote";
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.SendEnterAsKeyStroke = true;
            this.touchKeyboard.TabStop = false;
            // 
            // AuthorizeOperationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.pnlAuthorize);
            this.DoubleBuffered = true;
            this.Name = "AuthorizeOperationPage";
            this.Enter += new System.EventHandler(this.AuthorizeOperationPage_Enter);
            this.Leave += new System.EventHandler(this.AuthorizeOperationPage_Leave);
            this.pnlAuthorize.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private LSOne.Controls.TouchButton btnOK;
        private LSOne.Controls.ShadeTextBoxTouch tbPassword;
        private LSOne.Controls.ShadeTextBoxTouch tbUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUser;
        private LSOne.Controls.TouchButton btnSwitchLoginMethod;
        private System.Windows.Forms.Panel pnlAuthorize;
        private TouchKeyboard touchKeyboard;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblHeader;
    }
}
