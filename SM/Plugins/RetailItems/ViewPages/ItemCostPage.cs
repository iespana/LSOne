using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.ViewCore.Dialogs;
using LSOne.Controls.Rows;
using LSOne.Controls;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewPlugins.RetailItems.Dialogs;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class ItemCostPage : UserControl, ITabViewV2
    {
        private RetailItem item;
        private RecordIdentifier itemID;
        private DecimalLimit priceLimiter;
        private DecimalLimit quantityLimiter;
        private List<RetailItemCost> itemCosts;
        private ISiteServiceService service;

        private static Guid BarSettingID = new Guid("76231B55-6954-417C-97E1-52AF6B1C6D18");
        private Setting searchBarSetting;

        public ItemCostPage()
        {
            InitializeComponent();

            itemCostDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemCostDataScroll.Reset();

            lvCosts.ContextMenuStrip = new ContextMenuStrip();
            lvCosts.ContextMenuStrip.Opening += lvCosts_Opening;

            searchBar.BuddyControl = lvCosts;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ItemCostPage();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            lvCosts.SetSortColumn(0, true);
            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            itemID = context;
            item = (RetailItem)internalContext;
            service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            SearchItemCosts();
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void SearchItemCosts()
        {
            lvCosts.ClearRows();
            int totalCount = 0;

            try
            {
                itemCosts = service.GetRetailItemCostList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), itemID, GetSearchFilter(), out totalCount, true);
            }
            catch
            {
                MessageDialog.Show(Properties.Resources.NoConnectionToSiteServiceHeader);
                return;
            }

            itemCostDataScroll.RefreshState(itemCosts.Take(itemCosts.Count - 1).ToList(), totalCount);

            Row row = null;

            for (int i = 0; i < itemCosts.Count; i++)
            {
                RetailItemCost cost = itemCosts[i];
                row = new Row();

                if(i == itemCosts.Count - 1) //Last row should be average of all rows
                {
                    Style averageCellStyle = new Style(lvCosts.DefaultStyle);
                    averageCellStyle.BorderTop = true;
                    averageCellStyle.BorderColor = Color.Black;
                    averageCellStyle.Font = new Font(lvCosts.DefaultStyle.Font, FontStyle.Bold);

                    row.AddCell(new Controls.Cells.Cell(Properties.Resources.OverallCost, averageCellStyle));
                    row.AddCell(new Controls.Cells.Cell(cost.Cost.FormatWithLimits(priceLimiter), averageCellStyle));
                    row.AddCell(new Controls.Cells.Cell(cost.Quantity.FormatWithLimits(quantityLimiter) + " " + cost.UnitName, averageCellStyle));
                    row.AddCell(new Controls.Cells.Cell(cost.TotalCostValue.FormatWithLimits(priceLimiter), averageCellStyle));
                    row.AddCell(new Controls.Cells.Cell("", averageCellStyle));
                    row.AddCell(new Controls.Cells.Cell("", averageCellStyle));
                    row.AddCell(new Controls.Cells.Cell("", averageCellStyle));
                    row.AddCell(new Controls.Cells.Cell("", averageCellStyle));
                    row.Selectable = false;
                }
                else
                {
                    row.AddText(cost.StoreName);
                    row.AddText(cost.Cost.FormatWithLimits(priceLimiter));
                    row.AddText(cost.Quantity.FormatWithLimits(quantityLimiter) + " " + cost.UnitName);
                    row.AddText(cost.TotalCostValue.FormatWithLimits(priceLimiter));
                    row.AddText(cost.EntryDate.ToShortDateString() + " " + cost.EntryDate.DateTime.ToShortTimeString());
                    row.AddText(cost.RecalculationReason);
                    row.AddText(cost.UserLogin + " - " + cost.UserName);
                    row.AddText("");
                    row.Tag = cost;
                }
                
                lvCosts.AddRow(row);
            }

            lvCosts.AutoSizeColumns();
        }

        private RetailItemCostFilter GetSearchFilter()
        {
            RetailItemCostFilter filter = new RetailItemCostFilter();

            filter.Sort = (RetailItemCostSort)lvCosts.Columns.IndexOf(lvCosts.SortColumn);
            filter.SortDescending = !lvCosts.SortedAscending;
            filter.RowFrom = itemCostDataScroll.StartRecord;
            filter.RowTo = itemCostDataScroll.EndRecord + 1;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        filter.Description = result.StringValue;
                        filter.DescriptionBeginsWith = result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith;
                        break;
                    case "City":
                        filter.City = result.StringValue;
                        filter.CityBeginsWith = result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith;
                        break;
                    case "Region":
                        filter.RegionID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Currency":
                        filter.CurrencyID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "CalculationDate":
                        if (result.Date.Checked)
                        {
                            filter.CalculationDateFrom = result.Date.Value.Date;

                            if(result.Time.Checked)
                            {
                                filter.CalculationDateFrom = filter.CalculationDateFrom.Value.AddTicks(result.Time.Value.TimeOfDay.Ticks);
                            }
                        }

                        if (result.DateTo.Checked)
                        {
                            filter.CalculationDateTo = result.DateTo.Value.Date;

                            if (result.TimeTo.Checked)
                            {
                                filter.CalculationDateTo = filter.CalculationDateTo.Value.AddTicks(result.TimeTo.Value.TimeOfDay.Ticks);
                            }
                            else
                            {
                                filter.CalculationDateTo = filter.CalculationDateTo.Value.AddDays(1).AddSeconds(-1);
                            }
                        }
                        break;
                    case "Employee":
                        filter.User = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                }
            }

            return filter;
        }

        private void lvCosts_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvCosts.SortColumn == args.Column)
            {
                lvCosts.SetSortColumn(args.Column, !lvCosts.SortedAscending);
            }
            else
            {
                lvCosts.SetSortColumn(args.Column, true);
            }

            itemCostDataScroll.Reset();
            SearchItemCosts();
        }

        private void ItemCostPage_SizeChanged(object sender, EventArgs e)
        {
            lvCosts.AutoSizeColumns();
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
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
            itemCostDataScroll.Reset();
            SearchItemCosts();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Properties.Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Properties.Resources.City, "City", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Region, "Region", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Currency, "Currency", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Properties.Resources.CalculationDate, "CalculationDate", ConditionType.ConditionTypeEnum.DateAndTimeRange));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Employee, "Employee", ConditionType.ConditionTypeEnum.Unknown));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            args.UnknownControl = new DualDataComboBox();
            args.UnknownControl.Size = new Size(200, 21);
            args.MaxSize = 200;
            args.AutoSize = false;
            ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
            ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

            switch (args.TypeKey)
            {
                case "Region":
                    ((DualDataComboBox)args.UnknownControl).RequestData += Region_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += Region_RequestClear;
                    break;
                case "Currency":
                    ((DualDataComboBox)args.UnknownControl).RequestData += Currency_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += Currency_RequestClear;
                    break;
                case "Employee":
                    ((DualDataComboBox)args.UnknownControl).DropDown += cmbEmployee__DropDown;
                    break;
            }
        }

        private void Currency_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void Region_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void Currency_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void Region_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SetData(Providers.RegionData.GetList(PluginEntry.DataModel, DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.Description, false), null);
        }

        private void cmbEmployee__DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchPanel(PluginEntry.DataModel, e.DisplayText);
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Region":
                case "Currency":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
                case "Employee":
                    args.Selection = ((DualDataComboBox)args.UnknownControl).SelectedData.Text;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Region":
                case "Currency":
                    args.HasSelection = !RecordIdentifier.IsEmptyOrNull(((DualDataComboBox)args.UnknownControl).SelectedData.ID);
                    break;
                case "Employee":
                    args.HasSelection = !RecordIdentifier.IsEmptyOrNull(((DualDataComboBox)args.UnknownControl).SelectedData.ID);
                    break;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Region":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Region_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= Region_RequestClear;
                    break;
                case "Currency":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Currency_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= Currency_RequestClear;
                    break;
                case "Employee":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= cmbEmployee__DropDown;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Region":
                    entity = Providers.RegionData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Currency":
                    entity = Providers.CurrencyData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Employee":
                    Guid guid = Guid.Empty;
                    Guid.TryParse(args.Selection, out guid);
                    entity = Providers.UserData.Get(PluginEntry.DataModel, guid);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void itemCostDataScroll_PageChanged(object sender, EventArgs e)
        {
            SearchItemCosts();
        }

        private void lvCosts_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = lvCosts.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageItemCostPrice);
        }

        private void lvCosts_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            btnEdit_Click(sender, EventArgs.Empty);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            List<DataEntity> selectedStores = new List<DataEntity>();
            decimal initialCostPrice = 0;

            for (int i = 0; i < lvCosts.Selection.Count; i++)
            {
                RetailItemCost selectedCost = (RetailItemCost)lvCosts.Selection[i].Tag;
                selectedStores.Add(new DataEntity(selectedCost.StoreID, selectedCost.StoreName));

                if (i == 0)
                {
                    initialCostPrice = selectedCost.Cost;
                }
                else if (initialCostPrice != selectedCost.Cost) //We don't have the same cost price for all stores
                {
                    initialCostPrice = 0;
                }
            }

            using (EditItemCostsDialog dlg = new EditItemCostsDialog(selectedStores, initialCostPrice))
            {
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    List<RetailItemCost> newItemCosts = new List<RetailItemCost>();
                    decimal costPriceInInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, itemID, item.SalesUnitID, item.InventoryUnitID, dlg.CostPrice);

                    foreach(RecordIdentifier storeID in dlg.StoreIDs)
                    {
                        newItemCosts.Add(new RetailItemCost
                        {
                            Cost = costPriceInInventoryUnit,
                            RecalculationReason = dlg.Reason,
                            EntryDate = Date.Now,
                            ItemID = itemID,
                            StoreID = storeID,
                            UserID = PluginEntry.DataModel.CurrentUser.ID,
                            UnitID = item.InventoryUnitID
                        });
                    }

                    try
                    {
                        service.InsertRetailItemCosts(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), newItemCosts, true);
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.NoConnectionToSiteServiceHeader);
                    }

                    SearchItemCosts();
                }
            }
        }

        private void lvCosts_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvCosts.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(Properties.Resources.Edit, 100, btnEdit_Click)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnEdit.Enabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ItemCostPageList", lvCosts.ContextMenuStrip, lvCosts);
            e.Cancel = menu.Items.Count == 0;
        }
    }
}
