using LSOne.Controls;
using LSOne.Controls.Columns;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.LoginPanel.Controls
{
    partial class LoginControlList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginControlList));
            this.lvUsers = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbPassword = new LSOne.Controls.ShadeTextBoxTouch();
            this.btnSwitchLoginMethod = new LSOne.Controls.TouchButton();
            this.btnLogin = new LSOne.Controls.TouchButton();
            this.pnlError = new System.Windows.Forms.Panel();
            this.lblError = new System.Windows.Forms.Label();
            this.demoLinkLabel = new System.Windows.Forms.LinkLabel();
            this.pbError = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lvUsers
            // 
            resources.ApplyResources(this.lvUsers, "lvUsers");
            this.lvUsers.ApplyVisualStyles = false;
            this.lvUsers.BackColor = System.Drawing.Color.White;
            this.lvUsers.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvUsers.BuddyControl = null;
            this.lvUsers.Columns.Add(this.column1);
            this.lvUsers.Columns.Add(this.column2);
            this.lvUsers.ContentBackColor = System.Drawing.Color.White;
            this.lvUsers.DefaultRowHeight = ((short)(50));
            this.lvUsers.EvenRowColor = System.Drawing.Color.White;
            this.lvUsers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvUsers.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvUsers.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvUsers.HeaderHeight = ((short)(30));
            this.lvUsers.HideHorizontalScrollbarWhenDisabled = true;
            this.lvUsers.HideVerticalScrollbarWhenDisabled = true;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.OddRowColor = System.Drawing.Color.White;
            this.lvUsers.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvUsers.RowLines = true;
            this.lvUsers.SecondarySortColumn = ((short)(-1));
            this.lvUsers.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvUsers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvUsers.SortSetting = "0:1";
            this.lvUsers.TouchScroll = true;
            this.lvUsers.UseFocusRectangle = false;
            this.lvUsers.VerticalScrollbarValue = 0;
            this.lvUsers.VerticalScrollbarYOffset = 0;
            this.lvUsers.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvUsers.SelectionChanged += new System.EventHandler(this.lvUsers_SelectionChanged);
            this.lvUsers.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvUsers_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(50));
            this.column1.RelativeSize = 30;
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Sizable = false;
            this.column1.Tag = null;
            this.column1.Width = ((short)(150));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.Clickable = false;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(700));
            this.column2.RelativeSize = 70;
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Sizable = false;
            this.column2.Tag = null;
            this.column2.Width = ((short)(700));
            // 
            // lblPassword
            // 
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPassword.Name = "lblPassword";
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
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
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
            this.pictureBox1.BackgroundImage = global::LSOne.Services.LoginPanel.Properties.Resources.LS_One_logo_120px;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.label1.Name = "label1";
            // 
            // LoginControlList
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pnlError);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnSwitchLoginMethod);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lvUsers);
            this.DoubleBuffered = true;
            this.Name = "LoginControlList";
            this.Load += new System.EventHandler(this.LoginControl_Load);
            this.Enter += new System.EventHandler(this.LoginControlList_Enter);
            this.Leave += new System.EventHandler(this.LoginControlList_Leave);
            this.pnlError.ResumeLayout(false);
            this.pnlError.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ListView lvUsers;
        private Column column1;
        private Column column2;
        private System.Windows.Forms.Label lblPassword;
        private ShadeTextBoxTouch tbPassword;
        private TouchButton btnSwitchLoginMethod;
        private TouchButton btnLogin;
        private System.Windows.Forms.Panel pnlError;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.LinkLabel demoLinkLabel;
        private System.Windows.Forms.PictureBox pbError;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
    }
}
