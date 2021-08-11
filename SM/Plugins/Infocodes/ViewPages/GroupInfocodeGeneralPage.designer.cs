using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    partial class GroupInfocodeGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupInfocodeGeneralPage));
            this.lblDescription = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbExplanatoryHeaderText = new System.Windows.Forms.TextBox();
            this.cmbTriggering = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblLinkedInfocode = new System.Windows.Forms.Label();
            this.chkLinkItemLinesToTriggerLine = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbLinkedInfocode = new DualDataComboBox();
            this.ntbMaxSelection = new NumericTextBox();
            this.ntbMinSelection = new NumericTextBox();
            this.lblMaxSelection = new System.Windows.Forms.Label();
            this.lblMinSelection = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbExplanatoryHeaderText
            // 
            resources.ApplyResources(this.tbExplanatoryHeaderText, "tbExplanatoryHeaderText");
            this.tbExplanatoryHeaderText.Name = "tbExplanatoryHeaderText";
            // 
            // cmbTriggering
            // 
            this.cmbTriggering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTriggering.FormattingEnabled = true;
            this.cmbTriggering.Items.AddRange(new object[] {
            resources.GetString("cmbTriggering.Items"),
            resources.GetString("cmbTriggering.Items1")});
            resources.ApplyResources(this.cmbTriggering, "cmbTriggering");
            this.cmbTriggering.Name = "cmbTriggering";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblLinkedInfocode
            // 
            resources.ApplyResources(this.lblLinkedInfocode, "lblLinkedInfocode");
            this.lblLinkedInfocode.Name = "lblLinkedInfocode";
            // 
            // chkLinkItemLinesToTriggerLine
            // 
            resources.ApplyResources(this.chkLinkItemLinesToTriggerLine, "chkLinkItemLinesToTriggerLine");
            this.chkLinkItemLinesToTriggerLine.Name = "chkLinkItemLinesToTriggerLine";
            this.chkLinkItemLinesToTriggerLine.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // cmbLinkedInfocode
            // 
            resources.ApplyResources(this.cmbLinkedInfocode, "cmbLinkedInfocode");
            this.cmbLinkedInfocode.MaxLength = 32767;
            this.cmbLinkedInfocode.Name = "cmbLinkedInfocode";
            this.cmbLinkedInfocode.SelectedData = null;
            this.cmbLinkedInfocode.SkipIDColumn = true;
            this.cmbLinkedInfocode.RequestData += new System.EventHandler(this.cmbLinkedInfocode_RequestData);
            this.cmbLinkedInfocode.FormatData += new DropDownFormatDataHandler(this.cmbLinkedInfocode_FormatData);
            this.cmbLinkedInfocode.SelectedDataChanged += new System.EventHandler(this.cmbLinkedInfocode_SelectedDataChanged);
            this.cmbLinkedInfocode.RequestClear += new System.EventHandler(this.cmbLinkedInfocode_RequestClear);
            // 
            // ntbMaxSelection
            // 
            this.ntbMaxSelection.AllowDecimal = false;
            this.ntbMaxSelection.AllowNegative = false;
            this.ntbMaxSelection.DecimalLetters = 2;
            this.ntbMaxSelection.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxSelection, "ntbMaxSelection");
            this.ntbMaxSelection.MaxValue = 0D;
            this.ntbMaxSelection.MinValue = 0D;
            this.ntbMaxSelection.Name = "ntbMaxSelection";
            this.ntbMaxSelection.Value = 0D;
            // 
            // ntbMinSelection
            // 
            this.ntbMinSelection.AllowDecimal = false;
            this.ntbMinSelection.AllowNegative = false;
            this.ntbMinSelection.DecimalLetters = 2;
            this.ntbMinSelection.HasMinValue = false;
            resources.ApplyResources(this.ntbMinSelection, "ntbMinSelection");
            this.ntbMinSelection.MaxValue = 0D;
            this.ntbMinSelection.MinValue = 0D;
            this.ntbMinSelection.Name = "ntbMinSelection";
            this.ntbMinSelection.Value = 0D;
            // 
            // lblMaxSelection
            // 
            this.lblMaxSelection.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMaxSelection, "lblMaxSelection");
            this.lblMaxSelection.Name = "lblMaxSelection";
            // 
            // lblMinSelection
            // 
            this.lblMinSelection.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMinSelection, "lblMinSelection");
            this.lblMinSelection.Name = "lblMinSelection";
            // 
            // GroupInfocodeGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbMaxSelection);
            this.Controls.Add(this.ntbMinSelection);
            this.Controls.Add(this.lblMaxSelection);
            this.Controls.Add(this.lblMinSelection);
            this.Controls.Add(this.cmbLinkedInfocode);
            this.Controls.Add(this.chkLinkItemLinesToTriggerLine);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblLinkedInfocode);
            this.Controls.Add(this.cmbTriggering);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbExplanatoryHeaderText);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbID);
            this.DoubleBuffered = true;
            this.Name = "GroupInfocodeGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbExplanatoryHeaderText;
        private System.Windows.Forms.ComboBox cmbTriggering;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblLinkedInfocode;
        private System.Windows.Forms.CheckBox chkLinkItemLinesToTriggerLine;
        private System.Windows.Forms.Label label11;
        private DualDataComboBox cmbLinkedInfocode;
        private NumericTextBox ntbMaxSelection;
        private NumericTextBox ntbMinSelection;
        private System.Windows.Forms.Label lblMaxSelection;
        private System.Windows.Forms.Label lblMinSelection;

    }
}
