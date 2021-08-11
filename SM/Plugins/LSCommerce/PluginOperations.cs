using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.LSCommerce.Properties;
using LSOne.ViewPlugins.LSCommerce.Views;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.TouchButtons;

namespace LSOne.ViewPlugins.LSCommerce
{
    internal class PluginOperations
    {
        private static SiteServiceProfile siteServiceProfile;

        public static bool TestSiteService(bool displayMsg = true)
        {
            return TestSiteService(PluginEntry.DataModel, displayMsg);
        }

        public static bool TestSiteService(IConnectionManager entry, bool displayMsg = true)
        {
            bool serviceConfigured = false;
            bool serviceActivce = false;
            bool serviceValid;

            SiteServiceProfile siteServiceProfile = GetSiteServiceProfile(entry);
            serviceConfigured = siteServiceProfile != null;

            if (siteServiceProfile != null)
            {
                ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                ConnectionEnum result = service.TestConnection(entry,
                    siteServiceProfile.SiteServiceAddress,
                    (ushort)siteServiceProfile.SiteServicePortNumber);

                PluginEntry.Framework.ViewController.CurrentView.HideProgress();

                if (result != ConnectionEnum.Success)
                {
                    if (displayMsg)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                    }
                }
                else
                {
                    serviceActivce = true;
                }
            }
            else
            {
                if (displayMsg)
                {
                    MessageDialog.Show(Resources.NoStoreServerIsSetUp);
                }
            }

            serviceValid = serviceActivce && serviceConfigured;
            if (!serviceValid && displayMsg)
            {
                IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanViewAdministrationTab", null);

                if (plugin != null)
                {
                    plugin.Message(null, "ViewSiteServiceTab", null);
                }
            }
            return serviceValid;
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Terminals.Views.TerminalView")
            {
                args.Add(new TabControl.Tab(Resources.LSCommerceLicense, ViewPages.SettingsPage.CreateInstance), 600);
            }

            else if (args.ContextName == "LSOne.ViewPlugins.Profiles.Views.FunctionalityProfileView" && PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
            {
                args.Add(new TabControl.Tab(Resources.LSCommerce, ViewPages.FunctionalityProfilePage.CreateInstance), 200);
            }
            else if(args.ContextName == "LSOne.ViewPlugins.Replenishment.Views.InventoryTemplateView")
            {
                args.Add(new TabControl.Tab(Resources.LSCommerceGeneral, ViewPages.InventoryTemplateGeneralPage.CreateInstance), 10);
            }
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sites")
            {
                args.Add(new PageCategory(Resources.LSCommerce, "LSCommerce"), 700);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sites" && args.CategoryKey == "LSCommerce")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.StoreView))
                {

                    args.Add(
                        new CategoryItem(
                            Resources.LSCommerceLicenses,
                            Resources.LSCommerceLicenses,
                            Resources.ManageLSCommerceLicenses,
                            CategoryItem.CategoryItemFlags.Button,
                            Resources.LSCommerce_licenses_16,
                            ShowLSCommerceLicenses,
                            "CompanyInfo"), 50);
                }
                if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout))
                {
                    args.Add(
                    new CategoryItem(
                        Resources.LSCommerceInventoryButtonMenus,
                        Resources.LSCommerceInventoryButtonMenus,
                        Resources.ManageLSCommerceInventoryButtonMenus,
                        CategoryItem.CategoryItemFlags.Button,
                        Resources.POS_button_grid_menu_16,
                        ShowMobileInventoryMenu,
                        "MobileInventoryMenu"), 100);
                }
            }
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Store setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
                {
                    args.Add(new Item(Resources.LSCommerceLicenses, "LS Commerce licenses", null), 600);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Store setup" && args.ItemKey == "LS Commerce licenses")
            {
                args.Add(new ItemButton(Resources.LSCommerceLicenses, Resources.ManageLSCommerceLicenses, ShowLSCommerceLicenses), 200);
            }
        }

        public static void ShowLSCommerceLicenses(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new LicensesView());
            }
        }
        public static void EditLicence(OmniLicense omniLicense)
        {
            var dlg = new Dialogs.EditLicenseDialog(omniLicense);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "LSCommerce license", omniLicense.Licensekey, null);
            }
        }

        public static void DeleteLicense(RecordIdentifier omniLicenseID)
        {
            if (MessageDialog.Show(Resources.DeleteLSCommerceLicenseQuestion, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                try
                {
                    service.DeleteOmniLicense(PluginEntry.DataModel, GetSiteServiceProfile(), omniLicenseID, true);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "LSCommerce license", omniLicenseID, null);
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }
            }
        }

        public static void ShowMobileInventoryMenu(object sender, EventArgs args)
        {
            IPlugin touchButtons = PluginEntry.Framework.FindImplementor(null, "CanManagePosMenuHeaders", null);
            if(touchButtons != null)
            {
                touchButtons.Message(null, "ManagePosMenuHeaders",
                                            new object[] { RecordIdentifier.Empty, DeviceTypeEnum.MobileInventory });
            }
        }

        /// <summary>
        /// Returns the selected site service profile for the Site Manager. If no site service profile has been selected then the function returns null
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>The Site managers site service profile. Returns null if no site service profile has been selected</returns>
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

        /// <summary>
        /// Returns the selected site service profile for the Site Manager. If no site service profile has been selected then the function returns null
        /// </summary>
        /// <returns>The Site managers site service profile. Returns null if no site service profile has been selected</returns>
        public static SiteServiceProfile GetSiteServiceProfile()
        {
            if (siteServiceProfile == null)
            {
                Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
                if (parameters != null)
                {
                    siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, parameters.SiteServiceProfile);
                }
            }
            return siteServiceProfile;
        }
    }
}
