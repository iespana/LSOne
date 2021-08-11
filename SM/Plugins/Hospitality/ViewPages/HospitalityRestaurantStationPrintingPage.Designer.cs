using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityRestaurantStationPrintingPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityRestaurantStationPrintingPage));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCheckPrinting = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPrinting = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbOutputLines = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ntbPrintingPriority = new NumericTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbStation3 = new DualDataComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbStation2 = new DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbStation1 = new DualDataComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbCheckPrinting
            // 
            this.cmbCheckPrinting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCheckPrinting.FormattingEnabled = true;
            this.cmbCheckPrinting.Items.AddRange(new object[] {
            resources.GetString("cmbCheckPrinting.Items"),
            resources.GetString("cmbCheckPrinting.Items1")});
            resources.ApplyResources(this.cmbCheckPrinting, "cmbCheckPrinting");
            this.cmbCheckPrinting.Name = "cmbCheckPrinting";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbPrinting
            // 
            this.cmbPrinting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinting.FormattingEnabled = true;
            this.cmbPrinting.Items.AddRange(new object[] {
            resources.GetString("cmbPrinting.Items"),
            resources.GetString("cmbPrinting.Items1"),
            resources.GetString("cmbPrinting.Items2")});
            resources.ApplyResources(this.cmbPrinting, "cmbPrinting");
            this.cmbPrinting.Name = "cmbPrinting";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbOutputLines
            // 
            this.cmbOutputLines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutputLines.FormattingEnabled = true;
            this.cmbOutputLines.Items.AddRange(new object[] {
            resources.GetString("cmbOutputLines.Items"),
            resources.GetString("cmbOutputLines.Items1")});
            resources.ApplyResources(this.cmbOutputLines, "cmbOutputLines");
            this.cmbOutputLines.Name = "cmbOutputLines";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // ntbPrintingPriority
            // 
            this.ntbPrintingPriority.AllowDecimal = false;
            this.ntbPrintingPriority.AllowNegative = false;
            this.ntbPrintingPriority.DecimalLetters = 2;
            resources.ApplyResources(this.ntbPrintingPriority, "ntbPrintingPriority");
            this.ntbPrintingPriority.MaxValue = 0D;
            this.ntbPrintingPriority.Name = "ntbPrintingPriority";
            this.ntbPrintingPriority.Value = 0D;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbStation3);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cmbStation2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbStation1);
            this.groupBox1.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbStation3
            // 
            resources.ApplyResources(this.cmbStation3, "cmbStation3");
            this.cmbStation3.Name = "cmbStation3";
            this.cmbStation3.SelectedData = null;
            this.cmbStation3.RequestData += new System.EventHandler(this.cmbStation_RequestData);
            this.cmbStation3.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbStation2
            // 
            resources.ApplyResources(this.cmbStation2, "cmbStation2");
            this.cmbStation2.Name = "cmbStation2";
            this.cmbStation2.SelectedData = null;
            this.cmbStation2.RequestData += new System.EventHandler(this.cmbStation_RequestData);
            this.cmbStation2.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbStation1
            // 
            resources.ApplyResources(this.cmbStation1, "cmbStation1");
            this.cmbStation1.Name = "cmbStation1";
            this.cmbStation1.SelectedData = null;
            this.cmbStation1.RequestData += new System.EventHandler(this.cmbStation_RequestData);
            this.cmbStation1.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // HospitalityRestaurantStationPrintingPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ntbPrintingPriority);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbOutputLines);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbPrinting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCheckPrinting);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalityRestaurantStationPrintingPage";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCheckPrinting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPrinting;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbOutputLines;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbPrintingPriority;
        private System.Windows.Forms.GroupBox groupBox1;
        private DualDataComboBox cmbStation3;
        private System.Windows.Forms.Label label8;
        private DualDataComboBox cmbStation2;
        private System.Windows.Forms.Label label7;
        private DualDataComboBox cmbStation1;
        private System.Windows.Forms.Label label6;
    }
}
