namespace LSOne.ViewPlugins.Terminals.ViewPages
{
    partial class TerminalCustomerDisplayPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalCustomerDisplayPage));
            this.label9 = new System.Windows.Forms.Label();
            this.tbText1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbText2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // tbText1
            // 
            resources.ApplyResources(this.tbText1, "tbText1");
            this.tbText1.Name = "tbText1";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbText2
            // 
            resources.ApplyResources(this.tbText2, "tbText2");
            this.tbText2.Name = "tbText2";
            // 
            // TerminalCustomerDisplayPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbText2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbText1);
            this.DoubleBuffered = true;
            this.Name = "TerminalCustomerDisplayPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbText1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbText2;
    }
}
