using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    partial class InfocodeConfigurationPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfocodeConfigurationPage));
            this.lblMinSelection = new System.Windows.Forms.Label();
            this.lblMaxSelection = new System.Windows.Forms.Label();
            this.chkMultipleSelection = new System.Windows.Forms.CheckBox();
            this.lblMultipleSelection = new System.Windows.Forms.Label();
            this.ntbMinSelection = new NumericTextBox();
            this.ntbMaxSelection = new NumericTextBox();
            this.SuspendLayout();
            // 
            // lblMinSelection
            // 
            this.lblMinSelection.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMinSelection, "lblMinSelection");
            this.lblMinSelection.Name = "lblMinSelection";
            // 
            // lblMaxSelection
            // 
            this.lblMaxSelection.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMaxSelection, "lblMaxSelection");
            this.lblMaxSelection.Name = "lblMaxSelection";
            // 
            // chkMultipleSelection
            // 
            resources.ApplyResources(this.chkMultipleSelection, "chkMultipleSelection");
            this.chkMultipleSelection.Name = "chkMultipleSelection";
            this.chkMultipleSelection.UseVisualStyleBackColor = true;
            // 
            // lblMultipleSelection
            // 
            resources.ApplyResources(this.lblMultipleSelection, "lblMultipleSelection");
            this.lblMultipleSelection.Name = "lblMultipleSelection";
            // 
            // ntbMinSelection
            // 
            this.ntbMinSelection.AllowDecimal = false;
            this.ntbMinSelection.AllowNegative = false;
            this.ntbMinSelection.CultureInfo = null;
            this.ntbMinSelection.DecimalLetters = 2;
            this.ntbMinSelection.HasMinValue = false;
            resources.ApplyResources(this.ntbMinSelection, "ntbMinSelection");
            this.ntbMinSelection.MaxValue = 0D;
            this.ntbMinSelection.MinValue = 0D;
            this.ntbMinSelection.Name = "ntbMinSelection";
            this.ntbMinSelection.Value = 0D;
            // 
            // ntbMaxSelection
            // 
            this.ntbMaxSelection.AllowDecimal = false;
            this.ntbMaxSelection.AllowNegative = false;
            this.ntbMaxSelection.CultureInfo = null;
            this.ntbMaxSelection.DecimalLetters = 2;
            this.ntbMaxSelection.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxSelection, "ntbMaxSelection");
            this.ntbMaxSelection.MaxValue = 0D;
            this.ntbMaxSelection.MinValue = 0D;
            this.ntbMaxSelection.Name = "ntbMaxSelection";
            this.ntbMaxSelection.Value = 0D;
            // 
            // InfocodeConfigurationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbMaxSelection);
            this.Controls.Add(this.ntbMinSelection);
            this.Controls.Add(this.chkMultipleSelection);
            this.Controls.Add(this.lblMultipleSelection);
            this.Controls.Add(this.lblMaxSelection);
            this.Controls.Add(this.lblMinSelection);
            this.DoubleBuffered = true;
            this.Name = "InfocodeConfigurationPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMinSelection;
        private System.Windows.Forms.CheckBox chkMultipleSelection;
        private System.Windows.Forms.Label lblMultipleSelection;
        private System.Windows.Forms.Label lblMaxSelection;
        private NumericTextBox ntbMinSelection;
        private NumericTextBox ntbMaxSelection;

    }
}
