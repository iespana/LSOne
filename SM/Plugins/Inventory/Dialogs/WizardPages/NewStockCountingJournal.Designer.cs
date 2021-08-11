using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewStockCountingJournal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewStockCountingJournal));
            this.btnImportStockCounting = new LSOne.Controls.WizardOptionButton();
            this.btnFilter = new LSOne.Controls.WizardOptionButton();
            this.btnGenerateStockCounting = new LSOne.Controls.WizardOptionButton();
            this.btnNewStockCounting = new LSOne.Controls.WizardOptionButton();
            this.btnManageStockCounting = new LSOne.Controls.WizardOptionButton();
            this.btnViewStockCountingTemplates = new LSOne.Controls.WizardOptionButton();
            this.SuspendLayout();
            // 
            // btnImportStockCounting
            // 
            resources.ApplyResources(this.btnImportStockCounting, "btnImportStockCounting");
            this.btnImportStockCounting.Name = "btnImportStockCounting";
            this.btnImportStockCounting.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.ImportStockCountingFile;
            this.btnImportStockCounting.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.UseToImportStockCountingFile;
            this.btnImportStockCounting.Click += new System.EventHandler(this.btnImportStockCounting_Click);
            // 
            // btnFilter
            // 
            resources.ApplyResources(this.btnFilter, "btnFilter");
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.GenerateFromFilter;
            this.btnFilter.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.UseToGenerateFromFilter;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnGenerateStockCounting
            // 
            resources.ApplyResources(this.btnGenerateStockCounting, "btnGenerateStockCounting");
            this.btnGenerateStockCounting.Name = "btnGenerateStockCounting";
            this.btnGenerateStockCounting.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.GenerateFromExistingStockCountingJournal;
            this.btnGenerateStockCounting.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.UseToGenerateFromExistingStockCountingJournal;
            this.btnGenerateStockCounting.Click += new System.EventHandler(this.btnGenerateStockCounting_Click);
            // 
            // btnNewStockCounting
            // 
            resources.ApplyResources(this.btnNewStockCounting, "btnNewStockCounting");
            this.btnNewStockCounting.Name = "btnNewStockCounting";
            this.btnNewStockCounting.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.NewStockCountingJournal;
            this.btnNewStockCounting.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.UseToCreateNewStockCountingJournal;
            this.btnNewStockCounting.Click += new System.EventHandler(this.btnNewStockCounting_Click);
            // 
            // btnManageStockCounting
            // 
            resources.ApplyResources(this.btnManageStockCounting, "btnManageStockCounting");
            this.btnManageStockCounting.Name = "btnManageStockCounting";
            this.btnManageStockCounting.PrimaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.ManageStockCountingJournals;
            this.btnManageStockCounting.SecondaryText = global::LSOne.ViewPlugins.Inventory.Properties.Resources.UseToManageStockCountingJournals;
            this.btnManageStockCounting.Click += new System.EventHandler(this.btnManageStockCounting_Click);
            // 
            // btnViewStockCountingTemplates
            // 
            resources.ApplyResources(this.btnViewStockCountingTemplates, "btnViewStockCountingTemplates");
            this.btnViewStockCountingTemplates.Name = "btnViewStockCountingTemplates";
            this.btnViewStockCountingTemplates.Click += new System.EventHandler(this.btnViewStockCountingTemplates_Click);
            // 
            // NewStockCountingJournal
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.btnViewStockCountingTemplates);
            this.Controls.Add(this.btnImportStockCounting);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnGenerateStockCounting);
            this.Controls.Add(this.btnNewStockCounting);
            this.Controls.Add(this.btnManageStockCounting);
            this.Name = "NewStockCountingJournal";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.WizardOptionButton btnManageStockCounting;
        private Controls.WizardOptionButton btnNewStockCounting;
        private Controls.WizardOptionButton btnGenerateStockCounting;
        private Controls.WizardOptionButton btnFilter;
        private Controls.WizardOptionButton btnImportStockCounting;
        private Controls.WizardOptionButton btnViewStockCountingTemplates;
    }
}
