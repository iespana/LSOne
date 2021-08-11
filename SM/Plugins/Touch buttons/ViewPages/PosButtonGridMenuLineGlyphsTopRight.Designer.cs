using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class PosButtonGridMenuLineGlyphsTopRightPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosButtonGridMenuLineGlyphsTopRightPage));
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.ntbGlyphOffset = new NumericTextBox();
            this.cwGlyphTextForeColor = new ColorWell();
            this.ntbGlyphTextFontSize = new NumericTextBox();
            this.btnEditGlyphFont = new ContextButton();
            this.tbGlyphTextFont = new System.Windows.Forms.TextBox();
            this.tbGlyphText = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cmbGlyph = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // fontDialog
            // 
            this.fontDialog.Color = System.Drawing.SystemColors.ControlText;
            this.fontDialog.ShowColor = true;
            this.fontDialog.ShowEffects = false;
            // 
            // ntbGlyphOffset
            // 
            this.ntbGlyphOffset.AllowDecimal = false;
            this.ntbGlyphOffset.AllowNegative = false;
            this.ntbGlyphOffset.CultureInfo = null;
            this.ntbGlyphOffset.DecimalLetters = 2;
            this.ntbGlyphOffset.HasMinValue = false;
            resources.ApplyResources(this.ntbGlyphOffset, "ntbGlyphOffset");
            this.ntbGlyphOffset.MaxValue = 0D;
            this.ntbGlyphOffset.MinValue = 0D;
            this.ntbGlyphOffset.Name = "ntbGlyphOffset";
            this.ntbGlyphOffset.Value = 0D;
            this.ntbGlyphOffset.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // cwGlyphTextForeColor
            // 
            resources.ApplyResources(this.cwGlyphTextForeColor, "cwGlyphTextForeColor");
            this.cwGlyphTextForeColor.Name = "cwGlyphTextForeColor";
            this.cwGlyphTextForeColor.SelectedColor = System.Drawing.Color.White;
            this.cwGlyphTextForeColor.SelectedColorChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // ntbGlyphTextFontSize
            // 
            this.ntbGlyphTextFontSize.AllowDecimal = false;
            this.ntbGlyphTextFontSize.AllowNegative = false;
            this.ntbGlyphTextFontSize.CultureInfo = null;
            this.ntbGlyphTextFontSize.DecimalLetters = 2;
            this.ntbGlyphTextFontSize.HasMinValue = false;
            resources.ApplyResources(this.ntbGlyphTextFontSize, "ntbGlyphTextFontSize");
            this.ntbGlyphTextFontSize.MaxValue = 0D;
            this.ntbGlyphTextFontSize.MinValue = 0D;
            this.ntbGlyphTextFontSize.Name = "ntbGlyphTextFontSize";
            this.ntbGlyphTextFontSize.Value = 1D;
            this.ntbGlyphTextFontSize.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // btnEditGlyphFont
            // 
            this.btnEditGlyphFont.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditGlyphFont, "btnEditGlyphFont");
            this.btnEditGlyphFont.Name = "btnEditGlyphFont";
            this.btnEditGlyphFont.Click += new System.EventHandler(this.btnEditGlyphFont4_Click);
            // 
            // tbGlyphTextFont
            // 
            resources.ApplyResources(this.tbGlyphTextFont, "tbGlyphTextFont");
            this.tbGlyphTextFont.Name = "tbGlyphTextFont";
            this.tbGlyphTextFont.ReadOnly = true;
            this.tbGlyphTextFont.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // tbGlyphText
            // 
            resources.ApplyResources(this.tbGlyphText, "tbGlyphText");
            this.tbGlyphText.Name = "tbGlyphText";
            this.tbGlyphText.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // cmbGlyph
            // 
            this.cmbGlyph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGlyph.FormattingEnabled = true;
            this.cmbGlyph.Items.AddRange(new object[] {
            resources.GetString("cmbGlyph.Items"),
            resources.GetString("cmbGlyph.Items1"),
            resources.GetString("cmbGlyph.Items2"),
            resources.GetString("cmbGlyph.Items3"),
            resources.GetString("cmbGlyph.Items4"),
            resources.GetString("cmbGlyph.Items5"),
            resources.GetString("cmbGlyph.Items6"),
            resources.GetString("cmbGlyph.Items7"),
            resources.GetString("cmbGlyph.Items8"),
            resources.GetString("cmbGlyph.Items9"),
            resources.GetString("cmbGlyph.Items10"),
            resources.GetString("cmbGlyph.Items11"),
            resources.GetString("cmbGlyph.Items12"),
            resources.GetString("cmbGlyph.Items13"),
            resources.GetString("cmbGlyph.Items14"),
            resources.GetString("cmbGlyph.Items15"),
            resources.GetString("cmbGlyph.Items16"),
            resources.GetString("cmbGlyph.Items17"),
            resources.GetString("cmbGlyph.Items18"),
            resources.GetString("cmbGlyph.Items19"),
            resources.GetString("cmbGlyph.Items20"),
            resources.GetString("cmbGlyph.Items21"),
            resources.GetString("cmbGlyph.Items22"),
            resources.GetString("cmbGlyph.Items23"),
            resources.GetString("cmbGlyph.Items24"),
            resources.GetString("cmbGlyph.Items25")});
            resources.ApplyResources(this.cmbGlyph, "cmbGlyph");
            this.cmbGlyph.Name = "cmbGlyph";
            this.cmbGlyph.SelectedIndexChanged += new System.EventHandler(this.cmbGlyph4_SelectedIndexChanged);
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // PosButtonGridMenuLineGlyphsTopRightPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbGlyphOffset);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.cwGlyphTextForeColor);
            this.Controls.Add(this.cmbGlyph);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.ntbGlyphTextFontSize);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.btnEditGlyphFont);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.tbGlyphTextFont);
            this.Controls.Add(this.tbGlyphText);
            this.Controls.Add(this.label26);
            this.DoubleBuffered = true;
            this.Name = "PosButtonGridMenuLineGlyphsTopRightPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog;
        private NumericTextBox ntbGlyphOffset;
        private ColorWell cwGlyphTextForeColor;
        private NumericTextBox ntbGlyphTextFontSize;
        private ContextButton btnEditGlyphFont;
        private System.Windows.Forms.TextBox tbGlyphTextFont;
        private System.Windows.Forms.TextBox tbGlyphText;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cmbGlyph;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ErrorProvider errorProvider1;

    }
}
