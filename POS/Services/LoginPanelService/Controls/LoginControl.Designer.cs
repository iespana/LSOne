using LSOne.Controls;

namespace LSOne.Services.LoginPanel.Controls
{
    partial class LoginControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginControl));
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.demoLinkLabel = new System.Windows.Forms.LinkLabel();
            this.pnlError = new System.Windows.Forms.Panel();
            this.lblError = new System.Windows.Forms.Label();
            this.pbError = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSwitchLoginMethod = new LSOne.Controls.TouchButton();
            this.tbPassword = new LSOne.Controls.ShadeTextBoxTouch();
            this.tbLogin = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.btnLogin = new LSOne.Controls.TouchButton();
            this.pnlError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLogin
            // 
            resources.ApplyResources(this.lblLogin, "lblLogin");
            this.lblLogin.BackColor = System.Drawing.Color.Transparent;
            this.lblLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblLogin.Name = "lblLogin";
            // 
            // lblPassword
            // 
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPassword.Name = "lblPassword";
            // 
            // demoLinkLabel
            // 
            resources.ApplyResources(this.demoLinkLabel, "demoLinkLabel");
            this.demoLinkLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.demoLinkLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.demoLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.demoLinkLabel.Name = "demoLinkLabel";
            this.demoLinkLabel.TabStop = true;
            this.demoLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.demoLinkLabel_LinkClicked);
            // 
            // pnlError
            // 
            resources.ApplyResources(this.pnlError, "pnlError");
            this.pnlError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.pnlError.Controls.Add(this.lblError);
            this.pnlError.Controls.Add(this.demoLinkLabel);
            this.pnlError.Controls.Add(this.pbError);
            this.pnlError.Name = "pnlError";
            // 
            // lblError
            // 
            resources.ApplyResources(this.lblError, "lblError");
            this.lblError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.lblError.Name = "lblError";
            // 
            // pbError
            // 
            this.pbError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.pbError.BackgroundImage = global::LSOne.Services.LoginPanel.Properties.Resources.Warning_red_24px;
            resources.ApplyResources(this.pbError, "pbError");
            this.pbError.Name = "pbError";
            this.pbError.TabStop = false;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackgroundImage = global::LSOne.Services.LoginPanel.Properties.Resources.LS_One_logo_180px;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // btnSwitchLoginMethod
            // 
            resources.ApplyResources(this.btnSwitchLoginMethod, "btnSwitchLoginMethod");
            this.btnSwitchLoginMethod.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnSwitchLoginMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.btnSwitchLoginMethod.BackgroundImage = global::LSOne.Services.LoginPanel.Properties.Resources.Login_48;
            this.btnSwitchLoginMethod.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Normal;
            this.btnSwitchLoginMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.btnSwitchLoginMethod.Image = global::LSOne.Services.LoginPanel.Properties.Resources.card_login_grey_24;
            this.btnSwitchLoginMethod.Name = "btnSwitchLoginMethod";
            this.btnSwitchLoginMethod.TabStop = false;
            this.btnSwitchLoginMethod.UseVisualStyleBackColor = false;
            this.btnSwitchLoginMethod.Click += new System.EventHandler(this.btnSwitchLoginMethod_Click);
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.tbPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbPassword.MaxLength = 32767;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '●';
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.Enter += new System.EventHandler(this.tbPassword_Enter);
            this.tbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPassword_KeyDown);
            this.tbPassword.Leave += new System.EventHandler(this.tbPassword_Leave);
            this.tbPassword.TextChanged += new System.EventHandler(this.tbLogin_TextChanged);
            // 
            // tbLogin
            // 
            resources.ApplyResources(this.tbLogin, "tbLogin");
            this.tbLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.tbLogin.EndCharacter = null;
            this.tbLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbLogin.LastTrack = null;
            this.tbLogin.ManualEntryOfTrack = true;
            this.tbLogin.MaxLength = 32767;
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.NumericOnly = false;
            this.tbLogin.Seperator = null;
            this.tbLogin.StartCharacter = null;
            this.tbLogin.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbLogin.Enter += new System.EventHandler(this.tbLogin_Enter);
            this.tbLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbLogin_KeyDown);
            this.tbLogin.Leave += new System.EventHandler(this.tbLogin_Leave);
            this.tbLogin.TextChanged += new System.EventHandler(this.tbLogin_TextChanged);
            this.tbLogin.VisibleChanged += new System.EventHandler(this.tbLogin_VisibleChanged);
            // 
            // btnLogin
            // 
            resources.ApplyResources(this.btnLogin, "btnLogin");
            this.btnLogin.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnLogin.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // LoginControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pnlError);
            this.Controls.Add(this.btnSwitchLoginMethod);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.btnLogin);
            this.DoubleBuffered = true;
            this.Name = "LoginControl";
            this.Load += new System.EventHandler(this.LoginControl_Load);
            this.Enter += new System.EventHandler(this.LoginControl_Enter);
            this.Leave += new System.EventHandler(this.LoginControl_Leave);
            this.pnlError.ResumeLayout(false);
            this.pnlError.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.ShadeTextBoxTouch tbPassword;
        private MSRTextBoxTouch tbLogin;
        private LSOne.Controls.TouchButton btnLogin;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblPassword;
        private LSOne.Controls.TouchButton btnSwitchLoginMethod;
        private System.Windows.Forms.LinkLabel demoLinkLabel;
        private System.Windows.Forms.Panel pnlError;
        private System.Windows.Forms.PictureBox pbError;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
