using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.CentralSuspensions.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.CentralSuspensions
{
    internal class PluginOperations
    {

        private static bool TestSiteService()
        {
            bool serviceConfigured = false;
            bool serviceActivce = false;

            if (PluginEntry.DataModel.SiteServiceAddress == "")
            {
                MessageDialog.Show(Properties.Resources.NoStoreServerIsSetUp);
            }
            else
            {
                serviceConfigured = true;
            }



            ISiteServiceService service =
                (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            ConnectionEnum result = service.TestConnection(PluginEntry.DataModel,
                                                           PluginEntry.DataModel.SiteServiceAddress,
                                                           PluginEntry.DataModel.SiteServicePortNumber);

            PluginEntry.Framework.ViewController.CurrentView.HideProgress();


            if (result != ConnectionEnum.Success)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
            }
            else
            {
                serviceActivce = true;
            }

            bool serviceValid = serviceActivce && serviceConfigured;
            if (!serviceValid)
            {
                IPlugin plugin;

                plugin = PluginEntry.Framework.FindImplementor(null, "CanViewAdministrationTab", null);

                if (plugin != null)
                {
                    plugin.Message(null, "ViewSiteServiceTab", null);
                }
            }

            return serviceValid;
        }
        public static void ShowSuspendedTransactionsTypeView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SuspendedTransactionsTypeView());
        }

        public static void ShowSuspendedTransactionsView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.SuspendedTransactionsView());
            }
        }

        public static void ShowSuspendedTransactionType(RecordIdentifier id)
        {          
            PluginEntry.Framework.ViewController.Add(new Views.SuspendedTransactionTypeView(id));            
        }

        public static void NewSuspensionType(object sender, EventArgs args)
        {
           RecordIdentifier selectedID = RecordIdentifier.Empty; 
           Dialogs.NewSuspensionsTypeDialog dlg = new Dialogs.NewSuspensionsTypeDialog();

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                selectedID = dlg.SuspensionTypeID;

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "SuspensionsType", selectedID, null);
                ShowSuspendedTransactionType(selectedID);
            }  
        }

        public static void NewSuspensionTypeAdditionalInfo(RecordIdentifier suspendedtransactionID)
       {
           RecordIdentifier selectedID = RecordIdentifier.Empty;
           Dialogs.SuspensionFieldDialog dlg = new Dialogs.SuspensionFieldDialog(suspendedtransactionID);

           if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
           {
               selectedID = dlg.SuspensionFieldID;
               PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "SuspensionsTypeAdditionalInfo", selectedID, null);               
           }

       }

        public static void EditSuspensionAdditionalInfo(RecordIdentifier suspensionfieldID, RecordIdentifier suspendedtransactionID)
       {
           RecordIdentifier selectedID = suspensionfieldID;
           Dialogs.SuspensionFieldDialog dlg = new Dialogs.SuspensionFieldDialog(suspensionfieldID, suspendedtransactionID);

           if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
           {
               selectedID = dlg.SuspensionFieldID;

               PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "SuspensionsTypeAdditionalInfo", selectedID,null);
           }
       }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.Retail, "Retail", null), 50);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
                {
                    args.Add(new Item(Properties.Resources.Transactions, "Receipts", null), 110);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Retail" && args.ItemKey == "Receipts")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
                {

                    args.Add(new ItemButton(Properties.Resources.SuspendedTransactions, Properties.Resources.CentralSuspendedTransactions, ShowSuspendedTransactionsView), 150);
                    args.Add(new ItemButton(Properties.Resources.SuspensionTransactionTypes, Properties.Resources.CentralSuspensionTypeDescription, ShowSuspendedTransactionsTypeView), 200);
                    
                   
                }
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Sales, "Sales"), 300);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sales")
            {
                args.Add(new PageCategory(Properties.Resources.Transactions, "Transactions"), 100);
                
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sales" && args.CategoryKey == "Transactions")
            {

                if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
                {
                    args.Add(new CategoryItem(
                    Properties.Resources.Suspended,
                    Properties.Resources.SuspendedTransactions,
                    Properties.Resources.SuspendedTransactionsTooltipDescription,
                    CategoryItem.CategoryItemFlags.DropDown | CategoryItem.CategoryItemFlags.BeginOfGroup,
                    null,
                    Properties.Resources.suspended_32,
                    null,
                    "SuspendedTransaction"), 20);
                }

                /*if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
                {
                    args.Add(new CategoryItem(
                         Properties.Resources.Suspended,
                         Properties.Resources.SuspendedTransactions,
                         Properties.Resources.CentralSuspendedTransactions,
                         CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
                         null,
                         null,
                         ShowSuspendedTransactionsView,
                         "SuspendedTransaction"), 20);

                    args.Add(new CategoryItem(
                           Properties.Resources.SuspensionsTypes,
                           Properties.Resources.SuspensionTransactionTypes,
                           Properties.Resources.CentralSuspensionTypeDescription,
                           CategoryItem.CategoryItemFlags.Button,
                           null,
                           null,
                           ShowSuspendedTransactionsTypeView,
                           "SuspenedTypes"), 30);
                }*/
            }
        }

        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;

            switch (args.Key)
            {
                // We dont need to check permissions here since thats been done on the parent menu allready
                case "RibbonSuspendedTransaction":
                    item = new ExtendedMenuItem(
                        Properties.Resources.SuspendedTransactions,
                        null,
                        10,
                        ShowSuspendedTransactionsView);

                    args.AddMenu(item);

                    item = new ExtendedMenuItem(
                        Properties.Resources.SuspensionsTypes,
                        null,
                        20,
                        ShowSuspendedTransactionsTypeView);

                    args.AddMenu(item);

                    break;
            }
        }

        public static bool DeleteSuspensionAdditionalInfo(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteSuspendedTypeQuestion, Properties.Resources.DeleteSuspendedType) == DialogResult.Yes)
                {

                    Providers.SuspensionTypeAdditionalInfoData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "SuspensionsTypeAdditionalInfo", (string)id, null);

                    return true;
                }
            }
            return false;
        }

        public static bool DeleteSuspensionsAdditionalInfo(List<IDataEntity> suspensionAdditionalInfotypes)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteSupensionAdditionalInfoQuestion, Properties.Resources.DeleteSupensionAdditionalInfo) == DialogResult.Yes)
                {

                    foreach (var suspensiontype in suspensionAdditionalInfotypes)
                    {
                        Providers.SuspensionTypeAdditionalInfoData.Delete(PluginEntry.DataModel, suspensiontype.ID);
                    }
                    
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "SuspensionsType", null, suspensionAdditionalInfotypes);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteSuspensionType(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteSuspendedTypeQuestion, Properties.Resources.DeleteSuspendedType) == DialogResult.Yes)
                {

                    if (Providers.SuspendedTransactionTypeData.InUse(PluginEntry.DataModel, id))
                    {
                        MessageDialog.Show(Properties.Resources.CannotDeleteSuspensionType);
                        return true;
                    }
                    Providers.SuspendedTransactionTypeData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "SuspensionsType", id, null);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteSuspensionTypes(List<IDataEntity> suspensiontypes)
        {
            PluginEntry.Framework.SuspendSearchBarClosing();

            

            if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
            {
                
                if (QuestionDialog.Show(Properties.Resources.DeleteSuspensionTypesQuestion, Properties.Resources.DeleteSuspensionTypes) == DialogResult.Yes)
                {

                    foreach (var item in suspensiontypes)
                    {
                        if (Providers.SuspendedTransactionTypeData.InUse(PluginEntry.DataModel,item.ID))
                        {
                            MessageDialog.Show(Properties.Resources.CannotDeleteSuspensionTypes.Replace("#1", item.Text));
                            return true;
                        }
                        
                        
                    }
                    foreach (var suspensiontype in suspensiontypes)
                    {
                        Providers.SuspendedTransactionTypeData.Delete(PluginEntry.DataModel, suspensiontype.ID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "SuspensionsType", null, suspensiontypes);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteSuspendedTransaction(RecordIdentifier id )
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
            {
                if(QuestionDialog.Show(Properties.Resources.DeleteSuspendedTransactionQuestion ,Properties.Resources.DeleteSuspendedTransaction) == DialogResult.Yes)
                {
                    Providers.SuspendedTransactionData.Delete(PluginEntry.DataModel, id);
                    Providers.SuspendedTransactionAnswerData.DeleteForTransaction(PluginEntry.DataModel, id);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null,DataEntityChangeType.Delete, "SuspendedTransaction",id, null);
                    return true;
                }
            }
            return false;
        }

        public static void DeleteSuspendedTransactions(List<IDataEntity> suspendedtransactions)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteSuspendedTransactionsQuestion, Properties.Resources.DeleteSuspendedTransactions) == DialogResult.Yes)
                {
                    foreach (var suspendedtransaction in suspendedtransactions)
                    {
                        Providers.SuspendedTransactionData.Delete(PluginEntry.DataModel, suspendedtransaction.ID);
                        Providers.SuspendedTransactionAnswerData.DeleteForTransaction(PluginEntry.DataModel, suspendedtransaction.ID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "SuspendedTransaction", null, suspendedtransactions);
                }
            }
        } 
        
       

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageCentralSuspensions))
            {
                if (args.ContextName == "LSOne.ViewPlugins.Store.Views.StoreView")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageSuspensionSettings))
                    {
                        args.Add(
                            new TabControl.Tab(Properties.Resources.SuspendedTransaction,
                                ViewPages.StoreSuspendTransactionsPage.CreateInstance), 550);
                    }
                }

                if (args.ContextName == "LSOne.ViewPlugins.Terminals.Views.TerminalView")
                {
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageSuspensionSettings))
                    {
                        args.Add(
                            new TabControl.Tab(Properties.Resources.SuspendedTransaction,
                                ViewPages.TerminalSuspendTransactionsPage.CreateInstance), 500);
                    }
                }
            }
        }

        /// <summary>
        /// Shows the <see cref="NewSuspensionsTypeDialog"/> without then showing the SuspensionType view after closing
        /// </summary>
        /// <param name="sender">The source of the method call</param>
        /// <param name="args">Contains additional parameters</param>
        public static void NewSuspensionTypeHandler(object sender, PluginOperationArguments args)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;
            Dialogs.NewSuspensionsTypeDialog dlg = new Dialogs.NewSuspensionsTypeDialog();

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                selectedID = dlg.SuspensionTypeID;

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "SuspensionsType", selectedID, null);
            }
        }
    }
}
