using System;
using LSOne.Controls;

namespace LSOne.ViewPlugins.PaymentLimitations.Dialogs
{
    partial class PaymentLimitationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentLimitationDialog));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkIncluded = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateAnother = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.cmbRelation = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbLimitationCode = new LSOne.Controls.DualDataComboBox();
            this.chkTaxExempt = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnsEditAdd = new LSOne.Controls.ContextButtons();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkIncluded
            // 
            resources.ApplyResources(this.chkIncluded, "chkIncluded");
            this.chkIncluded.Checked = true;
            this.chkIncluded.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncluded.Name = "chkIncluded";
            this.chkIncluded.UseVisualStyleBackColor = true;
            this.chkIncluded.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.chkCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // chkCreateAnother
            // 
            resources.ApplyResources(this.chkCreateAnother, "chkCreateAnother");
            this.chkCreateAnother.Name = "chkCreateAnother";
            this.chkCreateAnother.UseVisualStyleBackColor = true;
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
            this.cmbVariant.ReceiveKeyboardEvents = true;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.SkipIDColumn = true;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
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
            this.cmbRelation.SkipIDColumn = false;
            this.cmbRelation.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRelation_DropDown);
            this.cmbRelation.SelectedDataChanged += new System.EventHandler(this.cmbRelation_SelectedDataChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbLimitationCode
            // 
            this.cmbLimitationCode.AddList = null;
            this.cmbLimitationCode.AllowKeyboardSelection = false;
            this.cmbLimitationCode.EnableTextBox = true;
            resources.ApplyResources(this.cmbLimitationCode, "cmbLimitationCode");
            this.cmbLimitationCode.MaxLength = 32767;
            this.cmbLimitationCode.Name = "cmbLimitationCode";
            this.cmbLimitationCode.NoChangeAllowed = false;
            this.cmbLimitationCode.OnlyDisplayID = false;
            this.cmbLimitationCode.RemoveList = null;
            this.cmbLimitationCode.RowHeight = ((short)(22));
            this.cmbLimitationCode.SecondaryData = null;
            this.cmbLimitationCode.SelectedData = null;
            this.cmbLimitationCode.SelectedDataID = null;
            this.cmbLimitationCode.SelectionList = null;
            this.cmbLimitationCode.ShowDropDownOnTyping = true;
            this.cmbLimitationCode.SkipIDColumn = true;
            this.cmbLimitationCode.RequestData += new System.EventHandler(this.cmbLimitationCode_RequestData);
            this.cmbLimitationCode.SelectedDataChanged += new System.EventHandler(this.CmbLimitationCode_SelectedDataChanged);
            // 
            // chkTaxExempt
            // 
            resources.ApplyResources(this.chkTaxExempt, "chkTaxExempt");
            this.chkTaxExempt.Checked = true;
            this.chkTaxExempt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTaxExempt.Name = "chkTaxExempt";
            this.chkTaxExempt.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // btnsEditAdd
            // 
            this.btnsEditAdd.AddButtonEnabled = true;
            this.btnsEditAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAdd.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAdd.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAdd, "btnsEditAdd");
            this.btnsEditAdd.Name = "btnsEditAdd";
            this.btnsEditAdd.RemoveButtonEnabled = false;
            this.btnsEditAdd.EditButtonClicked += new System.EventHandler(this.BtnsEditAdd_EditButtonClicked);
            this.btnsEditAdd.AddButtonClicked += new System.EventHandler(this.BtnsEditAdd_AddButtonClicked);
            // 
            // PaymentLimitationDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnsEditAdd);
            this.Controls.Add(this.cmbLimitationCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.cmbRelation);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.chkIncluded);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkTaxExempt);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "PaymentLimitationDialog";
            this.Load += new System.EventHandler(this.PaymentLimitationDialog_Load);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.chkTaxExempt, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.chkIncluded, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.cmbRelation, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbLimitationCode, 0);
            this.Controls.SetChildIndex(this.btnsEditAdd, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }        
        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkIncluded;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.CheckBox chkCreateAnother;
        private System.Windows.Forms.ComboBox cmbType;
        private DualDataComboBox cmbVariant;
        private DualDataComboBox cmbRelation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbLimitationCode;
        private System.Windows.Forms.CheckBox chkTaxExempt;
        private System.Windows.Forms.Label label6;
        private ContextButtons btnsEditAdd;
    }
}