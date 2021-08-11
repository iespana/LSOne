using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    partial class SubcodeGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubcodeGeneralPage));
            this.label2 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.cmbPriceHandling = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbPriceType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbAmountPecent = new LSOne.Controls.NumericTextBox();
            this.lblAmountPercent = new System.Windows.Forms.Label();
            this.ntbQtyPerUnitOfMeasure = new LSOne.Controls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ntbMaxSelection = new LSOne.Controls.NumericTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbTriggerCode = new LSOne.Controls.DualDataComboBox();
            this.cmbTriggerFunction = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTriggerCode = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.lblUOM = new System.Windows.Forms.Label();
            this.chkQtyLinkedToTriggerLine = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblVariant = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // cmbPriceHandling
            // 
            this.cmbPriceHandling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriceHandling.FormattingEnabled = true;
            this.cmbPriceHandling.Items.AddRange(new object[] {
            resources.GetString("cmbPriceHandling.Items"),
            resources.GetString("cmbPriceHandling.Items1"),
            resources.GetString("cmbPriceHandling.Items2")});
            resources.ApplyResources(this.cmbPriceHandling, "cmbPriceHandling");
            this.cmbPriceHandling.Name = "cmbPriceHandling";
            this.cmbPriceHandling.SelectedIndexChanged += new System.EventHandler(this.cmbPriceHandling_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbPriceType
            // 
            this.cmbPriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriceType.FormattingEnabled = true;
            this.cmbPriceType.Items.AddRange(new object[] {
            resources.GetString("cmbPriceType.Items"),
            resources.GetString("cmbPriceType.Items1"),
            resources.GetString("cmbPriceType.Items2"),
            resources.GetString("cmbPriceType.Items3")});
            resources.ApplyResources(this.cmbPriceType, "cmbPriceType");
            this.cmbPriceType.Name = "cmbPriceType";
            this.cmbPriceType.SelectedIndexChanged += new System.EventHandler(this.cmbPriceType_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbAmountPecent
            // 
            this.ntbAmountPecent.AllowDecimal = false;
            this.ntbAmountPecent.AllowNegative = false;
            this.ntbAmountPecent.CultureInfo = null;
            this.ntbAmountPecent.DecimalLetters = 2;
            resources.ApplyResources(this.ntbAmountPecent, "ntbAmountPecent");
            this.ntbAmountPecent.ForeColor = System.Drawing.Color.Black;
            this.ntbAmountPecent.HasMinValue = false;
            this.ntbAmountPecent.MaxValue = 0D;
            this.ntbAmountPecent.MinValue = 0D;
            this.ntbAmountPecent.Name = "ntbAmountPecent";
            this.ntbAmountPecent.Value = 0D;
            // 
            // lblAmountPercent
            // 
            this.lblAmountPercent.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblAmountPercent, "lblAmountPercent");
            this.lblAmountPercent.Name = "lblAmountPercent";
            // 
            // ntbQtyPerUnitOfMeasure
            // 
            this.ntbQtyPerUnitOfMeasure.AllowDecimal = false;
            this.ntbQtyPerUnitOfMeasure.AllowNegative = false;
            this.ntbQtyPerUnitOfMeasure.CultureInfo = null;
            this.ntbQtyPerUnitOfMeasure.DecimalLetters = 2;
            this.ntbQtyPerUnitOfMeasure.ForeColor = System.Drawing.Color.Black;
            this.ntbQtyPerUnitOfMeasure.HasMinValue = false;
            resources.ApplyResources(this.ntbQtyPerUnitOfMeasure, "ntbQtyPerUnitOfMeasure");
            this.ntbQtyPerUnitOfMeasure.MaxValue = 0D;
            this.ntbQtyPerUnitOfMeasure.MinValue = 0D;
            this.ntbQtyPerUnitOfMeasure.Name = "ntbQtyPerUnitOfMeasure";
            this.ntbQtyPerUnitOfMeasure.Value = 0D;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            this.ntbMaxSelection.MaxValue = 0D;
            this.ntbMaxSelection.MinValue = 0D;
            this.ntbMaxSelection.Name = "ntbMaxSelection";
            this.ntbMaxSelection.Value = 0D;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbTriggerCode
            // 
            this.cmbTriggerCode.AddList = null;
            this.cmbTriggerCode.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTriggerCode, "cmbTriggerCode");
            this.cmbTriggerCode.EnableTextBox = true;
            this.cmbTriggerCode.MaxLength = 327;
            this.cmbTriggerCode.Name = "cmbTriggerCode";
            this.cmbTriggerCode.NoChangeAllowed = false;
            this.cmbTriggerCode.OnlyDisplayID = false;
            this.cmbTriggerCode.RemoveList = null;
            this.cmbTriggerCode.RowHeight = ((short)(22));
            this.cmbTriggerCode.SecondaryData = null;
            this.cmbTriggerCode.SelectedData = null;
            this.cmbTriggerCode.SelectedDataID = null;
            this.cmbTriggerCode.SelectionList = null;
            this.cmbTriggerCode.ShowDropDownOnTyping = true;
            this.cmbTriggerCode.SkipIDColumn = true;
            this.cmbTriggerCode.RequestData += new System.EventHandler(this.cmbTriggerCode_RequestData);
            this.cmbTriggerCode.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbTriggerCode_DropDown);
            this.cmbTriggerCode.SelectedDataChanged += new System.EventHandler(this.cmbTriggerCode_SelectedDataChanged);
            // 
            // cmbTriggerFunction
            // 
            this.cmbTriggerFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTriggerFunction.FormattingEnabled = true;
            this.cmbTriggerFunction.Items.AddRange(new object[] {
            resources.GetString("cmbTriggerFunction.Items"),
            resources.GetString("cmbTriggerFunction.Items1"),
            resources.GetString("cmbTriggerFunction.Items2"),
            resources.GetString("cmbTriggerFunction.Items3")});
            resources.ApplyResources(this.cmbTriggerFunction, "cmbTriggerFunction");
            this.cmbTriggerFunction.Name = "cmbTriggerFunction";
            this.cmbTriggerFunction.SelectedIndexChanged += new System.EventHandler(this.cmbTriggerFunction_SelectedIndexChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // lblTriggerCode
            // 
            resources.ApplyResources(this.lblTriggerCode, "lblTriggerCode");
            this.lblTriggerCode.Name = "lblTriggerCode";
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.MaxLength = 327;
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.NoChangeAllowed = false;
            this.cmbUnit.OnlyDisplayID = false;
            this.cmbUnit.RemoveList = null;
            this.cmbUnit.RowHeight = ((short)(22));
            this.cmbUnit.SecondaryData = null;
            this.cmbUnit.SelectedData = null;
            this.cmbUnit.SelectedDataID = null;
            this.cmbUnit.SelectionList = null;
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.RequestData += new System.EventHandler(this.cmbUnit_RequestData);
            // 
            // lblUOM
            // 
            resources.ApplyResources(this.lblUOM, "lblUOM");
            this.lblUOM.Name = "lblUOM";
            // 
            // chkQtyLinkedToTriggerLine
            // 
            resources.ApplyResources(this.chkQtyLinkedToTriggerLine, "chkQtyLinkedToTriggerLine");
            this.chkQtyLinkedToTriggerLine.Name = "chkQtyLinkedToTriggerLine";
            this.chkQtyLinkedToTriggerLine.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblVariant
            // 
            resources.ApplyResources(this.lblVariant, "lblVariant");
            this.lblVariant.Name = "lblVariant";
            // 
            // cmbVariant
            // 
            this.cmbVariant.AddList = null;
            this.cmbVariant.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariant, "cmbVariant");
            this.cmbVariant.EnableTextBox = true;
            this.cmbVariant.MaxLength = 32767;
            this.cmbVariant.Name = "cmbVariant";
            this.cmbVariant.NoChangeAllowed = false;
            this.cmbVariant.OnlyDisplayID = false;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.ShowDropDownOnTyping = true;
            this.cmbVariant.SkipIDColumn = true;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // SubcodeGeneralPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkQtyLinkedToTriggerLine);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblUOM);
            this.Controls.Add(this.lblTriggerCode);
            this.Controls.Add(this.cmbTriggerFunction);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbTriggerCode);
            this.Controls.Add(this.ntbMaxSelection);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ntbQtyPerUnitOfMeasure);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ntbAmountPecent);
            this.Controls.Add(this.lblAmountPercent);
            this.Controls.Add(this.cmbPriceType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbPriceHandling);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbID);
            this.DoubleBuffered = true;
            this.Name = "SubcodeGeneralPage";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.ComboBox cmbPriceHandling;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbPriceType;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbAmountPecent;
        private System.Windows.Forms.Label lblAmountPercent;
        private NumericTextBox ntbQtyPerUnitOfMeasure;
        private System.Windows.Forms.Label label3;
        private NumericTextBox ntbMaxSelection;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbTriggerCode;
        private System.Windows.Forms.ComboBox cmbTriggerFunction;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblTriggerCode;
        private DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label lblUOM;
        private System.Windows.Forms.CheckBox chkQtyLinkedToTriggerLine;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblVariant;
        private DualDataComboBox cmbVariant;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
