using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class CreditMemoPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreditMemoPage));
            this.chkUseCreditVouchers = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkUseCreditVouchers
            // 
            resources.ApplyResources(this.chkUseCreditVouchers, "chkUseCreditVouchers");
            this.chkUseCreditVouchers.Name = "chkUseCreditVouchers";
            this.chkUseCreditVouchers.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // CreditMemoPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkUseCreditVouchers);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.Name = "CreditMemoPage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseCreditVouchers;
        private System.Windows.Forms.Label label2;



    }
}
