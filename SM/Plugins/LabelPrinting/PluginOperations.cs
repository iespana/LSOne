using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.Labels;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.LabelPrinting.Dialogs;
using LSOne.ViewPlugins.LabelPrinting.Properties;

namespace LSOne.ViewPlugins.LabelPrinting
{
    internal class PluginOperations
    {
        public static void TaskBarCategoryCallback(object sender, ContextBarHeaderConstructionArguments arguments)
        {
            if ((arguments.Key == "LSOne.ViewPlugins.RetailItems.Views.ItemView"))
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.Actions, arguments.Key + ".Actions"), 200);
            }
            else if ((arguments.Key == "LSOne.ViewPlugins.Customer.Views.CustomerView") ||
                (arguments.Key == "LSOne.ViewPlugins.Customer.Views.CustomersView"))
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.Actions, arguments.Key + ".Actions"), 200);
            }
            else if ((arguments.Key == "LSOne.ViewPlugins.Inventory.Views.PurchaseOrderView"))
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.Actions, arguments.Key + ".Actions"), 200);
            }
            else if ((arguments.Key == "LSOne.ViewPlugins.Inventory.Views.StoreTransferView"))
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.Actions, arguments.Key + ".Actions"), 200);
            }
            else if ((arguments.Key == "LSOne.ViewPlugins.Inventory.Views.GoodsReceivingDocumentView"))
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.Actions, arguments.Key + ".Actions"), 200);
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            const int priority = 100;
            if (arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.ItemView.Actions" && !arguments.View.MultiEditMode)
            {
                var item = new ContextBarItem(Properties.Resources.LabelPrinting, LabelsClickedFromContextBar)
                    {
                        Tag = new RecordIdentifier("Item", "")
                    };

                arguments.Add(item, priority);
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.ItemsView.Actions")
            {
                var item = new ContextBarItem(Properties.Resources.LabelPrinting, LabelsItemsClickedFromContextBar)
                    {
                        Tag = new RecordIdentifier("Item", "")
                    };

                arguments.Add(item, priority);
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Customer.Views.CustomerView.Actions")
            {
                var item = new ContextBarItem(Properties.Resources.LabelPrinting, LabelsClickedFromContextBar)
                {
                    Tag = new RecordIdentifier("Customer", "")
                };

                arguments.Add(item, priority);
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Customer.Views.CustomersView.Actions")
            {
                var item = new ContextBarItem(Properties.Resources.LabelPrinting, LabelsItemsClickedFromContextBar)
                {
                    Tag = new RecordIdentifier("Customer", "")
                };

                arguments.Add(item, priority);
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Inventory.Views.PurchaseOrderView.Actions" && !arguments.View.MultiEditMode)
            {
                var item = new ContextBarItem(Properties.Resources.LabelPrinting, LabelsItemsClickedFromContextBar)
                {
                    Tag = new RecordIdentifier("Order", "")
                };

                arguments.Add(item, priority);
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Inventory.Views.GoodsReceivingDocumentView.Actions" && !arguments.View.MultiEditMode)
            {
                var item = new ContextBarItem(Properties.Resources.LabelPrinting, LabelsItemsClickedFromContextBar)
                {
                    Tag = new RecordIdentifier("Order", "")
                };

                arguments.Add(item, priority);
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Inventory.Views.StoreTransferView.Actions" && !arguments.View.MultiEditMode && arguments.View.Tag != null && (InventoryTransferType)arguments.View.Tag == InventoryTransferType.Incoming)
            {
                var item = new ContextBarItem(Properties.Resources.LabelPrinting, LabelsItemsClickedFromContextBar)
                {
                    Tag = new RecordIdentifier("Order", "")
                };

                arguments.Add(item, 200);
            }
        }

        private static void LabelsClickedFromContextBar(object sender, ContextBarClickEventArguments args)
        {
            var context = (string) ((RecordIdentifier) ((ContextBarItem) sender).Tag).PrimaryID;

            switch (context)
            {
                case "Item":
                    {
                        var dlg = new LabelPrintingDialog(LabelTemplate.ContextEnum.Items);
                        dlg.EntitiesToPrint.Add(Providers.RetailItemData.Get(PluginEntry.DataModel, args.ContextID));
                        dlg.ShowDialog(PluginEntry.Framework.MainWindow);
                    }
                    break;
                case "Customer":
                    {
                        var dlg = new LabelPrintingDialog(LabelTemplate.ContextEnum.Customers);
                        dlg.EntitiesToPrint.Add(Providers.CustomerData.Get(PluginEntry.DataModel, args.ContextID, UsageIntentEnum.Normal));
                        dlg.ShowDialog(PluginEntry.Framework.MainWindow);
                    }
                    break;

                default:
                    break;
            }
        }

        private static void LabelsItemsClickedFromContextBar(object sender, ContextBarClickEventArguments args)
        {
            string context = (string)((RecordIdentifier)((ContextBarItem)sender).Tag).PrimaryID;
            
            List<IDataEntity> entities = (args.ContextView as ViewBase).GetListSelection();
            if (entities.Count == 0)
            {
                MessageDialog.Show(Resources.NoItemSelected + ".",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            switch (context)
            {
                case "Item":
                    {
                        var dlg = new LabelPrintingDialog(LabelTemplate.ContextEnum.Items)
                        {
                            EntitiesToPrint = entities
                        };

                        dlg.ShowDialog(PluginEntry.Framework.MainWindow);
                    }
                    break;
                case "Order":
                    {
                        bool multipleSelected = false;
                        int qty = 0;
                        if (entities.Count > 1)
                        {
                            multipleSelected = true;
                        }
                        if (entities != null && entities.Count != 0)
                        {
                            qty = (int)entities[0].ID.SecondaryID;
                        }

                        var dlg = new LabelPrintingDialog(LabelTemplate.ContextEnum.Items, multipleSelected, qty)
                        {
                            EntitiesToPrint = entities
                        };

                        dlg.ShowDialog(PluginEntry.Framework.MainWindow);
                    }
                    break;
                case "Customer":
                    {
                        var dlg = new LabelPrintingDialog(LabelTemplate.ContextEnum.Customers)
                        {
                            EntitiesToPrint = entities
                        };

                        dlg.ShowDialog(PluginEntry.Framework.MainWindow);
                    }
                    break;

                default:
                    break;
            }
        }

        internal static bool DeleteLabelTemplate(RecordIdentifier labelTemplateID)
        {
            if (QuestionDialog.Show(Properties.Resources.DeleteLabelTemplatesQuestion, Properties.Resources.DeleteLabelTemplates) == DialogResult.Yes)
            {
                Providers.LabelTemplateData.Delete(PluginEntry.DataModel, labelTemplateID);
                return true;
            }
            return false;
        }

        internal static bool DeleteLabelTemplates(List<RecordIdentifier> IDs)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteLabelTemplatesQuestion, Properties.Resources.DeleteLabelTemplates) == DialogResult.Yes)
                {
                    foreach (var ID in IDs)
                    {
                        Providers.LabelTemplateData.Delete(PluginEntry.DataModel, ID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "LabelTemplate", null, IDs);
                    return true;
                }
            }
            return false;
        }

        public static RecordIdentifier NewLabelTemplate(LabelTemplate.ContextEnum context)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit))
            {
                var dlg = new NewLabelTemplateDialog(context);
                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.LabelTemplate.ID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "LabelTemplate", selectedID, null);

                    ShowLabelTemplateSheet(selectedID);
                }
            }

            return selectedID;
        }

        public static LabelTemplate ShowNewLabelTemplateDialog(LabelTemplate.ContextEnum context)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit))
            {
                var dlg = new NewLabelTemplateDialog(context);
                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "LabelTemplate", dlg.LabelTemplate.ID, null);
                    return dlg.LabelTemplate;
                }
            }

            return null;
        }

        public static void ShowItemLabelTemplatesView(object sender, EventArgs args)
        {
            ShowLabelTemplatesSheet("", LabelTemplate.ContextEnum.Items);
        }

        public static void ShowCustomerLabelTemplatesView(object sender, EventArgs args)
        {
            ShowLabelTemplatesSheet("", LabelTemplate.ContextEnum.Customers);
        }

        public static void ShowLabelTemplateSheet(RecordIdentifier labelTemplateID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.LabelTemplateView(labelTemplateID));
        }

        public static void ShowLabelTemplatesSheet(RecordIdentifier labelTemplateID, LabelTemplate.ContextEnum context)
        {
            PluginEntry.Framework.ViewController.Add(new Views.LabelTemplatesView(labelTemplateID, context));
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.GeneralSetup, "Store setup"), 75);
            args.Add(new Page(Properties.Resources.Item, "Item"), 200);
            args.Add(new Page(Properties.Resources.Customers, "Customers"), 500);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Item")
            {
                args.Add(new PageCategory(Properties.Resources.Labels, "ItemLabels"), 400);
            }
            if (args.PageKey == "Customers")
            {
                args.Add(new PageCategory(Properties.Resources.Labels, "CustomerLabels"), 500);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {

            if (args.PageKey == "Item" && args.CategoryKey == "ItemLabels")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.ItemLabelTemplates,
                            Properties.Resources.ItemLabelTemplates,
                            Properties.Resources.ItemLabelTemplateTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            Properties.Resources.label_templates_32,
                            ShowItemLabelTemplatesView,
                            "LabelTemplates"), 10);
                }
            }
            if (args.PageKey == "Customers" && args.CategoryKey == "CustomerLabels")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.CustomerLableTemplates,
                            Properties.Resources.CustomerLableTemplates,
                            Properties.Resources.CustomerLabelTemplateTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            Properties.Resources.label_templates_32,
                            ShowCustomerLabelTemplatesView,
                            "LabelTemplates"), 10);
                }
            }
        }

        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;

            switch (args.Key)
            {
                // We dont need to check permissions here since thats been done on the parent menu allready
                case "RibbonLabelTemplates":
                    item = new ExtendedMenuItem(
                        Properties.Resources.ItemLabelTemplates + "...",
                        null,
                        10,
                        new EventHandler(ShowItemLabelTemplatesView));

                    args.AddMenu(item);

                    item = new ExtendedMenuItem(
                        Properties.Resources.CustomerLableTemplates,
                        null,
                        20,
                        new EventHandler(ShowCustomerLabelTemplatesView));

                    args.AddMenu(item);

                    break;
            }
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.GeneralSetup, "General setup", null), 100);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Store setup")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsView))
                {
                    args.Add(new Item(Properties.Resources.FormsAndLabels, "Forms", null), 1000);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Store setup" && args.ItemKey == "Forms")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit))
                {
                    args.Add(new ItemButton(Properties.Resources.ItemLabelTemplates, Properties.Resources.ItemLabelTemplateDescription, new EventHandler(ShowItemLabelTemplatesView)), 20);

                    args.Add(new ItemButton(Properties.Resources.CustomerLableTemplates, Properties.Resources.CustomerLabelTemplateDescription, new EventHandler(ShowCustomerLabelTemplatesView)), 30);
                }
            }
        }
    }
}
