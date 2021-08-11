namespace LSOne.ViewPlugins.BarCodes.DialogPages
{
    partial class NewRetailItemBarCodePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRetailItemBarCodePage));
            this.tbBarCode = new System.Windows.Forms.TextBox();
            this.lblBarCodeSetup = new System.Windows.Forms.Label();
            this.lblDefaultBarCode = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.linkBarCode = new LSOne.Controls.LinkFields();
            this.cmbBarCodeSetup = new LSOne.Controls.DualDataComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbBarCode
            // 
            resources.ApplyResources(this.tbBarCode, "tbBarCode");
            this.tbBarCode.Name = "tbBarCode";
            this.tbBarCode.TextChanged += new System.EventHandler(this.tbBarCode_TextChanged);
            // 
            // lblBarCodeSetup
            // 
            resources.ApplyResources(this.lblBarCodeSetup, "lblBarCodeSetup");
            this.lblBarCodeSetup.Name = "lblBarCodeSetup";
            // 
            // lblDefaultBarCode
            // 
            resources.ApplyResources(this.lblDefaultBarCode, "lblDefaultBarCode");
            this.lblDefaultBarCode.Name = "lblDefaultBarCode";
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // linkBarCode
            // 
            this.linkBarCode.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkBarCode, "linkBarCode");
            this.linkBarCode.Name = "linkBarCode";
            this.linkBarCode.TabStop = false;
            // 
            // cmbBarCodeSetup
            // 
            this.cmbBarCodeSetup.AddList = null;
            this.cmbBarCodeSetup.AllowKeyboardSelection = false;
            this.cmbBarCodeSetup.EnableTextBox = true;
            resources.ApplyResources(this.cmbBarCodeSetup, "cmbBarCodeSetup");
            this.cmbBarCodeSetup.MaxLength = 32767;
            this.cmbBarCodeSetup.Name = "cmbBarCodeSetup";
            this.cmbBarCodeSetup.NoChangeAllowed = false;
            this.cmbBarCodeSetup.OnlyDisplayID = false;
            this.cmbBarCodeSetup.RemoveList = null;
            this.cmbBarCodeSetup.RowHeight = ((short)(22));
            this.cmbBarCodeSetup.SecondaryData = null;
            this.cmbBarCodeSetup.SelectedData = null;
            this.cmbBarCodeSetup.SelectedDataID = null;
            this.cmbBarCodeSetup.SelectionList = null;
            this.cmbBarCodeSetup.ShowDropDownOnTyping = true;
            this.cmbBarCodeSetup.SkipIDColumn = true;
            this.cmbBarCodeSetup.RequestData += new System.EventHandler(this.cmbBarCodeSetup_RequestData);
            this.cmbBarCodeSetup.SelectedDataChanged += new System.EventHandler(this.cmbBarCodeSetup_SelectedDataChanged);
            this.cmbBarCodeSetup.RequestClear += new System.EventHandler(this.cmbBarCodeSetup_RequestClear);
            // 
            // NewRetailItemBarCodePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.linkBarCode);
            this.Controls.Add(this.lblDefaultBarCode);
            this.Controls.Add(this.lblBarCodeSetup);
            this.Controls.Add(this.cmbBarCodeSetup);
            this.Controls.Add(this.tbBarCode);
            this.DoubleBuffered = true;
            this.Name = "NewRetailItemBarCodePage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbBarCode;
        private Controls.DualDataComboBox cmbBarCodeSetup;
        private System.Windows.Forms.Label lblBarCodeSetup;
        private System.Windows.Forms.Label lblDefaultBarCode;
        private Controls.LinkFields linkBarCode;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
