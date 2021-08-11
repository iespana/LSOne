using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityRetailItemPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityRetailItemPage));
            this.lblProductionTime = new System.Windows.Forms.Label();
            this.tbProductionTime = new LSOne.Controls.NumericTextBox();
            this.SuspendLayout();
            // 
            // lblProductionTime
            // 
            this.lblProductionTime.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblProductionTime, "lblProductionTime");
            this.lblProductionTime.Name = "lblProductionTime";
            // 
            // tbProductionTime
            // 
            this.tbProductionTime.AllowDecimal = false;
            this.tbProductionTime.AllowNegative = false;
            this.tbProductionTime.CultureInfo = null;
            this.tbProductionTime.DecimalLetters = 0;
            this.tbProductionTime.ForeColor = System.Drawing.Color.Black;
            this.tbProductionTime.HasMinValue = false;
            resources.ApplyResources(this.tbProductionTime, "tbProductionTime");
            this.tbProductionTime.MaxValue = 9999D;
            this.tbProductionTime.MinValue = 0D;
            this.tbProductionTime.Name = "tbProductionTime";
            this.tbProductionTime.Value = 0D;
            // 
            // HospitalityRetailItemPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbProductionTime);
            this.Controls.Add(this.lblProductionTime);
            this.DoubleBuffered = true;
            this.Name = "HospitalityRetailItemPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProductionTime;
        private NumericTextBox tbProductionTime;
    }
}
