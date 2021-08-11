using LSOne.Controls;

namespace LSOne.ViewPlugins.CentralSuspensions.ViewPages
{
    partial class StoreSuspendTransactionsPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreSuspendTransactionsPage));
            this.lvInfocodesImages = new System.Windows.Forms.ImageList(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.groupPanel1 = new GroupPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAllowEOD = new System.Windows.Forms.CheckBox();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvInfocodesImages
            // 
            this.lvInfocodesImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.lvInfocodesImages, "lvInfocodesImages");
            this.lvInfocodesImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // chkAllowEOD
            // 
            resources.ApplyResources(this.chkAllowEOD, "chkAllowEOD");
            this.chkAllowEOD.Name = "chkAllowEOD";
            this.chkAllowEOD.UseVisualStyleBackColor = true;
            // 
            // StoreSuspendTransactionsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkAllowEOD);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label6);
            this.DoubleBuffered = true;
            this.Name = "StoreSuspendTransactionsPage";
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList lvInfocodesImages;
        private System.Windows.Forms.Label label6;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAllowEOD;

    }
}
