using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class StoreFormSettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreFormSettingsPage));
            this.lblFormProfile = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbFormProfile = new LSOne.Controls.DualDataComboBox();
            this.picLogoImage = new System.Windows.Forms.PictureBox();
            this.lblPreview = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.txtFormInfo4 = new System.Windows.Forms.TextBox();
            this.lblFormInfo4 = new System.Windows.Forms.Label();
            this.txtFormInfo3 = new System.Windows.Forms.TextBox();
            this.lblFormInfo3 = new System.Windows.Forms.Label();
            this.txtFormInfo2 = new System.Windows.Forms.TextBox();
            this.lblFormInfo2 = new System.Windows.Forms.Label();
            this.txtFormInfo1 = new System.Windows.Forms.TextBox();
            this.lblFormInfo1 = new System.Windows.Forms.Label();
            this.lblBarcodeSymbology = new System.Windows.Forms.Label();
            this.cmbBarcodeSymbology = new System.Windows.Forms.ComboBox();
            this.chkTenderReceiptsAreReprinted = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chkReturnsPrintedTwice = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbEmailFormProfile = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLogoSize = new System.Windows.Forms.ComboBox();
            this.txtImage = new System.Windows.Forms.TextBox();
            this.lblLogo = new System.Windows.Forms.Label();
            this.btnDeleteImage = new LSOne.Controls.ContextButton();
            this.btnAddImage = new LSOne.Controls.ContextButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFormProfile
            // 
            this.lblFormProfile.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormProfile, "lblFormProfile");
            this.lblFormProfile.Name = "lblFormProfile";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // cmbFormProfile
            // 
            this.cmbFormProfile.AddList = null;
            this.cmbFormProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFormProfile, "cmbFormProfile");
            this.cmbFormProfile.MaxLength = 32767;
            this.cmbFormProfile.Name = "cmbFormProfile";
            this.cmbFormProfile.NoChangeAllowed = false;
            this.cmbFormProfile.OnlyDisplayID = false;
            this.cmbFormProfile.RemoveList = null;
            this.cmbFormProfile.RowHeight = ((short)(22));
            this.cmbFormProfile.SecondaryData = null;
            this.cmbFormProfile.SelectedData = null;
            this.cmbFormProfile.SelectedDataID = null;
            this.cmbFormProfile.SelectionList = null;
            this.cmbFormProfile.SkipIDColumn = true;
            this.cmbFormProfile.RequestData += new System.EventHandler(this.cmbFormProfile_RequestData);
            // 
            // picLogoImage
            // 
            this.picLogoImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.picLogoImage, "picLogoImage");
            this.picLogoImage.Name = "picLogoImage";
            this.picLogoImage.TabStop = false;
            // 
            // lblPreview
            // 
            this.lblPreview.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPreview, "lblPreview");
            this.lblPreview.Name = "lblPreview";
            // 
            // lblWidth
            // 
            this.lblWidth.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblWidth, "lblWidth");
            this.lblWidth.Name = "lblWidth";
            // 
            // txtFormInfo4
            // 
            resources.ApplyResources(this.txtFormInfo4, "txtFormInfo4");
            this.txtFormInfo4.Name = "txtFormInfo4";
            // 
            // lblFormInfo4
            // 
            this.lblFormInfo4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormInfo4, "lblFormInfo4");
            this.lblFormInfo4.Name = "lblFormInfo4";
            // 
            // txtFormInfo3
            // 
            resources.ApplyResources(this.txtFormInfo3, "txtFormInfo3");
            this.txtFormInfo3.Name = "txtFormInfo3";
            // 
            // lblFormInfo3
            // 
            this.lblFormInfo3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormInfo3, "lblFormInfo3");
            this.lblFormInfo3.Name = "lblFormInfo3";
            // 
            // txtFormInfo2
            // 
            resources.ApplyResources(this.txtFormInfo2, "txtFormInfo2");
            this.txtFormInfo2.Name = "txtFormInfo2";
            // 
            // lblFormInfo2
            // 
            this.lblFormInfo2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormInfo2, "lblFormInfo2");
            this.lblFormInfo2.Name = "lblFormInfo2";
            // 
            // txtFormInfo1
            // 
            resources.ApplyResources(this.txtFormInfo1, "txtFormInfo1");
            this.txtFormInfo1.Name = "txtFormInfo1";
            // 
            // lblFormInfo1
            // 
            this.lblFormInfo1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFormInfo1, "lblFormInfo1");
            this.lblFormInfo1.Name = "lblFormInfo1";
            // 
            // lblBarcodeSymbology
            // 
            this.lblBarcodeSymbology.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBarcodeSymbology, "lblBarcodeSymbology");
            this.lblBarcodeSymbology.Name = "lblBarcodeSymbology";
            // 
            // cmbBarcodeSymbology
            // 
            this.cmbBarcodeSymbology.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBarcodeSymbology.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBarcodeSymbology, "cmbBarcodeSymbology");
            this.cmbBarcodeSymbology.Name = "cmbBarcodeSymbology";
            // 
            // chkTenderReceiptsAreReprinted
            // 
            resources.ApplyResources(this.chkTenderReceiptsAreReprinted, "chkTenderReceiptsAreReprinted");
            this.chkTenderReceiptsAreReprinted.Name = "chkTenderReceiptsAreReprinted";
            this.chkTenderReceiptsAreReprinted.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // chkReturnsPrintedTwice
            // 
            resources.ApplyResources(this.chkReturnsPrintedTwice, "chkReturnsPrintedTwice");
            this.chkReturnsPrintedTwice.Name = "chkReturnsPrintedTwice";
            this.chkReturnsPrintedTwice.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // cmbEmailFormProfile
            // 
            this.cmbEmailFormProfile.AddList = null;
            this.cmbEmailFormProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbEmailFormProfile, "cmbEmailFormProfile");
            this.cmbEmailFormProfile.MaxLength = 32767;
            this.cmbEmailFormProfile.Name = "cmbEmailFormProfile";
            this.cmbEmailFormProfile.NoChangeAllowed = false;
            this.cmbEmailFormProfile.OnlyDisplayID = false;
            this.cmbEmailFormProfile.RemoveList = null;
            this.cmbEmailFormProfile.RowHeight = ((short)(22));
            this.cmbEmailFormProfile.SecondaryData = null;
            this.cmbEmailFormProfile.SelectedData = null;
            this.cmbEmailFormProfile.SelectedDataID = null;
            this.cmbEmailFormProfile.SelectionList = null;
            this.cmbEmailFormProfile.SkipIDColumn = true;
            this.cmbEmailFormProfile.RequestData += new System.EventHandler(this.cmbEmailFormProfile_RequestData);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbLogoSize
            // 
            this.cmbLogoSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogoSize.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLogoSize, "cmbLogoSize");
            this.cmbLogoSize.Name = "cmbLogoSize";
            // 
            // txtImage
            // 
            resources.ApplyResources(this.txtImage, "txtImage");
            this.txtImage.Name = "txtImage";
            // 
            // lblLogo
            // 
            this.lblLogo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLogo, "lblLogo");
            this.lblLogo.Name = "lblLogo";
            // 
            // btnDeleteImage
            // 
            this.btnDeleteImage.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteImage.Context = LSOne.Controls.ButtonType.Remove;
            resources.ApplyResources(this.btnDeleteImage, "btnDeleteImage");
            this.btnDeleteImage.Name = "btnDeleteImage";
            this.btnDeleteImage.Click += new System.EventHandler(this.btnDeleteImage_Click);
            // 
            // btnAddImage
            // 
            this.btnAddImage.BackColor = System.Drawing.Color.Transparent;
            this.btnAddImage.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnAddImage, "btnAddImage");
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // StoreFormSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnDeleteImage);
            this.Controls.Add(this.btnAddImage);
            this.Controls.Add(this.txtImage);
            this.Controls.Add(this.lblLogo);
            this.Controls.Add(this.cmbLogoSize);
            this.Controls.Add(this.cmbEmailFormProfile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkTenderReceiptsAreReprinted);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.chkReturnsPrintedTwice);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbBarcodeSymbology);
            this.Controls.Add(this.lblBarcodeSymbology);
            this.Controls.Add(this.txtFormInfo4);
            this.Controls.Add(this.lblFormInfo4);
            this.Controls.Add(this.txtFormInfo3);
            this.Controls.Add(this.lblFormInfo3);
            this.Controls.Add(this.txtFormInfo2);
            this.Controls.Add(this.lblFormInfo2);
            this.Controls.Add(this.txtFormInfo1);
            this.Controls.Add(this.lblFormInfo1);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.picLogoImage);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.cmbFormProfile);
            this.Controls.Add(this.lblFormProfile);
            this.DoubleBuffered = true;
            this.Name = "StoreFormSettingsPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFormProfile;
        private DualDataComboBox cmbFormProfile;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.PictureBox picLogoImage;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.TextBox txtFormInfo4;
        private System.Windows.Forms.Label lblFormInfo4;
        private System.Windows.Forms.TextBox txtFormInfo3;
        private System.Windows.Forms.Label lblFormInfo3;
        private System.Windows.Forms.TextBox txtFormInfo2;
        private System.Windows.Forms.Label lblFormInfo2;
        private System.Windows.Forms.TextBox txtFormInfo1;
        private System.Windows.Forms.Label lblFormInfo1;
        private System.Windows.Forms.ComboBox cmbBarcodeSymbology;
        private System.Windows.Forms.Label lblBarcodeSymbology;
        private System.Windows.Forms.CheckBox chkTenderReceiptsAreReprinted;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkReturnsPrintedTwice;
        private System.Windows.Forms.Label label12;
        private DualDataComboBox cmbEmailFormProfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbLogoSize;
        private System.Windows.Forms.TextBox txtImage;
        private System.Windows.Forms.Label lblLogo;
        private ContextButton btnDeleteImage;
        private ContextButton btnAddImage;
    }
}
