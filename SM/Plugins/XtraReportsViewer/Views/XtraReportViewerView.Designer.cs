using LSOne.ViewPlugins.XtraReportsViewer.Controls;

namespace LSOne.ViewPlugins.XtraReportsViewer.Views
{
    partial class XtraReportViewerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraReportViewerView));
            this.reportViewerControl1 = new ReportViewerControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.reportViewerControl1);
            // 
            // reportViewerControl1
            // 
            resources.ApplyResources(this.reportViewerControl1, "reportViewerControl1");
            this.reportViewerControl1.Name = "reportViewerControl1";
            this.reportViewerControl1.Report = null;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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

        private ReportViewerControl reportViewerControl1;
        private System.Windows.Forms.Timer timer1;


    }
}
