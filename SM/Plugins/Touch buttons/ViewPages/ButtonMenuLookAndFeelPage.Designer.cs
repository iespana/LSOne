namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class ButtonMenuLookAndFeelPage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonMenuLookAndFeelPage));
			LSOne.DataLayer.BusinessObjects.TouchButtons.PosMenuHeader posMenuHeader3 = new LSOne.DataLayer.BusinessObjects.TouchButtons.PosMenuHeader();
			LSOne.DataLayer.BusinessObjects.TouchButtons.Style style3 = new LSOne.DataLayer.BusinessObjects.TouchButtons.Style();
			this.buttonProperties = new LSOne.ViewPlugins.TouchButtons.Controls.ButtonPropertiesControl();
			this.grpPreviewButton = new System.Windows.Forms.GroupBox();
			this.btnMenuButtonPreview = new LSOne.Controls.MenuButton();
			this.cwBorderColor = new LSOne.Controls.ColorWell();
			this.lblBorderColor = new System.Windows.Forms.Label();
			this.lblMargin = new System.Windows.Forms.Label();
			this.ntbMargin = new LSOne.Controls.NumericTextBox();
			this.lblBorderWidth = new System.Windows.Forms.Label();
			this.ntbBorderWidth = new LSOne.Controls.NumericTextBox();
			this.grpPreviewButton.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonProperties
			// 
			resources.ApplyResources(this.buttonProperties, "buttonProperties");
			this.buttonProperties.BackColor = System.Drawing.Color.Transparent;
			this.buttonProperties.EnableStyleUse = true;
			this.buttonProperties.FontBold = false;
			this.buttonProperties.FontCharset = 0;
			this.buttonProperties.FontItalic = false;
			this.buttonProperties.FontName = "Tahoma";
			this.buttonProperties.FontSize = 0;
			this.buttonProperties.FontStrikethrough = false;
			this.buttonProperties.GradientMode = LSOne.Utilities.Enums.GradientModeEnum.None;
			this.buttonProperties.Name = "buttonProperties";
			posMenuHeader3.AppliesTo = LSOne.DataLayer.BusinessObjects.TouchButtons.PosMenuHeader.AppliesToEnum.None;
			posMenuHeader3.BackColor = -1;
			posMenuHeader3.BackColor2 = -1;
			posMenuHeader3.BorderColor = -3352871;
			posMenuHeader3.BorderWidth = 1;
			posMenuHeader3.Columns = 0;
			posMenuHeader3.DefaultOperation = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("posMenuHeader3.DefaultOperation")));
			posMenuHeader3.DeviceType = LSOne.DataLayer.BusinessObjects.TouchButtons.DeviceTypeEnum.POS;
			posMenuHeader3.FontBold = false;
			posMenuHeader3.FontCharset = 0;
			posMenuHeader3.FontItalic = false;
			posMenuHeader3.FontName = "Tahoma";
			posMenuHeader3.FontSize = 0;
			posMenuHeader3.ForeColor = -1;
			posMenuHeader3.GradientMode = LSOne.Utilities.Enums.GradientModeEnum.None;
			posMenuHeader3.Guid = new System.Guid("00000000-0000-0000-0000-000000000000");
			posMenuHeader3.ID = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("posMenuHeader3.ID")));
			posMenuHeader3.ImportDateTime = null;
			posMenuHeader3.KitchenDisplay = false;
			posMenuHeader3.MainMenu = false;
			posMenuHeader3.Margin = 0;
			posMenuHeader3.MenuColor = 0;
			posMenuHeader3.MenuType = LSOne.DataLayer.BusinessObjects.TouchButtons.MenuTypeEnum.Hospitality;
			posMenuHeader3.Rows = 0;
			posMenuHeader3.Shape = LSOne.Utilities.Enums.ShapeEnum.RoundRectangle;
			posMenuHeader3.StyleID = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("posMenuHeader3.StyleID")));
			posMenuHeader3.Text = "";
			posMenuHeader3.TextPosition = LSOne.DataLayer.BusinessObjects.TouchButtons.Position.Center;
			posMenuHeader3.UsageIntent = LSOne.Utilities.DataTypes.UsageIntentEnum.Normal;
			posMenuHeader3.UseNavOperation = false;
			this.buttonProperties.PosMenuHeader = posMenuHeader3;
			this.buttonProperties.PosStyle = ((LSOne.DataLayer.BusinessObjects.TouchButtons.PosStyle)(resources.GetObject("buttonProperties.PosStyle")));
			this.buttonProperties.PosStyleID = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("buttonProperties.PosStyleID")));
			this.buttonProperties.Shape = LSOne.Utilities.Enums.ShapeEnum.RoundRectangle;
			style3.BackColor = System.Drawing.Color.White;
			style3.BackColor2 = System.Drawing.Color.White;
			style3.Font = new System.Drawing.Font("Tahoma", 1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			style3.FontBold = false;
			style3.FontCharset = 0;
			style3.FontItalic = false;
			style3.FontName = "Tahoma";
			style3.FontSize = 0;
			style3.ForeColor = System.Drawing.Color.White;
			style3.GradientMode = LSOne.Utilities.Enums.GradientModeEnum.None;
			style3.ID = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("style3.ID")));
			style3.Shape = LSOne.Utilities.Enums.ShapeEnum.RoundRectangle;
			style3.Text = "";
			style3.TextPosition = LSOne.DataLayer.BusinessObjects.TouchButtons.Position.Center;
			style3.UsageIntent = LSOne.Utilities.DataTypes.UsageIntentEnum.Normal;
			this.buttonProperties.Style = style3;
			this.buttonProperties.StyleBackColor = System.Drawing.Color.White;
			this.buttonProperties.StyleBackColor2 = System.Drawing.Color.White;
			this.buttonProperties.StyleForeColor = System.Drawing.Color.White;
			this.buttonProperties.TextPosition = LSOne.DataLayer.BusinessObjects.TouchButtons.Position.Center;
			this.buttonProperties.Modified += new System.EventHandler(this.buttonProperties_Modified);
			// 
			// grpPreviewButton
			// 
			this.grpPreviewButton.Controls.Add(this.btnMenuButtonPreview);
			resources.ApplyResources(this.grpPreviewButton, "grpPreviewButton");
			this.grpPreviewButton.Name = "grpPreviewButton";
			this.grpPreviewButton.TabStop = false;
			// 
			// btnMenuButtonPreview
			// 
			this.btnMenuButtonPreview.AllowDrop = true;
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
			resources.ApplyResources(this.btnMenuButtonPreview, "btnMenuButtonPreview");
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
			this.btnMenuButtonPreview.HeightMM = 1667;
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
			this.btnMenuButtonPreview.WidthMM = 2910;
			this.btnMenuButtonPreview.XPos = 3;
			this.btnMenuButtonPreview.XPosMM = 79;
			this.btnMenuButtonPreview.YPos = 16;
			this.btnMenuButtonPreview.YPosMM = 423;
			// 
			// cwBorderColor
			// 
			resources.ApplyResources(this.cwBorderColor, "cwBorderColor");
			this.cwBorderColor.Name = "cwBorderColor";
			this.cwBorderColor.SelectedColor = System.Drawing.Color.White;
			this.cwBorderColor.SelectedColorChanged += new System.EventHandler(this.buttonProperties_Modified);
			// 
			// lblBorderColor
			// 
			this.lblBorderColor.BackColor = System.Drawing.Color.Transparent;
			this.lblBorderColor.ForeColor = System.Drawing.SystemColors.ControlText;
			resources.ApplyResources(this.lblBorderColor, "lblBorderColor");
			this.lblBorderColor.Name = "lblBorderColor";
			// 
			// lblMargin
			// 
			this.lblMargin.BackColor = System.Drawing.Color.Transparent;
			this.lblMargin.ForeColor = System.Drawing.SystemColors.ControlText;
			resources.ApplyResources(this.lblMargin, "lblMargin");
			this.lblMargin.Name = "lblMargin";
			// 
			// ntbMargin
			// 
			this.ntbMargin.AllowDecimal = false;
			this.ntbMargin.AllowNegative = false;
			this.ntbMargin.CultureInfo = null;
			this.ntbMargin.DecimalLetters = 2;
			this.ntbMargin.DecimalLimit = null;
			this.ntbMargin.ForeColor = System.Drawing.Color.Black;
			this.ntbMargin.HasMinValue = false;
			resources.ApplyResources(this.ntbMargin, "ntbMargin");
			this.ntbMargin.MaxValue = 9999D;
			this.ntbMargin.MinValue = 0D;
			this.ntbMargin.Name = "ntbMargin";
			this.ntbMargin.Value = 0D;
			// 
			// lblBorderWidth
			// 
			this.lblBorderWidth.BackColor = System.Drawing.Color.Transparent;
			this.lblBorderWidth.ForeColor = System.Drawing.SystemColors.ControlText;
			resources.ApplyResources(this.lblBorderWidth, "lblBorderWidth");
			this.lblBorderWidth.Name = "lblBorderWidth";
			// 
			// ntbBorderWidth
			// 
			this.ntbBorderWidth.AllowDecimal = false;
			this.ntbBorderWidth.AllowNegative = false;
			this.ntbBorderWidth.CultureInfo = null;
			this.ntbBorderWidth.DecimalLetters = 2;
			this.ntbBorderWidth.DecimalLimit = null;
			this.ntbBorderWidth.ForeColor = System.Drawing.Color.Black;
			this.ntbBorderWidth.HasMinValue = false;
			resources.ApplyResources(this.ntbBorderWidth, "ntbBorderWidth");
			this.ntbBorderWidth.MaxValue = 9999D;
			this.ntbBorderWidth.MinValue = 0D;
			this.ntbBorderWidth.Name = "ntbBorderWidth";
			this.ntbBorderWidth.Value = 0D;
			this.ntbBorderWidth.TextChanged += new System.EventHandler(this.buttonProperties_Modified);
			// 
			// ButtonMenuLookAndFeelPage
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.cwBorderColor);
			this.Controls.Add(this.lblBorderColor);
			this.Controls.Add(this.lblMargin);
			this.Controls.Add(this.ntbMargin);
			this.Controls.Add(this.lblBorderWidth);
			this.Controls.Add(this.ntbBorderWidth);
			this.Controls.Add(this.grpPreviewButton);
			this.Controls.Add(this.buttonProperties);
			this.Name = "ButtonMenuLookAndFeelPage";
			this.grpPreviewButton.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private Controls.ButtonPropertiesControl buttonProperties;
        private System.Windows.Forms.GroupBox grpPreviewButton;
        private LSOne.Controls.MenuButton btnMenuButtonPreview;
        private LSOne.Controls.ColorWell cwBorderColor;
        private System.Windows.Forms.Label lblBorderColor;
        private System.Windows.Forms.Label lblMargin;
        private LSOne.Controls.NumericTextBox ntbMargin;
        private System.Windows.Forms.Label lblBorderWidth;
        private LSOne.Controls.NumericTextBox ntbBorderWidth;
    }
}
