namespace LSOne.ViewPlugins.Profiles.ViewPages.User
{
    partial class UserProfileUsersPage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserProfileUsersPage));
			this.lvUsers = new LSOne.Controls.ListView();
			this.clmUsername = new LSOne.Controls.Columns.Column();
			this.clmLogin = new LSOne.Controls.Columns.Column();
			this.clmUserGroups = new LSOne.Controls.Columns.Column();
			this.clmNameOnReceipt = new LSOne.Controls.Columns.Column();
			this.clmEnabled = new LSOne.Controls.Columns.Column();
			this.btnAdd = new LSOne.Controls.ContextButton();
			this.SuspendLayout();
			// 
			// lvUsers
			// 
			resources.ApplyResources(this.lvUsers, "lvUsers");
			this.lvUsers.BorderColor = System.Drawing.Color.DarkGray;
			this.lvUsers.BuddyControl = null;
			this.lvUsers.Columns.Add(this.clmUsername);
			this.lvUsers.Columns.Add(this.clmLogin);
			this.lvUsers.Columns.Add(this.clmUserGroups);
			this.lvUsers.Columns.Add(this.clmNameOnReceipt);
			this.lvUsers.Columns.Add(this.clmEnabled);
			this.lvUsers.ContentBackColor = System.Drawing.Color.White;
			this.lvUsers.DefaultRowHeight = ((short)(22));
			this.lvUsers.DimSelectionWhenDisabled = true;
			this.lvUsers.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
			this.lvUsers.HeaderBackColor = System.Drawing.Color.White;
			this.lvUsers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvUsers.HeaderHeight = ((short)(25));
			this.lvUsers.Name = "lvUsers";
			this.lvUsers.OddRowColor = System.Drawing.Color.White;
			this.lvUsers.RowLineColor = System.Drawing.Color.LightGray;
			this.lvUsers.SecondarySortColumn = ((short)(-1));
			this.lvUsers.SelectedRowColor = System.Drawing.Color.LightGray;
			this.lvUsers.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
			this.lvUsers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
			this.lvUsers.SortSetting = "0:1";
			this.lvUsers.VerticalScrollbarValue = 0;
			this.lvUsers.VerticalScrollbarYOffset = 0;
			// 
			// clmUsername
			// 
			this.clmUsername.AutoSize = true;
			this.clmUsername.DefaultStyle = null;
			resources.ApplyResources(this.clmUsername, "clmUsername");
			this.clmUsername.InternalSort = true;
			this.clmUsername.MaximumWidth = ((short)(0));
			this.clmUsername.MinimumWidth = ((short)(10));
			this.clmUsername.SecondarySortColumn = ((short)(-1));
			this.clmUsername.Tag = null;
			this.clmUsername.Width = ((short)(50));
			// 
			// clmLogin
			// 
			this.clmLogin.AutoSize = true;
			this.clmLogin.DefaultStyle = null;
			resources.ApplyResources(this.clmLogin, "clmLogin");
			this.clmLogin.InternalSort = true;
			this.clmLogin.MaximumWidth = ((short)(0));
			this.clmLogin.MinimumWidth = ((short)(10));
			this.clmLogin.SecondarySortColumn = ((short)(-1));
			this.clmLogin.Tag = null;
			this.clmLogin.Width = ((short)(50));
			// 
			// clmUserGroups
			// 
			this.clmUserGroups.AutoSize = true;
			this.clmUserGroups.DefaultStyle = null;
			resources.ApplyResources(this.clmUserGroups, "clmUserGroups");
			this.clmUserGroups.InternalSort = true;
			this.clmUserGroups.MaximumWidth = ((short)(0));
			this.clmUserGroups.MinimumWidth = ((short)(10));
			this.clmUserGroups.SecondarySortColumn = ((short)(-1));
			this.clmUserGroups.Tag = null;
			this.clmUserGroups.Width = ((short)(50));
			// 
			// clmNameOnReceipt
			// 
			this.clmNameOnReceipt.AutoSize = true;
			this.clmNameOnReceipt.DefaultStyle = null;
			resources.ApplyResources(this.clmNameOnReceipt, "clmNameOnReceipt");
			this.clmNameOnReceipt.InternalSort = true;
			this.clmNameOnReceipt.MaximumWidth = ((short)(0));
			this.clmNameOnReceipt.MinimumWidth = ((short)(10));
			this.clmNameOnReceipt.SecondarySortColumn = ((short)(-1));
			this.clmNameOnReceipt.Tag = null;
			this.clmNameOnReceipt.Width = ((short)(50));
			// 
			// clmEnabled
			// 
			this.clmEnabled.AutoSize = true;
			this.clmEnabled.DefaultStyle = null;
			resources.ApplyResources(this.clmEnabled, "clmEnabled");
			this.clmEnabled.MaximumWidth = ((short)(0));
			this.clmEnabled.MinimumWidth = ((short)(10));
			this.clmEnabled.SecondarySortColumn = ((short)(-1));
			this.clmEnabled.Tag = null;
			this.clmEnabled.Width = ((short)(50));
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.BackColor = System.Drawing.Color.Transparent;
			this.btnAdd.Context = LSOne.Controls.ButtonType.Add;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Click += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
			// 
			// UserProfileUsersPage
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lvUsers);
			this.DoubleBuffered = true;
			this.Name = "UserProfileUsersPage";
			this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvUsers;
        private Controls.Columns.Column clmUsername;
        private Controls.Columns.Column clmLogin;
        private Controls.Columns.Column clmUserGroups;
        private Controls.Columns.Column clmNameOnReceipt;
        private Controls.Columns.Column clmEnabled;
		private Controls.ContextButton btnAdd;
	}
}
