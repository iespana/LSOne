using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class StockCountingLineDialogMulti : DialogBase
    {
        InventoryAdjustment stockCounting;
        private DialogResult dialogResult = DialogResult.Cancel;
        List<InventoryJournalTransaction> inventoryJournalTransactions;
        
        IInventoryService service = null;
        public AdjustmentStatus JournalStatus { get; private set; }

        public StockCountingLineDialogMulti(RecordIdentifier stockCountingID, List<InventoryJournalTransaction> inventoryJournalTransactions)
        {
            InitializeComponent();

            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            stockCounting = service.GetInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), stockCountingID, true);

            this.inventoryJournalTransactions = inventoryJournalTransactions;
            cmbUnit.SelectedData = new DataEntity("", "");

            ntbQty.Value = 0;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = (chkEnableCounted.Checked && !string.IsNullOrEmpty(ntbQty.Text)) ||
                            (chkEnableUnit.Checked && cmbUnit.SelectedDataID != "");
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            (bool CanEdit, AdjustmentStatus Status) processingStatus = PluginOperations.CheckJournalProcessingStatus(stockCounting.ID);
            JournalStatus = processingStatus.Status;

            if (processingStatus.CanEdit)
            { 
                if (Save())
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "StockCountLine", stockCounting.ID, null);
                }
            }
            else
            {
                DialogResult = DialogResult.Abort;
                Close();
            }
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            cmbUnit.SetData(Providers.UnitData.GetAllUnits(PluginEntry.DataModel), null);
        }

        private bool Save()
        {
            var shouldUpdateCounted = chkEnableCounted.Checked;
            var shouldUpdateUnitID = chkEnableUnit.Checked;
            var counted = (decimal)ntbQty.Value;
            var unitID = cmbUnit.SelectedDataID == "" ? RecordIdentifier.Empty : ((DataEntity)cmbUnit.SelectedData).ID;

            PluginOperations.SaveJournalTransactions(inventoryJournalTransactions, counted, unitID, shouldUpdateCounted, shouldUpdateUnitID);

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = (dialogResult == DialogResult.OK)
                               ? DialogResult = DialogResult.OK
                               : DialogResult = DialogResult.Cancel;
            Close();
        }

        private void chkEnableUnit_CheckedChanged(object sender, EventArgs e)
        {
            cmbUnit.Enabled = chkEnableUnit.Checked;
            CheckEnabled(sender, e);
        }

        private void chkEnableCounted_CheckedChanged(object sender, EventArgs e)
        {
            ntbQty.Enabled = chkEnableCounted.Checked;
            CheckEnabled(sender,e);
        }

        private void cmbUnit_SelectedDataCleared(object sender, EventArgs e)
        {
            if (!ntbQty.Enabled)
            {
                btnOK.Enabled = ntbQty.Enabled && !string.IsNullOrEmpty(ntbQty.Text);
            }
        }
    }
}