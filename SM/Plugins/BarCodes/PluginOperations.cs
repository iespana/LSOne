using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.BarCodes.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.BarCodes
{
    internal class PluginOperations
    {
        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.RetailItems.Views.ItemView")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageItemBarcodes))
                {
                    args.Add(new TabControl.Tab(Resources.BarCodes, ItemTabKey.Barcodes.ToString(), new PanelFactoryHandler(ViewPages.ItemViewBarCodesPage.CreateInstance)) { Enabled = !args.IsMultiEdit }, 250);
                }
            }
            else if(args.ContextName == "LSOne.ViewPlugins.RetailItems.Dialogs.NewRetailItemDialog")
            {
                args.Add(new TabControl.Tab(Resources.BarCodes, new PanelFactoryHandler(DialogPages.NewRetailItemBarCodePage.CreateInstance), true, true), 50);
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.ItemView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks))
                {
                    arguments.Add(new ContextBarItem(
                        Properties.Resources.ManageBarCodeMasks,
                        new ContextbarClickEventHandler(PluginOperations.ShowBarCodeMaskSetup)), 300);
                }
            }
        }

        internal static void ShowBarCodeSetup(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.BarCodeSetupView());
        }

        internal static void ShowBarCodeSetup(RecordIdentifier args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.BarCodeSetupView());
        }

        internal static void ShowBarCodeMaskSetup(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.BarCodeMaskSetupView());
        }

        internal static void ShowBarCodeSetupEditor(RecordIdentifier barCodeSetupID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.BarCodeSetupDataView(barCodeSetupID));
        }

        public static RecordIdentifier NewBarCode(RecordIdentifier itemID, RecordIdentifier itemMasterID, bool sizeAllowed, bool colorAllowed, bool styleAllowed, DataEntity selectedBarCodeSetup, bool forceDefaultBarcode)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ItemsEdit))
            {
                Dialogs.BarCodeDialog dlg = new Dialogs.BarCodeDialog(itemID, itemMasterID, selectedBarCodeSetup, forceDefaultBarcode);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    selectedID = new RecordIdentifier(dlg.BarCodeID, itemID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "BarCode", selectedID, null);
                }
            }

            return selectedID;
        }

        public static bool DeleteBarCodeSetup(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup))
            {
                if (Providers.BarCodeSetupData.BarCodeSetupInUse(PluginEntry.DataModel, id))
                {
                    MessageDialog.Show(Resources.BarcodeSetupInUse);
                    return false;
                }
                if (QuestionDialog.Show(Properties.Resources.DeleteBarCodeSetupQuestion, Properties.Resources.DeleteBarCodeSetup) == DialogResult.Yes)
                {
                    Providers.BarCodeSetupData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "BarCodeSetup", (string)id, null);

                    return true;
                }
            }

            return false;
        }

        public static RecordIdentifier NewBarCodeSetup()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup))
            {
                Dialogs.NewBarcodeSetupDialog dlg = new Dialogs.NewBarcodeSetupDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.BarCodeSetupID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "BarCodeSetup", selectedID, null);

                    PluginOperations.ShowBarCodeSetupEditor(selectedID);
                }
            }

            return selectedID;
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.GeneralSetup, "General setup", null), 100);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "General setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks) ||
                    PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup))
                {
                    args.Add(new Item(Properties.Resources.BarCodes, "BarCodes", null), 450);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "General setup" && args.ItemKey == "BarCodes")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup))
                {
                    args.Add(new ItemButton(Properties.Resources.BarCodeSetup, Properties.Resources.BarCodeSetupDescription, new EventHandler(ShowBarCodeSetup)), 100);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks))
                {
                    args.Add(new ItemButton(Properties.Resources.BarCodeMaskSetup, Properties.Resources.BarCodeMaskSetupDescription, new EventHandler(ShowBarCodeMaskSetup)), 200);
                }

            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Setup, "Setup"), 900);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Setup")
            {
                args.Add(new PageCategory(Properties.Resources.BarCodes, "BarCodes"), 300);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Setup" && args.CategoryKey == "BarCodes")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup) || PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks))
                {
                    args.Add(new CategoryItem(
                           Properties.Resources.BarcodesOneWord,
                           Properties.Resources.BarcodesOneWord,
                           Properties.Resources.BarcodesTooltip,
                           CategoryItem.CategoryItemFlags.DropDown,
                           null,
                           Properties.Resources.barcodes_32,
                           null,
                           "BarCodes"), 10);
                }
               /* if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.SetupBarCode,
                            Properties.Resources.SetupBarCode,
                            Properties.Resources.BarCodeSetupDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            null,
                            new EventHandler(ShowBarCodeSetup),
                            "Setup"), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.MaskSetupBarCode,
                            Properties.Resources.MaskSetupBarCode,
                            Properties.Resources.BarCodeMaskSetupDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            null,
                            new EventHandler(ShowBarCodeMaskSetup),
                            "MaskSetup"), 20);
                }*/
            }
        }

        internal static string TypeText(BarcodeSegmentType type)
        {
            return Properties.Resources.ResourceManager.GetString(type.ToString(), Properties.Resources.Culture);
        }

        internal static string SymbologyText(BarcodeType symbology)
        {
            return Datalayer.DataEntities.BarCodeTypes.BarCodeType.GetBarCodeType((int)symbology).Name;
        }
    }
}
