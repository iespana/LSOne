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
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.GiftCards.Dialogs
{
    public partial class NewGiftCardManualIDDialog : DialogBase
    {        
        WeakReference currencyAdder;        
        private SiteServiceProfile siteServiceProfile;

        public List<GiftCard> GiftCardsCreated;

        public NewGiftCardManualIDDialog(SiteServiceProfile siteServiceProfile)
        {
            IPlugin plugin;            

            this.siteServiceProfile = siteServiceProfile;
            this.GiftCardsCreated = new List<GiftCard>();

            InitializeComponent();            

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

            SetRefillableOption();            
        }                

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void SetRefillableOption()
        {
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
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GiftCard card;            
            errorProvider1.Clear();

            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);


            if (!string.IsNullOrEmpty(tbGiftCardNumber.Text) && service.GetGiftCard(PluginEntry.DataModel, siteServiceProfile, tbGiftCardNumber.Text, true) != null)
            {
                errorProvider1.SetError(tbGiftCardNumber, Resources.GiftCardAlreadyExists);
                tbGiftCardNumber.Focus();
                return;
            }            
            
            //Create a new gift card
            card = new GiftCard();
            card.ID = tbGiftCardNumber.Text;
            card.Active = false;
            card.Issued = true;
            card.Balance = (decimal)ntbAmount.Value;
            card.Currency = cmbCurrency.SelectedData.ID;
            card.Refillable = chkFillable.Checked;
            card.CreatedDate = Date.Now;

            try
            {
                card.ID = service.AddNewGiftCard(PluginEntry.DataModel, siteServiceProfile, card, true, "");                
            }
            catch (Exception ex)
            {
                MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                return;
            }

            GiftCardsCreated.Add(card);

            if (service != null)
            {
                service.Disconnect(PluginEntry.DataModel);
            }

            if (chkCreateAnother.Checked)
            {
                ntbAmount.Clear();
                tbGiftCardNumber.Text = "";
                tbGiftCardNumber.Focus();
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "GiftCard", RecordIdentifier.Empty, GiftCardsCreated);
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
         
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            btnOK.Enabled = cmbCurrency.SelectedData.ID != "";
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
    }
}
