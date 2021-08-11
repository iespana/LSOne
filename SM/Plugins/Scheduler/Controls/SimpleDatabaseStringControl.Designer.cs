

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class SimpleDatabaseStringControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleDatabaseStringControl));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonTextBox = new ButtonTextBox();
            this.SuspendLayout();
            // 
            // buttonTextBox
            // 
            this.buttonTextBox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonTextBox, "buttonTextBox");
            this.buttonTextBox.Name = "buttonTextBox";
            this.buttonTextBox.TextChanged += new System.EventHandler(this.buttonTextBox_TextChanged);
            this.buttonTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.buttonTextBox_Validating);
            // 
            // SimpleDatabaseStringControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.buttonTextBox);
            this.Name = "SimpleDatabaseStringControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        protected ButtonTextBox buttonTextBox;
    }
}
