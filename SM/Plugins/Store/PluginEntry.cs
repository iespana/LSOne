using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Store.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.ViewCore.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.StoreManagement.Validity;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.ViewPlugins.Store.ViewPages;
using ListView = System.Windows.Forms.ListView;

namespace LSOne.ViewPlugins.Store
{
    public class PluginEntry : IPlugin, IPluginDashboardProvider
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        internal static StoreSearchPanelFactory storeSearchProvider = null;

        internal static int StoreImageIndex;
        internal static int TerminalImageIndex;
        //internal static DashboardItem InitialConfigurationDashboardItem;
        internal static Guid InitialConfigurationDashboardID = new Guid("f58ece32-5f38-45ac-8c67-70b7a762fe8c");
        internal static Guid LocationDashboardID = new Guid("dc2ddc48-56e6-4943-a041-cfd0ccdbedee");

        private void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;
            SearchListViewItem selectedItem;

            switch (args.Key)
            {
                case "CurrencyList":
                    args.AddSeparator(1000);

                    item = new ExtendedMenuItem(
                        Resources.EditCompanyCurrency,
                        null,
                        1002,
                        PluginOperations.ShowCompanyInfo
                        );

                    args.AddMenu(item);
                    break;

                case "RibbonNewStore":
                    if (DataModel.HasPermission(Permission.StoreEdit) && PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty)
                    {

                        item = new ExtendedMenuItem(
                            Resources.RibbonNewStore + "...",
                            Resources.new_store_16,
                            100,
                            PluginOperations.NewStore);

                        args.AddMenu(item);
                    }
                    break;
                    
                case "RibbonViewStoreTerminals":
                    if (PluginEntry.DataModel.HasPermission(Permission.StoreView))
                    {
                        item = new ExtendedMenuItem(
                            Resources.Stores,
                            Resources.store_16,
                            100,
                            PluginOperations.ShowStoresListView);

                        args.AddMenu(item);
                    }
                    break;

                case "Insert":
                    if (DataModel.HasPermission(Permission.StoreEdit) && PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty)
                    {

                        item = new ExtendedMenuItem(
                            Resources.NewStore + "...",
                            Resources.new_store_16,
                            100,
                            PluginOperations.NewStore);

                        args.AddMenu(item);
                    }

                    //if (DataModel.HasPermission(Permission.TerminalEdit))
                    //{

                    //    item = new ExtendedMenuItem(
                    //        Properties.Resources.NewTerminal + "...",
                    //        Properties.Resources.NewTerminalImage,
                    //        200,
                    //        new EventHandler(PluginOperations.NewTerminal));

                    //    args.AddMenu(item);
                    //}
                    break;

                case "TerminalSearchList":
                case "StoreSearchList":
                case "AllSearchList":
                    if (args.Key != "AllSearchList" && args.Key != "TerminalSearchList")
                    {
                        if (DataModel.HasPermission(Permission.StoreEdit))
                        {

                            item = new ExtendedMenuItem(
                                Resources.NewStore + "...",
                                ContextButtons.GetAddButtonImage(),
                                100,
                                PluginOperations.NewStore);

                            args.AddMenu(item);
                        }
                    }
                    

                    
                    if (((ListView)args.Context).SelectedItems.Count > 0)
                    {
                        selectedItem = ((SearchListViewItem)((ListView)args.Context).SelectedItems[0]);

                        if (selectedItem.Key == "Store")
                        {

                            if (DataModel.HasPermission(Permission.StoreView))
                            {

                                item = new ExtendedMenuItem(
                                    Resources.EditStore + "...",
                                    ContextButtons.GetEditButtonImage(),
                                    50,
                                    PluginOperations.EditStoreStoreContextHandler);

                                item.Default = true;

                                args.AddMenu(item);
                            }


                            if (DataModel.HasPermission(Permission.StoreEdit) && (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty))
                            {

                                item = new ExtendedMenuItem(
                                    Resources.DeleteStore + "...",
                                    ContextButtons.GetRemoveButtonImage(),
                                    150,
                                    PluginOperations.DeleteStoreContextHandler);

                                args.AddMenu(item);
                            }
                        }
                        else if (selectedItem.Key == "Terminal")
                        {
                            


                            if (DataModel.HasPermission(Permission.TerminalEdit) && (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == selectedItem.ID.SecondaryID))
                            {

                                item = new ExtendedMenuItem(
                                    Resources.DeleteTerminal + "...",
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

        #region IPlugin Members

        public string Description
        {
            get { return Resources.StoreManagement; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "CanEditStore": return PluginEntry.DataModel.HasPermission(Permission.StoreView);
                case "GetStoreHost": return true;
                case "ViewStores": return PluginEntry.DataModel.HasPermission(Permission.StoreView);
                case "CanCreateStore": return PluginEntry.DataModel.HasPermission(Permission.StoreEdit);
                case "CanEditCompanyInfo": return true;
                case "SelectLocation": return true;
                case "DisplayStoreStatementSettings": return true;
 

                default: return false;
            }
        }

        public object Message(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "EditStore":
                    if (parameters is RecordIdentifier)
                    {
                        PluginOperations.ShowStore((RecordIdentifier)parameters);
                    }
                    else
                    {
                        PluginOperations.ShowStore((RecordIdentifier)((object[])parameters)[0], (string)((object[])parameters)[1]);
                    }
                    
                    break;
                case "CanCreateStore": 
                    PluginOperations.ShowStoresListView(sender, EventArgs.Empty);
                    break;
                case "NewStore": 
                    PluginOperations.NewStoreDialog();
                    break;
                case "StoreImageIndex": 
                    return StoreImageIndex;
                case "TerminalImageIndex":
                    return TerminalImageIndex;
                case "ViewStores":
                    PluginOperations.ShowStoresListView(sender, EventArgs.Empty);
                    break;
                case "EditCompanyInfo":
                    PluginOperations.ShowCompanyInfo(null, EventArgs.Empty);
                    break;

                case "SelectLocation":
                    return PluginOperations.SelectLocation();    
                
            }

            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            ImageList iml;
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // Register Icons that this plugin uses to the framework
            // -------------------------------------------------
            iml = frameworkCallbacks.GetImageList();

            iml.Images.Add(Properties.Resources.store_16);
            StoreImageIndex = iml.Images.Count - 1;

            iml.Images.Add(Properties.Resources.terminal_16);
            TerminalImageIndex = iml.Images.Count - 1;

            frameworkCallbacks.AddLoginHandler(PluginOperations.OnLogin);

            frameworkCallbacks.AddMenuConstructionConstructionHandler(ConstructMenus);

            frameworkCallbacks.AddSearchBarConstructionHandler(PluginOperations.AddSearchHandler);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;
        }
        
       
        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Resources.NewStore, PluginOperations.NewStore, Permission.StoreEdit);
            operations.AddOperation(Resources.ViewAllStores, PluginOperations.ShowStoresListView);
            operations.AddOperation(Resources.CompanyInformation, PluginOperations.ShowCompanyInfo);
        }

        #endregion

        public void LoadDashboardItem(IConnectionManager threadedEntry, ViewCore.Controls.DashboardItem item)
        {
            // In case if the plugin is registering more than one then we check which one it is though we will never get item from other plugin here.
            if(item.ID == LocationDashboardID)
            {
                item.State = DashboardItem.ItemStateEnum.Info;

                if(threadedEntry.IsHeadOffice)
                {
                    item.InformationText = Properties.Resources.HeadOffice + ". " + Properties.Resources.InventoryOperationsWarning;
                }
                else
                {
                    DataEntity storeEntity = Providers.StoreData.GetStoreEntity(threadedEntry, threadedEntry.CurrentStoreID);

                    item.InformationText = (storeEntity != null) ? storeEntity.Text : (string)threadedEntry.CurrentStoreID;
                }

                if(PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                {
                    item.SetButton(0, Properties.Resources.Change, PluginOperations.ChangeCurrentStore);
                }
            }
            else if (item.ID == InitialConfigurationDashboardID)
            {
                bool hasCompanyCurrency = Providers.CompanyInfoData.HasCompanyCurrency(threadedEntry);

                if(!hasCompanyCurrency)
                {
                    item.State = DashboardItem.ItemStateEnum.Error;

                    item.InformationText = Properties.Resources.CompanyCurrencyIsMissing;

                    item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowCompanyInfo);
                }
                else
                {
                    bool hasError = false;
                    List<StoreValidity> storeValidities = Providers.StoreData.CheckStoreValidity(threadedEntry);

                    if (storeValidities.Count == 0)
                    {
                        item.State = DashboardItem.ItemStateEnum.Error;
                        item.InformationText = Properties.Resources.NoStoreExists;

                        item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowStoresListView);

                        hasError = true;
                    }
                    else
                    {
                        foreach (StoreValidity storeValidity in storeValidities)
                        {
                            if (!storeValidity.TerminalExists)
                            {
                                item.State = DashboardItem.ItemStateEnum.Error;

                                item.InformationText = Properties.Resources.StoreNamedHasNoTerminal.Replace("#1", storeValidity.Text);

                                item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowStoreHandler, storeValidity.ID);

                                hasError = true;
                                break;
                            }
                            else if (!storeValidity.ButtonLayoutExists)
                            {
                                item.State = DashboardItem.ItemStateEnum.Error;

                                item.InformationText = Properties.Resources.StoreNamedHasNoTouchButtons.Replace("#1", storeValidity.Text);

                                item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowStoreHandler, new RecordIdentifier(storeValidity.ID, "SettingsPage"));

                                hasError = true;
                                break;
                            }
                            else if (!storeValidity.FunctionalityProfileExist)
                            {
                                item.State = DashboardItem.ItemStateEnum.Error;

                                item.InformationText = Properties.Resources.StoreNamedHasNoFunctionalityProfile.Replace("#1", storeValidity.Text);

                                item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowStoreHandler, new RecordIdentifier(storeValidity.ID, "SettingsPage"));

                                hasError = true;
                                break;
                            }
                            else if (!storeValidity.PaymentTypesExists)
                            {
                                item.State = DashboardItem.ItemStateEnum.Error;
                                item.InformationText = Properties.Resources.StoreNamedHasNoPaymentTypes.Replace("#1", storeValidity.Text);
                                
                                item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowStoreHandler, new RecordIdentifier(storeValidity.ID, "AllowedPaymentTypesPage"));

                                hasError = true;
                                break;
                            }
                            else if (!storeValidity.PriceSettingsMatched)
                            {
                                item.State = DashboardItem.ItemStateEnum.Error;
                                item.InformationText = Properties.Resources.StoreNamedHasMismatchedPriceConfiguration.Replace("#1", storeValidity.Text);

                                item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowStoreHandler, new RecordIdentifier(storeValidity.ID, "SettingsPage"));

                                hasError = true;
                                break;
                            }
                        }
                    }

                    if(!hasError)
                    {
                        List<TerminalValidity> terminalValidities = Providers.TerminalData.CheckTerminalValidity(threadedEntry);

                        foreach (TerminalValidity terminalValidity in terminalValidities)
                        {
                            if (!terminalValidity.VisualProfileExists)
                            {
                                item.State = DashboardItem.ItemStateEnum.Error;

                                item.InformationText = Properties.Resources.TerminalNamedHasNoVisualProfile.Replace("#1", terminalValidity.Text);

                                if (PluginEntry.Framework.FindImplementor(this, "ViewTerminal", null) != null)
                                {
                                    item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowTerminalHandler, new RecordIdentifier(terminalValidity.ID, terminalValidity.StoreID));
                                }
                                else
                                {
                                    // We have no permission to resolve this or the Terminal plugin is not installed so we display no button
                                    item.SetButton(0, "", null);
                                }

                                hasError = true;
                                break;
                            }

                            if (!terminalValidity.HardwareProfileExists)
                            {
                                item.State = DashboardItem.ItemStateEnum.Error;

                                item.InformationText = Properties.Resources.TerminalNamedHasNoHardwareProfile.Replace("#1", terminalValidity.Text);

                                if (PluginEntry.Framework.FindImplementor(this, "ViewTerminal", null) != null)
                                {
                                    item.SetButton(0, Properties.Resources.Resolve, PluginOperations.ShowTerminalHandler, new RecordIdentifier(terminalValidity.ID, terminalValidity.StoreID));
                                }
                                else
                                {
                                    // We have no permission to resolve this or the Terminal plugin is not installed so we display no button
                                    item.SetButton(0, "", null);
                                }

                                hasError = true;
                                break;
                            }
                        }
                    }

                    if (!hasError)
                    {
                        item.State = DashboardItem.ItemStateEnum.Passed;

                        item.InformationText = Properties.Resources.InitialConfigurationLooksComplete;

                        item.SetButton(0, "", null);
                    }
                }
            }
        }

        public void RegisterDashBoardItems(DashboardItemArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.StoreView))
            {
                DashboardItem initialConfigurationDashboardItem = new DashboardItem(InitialConfigurationDashboardID, Properties.Resources.InitialConfiguration, true, 60); // 1 hour refresh interval

                args.Add(new DashboardItemPluginResolver(initialConfigurationDashboardItem, this), 20); // Priority 20
            }

            DashboardItem locationDashboardItem = new DashboardItem(LocationDashboardID, Properties.Resources.CurrentStore, true, 0); //0 = no refresh

            args.Add(new DashboardItemPluginResolver(locationDashboardItem, this), 15); 
        }
    }
}
