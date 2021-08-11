using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    partial class GroupSubcodeGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupSubcodeGeneralPage));
            this.label2 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblInfocodePrompt = new System.Windows.Forms.Label();
            this.tbInfocodePrompt = new System.Windows.Forms.TextBox();
            this.cmbInfocode = new DualDataComboBox();
            this.lblInfocode = new System.Windows.Forms.Label();
            this.btnInfocode = new ContextButton();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // lblInfocodePrompt
            // 
            this.lblInfocodePrompt.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblInfocodePrompt, "lblInfocodePrompt");
            this.lblInfocodePrompt.Name = "lblInfocodePrompt";
            // 
            // tbInfocodePrompt
            // 
            resources.ApplyResources(this.tbInfocodePrompt, "tbInfocodePrompt");
            this.tbInfocodePrompt.Name = "tbInfocodePrompt";
            // 
            // cmbInfocode
            // 
            resources.ApplyResources(this.cmbInfocode, "cmbInfocode");
            this.cmbInfocode.MaxLength = 32767;
            this.cmbInfocode.Name = "cmbInfocode";
            this.cmbInfocode.SelectedData = null;
            this.cmbInfocode.SkipIDColumn = true;
            this.cmbInfocode.RequestData += new System.EventHandler(this.cmbInfocode_RequestData);
            this.cmbInfocode.FormatData += new DropDownFormatDataHandler(this.cmbInfocode_FormatData);
            this.cmbInfocode.SelectedDataChanged += new System.EventHandler(this.cmbInfocode_SelectedDataChanged);
            this.cmbInfocode.RequestClear += new System.EventHandler(this.cmbInfocode_RequestClear);
            // 
            // lblInfocode
            // 
            this.lblInfocode.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblInfocode, "lblInfocode");
            this.lblInfocode.Name = "lblInfocode";
            // 
            // btnInfocode
            // 
            this.btnInfocode.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnInfocode, "btnInfocode");
            this.btnInfocode.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnInfocode.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnInfocode.Name = "btnInfocode";
            this.btnInfocode.Click += new System.EventHandler(this.btnInfocode_Click);
            // 
            // GroupSubcodeGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnInfocode);
            this.Controls.Add(this.cmbInfocode);
            this.Controls.Add(this.lblInfocode);
            this.Controls.Add(this.lblInfocodePrompt);
            this.Controls.Add(this.tbInfocodePrompt);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbID);
            this.DoubleBuffered = true;
            this.Name = "GroupSubcodeGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblInfocodePrompt;
        private System.Windows.Forms.TextBox tbInfocodePrompt;
        private DualDataComboBox cmbInfocode;
        private System.Windows.Forms.Label lblInfocode;
        private ContextButton btnInfocode;

    }
}
