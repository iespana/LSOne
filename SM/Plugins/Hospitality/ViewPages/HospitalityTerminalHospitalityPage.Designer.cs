namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityTerminalHospitalityPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTerminalHospitalityPage));
            this.grpHospitalityTypes = new System.Windows.Forms.GroupBox();
            this.pnlHospitalityTypes = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.chkSwitchUserWhenEnteringPOS = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.grpHospitalityTypes.SuspendLayout();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpHospitalityTypes
            // 
            resources.ApplyResources(this.grpHospitalityTypes, "grpHospitalityTypes");
            this.grpHospitalityTypes.Controls.Add(this.pnlHospitalityTypes);
            this.grpHospitalityTypes.Name = "grpHospitalityTypes";
            this.grpHospitalityTypes.TabStop = false;
            // 
            // pnlHospitalityTypes
            // 
            resources.ApplyResources(this.pnlHospitalityTypes, "pnlHospitalityTypes");
            this.pnlHospitalityTypes.BackColor = System.Drawing.Color.White;
            this.pnlHospitalityTypes.Name = "pnlHospitalityTypes";
            // 
            // btnSelectNone
            // 
            resources.ApplyResources(this.btnSelectNone, "btnSelectNone");
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // gbSettings
            // 
            resources.ApplyResources(this.gbSettings, "gbSettings");
            this.gbSettings.Controls.Add(this.chkSwitchUserWhenEnteringPOS);
            this.gbSettings.Controls.Add(this.label8);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.TabStop = false;
            // 
            // chkSwitchUserWhenEnteringPOS
            // 
            resources.ApplyResources(this.chkSwitchUserWhenEnteringPOS, "chkSwitchUserWhenEnteringPOS");
            this.chkSwitchUserWhenEnteringPOS.Name = "chkSwitchUserWhenEnteringPOS";
            this.chkSwitchUserWhenEnteringPOS.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // HospitalityTerminalHospitalityPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.grpHospitalityTypes);
            this.Name = "HospitalityTerminalHospitalityPage";
            this.grpHospitalityTypes.ResumeLayout(false);
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpHospitalityTypes;
        private System.Windows.Forms.FlowLayoutPanel pnlHospitalityTypes;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.CheckBox chkSwitchUserWhenEnteringPOS;
        private System.Windows.Forms.Label label8;

    }
}
