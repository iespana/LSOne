using LSOne.Controls;

namespace LSOne.ViewPlugins.User.Views
{
    partial class UsersView
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
                searchTimer.Tick -= SearchTimerOnTick;
                searchTimer.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsersView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.lvUsers = new LSOne.Controls.ListView();
            this.colUserName = new LSOne.Controls.Columns.Column();
            this.colLogin = new LSOne.Controls.Columns.Column();
            this.colUserGroups = new LSOne.Controls.Columns.Column();
            this.colNameOnReceipt = new LSOne.Controls.Columns.Column();
            this.colEmail = new LSOne.Controls.Columns.Column();
            this.colUserProfile = new LSOne.Controls.Columns.Column();
            this.colEnabled = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvUsers);
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SearchOptionEnabled = true;
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.SaveAsDefault += new System.EventHandler(this.searchBar1_SaveAsDefault);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            this.searchBar1.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar1_UnknownControlAdd);
            this.searchBar1.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar1_UnknownControlHasSelection);
            this.searchBar1.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar1_UnknownControlGetSelection);
            // 
            // lvUsers
            // 
            resources.ApplyResources(this.lvUsers, "lvUsers");
            this.lvUsers.BuddyControl = null;
            this.lvUsers.Columns.Add(this.colUserName);
            this.lvUsers.Columns.Add(this.colLogin);
            this.lvUsers.Columns.Add(this.colUserGroups);
            this.lvUsers.Columns.Add(this.colNameOnReceipt);
            this.lvUsers.Columns.Add(this.colEmail);
            this.lvUsers.Columns.Add(this.colUserProfile);
            this.lvUsers.Columns.Add(this.colEnabled);
            this.lvUsers.ContentBackColor = System.Drawing.Color.White;
            this.lvUsers.DefaultRowHeight = ((short)(22));
            this.lvUsers.DimSelectionWhenDisabled = true;
            this.lvUsers.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvUsers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvUsers.HeaderHeight = ((short)(25));
            this.lvUsers.HorizontalScrollbar = true;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.OddRowColor = System.Drawing.Color.White;
            this.lvUsers.RowLineColor = System.Drawing.Color.LightGray;
            this.lvUsers.SecondarySortColumn = ((short)(-1));
            this.lvUsers.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvUsers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvUsers.SortSetting = "0:1";
            this.lvUsers.SelectionChanged += new System.EventHandler(this.lvUsers_SelectedIndexChanged);
            this.lvUsers.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvUsers_RowDoubleClick);
            // 
            // colUserName
            // 
            this.colUserName.AutoSize = true;
            this.colUserName.DefaultStyle = null;
            resources.ApplyResources(this.colUserName, "colUserName");
            this.colUserName.InternalSort = true;
            this.colUserName.MaximumWidth = ((short)(0));
            this.colUserName.MinimumWidth = ((short)(10));
            this.colUserName.SecondarySortColumn = ((short)(-1));
            this.colUserName.Tag = null;
            this.colUserName.Width = ((short)(50));
            // 
            // colLogin
            // 
            this.colLogin.AutoSize = true;
            this.colLogin.DefaultStyle = null;
            resources.ApplyResources(this.colLogin, "colLogin");
            this.colLogin.InternalSort = true;
            this.colLogin.MaximumWidth = ((short)(0));
            this.colLogin.MinimumWidth = ((short)(10));
            this.colLogin.SecondarySortColumn = ((short)(-1));
            this.colLogin.Tag = null;
            this.colLogin.Width = ((short)(50));
            // 
            // colUserGroups
            // 
            this.colUserGroups.AutoSize = true;
            this.colUserGroups.DefaultStyle = null;
            resources.ApplyResources(this.colUserGroups, "colUserGroups");
            this.colUserGroups.InternalSort = true;
            this.colUserGroups.MaximumWidth = ((short)(0));
            this.colUserGroups.MinimumWidth = ((short)(10));
            this.colUserGroups.SecondarySortColumn = ((short)(-1));
            this.colUserGroups.Tag = null;
            this.colUserGroups.Width = ((short)(50));
            // 
            // colNameOnReceipt
            // 
            this.colNameOnReceipt.AutoSize = true;
            this.colNameOnReceipt.DefaultStyle = null;
            this.colNameOnReceipt.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colNameOnReceipt, "colNameOnReceipt");
            this.colNameOnReceipt.InternalSort = true;
            this.colNameOnReceipt.MaximumWidth = ((short)(0));
            this.colNameOnReceipt.MinimumWidth = ((short)(10));
            this.colNameOnReceipt.NoTextWhenSmall = true;
            this.colNameOnReceipt.SecondarySortColumn = ((short)(-1));
            this.colNameOnReceipt.Tag = null;
            this.colNameOnReceipt.Width = ((short)(50));
            // 
            // colEmail
            // 
            this.colEmail.AutoSize = true;
            this.colEmail.DefaultStyle = null;
            resources.ApplyResources(this.colEmail, "colEmail");
            this.colEmail.InternalSort = true;
            this.colEmail.MaximumWidth = ((short)(0));
            this.colEmail.MinimumWidth = ((short)(10));
            this.colEmail.SecondarySortColumn = ((short)(-1));
            this.colEmail.Tag = null;
            this.colEmail.Width = ((short)(50));
            // 
            // colUserProfile
            // 
            this.colUserProfile.AutoSize = true;
            this.colUserProfile.DefaultStyle = null;
            resources.ApplyResources(this.colUserProfile, "colUserProfile");
            this.colUserProfile.InternalSort = true;
            this.colUserProfile.MaximumWidth = ((short)(0));
            this.colUserProfile.MinimumWidth = ((short)(10));
            this.colUserProfile.SecondarySortColumn = ((short)(-1));
            this.colUserProfile.Tag = null;
            this.colUserProfile.Width = ((short)(50));
            // 
            // colEnabled
            // 
            this.colEnabled.AutoSize = true;
            this.colEnabled.DefaultStyle = null;
            resources.ApplyResources(this.colEnabled, "colEnabled");
            this.colEnabled.InternalSort = true;
            this.colEnabled.MaximumWidth = ((short)(0));
            this.colEnabled.MinimumWidth = ((short)(10));
            this.colEnabled.SecondarySortColumn = ((short)(-1));
            this.colEnabled.Tag = null;
            this.colEnabled.Width = ((short)(50));
            // 
            // UsersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UsersView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsEditAddRemove;
        private Controls.SearchBar searchBar1;
        private ListView lvUsers;
        private Controls.Columns.Column colUserName;
        private Controls.Columns.Column colLogin;
        private Controls.Columns.Column colEmail;
        private Controls.Columns.Column colUserGroups;
        private Controls.Columns.Column colEnabled;
        private Controls.Columns.Column colNameOnReceipt;
        private Controls.Columns.Column colUserProfile;
    }
}
