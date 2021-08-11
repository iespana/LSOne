using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class PosButtonGridMenuLineGlyphsTopLeftPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosButtonGridMenuLineGlyphsTopLeftPage));
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.ntbGlyph4Offset = new NumericTextBox();
            this.cwGlyphText4ForeColor = new ColorWell();
            this.ntbGlyphText4FontSize = new NumericTextBox();
            this.btnEditGlyphFont4 = new ContextButton();
            this.tbGlyphText4Font = new System.Windows.Forms.TextBox();
            this.tbGlyphText4 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cmbGlyph4 = new System.Windows.Forms.ComboBox();
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
            // ntbGlyph4Offset
            // 
            this.ntbGlyph4Offset.AllowDecimal = false;
            this.ntbGlyph4Offset.AllowNegative = false;
            this.ntbGlyph4Offset.CultureInfo = null;
            this.ntbGlyph4Offset.DecimalLetters = 2;
            this.ntbGlyph4Offset.HasMinValue = false;
            resources.ApplyResources(this.ntbGlyph4Offset, "ntbGlyph4Offset");
            this.ntbGlyph4Offset.MaxValue = 0D;
            this.ntbGlyph4Offset.MinValue = 0D;
            this.ntbGlyph4Offset.Name = "ntbGlyph4Offset";
            this.ntbGlyph4Offset.Value = 0D;
            this.ntbGlyph4Offset.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // cwGlyphText4ForeColor
            // 
            resources.ApplyResources(this.cwGlyphText4ForeColor, "cwGlyphText4ForeColor");
            this.cwGlyphText4ForeColor.Name = "cwGlyphText4ForeColor";
            this.cwGlyphText4ForeColor.SelectedColor = System.Drawing.Color.White;
            this.cwGlyphText4ForeColor.SelectedColorChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // ntbGlyphText4FontSize
            // 
            this.ntbGlyphText4FontSize.AllowDecimal = false;
            this.ntbGlyphText4FontSize.AllowNegative = false;
            this.ntbGlyphText4FontSize.CultureInfo = null;
            this.ntbGlyphText4FontSize.DecimalLetters = 2;
            this.ntbGlyphText4FontSize.HasMinValue = false;
            resources.ApplyResources(this.ntbGlyphText4FontSize, "ntbGlyphText4FontSize");
            this.ntbGlyphText4FontSize.MaxValue = 0D;
            this.ntbGlyphText4FontSize.MinValue = 0D;
            this.ntbGlyphText4FontSize.Name = "ntbGlyphText4FontSize";
            this.ntbGlyphText4FontSize.Value = 1D;
            this.ntbGlyphText4FontSize.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // btnEditGlyphFont4
            // 
            this.btnEditGlyphFont4.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditGlyphFont4, "btnEditGlyphFont4");
            this.btnEditGlyphFont4.Name = "btnEditGlyphFont4";
            this.btnEditGlyphFont4.Click += new System.EventHandler(this.btnEditGlyphFont4_Click);
            // 
            // tbGlyphText4Font
            // 
            resources.ApplyResources(this.tbGlyphText4Font, "tbGlyphText4Font");
            this.tbGlyphText4Font.Name = "tbGlyphText4Font";
            this.tbGlyphText4Font.ReadOnly = true;
            this.tbGlyphText4Font.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // tbGlyphText4
            // 
            resources.ApplyResources(this.tbGlyphText4, "tbGlyphText4");
            this.tbGlyphText4.Name = "tbGlyphText4";
            this.tbGlyphText4.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // cmbGlyph4
            // 
            this.cmbGlyph4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGlyph4.FormattingEnabled = true;
            this.cmbGlyph4.Items.AddRange(new object[] {
            resources.GetString("cmbGlyph4.Items"),
            resources.GetString("cmbGlyph4.Items1"),
            resources.GetString("cmbGlyph4.Items2"),
            resources.GetString("cmbGlyph4.Items3"),
            resources.GetString("cmbGlyph4.Items4"),
            resources.GetString("cmbGlyph4.Items5"),
            resources.GetString("cmbGlyph4.Items6"),
            resources.GetString("cmbGlyph4.Items7"),
            resources.GetString("cmbGlyph4.Items8"),
            resources.GetString("cmbGlyph4.Items9"),
            resources.GetString("cmbGlyph4.Items10"),
            resources.GetString("cmbGlyph4.Items11"),
            resources.GetString("cmbGlyph4.Items12"),
            resources.GetString("cmbGlyph4.Items13"),
            resources.GetString("cmbGlyph4.Items14"),
            resources.GetString("cmbGlyph4.Items15"),
            resources.GetString("cmbGlyph4.Items16"),
            resources.GetString("cmbGlyph4.Items17"),
            resources.GetString("cmbGlyph4.Items18"),
            resources.GetString("cmbGlyph4.Items19"),
            resources.GetString("cmbGlyph4.Items20"),
            resources.GetString("cmbGlyph4.Items21"),
            resources.GetString("cmbGlyph4.Items22"),
            resources.GetString("cmbGlyph4.Items23"),
            resources.GetString("cmbGlyph4.Items24"),
            resources.GetString("cmbGlyph4.Items25")});
            resources.ApplyResources(this.cmbGlyph4, "cmbGlyph4");
            this.cmbGlyph4.Name = "cmbGlyph4";
            this.cmbGlyph4.SelectedIndexChanged += new System.EventHandler(this.cmbGlyph4_SelectedIndexChanged);
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
            // PosButtonGridMenuLineGlyphsTopLeftPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbGlyph4Offset);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.cwGlyphText4ForeColor);
            this.Controls.Add(this.cmbGlyph4);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.ntbGlyphText4FontSize);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.btnEditGlyphFont4);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.tbGlyphText4Font);
            this.Controls.Add(this.tbGlyphText4);
            this.Controls.Add(this.label26);
            this.DoubleBuffered = true;
            this.Name = "PosButtonGridMenuLineGlyphsTopLeftPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog;
        private NumericTextBox ntbGlyph4Offset;
        private ColorWell cwGlyphText4ForeColor;
        private NumericTextBox ntbGlyphText4FontSize;
        private ContextButton btnEditGlyphFont4;
        private System.Windows.Forms.TextBox tbGlyphText4Font;
        private System.Windows.Forms.TextBox tbGlyphText4;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cmbGlyph4;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ErrorProvider errorProvider1;

    }
}
