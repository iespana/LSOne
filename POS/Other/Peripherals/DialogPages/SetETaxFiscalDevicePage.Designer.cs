using LSOne.Controls;

namespace LSOne.Peripherals.DialogPages
{
    partial class SetETaxFiscalDevicePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetETaxFiscalDevicePage));
            this.lblComPortName = new System.Windows.Forms.Label();
            this.cmbETaxComPortName = new LSOne.Controls.DualDataComboBox();
            this.chkDeviceConnected = new LSOne.Controls.TouchCheckBox();
            this.btnTest = new LSOne.Controls.TouchButton();
            this.errorProvider = new LSOne.Peripherals.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // lblComPortName
            // 
            resources.ApplyResources(this.lblComPortName, "lblComPortName");
            this.lblComPortName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblComPortName.Name = "lblComPortName";
            // 
            // cmbETaxComPortName
            // 
            this.cmbETaxComPortName.AddList = null;
            this.cmbETaxComPortName.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbETaxComPortName, "cmbETaxComPortName");
            this.cmbETaxComPortName.EnableTextBox = true;
            this.cmbETaxComPortName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.cmbETaxComPortName.IsPOSControl = true;
            this.cmbETaxComPortName.MaxLength = 32767;
            this.cmbETaxComPortName.Name = "cmbETaxComPortName";
            this.cmbETaxComPortName.NoChangeAllowed = false;
            this.cmbETaxComPortName.OnlyDisplayID = false;
            this.cmbETaxComPortName.ReadOnly = true;
            this.cmbETaxComPortName.RemoveList = null;
            this.cmbETaxComPortName.RowHeight = ((short)(22));
            this.cmbETaxComPortName.SecondaryData = null;
            this.cmbETaxComPortName.SelectedData = null;
            this.cmbETaxComPortName.SelectedDataID = null;
            this.cmbETaxComPortName.SelectionList = null;
            this.cmbETaxComPortName.ShowDropDownOnTyping = true;
            this.cmbETaxComPortName.SkipIDColumn = true;
            this.cmbETaxComPortName.Touch = true;
            this.cmbETaxComPortName.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbETaxComPortName_DropDown);
            this.cmbETaxComPortName.SelectedDataChanged += new System.EventHandler(this.cmbDeviceName_SelectedDataChanged);
            this.cmbETaxComPortName.Leave += new System.EventHandler(this.cmbDeviceName_Leave);
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
            // SetETaxFiscalDevicePage
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.chkDeviceConnected);
            this.Controls.Add(this.cmbETaxComPortName);
            this.Controls.Add(this.lblComPortName);
            this.Name = "SetETaxFiscalDevicePage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblComPortName;
        private LSOne.Controls.DualDataComboBox cmbETaxComPortName;
        private TouchCheckBox chkDeviceConnected;
        private LSOne.Controls.TouchButton btnTest;
        private TouchErrorProvider errorProvider;
    }
}
