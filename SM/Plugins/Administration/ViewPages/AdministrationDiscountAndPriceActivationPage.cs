using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    public partial class AdministrationDiscountAndPriceActivationPage : ContainerControl, ITabViewV2
    {
        private DiscountAndPriceActivation discountAndPriceActivation;

        private bool ActiveDiscDirty;

        public AdministrationDiscountAndPriceActivationPage()
        {
            InitializeComponent();

            GetDataFromDatabase();
        }

        private void GetDataFromDatabase()
        {
            discountAndPriceActivation = Providers.DiscountAndPriceActivationData.Get(PluginEntry.DataModel);
            if (discountAndPriceActivation == null)
            {
                discountAndPriceActivation = new DiscountAndPriceActivation();
                ActiveDiscDirty = true;
            }
        }

        private void PopulateControls()
        {
            chkPriceCustomerItem.Checked = discountAndPriceActivation.PriceCustomerItem;
            chkPriceCustomerGroupItem.Checked = discountAndPriceActivation.PriceCustomerGroupItem;
            chkPriceAllCustomersItem.Checked = discountAndPriceActivation.PriceAllCustomersItem;

            chkLineDiscountCustomerItem.Checked = discountAndPriceActivation.LineDiscountCustomerItem;
            chkLineDiscountCustomerGroupItem.Checked = discountAndPriceActivation.LineDiscountCustomerGroupItem;
            chkLineDiscountAllCustomersItem.Checked = discountAndPriceActivation.LineDiscountAllCustomersItem;
            chkLineDiscountCustomerItemGroup.Checked = discountAndPriceActivation.LineDiscountCustomerItemGroup;
            chkLineDiscountCustomerGroupItemGroup.Checked =
                discountAndPriceActivation.LineDiscountCustomerGroupItemGroup;
            chkLineDiscountAllCustomersItemGroup.Checked = discountAndPriceActivation.LineDiscountAllCustomersItemGroup;
            chkLineDiscountCustomerAllItems.Checked = discountAndPriceActivation.LineDiscountCustomerAllItems;
            chkLineDiscountCustomerGroupAllItems.Checked = discountAndPriceActivation.LineDiscountCustomerGroupAllItems;

            chkMultilineDiscountCustomerItemGroup.Checked =
                discountAndPriceActivation.MultilineDiscountCustomerItemGroup;
            chkMultilineDiscountCustomerGroupItemGroup.Checked =
                discountAndPriceActivation.MultilineDiscountCustomerGroupItemGroup;
            chkMultilineDiscountAllCustomersItemGroup.Checked =
                discountAndPriceActivation.MultilineDiscountAllCustomersItemGroup;
            chkMultilineDiscountCustomerAllItems.Checked =
                discountAndPriceActivation.MultilineDiscountCustomerAllItems;
            chkMultilineDiscountCustomerGroupAllItems.Checked =
                discountAndPriceActivation.MultilineDiscountCustomerGroupAllItems;

            chkTotalDiscountCustomerAllItems.Checked = discountAndPriceActivation.TotalDiscountCustomerAllItems;
            chkTotalDiscountCustomerGroupAllItems.Checked =
                discountAndPriceActivation.TotalDiscountCustomerGroupAllItems;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.AdministrationDiscountAndPriceActivationPage();
        }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }
        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            PopulateControls();
        }

        private bool ActivePriceDiscIsDirty()
        {
            if (chkPriceCustomerItem.Checked != discountAndPriceActivation.PriceCustomerItem) return true;
            if (chkPriceCustomerGroupItem.Checked != discountAndPriceActivation.PriceCustomerGroupItem) return true;
            if (chkPriceAllCustomersItem.Checked != discountAndPriceActivation.PriceAllCustomersItem) return true;

            if (chkLineDiscountCustomerItem.Checked != discountAndPriceActivation.LineDiscountCustomerItem) return true;
            if (chkLineDiscountCustomerGroupItem.Checked != discountAndPriceActivation.LineDiscountCustomerGroupItem) return true;
            if (chkLineDiscountAllCustomersItem.Checked != discountAndPriceActivation.LineDiscountAllCustomersItem) return true;
            if (chkLineDiscountCustomerItemGroup.Checked != discountAndPriceActivation.LineDiscountCustomerItemGroup) return true;
            if (chkLineDiscountCustomerGroupItemGroup.Checked != discountAndPriceActivation.LineDiscountCustomerGroupItemGroup) return true;
            if (chkLineDiscountAllCustomersItemGroup.Checked != discountAndPriceActivation.LineDiscountAllCustomersItemGroup) return true;
            if (chkLineDiscountCustomerAllItems.Checked != discountAndPriceActivation.LineDiscountCustomerAllItems) return true;
            if (chkLineDiscountCustomerGroupAllItems.Checked != discountAndPriceActivation.LineDiscountCustomerGroupAllItems) return true;

            if (chkMultilineDiscountCustomerItemGroup.Checked != discountAndPriceActivation.MultilineDiscountCustomerItemGroup) return true;
            if (chkMultilineDiscountCustomerGroupItemGroup.Checked != discountAndPriceActivation.MultilineDiscountCustomerGroupItemGroup) return true;
            if (chkMultilineDiscountAllCustomersItemGroup.Checked != discountAndPriceActivation.MultilineDiscountAllCustomersItemGroup) return true;
            if (chkMultilineDiscountCustomerAllItems.Checked != discountAndPriceActivation.MultilineDiscountCustomerGroupAllItems) return true;
            if (chkMultilineDiscountCustomerGroupAllItems.Checked != discountAndPriceActivation.MultilineDiscountCustomerGroupAllItems) return true;

            if (chkTotalDiscountCustomerAllItems.Checked != discountAndPriceActivation.TotalDiscountCustomerAllItems) return true;
            if (chkTotalDiscountCustomerGroupAllItems.Checked != discountAndPriceActivation.TotalDiscountCustomerGroupAllItems) return true;

            return false;
        }

        public bool DataIsModified()
        {

            ActiveDiscDirty = ActiveDiscDirty || ActivePriceDiscIsDirty();

            return ActiveDiscDirty;
        }

        public bool SaveData()
        {
            if (ActiveDiscDirty)
            {
                discountAndPriceActivation.PriceCustomerItem = chkPriceCustomerItem.Checked;
                discountAndPriceActivation.PriceCustomerGroupItem = chkPriceCustomerGroupItem.Checked;
                discountAndPriceActivation.PriceAllCustomersItem = chkPriceAllCustomersItem.Checked;

                discountAndPriceActivation.LineDiscountCustomerItem = chkLineDiscountCustomerItem.Checked;
                discountAndPriceActivation.LineDiscountCustomerGroupItem = chkLineDiscountCustomerGroupItem.Checked;
                discountAndPriceActivation.LineDiscountAllCustomersItem = chkLineDiscountAllCustomersItem.Checked;
                discountAndPriceActivation.LineDiscountCustomerItemGroup = chkLineDiscountCustomerItemGroup.Checked;
                discountAndPriceActivation.LineDiscountCustomerGroupItemGroup = chkLineDiscountCustomerGroupItemGroup.Checked;
                discountAndPriceActivation.LineDiscountAllCustomersItemGroup = chkLineDiscountAllCustomersItemGroup.Checked;
                discountAndPriceActivation.LineDiscountCustomerAllItems = chkLineDiscountCustomerAllItems.Checked;
                discountAndPriceActivation.LineDiscountCustomerGroupAllItems = chkLineDiscountCustomerGroupAllItems.Checked;

                discountAndPriceActivation.MultilineDiscountCustomerItemGroup =
                    chkMultilineDiscountCustomerItemGroup.Checked;
                discountAndPriceActivation.MultilineDiscountCustomerGroupItemGroup =
                    chkMultilineDiscountCustomerGroupItemGroup.Checked;
                discountAndPriceActivation.MultilineDiscountAllCustomersItemGroup =
                    chkMultilineDiscountAllCustomersItemGroup.Checked;
                discountAndPriceActivation.MultilineDiscountCustomerAllItems =
                    chkMultilineDiscountCustomerAllItems.Checked;
                discountAndPriceActivation.MultilineDiscountCustomerGroupAllItems =
                    chkMultilineDiscountCustomerGroupAllItems.Checked;

                discountAndPriceActivation.TotalDiscountCustomerAllItems = chkTotalDiscountCustomerAllItems.Checked;
                discountAndPriceActivation.TotalDiscountCustomerGroupAllItems =
                    chkTotalDiscountCustomerGroupAllItems.Checked;

                Providers.DiscountAndPriceActivationData.Save(PluginEntry.DataModel, discountAndPriceActivation);
            }

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("AdministrationDiscountAndPriceActivationPage", 1, Properties.Resources.ActivePriceDiscount, true));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
           
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            GetDataFromDatabase();
            PopulateControls();
        }

        public void OnClose()
        {
            
        }

        public void SaveUserInterface()
        {
        }

        #endregion

    }
}
