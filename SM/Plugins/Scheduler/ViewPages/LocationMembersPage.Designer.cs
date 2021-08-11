using LSOne.Controls;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class LocationMembersPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationMembersPage));
            this.contextButtons1 = new LSOne.Controls.ContextButtons();
            this.lrParents = new LSOne.ViewPlugins.Scheduler.Controls.LocationRelation();
            this.lrMembers = new LSOne.ViewPlugins.Scheduler.Controls.LocationRelation();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextButtons1
            // 
            this.contextButtons1.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtons1, "contextButtons1");
            this.contextButtons1.BackColor = System.Drawing.Color.Transparent;
            this.contextButtons1.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.contextButtons1.EditButtonEnabled = false;
            this.contextButtons1.Name = "contextButtons1";
            this.contextButtons1.RemoveButtonEnabled = true;
            // 
            // lrParents
            // 
            resources.ApplyResources(this.lrParents, "lrParents");
            this.lrParents.BackColor = System.Drawing.Color.Transparent;
            this.lrParents.IsParent = false;
            this.lrParents.LocationsList = null;
            this.lrParents.Name = "lrParents";
            this.lrParents.RelationLocation = null;
            // 
            // lrMembers
            // 
            resources.ApplyResources(this.lrMembers, "lrMembers");
            this.lrMembers.BackColor = System.Drawing.Color.Transparent;
            this.lrMembers.IsParent = false;
            this.lrMembers.LocationsList = null;
            this.lrMembers.Name = "lrMembers";
            this.lrMembers.RelationLocation = null;
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.lrMembers, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lrParents, 0, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // LocationMembersPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.contextButtons1);
            this.DoubleBuffered = true;
            this.Name = "LocationMembersPage";
            resources.ApplyResources(this, "$this");
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons contextButtons1;
        private LocationRelation lrMembers;
        private LocationRelation lrParents;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
