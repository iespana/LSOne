using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Store.Properties;

namespace LSOne.ViewPlugins.Store.Views
{
    public partial class StoreTenderView : ViewBase
    {
        RecordIdentifier storeAndPaymentMethodID;
        StorePaymentMethod paymentMethod;
        string storeDescription;
        private TabControl.Tab generalSettingsTab;
        private TabControl.Tab usageSettingsTab;
        private TabControl.Tab overunderTenderTab;
        private TabControl.Tab storeTenderCardTypesTab;

        public StoreTenderView(RecordIdentifier storeAndPaymentMethodID,string storeDescription)
        {
            InitializeComponent();

            this.storeAndPaymentMethodID = storeAndPaymentMethodID;
            this.storeDescription = storeDescription;

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;



            this.ReadOnly = !(PluginEntry.DataModel.HasPermission(Permission.StoreEdit) && (PluginEntry.DataModel.HasPermission(Permission.ManageAllowedPaymentSettings)) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == storeAndPaymentMethodID.PrimaryID));
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("StorePaymentType", storeAndPaymentMethodID, Resources.StoreText, true));
            contexts.Add(new AuditDescriptor("StorePaymentTypeCards", storeAndPaymentMethodID, Resources.AllowedCardTypes));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.PaymentType;
            }
        }

        public string Description
        {
            get
            {
                return Resources.PaymentType + ": " + paymentMethod.Text;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return storeAndPaymentMethodID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            paymentMethod = Providers.StorePaymentMethodData.Get(PluginEntry.DataModel, storeAndPaymentMethodID);            

            if (!isRevert)
            {
                generalSettingsTab = new TabControl.Tab(Resources.Settings, ViewPages.StoreTenderGeneralPage.CreateInstance);
                usageSettingsTab = new TabControl.Tab(Resources.Usage, ViewPages.StoreTenderUsagePage.CreateInstance);
                overunderTenderTab = new TabControl.Tab(Resources.OverUnderTender, ViewPages.StoreTenderOverUndertenderPage.CreateInstance);
                storeTenderCardTypesTab = new TabControl.Tab(Resources.AllowedCardTypes, ViewPages.StoreTenderCardTypesPage.CreateInstance);

                tabSheetTabs.AddTab(generalSettingsTab);
                tabSheetTabs.AddTab(usageSettingsTab);
                tabSheetTabs.AddTab(overunderTenderTab);
                tabSheetTabs.AddTab(storeTenderCardTypesTab);

                AddParentViewDescriptor(new ParentViewDescriptor(
                        paymentMethod.StoreID,
                        Resources.StoreText + ": " + (string)storeAndPaymentMethodID + " - " + storeDescription,
                        Resources.store_16,
                        PluginOperations.ShowStore));

                
                PluginEntry.Framework.FindImplementor(this, "CanEditPaymentTypes", null);


                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, new RecordIdentifier(paymentMethod.StoreID), paymentMethod);

            }


            tbMethod.Text = paymentMethod.Text;

            tbStore.Text = storeAndPaymentMethodID + " - " + storeDescription;
           

            HeaderText = Description;
            //HeaderIcon = Resources.Coins;

            tabSheetTabs.SetData(isRevert, storeAndPaymentMethodID, paymentMethod);

        }

        protected override bool DataIsModified()
        {
          
            if (tabSheetTabs.IsModified()) return true;

            if (tbMethod.Text != paymentMethod.Text)
                return true;
            return false;
        }

        protected override bool SaveData()
        {
            paymentMethod.Text = tbMethod.Text;

            tabSheetTabs.GetData();

            Providers.StorePaymentMethodData.Save(PluginEntry.DataModel, paymentMethod, storeAndPaymentMethodID.SecondaryID);

            storeAndPaymentMethodID.SecondaryID = paymentMethod.PaymentTypeID;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "StorePaymentMethod", paymentMethod.StoreAndTenderTypeID, null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Store":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == storeAndPaymentMethodID.PrimaryID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;

                case "StorePaymentMethod":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == storeAndPaymentMethodID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }

                    break;

            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }


        protected override void OnDelete()
        {
            PluginOperations.DeleteStorePaymentMethod(storeAndPaymentMethodID);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
