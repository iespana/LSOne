using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.CustomerLedger.ViewPages;

namespace LSOne.ViewPlugins.CustomerLedger
{
    internal class PluginOperations
    {

        public static bool UseCentralCustomer(RecordIdentifier siteServiceProfileID)
        {
            siteServiceProfileID = siteServiceProfileID == "" ? RecordIdentifier.Empty : siteServiceProfileID;

            if (siteServiceProfileID == RecordIdentifier.Empty)
            {
                return false;
            }

            SiteServiceProfile profile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, siteServiceProfileID, CacheType.CacheTypeApplicationLifeTime);
            return profile == null || profile.CheckCustomer;
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
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

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Administration.Views.AdministrationView")
            {
                // Here we might want to check permission depending on what you want to do,
                // the fact that the Admin sheet is showing allready means that the admin sheet made sure
                // the user had permission to show admin sheet, so you would only want to check
                // permission here if the additional tab requires different permission than the permission to
                // enter the parent sheet, in this case the Administration sheet.
                args.Add(new TabControl.Tab(Properties.Resources.CustomerLedger, CustomerLedgerViewPage.CreateInstance), 110);
            }

            if (args.ContextName == "LSOne.ViewPlugins.Customer.Views.CustomerView")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CustomerLedgerEntriesView))
                {
                    args.Add(new TabControl.Tab(Properties.Resources.Ledger, CustomerLedgerPage.CreateInstance), 128);
                }
            }
        }
    }
}
