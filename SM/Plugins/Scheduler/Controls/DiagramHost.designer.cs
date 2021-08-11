namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class DiagramHost
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramHost));
            this.SuspendLayout();
            // 
            // DiagramHost
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "DiagramHost";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DiagramHost_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DiagramHost_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
