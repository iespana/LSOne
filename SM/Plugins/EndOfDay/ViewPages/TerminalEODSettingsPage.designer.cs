using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.ViewPages
{
    partial class TerminalEODSettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalEODSettingsPage));
            this.lblStatementPosting = new System.Windows.Forms.Label();
            this.cmbStatementPosting = new System.Windows.Forms.ComboBox();
            this.chkIncludeTerminal = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.SuspendLayout();
            // 
            // lblStatementPosting
            // 
            resources.ApplyResources(this.lblStatementPosting, "lblStatementPosting");
            this.lblStatementPosting.Name = "lblStatementPosting";
            // 
            // cmbStatementPosting
            // 
            this.cmbStatementPosting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbStatementPosting, "cmbStatementPosting");
            this.cmbStatementPosting.FormattingEnabled = true;
            this.cmbStatementPosting.Items.AddRange(new object[] {
            resources.GetString("cmbStatementPosting.Items"),
            resources.GetString("cmbStatementPosting.Items1")});
            this.cmbStatementPosting.Name = "cmbStatementPosting";
            // 
            // chkIncludeTerminal
            // 
            resources.ApplyResources(this.chkIncludeTerminal, "chkIncludeTerminal");
            this.chkIncludeTerminal.Name = "chkIncludeTerminal";
            this.chkIncludeTerminal.UseVisualStyleBackColor = true;
            this.chkIncludeTerminal.CheckedChanged += new System.EventHandler(this.chkIncludeTerminal_CheckedChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // TerminalEODSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.chkIncludeTerminal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbStatementPosting);
            this.Controls.Add(this.lblStatementPosting);
            this.DoubleBuffered = true;
            this.Name = "TerminalEODSettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatementPosting;
        private System.Windows.Forms.ComboBox cmbStatementPosting;
        private System.Windows.Forms.CheckBox chkIncludeTerminal;
        private System.Windows.Forms.Label label3;
        private LinkFields linkFields1;

    }
}
