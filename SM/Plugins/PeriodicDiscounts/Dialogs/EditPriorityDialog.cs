using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.PeriodicDiscounts.Properties;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class EditPriorityDialog : DialogBase
    {
        private DiscountOffer offer;
        private RecordIdentifier offerID;
        private List<int> prioritiesInUse;
        private bool priorityChanged;

        public EditPriorityDialog(RecordIdentifier id)
        {
            InitializeComponent();

            offerID = id;

            prioritiesInUse = Providers.DiscountOfferData.GetPrioritiesInUse(PluginEntry.DataModel);
            offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.All);

            tbOfferNumber.Text = (string)offer.ID;
            tbDescription.Text = offer.Text;
            ntbPriority.Value = offer.Priority;
            cmbTriggering.SelectedIndex = (int)offer.Triggering;
            tbBarcode.Text = offer.BarCode ?? "";

            priorityChanged = false;
        }

        private void ntbPriority_TextChanged(object sender, EventArgs e)
        {
            priorityChanged = true;
            errorProvider.Clear();
            CheckEnabled();
        }

        private void cmbTriggering_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            CheckEnabled();
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = (int)ntbPriority.Value != offer.Priority ||
                            cmbTriggering.SelectedIndex != (int)offer.Triggering ||
                            tbBarcode.Text != offer.BarCode;

            lblBarcode.Enabled = tbBarcode.Enabled = cmbTriggering.SelectedIndex == (int)DiscountOffer.TriggeringEnum.Manual;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (prioritiesInUse.Contains((int)ntbPriority.Value) && priorityChanged)
            {
                errorProvider.SetError(ntbPriority, Properties.Resources.PriorityAlreadyInUse);
                return;
            }

            if (Providers.DiscountOfferData.BarcodeExists(PluginEntry.DataModel, tbBarcode.Text) && tbBarcode.Text != "" && offer.BarCode != tbBarcode.Text)
            {
                errorProvider.SetError(tbBarcode, Resources.BarcodeAlreadyAssignedToAnOffer);
                return;
            }

            offer.Priority = (int)ntbPriority.Value;
            offer.Triggering = (DiscountOffer.TriggeringEnum)cmbTriggering.SelectedIndex;
            offer.BarCode = tbBarcode.Text;
            Providers.DiscountOfferData.Save(PluginEntry.DataModel, offer);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PeriodicDiscount", offer.ID, offer.OfferType);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbBarcode_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            CheckEnabled();
        }
    }
}
