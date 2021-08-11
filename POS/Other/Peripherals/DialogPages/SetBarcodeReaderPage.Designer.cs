using LSOne.Controls;

namespace LSOne.Peripherals.DialogPages
{
    partial class SetBarcodeReaderPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetBarcodeReaderPage));
            this.lblPrinterName = new System.Windows.Forms.Label();
            this.cmbDeviceName = new LSOne.Controls.DualDataComboBox();
            this.chkDeviceConnected = new LSOne.Controls.TouchCheckBox();
            this.btnTest = new LSOne.Controls.TouchButton();
            this.errorProvider = new LSOne.Peripherals.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // lblPrinterName
            // 
            resources.ApplyResources(this.lblPrinterName, "lblPrinterName");
            this.lblPrinterName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblPrinterName.Name = "lblPrinterName";
            // 
            // cmbDeviceName
            // 
            this.cmbDeviceName.AddList = null;
            this.cmbDeviceName.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDeviceName, "cmbDeviceName");
            this.cmbDeviceName.EnableTextBox = true;
            this.cmbDeviceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbDeviceName.IsPOSControl = true;
            this.cmbDeviceName.MaxLength = 32767;
            this.cmbDeviceName.Name = "cmbDeviceName";
            this.cmbDeviceName.NoChangeAllowed = false;
            this.cmbDeviceName.OnlyDisplayID = false;
            this.cmbDeviceName.ReadOnly = true;
            this.cmbDeviceName.RemoveList = null;
            this.cmbDeviceName.RowHeight = ((short)(22));
            this.cmbDeviceName.SecondaryData = null;
            this.cmbDeviceName.SelectedData = null;
            this.cmbDeviceName.SelectedDataID = null;
            this.cmbDeviceName.SelectionList = null;
            this.cmbDeviceName.ShowDropDownOnTyping = true;
            this.cmbDeviceName.SkipIDColumn = true;
            this.cmbDeviceName.Touch = true;
            this.cmbDeviceName.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbDeviceName_DropDown);
            this.cmbDeviceName.SelectedDataChanged += new System.EventHandler(this.cmbDeviceName_SelectedDataChanged);
            this.cmbDeviceName.Leave += new System.EventHandler(this.cmbDeviceName_Leave);
            // 
            // chkDeviceConnected
            // 
            resources.ApplyResources(this.chkDeviceConnected, "chkDeviceConnected");
            this.chkDeviceConnected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.chkDeviceConnected.Name = "chkDeviceConnected";
            this.chkDeviceConnected.CheckedChanged += new System.EventHandler(this.chkDrawerConnected_CheckedChanged);
            // 
            // btnTest
            // 
            this.btnTest.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnTest.BackgroundImage = global::LSOne.Peripherals.Properties.Resources.Checkmark_white_32px;
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnTest.DrawBorder = false;
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            this.errorProvider.Name = "errorProvider";
            // 
            // SetBarcodeReaderPage
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.chkDeviceConnected);
            this.Controls.Add(this.cmbDeviceName);
            this.Controls.Add(this.lblPrinterName);
            this.Name = "SetBarcodeReaderPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrinterName;
        private LSOne.Controls.DualDataComboBox cmbDeviceName;
        private TouchCheckBox chkDeviceConnected;
        private LSOne.Controls.TouchButton btnTest;
        private TouchErrorProvider errorProvider;
    }
}
