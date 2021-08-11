using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    partial class InfocodeGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfocodeGeneralPage));
            this.lblDescription = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPrompt = new System.Windows.Forms.TextBox();
            this.cmbTriggering = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkInputRequired = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkOncePerTransaction = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbLinkedInfocode = new LSOne.Controls.DualDataComboBox();
            this.ntbRandomFactor = new LSOne.Controls.NumericTextBox();
            this.lblRandomFactor = new System.Windows.Forms.Label();
            this.lblMinLength = new System.Windows.Forms.Label();
            this.lblMaxLength = new System.Windows.Forms.Label();
            this.ntbMinLength = new LSOne.Controls.NumericTextBox();
            this.ntbMaxLength = new LSOne.Controls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkLinkedInfocode = new System.Windows.Forms.CheckBox();
            this.ntbMinValue = new LSOne.Controls.NumericTextBox();
            this.ntbMaxValue = new LSOne.Controls.NumericTextBox();
            this.ntbMinSelection = new LSOne.Controls.NumericTextBox();
            this.ntbMaxSelection = new LSOne.Controls.NumericTextBox();
            this.cmbType = new LSOne.Controls.DualDataComboBox();
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
            // tbPrompt
            // 
            resources.ApplyResources(this.tbPrompt, "tbPrompt");
            this.tbPrompt.Name = "tbPrompt";
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
            // chkInputRequired
            // 
            resources.ApplyResources(this.chkInputRequired, "chkInputRequired");
            this.chkInputRequired.Name = "chkInputRequired";
            this.chkInputRequired.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkOncePerTransaction
            // 
            resources.ApplyResources(this.chkOncePerTransaction, "chkOncePerTransaction");
            this.chkOncePerTransaction.Name = "chkOncePerTransaction";
            this.chkOncePerTransaction.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmbLinkedInfocode
            // 
            this.cmbLinkedInfocode.AddList = null;
            this.cmbLinkedInfocode.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbLinkedInfocode, "cmbLinkedInfocode");
            this.cmbLinkedInfocode.MaxLength = 32767;
            this.cmbLinkedInfocode.Name = "cmbLinkedInfocode";
            this.cmbLinkedInfocode.NoChangeAllowed = false;
            this.cmbLinkedInfocode.OnlyDisplayID = false;
            this.cmbLinkedInfocode.RemoveList = null;
            this.cmbLinkedInfocode.RowHeight = ((short)(22));
            this.cmbLinkedInfocode.SecondaryData = null;
            this.cmbLinkedInfocode.SelectedData = null;
            this.cmbLinkedInfocode.SelectedDataID = null;
            this.cmbLinkedInfocode.SelectionList = null;
            this.cmbLinkedInfocode.SkipIDColumn = true;
            this.cmbLinkedInfocode.RequestData += new System.EventHandler(this.cmbLinkedInfocode_RequestData);
            this.cmbLinkedInfocode.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbLinkedInfocode_FormatData);
            this.cmbLinkedInfocode.SelectedDataChanged += new System.EventHandler(this.cmbLinkedInfocode_SelectedDataChanged);
            this.cmbLinkedInfocode.RequestClear += new System.EventHandler(this.cmbLinkedInfocode_RequestClear);
            // 
            // ntbRandomFactor
            // 
            this.ntbRandomFactor.AllowDecimal = false;
            this.ntbRandomFactor.AllowNegative = false;
            this.ntbRandomFactor.CultureInfo = null;
            this.ntbRandomFactor.DecimalLetters = 2;
            this.ntbRandomFactor.ForeColor = System.Drawing.Color.Black;
            this.ntbRandomFactor.HasMinValue = false;
            resources.ApplyResources(this.ntbRandomFactor, "ntbRandomFactor");
            this.ntbRandomFactor.MaxValue = 100D;
            this.ntbRandomFactor.MinValue = 0D;
            this.ntbRandomFactor.Name = "ntbRandomFactor";
            this.ntbRandomFactor.Value = 0D;
            // 
            // lblRandomFactor
            // 
            this.lblRandomFactor.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRandomFactor, "lblRandomFactor");
            this.lblRandomFactor.Name = "lblRandomFactor";
            // 
            // lblMinLength
            // 
            this.lblMinLength.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMinLength, "lblMinLength");
            this.lblMinLength.Name = "lblMinLength";
            // 
            // lblMaxLength
            // 
            this.lblMaxLength.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMaxLength, "lblMaxLength");
            this.lblMaxLength.Name = "lblMaxLength";
            // 
            // ntbMinLength
            // 
            this.ntbMinLength.AllowDecimal = false;
            this.ntbMinLength.AllowNegative = false;
            this.ntbMinLength.CultureInfo = null;
            this.ntbMinLength.DecimalLetters = 2;
            this.ntbMinLength.ForeColor = System.Drawing.Color.Black;
            this.ntbMinLength.HasMinValue = false;
            resources.ApplyResources(this.ntbMinLength, "ntbMinLength");
            this.ntbMinLength.MaxValue = 999999D;
            this.ntbMinLength.MinValue = 0D;
            this.ntbMinLength.Name = "ntbMinLength";
            this.ntbMinLength.Value = 0D;
            // 
            // ntbMaxLength
            // 
            this.ntbMaxLength.AllowDecimal = false;
            this.ntbMaxLength.AllowNegative = false;
            this.ntbMaxLength.CultureInfo = null;
            this.ntbMaxLength.DecimalLetters = 2;
            this.ntbMaxLength.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxLength.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxLength, "ntbMaxLength");
            this.ntbMaxLength.MaxValue = 999999D;
            this.ntbMaxLength.MinValue = 0D;
            this.ntbMaxLength.Name = "ntbMaxLength";
            this.ntbMaxLength.Value = 0D;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkLinkedInfocode
            // 
            resources.ApplyResources(this.chkLinkedInfocode, "chkLinkedInfocode");
            this.chkLinkedInfocode.Name = "chkLinkedInfocode";
            this.chkLinkedInfocode.UseVisualStyleBackColor = true;
            this.chkLinkedInfocode.CheckedChanged += new System.EventHandler(this.chkLinkedInfocode_CheckedChanged);
            // 
            // ntbMinValue
            // 
            this.ntbMinValue.AllowDecimal = false;
            this.ntbMinValue.AllowNegative = false;
            this.ntbMinValue.CultureInfo = null;
            this.ntbMinValue.DecimalLetters = 2;
            this.ntbMinValue.ForeColor = System.Drawing.Color.Black;
            this.ntbMinValue.HasMinValue = false;
            resources.ApplyResources(this.ntbMinValue, "ntbMinValue");
            this.ntbMinValue.MaxValue = 999999D;
            this.ntbMinValue.MinValue = 0D;
            this.ntbMinValue.Name = "ntbMinValue";
            this.ntbMinValue.Value = 0D;
            // 
            // ntbMaxValue
            // 
            this.ntbMaxValue.AllowDecimal = false;
            this.ntbMaxValue.AllowNegative = false;
            this.ntbMaxValue.CultureInfo = null;
            this.ntbMaxValue.DecimalLetters = 2;
            this.ntbMaxValue.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxValue.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxValue, "ntbMaxValue");
            this.ntbMaxValue.MaxValue = 999999D;
            this.ntbMaxValue.MinValue = 0D;
            this.ntbMaxValue.Name = "ntbMaxValue";
            this.ntbMaxValue.Value = 0D;
            // 
            // ntbMinSelection
            // 
            this.ntbMinSelection.AllowDecimal = false;
            this.ntbMinSelection.AllowNegative = false;
            this.ntbMinSelection.CultureInfo = null;
            this.ntbMinSelection.DecimalLetters = 2;
            this.ntbMinSelection.ForeColor = System.Drawing.Color.Black;
            this.ntbMinSelection.HasMinValue = false;
            resources.ApplyResources(this.ntbMinSelection, "ntbMinSelection");
            this.ntbMinSelection.MaxValue = 120D;
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
            this.ntbMaxSelection.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxSelection.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxSelection, "ntbMaxSelection");
            this.ntbMaxSelection.MaxValue = 120D;
            this.ntbMaxSelection.MinValue = 0D;
            this.ntbMaxSelection.Name = "ntbMaxSelection";
            this.ntbMaxSelection.Value = 0D;
            // 
            // cmbType
            // 
            this.cmbType.AddList = null;
            this.cmbType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.MaxLength = 32767;
            this.cmbType.Name = "cmbType";
            this.cmbType.NoChangeAllowed = false;
            this.cmbType.OnlyDisplayID = false;
            this.cmbType.RemoveList = null;
            this.cmbType.RowHeight = ((short)(22));
            this.cmbType.SecondaryData = null;
            this.cmbType.SelectedData = null;
            this.cmbType.SelectedDataID = null;
            this.cmbType.SelectionList = null;
            this.cmbType.SkipIDColumn = true;
            this.cmbType.RequestData += new System.EventHandler(this.cmbStatus_RequestData);
            this.cmbType.SelectedDataChanged += new System.EventHandler(this.cmbType_SelectedDataChanged);
            // 
            // InfocodeGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.ntbMaxSelection);
            this.Controls.Add(this.ntbMinSelection);
            this.Controls.Add(this.chkLinkedInfocode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblMaxLength);
            this.Controls.Add(this.lblMinLength);
            this.Controls.Add(this.ntbRandomFactor);
            this.Controls.Add(this.lblRandomFactor);
            this.Controls.Add(this.cmbLinkedInfocode);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chkOncePerTransaction);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkInputRequired);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbTriggering);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPrompt);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.ntbMinValue);
            this.Controls.Add(this.ntbMinLength);
            this.Controls.Add(this.ntbMaxValue);
            this.Controls.Add(this.ntbMaxLength);
            this.DoubleBuffered = true;
            this.Name = "InfocodeGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPrompt;
        private System.Windows.Forms.ComboBox cmbTriggering;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkInputRequired;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkOncePerTransaction;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private DualDataComboBox cmbLinkedInfocode;
        private NumericTextBox ntbRandomFactor;
        private System.Windows.Forms.Label lblRandomFactor;
        private System.Windows.Forms.Label lblMinLength;
        private System.Windows.Forms.Label lblMaxLength;
        private NumericTextBox ntbMinLength;
        private NumericTextBox ntbMaxLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkLinkedInfocode;
        private NumericTextBox ntbMinValue;
        private NumericTextBox ntbMaxValue;
        private NumericTextBox ntbMinSelection;
        private NumericTextBox ntbMaxSelection;
        private DualDataComboBox cmbType;
    }
}
