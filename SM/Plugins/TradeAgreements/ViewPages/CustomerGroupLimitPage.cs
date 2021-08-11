using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TradeAgreements.Dialogs;
using LSOne.ViewPlugins.TradeAgreements.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    public partial class CustomerGroupLimitPage : UserControl, ITabView
    {
        private RecordIdentifier customerId;
        WeakReference owner;

        public CustomerGroupLimitPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CustomerGroupLimitPage((TabControl)sender);
        }

        public CustomerGroupLimitPage()
        {
            InitializeComponent();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            customerId = context;
            var customersDefaultGroup = Providers.CustomerGroupData.GetDefaultCustomerGroup(PluginEntry.DataModel, customerId);
            if (customersDefaultGroup != null)
            {
                chkGroupUsesLimit.Checked = customersDefaultGroup.UsesDiscountedPurchaseLimit;

                switch (customersDefaultGroup.Period)
                {
                    case CustomerGroup.PeriodEnum.Day:
                        tbCurrentPeriod.Text = customersDefaultGroup.CurrentPeriodStart.Date.ToShortDateString();
                        break;
                    case CustomerGroup.PeriodEnum.Week:
                        var fromText = customersDefaultGroup.CurrentPeriodStart.Date.ToShortDateString();
                        var toText = customersDefaultGroup.CurrentPeriodEnd.Date.ToShortDateString();
                        tbCurrentPeriod.Text = fromText + " - " + toText;
                        break;
                    case CustomerGroup.PeriodEnum.Month:
                        tbCurrentPeriod.Text = customersDefaultGroup.CurrentPeriodStart.ToString("MMM", CultureInfo.InvariantCulture);
                        break;
                    case CustomerGroup.PeriodEnum.Year:
                        tbCurrentPeriod.Text = customersDefaultGroup.CurrentPeriodStart.Year.ToString(CultureInfo.InvariantCulture);
                        break;
                }

                var siteServiceService = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

                decimal maxDiscountedPurchases = 0;
                decimal currentPeriodDiscountedPurchases = 0;
                try
                {
                    siteServiceService.CustomersDiscountedPurchasesStatus(
                    PluginEntry.DataModel,
                    siteServiceProfile,
                    (string)customerId,
                    out maxDiscountedPurchases,
                    out currentPeriodDiscountedPurchases,
                    true);
                }
                catch (Exception ex)
                {
                    errorProvider1.SetError(tbCurrentPeriod, String.Format(Properties.Resources.ErrorConnectingToSiteService));
                    PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.ErrorConnectingToSiteService, ex);
                }

                ntbPurchaseLimit.SetValueWithLimit(
                    maxDiscountedPurchases,
                    PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                ntbDiscountedPurchases.SetValueWithLimit(
                    currentPeriodDiscountedPurchases,
                    PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
            }
            else
            {
                tbCurrentPeriod.Text = Properties.Resources.CustomerNotInCustomerGroup;                
            }
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeIdentifier != customerId) return;
            switch (objectName)
            {
                case "CustomerDefaultGroupChanged":
                    LoadData(false, changeIdentifier, param);
                    break;
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }
    }
}
