using LSOne.ViewCore.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.GenericConnector;
using System.IO;
using LSOne.ViewCore.Ribbon;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.SerialNumbers
{
    public class PluginOperations
    {

        private static SiteServiceProfile siteServiceProfile;

        #region Internal Methods

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "RetailItems")
            {
                args.Add(new ItemButton(Properties.Resources.SerialNumbersMenuItem,
                    Properties.Resources.SerialNumbersMenuItemDescription,
                    ShowSerialNumberListView), 500);
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Item, "Item"), 200);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Item")
            {
                args.Add(new PageCategory(Properties.Resources.Items, "Items"), 100);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Item" && args.CategoryKey == "Items")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageSerialNumbers))
                {
                    args.Add(new CategoryItem(
                           Properties.Resources.SerialNumbersMenuItem,
                           Properties.Resources.SerialNumbersMenuItem,
                           Properties.Resources.SerialNumbersTooltipDescription,
                           CategoryItem.CategoryItemFlags.Button,
                           Properties.Resources.serial_number_16,
                           null,
                           ShowSerialNumberListView,
                           "SerialNumbers"), 30);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays the serial number list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void ShowSerialNumberListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SerialNumbersView());
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
        /// Returns information about the item. If it cannot be found locally the site service will be used to retrieve information about the item
        /// </summary>
        /// <param name="itemID">The unique ID of the item to retrieve</param>
        /// <returns></returns>
        public static RetailItem GetRetailItem(RecordIdentifier itemID, bool displayErrorMsg = true)
        {
            RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
            if (retailItem != null)
            {
                return retailItem;
            }

            try
            {
                retailItem = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).GetRetailItem(
                    PluginEntry.DataModel,
                    GetSiteServiceProfile(PluginEntry.DataModel),
                    itemID, true);

                if (retailItem != null)
                {
                    return retailItem;
                }

                if (displayErrorMsg)
                {
                    MessageDialog.Show(Properties.Resources.InformationAboutItemCannotBeFound);
                }
                return null;
            }
            catch (Exception ex)
            {
                if (displayErrorMsg)
                {
                    MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }
                return null;
            }
        }

        /// <summary>
        /// Retreives files from default user folder
        /// </summary>
        /// <param name="workingFolder"></param>
        /// <param name="fileTypeFilters"></param>
        /// <returns></returns>
        public static List<string> GetFilesFromDefaultFolder(Setting workingFolder, List<string> fileTypeFilters)
        {
            List<string> fileNames = new List<string>();

            if (workingFolder != null && !string.IsNullOrEmpty(workingFolder.Value) && Directory.Exists(workingFolder.Value))
            {
                fileNames = Directory.GetFiles(workingFolder.Value).Where(s => fileTypeFilters.Any(s.EndsWith)).ToList();
            }
            return fileNames;
        }

        public static bool IsDefaultProfileValid(RecordIdentifier defaultProfileMasterId)
        {
            return Providers.ImportProfileLineData.GetSelectList(PluginEntry.DataModel, defaultProfileMasterId)
                .Any();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
