using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.Dialogs.WizardPages
{
    partial class CreateStoreFirstPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateStoreFirstPage));
            this.label1 = new System.Windows.Forms.Label();
            this.btnDoNothing = new WizardOptionButton();
            this.btnCreate = new WizardOptionButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label1.Name = "label1";
            // 
            // btnDoNothing
            // 
            resources.ApplyResources(this.btnDoNothing, "btnDoNothing");
            this.btnDoNothing.Name = "btnDoNothing";
            this.btnDoNothing.Click += new System.EventHandler(this.btnDoNothing_Click);
            // 
            // btnCreate
            // 
            resources.ApplyResources(this.btnCreate, "btnCreate");
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // CreateStoreFirstPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDoNothing);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.label1);
            this.Name = "CreateStoreFirstPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private WizardOptionButton btnCreate;
        private WizardOptionButton btnDoNothing;
    }
}
