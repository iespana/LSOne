using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class UnitConversionDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitConversionDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbItem = new LSOne.Controls.DualDataComboBox();
            this.lblItem = new System.Windows.Forms.Label();
            this.cmbConversionRuleFor = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFromUnit = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ntbFactor = new LSOne.Controls.NumericTextBox();
            this.cmbToUnit = new LSOne.Controls.DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lblExample = new System.Windows.Forms.Label();
            this.btnSwapUnits = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbItem
            // 
            this.cmbItem.AddList = null;
            this.cmbItem.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbItem, "cmbItem");
            this.cmbItem.EnableTextBox = true;
            this.cmbItem.MaxLength = 32767;
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.NoChangeAllowed = false;
            this.cmbItem.OnlyDisplayID = false;
            this.cmbItem.RemoveList = null;
            this.cmbItem.RowHeight = ((short)(22));
            this.cmbItem.SecondaryData = null;
            this.cmbItem.SelectedData = null;
            this.cmbItem.SelectedDataID = null;
            this.cmbItem.SelectionList = null;
            this.cmbItem.ShowDropDownOnTyping = true;
            this.cmbItem.SkipIDColumn = false;
            this.cmbItem.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbItem_DropDown);
            this.cmbItem.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblItem
            // 
            resources.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            // 
            // cmbConversionRuleFor
            // 
            this.cmbConversionRuleFor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConversionRuleFor.FormattingEnabled = true;
            this.cmbConversionRuleFor.Items.AddRange(new object[] {
            resources.GetString("cmbConversionRuleFor.Items"),
            resources.GetString("cmbConversionRuleFor.Items1")});
            resources.ApplyResources(this.cmbConversionRuleFor, "cmbConversionRuleFor");
            this.cmbConversionRuleFor.Name = "cmbConversionRuleFor";
            this.cmbConversionRuleFor.SelectedIndexChanged += new System.EventHandler(this.cmbConversionRuleFor_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbFromUnit
            // 
            this.cmbFromUnit.AddList = null;
            this.cmbFromUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFromUnit, "cmbFromUnit");
            this.cmbFromUnit.MaxLength = 32767;
            this.cmbFromUnit.Name = "cmbFromUnit";
            this.cmbFromUnit.NoChangeAllowed = false;
            this.cmbFromUnit.OnlyDisplayID = false;
            this.cmbFromUnit.RemoveList = null;
            this.cmbFromUnit.RowHeight = ((short)(22));
            this.cmbFromUnit.SecondaryData = null;
            this.cmbFromUnit.SelectedData = null;
            this.cmbFromUnit.SelectedDataID = null;
            this.cmbFromUnit.SelectionList = null;
            this.cmbFromUnit.SkipIDColumn = true;
            this.cmbFromUnit.RequestData += new System.EventHandler(this.cmbFromUnit_RequestData);
            this.cmbFromUnit.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // ntbFactor
            // 
            this.ntbFactor.AllowDecimal = true;
            this.ntbFactor.AllowNegative = false;
            this.ntbFactor.CultureInfo = null;
            this.ntbFactor.DecimalLetters = 2;
            this.ntbFactor.ForeColor = System.Drawing.Color.Black;
            this.ntbFactor.HasMinValue = false;
            resources.ApplyResources(this.ntbFactor, "ntbFactor");
            this.ntbFactor.MaxValue = 10000000D;
            this.ntbFactor.MinValue = 0D;
            this.ntbFactor.Name = "ntbFactor";
            this.ntbFactor.Value = 0D;
            this.ntbFactor.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbToUnit
            // 
            this.cmbToUnit.AddList = null;
            this.cmbToUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbToUnit, "cmbToUnit");
            this.cmbToUnit.MaxLength = 32767;
            this.cmbToUnit.Name = "cmbToUnit";
            this.cmbToUnit.NoChangeAllowed = false;
            this.cmbToUnit.OnlyDisplayID = false;
            this.cmbToUnit.RemoveList = null;
            this.cmbToUnit.RowHeight = ((short)(22));
            this.cmbToUnit.SecondaryData = null;
            this.cmbToUnit.SelectedData = null;
            this.cmbToUnit.SelectedDataID = null;
            this.cmbToUnit.SelectionList = null;
            this.cmbToUnit.SkipIDColumn = true;
            this.cmbToUnit.RequestData += new System.EventHandler(this.cmbToUnit_RequestData);
            this.cmbToUnit.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.lblExample);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lblExample
            // 
            this.lblExample.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblExample, "lblExample");
            this.lblExample.Name = "lblExample";
            // 
            // btnSwapUnits
            // 
            resources.ApplyResources(this.btnSwapUnits, "btnSwapUnits");
            this.btnSwapUnits.Name = "btnSwapUnits";
            this.btnSwapUnits.UseVisualStyleBackColor = true;
            this.btnSwapUnits.Click += new System.EventHandler(this.btnSwapUnits_Click);
            // 
            // UnitConversionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnSwapUnits);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.cmbToUnit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbFromUnit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbConversionRuleFor);
            this.Controls.Add(this.cmbItem);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.ntbFactor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "UnitConversionDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ntbFactor, 0);
            this.Controls.SetChildIndex(this.lblItem, 0);
            this.Controls.SetChildIndex(this.cmbItem, 0);
            this.Controls.SetChildIndex(this.cmbConversionRuleFor, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbFromUnit, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbToUnit, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.Controls.SetChildIndex(this.btnSwapUnits, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbConversionRuleFor;
        private DualDataComboBox cmbItem;
        private System.Windows.Forms.Label lblItem;
        private DualDataComboBox cmbToUnit;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbFromUnit;
        private System.Windows.Forms.Label label3;
        private NumericTextBox ntbFactor;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.Button btnSwapUnits;
    }
}