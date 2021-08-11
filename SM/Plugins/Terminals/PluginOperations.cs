using System;
using System.Security;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Terminals.Properties;
using LSOne.ViewPlugins.Terminals.Views;
using ListView = System.Windows.Forms.ListView;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Terminals
{
    internal class PluginOperations
    {

        internal static void ShowTerminals(object sender, EventArgs e)
        {
            PluginEntry.Framework.ViewController.Add(new TerminalsView());
        }

        //internal static void ShowTerminalGroups(object sender, EventArgs e)
        //{
        //    PluginEntry.Framework.ViewController.Add(new TerminalsGroupView());
        //}

        public static void ShowTerminalOperationsAudit(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new OperationsAuditView());
        }

        public static bool DeleteTerminal(RecordIdentifier id)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
            {
                if (
                    QuestionDialog.Show(Properties.Resources.DeleteTerminalQuestion, Properties.Resources.DeleteTerminal) ==
                    DialogResult.Yes)
                {
                    Providers.TerminalData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "Terminal",id, null);
                    PluginEntry.Framework.SetDashboardItemDirty(new Guid("f58ece32-5f38-45ac-8c67-70b7a762fe8c")); // Inititial configuration dashboard item

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        public static void NewTerminal(object sender, EventArgs args)
        {
            NewTerminal(RecordIdentifier.Empty, "");
        }

        public static RecordIdentifier NewTerminal(RecordIdentifier storeID, string storeName)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
            {
                Dialogs.NewTerminalDialog dlg = new Dialogs.NewTerminalDialog(storeID, storeName);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.TerminalID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Terminal",
                                                                           dlg.TerminalID, null);

                    ShowTerminal(dlg.TerminalID, dlg.StoreID);
                }
            }

            return selectedID;
        }

        public static void ShowTerminal(RecordIdentifier terminalID, RecordIdentifier storeID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.TerminalView((string)terminalID, (string)storeID));
        }

        public static void EditTerminalStoreContextHandler(object sender, EventArgs args)
        {
            ListView lv = (ListView)PluginEntry.Framework.GetContextMenuContext();
            RecordIdentifier terminalID = (RecordIdentifier)lv.SelectedItems[0].Tag;

            ShowTerminal(terminalID[0], terminalID[1]);
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Resources.StoreSetup, "Store setup", null), 75);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Store setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.TerminalView))
                {
                    args.Add(new Item(Resources.Terminals, "Terminals", null), 50);
                }
            }
            else if (args.CategoryKey == "Security"  && PluginEntry.DataModel.HasPermission(Permission.ViewTerminalOperationsAudit))
            {
                args.Add(new Item(Resources.Auditing, "Auditing", null), 100);
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Store setup" && args.ItemKey == "Terminals")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
                {
                    args.Add(new ItemButton(Resources.NewTerminal, Resources.NewTerminalDescription, NewTerminal), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.TerminalView))
                {
                    args.Add(new ItemButton(Resources.Terminals, Resources.TerminalDescription, ShowTerminals), 20);
                    //args.Add(new ItemButton(Resources.TerminalGroups, Resources.TerminalGroupDescription, ShowTerminalGroups),30);
                }
            }
            else if (args.CategoryKey == "Security" && args.ItemKey == "Auditing" && PluginEntry.DataModel.HasPermission(Permission.ViewTerminalOperationsAudit))
            {
                args.Add(new ItemButton(Resources.TerminalOperationAudit, Resources.ViewTerminalOperationAuditDescription, ShowTerminalOperationsAudit), 500);
            }
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Store.Views.StoreView")
            {
                args.Add(
                    new TabControl.Tab(Resources.Terminals,
                                                ViewPages.StoreTerminalsPage.CreateInstance), 50);
            }
        }

        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;
            SearchListViewItem selectedItem;

            switch (args.Key)
            {

                case "RibbonNewStore":

                    if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
                    {

                        item = new ExtendedMenuItem(
                            Properties.Resources.RibbonNewTerminal + "...",
                            Properties.Resources.new_terminal_16, 
                            200,
                            NewTerminal);

                        args.AddMenu(item);
                    }
                    break;

                case "RibbonViewStoreTerminals":
                    if (PluginEntry.DataModel.HasPermission(Permission.TerminalView))
                    {
                        item = new ExtendedMenuItem(
                            Properties.Resources.Terminals,
                            Resources.terminal_16,
                            200,
                            PluginOperations.ShowTerminals);

                        args.AddMenu(item);
                    }
                    break;

                case "Insert":
                    if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
                    {

                        item = new ExtendedMenuItem(
                            Properties.Resources.NewTerminal + "...",
                            Properties.Resources.new_terminal_16,  
                            200,
                            PluginOperations.NewTerminal);

                        args.AddMenu(item);
                    }
                    break;

                case "AllSearchList":

                    if (args.Key != "AllSearchList" && args.Key != "StoreSearchList")
                    {
                        if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit))
                        {

                            item = new ExtendedMenuItem(
                                Properties.Resources.NewTerminal + "...",
                                Properties.Resources.new_terminal_16, 
                                100,
                                PluginOperations.NewTerminal);

                            args.AddMenu(item);
                        }
                    }

                    if (((ListView)args.Context).SelectedItems.Count > 0)
                    {
                        selectedItem = ((SearchListViewItem)((ListView)args.Context).SelectedItems[0]);

                        if (selectedItem.Key == "Terminal")
                        {
                            if (PluginEntry.DataModel.HasPermission(Permission.TerminalView))
                            {

                                item = new ExtendedMenuItem(
                                    Properties.Resources.EditTerminal + "...",
                                    ContextButtons.GetEditButtonImage(),
                                    50,
                                    PluginOperations.EditTerminalStoreContextHandler);

                                item.Default = true;

                                args.AddMenu(item);
                            }

                            if (PluginEntry.DataModel.HasPermission(Permission.TerminalEdit) && (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == selectedItem.ID.SecondaryID))
                            {

                                item = new ExtendedMenuItem(
                                    Properties.Resources.DeleteTerminal + "...",
                                    ContextButtons.GetRemoveButtonImage(),
                                    150,
                                    PluginOperations.DeleteTerminalContextHandler);

                                args.AddMenu(item);
                            }
                        }
                    }

                    break;


            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Resources.Sites, "Sites"), 700);
            args.Add(new Page(Resources.Tools, "Tools"), 1000);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sites")
            {
                args.Add(new PageCategory(Resources.SitesTerminals, "SiteTerminal"), 100);
            }

            if (args.PageKey == "Tools")
            {
                args.Add(new PageCategory(Resources.TerminalLog, "TerminalLog"), 100);
            }
        }


        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
           if (args.PageKey == "Sites" && args.CategoryKey == "SiteTerminal")
            {

                if (PluginEntry.DataModel.HasPermission(Permission.StoreView))
                {
                    args.Add(new CategoryItem(
                                Resources.Terminals,
                                Resources.Terminals,
                                Resources.TerminalTooltipDescription,
                                CategoryItem.CategoryItemFlags.Button,
                                null,
                                Resources.terminal_32,
                                ShowTerminals,
                                "Terminals"), 20);
                }

            }

            if (args.PageKey == "Tools" && args.CategoryKey == "AuditLog")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ViewTerminalOperationsAudit))
                {
                    args.Add(new CategoryItem(
                                Resources.TerminalLog,
                                Resources.TerminalOperationLog,
                                Resources.ViewTerminalOperationAuditDescription,
                                CategoryItem.CategoryItemFlags.Button,
                                Resources.terminal_16,
                                null,
                                ShowTerminalOperationsAudit,
                                "ViewTerminalLog"), 30);
                }
            }

        }

        public static void DeleteTerminalContextHandler(object sender, EventArgs args)
        {
            ListView lv = (ListView)PluginEntry.Framework.GetContextMenuContext();

            DeleteTerminal((RecordIdentifier)lv.SelectedItems[0].Tag);
        }

        public static void AddSearchHandler(object sender, SearchBarConstructionArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.TerminalView))
            {
                args.AddItem(Resources.Terminals, PluginEntry.TerminalImageIndex, new TerminalSearchPanelFactory(), 110);
            }
        }
    }
}
