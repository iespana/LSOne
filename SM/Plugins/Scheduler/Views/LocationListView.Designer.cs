using LSOne.Controls;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class LocationListView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationListView));
            this.lblMembers = new System.Windows.Forms.Label();
            this.lblLocations = new System.Windows.Forms.Label();
            this.labelSynchronizing = new System.Windows.Forms.Label();
            this.lcMembers = new LSOne.ViewPlugins.Scheduler.Controls.LocationListControl();
            this.lcLocations = new LSOne.ViewPlugins.Scheduler.Controls.LocationListControl();
            this.contextButtonsLocations = new LSOne.Controls.ContextButtons();
            this.contextButtonsMembers = new LSOne.Controls.ContextButtons();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.contextButtonsMembers);
            this.pnlBottom.Controls.Add(this.contextButtonsLocations);
            this.pnlBottom.Controls.Add(this.lcLocations);
            this.pnlBottom.Controls.Add(this.lcMembers);
            this.pnlBottom.Controls.Add(this.labelSynchronizing);
            this.pnlBottom.Controls.Add(this.lblMembers);
            this.pnlBottom.Controls.Add(this.lblLocations);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // lblMembers
            // 
            resources.ApplyResources(this.lblMembers, "lblMembers");
            this.lblMembers.BackColor = System.Drawing.Color.Transparent;
            this.lblMembers.Name = "lblMembers";
            // 
            // lblLocations
            // 
            this.lblLocations.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLocations, "lblLocations");
            this.lblLocations.Name = "lblLocations";
            // 
            // labelSynchronizing
            // 
            resources.ApplyResources(this.labelSynchronizing, "labelSynchronizing");
            this.labelSynchronizing.BackColor = System.Drawing.Color.Transparent;
            this.labelSynchronizing.Name = "labelSynchronizing";
            // 
            // lcMembers
            // 
            resources.ApplyResources(this.lcMembers, "lcMembers");
            this.lcMembers.ContextMenuStrip = null;
            this.lcMembers.MultiSelect = false;
            this.lcMembers.Name = "lcMembers";
            this.lcMembers.SelectedLocationId = null;
            this.lcMembers.LocationsSelected += new System.EventHandler<System.EventArgs>(this.lcMembers_LocationsSelected);
            // 
            // lcLocations
            // 
            resources.ApplyResources(this.lcLocations, "lcLocations");
            this.lcLocations.ContextMenuStrip = null;
            this.lcLocations.MultiSelect = false;
            this.lcLocations.Name = "lcLocations";
            this.lcLocations.SelectedLocationId = null;
            this.lcLocations.LocationsSelected += new System.EventHandler<System.EventArgs>(this.locationListControl_LocationsSelected);
            this.lcLocations.SizeChanged += new System.EventHandler(this.lcLocations_SizeChanged);
            this.lcLocations.DoubleClick += new System.EventHandler(this.locationListControl_DoubleClick);
            // 
            // contextButtonsLocations
            // 
            this.contextButtonsLocations.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtonsLocations, "contextButtonsLocations");
            this.contextButtonsLocations.BackColor = System.Drawing.Color.Transparent;
            this.contextButtonsLocations.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.contextButtonsLocations.EditButtonEnabled = true;
            this.contextButtonsLocations.Name = "contextButtonsLocations";
            this.contextButtonsLocations.RemoveButtonEnabled = true;
            this.contextButtonsLocations.EditButtonClicked += new System.EventHandler(this.EditLocationClicked);
            this.contextButtonsLocations.AddButtonClicked += new System.EventHandler(this.AddLocationClicked);
            this.contextButtonsLocations.RemoveButtonClicked += new System.EventHandler(this.RemoveLocationClicked);
            // 
            // contextButtonsMembers
            // 
            this.contextButtonsMembers.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtonsMembers, "contextButtonsMembers");
            this.contextButtonsMembers.BackColor = System.Drawing.Color.Transparent;
            this.contextButtonsMembers.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.contextButtonsMembers.EditButtonEnabled = false;
            this.contextButtonsMembers.Name = "contextButtonsMembers";
            this.contextButtonsMembers.RemoveButtonEnabled = true;
            this.contextButtonsMembers.AddButtonClicked += new System.EventHandler(this.AddMemberClicked);
            this.contextButtonsMembers.RemoveButtonClicked += new System.EventHandler(this.RemoveMemberClicked);
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
            // LocationListView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 34;
            this.Name = "LocationListView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMembers;
        private System.Windows.Forms.Label lblLocations;
        private System.Windows.Forms.Label labelSynchronizing;
        private Controls.LocationListControl lcLocations;
        private Controls.LocationListControl lcMembers;
        private ContextButtons contextButtonsMembers;
        private ContextButtons contextButtonsLocations;
        private LSOne.Controls.SearchBar searchBar;
    }
}
