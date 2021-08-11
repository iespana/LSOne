namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemAdditionalInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemAdditionalInfo));
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tbSearchAlias = new System.Windows.Forms.TextBox();
            this.tbNotes = new System.Windows.Forms.TextBox();
            this.tbSearchKeywords = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // tbSearchAlias
            // 
            resources.ApplyResources(this.tbSearchAlias, "tbSearchAlias");
            this.tbSearchAlias.Name = "tbSearchAlias";
            // 
            // tbNotes
            // 
            this.tbNotes.AcceptsReturn = true;
            resources.ApplyResources(this.tbNotes, "tbNotes");
            this.tbNotes.Name = "tbNotes";
            // 
            // tbSearchKeywords
            // 
            resources.ApplyResources(this.tbSearchKeywords, "tbSearchKeywords");
            this.tbSearchKeywords.Name = "tbSearchKeywords";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ItemAdditionalInfo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbSearchKeywords);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbNotes);
            this.Controls.Add(this.tbSearchAlias);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.DoubleBuffered = true;
            this.Name = "ItemAdditionalInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbSearchAlias;
        private System.Windows.Forms.TextBox tbNotes;
        private System.Windows.Forms.TextBox tbSearchKeywords;
        private System.Windows.Forms.Label label1;
    }
}
