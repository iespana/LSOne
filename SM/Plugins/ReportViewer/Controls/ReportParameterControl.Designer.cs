using LSOne.Controls;

namespace LSOne.ViewPlugins.ReportViewer.Controls
{
    partial class ReportParameterControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportParameterControl));
            this.hostPanel = new DoubleBufferedPanel();
            this.btnViewReport = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hostPanel
            // 
            resources.ApplyResources(this.hostPanel, "hostPanel");
            this.hostPanel.BackColor = System.Drawing.Color.Transparent;
            this.hostPanel.Name = "hostPanel";
            // 
            // btnViewReport
            // 
            resources.ApplyResources(this.btnViewReport, "btnViewReport");
            this.btnViewReport.Name = "btnViewReport";
            this.btnViewReport.UseVisualStyleBackColor = true;
            this.btnViewReport.Click += new System.EventHandler(this.btnViewReport_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnViewReport, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.hostPanel, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ReportParameterControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReportParameterControl";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel hostPanel;
        private System.Windows.Forms.Button btnViewReport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
