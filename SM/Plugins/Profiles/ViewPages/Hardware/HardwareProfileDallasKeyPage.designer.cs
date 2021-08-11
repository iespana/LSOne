using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    partial class HardwareProfileDallasKeyPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfileDallasKeyPage));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ntbDataBits = new LSOne.Controls.NumericTextBox();
            this.lblStopBits = new System.Windows.Forms.Label();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.lblPortName = new System.Windows.Forms.Label();
            this.lblParity = new System.Windows.Forms.Label();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.cmbStopBits = new System.Windows.Forms.ComboBox();
            this.ntbBaudRate = new LSOne.Controls.NumericTextBox();
            this.txtPortName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtKeyRemovedMessage = new System.Windows.Forms.TextBox();
            this.txtMessagePrefix = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDeviceConnected = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.ntbDataBits, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblStopBits, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblBaudRate, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblPortName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblParity, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblDataBits, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.cmbParity, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbStopBits, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.ntbBaudRate, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtPortName, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // ntbDataBits
            // 
            this.ntbDataBits.AllowDecimal = false;
            this.ntbDataBits.AllowNegative = false;
            this.ntbDataBits.CultureInfo = null;
            this.ntbDataBits.DecimalLetters = 2;
            this.ntbDataBits.HasMinValue = false;
            resources.ApplyResources(this.ntbDataBits, "ntbDataBits");
            this.ntbDataBits.MaxValue = 0D;
            this.ntbDataBits.MinValue = 0D;
            this.ntbDataBits.Name = "ntbDataBits";
            this.ntbDataBits.Value = 8D;
            // 
            // lblStopBits
            // 
            resources.ApplyResources(this.lblStopBits, "lblStopBits");
            this.lblStopBits.BackColor = System.Drawing.Color.Transparent;
            this.lblStopBits.Name = "lblStopBits";
            // 
            // lblBaudRate
            // 
            resources.ApplyResources(this.lblBaudRate, "lblBaudRate");
            this.lblBaudRate.BackColor = System.Drawing.Color.Transparent;
            this.lblBaudRate.Name = "lblBaudRate";
            // 
            // lblPortName
            // 
            resources.ApplyResources(this.lblPortName, "lblPortName");
            this.lblPortName.BackColor = System.Drawing.Color.Transparent;
            this.lblPortName.Name = "lblPortName";
            // 
            // lblParity
            // 
            resources.ApplyResources(this.lblParity, "lblParity");
            this.lblParity.BackColor = System.Drawing.Color.Transparent;
            this.lblParity.Name = "lblParity";
            // 
            // lblDataBits
            // 
            resources.ApplyResources(this.lblDataBits, "lblDataBits");
            this.lblDataBits.BackColor = System.Drawing.Color.Transparent;
            this.lblDataBits.Name = "lblDataBits";
            // 
            // cmbParity
            // 
            this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Items.AddRange(new object[] {
            resources.GetString("cmbParity.Items"),
            resources.GetString("cmbParity.Items1"),
            resources.GetString("cmbParity.Items2"),
            resources.GetString("cmbParity.Items3"),
            resources.GetString("cmbParity.Items4")});
            resources.ApplyResources(this.cmbParity, "cmbParity");
            this.cmbParity.Name = "cmbParity";
            // 
            // cmbStopBits
            // 
            this.cmbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStopBits.FormattingEnabled = true;
            resources.ApplyResources(this.cmbStopBits, "cmbStopBits");
            this.cmbStopBits.Name = "cmbStopBits";
            // 
            // ntbBaudRate
            // 
            this.ntbBaudRate.AllowDecimal = false;
            this.ntbBaudRate.AllowNegative = false;
            this.ntbBaudRate.CultureInfo = null;
            this.ntbBaudRate.DecimalLetters = 2;
            this.ntbBaudRate.HasMinValue = false;
            resources.ApplyResources(this.ntbBaudRate, "ntbBaudRate");
            this.ntbBaudRate.MaxValue = 0D;
            this.ntbBaudRate.MinValue = 0D;
            this.ntbBaudRate.Name = "ntbBaudRate";
            this.ntbBaudRate.Value = 9600D;
            // 
            // txtPortName
            // 
            resources.ApplyResources(this.txtPortName, "txtPortName");
            this.txtPortName.Name = "txtPortName";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.txtKeyRemovedMessage, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtMessagePrefix, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkDeviceConnected, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // txtKeyRemovedMessage
            // 
            resources.ApplyResources(this.txtKeyRemovedMessage, "txtKeyRemovedMessage");
            this.txtKeyRemovedMessage.Name = "txtKeyRemovedMessage";
            // 
            // txtMessagePrefix
            // 
            resources.ApplyResources(this.txtMessagePrefix, "txtMessagePrefix");
            this.txtMessagePrefix.Name = "txtMessagePrefix";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // chkDeviceConnected
            // 
            resources.ApplyResources(this.chkDeviceConnected, "chkDeviceConnected");
            this.chkDeviceConnected.Name = "chkDeviceConnected";
            this.chkDeviceConnected.UseVisualStyleBackColor = true;
            this.chkDeviceConnected.CheckedChanged += new System.EventHandler(this.chkDeviceConnected_CheckedChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // HardwareProfileDallasKeyPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "HardwareProfileDallasKeyPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.Label lblPortName;
        private System.Windows.Forms.CheckBox chkDeviceConnected;
        private System.Windows.Forms.Label lblStopBits;
        private System.Windows.Forms.Label lblParity;
        private System.Windows.Forms.Label lblDataBits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.ComboBox cmbStopBits;
        private NumericTextBox ntbDataBits;
        private NumericTextBox ntbBaudRate;
        private System.Windows.Forms.TextBox txtPortName;
        private System.Windows.Forms.TextBox txtKeyRemovedMessage;
        private System.Windows.Forms.TextBox txtMessagePrefix;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;


    }
}
