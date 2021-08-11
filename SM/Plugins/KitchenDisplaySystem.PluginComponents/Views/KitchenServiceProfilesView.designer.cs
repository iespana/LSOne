using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class KitchenServiceProfilesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KitchenServiceProfilesView));
            this.lvKitchenServiceProfiles = new ExtendedListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsContextButtons = new ContextButtons();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvKitchenServiceProfiles);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            // 
            // lvKitchenServiceProfiles
            // 
            resources.ApplyResources(this.lvKitchenServiceProfiles, "lvKitchenServiceProfiles");
            this.lvKitchenServiceProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader1});
            this.lvKitchenServiceProfiles.FullRowSelect = true;
            this.lvKitchenServiceProfiles.HideSelection = false;
            this.lvKitchenServiceProfiles.LockDrawing = false;
            this.lvKitchenServiceProfiles.Name = "lvKitchenServiceProfiles";
            this.lvKitchenServiceProfiles.SortColumn = 0;
            this.lvKitchenServiceProfiles.SortedBackwards = false;
            this.lvKitchenServiceProfiles.UseCompatibleStateImageBehavior = false;
            this.lvKitchenServiceProfiles.UseEveryOtherRowColoring = true;
            this.lvKitchenServiceProfiles.View = System.Windows.Forms.View.Details;
            this.lvKitchenServiceProfiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvKitchenServiceProfiles_ColumnClick);
            this.lvKitchenServiceProfiles.SelectedIndexChanged += new System.EventHandler(this.lvKitchenServiceProfiles_SelectedIndexChanged);
            this.lvKitchenServiceProfiles.DoubleClick += new System.EventHandler(this.lvKitchenMangerProfiles_DoubleClick);
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // KitchenServiceProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "KitchenServiceProfilesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvKitchenServiceProfiles;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.ColumnHeader columnHeader1;

    }
}
