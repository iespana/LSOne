using System;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Store.Dialogs
{
  public partial class CardTypeDialog : DialogBase
    {
        RecordIdentifier storeAndTenderType;
        RecordIdentifier cardTypeID;
        StoreCardType cardType;

        private TabControl.Tab generalTab;

        public CardTypeDialog(RecordIdentifier storeAndTenderType, RecordIdentifier cardType)
            : this()
        {
            this.storeAndTenderType = storeAndTenderType;
            this.cardTypeID = cardType;
        }

        public CardTypeDialog()
        {
            this.storeAndTenderType = RecordIdentifier.Empty;
            this.cardTypeID = RecordIdentifier.Empty;

            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (cardTypeID != RecordIdentifier.Empty)
            {
                cardType = Providers.PaymentTypeCardTypesData.GetCardForTenderType(
                    PluginEntry.DataModel,
                    storeAndTenderType.PrimaryID,
                    storeAndTenderType.SecondaryID,
                    cardTypeID);
                
                cmbCardType.LoadData(Providers.PaymentTypeCardTypesData.GetUnusedCardTypesForTender(
                        PluginEntry.DataModel,
                        storeAndTenderType.PrimaryID,
                        storeAndTenderType.SecondaryID,
                        cardType.CardTypeID), 
                    cardType.CardTypeID);
            }
            else
            {
                cardType = new StoreCardType();
                cardType.StoreID = storeAndTenderType.PrimaryID;
                cardType.TenderTypeID = storeAndTenderType.SecondaryID;
                
                cmbCardType.LoadData(Providers.PaymentTypeCardTypesData.GetUnusedCardTypesForTender(
                        PluginEntry.DataModel,
                        storeAndTenderType.PrimaryID,
                        storeAndTenderType.SecondaryID,
                        RecordIdentifier.Empty).Cast<IDataEntity>(),
                    RecordIdentifier.Empty);
            }

            // Tabs that will add themseleves onto the tab control will need the full context:
            // card type, store and tender type
            RecordIdentifier contextID = new RecordIdentifier(cardTypeID, storeAndTenderType);

            // Initialize tab pages
            // Do any possible re-load on rever logic here.
            generalTab = new TabControl.Tab(Properties.Resources.General, ViewPages.CardTypeGeneralPage.CreateInstance,true);
          
            tabSheetTabs.AddTab(generalTab);

            // Allow other plugins to extend this tab panel
            tabSheetTabs.Broadcast(this, contextID, cardType);

            // Load all tab pages, since we might be editing a new card type
            tabSheetTabs.LoadAllTabs();
            
            // Set tab data
            tabSheetTabs.SetData(false, contextID, cardType);

            tabSheetTabs.DialogEnabledChanged += new EventHandler(CheckChanged);
            CheckChanged(this, EventArgs.Empty);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Avoid the Microsoft memory leak error on ListViews
            tabSheetTabs.SendCloseMessage();
        }

       
        private void cmbCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckChanged(this, EventArgs.Empty);
            cardType.CardTypeID = cmbCardType.SelectedID;
            tabSheetTabs.BroadcastChangeInformation(DataEntityChangeType.Edit, "CardTypeSelected", cmbCardType.SelectedID, cmbCardType.SelectedID);

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            tabSheetTabs.IsModified();
            cardType.CardTypeID = cmbCardType.SelectedID;
            cardType.Description = cmbCardType.SelectedItem.ToString();
            tabSheetTabs.GetData();

            Providers.PaymentTypeCardTypesData.Save(PluginEntry.DataModel, cardType);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            if (cmbCardType.SelectedIndex >= 0)
            {
                btnOK.Enabled = tabSheetTabs.AllRequiredFieldsAreValid().Result == ViewCore.EventArguments.FieldValidationArguments.FieldValidationEnum.Valid || cmbCardType.SelectedID != cardType.CardTypeID;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

       
    }
}
