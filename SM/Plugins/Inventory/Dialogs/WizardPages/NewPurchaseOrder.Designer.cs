using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewPurchaseOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewPurchaseOrder));
            this.btnViewPurchaseOrderTemplates = new LSOne.Controls.WizardOptionButton();
            this.btnFilter = new LSOne.Controls.WizardOptionButton();
            this.btnWorksheets = new LSOne.Controls.WizardOptionButton();
            this.btnExistingPO = new LSOne.Controls.WizardOptionButton();
            this.btnEmptyPO = new LSOne.Controls.WizardOptionButton();
            this.btnManagePO = new LSOne.Controls.WizardOptionButton();
            this.SuspendLayout();
            // 
            // btnViewPurchaseOrderTemplates
            // 
            resources.ApplyResources(this.btnViewPurchaseOrderTemplates, "btnViewPurchaseOrderTemplates");
            this.btnViewPurchaseOrderTemplates.Name = "btnViewPurchaseOrderTemplates";
            this.btnViewPurchaseOrderTemplates.Click += new System.EventHandler(this.btnViewPurchaseOrderTemplates_Click);
            // 
            // btnFilter
            // 
            resources.ApplyResources(this.btnFilter, "btnFilter");
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.GenerateFromFilter;
            this.btnFilter.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.GeneratePOFromFilter;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnWorksheets
            // 
            resources.ApplyResources(this.btnWorksheets, "btnWorksheets");
            this.btnWorksheets.Name = "btnWorksheets";
            this.btnWorksheets.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.ViewPurchaseWorksheets;
            this.btnWorksheets.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.GenerateMultiplePOFromPurchaseWorksheet;
            this.btnWorksheets.Click += new System.EventHandler(this.btnWorksheets_Click);
            // 
            // btnExistingPO
            // 
            resources.ApplyResources(this.btnExistingPO, "btnExistingPO");
            this.btnExistingPO.Name = "btnExistingPO";
            this.btnExistingPO.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.GenerateFromAnExistingPurchaseOrder;
            this.btnExistingPO.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.GeneratePurchaseOrderWorksheetFromPrevious;
            this.btnExistingPO.Click += new System.EventHandler(this.btnExistingPO_Click);
            // 
            // btnEmptyPO
            // 
            resources.ApplyResources(this.btnEmptyPO, "btnEmptyPO");
            this.btnEmptyPO.Name = "btnEmptyPO";
            this.btnEmptyPO.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.NewPurchaseOrder;
            this.btnEmptyPO.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.CreateNewPurchaseOrder;
            this.btnEmptyPO.Click += new System.EventHandler(this.btnEmptyPO_Click);
            // 
            // btnManagePO
            // 
            resources.ApplyResources(this.btnManagePO, "btnManagePO");
            this.btnManagePO.Name = "btnManagePO";
            this.btnManagePO.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.ManagePurchaseOrders;
            this.btnManagePO.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.UseThisToWiewExistingPurchaseOrders;
            this.btnManagePO.Click += new System.EventHandler(this.btnManagePO_Click);
            // 
            // NewPurchaseOrder
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.btnViewPurchaseOrderTemplates);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnWorksheets);
            this.Controls.Add(this.btnExistingPO);
            this.Controls.Add(this.btnEmptyPO);
            this.Controls.Add(this.btnManagePO);
            this.Name = "NewPurchaseOrder";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.WizardOptionButton btnManagePO;
        private Controls.WizardOptionButton btnEmptyPO;
        private Controls.WizardOptionButton btnExistingPO;
        private Controls.WizardOptionButton btnWorksheets;
        private Controls.WizardOptionButton btnFilter;
        private Controls.WizardOptionButton btnViewPurchaseOrderTemplates;
    }
}
