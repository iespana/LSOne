using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class PosButtonGridMenuLineGlyphsBottomRightPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosButtonGridMenuLineGlyphsBottomRightPage));
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.ntbGlyph2Offset = new NumericTextBox();
            this.cwGlyphText2ForeColor = new ColorWell();
            this.ntbGlyphText2FontSize = new NumericTextBox();
            this.btnEditGlyphFont2 = new ContextButton();
            this.tbGlyphText2Font = new System.Windows.Forms.TextBox();
            this.tbGlyphText2 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cmbGlyph2 = new System.Windows.Forms.ComboBox();
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
            // ntbGlyph2Offset
            // 
            this.ntbGlyph2Offset.AllowDecimal = false;
            this.ntbGlyph2Offset.AllowNegative = false;
            this.ntbGlyph2Offset.CultureInfo = null;
            this.ntbGlyph2Offset.DecimalLetters = 2;
            this.ntbGlyph2Offset.HasMinValue = false;
            resources.ApplyResources(this.ntbGlyph2Offset, "ntbGlyph2Offset");
            this.ntbGlyph2Offset.MaxValue = 0D;
            this.ntbGlyph2Offset.MinValue = 0D;
            this.ntbGlyph2Offset.Name = "ntbGlyph2Offset";
            this.ntbGlyph2Offset.Value = 0D;
            this.ntbGlyph2Offset.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // cwGlyphText2ForeColor
            // 
            resources.ApplyResources(this.cwGlyphText2ForeColor, "cwGlyphText2ForeColor");
            this.cwGlyphText2ForeColor.Name = "cwGlyphText2ForeColor";
            this.cwGlyphText2ForeColor.SelectedColor = System.Drawing.Color.White;
            this.cwGlyphText2ForeColor.SelectedColorChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // ntbGlyphText2FontSize
            // 
            this.ntbGlyphText2FontSize.AllowDecimal = false;
            this.ntbGlyphText2FontSize.AllowNegative = false;
            this.ntbGlyphText2FontSize.CultureInfo = null;
            this.ntbGlyphText2FontSize.DecimalLetters = 2;
            this.ntbGlyphText2FontSize.HasMinValue = false;
            resources.ApplyResources(this.ntbGlyphText2FontSize, "ntbGlyphText2FontSize");
            this.ntbGlyphText2FontSize.MaxValue = 0D;
            this.ntbGlyphText2FontSize.MinValue = 0D;
            this.ntbGlyphText2FontSize.Name = "ntbGlyphText2FontSize";
            this.ntbGlyphText2FontSize.Value = 1D;
            this.ntbGlyphText2FontSize.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // btnEditGlyphFont2
            // 
            this.btnEditGlyphFont2.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditGlyphFont2, "btnEditGlyphFont2");
            this.btnEditGlyphFont2.Name = "btnEditGlyphFont2";
            this.btnEditGlyphFont2.Click += new System.EventHandler(this.btnEditGlyphFont4_Click);
            // 
            // tbGlyphText2Font
            // 
            resources.ApplyResources(this.tbGlyphText2Font, "tbGlyphText2Font");
            this.tbGlyphText2Font.Name = "tbGlyphText2Font";
            this.tbGlyphText2Font.ReadOnly = true;
            this.tbGlyphText2Font.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // tbGlyphText2
            // 
            resources.ApplyResources(this.tbGlyphText2, "tbGlyphText2");
            this.tbGlyphText2.Name = "tbGlyphText2";
            this.tbGlyphText2.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // cmbGlyph2
            // 
            this.cmbGlyph2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGlyph2.FormattingEnabled = true;
            this.cmbGlyph2.Items.AddRange(new object[] {
            resources.GetString("cmbGlyph2.Items"),
            resources.GetString("cmbGlyph2.Items1"),
            resources.GetString("cmbGlyph2.Items2"),
            resources.GetString("cmbGlyph2.Items3"),
            resources.GetString("cmbGlyph2.Items4"),
            resources.GetString("cmbGlyph2.Items5"),
            resources.GetString("cmbGlyph2.Items6"),
            resources.GetString("cmbGlyph2.Items7"),
            resources.GetString("cmbGlyph2.Items8"),
            resources.GetString("cmbGlyph2.Items9"),
            resources.GetString("cmbGlyph2.Items10"),
            resources.GetString("cmbGlyph2.Items11"),
            resources.GetString("cmbGlyph2.Items12"),
            resources.GetString("cmbGlyph2.Items13"),
            resources.GetString("cmbGlyph2.Items14"),
            resources.GetString("cmbGlyph2.Items15"),
            resources.GetString("cmbGlyph2.Items16"),
            resources.GetString("cmbGlyph2.Items17"),
            resources.GetString("cmbGlyph2.Items18"),
            resources.GetString("cmbGlyph2.Items19"),
            resources.GetString("cmbGlyph2.Items20"),
            resources.GetString("cmbGlyph2.Items21"),
            resources.GetString("cmbGlyph2.Items22"),
            resources.GetString("cmbGlyph2.Items23"),
            resources.GetString("cmbGlyph2.Items24"),
            resources.GetString("cmbGlyph2.Items25")});
            resources.ApplyResources(this.cmbGlyph2, "cmbGlyph2");
            this.cmbGlyph2.Name = "cmbGlyph2";
            this.cmbGlyph2.SelectedIndexChanged += new System.EventHandler(this.cmbGlyph4_SelectedIndexChanged);
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
            // PosButtonGridMenuLineGlyphsBottomRightPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbGlyph2Offset);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.cwGlyphText2ForeColor);
            this.Controls.Add(this.cmbGlyph2);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.ntbGlyphText2FontSize);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.btnEditGlyphFont2);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.tbGlyphText2Font);
            this.Controls.Add(this.tbGlyphText2);
            this.Controls.Add(this.label26);
            this.DoubleBuffered = true;
            this.Name = "PosButtonGridMenuLineGlyphsBottomRightPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog;
        private NumericTextBox ntbGlyph2Offset;
        private ColorWell cwGlyphText2ForeColor;
        private NumericTextBox ntbGlyphText2FontSize;
        private ContextButton btnEditGlyphFont2;
        private System.Windows.Forms.TextBox tbGlyphText2Font;
        private System.Windows.Forms.TextBox tbGlyphText2;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cmbGlyph2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ErrorProvider errorProvider1;

    }
}
