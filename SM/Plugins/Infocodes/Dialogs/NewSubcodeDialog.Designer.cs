using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.Dialogs
{
    partial class NewSubcodeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewSubcodeDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.cmbTriggerFunction = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbTriggerCode = new LSOne.Controls.DualDataComboBox();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.lblUOM = new System.Windows.Forms.Label();
            this.lblVariant = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel2, "panel2");
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
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
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
            // cmbTriggerCode
            // 
            this.cmbTriggerCode.AddList = null;
            this.cmbTriggerCode.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTriggerCode, "cmbTriggerCode");
            this.cmbTriggerCode.EnableTextBox = true;
            this.cmbTriggerCode.MaxLength = 32767;
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
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.MaxLength = 32767;
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
            this.cmbVariant.RequestClear += new System.EventHandler(this.cmbVariant_RequestClear);
            // 
            // NewSubcodeDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblUOM);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbTriggerFunction);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbTriggerCode);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "NewSubcodeDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.cmbTriggerCode, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cmbTriggerFunction, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.lblUOM, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.Controls.SetChildIndex(this.lblVariant, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbTriggerFunction;
        private System.Windows.Forms.Label label8;
        private DualDataComboBox cmbTriggerCode;
        private DualDataComboBox cmbUnit;
        private System.Windows.Forms.Label lblUOM;
        private System.Windows.Forms.Label lblVariant;
        private DualDataComboBox cmbVariant;
    }
}
