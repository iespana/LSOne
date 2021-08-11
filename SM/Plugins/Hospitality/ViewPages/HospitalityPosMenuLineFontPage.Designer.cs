using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityPosMenuLineFontPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityPosMenuLineFontPage));
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEditFont = new ContextButton();
            this.tbFontName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ntbFontSize = new NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkFontBold = new System.Windows.Forms.CheckBox();
            this.chkFontItalic = new System.Windows.Forms.CheckBox();
            this.chkFontStrikeThrough = new System.Windows.Forms.CheckBox();
            this.chkFontUnderline = new System.Windows.Forms.CheckBox();
            this.cwForeColor = new ColorWell();
            this.ntbFontCharset = new NumericTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkUseHeaderConfiguration = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnEditFont
            // 
            this.btnEditFont.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditFont, "btnEditFont");
            this.btnEditFont.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnEditFont.MinimumSize = new System.Drawing.Size(23, 23);
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ntbFontSize
            // 
            this.ntbFontSize.AllowDecimal = false;
            this.ntbFontSize.AllowNegative = false;
            this.ntbFontSize.DecimalLetters = 2;
            this.ntbFontSize.HasMinValue = false;
            resources.ApplyResources(this.ntbFontSize, "ntbFontSize");
            this.ntbFontSize.MaxValue = 0D;
            this.ntbFontSize.MinValue = 0D;
            this.ntbFontSize.Name = "ntbFontSize";
            this.ntbFontSize.Value = 0D;
            this.ntbFontSize.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
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
            this.ntbFontCharset.DecimalLetters = 2;
            this.ntbFontCharset.HasMinValue = false;
            resources.ApplyResources(this.ntbFontCharset, "ntbFontCharset");
            this.ntbFontCharset.MaxValue = 0D;
            this.ntbFontCharset.MinValue = 0D;
            this.ntbFontCharset.Name = "ntbFontCharset";
            this.ntbFontCharset.ReadOnly = true;
            this.ntbFontCharset.Value = 0D;
            this.ntbFontCharset.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // chkUseHeaderConfiguration
            // 
            resources.ApplyResources(this.chkUseHeaderConfiguration, "chkUseHeaderConfiguration");
            this.chkUseHeaderConfiguration.Name = "chkUseHeaderConfiguration";
            this.chkUseHeaderConfiguration.UseVisualStyleBackColor = true;
            this.chkUseHeaderConfiguration.CheckedChanged += new System.EventHandler(this.chkUseHeaderConfiguration_CheckedChanged);
            // 
            // HospitalityPosMenuLineFontPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkUseHeaderConfiguration);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ntbFontCharset);
            this.Controls.Add(this.cwForeColor);
            this.Controls.Add(this.chkFontUnderline);
            this.Controls.Add(this.chkFontStrikeThrough);
            this.Controls.Add(this.chkFontItalic);
            this.Controls.Add(this.chkFontBold);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ntbFontSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEditFont);
            this.Controls.Add(this.tbFontName);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalityPosMenuLineFontPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label label1;
        private ContextButton btnEditFont;
        private System.Windows.Forms.TextBox tbFontName;
        private System.Windows.Forms.Label label2;
        private NumericTextBox ntbFontSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkFontBold;
        private System.Windows.Forms.CheckBox chkFontItalic;
        private System.Windows.Forms.CheckBox chkFontStrikeThrough;
        private System.Windows.Forms.CheckBox chkFontUnderline;
        private ColorWell cwForeColor;
        private NumericTextBox ntbFontCharset;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkUseHeaderConfiguration;

    }
}
