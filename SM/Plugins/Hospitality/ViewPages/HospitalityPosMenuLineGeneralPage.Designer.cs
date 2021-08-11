using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityPosMenuLineGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityPosMenuLineGeneralPage));
            this.chkBackgroundHidden = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkTransparent = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ntbColumnSpan = new NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ntbRowSpan = new NumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkBackgroundHidden
            // 
            resources.ApplyResources(this.chkBackgroundHidden, "chkBackgroundHidden");
            this.chkBackgroundHidden.Name = "chkBackgroundHidden";
            this.chkBackgroundHidden.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkTransparent
            // 
            resources.ApplyResources(this.chkTransparent, "chkTransparent");
            this.chkTransparent.Name = "chkTransparent";
            this.chkTransparent.UseVisualStyleBackColor = true;
            this.chkTransparent.CheckedChanged += new System.EventHandler(this.chkTransparent_CheckedChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // ntbColumnSpan
            // 
            this.ntbColumnSpan.AllowDecimal = false;
            this.ntbColumnSpan.AllowNegative = false;
            this.ntbColumnSpan.DecimalLetters = 2;
            this.ntbColumnSpan.HasMinValue = false;
            resources.ApplyResources(this.ntbColumnSpan, "ntbColumnSpan");
            this.ntbColumnSpan.MaxValue = 0D;
            this.ntbColumnSpan.MinValue = 0D;
            this.ntbColumnSpan.Name = "ntbColumnSpan";
            this.ntbColumnSpan.Value = 1D;
            this.ntbColumnSpan.TextChanged += new System.EventHandler(this.ntbColumnSpan_TextChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // ntbRowSpan
            // 
            this.ntbRowSpan.AllowDecimal = false;
            this.ntbRowSpan.AllowNegative = false;
            this.ntbRowSpan.DecimalLetters = 2;
            this.ntbRowSpan.HasMinValue = false;
            resources.ApplyResources(this.ntbRowSpan, "ntbRowSpan");
            this.ntbRowSpan.MaxValue = 0D;
            this.ntbRowSpan.MinValue = 0D;
            this.ntbRowSpan.Name = "ntbRowSpan";
            this.ntbRowSpan.Value = 1D;
            this.ntbRowSpan.TextChanged += new System.EventHandler(this.ntbRowSpan_TextChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // HospitalityPosMenuLineGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbRowSpan);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ntbColumnSpan);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkTransparent);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkBackgroundHidden);
            this.Controls.Add(this.label4);
            this.DoubleBuffered = true;
            this.Name = "HospitalityPosMenuLineGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkBackgroundHidden;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkTransparent;
        private System.Windows.Forms.Label label5;
        private NumericTextBox ntbColumnSpan;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbRowSpan;
        private System.Windows.Forms.Label label7;

    }
}
