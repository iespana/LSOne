using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityTypeTipsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTypeTipsPage));
            this.label1 = new System.Windows.Forms.Label();
            this.tbTipsAmtLine1 = new System.Windows.Forms.TextBox();
            this.tbTipsAmtLine2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTipsTotalLine = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbTipsIncomeAcc1 = new DualDataComboBox();
            this.cmbTipsIncomeAcc2 = new DualDataComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbTipsAmtLine1
            // 
            resources.ApplyResources(this.tbTipsAmtLine1, "tbTipsAmtLine1");
            this.tbTipsAmtLine1.Name = "tbTipsAmtLine1";
            // 
            // tbTipsAmtLine2
            // 
            resources.ApplyResources(this.tbTipsAmtLine2, "tbTipsAmtLine2");
            this.tbTipsAmtLine2.Name = "tbTipsAmtLine2";
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
            // tbTipsTotalLine
            // 
            resources.ApplyResources(this.tbTipsTotalLine, "tbTipsTotalLine");
            this.tbTipsTotalLine.Name = "tbTipsTotalLine";
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
            // cmbTipsIncomeAcc1
            // 
            resources.ApplyResources(this.cmbTipsIncomeAcc1, "cmbTipsIncomeAcc1");
            this.cmbTipsIncomeAcc1.MaxLength = 32767;
            this.cmbTipsIncomeAcc1.Name = "cmbTipsIncomeAcc1";
            this.cmbTipsIncomeAcc1.SelectedData = null;
            this.cmbTipsIncomeAcc1.SkipIDColumn = true;
            this.cmbTipsIncomeAcc1.RequestData += new System.EventHandler(this.cmbTipsIncomeAcc1_RequestData);
            this.cmbTipsIncomeAcc1.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbTipsIncomeAcc2
            // 
            resources.ApplyResources(this.cmbTipsIncomeAcc2, "cmbTipsIncomeAcc2");
            this.cmbTipsIncomeAcc2.MaxLength = 32767;
            this.cmbTipsIncomeAcc2.Name = "cmbTipsIncomeAcc2";
            this.cmbTipsIncomeAcc2.SelectedData = null;
            this.cmbTipsIncomeAcc2.SkipIDColumn = true;
            this.cmbTipsIncomeAcc2.RequestData += new System.EventHandler(this.cmbTipsIncomeAcc2_RequestData);
            this.cmbTipsIncomeAcc2.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // HospitalityTypeTipsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbTipsIncomeAcc2);
            this.Controls.Add(this.cmbTipsIncomeAcc1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbTipsTotalLine);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbTipsAmtLine2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbTipsAmtLine1);
            this.Controls.Add(this.label1);
            this.Name = "HospitalityTypeTipsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTipsAmtLine1;
        private System.Windows.Forms.TextBox tbTipsAmtLine2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTipsTotalLine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbTipsIncomeAcc1;
        private DualDataComboBox cmbTipsIncomeAcc2;
    }
}
