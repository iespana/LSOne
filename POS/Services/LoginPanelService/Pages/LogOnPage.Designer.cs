using LSOne.Controls;
using LSOne.Services.LoginPanel.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.LoginPanel.Pages
{
    partial class LogOnPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogOnPage));
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.loginControl = new LSOne.Services.LoginPanel.Controls.LoginControl();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInfo
            // 
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
            this.pnlInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(173)))), ((int)(((byte)(65)))));
            this.pnlInfo.Controls.Add(this.lblInfo);
            this.pnlInfo.Name = "pnlInfo";
            // 
            // lblInfo
            // 
            resources.ApplyResources(this.lblInfo, "lblInfo");
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.lblInfo.Name = "lblInfo";
            // 
            // lblVersion
            // 
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblVersion.Name = "lblVersion";
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
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // loginControl
            // 
            resources.ApplyResources(this.loginControl, "loginControl");
            this.loginControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.loginControl.ErrorCode = LSOne.DataLayer.GenericConnector.Enums.LoginResult.Success;
            this.loginControl.ErrorDisplayType = LSOne.Services.Interfaces.LicenseErrorDisplayType.NoError;
            this.loginControl.ExpiryDate = null;
            this.loginControl.Login = "";
            this.loginControl.Name = "loginControl";
            this.loginControl.OnLogin += new System.EventHandler(this.loginControl_OnLogin);
            this.loginControl.OnTokenLogin += new System.EventHandler(this.loginControl_OnTokenLogin);
            this.loginControl.Paint += new System.Windows.Forms.PaintEventHandler(this.loginControl_Paint);
            // 
            // LogOnPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.loginControl);
            this.Name = "LogOnPage";
            this.Load += new System.EventHandler(this.LogOnPage_Load);
            this.pnlInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LoginControl loginControl;
        private TouchKeyboard touchKeyboard;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblVersion;
    }
}
