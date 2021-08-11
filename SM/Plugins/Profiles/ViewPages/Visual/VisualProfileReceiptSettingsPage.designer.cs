using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Visual
{
    partial class VisualProfileReceiptSettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualProfileReceiptSettingsPage));
            this.cwReceiptReturnBorderColor = new LSOne.Controls.ColorWell();
            this.btnEditReceiptReturnImage = new LSOne.Controls.ContextButton();
            this.txtReceiptReturnImage = new System.Windows.Forms.TextBox();
            this.lblReceiptReturnBorderColor = new System.Windows.Forms.Label();
            this.lblReceiptReturnImageID = new System.Windows.Forms.Label();
            this.cmbReceiptReturnImageLayout = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbPaymentLines = new System.Windows.Forms.ComboBox();
            this.lblPaymentLineSize = new System.Windows.Forms.Label();
            this.chkShowCurrSymOnColumns = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cwReceiptReturnBorderColor
            // 
            resources.ApplyResources(this.cwReceiptReturnBorderColor, "cwReceiptReturnBorderColor");
            this.cwReceiptReturnBorderColor.Name = "cwReceiptReturnBorderColor";
            this.cwReceiptReturnBorderColor.SelectedColor = System.Drawing.Color.White;
            // 
            // btnEditReceiptReturnImage
            // 
            this.btnEditReceiptReturnImage.BackColor = System.Drawing.Color.Transparent;
            this.btnEditReceiptReturnImage.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditReceiptReturnImage, "btnEditReceiptReturnImage");
            this.btnEditReceiptReturnImage.Name = "btnEditReceiptReturnImage";
            this.btnEditReceiptReturnImage.Click += new System.EventHandler(this.btnEditReceiptReturnImage_Click);
            // 
            // txtReceiptReturnImage
            // 
            resources.ApplyResources(this.txtReceiptReturnImage, "txtReceiptReturnImage");
            this.txtReceiptReturnImage.Name = "txtReceiptReturnImage";
            // 
            // lblReceiptReturnBorderColor
            // 
            this.lblReceiptReturnBorderColor.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblReceiptReturnBorderColor, "lblReceiptReturnBorderColor");
            this.lblReceiptReturnBorderColor.Name = "lblReceiptReturnBorderColor";
            // 
            // lblReceiptReturnImageID
            // 
            this.lblReceiptReturnImageID.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblReceiptReturnImageID, "lblReceiptReturnImageID");
            this.lblReceiptReturnImageID.Name = "lblReceiptReturnImageID";
            // 
            // cmbReceiptReturnImageLayout
            // 
            this.cmbReceiptReturnImageLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReceiptReturnImageLayout.FormattingEnabled = true;
            this.cmbReceiptReturnImageLayout.Items.AddRange(new object[] {
            resources.GetString("cmbReceiptReturnImageLayout.Items"),
            resources.GetString("cmbReceiptReturnImageLayout.Items1"),
            resources.GetString("cmbReceiptReturnImageLayout.Items2"),
            resources.GetString("cmbReceiptReturnImageLayout.Items3"),
            resources.GetString("cmbReceiptReturnImageLayout.Items4")});
            resources.ApplyResources(this.cmbReceiptReturnImageLayout, "cmbReceiptReturnImageLayout");
            this.cmbReceiptReturnImageLayout.Name = "cmbReceiptReturnImageLayout";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbPaymentLines
            // 
            this.cmbPaymentLines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentLines.FormattingEnabled = true;
            this.cmbPaymentLines.Items.AddRange(new object[] {
            resources.GetString("cmbPaymentLines.Items"),
            resources.GetString("cmbPaymentLines.Items1"),
            resources.GetString("cmbPaymentLines.Items2"),
            resources.GetString("cmbPaymentLines.Items3")});
            resources.ApplyResources(this.cmbPaymentLines, "cmbPaymentLines");
            this.cmbPaymentLines.Name = "cmbPaymentLines";
            // 
            // lblPaymentLineSize
            // 
            this.lblPaymentLineSize.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPaymentLineSize, "lblPaymentLineSize");
            this.lblPaymentLineSize.Name = "lblPaymentLineSize";
            // 
            // chkShowCurrSymOnColumns
            // 
            resources.ApplyResources(this.chkShowCurrSymOnColumns, "chkShowCurrSymOnColumns");
            this.chkShowCurrSymOnColumns.Name = "chkShowCurrSymOnColumns";
            this.chkShowCurrSymOnColumns.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // VisualProfileReceiptSettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chkShowCurrSymOnColumns);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cwReceiptReturnBorderColor);
            this.Controls.Add(this.btnEditReceiptReturnImage);
            this.Controls.Add(this.txtReceiptReturnImage);
            this.Controls.Add(this.lblReceiptReturnBorderColor);
            this.Controls.Add(this.lblReceiptReturnImageID);
            this.Controls.Add(this.cmbReceiptReturnImageLayout);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbPaymentLines);
            this.Controls.Add(this.lblPaymentLineSize);
            this.DoubleBuffered = true;
            this.Name = "VisualProfileReceiptSettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ColorWell cwReceiptReturnBorderColor;
        private ContextButton btnEditReceiptReturnImage;
        private System.Windows.Forms.TextBox txtReceiptReturnImage;
        private System.Windows.Forms.Label lblReceiptReturnBorderColor;
        private System.Windows.Forms.Label lblReceiptReturnImageID;
        private System.Windows.Forms.ComboBox cmbReceiptReturnImageLayout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbPaymentLines;
        private System.Windows.Forms.Label lblPaymentLineSize;
        private System.Windows.Forms.CheckBox chkShowCurrSymOnColumns;
        private System.Windows.Forms.Label label8;
    }
}
