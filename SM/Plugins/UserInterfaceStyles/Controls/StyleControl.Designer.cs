using LSOne.Controls;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Controls
{
    partial class StyleControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleControl));
            this.grpPreviewButton = new System.Windows.Forms.GroupBox();
            this.btnMenuButtonPreview = new LSOne.Controls.MenuButton();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.chkStrikethrough = new System.Windows.Forms.CheckBox();
            this.backColorFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.cwBackColor = new LSOne.Controls.ColorWell();
            this.lblBackColor2 = new System.Windows.Forms.Label();
            this.cwBackColor2 = new LSOne.Controls.ColorWell();
            this.lblFontName = new System.Windows.Forms.Label();
            this.fontStyleFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.chkFontBold = new System.Windows.Forms.CheckBox();
            this.lblFontItalic = new System.Windows.Forms.Label();
            this.chkFontItalic = new System.Windows.Forms.CheckBox();
            this.cmbShape = new System.Windows.Forms.ComboBox();
            this.fontFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.tbFontName = new System.Windows.Forms.TextBox();
            this.btnEditFont = new LSOne.Controls.ContextButton();
            this.lblShape = new System.Windows.Forms.Label();
            this.cmbGradientMode = new System.Windows.Forms.ComboBox();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.lblGradientMode = new System.Windows.Forms.Label();
            this.ntbFontSize = new LSOne.Controls.NumericTextBox();
            this.lblFontBold = new System.Windows.Forms.Label();
            this.cwForeColor = new LSOne.Controls.ColorWell();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.ntbFontCharset = new LSOne.Controls.NumericTextBox();
            this.lblFontColor = new System.Windows.Forms.Label();
            this.lblFontCharset = new System.Windows.Forms.Label();
            this.styleFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.grpPreviewButton.SuspendLayout();
            this.mainLayout.SuspendLayout();
            this.backColorFlowLayout.SuspendLayout();
            this.fontStyleFlowLayout.SuspendLayout();
            this.fontFlowLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPreviewButton
            // 
            resources.ApplyResources(this.grpPreviewButton, "grpPreviewButton");
            this.grpPreviewButton.Controls.Add(this.btnMenuButtonPreview);
            this.grpPreviewButton.Name = "grpPreviewButton";
            this.grpPreviewButton.TabStop = false;
            // 
            // btnMenuButtonPreview
            // 
            this.btnMenuButtonPreview.AllowDrop = true;
            resources.ApplyResources(this.btnMenuButtonPreview, "btnMenuButtonPreview");
            this.btnMenuButtonPreview.BackColor = System.Drawing.Color.Transparent;
            this.btnMenuButtonPreview.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnMenuButtonPreview.BorderColorInt = -4934476;
            this.btnMenuButtonPreview.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnMenuButtonPreview.ButtonColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.btnMenuButtonPreview.ButtonColor2Int = -1842205;
            this.btnMenuButtonPreview.ButtonColorInt = -986896;
            this.btnMenuButtonPreview.ButtonFacingInt = 1;
            this.btnMenuButtonPreview.ButtonImagePositionInt = 0;
            this.btnMenuButtonPreview.ButtonKey = -1;
            this.btnMenuButtonPreview.ColumnIndex = -1;
            this.btnMenuButtonPreview.Designing = false;
            this.btnMenuButtonPreview.DragDropEnabled = false;
            this.btnMenuButtonPreview.FontSize = 11F;
            this.btnMenuButtonPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnMenuButtonPreview.ForeColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnMenuButtonPreview.ForeColor2Int = -16777216;
            this.btnMenuButtonPreview.ForeColorInt = -16777216;
            this.btnMenuButtonPreview.Glyph1Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnMenuButtonPreview.Glyph1ColorInt = -16777216;
            this.btnMenuButtonPreview.Glyph1Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnMenuButtonPreview.Glyph1FontName = "Microsoft Sans Serif";
            this.btnMenuButtonPreview.Glyph1FontSize = 8.25F;
            this.btnMenuButtonPreview.Glyph2Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnMenuButtonPreview.Glyph2ColorInt = -16777216;
            this.btnMenuButtonPreview.Glyph2Font = null;
            this.btnMenuButtonPreview.Glyph2FontName = "";
            this.btnMenuButtonPreview.Glyph2FontSize = 8F;
            this.btnMenuButtonPreview.Glyph3Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnMenuButtonPreview.Glyph3ColorInt = -16777216;
            this.btnMenuButtonPreview.Glyph3Font = null;
            this.btnMenuButtonPreview.Glyph3FontName = "";
            this.btnMenuButtonPreview.Glyph3FontSize = 8F;
            this.btnMenuButtonPreview.Glyph4Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnMenuButtonPreview.Glyph4ColorInt = -16777216;
            this.btnMenuButtonPreview.Glyph4Font = null;
            this.btnMenuButtonPreview.Glyph4FontName = "";
            this.btnMenuButtonPreview.Glyph4FontSize = 8F;
            this.btnMenuButtonPreview.GradientModeInt = 2;
            this.btnMenuButtonPreview.HeightMM = 2170;
            this.btnMenuButtonPreview.Highlighted = false;
            this.btnMenuButtonPreview.HotKey = System.Windows.Forms.Keys.None;
            this.btnMenuButtonPreview.Image = null;
            this.btnMenuButtonPreview.ImageFont = null;
            this.btnMenuButtonPreview.ImageText = "";
            this.btnMenuButtonPreview.ImageTextColor = System.Drawing.Color.Black;
            this.btnMenuButtonPreview.IsDirty = false;
            this.btnMenuButtonPreview.MenuID = null;
            this.btnMenuButtonPreview.MenuName = null;
            this.btnMenuButtonPreview.Name = "btnMenuButtonPreview";
            this.btnMenuButtonPreview.PushEffectInt = 0;
            this.btnMenuButtonPreview.Resizable = false;
            this.btnMenuButtonPreview.RowIndex = -1;
            this.btnMenuButtonPreview.ShapeInt = 0;
            this.btnMenuButtonPreview.SubTextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMenuButtonPreview.SubTextFontName = "Segoe UI";
            this.btnMenuButtonPreview.SubTextFontSize = 9F;
            this.btnMenuButtonPreview.TabStop = false;
            this.btnMenuButtonPreview.TextAlignmentInt = 32;
            this.btnMenuButtonPreview.TextGradientModeInt = 0;
            this.btnMenuButtonPreview.WidthMM = 3360;
            this.btnMenuButtonPreview.XPos = 209;
            this.btnMenuButtonPreview.XPosMM = 5530;
            this.btnMenuButtonPreview.YPos = 61;
            this.btnMenuButtonPreview.YPosMM = 1614;
            // 
            // mainLayout
            // 
            resources.ApplyResources(this.mainLayout, "mainLayout");
            this.mainLayout.Controls.Add(this.chkStrikethrough, 1, 5);
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
            this.mainLayout.Controls.Add(this.styleFlowLayout, 1, 0);
            this.mainLayout.Controls.Add(this.label1, 0, 5);
            this.mainLayout.Name = "mainLayout";
            // 
            // chkStrikethrough
            // 
            resources.ApplyResources(this.chkStrikethrough, "chkStrikethrough");
            this.chkStrikethrough.BackColor = System.Drawing.Color.Transparent;
            this.chkStrikethrough.Name = "chkStrikethrough";
            this.chkStrikethrough.UseVisualStyleBackColor = false;
            this.chkStrikethrough.CheckedChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // backColorFlowLayout
            // 
            resources.ApplyResources(this.backColorFlowLayout, "backColorFlowLayout");
            this.backColorFlowLayout.Controls.Add(this.cwBackColor);
            this.backColorFlowLayout.Controls.Add(this.lblBackColor2);
            this.backColorFlowLayout.Controls.Add(this.cwBackColor2);
            this.backColorFlowLayout.Name = "backColorFlowLayout";
            // 
            // cwBackColor
            // 
            resources.ApplyResources(this.cwBackColor, "cwBackColor");
            this.cwBackColor.Name = "cwBackColor";
            this.cwBackColor.SelectedColor = System.Drawing.Color.White;
            this.cwBackColor.SelectedColorChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // lblBackColor2
            // 
            resources.ApplyResources(this.lblBackColor2, "lblBackColor2");
            this.lblBackColor2.BackColor = System.Drawing.Color.Transparent;
            this.lblBackColor2.Name = "lblBackColor2";
            // 
            // cwBackColor2
            // 
            resources.ApplyResources(this.cwBackColor2, "cwBackColor2");
            this.cwBackColor2.Name = "cwBackColor2";
            this.cwBackColor2.SelectedColor = System.Drawing.Color.White;
            this.cwBackColor2.SelectedColorChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // lblFontName
            // 
            resources.ApplyResources(this.lblFontName, "lblFontName");
            this.lblFontName.BackColor = System.Drawing.Color.Transparent;
            this.lblFontName.Name = "lblFontName";
            // 
            // fontStyleFlowLayout
            // 
            resources.ApplyResources(this.fontStyleFlowLayout, "fontStyleFlowLayout");
            this.fontStyleFlowLayout.Controls.Add(this.chkFontBold);
            this.fontStyleFlowLayout.Controls.Add(this.lblFontItalic);
            this.fontStyleFlowLayout.Controls.Add(this.chkFontItalic);
            this.fontStyleFlowLayout.Name = "fontStyleFlowLayout";
            // 
            // chkFontBold
            // 
            resources.ApplyResources(this.chkFontBold, "chkFontBold");
            this.chkFontBold.BackColor = System.Drawing.Color.Transparent;
            this.chkFontBold.Name = "chkFontBold";
            this.chkFontBold.UseVisualStyleBackColor = false;
            // 
            // lblFontItalic
            // 
            resources.ApplyResources(this.lblFontItalic, "lblFontItalic");
            this.lblFontItalic.BackColor = System.Drawing.Color.Transparent;
            this.lblFontItalic.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblFontItalic.Name = "lblFontItalic";
            // 
            // chkFontItalic
            // 
            resources.ApplyResources(this.chkFontItalic, "chkFontItalic");
            this.chkFontItalic.BackColor = System.Drawing.Color.Transparent;
            this.chkFontItalic.Name = "chkFontItalic";
            this.chkFontItalic.UseVisualStyleBackColor = false;
            // 
            // cmbShape
            // 
            this.cmbShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShape.FormattingEnabled = true;
            resources.ApplyResources(this.cmbShape, "cmbShape");
            this.cmbShape.Name = "cmbShape";
            this.cmbShape.SelectedIndexChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // fontFlowLayout
            // 
            resources.ApplyResources(this.fontFlowLayout, "fontFlowLayout");
            this.fontFlowLayout.Controls.Add(this.tbFontName);
            this.fontFlowLayout.Controls.Add(this.btnEditFont);
            this.fontFlowLayout.Name = "fontFlowLayout";
            // 
            // tbFontName
            // 
            resources.ApplyResources(this.tbFontName, "tbFontName");
            this.tbFontName.Name = "tbFontName";
            this.tbFontName.ReadOnly = true;
            this.tbFontName.TabStop = false;
            // 
            // btnEditFont
            // 
            this.btnEditFont.BackColor = System.Drawing.Color.Transparent;
            this.btnEditFont.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditFont, "btnEditFont");
            this.btnEditFont.Name = "btnEditFont";
            this.btnEditFont.Click += new System.EventHandler(this.btnEditFont_Click);
            // 
            // lblShape
            // 
            resources.ApplyResources(this.lblShape, "lblShape");
            this.lblShape.BackColor = System.Drawing.Color.Transparent;
            this.lblShape.Name = "lblShape";
            // 
            // cmbGradientMode
            // 
            this.cmbGradientMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradientMode.FormattingEnabled = true;
            resources.ApplyResources(this.cmbGradientMode, "cmbGradientMode");
            this.cmbGradientMode.Name = "cmbGradientMode";
            this.cmbGradientMode.SelectedIndexChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // lblFontSize
            // 
            resources.ApplyResources(this.lblFontSize, "lblFontSize");
            this.lblFontSize.BackColor = System.Drawing.Color.Transparent;
            this.lblFontSize.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblFontSize.Name = "lblFontSize";
            // 
            // lblGradientMode
            // 
            resources.ApplyResources(this.lblGradientMode, "lblGradientMode");
            this.lblGradientMode.BackColor = System.Drawing.Color.Transparent;
            this.lblGradientMode.Name = "lblGradientMode";
            // 
            // ntbFontSize
            // 
            this.ntbFontSize.AllowDecimal = false;
            this.ntbFontSize.AllowNegative = false;
            this.ntbFontSize.CultureInfo = null;
            this.ntbFontSize.DecimalLetters = 2;
            this.ntbFontSize.ForeColor = System.Drawing.Color.Black;
            this.ntbFontSize.HasMinValue = false;
            resources.ApplyResources(this.ntbFontSize, "ntbFontSize");
            this.ntbFontSize.MaxValue = 0D;
            this.ntbFontSize.MinValue = 0D;
            this.ntbFontSize.Name = "ntbFontSize";
            this.ntbFontSize.ReadOnly = true;
            this.ntbFontSize.TabStop = false;
            this.ntbFontSize.Value = 0D;
            // 
            // lblFontBold
            // 
            resources.ApplyResources(this.lblFontBold, "lblFontBold");
            this.lblFontBold.BackColor = System.Drawing.Color.Transparent;
            this.lblFontBold.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblFontBold.Name = "lblFontBold";
            // 
            // cwForeColor
            // 
            resources.ApplyResources(this.cwForeColor, "cwForeColor");
            this.cwForeColor.Name = "cwForeColor";
            this.cwForeColor.SelectedColor = System.Drawing.Color.White;
            this.cwForeColor.SelectedColorChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // lblBackColor
            // 
            resources.ApplyResources(this.lblBackColor, "lblBackColor");
            this.lblBackColor.BackColor = System.Drawing.Color.Transparent;
            this.lblBackColor.Name = "lblBackColor";
            // 
            // ntbFontCharset
            // 
            this.ntbFontCharset.AllowDecimal = false;
            this.ntbFontCharset.AllowNegative = false;
            this.ntbFontCharset.CultureInfo = null;
            this.ntbFontCharset.DecimalLetters = 2;
            this.ntbFontCharset.ForeColor = System.Drawing.Color.Black;
            this.ntbFontCharset.HasMinValue = false;
            resources.ApplyResources(this.ntbFontCharset, "ntbFontCharset");
            this.ntbFontCharset.MaxValue = 0D;
            this.ntbFontCharset.MinValue = 0D;
            this.ntbFontCharset.Name = "ntbFontCharset";
            this.ntbFontCharset.ReadOnly = true;
            this.ntbFontCharset.TabStop = false;
            this.ntbFontCharset.Value = 0D;
            // 
            // lblFontColor
            // 
            resources.ApplyResources(this.lblFontColor, "lblFontColor");
            this.lblFontColor.BackColor = System.Drawing.Color.Transparent;
            this.lblFontColor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFontColor.Name = "lblFontColor";
            // 
            // lblFontCharset
            // 
            resources.ApplyResources(this.lblFontCharset, "lblFontCharset");
            this.lblFontCharset.BackColor = System.Drawing.Color.Transparent;
            this.lblFontCharset.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblFontCharset.Name = "lblFontCharset";
            // 
            // styleFlowLayout
            // 
            resources.ApplyResources(this.styleFlowLayout, "styleFlowLayout");
            this.styleFlowLayout.Name = "styleFlowLayout";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Name = "label1";
            // 
            // StyleControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.grpPreviewButton);
            this.Name = "StyleControl";
            this.grpPreviewButton.ResumeLayout(false);
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.backColorFlowLayout.ResumeLayout(false);
            this.backColorFlowLayout.PerformLayout();
            this.fontStyleFlowLayout.ResumeLayout(false);
            this.fontStyleFlowLayout.PerformLayout();
            this.fontFlowLayout.ResumeLayout(false);
            this.fontFlowLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPreviewButton;
        private MenuButton btnMenuButtonPreview;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.FlowLayoutPanel backColorFlowLayout;
        private ColorWell cwBackColor;
        private System.Windows.Forms.Label lblBackColor2;
        private ColorWell cwBackColor2;
        private System.Windows.Forms.Label lblFontName;
        private System.Windows.Forms.FlowLayoutPanel fontStyleFlowLayout;
        private System.Windows.Forms.CheckBox chkFontBold;
        private System.Windows.Forms.Label lblFontItalic;
        private System.Windows.Forms.CheckBox chkFontItalic;
        private System.Windows.Forms.ComboBox cmbShape;
        private System.Windows.Forms.FlowLayoutPanel fontFlowLayout;
        private System.Windows.Forms.TextBox tbFontName;
        private ContextButton btnEditFont;
        private System.Windows.Forms.Label lblShape;
        private System.Windows.Forms.ComboBox cmbGradientMode;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.Label lblGradientMode;
        private NumericTextBox ntbFontSize;
        private System.Windows.Forms.Label lblFontBold;
        private ColorWell cwForeColor;
        private System.Windows.Forms.Label lblBackColor;
        private NumericTextBox ntbFontCharset;
        private System.Windows.Forms.Label lblFontColor;
        private System.Windows.Forms.Label lblFontCharset;
        private System.Windows.Forms.FlowLayoutPanel styleFlowLayout;
        private System.Windows.Forms.CheckBox chkStrikethrough;
        private System.Windows.Forms.Label label1;
    }
}
