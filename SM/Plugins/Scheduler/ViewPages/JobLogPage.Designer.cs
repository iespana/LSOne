using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class JobLogPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobLogPage));
            this.jobLogControl = new LSOne.ViewPlugins.Scheduler.Controls.JobLogControl();
            this.SuspendLayout();
            // 
            // jobLogControl
            // 
            resources.ApplyResources(this.jobLogControl, "jobLogControl");
            this.jobLogControl.BackColor = System.Drawing.Color.Transparent;
            this.jobLogControl.Name = "jobLogControl";
            // 
            // JobLogPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.jobLogControl);
            this.Name = "JobLogPage";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.JobLogControl jobLogControl;

    }
}
