using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Controls;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.DataProviders;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewPlugins.RetailItemAssemblies.Dialogs;
using System.ComponentModel;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Profiles;

namespace LSOne.ViewPlugins.RetailItemAssemblies.ViewPages
{
    public partial class RetailItemAssembliesPage : UserControl, ITabView
    {
        private WeakReference owner;
        private bool readOnly;
        private RetailItem item;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("27A43547-10EF-4224-9AE7-9733B0018B4E");
        private DecimalLimit priceLimiter;
        private RetailItemAssembly selectedAssembly;
        private RecordIdentifier selectedAssemblyID;
        private RecordIdentifier selectedComponentID;
        private WeakReference itemViewer;

        public RetailItemAssembliesPage(TabControl owner) : this()
        {
            this.owner = new WeakReference(owner);
            readOnly = ((ViewBase)owner.Parent.Parent).ReadOnly;

            IPlugin itemPlugin = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);
            itemViewer = (itemPlugin != null) ? new WeakReference(itemPlugin) : null;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new RetailItemAssembliesPage((TabControl)sender);
        }

        public RetailItemAssembliesPage()
        {
            InitializeComponent();
            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            lvAssemblies.SetSortColumn(1, true);
            lvComponents.SetSortColumn(1, true);

            lvAssemblies.ContextMenuStrip = new ContextMenuStrip();
            lvAssemblies.ContextMenuStrip.Opening += lvAssemblies_Opening;

            lvComponents.ContextMenuStrip = new ContextMenuStrip();
            lvComponents.ContextMenuStrip.Opening += lvComponents_Opening;
        }

        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            btnsAssemblies.EditButtonEnabled = btnsAssemblies.RemoveButtonEnabled =
            btnsComponents.AddButtonEnabled = btnsComponents.EditButtonEnabled = btnsComponents.RemoveButtonEnabled = false;

            btnEnable.Enabled = false;

            LoadList();
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "RetailItemAssemblyComponent")
            {
                selectedComponentID = changeIdentifier;
                btnEnable.Enabled = true;
                LoadList();
                LoadComponentList();
            }
            else if(objectName == "RetailItemAssembly")
            {
                selectedAssemblyID = changeIdentifier;
                LoadList();
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private RetailItemAssemblySearchFilter GetSearchFilter()
        {
            RetailItemAssemblySearchFilter filter = new RetailItemAssemblySearchFilter();
            filter.ItemID = item.ID;

            foreach (SearchParameterResult result in searchBar.SearchParameterResults)
            {
                switch (result.ParameterKey)
                {
                    case "Store":
                        filter.StoreID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        filter.AllStores = RecordIdentifier.IsEmptyOrNull(filter.StoreID);
                        break;
                    case "Status":
                        switch (result.ComboSelectedIndex)
                        {
                            case 1: filter.AssemblyStatus = RetailItemAssemblyStatus.Disabled; break;
                            case 2: filter.AssemblyStatus = RetailItemAssemblyStatus.EnabledNotStarted; break;
                            case 3: filter.AssemblyStatus = RetailItemAssemblyStatus.Enabled; break;
                            case 4: filter.AssemblyStatus = RetailItemAssemblyStatus.Archived; break;
                            default:
                                break;
                        }
                        break;
                    case "StartingDate":
                        filter.StartingDateFrom = result.Date.Checked ? result.Date.Value.Date : (DateTime?)null;
                        filter.StartingDateTo = result.DateTo.Checked ? result.DateTo.Value.Date : (DateTime?)null;
                        break;
                }
            }

            return filter;
        }

        private void LoadList()
        {
            RecordIdentifier selectedID = selectedAssemblyID;
            lvAssemblies.ClearRows();
            selectedAssemblyID = selectedID;

            List<RetailItemAssembly> retailItemAssemblies = Providers.RetailItemAssemblyData.Search(PluginEntry.DataModel, GetSearchFilter());

            foreach(var itemAssembly in PluginOperations.SortList(retailItemAssemblies, (RetailItemAssemblySort)lvAssemblies.Columns.IndexOf(lvAssemblies.SortColumn), lvAssemblies.SortedAscending))
            {
                Bitmap statusImage = null;

                switch (itemAssembly.Status)
                {
                    case RetailItemAssemblyStatus.Disabled:
                        statusImage = Properties.Resources.dot_yellow_16;
                        break;
                    case RetailItemAssemblyStatus.EnabledNotStarted:
                        statusImage = Properties.Resources.dot_green_16;
                        break;
                    case RetailItemAssemblyStatus.Enabled:
                        statusImage = Properties.Resources.dot_finished_16;
                        break;
                    case RetailItemAssemblyStatus.Archived:
                        statusImage = Properties.Resources.dot_red_16;
                        break;
                }

                Row row = new Row();
                row.AddCell(new ExtendedCell(string.Empty, statusImage));
                row.AddText(itemAssembly.Text);
                row.AddText(itemAssembly.StoreName == "" ? Properties.Resources.All : itemAssembly.StoreName);
                row.AddText(itemAssembly.StartingDate.ToShortDateString());
                row.AddText(itemAssembly.TotalCost.FormatWithLimits(priceLimiter));
                row.AddText(itemAssembly.GetDisplayPrice().FormatWithLimits(priceLimiter));
                row.AddText(itemAssembly.Margin.FormatWithLimits(priceLimiter));
                row.AddText(itemAssembly.GetDisplayWithComponentsString());
                row.AddText(PluginOperations.GetSendToKdsDisplayName(itemAssembly.SendAssemblyComponentsToKds));
                row.AddText(itemAssembly.CalculatePriceFromComponents ? Properties.Resources.Yes : Properties.Resources.No);

                row.Tag = itemAssembly;
                lvAssemblies.AddRow(row);

                if (selectedAssemblyID == itemAssembly.ID)
                {
                    lvAssemblies.Selection.Set(lvAssemblies.RowCount - 1);
                }
            }

            lvAssemblies.AutoSizeColumns();
        }

        private void LoadComponentList()
        {
            RecordIdentifier selectedID = selectedComponentID;
            lvComponents.ClearRows();
            selectedComponentID = selectedID;

            if(lvAssemblies.Selection.Count == 1)
            {
                List<RetailItemAssemblyComponent> retailItemAssemblyComponents = Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, ((RetailItemAssembly)lvAssemblies.Selection[0].Tag).ID);

                foreach (var itemAssemblyComponent in PluginOperations.SortList(retailItemAssemblyComponents, (RetailItemAssemblyComponentSort)lvComponents.Columns.IndexOf(lvComponents.SortColumn), lvComponents.SortedAscending))
                {
                    Row row = new Row();
                    row.AddText(itemAssemblyComponent.ItemID.StringValue);
                    row.AddText(itemAssemblyComponent.ItemName);
                    row.AddText(itemAssemblyComponent.VariantName);
                    row.AddText(itemAssemblyComponent.Quantity.FormatWithLimits(Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, itemAssemblyComponent.UnitName))));
                    row.AddText(itemAssemblyComponent.UnitName);
                    row.AddText(itemAssemblyComponent.CostPerUnit.FormatWithLimits(priceLimiter));
                    row.AddText(itemAssemblyComponent.GetTotalCost().FormatWithLimits(priceLimiter));
                    row.Tag = itemAssemblyComponent;

                    lvComponents.AddRow(row);

                    if (selectedComponentID == itemAssemblyComponent.ID)
                    {
                        lvComponents.Selection.Set(lvComponents.RowCount - 1);
                    }
                }
            }

            lvComponents.AutoSizeColumns();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            if (lvAssemblies.Selection.Count == 1)
            {
                selectedAssembly = (RetailItemAssembly)lvAssemblies.Selection[0].Tag;

                if (selectedAssembly.Enabled)
                {
                    if (Providers.TransactionData.AssemblyItemIsActive(PluginEntry.DataModel, selectedAssembly.ID) || !DisableAllowed(selectedAssembly.ID))
                    {
                        MessageDialog.Show(Properties.Resources.AssemblyAlreadySoldOrSuspendedOnPOS, MessageBoxIcon.Warning);
                        btnEnable.Enabled = false;
                        return;
                    }
                    else
                    {
                        Providers.RetailItemAssemblyData.SetEnabled(PluginEntry.DataModel, selectedAssembly.ID, !selectedAssembly.Enabled);
                        LoadList();
                        LoadComponentList();
                    }
                }
                else
                {
                    Providers.RetailItemAssemblyData.SetEnabled(PluginEntry.DataModel, selectedAssembly.ID, !selectedAssembly.Enabled);
                    LoadList();
                    LoadComponentList();
                }
            }
        }

        private void btnsAssemblies_AddButtonClicked(object sender, EventArgs e)
        {
            DataEntity defaultSalesUnit = (DataEntity)((TabControl)this.owner.Target).SendViewPageMessage(this, "GetItemSalesUnit", null, out bool handledUnit);
            object salesPriceData = ((TabControl)this.owner.Target).SendViewPageMessage(this, "GetItemSalesPrice", null, out bool handledPrice);

            decimal salesPrice = 0;

            if(salesPriceData == null)
            {
                bool pricesAreWithTax = false;
                RecordIdentifier defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);

                if (!RecordIdentifier.IsEmptyOrNull(defaultStoreID))
                {
                    pricesAreWithTax = Providers.StoreData.GetPriceWithTaxForStore(PluginEntry.DataModel, defaultStoreID);
                }

                salesPrice = pricesAreWithTax ? item.SalesPriceIncludingTax : item.SalesPrice;
            }
            else
            {
                salesPrice = (decimal)salesPriceData;
            }

            if (RecordIdentifier.IsEmptyOrNull(defaultSalesUnit.ID))
            {
                MessageDialog.Show(Properties.Resources.SalesUnitMustBeSelected);
                return;
            }

            using (NewEditAssemblyDialog dlg = new NewEditAssemblyDialog(item.ID, true, Providers.RetailItemAssemblyData.GetList(PluginEntry.DataModel, item.ID), false, salesPrice, defaultSalesUnit))
            {
                dlg.ShowDialog();
            }
        }

        private void btnsAssemblies_EditButtonClicked(object sender, EventArgs e)
        {
            if(lvAssemblies.Selection.Count == 1)
            {
                DataEntity defaultSalesUnit = (DataEntity)((TabControl)this.owner.Target).SendViewPageMessage(this, "GetItemSalesUnit", null, out bool handledUnit);

                if (RecordIdentifier.IsEmptyOrNull(defaultSalesUnit.ID))
                {
                    MessageDialog.Show(Properties.Resources.SalesUnitMustBeSelected);
                    return;
                }

                using (NewEditAssemblyDialog dlg = new NewEditAssemblyDialog((RetailItemAssembly)lvAssemblies.Selection[0].Tag, true, defaultSalesUnit))
                {
                    dlg.ShowDialog();
                }
            }
        }

        private void btnsAssemblies_RemoveButtonClicked(object sender, EventArgs e)
        {
            if(lvAssemblies.Selection.Count == 1)
            {
                Providers.RetailItemAssemblyData.Delete(PluginEntry.DataModel, ((RetailItemAssembly)lvAssemblies.Selection[0].Tag).ID);
                LoadList();
                LoadComponentList();
            }
        }

        private void btnsComponents_AddButtonClicked(object sender, EventArgs e)
        {
            using (NewEditAssemblyComponentDialog dlg = new NewEditAssemblyComponentDialog(selectedAssemblyID, null, true, item.ID, selectedAssembly.StoreID))
            {
                dlg.ShowDialog();
            }
        }

        private void btnsComponents_EditButtonClicked(object sender, EventArgs e)
        {
            using (NewEditAssemblyComponentDialog dlg = new NewEditAssemblyComponentDialog(
                (RetailItemAssemblyComponent)lvComponents.Selection[0].Tag,
                null,
                true, 
                item.ID,
                selectedAssembly.StoreID
            ))
            {
                dlg.ShowDialog();
            }
        }

        private void btnsComponents_RemoveButtonClicked(object sender, EventArgs e)
        {
            Providers.RetailItemAssemblyComponentData.Delete(PluginEntry.DataModel, selectedComponentID);
            selectedComponentID = null;
            btnEnable.Enabled = Providers.RetailItemAssemblyComponentData.HasComponents(PluginEntry.DataModel, selectedAssembly.ID);
            LoadComponentList();
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadList();
            LoadComponentList();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusList = new List<object>();
            statusList.Add(Properties.Resources.All);
            statusList.Add(Properties.Resources.Disabled);
            statusList.Add(Properties.Resources.EnabledNotStarted);
            statusList.Add(Properties.Resources.Enabled);
            statusList.Add(Properties.Resources.Archived);

            searchBar.AddCondition(new ConditionType(Properties.Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 0, 0, false));
            searchBar.AddCondition(new ConditionType(Properties.Resources.StartingDate, "StartingDate", ConditionType.ConditionTypeEnum.DateRange));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private bool DisableAllowed(RecordIdentifier itemID)
        {
            if (PluginOperations.TestSiteService(PluginEntry.DataModel))
            {
                SiteServiceProfile ssProfile = PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel);

                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                List<SuspendedTransaction> suspendedTransactions = service.GetAllSuspendedTransactions(PluginEntry.DataModel, ssProfile, false);

                foreach (SuspendedTransaction transaction in suspendedTransactions)
                {
                    if (transaction.TransactionXML.Contains((string)itemID))
                    {
                        service.Disconnect(PluginEntry.DataModel);
                        return false;
                    }
                } 

                int totalRecordsMatching = 0;
                List<CustomerOrder> customerOrders = service.CustomerOrderSearch(PluginEntry.DataModel, ssProfile, out totalRecordsMatching, int.MaxValue, new CustomerOrderSearch(), false);

                foreach (CustomerOrder order in customerOrders)
                {
                    if (order.OrderXML.Contains((string)itemID))
                    {
                        service.Disconnect(PluginEntry.DataModel);
                        return false;
                    }
                }

                service.Disconnect(PluginEntry.DataModel);
                return true;
            }
            else
            {
                if (QuestionDialog.Show(Properties.Resources.CouldNotCheckDisableAllowed + " \n\n" + Properties.Resources.Proceed) == DialogResult.OK)
                {
                    return true;
                }

                return false;
            }
        }

        private void lvAssemblies_SelectionChanged(object sender, EventArgs e)
        {
            if (lvAssemblies.Selection.Count == 1)
            {
                selectedAssembly = (RetailItemAssembly)lvAssemblies.Selection[0].Tag;
                btnsAssemblies.EditButtonEnabled = btnsAssemblies.RemoveButtonEnabled = !selectedAssembly.Enabled;
                
                btnEnable.Enabled = Providers.RetailItemAssemblyComponentData.HasComponents(PluginEntry.DataModel, selectedAssembly.ID) ? selectedAssembly.Enabled ? !Providers.TransactionData.AssemblyItemIsActive(PluginEntry.DataModel, selectedAssembly.ID) && DisableAllowed(selectedAssembly.ID) : true : false;

                btnEnable.Text = selectedAssembly.Enabled ? Properties.Resources.Disable : Properties.Resources.Enable;
                
                if(selectedAssembly.Status != RetailItemAssemblyStatus.Disabled)
                {
                    btnsComponents.AddButtonEnabled = btnsComponents.EditButtonEnabled = btnsComponents.RemoveButtonEnabled = false;
                }
                else
                {
                    btnsComponents.AddButtonEnabled = true;
                }

                selectedAssemblyID = selectedAssembly.ID;
            }
            else
            {
                btnsAssemblies.EditButtonEnabled = btnsAssemblies.RemoveButtonEnabled = false;
                btnsComponents.AddButtonEnabled = btnsComponents.EditButtonEnabled = btnsComponents.RemoveButtonEnabled = false;
                btnEnable.Enabled = false;
                btnEnable.Text = Properties.Resources.Enable;
                selectedAssemblyID = null;
            }

            LoadComponentList();
        }

        private void lvComponents_SelectionChanged(object sender, EventArgs e)
        {
            selectedComponentID = lvComponents.Selection.Count == 1 ? ((RetailItemAssemblyComponent)lvComponents.Selection[0].Tag).ID : null;

            if (lvAssemblies.Selection.Count == 1 && ((RetailItemAssembly)lvAssemblies.Selection[0].Tag).Status != RetailItemAssemblyStatus.Disabled)
            {
                btnsComponents.AddButtonEnabled = btnsComponents.EditButtonEnabled = btnsComponents.RemoveButtonEnabled = false;
            }
            else
            {
                btnsComponents.AddButtonEnabled = lvAssemblies.Selection.Count == 1;
                btnsComponents.EditButtonEnabled = btnsComponents.RemoveButtonEnabled = lvComponents.Selection.Count == 1;
            }
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            args.UnknownControl = new DualDataComboBox();
            args.UnknownControl.Size = new Size(200, 21);
            args.MaxSize = 200;
            args.AutoSize = false;
            ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
            ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
            ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Store_DropDown);
            ((DualDataComboBox)args.UnknownControl).RequestClear += new EventHandler(Store_RequestClear);
        }

        private void Store_RequestClear(object sender, EventArgs e)
        {
            
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            ((DualDataComboBox)args.UnknownControl).RequestData -= Store_DropDown;
            ((DualDataComboBox)args.UnknownControl).RequestClear -= Store_RequestClear;
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection);
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void Store_DropDown(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void lvAssemblies_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if(btnsAssemblies.EditButtonEnabled)
            {
                btnsAssemblies_EditButtonClicked(sender, args);
            }
        }

        private void lvComponents_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if(btnsComponents.EditButtonEnabled)
            {
                btnsComponents_EditButtonClicked(sender, args);
            }
        }

        void lvAssemblies_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvAssemblies.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsAssemblies_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsAssemblies.EditButtonEnabled,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsAssemblies_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsAssemblies.AddButtonEnabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsAssemblies_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsAssemblies.RemoveButtonEnabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    btnEnable.Text,
                    400,
                    btnEnable_Click)
            {
                Enabled = btnEnable.Enabled
            };
            menu.Items.Add(item);

            e.Cancel = menu.Items.Count == 0;
        }

        void lvComponents_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvComponents.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsComponents_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsComponents.EditButtonEnabled,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsComponents_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsComponents.AddButtonEnabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsComponents_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsComponents.RemoveButtonEnabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.ViewItem,
                    400,
                    btnsComponents_ViewItem)
            {
                Enabled = !RecordIdentifier.IsEmptyOrNull(selectedComponentID)
            };
            menu.Items.Add(item);

            e.Cancel = menu.Items.Count == 0;
        }

        private void btnsComponents_ViewItem(object sender, EventArgs e)
        {
            if (itemViewer.IsAlive)
            {
                ((IPlugin)itemViewer.Target).Message(this, "ViewItem", Providers.RetailItemAssemblyComponentData.Get(PluginEntry.DataModel, selectedComponentID).ItemID);
            }
        }
    }
}
