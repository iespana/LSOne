﻿using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.BarCodes.Views
{
    partial class BarCodeSetupDataView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BarCodeSetupDataView));
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblResolution = new System.Windows.Forms.Label();
			this.cmbBarCodeType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.ntbMaximumLength = new LSOne.Controls.NumericTextBox();
			this.ntbMinimumLength = new LSOne.Controls.NumericTextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmbBarCodeMask = new LSOne.Controls.DualDataComboBox();
			this.tbBarcodeMask = new System.Windows.Forms.TextBox();
			this.btnsEditAdd = new LSOne.Controls.ContextButtons();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.btnsEditAdd);
			this.pnlBottom.Controls.Add(this.tbBarcodeMask);
			this.pnlBottom.Controls.Add(this.cmbBarCodeMask);
			this.pnlBottom.Controls.Add(this.ntbMaximumLength);
			this.pnlBottom.Controls.Add(this.label7);
			this.pnlBottom.Controls.Add(this.ntbMinimumLength);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.label6);
			this.pnlBottom.Controls.Add(this.cmbBarCodeType);
			this.pnlBottom.Controls.Add(this.lblResolution);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.label2);
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// tbID
			// 
			this.tbID.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbID, "tbID");
			this.tbID.Name = "tbID";
			this.tbID.ReadOnly = true;
			this.tbID.BackColor = ColorPalette.BackgroundColor;
			this.tbID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// lblResolution
			// 
			this.lblResolution.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblResolution, "lblResolution");
			this.lblResolution.Name = "lblResolution";
			// 
			// cmbBarCodeType
			// 
			this.cmbBarCodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBarCodeType.FormattingEnabled = true;
			this.cmbBarCodeType.Items.AddRange(new object[] {
            resources.GetString("cmbBarCodeType.Items"),
            resources.GetString("cmbBarCodeType.Items1"),
            resources.GetString("cmbBarCodeType.Items2"),
            resources.GetString("cmbBarCodeType.Items3"),
            resources.GetString("cmbBarCodeType.Items4"),
            resources.GetString("cmbBarCodeType.Items5"),
            resources.GetString("cmbBarCodeType.Items6"),
            resources.GetString("cmbBarCodeType.Items7"),
            resources.GetString("cmbBarCodeType.Items8"),
            resources.GetString("cmbBarCodeType.Items9"),
            resources.GetString("cmbBarCodeType.Items10")});
			resources.ApplyResources(this.cmbBarCodeType, "cmbBarCodeType");
			this.cmbBarCodeType.Name = "cmbBarCodeType";
			this.cmbBarCodeType.SelectedIndexChanged += new System.EventHandler(this.cmbBarCodeType_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// ntbMaximumLength
			// 
			this.ntbMaximumLength.AllowDecimal = false;
			this.ntbMaximumLength.AllowNegative = false;
			this.ntbMaximumLength.CultureInfo = null;
			this.ntbMaximumLength.DecimalLetters = 2;
			this.ntbMaximumLength.ForeColor = System.Drawing.Color.Black;
			this.ntbMaximumLength.HasMinValue = false;
			resources.ApplyResources(this.ntbMaximumLength, "ntbMaximumLength");
			this.ntbMaximumLength.MaxValue = 200D;
			this.ntbMaximumLength.MinValue = 0D;
			this.ntbMaximumLength.Name = "ntbMaximumLength";
			this.ntbMaximumLength.Value = 0D;
			// 
			// ntbMinimumLength
			// 
			this.ntbMinimumLength.AllowDecimal = false;
			this.ntbMinimumLength.AllowNegative = false;
			this.ntbMinimumLength.CultureInfo = null;
			this.ntbMinimumLength.DecimalLetters = 2;
			this.ntbMinimumLength.ForeColor = System.Drawing.Color.Black;
			this.ntbMinimumLength.HasMinValue = false;
			resources.ApplyResources(this.ntbMinimumLength, "ntbMinimumLength");
			this.ntbMinimumLength.MaxValue = 200D;
			this.ntbMinimumLength.MinValue = 0D;
			this.ntbMinimumLength.Name = "ntbMinimumLength";
			this.ntbMinimumLength.Value = 0D;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// cmbBarCodeMask
			// 
			this.cmbBarCodeMask.AddList = null;
			this.cmbBarCodeMask.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbBarCodeMask, "cmbBarCodeMask");
			this.cmbBarCodeMask.MaxLength = 32767;
			this.cmbBarCodeMask.Name = "cmbBarCodeMask";
			this.cmbBarCodeMask.NoChangeAllowed = false;
			this.cmbBarCodeMask.OnlyDisplayID = true;
			this.cmbBarCodeMask.RemoveList = null;
			this.cmbBarCodeMask.RowHeight = ((short)(22));
			this.cmbBarCodeMask.SecondaryData = null;
			this.cmbBarCodeMask.SelectedData = null;
			this.cmbBarCodeMask.SelectedDataID = null;
			this.cmbBarCodeMask.SelectionList = null;
			this.cmbBarCodeMask.SkipIDColumn = false;
			this.cmbBarCodeMask.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbBarCodeMask_DropDown);
			this.cmbBarCodeMask.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbBarCodeMask_FormatData);
			this.cmbBarCodeMask.SelectedDataChanged += new System.EventHandler(this.cmbBarCodeMask_SelectedDataChanged);
			// 
			// tbBarcodeMask
			// 
			this.tbBarcodeMask.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbBarcodeMask, "tbBarcodeMask");
			this.tbBarcodeMask.Name = "tbBarcodeMask";
			this.tbBarcodeMask.ReadOnly = true;
			// 
			// btnsEditAdd
			// 
			this.btnsEditAdd.AddButtonEnabled = true;
			this.btnsEditAdd.BackColor = System.Drawing.Color.Transparent;
			this.btnsEditAdd.Context = LSOne.Controls.ButtonTypes.EditAdd;
			this.btnsEditAdd.EditButtonEnabled = true;
			resources.ApplyResources(this.btnsEditAdd, "btnsEditAdd");
			this.btnsEditAdd.Name = "btnsEditAdd";
			this.btnsEditAdd.RemoveButtonEnabled = false;
			this.btnsEditAdd.EditButtonClicked += new System.EventHandler(this.btnsEditAdd_EditButtonClicked);
			this.btnsEditAdd.AddButtonClicked += new System.EventHandler(this.btnAddBarCodeSetup_Click);
			// 
			// BarCodeSetupDataView
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.GrayHeaderHeight = 72;
			resources.ApplyResources(this, "$this");
			this.Name = "BarCodeSetupDataView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBarCodeType;
        private System.Windows.Forms.Label label7;
        private NumericTextBox ntbMinimumLength;
        private NumericTextBox ntbMaximumLength;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbBarCodeMask;
        private System.Windows.Forms.TextBox tbBarcodeMask;
        private ContextButtons btnsEditAdd;
    }
}
