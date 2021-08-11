using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.Controls
{
    partial class ButtonPropertiesControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonPropertiesControl));
			this.cmbShape = new System.Windows.Forms.ComboBox();
			this.cmbGradientMode = new System.Windows.Forms.ComboBox();
			this.lblShape = new System.Windows.Forms.Label();
			this.lblGradientMode = new System.Windows.Forms.Label();
			this.cwBackColor2 = new LSOne.Controls.ColorWell();
			this.lblBackColor2 = new System.Windows.Forms.Label();
			this.ntbFontCharset = new LSOne.Controls.NumericTextBox();
			this.cwForeColor = new LSOne.Controls.ColorWell();
			this.cwBackColor = new LSOne.Controls.ColorWell();
			this.lblBackColor = new System.Windows.Forms.Label();
			this.lblFontCharset = new System.Windows.Forms.Label();
			this.lblFontColor = new System.Windows.Forms.Label();
			this.ntbFontSize = new LSOne.Controls.NumericTextBox();
			this.chkFontItalic = new System.Windows.Forms.CheckBox();
			this.chkFontBold = new System.Windows.Forms.CheckBox();
			this.lblFontItalic = new System.Windows.Forms.Label();
			this.lblFontBold = new System.Windows.Forms.Label();
			this.lblFontSize = new System.Windows.Forms.Label();
			this.btnEditFont = new LSOne.Controls.ContextButton();
			this.tbFontName = new System.Windows.Forms.TextBox();
			this.lblFontName = new System.Windows.Forms.Label();
			this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
			this.chkStrikethrough = new System.Windows.Forms.CheckBox();
			this.lblTextPosition = new System.Windows.Forms.Label();
			this.backColorFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.fontStyleFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.fontFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.lblStyle = new System.Windows.Forms.Label();
			this.styleFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.cmbStyle = new LSOne.Controls.DualDataComboBox();
			this.btnEditStyle = new LSOne.Controls.ContextButton();
			this.cmbTextPosition = new System.Windows.Forms.ComboBox();
			this.lblFontStrikethrough = new System.Windows.Forms.Label();
			this.mainLayout.SuspendLayout();
			this.backColorFlowLayout.SuspendLayout();
			this.fontStyleFlowLayout.SuspendLayout();
			this.fontFlowLayout.SuspendLayout();
			this.styleFlowLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmbShape
			// 
			this.cmbShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbShape.FormattingEnabled = true;
			resources.ApplyResources(this.cmbShape, "cmbShape");
			this.cmbShape.Name = "cmbShape";
			this.cmbShape.TabStop = false;
			this.cmbShape.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// cmbGradientMode
			// 
			this.cmbGradientMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbGradientMode.FormattingEnabled = true;
			resources.ApplyResources(this.cmbGradientMode, "cmbGradientMode");
			this.cmbGradientMode.Name = "cmbGradientMode";
			this.cmbGradientMode.TabStop = false;
			this.cmbGradientMode.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// lblShape
			// 
			resources.ApplyResources(this.lblShape, "lblShape");
			this.lblShape.BackColor = System.Drawing.Color.Transparent;
			this.lblShape.Name = "lblShape";
			// 
			// lblGradientMode
			// 
			resources.ApplyResources(this.lblGradientMode, "lblGradientMode");
			this.lblGradientMode.BackColor = System.Drawing.Color.Transparent;
			this.lblGradientMode.Name = "lblGradientMode";
			// 
			// cwBackColor2
			// 
			resources.ApplyResources(this.cwBackColor2, "cwBackColor2");
			this.cwBackColor2.Name = "cwBackColor2";
			this.cwBackColor2.SelectedColor = System.Drawing.Color.White;
			this.cwBackColor2.TabStop = false;
			this.cwBackColor2.SelectedColorChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// lblBackColor2
			// 
			resources.ApplyResources(this.lblBackColor2, "lblBackColor2");
			this.lblBackColor2.BackColor = System.Drawing.Color.Transparent;
			this.lblBackColor2.Name = "lblBackColor2";
			// 
			// ntbFontCharset
			// 
			this.ntbFontCharset.AllowDecimal = false;
			this.ntbFontCharset.AllowNegative = false;
			this.ntbFontCharset.CultureInfo = null;
			this.ntbFontCharset.DecimalLetters = 2;
			this.ntbFontCharset.DecimalLimit = null;
			this.ntbFontCharset.ForeColor = System.Drawing.Color.Black;
			this.ntbFontCharset.HasMinValue = false;
			resources.ApplyResources(this.ntbFontCharset, "ntbFontCharset");
			this.ntbFontCharset.MaxValue = 0D;
			this.ntbFontCharset.MinValue = 0D;
			this.ntbFontCharset.Name = "ntbFontCharset";
			this.ntbFontCharset.ReadOnly = true;
			this.ntbFontCharset.Value = 0D;
			// 
			// cwForeColor
			// 
			resources.ApplyResources(this.cwForeColor, "cwForeColor");
			this.cwForeColor.Name = "cwForeColor";
			this.cwForeColor.SelectedColor = System.Drawing.Color.White;
			this.cwForeColor.TabStop = false;
			this.cwForeColor.SelectedColorChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// cwBackColor
			// 
			resources.ApplyResources(this.cwBackColor, "cwBackColor");
			this.cwBackColor.Name = "cwBackColor";
			this.cwBackColor.SelectedColor = System.Drawing.Color.White;
			this.cwBackColor.TabStop = false;
			this.cwBackColor.SelectedColorChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// lblBackColor
			// 
			resources.ApplyResources(this.lblBackColor, "lblBackColor");
			this.lblBackColor.BackColor = System.Drawing.Color.Transparent;
			this.lblBackColor.Name = "lblBackColor";
			// 
			// lblFontCharset
			// 
			resources.ApplyResources(this.lblFontCharset, "lblFontCharset");
			this.lblFontCharset.BackColor = System.Drawing.Color.Transparent;
			this.lblFontCharset.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblFontCharset.Name = "lblFontCharset";
			// 
			// lblFontColor
			// 
			resources.ApplyResources(this.lblFontColor, "lblFontColor");
			this.lblFontColor.BackColor = System.Drawing.Color.Transparent;
			this.lblFontColor.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblFontColor.Name = "lblFontColor";
			// 
			// ntbFontSize
			// 
			this.ntbFontSize.AllowDecimal = false;
			this.ntbFontSize.AllowNegative = false;
			this.ntbFontSize.CultureInfo = null;
			this.ntbFontSize.DecimalLetters = 2;
			this.ntbFontSize.DecimalLimit = null;
			this.ntbFontSize.ForeColor = System.Drawing.Color.Black;
			this.ntbFontSize.HasMinValue = false;
			resources.ApplyResources(this.ntbFontSize, "ntbFontSize");
			this.ntbFontSize.MaxValue = 0D;
			this.ntbFontSize.MinValue = 0D;
			this.ntbFontSize.Name = "ntbFontSize";
			this.ntbFontSize.ReadOnly = true;
			this.ntbFontSize.Value = 0D;
			// 
			// chkFontItalic
			// 
			resources.ApplyResources(this.chkFontItalic, "chkFontItalic");
			this.chkFontItalic.BackColor = System.Drawing.Color.Transparent;
			this.chkFontItalic.Name = "chkFontItalic";
			this.chkFontItalic.TabStop = false;
			this.chkFontItalic.UseVisualStyleBackColor = false;
			this.chkFontItalic.CheckedChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// chkFontBold
			// 
			resources.ApplyResources(this.chkFontBold, "chkFontBold");
			this.chkFontBold.BackColor = System.Drawing.Color.Transparent;
			this.chkFontBold.Name = "chkFontBold";
			this.chkFontBold.TabStop = false;
			this.chkFontBold.UseVisualStyleBackColor = false;
			this.chkFontBold.CheckedChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// lblFontItalic
			// 
			resources.ApplyResources(this.lblFontItalic, "lblFontItalic");
			this.lblFontItalic.BackColor = System.Drawing.Color.Transparent;
			this.lblFontItalic.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblFontItalic.Name = "lblFontItalic";
			// 
			// lblFontBold
			// 
			resources.ApplyResources(this.lblFontBold, "lblFontBold");
			this.lblFontBold.BackColor = System.Drawing.Color.Transparent;
			this.lblFontBold.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblFontBold.Name = "lblFontBold";
			// 
			// lblFontSize
			// 
			resources.ApplyResources(this.lblFontSize, "lblFontSize");
			this.lblFontSize.BackColor = System.Drawing.Color.Transparent;
			this.lblFontSize.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblFontSize.Name = "lblFontSize";
			// 
			// btnEditFont
			// 
			this.btnEditFont.BackColor = System.Drawing.Color.Transparent;
			this.btnEditFont.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditFont, "btnEditFont");
			this.btnEditFont.Name = "btnEditFont";
			this.btnEditFont.TabStop = false;
			this.btnEditFont.Click += new System.EventHandler(this.btnEditFont_Click);
			// 
			// tbFontName
			// 
			resources.ApplyResources(this.tbFontName, "tbFontName");
			this.tbFontName.Name = "tbFontName";
			this.tbFontName.ReadOnly = true;
			this.tbFontName.TabStop = false;
			// 
			// lblFontName
			// 
			resources.ApplyResources(this.lblFontName, "lblFontName");
			this.lblFontName.BackColor = System.Drawing.Color.Transparent;
			this.lblFontName.Name = "lblFontName";
			// 
			// mainLayout
			// 
			resources.ApplyResources(this.mainLayout, "mainLayout");
			this.mainLayout.Controls.Add(this.chkStrikethrough, 1, 5);
			this.mainLayout.Controls.Add(this.lblTextPosition, 0, 10);
			this.mainLayout.Controls.Add(this.backColorFlowLayout, 1, 7);
			this.mainLayout.Controls.Add(this.lblFontName, 0, 1);
			this.mainLayout.Controls.Add(this.fontStyleFlowLayout, 1, 3);
			this.mainLayout.Controls.Add(this.cmbShape, 1, 9);
			this.mainLayout.Controls.Add(this.fontFlowLayout, 1, 1);
			this.mainLayout.Controls.Add(this.lblShape, 0, 9);
			this.mainLayout.Controls.Add(this.cmbGradientMode, 1, 8);
			this.mainLayout.Controls.Add(this.lblFontSize, 0, 2);
			this.mainLayout.Controls.Add(this.lblGradientMode, 0, 8);
			this.mainLayout.Controls.Add(this.ntbFontSize, 1, 2);
			this.mainLayout.Controls.Add(this.lblFontBold, 0, 3);
			this.mainLayout.Controls.Add(this.cwForeColor, 1, 6);
			this.mainLayout.Controls.Add(this.lblBackColor, 0, 7);
			this.mainLayout.Controls.Add(this.ntbFontCharset, 1, 4);
			this.mainLayout.Controls.Add(this.lblFontColor, 0, 6);
			this.mainLayout.Controls.Add(this.lblFontCharset, 0, 4);
			this.mainLayout.Controls.Add(this.lblStyle, 0, 0);
			this.mainLayout.Controls.Add(this.styleFlowLayout, 1, 0);
			this.mainLayout.Controls.Add(this.cmbTextPosition, 1, 10);
			this.mainLayout.Controls.Add(this.lblFontStrikethrough, 0, 5);
			this.mainLayout.Name = "mainLayout";
			// 
			// chkStrikethrough
			// 
			resources.ApplyResources(this.chkStrikethrough, "chkStrikethrough");
			this.chkStrikethrough.BackColor = System.Drawing.Color.Transparent;
			this.chkStrikethrough.Name = "chkStrikethrough";
			this.chkStrikethrough.UseVisualStyleBackColor = false;
			this.chkStrikethrough.CheckedChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// lblTextPosition
			// 
			resources.ApplyResources(this.lblTextPosition, "lblTextPosition");
			this.lblTextPosition.BackColor = System.Drawing.Color.Transparent;
			this.lblTextPosition.Name = "lblTextPosition";
			// 
			// backColorFlowLayout
			// 
			resources.ApplyResources(this.backColorFlowLayout, "backColorFlowLayout");
			this.backColorFlowLayout.Controls.Add(this.cwBackColor);
			this.backColorFlowLayout.Controls.Add(this.lblBackColor2);
			this.backColorFlowLayout.Controls.Add(this.cwBackColor2);
			this.backColorFlowLayout.Name = "backColorFlowLayout";
			// 
			// fontStyleFlowLayout
			// 
			resources.ApplyResources(this.fontStyleFlowLayout, "fontStyleFlowLayout");
			this.fontStyleFlowLayout.Controls.Add(this.chkFontBold);
			this.fontStyleFlowLayout.Controls.Add(this.lblFontItalic);
			this.fontStyleFlowLayout.Controls.Add(this.chkFontItalic);
			this.fontStyleFlowLayout.Name = "fontStyleFlowLayout";
			// 
			// fontFlowLayout
			// 
			resources.ApplyResources(this.fontFlowLayout, "fontFlowLayout");
			this.fontFlowLayout.Controls.Add(this.tbFontName);
			this.fontFlowLayout.Controls.Add(this.btnEditFont);
			this.fontFlowLayout.Name = "fontFlowLayout";
			// 
			// lblStyle
			// 
			resources.ApplyResources(this.lblStyle, "lblStyle");
			this.lblStyle.BackColor = System.Drawing.Color.Transparent;
			this.lblStyle.Name = "lblStyle";
			// 
			// styleFlowLayout
			// 
			resources.ApplyResources(this.styleFlowLayout, "styleFlowLayout");
			this.styleFlowLayout.Controls.Add(this.cmbStyle);
			this.styleFlowLayout.Controls.Add(this.btnEditStyle);
			this.styleFlowLayout.Name = "styleFlowLayout";
			// 
			// cmbStyle
			// 
			this.cmbStyle.AddList = null;
			this.cmbStyle.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbStyle, "cmbStyle");
			this.cmbStyle.MaxLength = 32767;
			this.cmbStyle.Name = "cmbStyle";
			this.cmbStyle.NoChangeAllowed = false;
			this.cmbStyle.OnlyDisplayID = false;
			this.cmbStyle.RemoveList = null;
			this.cmbStyle.RowHeight = ((short)(22));
			this.cmbStyle.SecondaryData = null;
			this.cmbStyle.SelectedData = null;
			this.cmbStyle.SelectedDataID = null;
			this.cmbStyle.SelectionList = null;
			this.cmbStyle.SkipIDColumn = true;
			this.cmbStyle.RequestData += new System.EventHandler(this.cmbStyle_RequestData);
			this.cmbStyle.SelectedDataChanged += new System.EventHandler(this.cmbStyle_SelectedDataChanged);
			this.cmbStyle.RequestClear += new System.EventHandler(this.cmbStyle_RequestClear);
			// 
			// btnEditStyle
			// 
			this.btnEditStyle.BackColor = System.Drawing.Color.Transparent;
			this.btnEditStyle.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditStyle, "btnEditStyle");
			this.btnEditStyle.Name = "btnEditStyle";
			this.btnEditStyle.Click += new System.EventHandler(this.btnEditStyle_Click);
			// 
			// cmbTextPosition
			// 
			this.cmbTextPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTextPosition.FormattingEnabled = true;
			resources.ApplyResources(this.cmbTextPosition, "cmbTextPosition");
			this.cmbTextPosition.Name = "cmbTextPosition";
			this.cmbTextPosition.TabStop = false;
			this.cmbTextPosition.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
			// 
			// lblFontStrikethrough
			// 
			resources.ApplyResources(this.lblFontStrikethrough, "lblFontStrikethrough");
			this.lblFontStrikethrough.BackColor = System.Drawing.Color.Transparent;
			this.lblFontStrikethrough.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblFontStrikethrough.Name = "lblFontStrikethrough";
			// 
			// ButtonPropertiesControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.mainLayout);
			this.Name = "ButtonPropertiesControl";
			this.mainLayout.ResumeLayout(false);
			this.mainLayout.PerformLayout();
			this.backColorFlowLayout.ResumeLayout(false);
			this.backColorFlowLayout.PerformLayout();
			this.fontStyleFlowLayout.ResumeLayout(false);
			this.fontStyleFlowLayout.PerformLayout();
			this.fontFlowLayout.ResumeLayout(false);
			this.fontFlowLayout.PerformLayout();
			this.styleFlowLayout.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbShape;
        private System.Windows.Forms.ComboBox cmbGradientMode;
        private System.Windows.Forms.Label lblShape;
        private System.Windows.Forms.Label lblGradientMode;
        private ColorWell cwBackColor2;
        private System.Windows.Forms.Label lblBackColor2;
        private NumericTextBox ntbFontCharset;
        private ColorWell cwForeColor;
        private ColorWell cwBackColor;
        private System.Windows.Forms.Label lblBackColor;
        private System.Windows.Forms.Label lblFontCharset;
        private System.Windows.Forms.Label lblFontColor;
        private NumericTextBox ntbFontSize;
        private System.Windows.Forms.CheckBox chkFontItalic;
        private System.Windows.Forms.CheckBox chkFontBold;
        private System.Windows.Forms.Label lblFontItalic;
        private System.Windows.Forms.Label lblFontBold;
        private System.Windows.Forms.Label lblFontSize;
        private ContextButton btnEditFont;
        private System.Windows.Forms.TextBox tbFontName;
        private System.Windows.Forms.Label lblFontName;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.FlowLayoutPanel backColorFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel fontStyleFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel fontFlowLayout;
        private System.Windows.Forms.Label lblStyle;
        private System.Windows.Forms.FlowLayoutPanel styleFlowLayout;
        private ContextButton btnEditStyle;
        private DualDataComboBox cmbStyle;
        private System.Windows.Forms.Label lblTextPosition;
        private System.Windows.Forms.ComboBox cmbTextPosition;
        private System.Windows.Forms.CheckBox chkStrikethrough;
        private System.Windows.Forms.Label lblFontStrikethrough;
    }
}
