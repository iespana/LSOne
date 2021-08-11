using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    partial class HardwareProfileDualDisplayPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfileDualDisplayPage));
            this.chkDeviceConnected = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.screenFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbScreens = new System.Windows.Forms.ComboBox();
            this.lnkIdentify = new System.Windows.Forms.LinkLabel();
            this.percFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.ntbReceiptWidth = new LSOne.Controls.NumericTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnShowWebPage = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbWebPage = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnPathSelect = new System.Windows.Forms.Button();
            this.cmbAdvertType = new System.Windows.Forms.ComboBox();
            this.ntbInterval = new LSOne.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbImageRotatorPath = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.screenFlowLayout.SuspendLayout();
            this.percFlowLayout.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDeviceConnected
            // 
            resources.ApplyResources(this.chkDeviceConnected, "chkDeviceConnected");
            this.chkDeviceConnected.Name = "chkDeviceConnected";
            this.chkDeviceConnected.UseVisualStyleBackColor = true;
            this.chkDeviceConnected.CheckedChanged += new System.EventHandler(this.chkDeviceConnected_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.screenFlowLayout, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.percFlowLayout, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkDeviceConnected, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbDescription, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // screenFlowLayout
            // 
            resources.ApplyResources(this.screenFlowLayout, "screenFlowLayout");
            this.screenFlowLayout.Controls.Add(this.cmbScreens);
            this.screenFlowLayout.Controls.Add(this.lnkIdentify);
            this.screenFlowLayout.Name = "screenFlowLayout";
            // 
            // cmbScreens
            // 
            this.cmbScreens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScreens.FormattingEnabled = true;
            this.cmbScreens.Items.AddRange(new object[] {
            resources.GetString("cmbScreens.Items"),
            resources.GetString("cmbScreens.Items1"),
            resources.GetString("cmbScreens.Items2"),
            resources.GetString("cmbScreens.Items3")});
            resources.ApplyResources(this.cmbScreens, "cmbScreens");
            this.cmbScreens.Name = "cmbScreens";
            // 
            // lnkIdentify
            // 
            resources.ApplyResources(this.lnkIdentify, "lnkIdentify");
            this.lnkIdentify.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkIdentify.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
            this.lnkIdentify.Name = "lnkIdentify";
            this.lnkIdentify.TabStop = true;
            this.lnkIdentify.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnIdentifyScreensClicked);
            // 
            // percFlowLayout
            // 
            resources.ApplyResources(this.percFlowLayout, "percFlowLayout");
            this.percFlowLayout.Controls.Add(this.ntbReceiptWidth);
            this.percFlowLayout.Controls.Add(this.label8);
            this.percFlowLayout.Name = "percFlowLayout";
            // 
            // ntbReceiptWidth
            // 
            this.ntbReceiptWidth.AllowDecimal = true;
            this.ntbReceiptWidth.AllowNegative = false;
            this.ntbReceiptWidth.CultureInfo = null;
            this.ntbReceiptWidth.DecimalLetters = 2;
            this.ntbReceiptWidth.HasMinValue = false;
            resources.ApplyResources(this.ntbReceiptWidth, "ntbReceiptWidth");
            this.ntbReceiptWidth.MaxValue = 100D;
            this.ntbReceiptWidth.MinValue = 0D;
            this.ntbReceiptWidth.Name = "ntbReceiptWidth";
            this.ntbReceiptWidth.Value = 0D;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Name = "label11";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.btnShowWebPage, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbWebPage, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnPathSelect, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbAdvertType, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ntbInterval, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbImageRotatorPath, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // btnShowWebPage
            // 
            resources.ApplyResources(this.btnShowWebPage, "btnShowWebPage");
            this.btnShowWebPage.Image = global::LSOne.ViewPlugins.Profiles.Properties.Resources.BrowseWebImage;
            this.btnShowWebPage.Name = "btnShowWebPage";
            this.btnShowWebPage.UseVisualStyleBackColor = true;
            this.btnShowWebPage.Click += new System.EventHandler(this.btnShowWebPage_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // tbWebPage
            // 
            resources.ApplyResources(this.tbWebPage, "tbWebPage");
            this.tbWebPage.Name = "tbWebPage";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Name = "label10";
            // 
            // btnPathSelect
            // 
            resources.ApplyResources(this.btnPathSelect, "btnPathSelect");
            this.btnPathSelect.Image = global::LSOne.ViewPlugins.Profiles.Properties.Resources.FolderImage;
            this.btnPathSelect.Name = "btnPathSelect";
            this.btnPathSelect.UseVisualStyleBackColor = true;
            this.btnPathSelect.Click += new System.EventHandler(this.btnPathSelect_Click);
            // 
            // cmbAdvertType
            // 
            this.cmbAdvertType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAdvertType.FormattingEnabled = true;
            this.cmbAdvertType.Items.AddRange(new object[] {
            resources.GetString("cmbAdvertType.Items"),
            resources.GetString("cmbAdvertType.Items1"),
            resources.GetString("cmbAdvertType.Items2"),
            resources.GetString("cmbAdvertType.Items3")});
            resources.ApplyResources(this.cmbAdvertType, "cmbAdvertType");
            this.cmbAdvertType.Name = "cmbAdvertType";
            this.cmbAdvertType.SelectedIndexChanged += new System.EventHandler(this.cmbAdvertType_SelectedIndexChanged);
            // 
            // ntbInterval
            // 
            this.ntbInterval.AllowDecimal = false;
            this.ntbInterval.AllowNegative = false;
            this.ntbInterval.CultureInfo = null;
            this.ntbInterval.DecimalLetters = 2;
            this.ntbInterval.HasMinValue = false;
            resources.ApplyResources(this.ntbInterval, "ntbInterval");
            this.ntbInterval.MaxValue = 99999D;
            this.ntbInterval.MinValue = 0D;
            this.ntbInterval.Name = "ntbInterval";
            this.ntbInterval.Value = 0D;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Name = "label9";
            // 
            // tbImageRotatorPath
            // 
            resources.ApplyResources(this.tbImageRotatorPath, "tbImageRotatorPath");
            this.tbImageRotatorPath.Name = "tbImageRotatorPath";
            // 
            // HardwareProfileDualDisplayPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "HardwareProfileDualDisplayPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.screenFlowLayout.ResumeLayout(false);
            this.screenFlowLayout.PerformLayout();
            this.percFlowLayout.ResumeLayout(false);
            this.percFlowLayout.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDeviceConnected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbImageRotatorPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbAdvertType;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbReceiptWidth;
        private System.Windows.Forms.Label label8;
        private NumericTextBox ntbInterval;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbWebPage;
        private System.Windows.Forms.Button btnPathSelect;
        private System.Windows.Forms.Button btnShowWebPage;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbScreens;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel percFlowLayout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel screenFlowLayout;
        private System.Windows.Forms.LinkLabel lnkIdentify;


    }
}
