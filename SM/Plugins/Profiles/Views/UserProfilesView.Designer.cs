namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class UserProfilesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserProfilesView));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.lvUserProfiles = new LSOne.Controls.ListView();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.clmVisualProfile = new LSOne.Controls.Columns.Column();
            this.clmLayout = new LSOne.Controls.Columns.Column();
            this.clmLanguage = new LSOne.Controls.Columns.Column();
            this.clmKeyboard = new LSOne.Controls.Columns.Column();
            this.clmDiscount = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvUserProfiles);
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnsContextButtons_RemoveButtonClicked);
            // 
            // searchBar
            // 
            resources.ApplyResources(this.searchBar, "searchBar");
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.Name = "searchBar";
            this.searchBar.SearchOptionEnabled = true;
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            this.searchBar.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar_UnknownControlAdd);
            this.searchBar.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar_UnknownControlRemove);
            this.searchBar.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar_UnknownControlHasSelection);
            this.searchBar.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar_UnknownControlGetSelection);
            this.searchBar.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar_UnknownControlSetSelection);
            this.searchBar.SearchOptionChanged += new System.EventHandler(this.searchBar_SearchOptionChanged);
            // 
            // lvUserProfiles
            // 
            resources.ApplyResources(this.lvUserProfiles, "lvUserProfiles");
            this.lvUserProfiles.BuddyControl = null;
            this.lvUserProfiles.Columns.Add(this.clmDescription);
            this.lvUserProfiles.Columns.Add(this.clmStore);
            this.lvUserProfiles.Columns.Add(this.clmVisualProfile);
            this.lvUserProfiles.Columns.Add(this.clmLayout);
            this.lvUserProfiles.Columns.Add(this.clmLanguage);
            this.lvUserProfiles.Columns.Add(this.clmKeyboard);
            this.lvUserProfiles.Columns.Add(this.clmDiscount);
            this.lvUserProfiles.ContentBackColor = System.Drawing.Color.White;
            this.lvUserProfiles.DefaultRowHeight = ((short)(22));
            this.lvUserProfiles.DimSelectionWhenDisabled = true;
            this.lvUserProfiles.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvUserProfiles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvUserProfiles.HeaderHeight = ((short)(25));
            this.lvUserProfiles.Name = "lvUserProfiles";
            this.lvUserProfiles.OddRowColor = System.Drawing.Color.White;
            this.lvUserProfiles.RowLineColor = System.Drawing.Color.LightGray;
            this.lvUserProfiles.SecondarySortColumn = ((short)(-1));
            this.lvUserProfiles.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvUserProfiles.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvUserProfiles.SortSetting = "0:1";
            this.lvUserProfiles.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvUserProfiles_HeaderClicked);
            this.lvUserProfiles.SelectionChanged += new System.EventHandler(this.lvUserProfiles_SelectionChanged);
            this.lvUserProfiles.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvUserProfiles_RowDoubleClick);
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(50));
            // 
            // clmStore
            // 
            this.clmStore.AutoSize = true;
            this.clmStore.DefaultStyle = null;
            resources.ApplyResources(this.clmStore, "clmStore");
            this.clmStore.MaximumWidth = ((short)(0));
            this.clmStore.MinimumWidth = ((short)(10));
            this.clmStore.SecondarySortColumn = ((short)(-1));
            this.clmStore.Tag = null;
            this.clmStore.Width = ((short)(50));
            // 
            // clmVisualProfile
            // 
            this.clmVisualProfile.AutoSize = true;
            this.clmVisualProfile.DefaultStyle = null;
            resources.ApplyResources(this.clmVisualProfile, "clmVisualProfile");
            this.clmVisualProfile.MaximumWidth = ((short)(0));
            this.clmVisualProfile.MinimumWidth = ((short)(10));
            this.clmVisualProfile.SecondarySortColumn = ((short)(-1));
            this.clmVisualProfile.Tag = null;
            this.clmVisualProfile.Width = ((short)(50));
            // 
            // clmLayout
            // 
            this.clmLayout.AutoSize = true;
            this.clmLayout.DefaultStyle = null;
            resources.ApplyResources(this.clmLayout, "clmLayout");
            this.clmLayout.MaximumWidth = ((short)(0));
            this.clmLayout.MinimumWidth = ((short)(10));
            this.clmLayout.SecondarySortColumn = ((short)(-1));
            this.clmLayout.Tag = null;
            this.clmLayout.Width = ((short)(50));
            // 
            // clmLanguage
            // 
            this.clmLanguage.AutoSize = true;
            this.clmLanguage.DefaultStyle = null;
            resources.ApplyResources(this.clmLanguage, "clmLanguage");
            this.clmLanguage.MaximumWidth = ((short)(0));
            this.clmLanguage.MinimumWidth = ((short)(10));
            this.clmLanguage.SecondarySortColumn = ((short)(-1));
            this.clmLanguage.Tag = null;
            this.clmLanguage.Width = ((short)(50));
            // 
            // clmKeyboard
            // 
            this.clmKeyboard.AutoSize = true;
            this.clmKeyboard.DefaultStyle = null;
            resources.ApplyResources(this.clmKeyboard, "clmKeyboard");
            this.clmKeyboard.MaximumWidth = ((short)(0));
            this.clmKeyboard.MinimumWidth = ((short)(10));
            this.clmKeyboard.SecondarySortColumn = ((short)(-1));
            this.clmKeyboard.Tag = null;
            this.clmKeyboard.Width = ((short)(50));
            // 
            // clmDiscount
            // 
            this.clmDiscount.AutoSize = true;
            this.clmDiscount.Clickable = false;
            this.clmDiscount.DefaultStyle = null;
            resources.ApplyResources(this.clmDiscount, "clmDiscount");
            this.clmDiscount.MaximumWidth = ((short)(0));
            this.clmDiscount.MinimumWidth = ((short)(10));
            this.clmDiscount.SecondarySortColumn = ((short)(-1));
            this.clmDiscount.Tag = null;
            this.clmDiscount.Width = ((short)(50));
            // 
            // UserProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UserProfilesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ContextButtons btnsContextButtons;
        private Controls.SearchBar searchBar;
        private Controls.ListView lvUserProfiles;
        private Controls.Columns.Column clmDescription;
        private Controls.Columns.Column clmStore;
        private Controls.Columns.Column clmVisualProfile;
        private Controls.Columns.Column clmLayout;
        private Controls.Columns.Column clmLanguage;
        private Controls.Columns.Column clmKeyboard;
        private Controls.Columns.Column clmDiscount;
    }
}
