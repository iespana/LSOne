using LSOne.Controls;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    partial class NewUserUserExistsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewUserUserExistsPage));
            this.wizardOptionButton2 = new WizardOptionButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLoginName = new System.Windows.Forms.TextBox();
            this.lblLoginName = new System.Windows.Forms.Label();
            this.fullNameField1 = new FullNameControl();
            this.wizardPanel1 = new WizardPanel();
            this.wizardPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardOptionButton2
            // 
            resources.ApplyResources(this.wizardOptionButton2, "wizardOptionButton2");
            this.wizardOptionButton2.Name = "wizardOptionButton2";
            this.wizardOptionButton2.Click += new System.EventHandler(this.wizardOptionButton2_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label1.Name = "label1";
            // 
            // tbLoginName
            // 
            resources.ApplyResources(this.tbLoginName, "tbLoginName");
            this.tbLoginName.Name = "tbLoginName";
            // 
            // lblLoginName
            // 
            this.lblLoginName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLoginName, "lblLoginName");
            this.lblLoginName.Name = "lblLoginName";
            // 
            // fullNameField1
            // 
            this.fullNameField1.Alias = "";
            resources.ApplyResources(this.fullNameField1, "fullNameField1");
            this.fullNameField1.BackColor = System.Drawing.Color.Transparent;
            this.fullNameField1.FirstName = "";
            this.fullNameField1.LastName = "";
            this.fullNameField1.MiddleName = "";
            this.fullNameField1.Name = "fullNameField1";
            this.fullNameField1.Prefix = "";
            this.fullNameField1.ShowAlias = false;
            this.fullNameField1.Suffix = "";
            // 
            // wizardPanel1
            // 
            this.wizardPanel1.Controls.Add(this.fullNameField1);
            this.wizardPanel1.Controls.Add(this.tbLoginName);
            this.wizardPanel1.Controls.Add(this.lblLoginName);
            resources.ApplyResources(this.wizardPanel1, "wizardPanel1");
            this.wizardPanel1.Name = "wizardPanel1";
            // 
            // NewUserUserExistsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wizardPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wizardOptionButton2);
            this.Name = "NewUserUserExistsPage";
            this.wizardPanel1.ResumeLayout(false);
            this.wizardPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WizardOptionButton wizardOptionButton2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLoginName;
        private System.Windows.Forms.Label lblLoginName;
        private FullNameControl fullNameField1;
        private WizardPanel wizardPanel1;
    }
}
