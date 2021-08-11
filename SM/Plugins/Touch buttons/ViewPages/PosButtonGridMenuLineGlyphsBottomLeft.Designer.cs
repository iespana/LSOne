using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class PosButtonGridMenuLineGlyphsBottomLeftPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosButtonGridMenuLineGlyphsBottomLeftPage));
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.ntbGlyph3Offset = new NumericTextBox();
            this.cwGlyphText3ForeColor = new ColorWell();
            this.ntbGlyphText3FontSize = new NumericTextBox();
            this.btnEditGlyphFont3 = new ContextButton();
            this.tbGlyphText3Font = new System.Windows.Forms.TextBox();
            this.tbGlyphText3 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.cmbGlyph3 = new System.Windows.Forms.ComboBox();
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
            // ntbGlyph3Offset
            // 
            this.ntbGlyph3Offset.AllowDecimal = false;
            this.ntbGlyph3Offset.AllowNegative = false;
            this.ntbGlyph3Offset.CultureInfo = null;
            this.ntbGlyph3Offset.DecimalLetters = 2;
            this.ntbGlyph3Offset.HasMinValue = false;
            resources.ApplyResources(this.ntbGlyph3Offset, "ntbGlyph3Offset");
            this.ntbGlyph3Offset.MaxValue = 0D;
            this.ntbGlyph3Offset.MinValue = 0D;
            this.ntbGlyph3Offset.Name = "ntbGlyph3Offset";
            this.ntbGlyph3Offset.Value = 0D;
            this.ntbGlyph3Offset.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // cwGlyphText3ForeColor
            // 
            resources.ApplyResources(this.cwGlyphText3ForeColor, "cwGlyphText3ForeColor");
            this.cwGlyphText3ForeColor.Name = "cwGlyphText3ForeColor";
            this.cwGlyphText3ForeColor.SelectedColor = System.Drawing.Color.White;
            this.cwGlyphText3ForeColor.SelectedColorChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // ntbGlyphText3FontSize
            // 
            this.ntbGlyphText3FontSize.AllowDecimal = false;
            this.ntbGlyphText3FontSize.AllowNegative = false;
            this.ntbGlyphText3FontSize.CultureInfo = null;
            this.ntbGlyphText3FontSize.DecimalLetters = 2;
            this.ntbGlyphText3FontSize.HasMinValue = false;
            resources.ApplyResources(this.ntbGlyphText3FontSize, "ntbGlyphText3FontSize");
            this.ntbGlyphText3FontSize.MaxValue = 0D;
            this.ntbGlyphText3FontSize.MinValue = 0D;
            this.ntbGlyphText3FontSize.Name = "ntbGlyphText3FontSize";
            this.ntbGlyphText3FontSize.Value = 1D;
            this.ntbGlyphText3FontSize.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // btnEditGlyphFont3
            // 
            this.btnEditGlyphFont3.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditGlyphFont3, "btnEditGlyphFont3");
            this.btnEditGlyphFont3.Name = "btnEditGlyphFont3";
            this.btnEditGlyphFont3.Click += new System.EventHandler(this.btnEditGlyphFont4_Click);
            // 
            // tbGlyphText3Font
            // 
            resources.ApplyResources(this.tbGlyphText3Font, "tbGlyphText3Font");
            this.tbGlyphText3Font.Name = "tbGlyphText3Font";
            this.tbGlyphText3Font.ReadOnly = true;
            this.tbGlyphText3Font.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // tbGlyphText3
            // 
            resources.ApplyResources(this.tbGlyphText3, "tbGlyphText3");
            this.tbGlyphText3.Name = "tbGlyphText3";
            this.tbGlyphText3.TextChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // cmbGlyph3
            // 
            this.cmbGlyph3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGlyph3.FormattingEnabled = true;
            this.cmbGlyph3.Items.AddRange(new object[] {
            resources.GetString("cmbGlyph3.Items"),
            resources.GetString("cmbGlyph3.Items1"),
            resources.GetString("cmbGlyph3.Items2"),
            resources.GetString("cmbGlyph3.Items3"),
            resources.GetString("cmbGlyph3.Items4"),
            resources.GetString("cmbGlyph3.Items5"),
            resources.GetString("cmbGlyph3.Items6"),
            resources.GetString("cmbGlyph3.Items7"),
            resources.GetString("cmbGlyph3.Items8"),
            resources.GetString("cmbGlyph3.Items9"),
            resources.GetString("cmbGlyph3.Items10"),
            resources.GetString("cmbGlyph3.Items11"),
            resources.GetString("cmbGlyph3.Items12"),
            resources.GetString("cmbGlyph3.Items13"),
            resources.GetString("cmbGlyph3.Items14"),
            resources.GetString("cmbGlyph3.Items15"),
            resources.GetString("cmbGlyph3.Items16"),
            resources.GetString("cmbGlyph3.Items17"),
            resources.GetString("cmbGlyph3.Items18"),
            resources.GetString("cmbGlyph3.Items19"),
            resources.GetString("cmbGlyph3.Items20"),
            resources.GetString("cmbGlyph3.Items21"),
            resources.GetString("cmbGlyph3.Items22"),
            resources.GetString("cmbGlyph3.Items23"),
            resources.GetString("cmbGlyph3.Items24"),
            resources.GetString("cmbGlyph3.Items25")});
            resources.ApplyResources(this.cmbGlyph3, "cmbGlyph3");
            this.cmbGlyph3.Name = "cmbGlyph3";
            this.cmbGlyph3.SelectedIndexChanged += new System.EventHandler(this.cmbGlyph4_SelectedIndexChanged);
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
            // PosButtonGridMenuLineGlyphsBottomLeftPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbGlyph3Offset);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.cwGlyphText3ForeColor);
            this.Controls.Add(this.cmbGlyph3);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.ntbGlyphText3FontSize);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.btnEditGlyphFont3);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.tbGlyphText3Font);
            this.Controls.Add(this.tbGlyphText3);
            this.Controls.Add(this.label26);
            this.DoubleBuffered = true;
            this.Name = "PosButtonGridMenuLineGlyphsBottomLeftPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog;
        private NumericTextBox ntbGlyph3Offset;
        private ColorWell cwGlyphText3ForeColor;
        private NumericTextBox ntbGlyphText3FontSize;
        private ContextButton btnEditGlyphFont3;
        private System.Windows.Forms.TextBox tbGlyphText3Font;
        private System.Windows.Forms.TextBox tbGlyphText3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cmbGlyph3;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ErrorProvider errorProvider1;

    }
}
