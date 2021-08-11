using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class InventoryAdminPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryAdminPage));
            this.label1 = new System.Windows.Forms.Label();
            this.ntbMaxOverReceive = new LSOne.Controls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBlindReceivingPurchaseOrder = new System.Windows.Forms.CheckBox();
            this.lblCostCalculation = new System.Windows.Forms.Label();
            this.cmbCostCalculation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbMaxOverReceive
            // 
            this.ntbMaxOverReceive.AllowDecimal = true;
            this.ntbMaxOverReceive.AllowNegative = false;
            this.ntbMaxOverReceive.CultureInfo = null;
            this.ntbMaxOverReceive.DecimalLetters = 0;
            this.ntbMaxOverReceive.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxOverReceive.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxOverReceive, "ntbMaxOverReceive");
            this.ntbMaxOverReceive.MaxValue = 0D;
            this.ntbMaxOverReceive.MinValue = 0D;
            this.ntbMaxOverReceive.Name = "ntbMaxOverReceive";
            this.ntbMaxOverReceive.Value = 0D;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbBlindReceivingPurchaseOrder
            // 
            resources.ApplyResources(this.cbBlindReceivingPurchaseOrder, "cbBlindReceivingPurchaseOrder");
            this.cbBlindReceivingPurchaseOrder.Name = "cbBlindReceivingPurchaseOrder";
            this.cbBlindReceivingPurchaseOrder.UseVisualStyleBackColor = true;
            // 
            // lblCostCalculation
            // 
            this.lblCostCalculation.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCostCalculation, "lblCostCalculation");
            this.lblCostCalculation.Name = "lblCostCalculation";
            // 
            // cmbCostCalculation
            // 
            this.cmbCostCalculation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCostCalculation.FormattingEnabled = true;
            this.cmbCostCalculation.Items.AddRange(new object[] {
            resources.GetString("cmbCostCalculation.Items"),
            resources.GetString("cmbCostCalculation.Items1"),
            resources.GetString("cmbCostCalculation.Items2")});
            resources.ApplyResources(this.cmbCostCalculation, "cmbCostCalculation");
            this.cmbCostCalculation.Name = "cmbCostCalculation";
            // 
            // InventoryAdminPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbCostCalculation);
            this.Controls.Add(this.lblCostCalculation);
            this.Controls.Add(this.cbBlindReceivingPurchaseOrder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ntbMaxOverReceive);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "InventoryAdminPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbMaxOverReceive;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbBlindReceivingPurchaseOrder;
        private System.Windows.Forms.Label lblCostCalculation;
        private System.Windows.Forms.ComboBox cmbCostCalculation;
    }
}
