using LSOne.Controls;
using LSOne.POS.Processes.WinControls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services
{
    partial class DualDisplayForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DualDisplayForm));
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.pnlImages = new LSOne.Controls.DoubleBufferedPanel();
            this.pnlReceipt = new LSOne.Controls.DoubleBufferedPanel();
            this.pnlTotals = new LSOne.Controls.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            resources.ApplyResources(this.webBrowser, "webBrowser");
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScrollBarsEnabled = false;
            // 
            // pnlImages
            // 
            resources.ApplyResources(this.pnlImages, "pnlImages");
            this.pnlImages.BackColor = System.Drawing.Color.White;
            this.pnlImages.Name = "pnlImages";
            this.pnlImages.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlImages_Paint);
            this.pnlImages.Resize += new System.EventHandler(this.doubleBufferedPanel1_Resize);
            // 
            // pnlReceipt
            // 
            resources.ApplyResources(this.pnlReceipt, "pnlReceipt");
            this.pnlReceipt.BackColor = System.Drawing.Color.White;
            this.pnlReceipt.Name = "pnlReceipt";
            // 
            // pnlTotals
            // 
            resources.ApplyResources(this.pnlTotals, "pnlTotals");
            this.pnlTotals.BackColor = System.Drawing.Color.White;
            this.pnlTotals.Name = "pnlTotals";
            // 
            // DualDisplayForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.pnlReceipt);
            this.Controls.Add(this.pnlImages);
            this.Controls.Add(this.webBrowser);
            this.Name = "DualDisplayForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private DoubleBufferedPanel pnlImages;
        private DoubleBufferedPanel pnlReceipt;
        private DoubleBufferedPanel pnlTotals;
    }
}