using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    partial class HardwareProfilePrinterPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfilePrinterPage));
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkPrintBinaryConversion = new System.Windows.Forms.CheckBox();
            this.cmbPrinter = new System.Windows.Forms.ComboBox();
            this.ntbCharset = new LSOne.Controls.NumericTextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ntbExtraLines = new LSOne.Controls.NumericTextBox();
            this.lblPrinterExtraLines = new System.Windows.Forms.Label();
            this.folderLocation = new System.Windows.Forms.ErrorProvider(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.btnEditPrintingHosts = new LSOne.Controls.ContextButton();
            this.cmbPrintingHosts = new LSOne.Controls.DualDataComboBox();
            this.cmbDeviceName = new LSOne.Controls.DualDataComboBox();
            this.btnEditDeviceName = new LSOne.Controls.ContextButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderLocation)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkPrintBinaryConversion
            // 
            resources.ApplyResources(this.chkPrintBinaryConversion, "chkPrintBinaryConversion");
            this.chkPrintBinaryConversion.Name = "chkPrintBinaryConversion";
            this.chkPrintBinaryConversion.UseVisualStyleBackColor = true;
            // 
            // cmbPrinter
            // 
            this.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinter.FormattingEnabled = true;
            this.cmbPrinter.Items.AddRange(new object[] {
            resources.GetString("cmbPrinter.Items"),
            resources.GetString("cmbPrinter.Items1"),
            resources.GetString("cmbPrinter.Items2"),
            resources.GetString("cmbPrinter.Items3")});
            resources.ApplyResources(this.cmbPrinter, "cmbPrinter");
            this.cmbPrinter.Name = "cmbPrinter";
            this.cmbPrinter.SelectedIndexChanged += new System.EventHandler(this.cmbPrinter_SelectedIndexChanged);
            // 
            // ntbCharset
            // 
            this.ntbCharset.AllowDecimal = false;
            this.ntbCharset.AllowNegative = false;
            this.ntbCharset.CultureInfo = null;
            this.ntbCharset.DecimalLetters = 2;
            this.ntbCharset.ForeColor = System.Drawing.Color.Black;
            this.ntbCharset.HasMinValue = false;
            resources.ApplyResources(this.ntbCharset, "ntbCharset");
            this.ntbCharset.MaxValue = 9999D;
            this.ntbCharset.MinValue = 0D;
            this.ntbCharset.Name = "ntbCharset";
            this.ntbCharset.Value = 0D;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ntbExtraLines
            // 
            this.ntbExtraLines.AllowDecimal = false;
            this.ntbExtraLines.AllowNegative = false;
            this.ntbExtraLines.CultureInfo = null;
            this.ntbExtraLines.DecimalLetters = 2;
            this.ntbExtraLines.ForeColor = System.Drawing.Color.Black;
            this.ntbExtraLines.HasMinValue = true;
            resources.ApplyResources(this.ntbExtraLines, "ntbExtraLines");
            this.ntbExtraLines.MaxValue = 99D;
            this.ntbExtraLines.MinValue = 0D;
            this.ntbExtraLines.Name = "ntbExtraLines";
            this.ntbExtraLines.Value = 0D;
            // 
            // lblPrinterExtraLines
            // 
            this.lblPrinterExtraLines.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPrinterExtraLines, "lblPrinterExtraLines");
            this.lblPrinterExtraLines.Name = "lblPrinterExtraLines";
            // 
            // folderLocation
            // 
            this.folderLocation.ContainerControl = this;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // btnEditPrintingHosts
            // 
            this.btnEditPrintingHosts.BackColor = System.Drawing.Color.Transparent;
            this.btnEditPrintingHosts.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditPrintingHosts, "btnEditPrintingHosts");
            this.btnEditPrintingHosts.Name = "btnEditPrintingHosts";
            this.btnEditPrintingHosts.Click += new System.EventHandler(this.btnEditPrintingHosts_Click);
            // 
            // cmbPrintingHosts
            // 
            this.cmbPrintingHosts.AddList = null;
            this.cmbPrintingHosts.AllowKeyboardSelection = false;
            this.cmbPrintingHosts.EnableTextBox = true;
            resources.ApplyResources(this.cmbPrintingHosts, "cmbPrintingHosts");
            this.cmbPrintingHosts.MaxLength = 32767;
            this.cmbPrintingHosts.Name = "cmbPrintingHosts";
            this.cmbPrintingHosts.NoChangeAllowed = false;
            this.cmbPrintingHosts.OnlyDisplayID = false;
            this.cmbPrintingHosts.RemoveList = null;
            this.cmbPrintingHosts.RowHeight = ((short)(22));
            this.cmbPrintingHosts.SecondaryData = null;
            this.cmbPrintingHosts.SelectedData = null;
            this.cmbPrintingHosts.SelectedDataID = null;
            this.cmbPrintingHosts.SelectionList = null;
            this.cmbPrintingHosts.SkipIDColumn = true;
            this.cmbPrintingHosts.RequestData += new System.EventHandler(this.cmbPrintingHosts_RequestData);
            this.cmbPrintingHosts.SelectedDataChanged += new System.EventHandler(this.cmbPrintingHosts_SelectedDataChanged);
            this.cmbPrintingHosts.RequestClear += new System.EventHandler(this.cmbPrintingHosts_RequestClear);
            // 
            // cmbDeviceName
            // 
            this.cmbDeviceName.AddList = null;
            this.cmbDeviceName.AllowKeyboardSelection = false;
            this.cmbDeviceName.EnableTextBox = true;
            resources.ApplyResources(this.cmbDeviceName, "cmbDeviceName");
            this.cmbDeviceName.MaxLength = 32767;
            this.cmbDeviceName.Name = "cmbDeviceName";
            this.cmbDeviceName.NoChangeAllowed = false;
            this.cmbDeviceName.OnlyDisplayID = false;
            this.cmbDeviceName.RemoveList = null;
            this.cmbDeviceName.RowHeight = ((short)(22));
            this.cmbDeviceName.SecondaryData = null;
            this.cmbDeviceName.SelectedData = null;
            this.cmbDeviceName.SelectedDataID = null;
            this.cmbDeviceName.SelectionList = null;
            this.cmbDeviceName.SkipIDColumn = true;
            this.cmbDeviceName.RequestData += new System.EventHandler(this.cmbDeviceName_RequestData);
            this.cmbDeviceName.SelectedDataChanged += new System.EventHandler(this.cmbDeviceName_SelectedDataChanged);
            this.cmbDeviceName.RequestClear += new System.EventHandler(this.cmbDeviceName_RequestClear);
            this.cmbDeviceName.SelectedDataCleared += new System.EventHandler(this.cmbDeviceName_SelectedDataCleared);
            this.cmbDeviceName.Leave += new System.EventHandler(this.cmbDeviceName_Leave);
            // 
            // btnEditDeviceName
            // 
            this.btnEditDeviceName.BackColor = System.Drawing.Color.Transparent;
            this.btnEditDeviceName.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditDeviceName, "btnEditDeviceName");
            this.btnEditDeviceName.Name = "btnEditDeviceName";
            this.btnEditDeviceName.Click += new System.EventHandler(this.btnEditDeviceName_Click);
            // 
            // HardwareProfilePrinterPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnEditDeviceName);
            this.Controls.Add(this.cmbDeviceName);
            this.Controls.Add(this.cmbPrintingHosts);
            this.Controls.Add(this.btnEditPrintingHosts);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ntbExtraLines);
            this.Controls.Add(this.lblPrinterExtraLines);
            this.Controls.Add(this.cmbPrinter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkPrintBinaryConversion);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ntbCharset);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.Name = "HardwareProfilePrinterPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderLocation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbCharset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkPrintBinaryConversion;
        private System.Windows.Forms.ComboBox cmbPrinter;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private NumericTextBox ntbExtraLines;
        private System.Windows.Forms.Label lblPrinterExtraLines;
        private System.Windows.Forms.ErrorProvider folderLocation;
        private LinkFields linkFields1;
        private System.Windows.Forms.Label label8;
        private ContextButton btnEditPrintingHosts;
        private DualDataComboBox cmbPrintingHosts;
        private DualDataComboBox cmbDeviceName;
        private ContextButton btnEditDeviceName;
    }
}
