using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class SubJobListView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubJobListView));
            this.chkShowDisabled = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gpSearch = new LSOne.Controls.GroupPanel();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSearchTerm = new System.Windows.Forms.Label();
            this.subJobListControl = new LSOne.ViewPlugins.Scheduler.Controls.SubJobListControl();
            this.groupPanel2 = new LSOne.Controls.GroupPanel();
            this.pnlBottom.SuspendLayout();
            this.gpSearch.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.groupPanel2);
            this.pnlBottom.Controls.Add(this.gpSearch);
            this.pnlBottom.Controls.Add(this.subJobListControl);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // chkShowDisabled
            // 
            resources.ApplyResources(this.chkShowDisabled, "chkShowDisabled");
            this.chkShowDisabled.BackColor = System.Drawing.Color.Transparent;
            this.chkShowDisabled.Name = "chkShowDisabled";
            this.chkShowDisabled.UseVisualStyleBackColor = false;
            this.chkShowDisabled.CheckedChanged += new System.EventHandler(this.chkShowDisabled_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // gpSearch
            // 
            resources.ApplyResources(this.gpSearch, "gpSearch");
            this.gpSearch.Controls.Add(this.btnClear);
            this.gpSearch.Controls.Add(this.tbSearch);
            this.gpSearch.Controls.Add(this.btnSearch);
            this.gpSearch.Controls.Add(this.lblSearchTerm);
            this.gpSearch.Name = "gpSearch";
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearch_KeyPress);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblSearchTerm
            // 
            this.lblSearchTerm.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSearchTerm, "lblSearchTerm");
            this.lblSearchTerm.Name = "lblSearchTerm";
            // 
            // subJobListControl
            // 
            resources.ApplyResources(this.subJobListControl, "subJobListControl");
            this.subJobListControl.EnableActions = true;
            this.subJobListControl.Name = "subJobListControl";
            this.subJobListControl.ShowSubJob += new System.EventHandler<LSOne.ViewPlugins.Scheduler.Controls.SubJobEventArgs>(this.subJobListControl_ShowSubJob);
            this.subJobListControl.AddMultipleAction += new System.EventHandler<System.EventArgs>(this.subJobListControl_AddMultipleAction);
            this.subJobListControl.AddAction += new System.EventHandler<LSOne.ViewPlugins.Scheduler.Controls.SubJobEventArgs>(this.subJobListControl_AddAction);
            this.subJobListControl.EditAction += new System.EventHandler<LSOne.ViewPlugins.Scheduler.Controls.SubJobEventArgs>(this.subJobListControl_EditAction);
            this.subJobListControl.RemoveAction += new System.EventHandler<LSOne.ViewPlugins.Scheduler.Controls.SubJobsEventArgs>(this.subJobListControl_RemoveAction);
            // 
            // groupPanel2
            // 
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Controls.Add(this.label1);
            this.groupPanel2.Controls.Add(this.chkShowDisabled);
            this.groupPanel2.Name = "groupPanel2";
            // 
            // SubJobListView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 95;
            this.Name = "SubJobListView";
            this.pnlBottom.ResumeLayout(false);
            this.gpSearch.ResumeLayout(false);
            this.gpSearch.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SubJobListControl subJobListControl;
        private System.Windows.Forms.CheckBox chkShowDisabled;
        private System.Windows.Forms.Label label1;
        private LSOne.Controls.GroupPanel gpSearch;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblSearchTerm;
        private System.Windows.Forms.Button btnClear;
        private LSOne.Controls.GroupPanel groupPanel2;

    }
}
