using LSOne.Controls;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    partial class NewUserUserTypePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewUserUserTypePage));
            this.label1 = new System.Windows.Forms.Label();
            this.btnActiveDirectory = new WizardOptionButton();
            this.btnStandalone = new WizardOptionButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label1.Name = "label1";
            // 
            // btnActiveDirectory
            // 
            resources.ApplyResources(this.btnActiveDirectory, "btnActiveDirectory");
            this.btnActiveDirectory.Name = "btnActiveDirectory";
            this.btnActiveDirectory.Click += new System.EventHandler(this.btnActiveDirectory_Click);
            // 
            // btnStandalone
            // 
            resources.ApplyResources(this.btnStandalone, "btnStandalone");
            this.btnStandalone.Name = "btnStandalone";
            this.btnStandalone.Click += new System.EventHandler(this.btnStandalone_Click);
            // 
            // NewUserUserTypePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnActiveDirectory);
            this.Controls.Add(this.btnStandalone);
            this.Controls.Add(this.label1);
            this.Name = "NewUserUserTypePage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private WizardOptionButton btnStandalone;
        private WizardOptionButton btnActiveDirectory;
    }
}
