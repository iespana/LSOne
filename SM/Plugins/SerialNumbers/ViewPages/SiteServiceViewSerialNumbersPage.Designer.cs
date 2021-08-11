namespace LSOne.ViewPlugins.SerialNumbers.ViewPages
{
    partial class SiteServiceViewSerialNumbersPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteServiceViewSerialNumbersPage));
            this.label1 = new System.Windows.Forms.Label();
            this.chkUseSerialNumbers = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkUseSerialNumbers
            // 
            resources.ApplyResources(this.chkUseSerialNumbers, "chkUseSerialNumbers");
            this.chkUseSerialNumbers.Name = "chkUseSerialNumbers";
            this.chkUseSerialNumbers.UseVisualStyleBackColor = true;
            // 
            // SiteServiceViewSerialNumbersPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkUseSerialNumbers);
            this.Controls.Add(this.label1);
            this.Name = "SiteServiceViewSerialNumbersPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkUseSerialNumbers;
    }
}
