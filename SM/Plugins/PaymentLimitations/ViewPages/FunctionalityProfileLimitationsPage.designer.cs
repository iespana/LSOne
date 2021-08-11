using LSOne.Controls;

namespace LSOne.ViewPlugins.PaymentLimitations.ViewPages
{
    partial class FunctionalityProfileLimitationsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalityProfileLimitationsPage));
            this.chkDisplayTotalsInPOS = new System.Windows.Forms.CheckBox();
            this.lblDisplayTotalsInPOS = new System.Windows.Forms.Label();
            this.lblDisplayItems = new System.Windows.Forms.Label();
            this.cmbDisplayItems = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // chkDisplayTotalsInPOS
            // 
            resources.ApplyResources(this.chkDisplayTotalsInPOS, "chkDisplayTotalsInPOS");
            this.chkDisplayTotalsInPOS.Name = "chkDisplayTotalsInPOS";
            this.chkDisplayTotalsInPOS.UseVisualStyleBackColor = true;
            // 
            // lblDisplayTotalsInPOS
            // 
            this.lblDisplayTotalsInPOS.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDisplayTotalsInPOS, "lblDisplayTotalsInPOS");
            this.lblDisplayTotalsInPOS.Name = "lblDisplayTotalsInPOS";
            // 
            // lblDisplayItems
            // 
            this.lblDisplayItems.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDisplayItems, "lblDisplayItems");
            this.lblDisplayItems.Name = "lblDisplayItems";
            // 
            // cmbDisplayItems
            // 
            this.cmbDisplayItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDisplayItems.FormattingEnabled = true;
            this.cmbDisplayItems.Items.AddRange(new object[] {
            resources.GetString("cmbDisplayItems.Items"),
            resources.GetString("cmbDisplayItems.Items1"),
            resources.GetString("cmbDisplayItems.Items2")});
            resources.ApplyResources(this.cmbDisplayItems, "cmbDisplayItems");
            this.cmbDisplayItems.Name = "cmbDisplayItems";
            // 
            // FunctionalityProfileLimitationsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cmbDisplayItems);
            this.Controls.Add(this.chkDisplayTotalsInPOS);
            this.Controls.Add(this.lblDisplayTotalsInPOS);
            this.Controls.Add(this.lblDisplayItems);
            this.DoubleBuffered = true;
            this.Name = "FunctionalityProfileLimitationsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDisplayTotalsInPOS;
        private System.Windows.Forms.Label lblDisplayTotalsInPOS;
        private System.Windows.Forms.Label lblDisplayItems;
        private System.Windows.Forms.ComboBox cmbDisplayItems;
    }
}
