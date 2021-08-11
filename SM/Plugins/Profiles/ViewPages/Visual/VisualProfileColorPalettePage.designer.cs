using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Visual
{
    partial class VisualProfileColorPalettePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualProfileColorPalettePage));
            this.lblPOSControlBorderColor = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.chkOverridePOSBorderColor = new System.Windows.Forms.CheckBox();
            this.cmbOtherButtonStyle = new LSOne.Controls.DualDataComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbNormalButtonStyle = new LSOne.Controls.DualDataComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbActionButtonStyle = new LSOne.Controls.DualDataComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbCancelButtonStyle = new LSOne.Controls.DualDataComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbConfirmButtonStyle = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnsEditAddOtherButtonStyle = new LSOne.Controls.ContextButtons();
            this.btnsEditAddNormalButtonStyle = new LSOne.Controls.ContextButtons();
            this.btnsEditAddActionButtonStyle = new LSOne.Controls.ContextButtons();
            this.btnsEditAddCancelButtonStyle = new LSOne.Controls.ContextButtons();
            this.btnsEditAddConfirmButtonStyle = new LSOne.Controls.ContextButtons();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.cwPOSControlBorderColor = new LSOne.Controls.ColorWell();
            this.linkFields2 = new LSOne.Controls.LinkFields();
            this.cwPOSSelectedRowColor = new LSOne.Controls.ColorWell();
            this.lblPOSSelectedRowColor = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.chkOverridePOSSelectedRowColor = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblPOSControlBorderColor
            // 
            this.lblPOSControlBorderColor.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPOSControlBorderColor, "lblPOSControlBorderColor");
            this.lblPOSControlBorderColor.Name = "lblPOSControlBorderColor";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // chkOverridePOSBorderColor
            // 
            resources.ApplyResources(this.chkOverridePOSBorderColor, "chkOverridePOSBorderColor");
            this.chkOverridePOSBorderColor.Name = "chkOverridePOSBorderColor";
            this.chkOverridePOSBorderColor.UseVisualStyleBackColor = true;
            this.chkOverridePOSBorderColor.CheckedChanged += new System.EventHandler(this.chkOverridePOSBorderColor_CheckedChanged);
            // 
            // cmbOtherButtonStyle
            // 
            this.cmbOtherButtonStyle.AddList = null;
            this.cmbOtherButtonStyle.AllowKeyboardSelection = false;
            this.cmbOtherButtonStyle.EnableTextBox = true;
            resources.ApplyResources(this.cmbOtherButtonStyle, "cmbOtherButtonStyle");
            this.cmbOtherButtonStyle.MaxLength = 32767;
            this.cmbOtherButtonStyle.Name = "cmbOtherButtonStyle";
            this.cmbOtherButtonStyle.NoChangeAllowed = false;
            this.cmbOtherButtonStyle.OnlyDisplayID = false;
            this.cmbOtherButtonStyle.RemoveList = null;
            this.cmbOtherButtonStyle.RowHeight = ((short)(22));
            this.cmbOtherButtonStyle.SecondaryData = null;
            this.cmbOtherButtonStyle.SelectedData = null;
            this.cmbOtherButtonStyle.SelectedDataID = null;
            this.cmbOtherButtonStyle.SelectionList = null;
            this.cmbOtherButtonStyle.SkipIDColumn = false;
            this.cmbOtherButtonStyle.RequestData += new System.EventHandler(this.colorPaletteStyleCombobox_RequestData);
            this.cmbOtherButtonStyle.SelectedDataChanged += new System.EventHandler(this.cmbOtherButtonStyle_SelectedDataChanged);
            this.cmbOtherButtonStyle.RequestClear += new System.EventHandler(this.colorPaletteStyleCombobox_RequestClear);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // cmbNormalButtonStyle
            // 
            this.cmbNormalButtonStyle.AddList = null;
            this.cmbNormalButtonStyle.AllowKeyboardSelection = false;
            this.cmbNormalButtonStyle.EnableTextBox = true;
            resources.ApplyResources(this.cmbNormalButtonStyle, "cmbNormalButtonStyle");
            this.cmbNormalButtonStyle.MaxLength = 32767;
            this.cmbNormalButtonStyle.Name = "cmbNormalButtonStyle";
            this.cmbNormalButtonStyle.NoChangeAllowed = false;
            this.cmbNormalButtonStyle.OnlyDisplayID = false;
            this.cmbNormalButtonStyle.RemoveList = null;
            this.cmbNormalButtonStyle.RowHeight = ((short)(22));
            this.cmbNormalButtonStyle.SecondaryData = null;
            this.cmbNormalButtonStyle.SelectedData = null;
            this.cmbNormalButtonStyle.SelectedDataID = null;
            this.cmbNormalButtonStyle.SelectionList = null;
            this.cmbNormalButtonStyle.SkipIDColumn = false;
            this.cmbNormalButtonStyle.RequestData += new System.EventHandler(this.colorPaletteStyleCombobox_RequestData);
            this.cmbNormalButtonStyle.SelectedDataChanged += new System.EventHandler(this.cmbNormalButtonStyle_SelectedDataChanged);
            this.cmbNormalButtonStyle.RequestClear += new System.EventHandler(this.colorPaletteStyleCombobox_RequestClear);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // cmbActionButtonStyle
            // 
            this.cmbActionButtonStyle.AddList = null;
            this.cmbActionButtonStyle.AllowKeyboardSelection = false;
            this.cmbActionButtonStyle.EnableTextBox = true;
            resources.ApplyResources(this.cmbActionButtonStyle, "cmbActionButtonStyle");
            this.cmbActionButtonStyle.MaxLength = 32767;
            this.cmbActionButtonStyle.Name = "cmbActionButtonStyle";
            this.cmbActionButtonStyle.NoChangeAllowed = false;
            this.cmbActionButtonStyle.OnlyDisplayID = false;
            this.cmbActionButtonStyle.RemoveList = null;
            this.cmbActionButtonStyle.RowHeight = ((short)(22));
            this.cmbActionButtonStyle.SecondaryData = null;
            this.cmbActionButtonStyle.SelectedData = null;
            this.cmbActionButtonStyle.SelectedDataID = null;
            this.cmbActionButtonStyle.SelectionList = null;
            this.cmbActionButtonStyle.SkipIDColumn = false;
            this.cmbActionButtonStyle.RequestData += new System.EventHandler(this.colorPaletteStyleCombobox_RequestData);
            this.cmbActionButtonStyle.SelectedDataChanged += new System.EventHandler(this.cmbActionButtonStyle_SelectedDataChanged);
            this.cmbActionButtonStyle.RequestClear += new System.EventHandler(this.colorPaletteStyleCombobox_RequestClear);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmbCancelButtonStyle
            // 
            this.cmbCancelButtonStyle.AddList = null;
            this.cmbCancelButtonStyle.AllowKeyboardSelection = false;
            this.cmbCancelButtonStyle.EnableTextBox = true;
            resources.ApplyResources(this.cmbCancelButtonStyle, "cmbCancelButtonStyle");
            this.cmbCancelButtonStyle.MaxLength = 32767;
            this.cmbCancelButtonStyle.Name = "cmbCancelButtonStyle";
            this.cmbCancelButtonStyle.NoChangeAllowed = false;
            this.cmbCancelButtonStyle.OnlyDisplayID = false;
            this.cmbCancelButtonStyle.RemoveList = null;
            this.cmbCancelButtonStyle.RowHeight = ((short)(22));
            this.cmbCancelButtonStyle.SecondaryData = null;
            this.cmbCancelButtonStyle.SelectedData = null;
            this.cmbCancelButtonStyle.SelectedDataID = null;
            this.cmbCancelButtonStyle.SelectionList = null;
            this.cmbCancelButtonStyle.SkipIDColumn = false;
            this.cmbCancelButtonStyle.RequestData += new System.EventHandler(this.colorPaletteStyleCombobox_RequestData);
            this.cmbCancelButtonStyle.SelectedDataChanged += new System.EventHandler(this.cmbCancelButtonStyle_SelectedDataChanged);
            this.cmbCancelButtonStyle.RequestClear += new System.EventHandler(this.colorPaletteStyleCombobox_RequestClear);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmbConfirmButtonStyle
            // 
            this.cmbConfirmButtonStyle.AddList = null;
            this.cmbConfirmButtonStyle.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbConfirmButtonStyle, "cmbConfirmButtonStyle");
            this.cmbConfirmButtonStyle.MaxLength = 32767;
            this.cmbConfirmButtonStyle.Name = "cmbConfirmButtonStyle";
            this.cmbConfirmButtonStyle.NoChangeAllowed = false;
            this.cmbConfirmButtonStyle.OnlyDisplayID = false;
            this.cmbConfirmButtonStyle.RemoveList = null;
            this.cmbConfirmButtonStyle.RowHeight = ((short)(22));
            this.cmbConfirmButtonStyle.SecondaryData = null;
            this.cmbConfirmButtonStyle.SelectedData = null;
            this.cmbConfirmButtonStyle.SelectedDataID = null;
            this.cmbConfirmButtonStyle.SelectionList = null;
            this.cmbConfirmButtonStyle.SkipIDColumn = false;
            this.cmbConfirmButtonStyle.RequestData += new System.EventHandler(this.colorPaletteStyleCombobox_RequestData);
            this.cmbConfirmButtonStyle.SelectedDataChanged += new System.EventHandler(this.cmbConfirmButtonStyle_SelectedDataChanged);
            this.cmbConfirmButtonStyle.RequestClear += new System.EventHandler(this.colorPaletteStyleCombobox_RequestClear);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // btnPreview
            // 
            resources.ApplyResources(this.btnPreview, "btnPreview");
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnsEditAddOtherButtonStyle
            // 
            this.btnsEditAddOtherButtonStyle.AddButtonEnabled = true;
            this.btnsEditAddOtherButtonStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddOtherButtonStyle.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddOtherButtonStyle.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddOtherButtonStyle, "btnsEditAddOtherButtonStyle");
            this.btnsEditAddOtherButtonStyle.Name = "btnsEditAddOtherButtonStyle";
            this.btnsEditAddOtherButtonStyle.RemoveButtonEnabled = false;
            this.btnsEditAddOtherButtonStyle.EditButtonClicked += new System.EventHandler(this.btnsEditAddOtherButtonStyle_EditButtonClicked);
            this.btnsEditAddOtherButtonStyle.AddButtonClicked += new System.EventHandler(this.btnsEditAddOtherButtonStyle_AddButtonClicked);
            // 
            // btnsEditAddNormalButtonStyle
            // 
            this.btnsEditAddNormalButtonStyle.AddButtonEnabled = true;
            this.btnsEditAddNormalButtonStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddNormalButtonStyle.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddNormalButtonStyle.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddNormalButtonStyle, "btnsEditAddNormalButtonStyle");
            this.btnsEditAddNormalButtonStyle.Name = "btnsEditAddNormalButtonStyle";
            this.btnsEditAddNormalButtonStyle.RemoveButtonEnabled = false;
            this.btnsEditAddNormalButtonStyle.EditButtonClicked += new System.EventHandler(this.btnsEditAddNormalButtonStyle_EditButtonClicked);
            this.btnsEditAddNormalButtonStyle.AddButtonClicked += new System.EventHandler(this.btnsEditAddNormalButtonStyle_AddButtonClicked);
            // 
            // btnsEditAddActionButtonStyle
            // 
            this.btnsEditAddActionButtonStyle.AddButtonEnabled = true;
            this.btnsEditAddActionButtonStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddActionButtonStyle.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddActionButtonStyle.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddActionButtonStyle, "btnsEditAddActionButtonStyle");
            this.btnsEditAddActionButtonStyle.Name = "btnsEditAddActionButtonStyle";
            this.btnsEditAddActionButtonStyle.RemoveButtonEnabled = false;
            this.btnsEditAddActionButtonStyle.EditButtonClicked += new System.EventHandler(this.btnsEditAddActionButtonStyle_EditButtonClicked);
            this.btnsEditAddActionButtonStyle.AddButtonClicked += new System.EventHandler(this.btnsEditAddActionButtonStyle_AddButtonClicked);
            // 
            // btnsEditAddCancelButtonStyle
            // 
            this.btnsEditAddCancelButtonStyle.AddButtonEnabled = true;
            this.btnsEditAddCancelButtonStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddCancelButtonStyle.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddCancelButtonStyle.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddCancelButtonStyle, "btnsEditAddCancelButtonStyle");
            this.btnsEditAddCancelButtonStyle.Name = "btnsEditAddCancelButtonStyle";
            this.btnsEditAddCancelButtonStyle.RemoveButtonEnabled = false;
            this.btnsEditAddCancelButtonStyle.EditButtonClicked += new System.EventHandler(this.btnsEditAddCancelButtonStyle_EditButtonClicked);
            this.btnsEditAddCancelButtonStyle.AddButtonClicked += new System.EventHandler(this.btnsEditAddCancelButtonStyle_AddButtonClicked);
            // 
            // btnsEditAddConfirmButtonStyle
            // 
            this.btnsEditAddConfirmButtonStyle.AddButtonEnabled = true;
            this.btnsEditAddConfirmButtonStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddConfirmButtonStyle.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddConfirmButtonStyle.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddConfirmButtonStyle, "btnsEditAddConfirmButtonStyle");
            this.btnsEditAddConfirmButtonStyle.Name = "btnsEditAddConfirmButtonStyle";
            this.btnsEditAddConfirmButtonStyle.RemoveButtonEnabled = false;
            this.btnsEditAddConfirmButtonStyle.EditButtonClicked += new System.EventHandler(this.btnsEditAddConfirmButtonStyle_EditButtonClicked);
            this.btnsEditAddConfirmButtonStyle.AddButtonClicked += new System.EventHandler(this.btnsEditAddConfirmButtonStyle_AddButtonClicked);
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // cwPOSControlBorderColor
            // 
            resources.ApplyResources(this.cwPOSControlBorderColor, "cwPOSControlBorderColor");
            this.cwPOSControlBorderColor.Name = "cwPOSControlBorderColor";
            this.cwPOSControlBorderColor.SelectedColor = System.Drawing.Color.White;
            // 
            // linkFields2
            // 
            this.linkFields2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields2, "linkFields2");
            this.linkFields2.Name = "linkFields2";
            this.linkFields2.TabStop = false;
            // 
            // cwPOSSelectedRowColor
            // 
            resources.ApplyResources(this.cwPOSSelectedRowColor, "cwPOSSelectedRowColor");
            this.cwPOSSelectedRowColor.Name = "cwPOSSelectedRowColor";
            this.cwPOSSelectedRowColor.SelectedColor = System.Drawing.Color.White;
            // 
            // lblPOSSelectedRowColor
            // 
            this.lblPOSSelectedRowColor.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPOSSelectedRowColor, "lblPOSSelectedRowColor");
            this.lblPOSSelectedRowColor.Name = "lblPOSSelectedRowColor";
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // chkOverridePOSSelectedRowColor
            // 
            resources.ApplyResources(this.chkOverridePOSSelectedRowColor, "chkOverridePOSSelectedRowColor");
            this.chkOverridePOSSelectedRowColor.Name = "chkOverridePOSSelectedRowColor";
            this.chkOverridePOSSelectedRowColor.UseVisualStyleBackColor = true;
            this.chkOverridePOSSelectedRowColor.CheckedChanged += new System.EventHandler(this.chkOverridePOSSelectedRowColor_CheckedChanged);
            // 
            // VisualProfileColorPalettePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.linkFields2);
            this.Controls.Add(this.cwPOSSelectedRowColor);
            this.Controls.Add(this.lblPOSSelectedRowColor);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.chkOverridePOSSelectedRowColor);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnsEditAddOtherButtonStyle);
            this.Controls.Add(this.btnsEditAddNormalButtonStyle);
            this.Controls.Add(this.btnsEditAddActionButtonStyle);
            this.Controls.Add(this.btnsEditAddCancelButtonStyle);
            this.Controls.Add(this.btnsEditAddConfirmButtonStyle);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.cwPOSControlBorderColor);
            this.Controls.Add(this.lblPOSControlBorderColor);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.chkOverridePOSBorderColor);
            this.Controls.Add(this.cmbOtherButtonStyle);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbNormalButtonStyle);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cmbActionButtonStyle);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbCancelButtonStyle);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbConfirmButtonStyle);
            this.Controls.Add(this.label7);
            this.DoubleBuffered = true;
            this.Name = "VisualProfileColorPalettePage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextButtons btnsEditAddOtherButtonStyle;
        private ContextButtons btnsEditAddNormalButtonStyle;
        private ContextButtons btnsEditAddActionButtonStyle;
        private ContextButtons btnsEditAddCancelButtonStyle;
        private ContextButtons btnsEditAddConfirmButtonStyle;
        private LinkFields linkFields1;
        private ColorWell cwPOSControlBorderColor;
        private System.Windows.Forms.Label lblPOSControlBorderColor;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkOverridePOSBorderColor;
        private DualDataComboBox cmbOtherButtonStyle;
        private System.Windows.Forms.Label label12;
        private DualDataComboBox cmbNormalButtonStyle;
        private System.Windows.Forms.Label label11;
        private DualDataComboBox cmbActionButtonStyle;
        private System.Windows.Forms.Label label10;
        private DualDataComboBox cmbCancelButtonStyle;
        private System.Windows.Forms.Label label9;
        private DualDataComboBox cmbConfirmButtonStyle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnPreview;
        private LinkFields linkFields2;
        private ColorWell cwPOSSelectedRowColor;
        private System.Windows.Forms.Label lblPOSSelectedRowColor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkOverridePOSSelectedRowColor;
    }
}
