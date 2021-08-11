using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class PosButtonGridMenuLineFontPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosButtonGridMenuLineFontPage));
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.lblFontName = new System.Windows.Forms.Label();
            this.btnEditFont = new LSOne.Controls.ContextButton();
            this.tbFontName = new System.Windows.Forms.TextBox();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.ntbFontSize = new LSOne.Controls.NumericTextBox();
            this.lblFontBold = new System.Windows.Forms.Label();
            this.lblFontItalic = new System.Windows.Forms.Label();
            this.lblFontStrikethrough = new System.Windows.Forms.Label();
            this.lblFontUnderline = new System.Windows.Forms.Label();
            this.lblFontColor = new System.Windows.Forms.Label();
            this.lblFontCharset = new System.Windows.Forms.Label();
            this.chkFontBold = new System.Windows.Forms.CheckBox();
            this.chkFontItalic = new System.Windows.Forms.CheckBox();
            this.chkFontStrikeThrough = new System.Windows.Forms.CheckBox();
            this.chkFontUnderline = new System.Windows.Forms.CheckBox();
            this.cwForeColor = new LSOne.Controls.ColorWell();
            this.ntbFontCharset = new LSOne.Controls.NumericTextBox();
            this.lblHeadConfiguration = new System.Windows.Forms.Label();
            this.chkUseHeaderConfiguration = new System.Windows.Forms.CheckBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblTextPosition = new System.Windows.Forms.Label();
            this.cmbTextPosition = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // fontDialog1
            // 
            this.fontDialog1.Color = System.Drawing.SystemColors.ControlText;
            // 
            // lblFontName
            // 
            resources.ApplyResources(this.lblFontName, "lblFontName");
            this.lblFontName.Name = "lblFontName";
            // 
            // btnEditFont
            // 
            this.btnEditFont.BackColor = System.Drawing.Color.Transparent;
            this.btnEditFont.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditFont, "btnEditFont");
            this.btnEditFont.Name = "btnEditFont";
            this.btnEditFont.Click += new System.EventHandler(this.btnEditFont_Click);
            // 
            // tbFontName
            // 
            resources.ApplyResources(this.tbFontName, "tbFontName");
            this.tbFontName.Name = "tbFontName";
            this.tbFontName.ReadOnly = true;
            this.tbFontName.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // lblFontSize
            // 
            resources.ApplyResources(this.lblFontSize, "lblFontSize");
            this.lblFontSize.Name = "lblFontSize";
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
            this.ntbFontSize.Value = 0D;
            this.ntbFontSize.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // lblFontBold
            // 
            resources.ApplyResources(this.lblFontBold, "lblFontBold");
            this.lblFontBold.Name = "lblFontBold";
            // 
            // lblFontItalic
            // 
            resources.ApplyResources(this.lblFontItalic, "lblFontItalic");
            this.lblFontItalic.Name = "lblFontItalic";
            // 
            // lblFontStrikethrough
            // 
            resources.ApplyResources(this.lblFontStrikethrough, "lblFontStrikethrough");
            this.lblFontStrikethrough.Name = "lblFontStrikethrough";
            // 
            // lblFontUnderline
            // 
            resources.ApplyResources(this.lblFontUnderline, "lblFontUnderline");
            this.lblFontUnderline.Name = "lblFontUnderline";
            // 
            // lblFontColor
            // 
            resources.ApplyResources(this.lblFontColor, "lblFontColor");
            this.lblFontColor.Name = "lblFontColor";
            // 
            // lblFontCharset
            // 
            resources.ApplyResources(this.lblFontCharset, "lblFontCharset");
            this.lblFontCharset.Name = "lblFontCharset";
            // 
            // chkFontBold
            // 
            resources.ApplyResources(this.chkFontBold, "chkFontBold");
            this.chkFontBold.Name = "chkFontBold";
            this.chkFontBold.UseVisualStyleBackColor = true;
            this.chkFontBold.CheckedChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // chkFontItalic
            // 
            resources.ApplyResources(this.chkFontItalic, "chkFontItalic");
            this.chkFontItalic.Name = "chkFontItalic";
            this.chkFontItalic.UseVisualStyleBackColor = true;
            this.chkFontItalic.CheckedChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // chkFontStrikeThrough
            // 
            resources.ApplyResources(this.chkFontStrikeThrough, "chkFontStrikeThrough");
            this.chkFontStrikeThrough.Name = "chkFontStrikeThrough";
            this.chkFontStrikeThrough.UseVisualStyleBackColor = true;
            this.chkFontStrikeThrough.CheckedChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // chkFontUnderline
            // 
            resources.ApplyResources(this.chkFontUnderline, "chkFontUnderline");
            this.chkFontUnderline.Name = "chkFontUnderline";
            this.chkFontUnderline.UseVisualStyleBackColor = true;
            this.chkFontUnderline.CheckedChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // cwForeColor
            // 
            resources.ApplyResources(this.cwForeColor, "cwForeColor");
            this.cwForeColor.Name = "cwForeColor";
            this.cwForeColor.SelectedColor = System.Drawing.Color.White;
            this.cwForeColor.SelectedColorChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
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
            this.ntbFontCharset.Value = 0D;
            this.ntbFontCharset.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // lblHeadConfiguration
            // 
            resources.ApplyResources(this.lblHeadConfiguration, "lblHeadConfiguration");
            this.lblHeadConfiguration.Name = "lblHeadConfiguration";
            // 
            // chkUseHeaderConfiguration
            // 
            resources.ApplyResources(this.chkUseHeaderConfiguration, "chkUseHeaderConfiguration");
            this.chkUseHeaderConfiguration.Name = "chkUseHeaderConfiguration";
            this.chkUseHeaderConfiguration.UseVisualStyleBackColor = true;
            this.chkUseHeaderConfiguration.CheckedChanged += new System.EventHandler(this.chkUseHeaderConfiguration_CheckedChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblTextPosition
            // 
            resources.ApplyResources(this.lblTextPosition, "lblTextPosition");
            this.lblTextPosition.Name = "lblTextPosition";
            // 
            // cmbTextPosition
            // 
            this.cmbTextPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTextPosition.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTextPosition, "cmbTextPosition");
            this.cmbTextPosition.Name = "cmbTextPosition";
            // 
            // PosButtonGridMenuLineFontPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbTextPosition);
            this.Controls.Add(this.lblTextPosition);
            this.Controls.Add(this.chkUseHeaderConfiguration);
            this.Controls.Add(this.lblHeadConfiguration);
            this.Controls.Add(this.ntbFontCharset);
            this.Controls.Add(this.cwForeColor);
            this.Controls.Add(this.chkFontUnderline);
            this.Controls.Add(this.chkFontStrikeThrough);
            this.Controls.Add(this.chkFontItalic);
            this.Controls.Add(this.chkFontBold);
            this.Controls.Add(this.lblFontCharset);
            this.Controls.Add(this.lblFontColor);
            this.Controls.Add(this.lblFontUnderline);
            this.Controls.Add(this.lblFontStrikethrough);
            this.Controls.Add(this.lblFontItalic);
            this.Controls.Add(this.lblFontBold);
            this.Controls.Add(this.ntbFontSize);
            this.Controls.Add(this.lblFontSize);
            this.Controls.Add(this.btnEditFont);
            this.Controls.Add(this.tbFontName);
            this.Controls.Add(this.lblFontName);
            this.DoubleBuffered = true;
            this.Name = "PosButtonGridMenuLineFontPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label lblFontName;
        private ContextButton btnEditFont;
        private System.Windows.Forms.TextBox tbFontName;
        private System.Windows.Forms.Label lblFontSize;
        private NumericTextBox ntbFontSize;
        private System.Windows.Forms.Label lblFontBold;
        private System.Windows.Forms.Label lblFontItalic;
        private System.Windows.Forms.Label lblFontStrikethrough;
        private System.Windows.Forms.Label lblFontUnderline;
        private System.Windows.Forms.Label lblFontColor;
        private System.Windows.Forms.Label lblFontCharset;
        private System.Windows.Forms.CheckBox chkFontBold;
        private System.Windows.Forms.CheckBox chkFontItalic;
        private System.Windows.Forms.CheckBox chkFontStrikeThrough;
        private System.Windows.Forms.CheckBox chkFontUnderline;
        private ColorWell cwForeColor;
        private NumericTextBox ntbFontCharset;
        private System.Windows.Forms.Label lblHeadConfiguration;
        private System.Windows.Forms.CheckBox chkUseHeaderConfiguration;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblTextPosition;
        private System.Windows.Forms.ComboBox cmbTextPosition;
    }
}
