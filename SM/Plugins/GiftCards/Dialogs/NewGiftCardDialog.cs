using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.GiftCards.Properties;

namespace LSOne.ViewPlugins.GiftCards.Dialogs
{
    public partial class NewGiftCardDialog : DialogBase
    {
        RecordIdentifier vendorID;
        WeakReference currencyAdder;
        List<GiftCard> newGiftCards;
        private SiteServiceProfile siteServiceProfile;
        private RecordIdentifier giftCardSequenceID;
        private NumberSequence numberSequence;

        public NewGiftCardDialog(SiteServiceProfile siteServiceProfile)
        {
            IPlugin plugin;

            vendorID = RecordIdentifier.Empty;

            this.siteServiceProfile = siteServiceProfile;

            InitializeComponent();

            giftCardSequenceID = Providers.GiftCardData.SequenceID;

            numberSequence = Providers.NumberSequenceData.Get(PluginEntry.DataModel, giftCardSequenceID);

            cmbCurrency.SelectedData = new DataEntity("", "");

            plugin = PluginEntry.Framework.FindImplementor(this,"CanAddCurrency",null);

            if (plugin != null)
            {
                btnAddCurrency.Visible = true;

                currencyAdder = new WeakReference(plugin);
            }
            else
            {
                currencyAdder = null;
            }

            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                SiteServiceProfile profile = Providers.SiteServiceProfileData.GetStoreProfile(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                if (profile != null)
                {
                    chkFillable.Checked = profile.GiftCardRefillSetting == SiteServiceProfile.GiftCardRefillSettingEnum.AlwaysYes;
                }
                else
                {
                    chkFillable.Enabled = label4.Enabled = true;
                }
            }
            else
            {
                chkFillable.Enabled = label4.Enabled = true;
            }

            ntbSequenceStart.Value = numberSequence.NextRecord;
        }

        public List<GiftCard> NewGiftCards
        {
            get
            {
                return newGiftCards;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier VendorID
        {
            get { return vendorID; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GiftCard card;
            ISiteServiceService service = null;
            bool setSequeneStart = true;

            service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

            if ((int)ntbSequenceStart.Value < numberSequence.NextRecord && ntbSequenceStart.Text != "")
            {
                MessageDialog.Show(string.Format(Resources.CurrentSequenceHigher, numberSequence.NextRecord, ntbSequenceStart.Text) +"\n"+ Resources.SelectHigherSequenceStart, MessageBoxIcon.Error);
                return;
            }

            newGiftCards = new List<GiftCard>();

            for (int i = 0; i < (int)ntbCount.Value; i++)
            {
                card = new GiftCard();
                card.Active = false;
                card.Issued = true;
                card.Balance = (decimal)ntbAmount.Value;
                card.Currency = cmbCurrency.SelectedData.ID;
                card.Refillable = chkFillable.Checked;
                card.CreatedDate = Date.Now;

                    try
                    {
                        
                        card.ID = service.AddNewGiftCard(PluginEntry.DataModel, siteServiceProfile, card, true, tbPrefix.Text, ntbSequenceStart.Text != "" && setSequeneStart ? (int)ntbSequenceStart.Value : (int?)null);

                        setSequeneStart = false;
                        
                        if (card.ID == RecordIdentifier.Empty)
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                        return;
                    }

                newGiftCards.Add(card);
            }

            if (service != null)
            {
                service.Disconnect(PluginEntry.DataModel);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
         
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            btnOK.Enabled = cmbCurrency.SelectedData.ID != "" && ntbCount.Value > 0.0d;
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            ((IPlugin)currencyAdder.Target).Message(this, "AddCurrency", null);
        }

        private void cmbCurrency_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void btnChangeSequence_Click(object sender, EventArgs e)
        {
            if (MessageDialog.Show(Resources.ChangingNumberSequenceStartingPoint +" "+ Resources.StartingPointCanNotBeLower) == DialogResult.OK)
            {
                label6.Enabled = ntbSequenceStart.Enabled = true;
                btnChangeSequence.Enabled = false;
            }
        }
    }
}
