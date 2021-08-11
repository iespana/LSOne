namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class ImageDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblImage = new System.Windows.Forms.Label();
            this.txtImage = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblBackground = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.cmbBackground = new LSOne.Controls.DualDataComboBox();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.grpPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtDescription
            // 
            resources.ApplyResources(this.txtDescription, "txtDescription");
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // lblImage
            // 
            resources.ApplyResources(this.lblImage, "lblImage");
            this.lblImage.Name = "lblImage";
            // 
            // txtImage
            // 
            resources.ApplyResources(this.txtImage, "txtImage");
            this.txtImage.Name = "txtImage";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // lblType
            // 
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            // 
            // lblBackground
            // 
            resources.ApplyResources(this.lblBackground, "lblBackground");
            this.lblBackground.Name = "lblBackground";
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // imageBox
            // 
            resources.ApplyResources(this.imageBox, "imageBox");
            this.imageBox.Name = "imageBox";
            this.imageBox.TabStop = false;
            // 
            // cmbBackground
            // 
            this.cmbBackground.AddList = null;
            this.cmbBackground.AllowKeyboardSelection = false;
            this.cmbBackground.EnableTextBox = true;
            resources.ApplyResources(this.cmbBackground, "cmbBackground");
            this.cmbBackground.MaxLength = 32767;
            this.cmbBackground.Name = "cmbBackground";
            this.cmbBackground.NoChangeAllowed = false;
            this.cmbBackground.OnlyDisplayID = false;
            this.cmbBackground.ReceiveKeyboardEvents = true;
            this.cmbBackground.RemoveList = null;
            this.cmbBackground.RowHeight = ((short)(22));
            this.cmbBackground.SecondaryData = null;
            this.cmbBackground.SelectedData = null;
            this.cmbBackground.SelectedDataID = null;
            this.cmbBackground.SelectionList = null;
            this.cmbBackground.SkipIDColumn = true;
            this.cmbBackground.RequestData += new System.EventHandler(this.cmbBackground_RequestData);
            this.cmbBackground.SelectedDataChanged += new System.EventHandler(this.cmbBackground_SelectedDataChanged);
            this.cmbBackground.RequestClear += new System.EventHandler(this.cmbBackground_RequestClear);
            // 
            // grpPreview
            // 
            this.grpPreview.Controls.Add(this.imageBox);
            resources.ApplyResources(this.grpPreview, "grpPreview");
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.TabStop = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ImageDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.cmbBackground);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblBackground);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.txtImage);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "ImageDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.txtDescription, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.txtImage, 0);
            this.Controls.SetChildIndex(this.lblImage, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.lblType, 0);
            this.Controls.SetChildIndex(this.lblBackground, 0);
            this.Controls.SetChildIndex(this.btnBrowse, 0);
            this.Controls.SetChildIndex(this.cmbBackground, 0);
            this.Controls.SetChildIndex(this.grpPreview, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.grpPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.TextBox txtImage;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblBackground;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox imageBox;
        private Controls.DualDataComboBox cmbBackground;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}