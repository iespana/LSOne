using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    partial class HardwareProfileLineDisplayPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardwareProfileLineDisplayPage));
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDeviceName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTillClosedLine1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbTillClosedLine2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ntbCharset = new LSOne.Controls.NumericTextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDisplayTotalText = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbDisplayBalanceText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbDisplay = new System.Windows.Forms.ComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.chkLineDisplayBinaryConversion = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbDeviceName
            // 
            this.cmbDeviceName.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDeviceName, "cmbDeviceName");
            this.cmbDeviceName.Name = "cmbDeviceName";
            this.cmbDeviceName.Leave += new System.EventHandler(this.cmbDeviceName_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbTillClosedLine1
            // 
            resources.ApplyResources(this.tbTillClosedLine1, "tbTillClosedLine1");
            this.tbTillClosedLine1.Name = "tbTillClosedLine1";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbTillClosedLine2
            // 
            resources.ApplyResources(this.tbTillClosedLine2, "tbTillClosedLine2");
            this.tbTillClosedLine2.Name = "tbTillClosedLine2";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // ntbCharset
            // 
            this.ntbCharset.AllowDecimal = false;
            this.ntbCharset.AllowNegative = false;
            this.ntbCharset.CultureInfo = null;
            this.ntbCharset.DecimalLetters = 2;
            this.ntbCharset.HasMinValue = false;
            resources.ApplyResources(this.ntbCharset, "ntbCharset");
            this.ntbCharset.MaxValue = 9999D;
            this.ntbCharset.MinValue = 0D;
            this.ntbCharset.Name = "ntbCharset";
            this.ntbCharset.Value = 0D;
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
            // tbDisplayTotalText
            // 
            resources.ApplyResources(this.tbDisplayTotalText, "tbDisplayTotalText");
            this.tbDisplayTotalText.Name = "tbDisplayTotalText";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // tbDisplayBalanceText
            // 
            resources.ApplyResources(this.tbDisplayBalanceText, "tbDisplayBalanceText");
            this.tbDisplayBalanceText.Name = "tbDisplayBalanceText";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbDisplay
            // 
            this.cmbDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDisplay.FormattingEnabled = true;
            this.cmbDisplay.Items.AddRange(new object[] {
            resources.GetString("cmbDisplay.Items"),
            resources.GetString("cmbDisplay.Items1"),
            resources.GetString("cmbDisplay.Items2")});
            resources.ApplyResources(this.cmbDisplay, "cmbDisplay");
            this.cmbDisplay.Name = "cmbDisplay";
            this.cmbDisplay.SelectedIndexChanged += new System.EventHandler(this.cmbDisplay_SelectedIndexChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // chkLineDisplayBinaryConversion
            // 
            resources.ApplyResources(this.chkLineDisplayBinaryConversion, "chkLineDisplayBinaryConversion");
            this.chkLineDisplayBinaryConversion.Name = "chkLineDisplayBinaryConversion";
            this.chkLineDisplayBinaryConversion.UseVisualStyleBackColor = true;
            // 
            // HardwareProfileLineDisplayPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkLineDisplayBinaryConversion);
            this.Controls.Add(this.cmbDisplay);
            this.Controls.Add(this.tbDisplayBalanceText);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbDisplayTotalText);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ntbCharset);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbTillClosedLine2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbTillClosedLine1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDeviceName);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.Name = "HardwareProfileLineDisplayPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDeviceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTillClosedLine1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbTillClosedLine2;
        private System.Windows.Forms.Label label5;
        private NumericTextBox ntbCharset;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbDisplayTotalText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbDisplayBalanceText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbDisplay;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkLineDisplayBinaryConversion;


    }
}
