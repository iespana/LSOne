using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.RMSMigration.Dialogs;
using LSOne.ViewPlugins.RMSMigration.Helper;
using LSOne.ViewPlugins.RMSMigration.Properties;
using System;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.RMSMigration
{
    public class PluginOperations
    {
        #region internal Methods

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Resources.GeneralSetup, "General setup", null), 100);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "General setup")
            {
                args.Add(new Item(Resources.RMSDataMigration, "RMS data migration", null), 10001);
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "General setup" && args.ItemKey == "RMS data migration")
            {
                args.Add(new ItemButton(Resources.RMSDataImport, Resources.RMSDataImportDescription, ShowRMSDataImport), 1);
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Resources.Tools, "Tools"), 1000);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Tools")
            {
                args.Add(new PageCategory(Resources.Import, "Import"), 200);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Tools" && args.CategoryKey == "Import")
            {
                args.Add(new CategoryItem(
                        Resources.RMSDataImport,
                        Resources.RMSDataImport,
                        Resources.RMSDataImportDescription,
                        CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
                        null,
                        Resources.RMS_import_32,
                        ShowRMSDataImport,
                        "RMSDataImport"), 40);
            }
        }

        #endregion

        #region Public Methods
        public static void ShowRMSDataImport(object sender, EventArgs args)
        {
            string rmsMigrationFlag = Providers.PosisInfoData.Get(PluginEntry.DataModel, Constants.RMSMigrationFlag);
            if (string.IsNullOrEmpty(rmsMigrationFlag))
            {
                RMSMigrationWizard dlg = new RMSMigrationWizard(PluginEntry.DataModel);
                PluginEntry.Framework.SuspendSearchBarClosing();

                dlg.ShowDialog();
            }

            else
            {
                MessageDialog.Show(Resources.MigrationAlreadyBeenRun, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            PluginEntry.Framework.ResumeSearchBarClosing();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}