namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewInventoryJournal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewInventoryJournal));
            this.btnParked = new LSOne.Controls.WizardOptionButton();
            this.btnStockReservation = new LSOne.Controls.WizardOptionButton();
            this.btnInvAdjustment = new LSOne.Controls.WizardOptionButton();
            this.SuspendLayout();
            // 
            // btnParked
            // 
            resources.ApplyResources(this.btnParked, "btnParked");
            this.btnParked.Name = "btnParked";
            this.btnParked.Click += new System.EventHandler(this.btnParked_Click);
            // 
            // btnStockReservation
            // 
            resources.ApplyResources(this.btnStockReservation, "btnStockReservation");
            this.btnStockReservation.Name = "btnStockReservation";
            this.btnStockReservation.Click += new System.EventHandler(this.btnStockReservation_Click);
            // 
            // btnInvAdjustment
            // 
            resources.ApplyResources(this.btnInvAdjustment, "btnInvAdjustment");
            this.btnInvAdjustment.Name = "btnInvAdjustment";
            this.btnInvAdjustment.Click += new System.EventHandler(this.btnInvAdjustment_Click);
            // 
            // NewInventoryJournal
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnParked);
            this.Controls.Add(this.btnStockReservation);
            this.Controls.Add(this.btnInvAdjustment);
            this.Name = "NewInventoryJournal";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.WizardOptionButton btnParked;
        private Controls.WizardOptionButton btnStockReservation;
        private Controls.WizardOptionButton btnInvAdjustment;
    }
}
