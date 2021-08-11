namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class PosButtonGridMenuLineImagePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosButtonGridMenuLineImagePage));
            this.btnEditImage = new LSOne.Controls.ContextButton();
            this.tbImageFile = new System.Windows.Forms.TextBox();
            this.cmbImagePosition = new System.Windows.Forms.ComboBox();
            this.lblImage = new System.Windows.Forms.Label();
            this.lblImagePosition = new System.Windows.Forms.Label();
            this.btnDeleteImage = new LSOne.Controls.ContextButton();
            this.chkUseFontImage = new System.Windows.Forms.CheckBox();
            this.lblUseFontImage = new System.Windows.Forms.Label();
            this.tbImageText = new System.Windows.Forms.TextBox();
            this.lblImageFontText = new System.Windows.Forms.Label();
            this.tbImageFontName = new System.Windows.Forms.TextBox();
            this.lblImageFontName = new System.Windows.Forms.Label();
            this.btnEditImageFont = new LSOne.Controls.ContextButton();
            this.tbImageFontSize = new System.Windows.Forms.TextBox();
            this.lblImageFontSize = new System.Windows.Forms.Label();
            this.cwImageFontColor = new LSOne.Controls.ColorWell();
            this.lblImageFontColor = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.SuspendLayout();
            // 
            // btnEditImage
            // 
            this.btnEditImage.BackColor = System.Drawing.Color.Transparent;
            this.btnEditImage.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditImage, "btnEditImage");
            this.btnEditImage.Name = "btnEditImage";
            this.btnEditImage.Click += new System.EventHandler(this.btnEditImage_Click);
            // 
            // tbImageFile
            // 
            resources.ApplyResources(this.tbImageFile, "tbImageFile");
            this.tbImageFile.Name = "tbImageFile";
            this.tbImageFile.ReadOnly = true;
            // 
            // cmbImagePosition
            // 
            this.cmbImagePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImagePosition.FormattingEnabled = true;
            resources.ApplyResources(this.cmbImagePosition, "cmbImagePosition");
            this.cmbImagePosition.Name = "cmbImagePosition";
            this.cmbImagePosition.SelectedIndexChanged += new System.EventHandler(this.cmbImagePosition_SelectedIndexChanged);
            // 
            // lblImage
            // 
            resources.ApplyResources(this.lblImage, "lblImage");
            this.lblImage.Name = "lblImage";
            // 
            // lblImagePosition
            // 
            resources.ApplyResources(this.lblImagePosition, "lblImagePosition");
            this.lblImagePosition.Name = "lblImagePosition";
            // 
            // btnDeleteImage
            // 
            this.btnDeleteImage.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteImage.Context = LSOne.Controls.ButtonType.Remove;
            resources.ApplyResources(this.btnDeleteImage, "btnDeleteImage");
            this.btnDeleteImage.Name = "btnDeleteImage";
            this.btnDeleteImage.Click += new System.EventHandler(this.btnDeleteImage_Click);
            // 
            // chkUseFontImage
            // 
            resources.ApplyResources(this.chkUseFontImage, "chkUseFontImage");
            this.chkUseFontImage.Name = "chkUseFontImage";
            this.chkUseFontImage.UseVisualStyleBackColor = true;
            this.chkUseFontImage.CheckedChanged += new System.EventHandler(this.chkUseFontImage_CheckedChanged);
            // 
            // lblUseFontImage
            // 
            resources.ApplyResources(this.lblUseFontImage, "lblUseFontImage");
            this.lblUseFontImage.Name = "lblUseFontImage";
            // 
            // tbImageText
            // 
            resources.ApplyResources(this.tbImageText, "tbImageText");
            this.tbImageText.Name = "tbImageText";
            this.tbImageText.TextChanged += new System.EventHandler(this.tbImageText_TextChanged);
            // 
            // lblImageFontText
            // 
            resources.ApplyResources(this.lblImageFontText, "lblImageFontText");
            this.lblImageFontText.Name = "lblImageFontText";
            // 
            // tbImageFontName
            // 
            resources.ApplyResources(this.tbImageFontName, "tbImageFontName");
            this.tbImageFontName.Name = "tbImageFontName";
            this.tbImageFontName.ReadOnly = true;
            // 
            // lblImageFontName
            // 
            resources.ApplyResources(this.lblImageFontName, "lblImageFontName");
            this.lblImageFontName.Name = "lblImageFontName";
            // 
            // btnEditImageFont
            // 
            this.btnEditImageFont.BackColor = System.Drawing.Color.Transparent;
            this.btnEditImageFont.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditImageFont, "btnEditImageFont");
            this.btnEditImageFont.Name = "btnEditImageFont";
            this.btnEditImageFont.Click += new System.EventHandler(this.btnEditImageFont_Click);
            // 
            // tbImageFontSize
            // 
            resources.ApplyResources(this.tbImageFontSize, "tbImageFontSize");
            this.tbImageFontSize.Name = "tbImageFontSize";
            this.tbImageFontSize.ReadOnly = true;
            // 
            // lblImageFontSize
            // 
            resources.ApplyResources(this.lblImageFontSize, "lblImageFontSize");
            this.lblImageFontSize.Name = "lblImageFontSize";
            // 
            // cwImageFontColor
            // 
            resources.ApplyResources(this.cwImageFontColor, "cwImageFontColor");
            this.cwImageFontColor.Name = "cwImageFontColor";
            this.cwImageFontColor.SelectedColor = System.Drawing.Color.White;
            this.cwImageFontColor.SelectedColorChanged += new System.EventHandler(this.cwImageFontColor_SelectedColorChanged);
            // 
            // lblImageFontColor
            // 
            resources.ApplyResources(this.lblImageFontColor, "lblImageFontColor");
            this.lblImageFontColor.Name = "lblImageFontColor";
            // 
            // PosButtonGridMenuLineImagePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cwImageFontColor);
            this.Controls.Add(this.lblImageFontColor);
            this.Controls.Add(this.tbImageFontSize);
            this.Controls.Add(this.lblImageFontSize);
            this.Controls.Add(this.btnEditImageFont);
            this.Controls.Add(this.tbImageFontName);
            this.Controls.Add(this.lblImageFontName);
            this.Controls.Add(this.tbImageText);
            this.Controls.Add(this.lblImageFontText);
            this.Controls.Add(this.chkUseFontImage);
            this.Controls.Add(this.lblUseFontImage);
            this.Controls.Add(this.btnDeleteImage);
            this.Controls.Add(this.btnEditImage);
            this.Controls.Add(this.tbImageFile);
            this.Controls.Add(this.cmbImagePosition);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.lblImagePosition);
            this.DoubleBuffered = true;
            this.Name = "PosButtonGridMenuLineImagePage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LSOne.Controls.ContextButton btnEditImage;
        private System.Windows.Forms.TextBox tbImageFile;
        private System.Windows.Forms.ComboBox cmbImagePosition;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.Label lblImagePosition;
        private LSOne.Controls.ContextButton btnDeleteImage;
        private System.Windows.Forms.CheckBox chkUseFontImage;
        private System.Windows.Forms.Label lblUseFontImage;
        private System.Windows.Forms.TextBox tbImageText;
        private System.Windows.Forms.Label lblImageFontText;
        private System.Windows.Forms.TextBox tbImageFontName;
        private System.Windows.Forms.Label lblImageFontName;
        private LSOne.Controls.ContextButton btnEditImageFont;
        private System.Windows.Forms.TextBox tbImageFontSize;
        private System.Windows.Forms.Label lblImageFontSize;
        private LSOne.Controls.ColorWell cwImageFontColor;
        private System.Windows.Forms.Label lblImageFontColor;
        private System.Windows.Forms.FontDialog fontDialog1;
    }
}
