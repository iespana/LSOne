using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    partial class CrossAndModifierInfocodeGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrossAndModifierInfocodeGeneralPage));
            this.lblDescription = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.cmbTriggering = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkLinkItemLinesToTriggerLine = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkCreateTransInfoCodeEntry = new System.Windows.Forms.CheckBox();
            this.cmbLinkedInfocode = new DualDataComboBox();
            this.lblExplanatoryHeaderText = new System.Windows.Forms.Label();
            this.tbExplanatoryHeaderText = new System.Windows.Forms.TextBox();
            this.ntbMaxSelection = new NumericTextBox();
            this.ntbMinSelection = new NumericTextBox();
            this.chkMultipleSelection = new System.Windows.Forms.CheckBox();
            this.lblMultipleSelection = new System.Windows.Forms.Label();
            this.lblMaxSelection = new System.Windows.Forms.Label();
            this.lblMinSelection = new System.Windows.Forms.Label();
            this.cmbOkPressedAction = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkLinkedInfocode = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Name = "lblDescription";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // tbDescription
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbDescription, 2);
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.tbID, 2);
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            // 
            // cmbTriggering
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cmbTriggering, 2);
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
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // chkLinkItemLinesToTriggerLine
            // 
            resources.ApplyResources(this.chkLinkItemLinesToTriggerLine, "chkLinkItemLinesToTriggerLine");
            this.tableLayoutPanel1.SetColumnSpan(this.chkLinkItemLinesToTriggerLine, 2);
            this.chkLinkItemLinesToTriggerLine.Name = "chkLinkItemLinesToTriggerLine";
            this.chkLinkItemLinesToTriggerLine.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // chkCreateTransInfoCodeEntry
            // 
            resources.ApplyResources(this.chkCreateTransInfoCodeEntry, "chkCreateTransInfoCodeEntry");
            this.tableLayoutPanel1.SetColumnSpan(this.chkCreateTransInfoCodeEntry, 2);
            this.chkCreateTransInfoCodeEntry.Name = "chkCreateTransInfoCodeEntry";
            this.chkCreateTransInfoCodeEntry.UseVisualStyleBackColor = true;
            // 
            // cmbLinkedInfocode
            // 
            this.cmbLinkedInfocode.AddList = null;
            this.cmbLinkedInfocode.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbLinkedInfocode, "cmbLinkedInfocode");
            this.cmbLinkedInfocode.MaxLength = 32767;
            this.cmbLinkedInfocode.Name = "cmbLinkedInfocode";
            this.cmbLinkedInfocode.RemoveList = null;
            this.cmbLinkedInfocode.RowHeight = ((short)(22));
            this.cmbLinkedInfocode.SelectedData = null;
            this.cmbLinkedInfocode.SelectionList = null;
            this.cmbLinkedInfocode.SkipIDColumn = true;
            this.cmbLinkedInfocode.RequestData += new System.EventHandler(this.cmbLinkedInfocode_RequestData);
            this.cmbLinkedInfocode.FormatData += new DropDownFormatDataHandler(this.cmbLinkedInfocode_FormatData);
            this.cmbLinkedInfocode.SelectedDataChanged += new System.EventHandler(this.cmbLinkedInfocode_SelectedDataChanged);
            this.cmbLinkedInfocode.RequestClear += new System.EventHandler(this.cmbLinkedInfocode_RequestClear);
            // 
            // lblExplanatoryHeaderText
            // 
            resources.ApplyResources(this.lblExplanatoryHeaderText, "lblExplanatoryHeaderText");
            this.lblExplanatoryHeaderText.BackColor = System.Drawing.Color.Transparent;
            this.lblExplanatoryHeaderText.Name = "lblExplanatoryHeaderText";
            // 
            // tbExplanatoryHeaderText
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbExplanatoryHeaderText, 2);
            resources.ApplyResources(this.tbExplanatoryHeaderText, "tbExplanatoryHeaderText");
            this.tbExplanatoryHeaderText.Name = "tbExplanatoryHeaderText";
            // 
            // ntbMaxSelection
            // 
            this.ntbMaxSelection.AllowDecimal = false;
            this.ntbMaxSelection.AllowNegative = false;
            this.tableLayoutPanel1.SetColumnSpan(this.ntbMaxSelection, 2);
            this.ntbMaxSelection.CultureInfo = null;
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
            this.tableLayoutPanel1.SetColumnSpan(this.ntbMinSelection, 2);
            this.ntbMinSelection.CultureInfo = null;
            this.ntbMinSelection.DecimalLetters = 2;
            this.ntbMinSelection.HasMinValue = false;
            resources.ApplyResources(this.ntbMinSelection, "ntbMinSelection");
            this.ntbMinSelection.MaxValue = 0D;
            this.ntbMinSelection.MinValue = 0D;
            this.ntbMinSelection.Name = "ntbMinSelection";
            this.ntbMinSelection.Value = 0D;
            // 
            // chkMultipleSelection
            // 
            resources.ApplyResources(this.chkMultipleSelection, "chkMultipleSelection");
            this.tableLayoutPanel1.SetColumnSpan(this.chkMultipleSelection, 2);
            this.chkMultipleSelection.Name = "chkMultipleSelection";
            this.chkMultipleSelection.UseVisualStyleBackColor = true;
            // 
            // lblMultipleSelection
            // 
            resources.ApplyResources(this.lblMultipleSelection, "lblMultipleSelection");
            this.lblMultipleSelection.Name = "lblMultipleSelection";
            // 
            // lblMaxSelection
            // 
            resources.ApplyResources(this.lblMaxSelection, "lblMaxSelection");
            this.lblMaxSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblMaxSelection.Name = "lblMaxSelection";
            // 
            // lblMinSelection
            // 
            resources.ApplyResources(this.lblMinSelection, "lblMinSelection");
            this.lblMinSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblMinSelection.Name = "lblMinSelection";
            // 
            // cmbOkPressedAction
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cmbOkPressedAction, 2);
            this.cmbOkPressedAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOkPressedAction.FormattingEnabled = true;
            this.cmbOkPressedAction.Items.AddRange(new object[] {
            resources.GetString("cmbOkPressedAction.Items"),
            resources.GetString("cmbOkPressedAction.Items1")});
            resources.ApplyResources(this.cmbOkPressedAction, "cmbOkPressedAction");
            this.cmbOkPressedAction.Name = "cmbOkPressedAction";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkLinkedInfocode
            // 
            resources.ApplyResources(this.chkLinkedInfocode, "chkLinkedInfocode");
            this.chkLinkedInfocode.Name = "chkLinkedInfocode";
            this.chkLinkedInfocode.UseVisualStyleBackColor = true;
            this.chkLinkedInfocode.CheckedChanged += new System.EventHandler(this.chkLinkedInfocode_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbOkPressedAction, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.chkLinkedInfocode, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.tbID, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbLinkedInfocode, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblDescription, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbDescription, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ntbMaxSelection, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.chkLinkItemLinesToTriggerLine, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.chkCreateTransInfoCodeEntry, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblExplanatoryHeaderText, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblMaxSelection, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.ntbMinSelection, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbExplanatoryHeaderText, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.chkMultipleSelection, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblMinSelection, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblMultipleSelection, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmbTriggering, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // CrossAndModifierInfocodeGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "CrossAndModifierInfocodeGeneralPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.ComboBox cmbTriggering;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkLinkItemLinesToTriggerLine;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkCreateTransInfoCodeEntry;
        private DualDataComboBox cmbLinkedInfocode;
        private System.Windows.Forms.Label lblExplanatoryHeaderText;
        private System.Windows.Forms.TextBox tbExplanatoryHeaderText;
        private NumericTextBox ntbMaxSelection;
        private NumericTextBox ntbMinSelection;
        private System.Windows.Forms.CheckBox chkMultipleSelection;
        private System.Windows.Forms.Label lblMultipleSelection;
        private System.Windows.Forms.Label lblMaxSelection;
        private System.Windows.Forms.Label lblMinSelection;
        private System.Windows.Forms.ComboBox cmbOkPressedAction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkLinkedInfocode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    }
}
