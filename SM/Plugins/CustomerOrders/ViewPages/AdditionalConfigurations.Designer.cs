namespace LSOne.ViewPlugins.CustomerOrders.ViewPages
{
    partial class AdditionalConfigurations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdditionalConfigurations));
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colType = new LSOne.Controls.Columns.Column();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvAdditionalConfig = new LSOne.Controls.ListView();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(150));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(250));
            // 
            // colType
            // 
            this.colType.AutoSize = true;
            this.colType.DefaultStyle = null;
            resources.ApplyResources(this.colType, "colType");
            this.colType.InternalSort = true;
            this.colType.MaximumWidth = ((short)(0));
            this.colType.MinimumWidth = ((short)(150));
            this.colType.Tag = null;
            this.colType.Width = ((short)(250));
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.btnsEditAddRemove);
            this.groupPanel1.Controls.Add(this.lvAdditionalConfig);
            this.groupPanel1.Controls.Add(this.searchBar);
            this.groupPanel1.Name = "groupPanel1";
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
            // lvAdditionalConfig
            // 
            resources.ApplyResources(this.lvAdditionalConfig, "lvAdditionalConfig");
            this.lvAdditionalConfig.BuddyControl = null;
            this.lvAdditionalConfig.Columns.Add(this.colDescription);
            this.lvAdditionalConfig.Columns.Add(this.colType);
            this.lvAdditionalConfig.ContentBackColor = System.Drawing.Color.White;
            this.lvAdditionalConfig.DefaultRowHeight = ((short)(22));
            this.lvAdditionalConfig.DimSelectionWhenDisabled = true;
            this.lvAdditionalConfig.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvAdditionalConfig.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAdditionalConfig.HeaderHeight = ((short)(25));
            this.lvAdditionalConfig.HorizontalScrollbar = true;
            this.lvAdditionalConfig.Name = "lvAdditionalConfig";
            this.lvAdditionalConfig.OddRowColor = System.Drawing.Color.White;
            this.lvAdditionalConfig.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAdditionalConfig.SecondarySortColumn = ((short)(-1));
            this.lvAdditionalConfig.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvAdditionalConfig.SortSetting = "0:1";
            this.lvAdditionalConfig.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvAdditionalConfig_HeaderClicked);
            this.lvAdditionalConfig.SelectionChanged += new System.EventHandler(this.lvAdditionalConfig_SelectionChanged);
            this.lvAdditionalConfig.DoubleClick += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
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
            // 
            // AdditionalConfigurations
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupPanel1);
            this.Name = "AdditionalConfigurations";
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        
        private Controls.ContextButtons btnsEditAddRemove;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colType;
        private Controls.GroupPanel groupPanel1;
        private Controls.SearchBar searchBar;
        private Controls.ListView lvAdditionalConfig;
    }
}
