namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class StoreTransferRequestWizardButtons
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTransferRequestWizardButtons));
            this.btnManageTransferRequest = new LSOne.Controls.WizardOptionButton();
            this.btnNewTransferRequest = new LSOne.Controls.WizardOptionButton();
            this.btnGenerateFromRequest = new LSOne.Controls.WizardOptionButton();
            this.btnGenerateFromOrder = new LSOne.Controls.WizardOptionButton();
            this.btnGenerateFromFilter = new LSOne.Controls.WizardOptionButton();
            this.btnViewStoreTransferTemplates = new LSOne.Controls.WizardOptionButton();
            this.SuspendLayout();
            // 
            // btnManageTransferRequest
            // 
            resources.ApplyResources(this.btnManageTransferRequest, "btnManageTransferRequest");
            this.btnManageTransferRequest.Name = "btnManageTransferRequest";
            this.btnManageTransferRequest.Click += new System.EventHandler(this.btnManageTransferRequest_Click);
            // 
            // btnNewTransferRequest
            // 
            resources.ApplyResources(this.btnNewTransferRequest, "btnNewTransferRequest");
            this.btnNewTransferRequest.Name = "btnNewTransferRequest";
            this.btnNewTransferRequest.Click += new System.EventHandler(this.btnNewTransferRequest_Click);
            // 
            // btnGenerateFromRequest
            // 
            resources.ApplyResources(this.btnGenerateFromRequest, "btnGenerateFromRequest");
            this.btnGenerateFromRequest.Name = "btnGenerateFromRequest";
            this.btnGenerateFromRequest.Click += new System.EventHandler(this.btnGenerateFromRequest_Click);
            // 
            // btnGenerateFromOrder
            // 
            resources.ApplyResources(this.btnGenerateFromOrder, "btnGenerateFromOrder");
            this.btnGenerateFromOrder.Name = "btnGenerateFromOrder";
            this.btnGenerateFromOrder.Click += new System.EventHandler(this.btnGenerateFromOrder_Click);
            // 
            // btnGenerateFromFilter
            // 
            resources.ApplyResources(this.btnGenerateFromFilter, "btnGenerateFromFilter");
            this.btnGenerateFromFilter.Name = "btnGenerateFromFilter";
            this.btnGenerateFromFilter.Click += new System.EventHandler(this.btnGenerateFromFilter_Click);
            // 
            // btnViewStoreTransferTemplates
            // 
            resources.ApplyResources(this.btnViewStoreTransferTemplates, "btnViewStoreTransferTemplates");
            this.btnViewStoreTransferTemplates.Name = "btnViewStoreTransferTemplates";
            this.btnViewStoreTransferTemplates.Click += new System.EventHandler(this.btnViewStoreTransferTemplates_Click);
            // 
            // StoreTransferRequestWizardButtons
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.btnViewStoreTransferTemplates);
            this.Controls.Add(this.btnGenerateFromFilter);
            this.Controls.Add(this.btnGenerateFromOrder);
            this.Controls.Add(this.btnGenerateFromRequest);
            this.Controls.Add(this.btnNewTransferRequest);
            this.Controls.Add(this.btnManageTransferRequest);
            this.Name = "StoreTransferRequestWizardButtons";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.WizardOptionButton btnManageTransferRequest;
        private Controls.WizardOptionButton btnNewTransferRequest;
        private Controls.WizardOptionButton btnGenerateFromRequest;
        private Controls.WizardOptionButton btnGenerateFromOrder;
        private Controls.WizardOptionButton btnGenerateFromFilter;
        private Controls.WizardOptionButton btnViewStoreTransferTemplates;
    }
}
