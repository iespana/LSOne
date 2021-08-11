using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class MultibuyLineDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultibuyLineDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.cmbRelation = new LSOne.Controls.DualDataComboBox();
            this.lblRelation = new System.Windows.Forms.Label();
            this.cmbVariantNumber = new LSOne.Controls.DualDataComboBox();
            this.lblVariantNumber = new System.Windows.Forms.Label();
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider3 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).BeginInit();
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
            this.errorProvider1.SetIconAlignment(this.btnOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnOK.IconAlignment"))));
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
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2"),
            resources.GetString("cmbType.Items3"),
            resources.GetString("cmbType.Items4")});
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // cmbRelation
            // 
            this.cmbRelation.AddList = null;
            this.cmbRelation.AllowKeyboardSelection = false;
            this.cmbRelation.EnableTextBox = true;
            resources.ApplyResources(this.cmbRelation, "cmbRelation");
            this.cmbRelation.MaxLength = 32767;
            this.cmbRelation.Name = "cmbRelation";
            this.cmbRelation.NoChangeAllowed = false;
            this.cmbRelation.OnlyDisplayID = false;
            this.cmbRelation.RemoveList = null;
            this.cmbRelation.RowHeight = ((short)(22));
            this.cmbRelation.SecondaryData = null;
            this.cmbRelation.SelectedData = null;
            this.cmbRelation.SelectedDataID = null;
            this.cmbRelation.SelectionList = null;
            this.cmbRelation.ShowDropDownOnTyping = true;
            this.cmbRelation.SkipIDColumn = true;
            this.cmbRelation.RequestData += new System.EventHandler(this.cmbRelation_RequestData);
            this.cmbRelation.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRelation_DropDown);
            this.cmbRelation.SelectedDataChanged += new System.EventHandler(this.cmbRelation_SelectedDataChanged);
            // 
            // lblRelation
            // 
            this.lblRelation.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRelation, "lblRelation");
            this.lblRelation.Name = "lblRelation";
            // 
            // cmbVariantNumber
            // 
            this.cmbVariantNumber.AddList = null;
            this.cmbVariantNumber.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariantNumber, "cmbVariantNumber");
            this.cmbVariantNumber.MaxLength = 32767;
            this.cmbVariantNumber.Name = "cmbVariantNumber";
            this.cmbVariantNumber.NoChangeAllowed = false;
            this.cmbVariantNumber.OnlyDisplayID = false;
            this.cmbVariantNumber.RemoveList = null;
            this.cmbVariantNumber.RowHeight = ((short)(22));
            this.cmbVariantNumber.SecondaryData = null;
            this.cmbVariantNumber.SelectedData = null;
            this.cmbVariantNumber.SelectedDataID = null;
            this.cmbVariantNumber.SelectionList = null;
            this.cmbVariantNumber.SkipIDColumn = true;
            this.cmbVariantNumber.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariantNumber_DropDown);
            // 
            // lblVariantNumber
            // 
            this.lblVariantNumber.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblVariantNumber, "lblVariantNumber");
            this.lblVariantNumber.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVariantNumber.Name = "lblVariantNumber";
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // errorProvider3
            // 
            this.errorProvider3.ContainerControl = this;
            // 
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUnit, "lblUnit");
            this.lblUnit.Name = "lblUnit";
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.EnableTextBox = true;
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
            this.cmbUnit.ShowDropDownOnTyping = true;
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.RequestData += new System.EventHandler(this.cmbUnit_RequestData);
            // 
            // MultibuyLineDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.cmbVariantNumber);
            this.Controls.Add(this.lblVariantNumber);
            this.Controls.Add(this.lblRelation);
            this.Controls.Add(this.cmbRelation);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "MultibuyLineDialog";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.cmbRelation, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.lblVariantNumber, 0);
            this.Controls.SetChildIndex(this.cmbVariantNumber, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.lblUnit, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblRelation;
        private DualDataComboBox cmbRelation;
        private System.Windows.Forms.ComboBox cmbType;
        private DualDataComboBox cmbVariantNumber;
        private System.Windows.Forms.Label lblVariantNumber;
        private System.Windows.Forms.ErrorProvider errorProvider2;
        private System.Windows.Forms.ErrorProvider errorProvider3;
        private System.Windows.Forms.Label lblUnit;
        private DualDataComboBox cmbUnit;
    }
}