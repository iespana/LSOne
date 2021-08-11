using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityRestaurantStationMonitorPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityRestaurantStationMonitorPage));
            this.label1 = new System.Windows.Forms.Label();
            this.ntbEndTurnsRedAfterMin = new NumericTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbEndTurnsRedAfterMin
            // 
            this.ntbEndTurnsRedAfterMin.AllowDecimal = false;
            this.ntbEndTurnsRedAfterMin.AllowNegative = false;
            this.ntbEndTurnsRedAfterMin.DecimalLetters = 2;
            resources.ApplyResources(this.ntbEndTurnsRedAfterMin, "ntbEndTurnsRedAfterMin");
            this.ntbEndTurnsRedAfterMin.MaxValue = 0D;
            this.ntbEndTurnsRedAfterMin.Name = "ntbEndTurnsRedAfterMin";
            this.ntbEndTurnsRedAfterMin.Value = 0D;
            // 
            // HospitalityRestaurantStationMonitorPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbEndTurnsRedAfterMin);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalityRestaurantStationMonitorPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbEndTurnsRedAfterMin;
    }
}
