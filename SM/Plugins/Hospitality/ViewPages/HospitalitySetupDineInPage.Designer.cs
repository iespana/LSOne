using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalitySetupDineInPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalitySetupDineInPage));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnResetColors = new System.Windows.Forms.Button();
            this.cmbOrderConfirmedColorF = new DualDataComboBox();
            this.cmbOrderFinishedColorF = new DualDataComboBox();
            this.cmbOrderStartedColorF = new DualDataComboBox();
            this.cmbOrderPrintedColorF = new DualDataComboBox();
            this.cmbOrderNotPrintedColorF = new DualDataComboBox();
            this.cmbTableLockedColorF = new DualDataComboBox();
            this.cmbTableNotAvailColorF = new DualDataComboBox();
            this.cmbTableFreeColorF = new DualDataComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.wellOrderConfirmed = new ColorWell();
            this.wellOrderFinished = new ColorWell();
            this.wellOrderStarted = new ColorWell();
            this.wellOrderPrinted = new ColorWell();
            this.wellOrderNotPrinted = new ColorWell();
            this.wellTableLocked = new ColorWell();
            this.wellTableNotAvail = new ColorWell();
            this.wellTableFree = new ColorWell();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbDineInTableSelection = new System.Windows.Forms.ComboBox();
            this.cmbDineInTableLocking = new System.Windows.Forms.ComboBox();
            this.cmbDineInSalesType = new DualDataComboBox();
            this.groupBox1.SuspendLayout();
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
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.btnResetColors);
            this.groupBox1.Controls.Add(this.cmbOrderConfirmedColorF);
            this.groupBox1.Controls.Add(this.cmbOrderFinishedColorF);
            this.groupBox1.Controls.Add(this.cmbOrderStartedColorF);
            this.groupBox1.Controls.Add(this.cmbOrderPrintedColorF);
            this.groupBox1.Controls.Add(this.cmbOrderNotPrintedColorF);
            this.groupBox1.Controls.Add(this.cmbTableLockedColorF);
            this.groupBox1.Controls.Add(this.cmbTableNotAvailColorF);
            this.groupBox1.Controls.Add(this.cmbTableFreeColorF);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.wellOrderConfirmed);
            this.groupBox1.Controls.Add(this.wellOrderFinished);
            this.groupBox1.Controls.Add(this.wellOrderStarted);
            this.groupBox1.Controls.Add(this.wellOrderPrinted);
            this.groupBox1.Controls.Add(this.wellOrderNotPrinted);
            this.groupBox1.Controls.Add(this.wellTableLocked);
            this.groupBox1.Controls.Add(this.wellTableNotAvail);
            this.groupBox1.Controls.Add(this.wellTableFree);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnResetColors
            // 
            resources.ApplyResources(this.btnResetColors, "btnResetColors");
            this.btnResetColors.Name = "btnResetColors";
            this.btnResetColors.UseVisualStyleBackColor = true;
            this.btnResetColors.Click += new System.EventHandler(this.btnResetColors_Click);
            // 
            // cmbOrderConfirmedColorF
            // 
            resources.ApplyResources(this.cmbOrderConfirmedColorF, "cmbOrderConfirmedColorF");
            this.cmbOrderConfirmedColorF.MaxLength = 32767;
            this.cmbOrderConfirmedColorF.Name = "cmbOrderConfirmedColorF";
            this.cmbOrderConfirmedColorF.SelectedData = null;
            this.cmbOrderConfirmedColorF.SkipIDColumn = true;
            this.cmbOrderConfirmedColorF.RequestData += new System.EventHandler(this.cmbOrderConfirmedColorF_RequestData);
            // 
            // cmbOrderFinishedColorF
            // 
            resources.ApplyResources(this.cmbOrderFinishedColorF, "cmbOrderFinishedColorF");
            this.cmbOrderFinishedColorF.MaxLength = 32767;
            this.cmbOrderFinishedColorF.Name = "cmbOrderFinishedColorF";
            this.cmbOrderFinishedColorF.SelectedData = null;
            this.cmbOrderFinishedColorF.SkipIDColumn = true;
            this.cmbOrderFinishedColorF.RequestData += new System.EventHandler(this.cmbOrderFinishedColorF_RequestData);
            // 
            // cmbOrderStartedColorF
            // 
            resources.ApplyResources(this.cmbOrderStartedColorF, "cmbOrderStartedColorF");
            this.cmbOrderStartedColorF.MaxLength = 32767;
            this.cmbOrderStartedColorF.Name = "cmbOrderStartedColorF";
            this.cmbOrderStartedColorF.SelectedData = null;
            this.cmbOrderStartedColorF.SkipIDColumn = true;
            this.cmbOrderStartedColorF.RequestData += new System.EventHandler(this.cmbOrderStartedColorF_RequestData);
            // 
            // cmbOrderPrintedColorF
            // 
            resources.ApplyResources(this.cmbOrderPrintedColorF, "cmbOrderPrintedColorF");
            this.cmbOrderPrintedColorF.MaxLength = 32767;
            this.cmbOrderPrintedColorF.Name = "cmbOrderPrintedColorF";
            this.cmbOrderPrintedColorF.SelectedData = null;
            this.cmbOrderPrintedColorF.SkipIDColumn = true;
            this.cmbOrderPrintedColorF.RequestData += new System.EventHandler(this.cmbOrderPrintedColorF_RequestData);
            // 
            // cmbOrderNotPrintedColorF
            // 
            resources.ApplyResources(this.cmbOrderNotPrintedColorF, "cmbOrderNotPrintedColorF");
            this.cmbOrderNotPrintedColorF.MaxLength = 32767;
            this.cmbOrderNotPrintedColorF.Name = "cmbOrderNotPrintedColorF";
            this.cmbOrderNotPrintedColorF.SelectedData = null;
            this.cmbOrderNotPrintedColorF.SkipIDColumn = true;
            this.cmbOrderNotPrintedColorF.RequestData += new System.EventHandler(this.cmbOrderNotPrintedColorF_RequestData);
            // 
            // cmbTableLockedColorF
            // 
            resources.ApplyResources(this.cmbTableLockedColorF, "cmbTableLockedColorF");
            this.cmbTableLockedColorF.MaxLength = 32767;
            this.cmbTableLockedColorF.Name = "cmbTableLockedColorF";
            this.cmbTableLockedColorF.SelectedData = null;
            this.cmbTableLockedColorF.SkipIDColumn = true;
            this.cmbTableLockedColorF.RequestData += new System.EventHandler(this.cmbTableLockedColorF_RequestData);
            // 
            // cmbTableNotAvailColorF
            // 
            resources.ApplyResources(this.cmbTableNotAvailColorF, "cmbTableNotAvailColorF");
            this.cmbTableNotAvailColorF.MaxLength = 32767;
            this.cmbTableNotAvailColorF.Name = "cmbTableNotAvailColorF";
            this.cmbTableNotAvailColorF.SelectedData = null;
            this.cmbTableNotAvailColorF.SkipIDColumn = true;
            this.cmbTableNotAvailColorF.RequestData += new System.EventHandler(this.cmbTableNotAvailColorF_RequestData);
            // 
            // cmbTableFreeColorF
            // 
            resources.ApplyResources(this.cmbTableFreeColorF, "cmbTableFreeColorF");
            this.cmbTableFreeColorF.MaxLength = 32767;
            this.cmbTableFreeColorF.Name = "cmbTableFreeColorF";
            this.cmbTableFreeColorF.SelectedData = null;
            this.cmbTableFreeColorF.SkipIDColumn = true;
            this.cmbTableFreeColorF.RequestData += new System.EventHandler(this.cmbTableFreeColorF_RequestData);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // wellOrderConfirmed
            // 
            resources.ApplyResources(this.wellOrderConfirmed, "wellOrderConfirmed");
            this.wellOrderConfirmed.Name = "wellOrderConfirmed";
            this.wellOrderConfirmed.SelectedColor = System.Drawing.Color.White;
            // 
            // wellOrderFinished
            // 
            resources.ApplyResources(this.wellOrderFinished, "wellOrderFinished");
            this.wellOrderFinished.Name = "wellOrderFinished";
            this.wellOrderFinished.SelectedColor = System.Drawing.Color.White;
            // 
            // wellOrderStarted
            // 
            resources.ApplyResources(this.wellOrderStarted, "wellOrderStarted");
            this.wellOrderStarted.Name = "wellOrderStarted";
            this.wellOrderStarted.SelectedColor = System.Drawing.Color.White;
            // 
            // wellOrderPrinted
            // 
            resources.ApplyResources(this.wellOrderPrinted, "wellOrderPrinted");
            this.wellOrderPrinted.Name = "wellOrderPrinted";
            this.wellOrderPrinted.SelectedColor = System.Drawing.Color.White;
            // 
            // wellOrderNotPrinted
            // 
            resources.ApplyResources(this.wellOrderNotPrinted, "wellOrderNotPrinted");
            this.wellOrderNotPrinted.Name = "wellOrderNotPrinted";
            this.wellOrderNotPrinted.SelectedColor = System.Drawing.Color.White;
            // 
            // wellTableLocked
            // 
            resources.ApplyResources(this.wellTableLocked, "wellTableLocked");
            this.wellTableLocked.Name = "wellTableLocked";
            this.wellTableLocked.SelectedColor = System.Drawing.Color.White;
            // 
            // wellTableNotAvail
            // 
            resources.ApplyResources(this.wellTableNotAvail, "wellTableNotAvail");
            this.wellTableNotAvail.Name = "wellTableNotAvail";
            this.wellTableNotAvail.SelectedColor = System.Drawing.Color.White;
            // 
            // wellTableFree
            // 
            resources.ApplyResources(this.wellTableFree, "wellTableFree");
            this.wellTableFree.Name = "wellTableFree";
            this.wellTableFree.SelectedColor = System.Drawing.Color.White;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
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
            // cmbDineInSalesType
            // 
            resources.ApplyResources(this.cmbDineInSalesType, "cmbDineInSalesType");
            this.cmbDineInSalesType.MaxLength = 32767;
            this.cmbDineInSalesType.Name = "cmbDineInSalesType";
            this.cmbDineInSalesType.SelectedData = null;
            this.cmbDineInSalesType.SkipIDColumn = true;
            this.cmbDineInSalesType.RequestData += new System.EventHandler(this.cmbDineInSalesType_RequestData);
            // 
            // HospitalitySetupDineInPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbDineInTableLocking);
            this.Controls.Add(this.cmbDineInTableSelection);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbDineInSalesType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalitySetupDineInPage";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbDineInSalesType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private ColorWell wellOrderConfirmed;
        private ColorWell wellOrderFinished;
        private ColorWell wellOrderStarted;
        private ColorWell wellOrderPrinted;
        private ColorWell wellOrderNotPrinted;
        private ColorWell wellTableLocked;
        private ColorWell wellTableNotAvail;
        private ColorWell wellTableFree;
        private System.Windows.Forms.ComboBox cmbDineInTableSelection;
        private System.Windows.Forms.ComboBox cmbDineInTableLocking;
        private DualDataComboBox cmbOrderConfirmedColorF;
        private DualDataComboBox cmbOrderFinishedColorF;
        private DualDataComboBox cmbOrderStartedColorF;
        private DualDataComboBox cmbOrderPrintedColorF;
        private DualDataComboBox cmbOrderNotPrintedColorF;
        private DualDataComboBox cmbTableLockedColorF;
        private DualDataComboBox cmbTableNotAvailColorF;
        private DualDataComboBox cmbTableFreeColorF;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnResetColors;
    }
}
