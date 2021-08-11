namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class NewEmptySCJ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewEmptySCJ));
            this.label4 = new System.Windows.Forms.Label();
            this.tbStockCountingJournalDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbStockCountingJournalDescription
            // 
            resources.ApplyResources(this.tbStockCountingJournalDescription, "tbStockCountingJournalDescription");
            this.tbStockCountingJournalDescription.Name = "tbStockCountingJournalDescription";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.ReceiveKeyboardEvents = true;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.cmbStore_SelectedDataChanged);
            // 
            // NewEmptySCJ
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.tbStockCountingJournalDescription);
            this.Controls.Add(this.label1);
            resources.ApplyResources(this, "$this");
            this.Name = "NewEmptySCJ";
            this.Load += new System.EventHandler(this.NewEmptySCJ_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private Controls.DualDataComboBox cmbStore;
        private System.Windows.Forms.TextBox tbStockCountingJournalDescription;
        private System.Windows.Forms.Label label1;
    }
}
