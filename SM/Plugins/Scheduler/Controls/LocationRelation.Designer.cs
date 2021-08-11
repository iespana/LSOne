using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class LocationRelation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationRelation));
            this.contextButtonsMembers = new LSOne.Controls.ContextButtons();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lcMembers = new LSOne.ViewPlugins.Scheduler.Controls.LocationListControl();
            this.SuspendLayout();
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
            this.contextButtonsMembers.AddButtonClicked += new System.EventHandler(this.contextButtonsMembers_AddButtonClicked);
            this.contextButtonsMembers.RemoveButtonClicked += new System.EventHandler(this.contextButtonsMembers_RemoveButtonClicked);
            // 
            // lblCaption
            // 
            resources.ApplyResources(this.lblCaption, "lblCaption");
            this.lblCaption.Name = "lblCaption";
            // 
            // lcMembers
            // 
            resources.ApplyResources(this.lcMembers, "lcMembers");
            this.lcMembers.BackColor = System.Drawing.Color.Transparent;
            this.lcMembers.ContextMenuStrip = null;
            this.lcMembers.MultiSelect = false;
            this.lcMembers.Name = "lcMembers";
            this.lcMembers.SelectedLocationId = null;
            // 
            // LocationRelation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.contextButtonsMembers);
            this.Controls.Add(this.lcMembers);
            this.Name = "LocationRelation";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextButtons contextButtonsMembers;
        private LocationListControl lcMembers;
        private System.Windows.Forms.Label lblCaption;

    }
}
