using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItemAssemblies
{
    internal class PluginOperations
    {
        private static SiteServiceProfile siteServiceProfile;

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.RetailItems.Dialogs.NewRetailItemDialog")
            {
                args.Add(new TabControl.Tab(Properties.Resources.Assemblies, new PanelFactoryHandler(DialogPages.NewRetailItemAssembliesPage.CreateInstance), true, true), 110);
            }

            if ((args.ContextName == "LSOne.ViewPlugins.RetailItems.Views.ItemView") && ((SimpleRetailItem)args.InternalContext).ItemType == ItemTypeEnum.AssemblyItem)
            {
                args.Add(new TabControl.Tab(Properties.Resources.Assemblies, ItemTabKey.Assemblies.ToString(), new PanelFactoryHandler(ViewPages.RetailItemAssembliesPage.CreateInstance)), 240);
            }
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            
        }

        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            
        }

        internal static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments args)
        {
            
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            
        }

        internal static List<RetailItemAssembly> SortList(List<RetailItemAssembly> retailItemAssemblies, RetailItemAssemblySort sort, bool ascending)
        {
            switch (sort)
            {
                case RetailItemAssemblySort.Description:
                default:
                    return ascending ? retailItemAssemblies.OrderBy(x => x.Text).ToList() : retailItemAssemblies.OrderByDescending(x => x.Text).ToList();
                case RetailItemAssemblySort.Store:
                    return ascending ? retailItemAssemblies.OrderBy(x => x.StoreName).ToList() : retailItemAssemblies.OrderByDescending(x => x.StoreName).ToList();
                case RetailItemAssemblySort.StartingDate:
                    return ascending ? retailItemAssemblies.OrderBy(x => x.StartingDate).ToList() : retailItemAssemblies.OrderByDescending(x => x.StartingDate).ToList();
                case RetailItemAssemblySort.Cost:
                    return ascending ? retailItemAssemblies.OrderBy(x => x.TotalCost).ToList() : retailItemAssemblies.OrderByDescending(x => x.TotalCost).ToList();
                case RetailItemAssemblySort.Price:
                    return ascending ? retailItemAssemblies.OrderBy(x => x.GetDisplayPrice()).ToList() : retailItemAssemblies.OrderByDescending(x => x.GetDisplayPrice()).ToList();
                case RetailItemAssemblySort.Margin:
                    return ascending ? retailItemAssemblies.OrderBy(x => x.Margin).ToList() : retailItemAssemblies.OrderByDescending(x => x.Margin).ToList();
                case RetailItemAssemblySort.DisplayWithComponents:
                    return ascending ? retailItemAssemblies.OrderBy(x => x.GetDisplayWithComponentsString()).ToList() : retailItemAssemblies.OrderByDescending(x => x.GetDisplayWithComponentsString()).ToList();
                case RetailItemAssemblySort.SendComponentsToKds:
                    return ascending 
                        ? retailItemAssemblies.OrderBy(x => GetSendToKdsDisplayName(x.SendAssemblyComponentsToKds)).ToList() 
                        : retailItemAssemblies.OrderByDescending(x => GetSendToKdsDisplayName(x.SendAssemblyComponentsToKds)).ToList();
                case RetailItemAssemblySort.CalculatePrice:
                    return ascending ? retailItemAssemblies.OrderBy(x => x.CalculatePriceFromComponents).ToList() : retailItemAssemblies.OrderByDescending(x => x.CalculatePriceFromComponents).ToList();

            }
        }

        internal static List<RetailItemAssemblyComponent> SortList(List<RetailItemAssemblyComponent> retailItemAssemblyComponents, RetailItemAssemblyComponentSort sort, bool ascending)
        {
            switch (sort)
            {
                case RetailItemAssemblyComponentSort.ItemID:
                    return ascending ? retailItemAssemblyComponents.OrderBy(x => x.ItemID).ToList() : retailItemAssemblyComponents.OrderByDescending(x => x.ItemID).ToList();
                case RetailItemAssemblyComponentSort.ItemName:
                default:
                    return ascending ? retailItemAssemblyComponents.OrderBy(x => x.ItemName).ToList() : retailItemAssemblyComponents.OrderByDescending(x => x.ItemName).ToList();
                case RetailItemAssemblyComponentSort.VariantName:
                    return ascending ? retailItemAssemblyComponents.OrderBy(x => x.VariantName).ToList() : retailItemAssemblyComponents.OrderByDescending(x => x.VariantName).ToList();
                case RetailItemAssemblyComponentSort.Quantity:
                    return ascending ? retailItemAssemblyComponents.OrderBy(x => x.Quantity).ToList() : retailItemAssemblyComponents.OrderByDescending(x => x.Quantity).ToList();
                case RetailItemAssemblyComponentSort.Unit:
                    return ascending ? retailItemAssemblyComponents.OrderBy(x => x.UnitName).ToList() : retailItemAssemblyComponents.OrderByDescending(x => x.UnitName).ToList();
                case RetailItemAssemblyComponentSort.CostPerUnit:
                    return ascending ? retailItemAssemblyComponents.OrderBy(x => x.CostPerUnit).ToList() : retailItemAssemblyComponents.OrderByDescending(x => x.CostPerUnit).ToList();
                case RetailItemAssemblyComponentSort.TotalCost:
                    return ascending ? retailItemAssemblyComponents.OrderBy(x => x.GetTotalCost()).ToList() : retailItemAssemblyComponents.OrderByDescending(x => x.GetTotalCost()).ToList();
            }
        }

        internal static decimal CalculateProfitMargin(decimal price, decimal cost)
        {
            return (price > 0 && cost > 0) ? ((price - cost) / price) * 100 : 0;
        }

        public static bool TestSiteService(IConnectionManager entry)
        {
            bool serviceActive = false;
            SiteServiceProfile ssProfile = GetSiteServiceProfile(entry);
            bool serviceConfigured = ssProfile != null;
            if (serviceConfigured)
            {
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                ConnectionEnum result = service.TestConnection(entry, ssProfile.SiteServiceAddress, (ushort)ssProfile.SiteServicePortNumber);
                PluginEntry.Framework.ViewController.CurrentView.HideProgress();

                serviceActive = result == ConnectionEnum.Success;
            }

            return serviceActive && serviceConfigured;
        }

        public static SiteServiceProfile GetSiteServiceProfile(IConnectionManager entry)
        {
            if (siteServiceProfile == null)
            {
                Parameters parameters = Providers.ParameterData.Get(entry);
                if (parameters != null)
                {
                    siteServiceProfile = Providers.SiteServiceProfileData.Get(entry, parameters.SiteServiceProfile);
                }
            }

            return siteServiceProfile;
        }

        public static string GetSendToKdsDisplayName(KitchenDisplayAssemblyComponentType sendToKdsSetting)
        {
            switch (sendToKdsSetting)
            {
                case KitchenDisplayAssemblyComponentType.DontSend:
                    return Properties.Resources.DontSend;

                case KitchenDisplayAssemblyComponentType.SendAsItemModifiers:
                    return Properties.Resources.AsItemModifiers;

                case KitchenDisplayAssemblyComponentType.SendAsSeparateItems:
                    return Properties.Resources.AsSeparateItems;

                default:
                    throw new ArgumentException();
            }
        }

    }
}
