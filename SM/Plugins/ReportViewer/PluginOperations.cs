using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.ReportViewer.Views;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Dialogs;
using System.Linq;

namespace LSOne.ViewPlugins.ReportViewer
{
    internal class PluginOperations
    {
        private static SiteServiceProfile siteServiceProfile;
        internal static void ManageReports(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new ReportManagementView());
        }

        private static void ViewReports(object sender, EventArgs args)
        {
            if (((ToolStripItem)sender).Tag != null)
            {
                ShowReport((RecordIdentifier)((ToolStripItem)sender).Tag);
            }
        }

        public static void ShowReport(RecordIdentifier reportID)
        {
            if (reportID != RecordIdentifier.Empty)
            {
                PluginEntry.Framework.ViewController.Add(new ReportViewerView(reportID));
            }
        }

        public static void ShowReport(RecordIdentifier reportID, Dictionary<string, ProcedureParameter> preSetValues)
        {
            PluginEntry.Framework.ViewController.Add(new ReportViewerView(reportID, preSetValues));
        }

        public static void ShowReportPreview(object sender, PluginOperationArguments args)
        {
            if (args.WantsViewReturned)
            {
                Dictionary<string, ProcedureParameter> preSetValues =
                    (Dictionary<string, ProcedureParameter>)args.Param;
                args.ResultView = new Views.ReportViewerView(args.ID, preSetValues, true);
                if (args.ResultView != null)
                {
                    PluginEntry.Framework.ViewController.Add(args.ResultView);
                }
            }
        }

        public static void ShowReportPrint(object sender, PluginOperationArguments args)
        {
            if (args.WantsViewReturned)
            {
                Dictionary<string, ProcedureParameter> preSetValues =
                    (Dictionary<string, ProcedureParameter>)args.Param;
                args.ResultView = new Views.ReportViewerView(args.ID, preSetValues, true, true);
                if (args.ResultView != null)
                {
                    PluginEntry.Framework.ViewController.Add(args.ResultView);
                }
            }
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
        public static void TaskBarCategoryCallback(object sender, ContextBarHeaderConstructionArguments arguments)
        {
            if (arguments.Key == "LSOne.ViewPlugins.RetailItems.Views.ItemView")
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.Reports, "LSOne.ViewPlugins.RetailItems.Views.ItemView.Reports") { Collapsed = true }, 400);
            }
            else if (arguments.Key == "LSOne.ViewPlugins.Customer.Views.CustomerView")
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.Reports, "LSOne.ViewPlugins.Customer.Views.CustomerView.Reports") { Collapsed = true }, 400);
            }
            else if (arguments.Key == "LSOne.ViewPlugins.Store.Views.StoreView")
            {
                if (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || arguments.ContextIdentifier == PluginEntry.DataModel.CurrentStoreID)
                {
                    arguments.Add(new ContextBarHeader(Properties.Resources.Reports, "LSOne.ViewPlugins.Store.Views.StoreView.Reports") { Collapsed = true }, 400);
                }
            }
            else if (arguments.Key == "LSOne.ViewPlugins.Store.Views.TerminalView")
            {
                var storeID = (RecordIdentifier)arguments.View.Message("StoreID", null);

                if (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || arguments.ContextIdentifier == storeID)
                {
                    arguments.Add(new ContextBarHeader(Properties.Resources.Reports, "LSOne.ViewPlugins.Store.Views.TerminalView.Reports") { Collapsed = true }, 400);
                }
            }
            else if (arguments.Key == "LSOne.ViewPlugins.Inventory.Views.VendorView")
            {
                arguments.Add(new ContextBarHeader(Properties.Resources.Reports, "LSOne.ViewPlugins.Inventory.Views.VendorView.Reports") { Collapsed = true }, 400);
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            List<ReportListItem> buttons;
            ContextBarItem item;
            int priority;

            if (arguments.CategoryKey == "LSOne.ViewPlugins.Store.Views.StoreView.Reports")
            {
                buttons = Providers.ReportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Store, CacheType.CacheTypeApplicationLifeTime);

                priority = 10;

                foreach (ReportListItem button in buttons)
                {
                    item = new ContextBarItem(button.Text, new ContextbarClickEventHandler(ReportClickedFromContextBar));
                    item.Tag = new RecordIdentifier("Store", button.ID);

                    arguments.Add(item, priority);

                    priority += 10;
                }
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Store.Views.TerminalView.Reports")
            {
                buttons = Providers.ReportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Terminal, CacheType.CacheTypeApplicationLifeTime);

                priority = 10;

                foreach (ReportListItem button in buttons)
                {
                    item = new ContextBarItem(button.Text, new ContextbarClickEventHandler(ReportClickedFromContextBar));
                    item.Tag = new RecordIdentifier("Terminal", button.ID);

                    arguments.Add(item, priority);

                    priority += 10;
                }
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.ItemView.Reports")
            {
                if (!arguments.View.MultiEditMode)
                {
                    buttons = Providers.ReportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Item, CacheType.CacheTypeApplicationLifeTime);

                    priority = 10;

                    foreach (ReportListItem button in buttons)
                    {
                        item = new ContextBarItem(button.Text, new ContextbarClickEventHandler(ReportClickedFromContextBar));
                        item.Tag = new RecordIdentifier("Item", button.ID);

                        arguments.Add(item, priority);

                        priority += 10;
                    }
                }
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Customer.Views.CustomerView.Reports")
            {
                buttons = Providers.ReportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Customer, CacheType.CacheTypeApplicationLifeTime);

                priority = 10;

                foreach (ReportListItem button in buttons)
                {
                    item = new ContextBarItem(button.Text, new ContextbarClickEventHandler(ReportClickedFromContextBar));
                    item.Tag = new RecordIdentifier("Customer", button.ID);

                    arguments.Add(item, priority);

                    priority += 10;
                }
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Inventory.Views.VendorView.Reports")
            {
                buttons = Providers.ReportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Vendor, CacheType.CacheTypeApplicationLifeTime);

                priority = 10;

                foreach (ReportListItem button in buttons)
                {
                    item = new ContextBarItem(button.Text, new ContextbarClickEventHandler(ReportClickedFromContextBar));
                    item.Tag = new RecordIdentifier("Vendor", button.ID);

                    arguments.Add(item, priority);

                    priority += 10;
                }
            }
        }

        private static void PlaceVendorParams(Dictionary<string, ProcedureParameter> preSetValues, RecordIdentifier vendorID, string vendorName)
        {
            ProcedureParameter param;

            param = new ProcedureParameter("@VENDORID");
            param.Value = (string)vendorID;

            preSetValues.Add("@VENDORID", param);

            param = new ProcedureParameter("@VENDORNAME");
            param.Value = vendorName;

            preSetValues.Add("@VENDORNAME", param);

            param = new ProcedureParameter("@VENDORSID");
            param.Value = new string[] { (string)vendorID };

            preSetValues.Add("@VENDORSID", param);

            param = new ProcedureParameter("@VENDORSNAME");
            param.Value = new string[] { vendorName };

            preSetValues.Add("@VENDORSNAME", param);
        }

        private static void PlaceItemParams(Dictionary<string, ProcedureParameter> preSetValues, RecordIdentifier itemID, string itemName)
        {
            ProcedureParameter param;

            param = new ProcedureParameter("@RETAILITEMID");
            param.Value = (string)itemID;

            preSetValues.Add("@RETAILITEMID", param);

            param = new ProcedureParameter("@RETAILITEMNAME");
            param.Value = itemName;

            preSetValues.Add("@RETAILITEMNAME", param);
        }


        private static void PlaceStoreParams(Dictionary<string, ProcedureParameter> preSetValues, RecordIdentifier storeID, string storeName)
        {
            ProcedureParameter param;

            param = new ProcedureParameter("@STOREID");
            param.Value = (string)storeID;

            preSetValues.Add("@STOREID", param);

            param = new ProcedureParameter("@STORENAME");
            param.Value = storeName;

            preSetValues.Add("@STORENAME", param);

            param = new ProcedureParameter("@STORESID");
            param.Value = new string[] { (string)storeID };

            preSetValues.Add("@STORESID", param);

            param = new ProcedureParameter("@STORESNAME");
            param.Value = new string[] { storeName };

            preSetValues.Add("@STORESNAME", param);
        }

        private static void PlaceCustomerParams(Dictionary<string, ProcedureParameter> preSetValues, RecordIdentifier customerID, string customerName)
        {
            ProcedureParameter param;

            param = new ProcedureParameter("@CUSTOMERID");
            param.Value = (string)customerID;

            preSetValues.Add("@CUSTOMERID", param);

            param = new ProcedureParameter("@CUSTOMERNAME");
            param.Value = customerName;

            preSetValues.Add("@CUSTOMERNAME", param);
        }

        private static void PlaceTerminalParams(Dictionary<string, ProcedureParameter> preSetValues, RecordIdentifier terminalID, string terminalName)
        {
            ProcedureParameter param;

            param = new ProcedureParameter("@TERMINALID");
            param.Value = (string)terminalID;

            preSetValues.Add("@TERMINALID", param);

            param = new ProcedureParameter("@TERMINALNAME");
            param.Value = terminalName;

            preSetValues.Add("@TERMINALNAME", param);

            param = new ProcedureParameter("@TERMINALSID");
            param.Value = new string[] { (string)terminalID };

            preSetValues.Add("@TERMINALSID", param);

            param = new ProcedureParameter("@TERMINALSNAME");
            param.Value = new string[] { terminalName };

            preSetValues.Add("@TERMINALSNAME", param);
        }

        private static void ReportClickedFromContextBar(object sender, ContextBarClickEventArguments args)
        {
            Dictionary<string, ProcedureParameter> preSetValues = new Dictionary<string, ProcedureParameter>();

            RecordIdentifier reportID = ((RecordIdentifier)((ContextBarItem)sender).Tag).SecondaryID;
            string context = (string)((RecordIdentifier)((ContextBarItem)sender).Tag).PrimaryID;



            switch (context)
            {
                case "Store":
                    PlaceStoreParams(preSetValues, ((ViewBase)args.ContextView).ID, ((ViewBase)args.ContextView).ContextDescription);

                    break;

                case "Terminal":
                    RecordIdentifier storeID = (RecordIdentifier)((ViewBase)args.ContextView).Message("StoreID", null);
                    string storeName = (string)((ViewBase)args.ContextView).Message("StoreName", null);

                    PlaceStoreParams(preSetValues, storeID, storeName);
                    PlaceTerminalParams(preSetValues, ((ViewBase)args.ContextView).ID, ((ViewBase)args.ContextView).ContextDescription);

                    break;

                case "Customer":
                    PlaceCustomerParams(preSetValues, ((ViewBase)args.ContextView).ID, ((ViewBase)args.ContextView).ContextDescription);

                    break;

                case "Vendor":
                    PlaceVendorParams(preSetValues, ((ViewBase)args.ContextView).ID, ((ViewBase)args.ContextView).ContextDescription);

                    break;

                case "Item":
                    PlaceItemParams(preSetValues, ((ViewBase)args.ContextView).ID, ((ViewBase)args.ContextView).ContextDescription);

                    break;

                default:
                    break;
            }

            ShowReport(reportID, preSetValues);
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Retail, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                args.Add(new Item(Properties.Resources.Reports, "RetailReports", null), 600);
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            ItemButton btn;
            List<ReportListItem> buttons;
            ItemDropDownButton dropBtn;


            if (args.CategoryKey == "Retail")
            {
                if (args.ItemKey == "RetailReports")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageReports))
                    {
                        args.Add(new ItemButton(Properties.Resources.ManageReports, Properties.Resources.ManageReportsDescription, new EventHandler(ManageReports)), 50);
                    }

                    dropBtn = new ItemDropDownButton(Properties.Resources.ViewReport, Properties.Resources.ViewReportDescription, new EventHandler(ViewReports));

                    var reportData = Providers.ReportData;
                    buttons = reportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Report, CacheType.CacheTypeApplicationLifeTime);

                    if (buttons.Count > 0)
                    {
                        foreach (ReportListItem button in buttons)
                        {
                            dropBtn.AddItem(button.Text, button.Description, button.ID);
                        }
                    }
                    else
                    {
                        dropBtn.AddItem(Properties.Resources.NoReportsAvailable, "", null);
                    }

                    args.Add(dropBtn, 100);

                    buttons = reportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Button, CacheType.CacheTypeApplicationLifeTime);

                    foreach (ReportListItem button in buttons)
                    {
                        btn = new ItemButton(button.Text, button.Description, new EventHandler(ReportButtonClicked));
                        btn.Tag = button.ID;

                        args.Add(btn, 500);
                    }
                }

                if (args.CategoryKey == "EOD")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.ViewFinancialReports))
                    {
                        args.Add(new ItemButton(Properties.Resources.FinancialReport, Properties.Resources.FinancialReportDescription, ShowFinancialReport), 40);
                        args.Add(new ItemButton(Properties.Resources.FinancialReportByEmployee, Properties.Resources.FinancialReportByEmployeeDescription, ShowFinancialReportByEmployee), 50);
                        args.Add(new ItemButton(Properties.Resources.FinancialReportByStatement, Properties.Resources.FinancialReportByStatementDescription, ShowFinancialReportByStatement), 60);
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ViewSalesReports))
                    {
                        args.Add(new ItemButton(Properties.Resources.ItemSales, Properties.Resources.ItemSalesReportsDescription, ShowItemSalesReport), 70);
                        args.Add(new ItemButton(Properties.Resources.GiftCardSales, Properties.Resources.GiftCardSalesDescription, ShowGiftCardSalesReport), 80);
                        args.Add(new ItemButton(Properties.Resources.IssuedCreditMemos, Properties.Resources.IssuedCreditMemosDescription, ShowIssuedCreditMemosReport), 90);
                        args.Add(new ItemButton(Properties.Resources.SalesPerEmployee, Properties.Resources.SalesPerEmployeeDescription, ShowSalesPerEmployeeReport), 100);
                        args.Add(new ItemButton(Properties.Resources.SalesPerTerminal, Properties.Resources.SalesPerTerminalDescription, ShowSalesPerTerminalReport), 110);
                    }
                }
            }

            if (args.CategoryKey == "General setup" && args.ItemKey == "Inventory")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryReports))
                {
                    args.Add(new ItemButton(Properties.Resources.StockLevelReport, Properties.Resources.StockLevelReportDescription, ShowStockLevelReport), 1000);
                    args.Add(new ItemButton(Properties.Resources.InventoryBelowReorderpoint, Properties.Resources.InventoryBelowReorderpointDescription, ShowInventoryBelowReorderpointReport), 1100);
                }
            }
        }

        internal static void ReportButtonClicked(object sender, EventArgs args)
        {
            ItemButton button = (ItemButton)sender;
            RecordIdentifier reportID = (RecordIdentifier)button.Tag;

            ShowReport(reportID);
        }

        internal static void RibbonReportButtonClicked(object sender, EventArgs args)
        {
            ExtendedMenuItem button = (ExtendedMenuItem)sender;
            RecordIdentifier reportID = (RecordIdentifier)button.Tag;

            ShowReport(reportID);
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Retail, "Tools"), 1000);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Tools"
            || args.PageKey == "Sales"
            || args.PageKey == "Item"
            || args.PageKey == "Customers"
            || args.PageKey == "Hospitality"
            || args.PageKey == "Setup"
            || args.PageKey == "Users"
            || args.PageKey == "Inventory"
                )
            {
                int priority = 0;

                switch(args.PageKey)
                {
                    case "Tools": priority = 250; break;
                    case "Sales": priority = 300; break;
                    case "Item": priority = 500; break;
                    case "Customers": priority = 600; break;
                    case "Hospitality": priority = 500; break;
                    case "Setup": priority = 600; break;
                    case "Users": priority = 400; break;
                    case "Inventory": priority = 400; break;
                }

                args.Add(new PageCategory(Properties.Resources.Reports, "Reports"), priority);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.CategoryKey == "Reports")
            {
                if (args.PageKey == "Tools")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageReports))
                    {
                        args.Add(new CategoryItem(
                        Properties.Resources.ManageReports,
                        Properties.Resources.ManageReports,
                        Properties.Resources.ManageReportsTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        Properties.Resources.manage_reports_32,
                        ManageReports,
                        "ManageReport"), 10);
                    }

                    //Add dropdown for all reports
                    if (PluginEntry.DataModel.HasPermission(Permission.ViewReports))
                    {
                        args.Add(new CategoryItem(
                               Properties.Resources.ViewReport,
                               Properties.Resources.ViewReport,
                               Properties.Resources.ViewReportTooltipDescription,
                               CategoryItem.CategoryItemFlags.DropDown,
                               null,
                               Properties.Resources.reports_32,
                               null,
                               "AllReports"), 20);
                    }
                }
                else
                {
                    List<ReportListItem> reportListItems = Providers.ReportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Report, CacheType.CacheTypeApplicationLifeTime);

                    if(args.PageKey == "Sales")
                    {
                        if(PluginEntry.DataModel.HasPermission(Permission.ViewFinancialReports) && reportListItems.Any(x => x.ReportCategory == ReportCategory.Financial))
                        {
                            args.Add(new CategoryItem(
                                Properties.Resources.Financial,
                                Properties.Resources.FinancialReports,
                                Properties.Resources.FinancialReportsTooltipDescription,
                                CategoryItem.CategoryItemFlags.DropDown,
                                null,
                                Properties.Resources.financial_reports_32,
                                null,
                                "FinancialReports"), 10);
                        }

                        if (PluginEntry.DataModel.HasPermission(Permission.ViewSalesReports) && reportListItems.Any(x => x.ReportCategory == ReportCategory.Sales))
                        {
                            args.Add(new CategoryItem(
                                Properties.Resources.Sales,
                                Properties.Resources.SalesReports,
                                Properties.Resources.SalesReportsTooltipDescription,
                                CategoryItem.CategoryItemFlags.DropDown,
                                null,
                                Properties.Resources.reports_32,
                                null,
                                "SalesReports"), 20);
                        }
                    }
                    else if (args.PageKey == "Item" && PluginEntry.DataModel.HasPermission(Permission.ViewItemReports) && reportListItems.Any(x => x.ReportCategory == ReportCategory.Item))
                    {
                        args.Add(new CategoryItem(
                            Properties.Resources.Item,
                            Properties.Resources.ItemReports,
                            Properties.Resources.ItemReportsTooltipDescription,
                            CategoryItem.CategoryItemFlags.DropDown,
                            null,
                            Properties.Resources.reports_32,
                            null,
                            "ItemReports"), 10);
                    }
                    else if(args.PageKey == "Customers" && PluginEntry.DataModel.HasPermission(Permission.ViewCustomerReports) && reportListItems.Any(x => x.ReportCategory == ReportCategory.Customer))
                    {
                        args.Add(new CategoryItem(
                            Properties.Resources.Customer,
                            Properties.Resources.CustomerReports,
                            Properties.Resources.CustomerReportsTooltipDescription,
                            CategoryItem.CategoryItemFlags.DropDown,
                            null,
                            Properties.Resources.reports_32,
                            null,
                            "CustomerReports"), 10);
                    }
                    else if (args.PageKey == "Hospitality" && PluginEntry.DataModel.HasPermission(Permission.ViewHospitalityReports) && reportListItems.Any(x => x.ReportCategory == ReportCategory.Hospitality))
                    {
                        args.Add(new CategoryItem(
                            Properties.Resources.Hospitality,
                            Properties.Resources.HospitalityReports,
                            Properties.Resources.HospitalityReportsTooltipDescription,
                            CategoryItem.CategoryItemFlags.DropDown,
                            null,
                            Properties.Resources.reports_32,
                            null,
                            "HospitalityReports"), 10);
                    }
                    else if (args.PageKey == "Setup" && PluginEntry.DataModel.HasPermission(Permission.ViewSetupReports) && reportListItems.Any(x => x.ReportCategory == ReportCategory.Setup))
                    {
                        args.Add(new CategoryItem(
                            Properties.Resources.Setup,
                            Properties.Resources.SetupReports,
                            Properties.Resources.SetupReportsTooltipDescription,
                            CategoryItem.CategoryItemFlags.DropDown,
                            null,
                            Properties.Resources.reports_32,
                            null,
                            "SetupReports"), 10);
                    }
                    else if (args.PageKey == "Users" && PluginEntry.DataModel.HasPermission(Permission.ViewUsersReports) && reportListItems.Any(x => x.ReportCategory == ReportCategory.User))
                    {
                        args.Add(new CategoryItem(
                            Properties.Resources.Users,
                            Properties.Resources.UsersReports,
                            Properties.Resources.UsersReportsTooltipDescription,
                            CategoryItem.CategoryItemFlags.DropDown,
                            null,
                            Properties.Resources.reports_32,
                            null,
                            "UsersReports"), 10);
                    }
                    else if (args.PageKey == "Inventory" && PluginEntry.DataModel.HasPermission(Permission.ViewInventoryReports) && reportListItems.Any(x => x.ReportCategory == ReportCategory.Inventory))
                    {
                        args.Add(new CategoryItem(
                            Properties.Resources.Inventory,
                            Properties.Resources.InventoryReports,
                            Properties.Resources.InventoryReportsTooltipDescription,
                            CategoryItem.CategoryItemFlags.DropDown,
                            null,
                            Properties.Resources.reports_32,
                            null,
                            "InventoryReports"), 10);
                    }
                }
            }
        }

        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            if(args.Key.Contains("Reports"))
            {
                List<ReportListItem> reports = Providers.ReportData.GetReportContextItems(PluginEntry.DataModel, ReportContextEnum.Report, CacheType.CacheTypeApplicationLifeTime);
                ExtendedMenuItem item;
                int priority = 10;

                if (args.Key == "RibbonAllReports")
                {
                    if (!PluginEntry.DataModel.HasPermission(Permission.ViewFinancialReports))
                    {
                        reports.RemoveAll(x => x.ReportCategory == ReportCategory.Financial);
                    }

                    if (!PluginEntry.DataModel.HasPermission(Permission.ViewSalesReports))
                    {
                        reports.RemoveAll(x => x.ReportCategory == ReportCategory.Sales);
                    }

                    if (!PluginEntry.DataModel.HasPermission(Permission.ViewInventoryReports))
                    {
                        reports.RemoveAll(x => x.ReportCategory == ReportCategory.Inventory);
                    }

                    if (!PluginEntry.DataModel.HasPermission(Permission.ViewItemReports))
                    {
                        reports.RemoveAll(x => x.ReportCategory == ReportCategory.Item);
                    }

                    if (!PluginEntry.DataModel.HasPermission(Permission.ViewCustomerReports))
                    {
                        reports.RemoveAll(x => x.ReportCategory == ReportCategory.Customer);
                    }

                    if (!PluginEntry.DataModel.HasPermission(Permission.ViewUsersReports))
                    {
                        reports.RemoveAll(x => x.ReportCategory == ReportCategory.User);
                    }

                    if (!PluginEntry.DataModel.HasPermission(Permission.ViewSetupReports))
                    {
                        reports.RemoveAll(x => x.ReportCategory == ReportCategory.Setup);
                    }

                    if (!PluginEntry.DataModel.HasPermission(Permission.ViewHospitalityReports))
                    {
                        reports.RemoveAll(x => x.ReportCategory == ReportCategory.Hospitality);
                    }

                    if (reports.Count > 0)
                    {
                        foreach (ReportListItem button in reports)
                        {
                            item = new ExtendedMenuItem(button.Text, priority, new EventHandler(RibbonReportButtonClicked));
                            item.Tag = button.ID;

                            args.AddMenu(item);
                            priority += 10;
                        }
                    }
                    else
                    {
                        item = new ExtendedMenuItem(Properties.Resources.NoReportsAvailable, priority, new EventHandler(RibbonReportButtonClicked));
                        item.Tag = RecordIdentifier.Empty;
                        args.AddMenu(item);
                    }
                }
                else
                {
                    List<ReportListItem> itemsToAdd = new List<ReportListItem>();

                    if(args.Key == "RibbonFinancialReports")
                    {
                        itemsToAdd = reports.Where(x => x.ReportCategory == ReportCategory.Financial).ToList();
                    }
                    else if (args.Key == "RibbonSalesReports")
                    {
                        itemsToAdd = reports.Where(x => x.ReportCategory == ReportCategory.Sales).ToList();
                    }
                    else if (args.Key == "RibbonItemReports")
                    {
                        itemsToAdd = reports.Where(x => x.ReportCategory == ReportCategory.Item).ToList();
                    }
                    else if (args.Key == "RibbonCustomerReports")
                    {
                        itemsToAdd = reports.Where(x => x.ReportCategory == ReportCategory.Customer).ToList();
                    }
                    else if (args.Key == "RibbonSetupReports")
                    {
                        itemsToAdd = reports.Where(x => x.ReportCategory == ReportCategory.Setup).ToList();
                    }
                    else if (args.Key == "RibbonHospitalityReports")
                    {
                        itemsToAdd = reports.Where(x => x.ReportCategory == ReportCategory.Hospitality).ToList();
                    }
                    else if (args.Key == "RibbonInventoryReports")
                    {
                        itemsToAdd = reports.Where(x => x.ReportCategory == ReportCategory.Inventory).ToList();
                    }
                    else if (args.Key == "RibbonUsersReports")
                    {
                        itemsToAdd = reports.Where(x => x.ReportCategory == ReportCategory.User).ToList();
                    }

                    foreach (ReportListItem button in itemsToAdd)
                    {
                        item = new ExtendedMenuItem(button.Text, priority, new EventHandler(RibbonReportButtonClicked));
                        item.Tag = button.ID;

                        args.AddMenu(item);
                        priority += 10;
                    }
                }
            }
        }

        #region System reports

        internal static void ShowFinancialReport(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.FinancialReport);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.FinancialReport), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowFinancialReportByEmployee(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.FinancialReportByEmployee);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.FinancialReportByEmployee), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowFinancialReportByStatement(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.FinancialReportByStatement);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.FinancialReportByStatement), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowItemSalesReport(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.ItemSales);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.ItemSales), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowGiftCardSalesReport(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.GiftCardSales);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.GiftCardSales), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowIssuedCreditMemosReport(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.IssuesCreditMemos);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.IssuedCreditMemos), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowSalesPerTerminalReport(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.SalesPerTerminal);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.SalesPerTerminal), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowSalesPerEmployeeReport(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.SalesPerEmployee);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.SalesPerEmployee), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowStockLevelReport(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.StockLevelReport);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.StockLevelReport), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void ShowInventoryBelowReorderpointReport(object sender, EventArgs args)
        {
            Guid reportID = new Guid(SystemReportConstants.InventoryBelowReorderPoint);

            if (Providers.ReportData.Exists(PluginEntry.DataModel, reportID))
            {
                ShowReport(reportID);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.InventoryBelowReorderpoint), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion System reports
    }
}

