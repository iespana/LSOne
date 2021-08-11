using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalitySetupGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalitySetupGeneralPage));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkConfStatPrinting = new System.Windows.Forms.CheckBox();
            this.chkLogStatPrinting = new System.Windows.Forms.CheckBox();
            this.chkAutoLogoffAtPOSExit = new System.Windows.Forms.CheckBox();
            this.cmbNormalPOSSalesType = new DualDataComboBox();
            this.ntbOrdListScrlPageSize = new NumericTextBox();
            this.ntbDaysBOMMonitorExist = new NumericTextBox();
            this.ntbDaysBOMPrintExist = new NumericTextBox();
            this.cmbDineInTableLocking = new System.Windows.Forms.ComboBox();
            this.cmbDineInTableSelection = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ntbTableUpdateTimerInterval = new NumericTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkConfStatPrinting
            // 
            resources.ApplyResources(this.chkConfStatPrinting, "chkConfStatPrinting");
            this.chkConfStatPrinting.Name = "chkConfStatPrinting";
            this.chkConfStatPrinting.UseVisualStyleBackColor = true;
            // 
            // chkLogStatPrinting
            // 
            resources.ApplyResources(this.chkLogStatPrinting, "chkLogStatPrinting");
            this.chkLogStatPrinting.Name = "chkLogStatPrinting";
            this.chkLogStatPrinting.UseVisualStyleBackColor = true;
            // 
            // chkAutoLogoffAtPOSExit
            // 
            resources.ApplyResources(this.chkAutoLogoffAtPOSExit, "chkAutoLogoffAtPOSExit");
            this.chkAutoLogoffAtPOSExit.Name = "chkAutoLogoffAtPOSExit";
            this.chkAutoLogoffAtPOSExit.UseVisualStyleBackColor = true;
            // 
            // cmbNormalPOSSalesType
            // 
            resources.ApplyResources(this.cmbNormalPOSSalesType, "cmbNormalPOSSalesType");
            this.cmbNormalPOSSalesType.MaxLength = 32767;
            this.cmbNormalPOSSalesType.Name = "cmbNormalPOSSalesType";
            this.cmbNormalPOSSalesType.SelectedData = null;
            this.cmbNormalPOSSalesType.SkipIDColumn = true;
            this.cmbNormalPOSSalesType.RequestData += new System.EventHandler(this.cmbNormalPOSSalesType_RequestData);
            // 
            // ntbOrdListScrlPageSize
            // 
            this.ntbOrdListScrlPageSize.AllowDecimal = false;
            this.ntbOrdListScrlPageSize.AllowNegative = false;
            this.ntbOrdListScrlPageSize.DecimalLetters = 2;
            resources.ApplyResources(this.ntbOrdListScrlPageSize, "ntbOrdListScrlPageSize");
            this.ntbOrdListScrlPageSize.HasMinValue = false;
            this.ntbOrdListScrlPageSize.MaxValue = 0D;
            this.ntbOrdListScrlPageSize.MinValue = 0D;
            this.ntbOrdListScrlPageSize.Name = "ntbOrdListScrlPageSize";
            this.ntbOrdListScrlPageSize.Value = 0D;
            // 
            // ntbDaysBOMMonitorExist
            // 
            this.ntbDaysBOMMonitorExist.AllowDecimal = false;
            this.ntbDaysBOMMonitorExist.AllowNegative = false;
            this.ntbDaysBOMMonitorExist.DecimalLetters = 2;
            resources.ApplyResources(this.ntbDaysBOMMonitorExist, "ntbDaysBOMMonitorExist");
            this.ntbDaysBOMMonitorExist.HasMinValue = false;
            this.ntbDaysBOMMonitorExist.MaxValue = 0D;
            this.ntbDaysBOMMonitorExist.MinValue = 0D;
            this.ntbDaysBOMMonitorExist.Name = "ntbDaysBOMMonitorExist";
            this.ntbDaysBOMMonitorExist.Value = 0D;
            // 
            // ntbDaysBOMPrintExist
            // 
            this.ntbDaysBOMPrintExist.AllowDecimal = false;
            this.ntbDaysBOMPrintExist.AllowNegative = false;
            this.ntbDaysBOMPrintExist.DecimalLetters = 2;
            resources.ApplyResources(this.ntbDaysBOMPrintExist, "ntbDaysBOMPrintExist");
            this.ntbDaysBOMPrintExist.HasMinValue = false;
            this.ntbDaysBOMPrintExist.MaxValue = 0D;
            this.ntbDaysBOMPrintExist.MinValue = 0D;
            this.ntbDaysBOMPrintExist.Name = "ntbDaysBOMPrintExist";
            this.ntbDaysBOMPrintExist.Value = 0D;
            // 
            // cmbDineInTableLocking
            // 
            this.cmbDineInTableLocking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDineInTableLocking.FormattingEnabled = true;
            this.cmbDineInTableLocking.Items.AddRange(new object[] {
            resources.GetString("cmbDineInTableLocking.Items"),
            resources.GetString("cmbDineInTableLocking.Items1")});
            resources.ApplyResources(this.cmbDineInTableLocking, "cmbDineInTableLocking");
            this.cmbDineInTableLocking.Name = "cmbDineInTableLocking";
            // 
            // cmbDineInTableSelection
            // 
            this.cmbDineInTableSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDineInTableSelection.FormattingEnabled = true;
            this.cmbDineInTableSelection.Items.AddRange(new object[] {
            resources.GetString("cmbDineInTableSelection.Items"),
            resources.GetString("cmbDineInTableSelection.Items1")});
            resources.ApplyResources(this.cmbDineInTableSelection, "cmbDineInTableSelection");
            this.cmbDineInTableSelection.Name = "cmbDineInTableSelection";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // ntbTableUpdateTimerInterval
            // 
            this.ntbTableUpdateTimerInterval.AllowDecimal = false;
            this.ntbTableUpdateTimerInterval.AllowNegative = false;
            this.ntbTableUpdateTimerInterval.DecimalLetters = 2;
            this.ntbTableUpdateTimerInterval.HasMinValue = false;
            resources.ApplyResources(this.ntbTableUpdateTimerInterval, "ntbTableUpdateTimerInterval");
            this.ntbTableUpdateTimerInterval.MaxValue = 0D;
            this.ntbTableUpdateTimerInterval.MinValue = 0D;
            this.ntbTableUpdateTimerInterval.Name = "ntbTableUpdateTimerInterval";
            this.ntbTableUpdateTimerInterval.Value = 0D;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // HospitalitySetupGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ntbTableUpdateTimerInterval);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbDineInTableLocking);
            this.Controls.Add(this.cmbDineInTableSelection);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbNormalPOSSalesType);
            this.Controls.Add(this.ntbOrdListScrlPageSize);
            this.Controls.Add(this.chkAutoLogoffAtPOSExit);
            this.Controls.Add(this.ntbDaysBOMMonitorExist);
            this.Controls.Add(this.ntbDaysBOMPrintExist);
            this.Controls.Add(this.chkLogStatPrinting);
            this.Controls.Add(this.chkConfStatPrinting);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalitySetupGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkConfStatPrinting;
        private System.Windows.Forms.CheckBox chkLogStatPrinting;
        private NumericTextBox ntbDaysBOMPrintExist;
        private NumericTextBox ntbDaysBOMMonitorExist;
        private System.Windows.Forms.CheckBox chkAutoLogoffAtPOSExit;
        private NumericTextBox ntbOrdListScrlPageSize;
        private DualDataComboBox cmbNormalPOSSalesType;
        private System.Windows.Forms.ComboBox cmbDineInTableLocking;
        private System.Windows.Forms.ComboBox cmbDineInTableSelection;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private NumericTextBox ntbTableUpdateTimerInterval;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;

    }
}
