using LSOne.ViewPlugins.ReportViewer.Controls;

namespace LSOne.ViewPlugins.ReportViewer.Views
{
    partial class ReportViewerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportViewerView));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.printTimer = new System.Windows.Forms.Timer(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.reportParameterControl1 = new ReportParameterControl();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.reportViewer1);
            this.pnlBottom.Controls.Add(this.reportParameterControl1);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // printTimer
            // 
            this.printTimer.Tick += new System.EventHandler(this.printTimer_Tick);
            // 
            // reportViewer1
            // 
            resources.ApplyResources(this.reportViewer1, "reportViewer1");
            this.reportViewer1.Name = "reportViewer1";
            // 
            // reportParameterControl1
            // 
            this.reportParameterControl1.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.reportParameterControl1, "reportParameterControl1");
            this.reportParameterControl1.Name = "reportParameterControl1";
            this.reportParameterControl1.ViewReport += new System.EventHandler(this.reportParameterControl1_ViewReport);
            // 
            // ReportViewerView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ReportViewerView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer printTimer;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private ReportParameterControl reportParameterControl1;


    }
}
