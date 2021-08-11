using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
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
    public partial class GoodsRecieveingLineDialogMulti : DialogBase
    {
        GoodsReceivingDocument goodsReceivingDocument;
        private DialogResult dialogResult = DialogResult.Cancel;
        List<GoodsReceivingDocumentLine> goodsReceivingDocumentLines;

        private SiteServiceProfile siteServiceProfile;
        IInventoryService service = null;

      

        public GoodsRecieveingLineDialogMulti(RecordIdentifier goodsRecievingDocumentID,
         List<GoodsReceivingDocumentLine> goodsReceivingDocumentLines)
         
        {
            try
            {
                InitializeComponent();

                siteServiceProfile = PluginOperations.GetSiteServiceProfile();

                service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);
                goodsReceivingDocument = service.GetGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, goodsRecievingDocumentID, true);

                this.goodsReceivingDocumentLines = goodsReceivingDocumentLines;

                ntbReceivedQuantity.Value = 0;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
}

       

        private void CheckEnabled(object sender, EventArgs e)
        {

            btnOK.Enabled = (chkEnableQuantity.Checked && !string.IsNullOrEmpty(ntbReceivedQuantity.Text))
                            || (chkEnableDate.Checked && chkEnableDate.Checked);
        }
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {

                DialogResult = DialogResult.OK;
                Close();

            }

        }

        private bool Save()
        {
            try
            {
                IConnectionManagerTemporary threadedConnection = PluginEntry.DataModel.CreateTemporaryConnection();

                // Prepare the progress dialog information
                using (ProgressDialog dlg = new ProgressDialog(Resources.SavingStockcountingLines, Resources.SavingCounter, goodsReceivingDocumentLines.Count))
                {
                    Action saveAction = () =>
                    {

                        decimal receivedQuantity = (decimal) ntbReceivedQuantity.Value;

                        DateTime receivedDate = dtpReceivedDate.Value;

                        int count = 1;
                        int totalCount = goodsReceivingDocumentLines.Count();

                        foreach (GoodsReceivingDocumentLine entity in goodsReceivingDocumentLines)
                        {
                            if (chkEnableQuantity.Checked)
                            {
                                entity.ReceivedQuantity = (decimal) ntbReceivedQuantity.Value;
                            }
                            if (chkEnableDate.Checked)
                            {
                                entity.ReceivedDate = dtpReceivedDate.Value;

                            }
                            service.SaveGoodsReceivingDocumentLine(PluginEntry.DataModel, siteServiceProfile, entity,
                                false);
                            dlg.Report(count, totalCount);

                            count++;

                        }
                        service.Disconnect(PluginEntry.DataModel);
                        threadedConnection.Close();

                    };
                    dlg.ProgressTask = Task.Run(saveAction);
                    dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit,
                        "GoodsReceivingDocumentLine",
                        goodsReceivingDocument.ID,
                        null);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = (dialogResult == DialogResult.OK)
                               ? DialogResult = DialogResult.OK
                               : DialogResult = DialogResult.Cancel;
            Close();
        }

        private void chkEnableDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpReceivedDate.Enabled = chkEnableDate.Checked;
            CheckEnabled(sender, e);
        }

        private void chkEnableQuantity_CheckedChanged(object sender, EventArgs e)
        {
            ntbReceivedQuantity.Enabled = chkEnableQuantity.Checked;
            CheckEnabled(sender,e);
        }

       
    }
}
