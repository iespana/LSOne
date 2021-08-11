using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;

namespace LSOne.ViewPlugins.PeriodicDiscounts
{
    public class PluginEntry : IPlugin, IPluginDashboardProvider
    {
        internal static Guid PeriodicDiscountDashboardItemID = new Guid("efa0b651-9fc6-466a-b85a-f8e7fb995d54");
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.PeriodicDiscounts; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "CanEditValidationPeriod":
                    return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageDiscounts);
            }

            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            try
            {
                switch (message)
                {
                    case "CanEditValidationPeriod":
                        PluginOperations.ShowSpecificValidationPeriod((RecordIdentifier)parameters);
                        break;
                }
            }
            catch (DataIntegrityException ex)
            {
                if (ex.EntityType == typeof(Dimension))
                {
                    MessageDialog.Show(Properties.Resources.VariationNotExistsError);
                }
                else if (ex.EntityType == typeof(RetailItem))
                {
                    MessageDialog.Show(Properties.Resources.RetailItemNotExistsError);
                }
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            //frameworkCallbacks.AddMenuConstructionConstructionHandler(new MenuConstructionEventHandler(PluginOperations.ConstructMenus));

            frameworkCallbacks.AddOperationCategoryConstructionHandler(new OperationCategoryEventHandler(PluginOperations.AddOperationCategoryHandler));
            frameworkCallbacks.AddOperationItemConstructionHandler(new OperationItemEventHandler(PluginOperations.AddOperationItemHandler));
            frameworkCallbacks.AddOperationButttonConstructionHandler(new OperationbuttonEventHandler(PluginOperations.AddOperationButtonHandler));

            frameworkCallbacks.AddRibbonPageConstructionHandler(new RibbonPageEventHandler(PluginOperations.AddRibbonPages));
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(new RibbonPageCategoryEventHandler(PluginOperations.AddRibbonPageCategories));
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(new RibbonPageCategoryItemEventHandler(PluginOperations.AddRibbonPageCategoryItems));

            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {    
            operations.AddOperation(Properties.Resources.Promotions, PluginOperations.ShowPromotionsView);
            operations.AddOperation(Properties.Resources.DiscountOffers, PluginOperations.ShowDiscountOffersView);
            operations.AddOperation(Properties.Resources.Multibuy, PluginOperations.ShowMultibuyView);
            operations.AddOperation(Properties.Resources.MixAndMatch, PluginOperations.ShowMixAndMatchView);
            operations.AddOperation(Properties.Resources.ValidationPeriods, PluginOperations.ShowDiscountPeriodsView);
            operations.AddOperation(Properties.Resources.PeriodicDiscountPriorities, PluginOperations.ShowPeriodicDiscountPrioritiesView);
        }

        

        #endregion

        private void PeriodicDiscountsManage(object sender, EventArgs args)
        {
            LSOne.ViewCore.Dialogs.DetailMultiChoiceDialog.DetailMultiChoiceDialogButton[] buttons = new DetailMultiChoiceDialog.DetailMultiChoiceDialogButton[] 
            {
                new DetailMultiChoiceDialog.DetailMultiChoiceDialogButton() {PrimaryText = Properties.Resources.MixAndMatch,SecondaryText = Properties.Resources.MixAndMatchDescription,OperationHandler = PluginOperations.ShowMixAndMatchView},
                new DetailMultiChoiceDialog.DetailMultiChoiceDialogButton() {PrimaryText = Properties.Resources.Multibuy,SecondaryText = Properties.Resources.MultibuyDescription, OperationHandler = PluginOperations.ShowMultibuyView},
                new DetailMultiChoiceDialog.DetailMultiChoiceDialogButton() {PrimaryText = Properties.Resources.DiscountOffer,SecondaryText = Properties.Resources.DiscountOffersDescription,OperationHandler = PluginOperations.ShowDiscountOffersView},
                new DetailMultiChoiceDialog.DetailMultiChoiceDialogButton() {PrimaryText = Properties.Resources.ValidationPeriods,SecondaryText = Properties.Resources.ValidationPeriodDescription,OperationHandler = PluginOperations.ShowDiscountPeriodsView}
            };

            LSOne.ViewCore.Dialogs.DetailMultiChoiceDialog.Show(Framework, Properties.Resources.PeriodicDiscounts, Properties.Resources.SelectOperation,  buttons);
        }

        public void LoadDashboardItem(IConnectionManager threadedEntry, DashboardItem item)
        {
            // In case if the plugin is registering more than one then we check which one it is though we will never get item from other plugin here.
            if (item.ID == PeriodicDiscountDashboardItemID)
            {
                IDiscountOfferData discountProvider = Providers.DiscountOfferData;

                int discountsExpiringThisWeek = discountProvider.GetNumberOfDiscountsExpiringOverTheNext7Days(threadedEntry);

                if (discountsExpiringThisWeek > 1)
                {
                    item.InformationText = Properties.Resources.XDiscountsAreExpiringThisWeek.Replace("#1", discountsExpiringThisWeek.ToString());
                }
                else if(discountsExpiringThisWeek == 1)
                {
                    item.InformationText = Properties.Resources.OneDiscountIsExpiringThisWeek;
                }
                else
                {
                    int activeDiscounts = discountProvider.GetNumberOfActiveDiscounts(threadedEntry);

                    if(activeDiscounts > 1)
                    {
                        item.InformationText = Properties.Resources.XDiscountsAreActive.Replace("#1", activeDiscounts.ToString());
                    }
                    else if(activeDiscounts == 1)
                    {
                        item.InformationText = Properties.Resources.OneDiscountsIsActive;
                    }
                    else
                    {
                        // We need to check if there are zero active discounts or if there are just simply no discounts configured
                        if(discountProvider.DiscountsAreConfigured(threadedEntry))
                        {
                            item.InformationText = Properties.Resources.NoDiscountsAreActive;
                        }
                        else
                        {
                            item.InformationText = Properties.Resources.NoDiscountsAreConfigured;
                        }
                    }
                }

                item.State = DashboardItem.ItemStateEnum.Info;

                item.SetButton(0, Properties.Resources.PeriodicDiscountsManage, PeriodicDiscountsManage);
            }
        }

        public void RegisterDashBoardItems(ViewCore.EventArguments.DashboardItemArguments args)
        {
            // Here we often would put a permission check but Manage users has no permission, and sometimes the Dashboard item will show new user and sometimes
            // manage, so we let it slide here and will handle it in LoadDashboardItem since this dashboard item will always be avalible in some form.

            DashboardItem periodicDiscountDashboardItem = new DashboardItem(PeriodicDiscountDashboardItemID, Properties.Resources.PeriodicDiscounts, false, 240); // 4 hour refresh interval

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageDiscounts))
            {
                args.Add(new DashboardItemPluginResolver(periodicDiscountDashboardItem, this), 140); // Priority 140
            }
        }
    }
}
