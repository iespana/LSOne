using LSOne.Controls;
using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.User.Views
{
    partial class User
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(User));
			this.lblLoginName = new System.Windows.Forms.Label();
			this.tbLoginName = new System.Windows.Forms.TextBox();
			this.chkIsDomainUser = new System.Windows.Forms.CheckBox();
			this.chkUserIsDisabled = new System.Windows.Forms.CheckBox();
			this.userName = new LSOne.Controls.FullNameControl();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlBottom.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.flowLayoutPanel1);
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.tbLoginName);
			this.pnlBottom.Controls.Add(this.lblLoginName);
			this.pnlBottom.Controls.Add(this.userName);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			// 
			// lblLoginName
			// 
			this.lblLoginName.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblLoginName, "lblLoginName");
			this.lblLoginName.Name = "lblLoginName";
			// 
			// tbLoginName
			// 
			resources.ApplyResources(this.tbLoginName, "tbLoginName");
			this.tbLoginName.Name = "tbLoginName";
			this.tbLoginName.ReadOnly = true;
			this.tbLoginName.BackColor = ColorPalette.BackgroundColor;
			this.tbLoginName.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// chkIsDomainUser
			// 
			resources.ApplyResources(this.chkIsDomainUser, "chkIsDomainUser");
			this.chkIsDomainUser.BackColor = System.Drawing.Color.Transparent;
			this.chkIsDomainUser.Name = "chkIsDomainUser";
			this.chkIsDomainUser.UseVisualStyleBackColor = false;
			// 
			// chkUserIsDisabled
			// 
			resources.ApplyResources(this.chkUserIsDisabled, "chkUserIsDisabled");
			this.chkUserIsDisabled.BackColor = System.Drawing.Color.Transparent;
			this.chkUserIsDisabled.Name = "chkUserIsDisabled";
			this.chkUserIsDisabled.UseVisualStyleBackColor = false;
			// 
			// userName
			// 
			this.userName.Alias = "";
			resources.ApplyResources(this.userName, "userName");
			this.userName.BackColor = System.Drawing.Color.Transparent;
			this.userName.FirstName = "";
			this.userName.LastName = "";
			this.userName.MiddleName = "";
			this.userName.Name = "userName";
			this.userName.Prefix = "";
			this.userName.ShowAlias = false;
			this.userName.Suffix = "";
			// 
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			// 
			// flowLayoutPanel1
			// 
			resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
			this.flowLayoutPanel1.Controls.Add(this.chkIsDomainUser);
			this.flowLayoutPanel1.Controls.Add(this.chkUserIsDisabled);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			// 
			// User
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 72;
			this.Name = "User";
			this.SizeChanged += new System.EventHandler(this.User_SizeChanged);
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private FullNameControl userName;
        private System.Windows.Forms.TextBox tbLoginName;
        private System.Windows.Forms.Label lblLoginName;
        private System.Windows.Forms.CheckBox chkIsDomainUser;
        private System.Windows.Forms.CheckBox chkUserIsDisabled;
        private TabControl tabSheetTabs;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
