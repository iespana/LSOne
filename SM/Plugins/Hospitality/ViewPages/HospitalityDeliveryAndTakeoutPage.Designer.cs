using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityDeliveryAndTakeoutPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityDeliveryAndTakeoutPage));
            this.lblOrdersInProg = new System.Windows.Forms.Label();
            this.txtEstimProdTime = new System.Windows.Forms.TextBox();
            this.lblEstimProdTime = new System.Windows.Forms.Label();
            this.txtAvgProdTiming = new System.Windows.Forms.TextBox();
            this.lblAvgProdTiming = new System.Windows.Forms.Label();
            this.txtAvgDeliveryTiming = new System.Windows.Forms.TextBox();
            this.lblAvgDeliveryTiming = new System.Windows.Forms.Label();
            this.numOrdersInProg = new NumericTextBox();
            this.SuspendLayout();
            // 
            // lblOrdersInProg
            // 
            resources.ApplyResources(this.lblOrdersInProg, "lblOrdersInProg");
            this.lblOrdersInProg.Name = "lblOrdersInProg";
            // 
            // txtEstimProdTime
            // 
            resources.ApplyResources(this.txtEstimProdTime, "txtEstimProdTime");
            this.txtEstimProdTime.Name = "txtEstimProdTime";
            // 
            // lblEstimProdTime
            // 
            resources.ApplyResources(this.lblEstimProdTime, "lblEstimProdTime");
            this.lblEstimProdTime.Name = "lblEstimProdTime";
            // 
            // txtAvgProdTiming
            // 
            resources.ApplyResources(this.txtAvgProdTiming, "txtAvgProdTiming");
            this.txtAvgProdTiming.Name = "txtAvgProdTiming";
            // 
            // lblAvgProdTiming
            // 
            resources.ApplyResources(this.lblAvgProdTiming, "lblAvgProdTiming");
            this.lblAvgProdTiming.Name = "lblAvgProdTiming";
            // 
            // txtAvgDeliveryTiming
            // 
            resources.ApplyResources(this.txtAvgDeliveryTiming, "txtAvgDeliveryTiming");
            this.txtAvgDeliveryTiming.Name = "txtAvgDeliveryTiming";
            // 
            // lblAvgDeliveryTiming
            // 
            resources.ApplyResources(this.lblAvgDeliveryTiming, "lblAvgDeliveryTiming");
            this.lblAvgDeliveryTiming.Name = "lblAvgDeliveryTiming";
            // 
            // numOrdersInProg
            // 
            this.numOrdersInProg.AllowDecimal = false;
            this.numOrdersInProg.AllowNegative = false;
            this.numOrdersInProg.DecimalLetters = 2;
            resources.ApplyResources(this.numOrdersInProg, "numOrdersInProg");
            this.numOrdersInProg.MaxValue = 0D;
            this.numOrdersInProg.Name = "numOrdersInProg";
            this.numOrdersInProg.ReadOnly = true;
            this.numOrdersInProg.Value = 0D;
            // 
            // HospitalityDeliveryAndTakeoutPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.numOrdersInProg);
            this.Controls.Add(this.txtAvgDeliveryTiming);
            this.Controls.Add(this.lblAvgDeliveryTiming);
            this.Controls.Add(this.txtAvgProdTiming);
            this.Controls.Add(this.lblAvgProdTiming);
            this.Controls.Add(this.txtEstimProdTime);
            this.Controls.Add(this.lblEstimProdTime);
            this.Controls.Add(this.lblOrdersInProg);
            this.DoubleBuffered = true;
            this.Name = "HospitalityDeliveryAndTakeoutPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrdersInProg;
        private System.Windows.Forms.TextBox txtEstimProdTime;
        private System.Windows.Forms.Label lblEstimProdTime;
        private System.Windows.Forms.TextBox txtAvgProdTiming;
        private System.Windows.Forms.Label lblAvgProdTiming;
        private System.Windows.Forms.TextBox txtAvgDeliveryTiming;
        private System.Windows.Forms.Label lblAvgDeliveryTiming;
        private NumericTextBox numOrdersInProg;

    }
}
