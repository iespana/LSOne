using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.ViewPlugins.RetailItemAssemblies.Dialogs;
using LSOne.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.RetailItemAssemblies.DialogPages
{
    public partial class NewRetailItemAssembliesPage : UserControl, IDialogTabViewWithRequiredFields, IMessageTabExtension
    {
        public event EventHandler RequiredInputValidate;
        private RetailItem retailItem;

        private DecimalLimit priceLimiter;

        private RecordIdentifier selectedAssemblyID;
        private RecordIdentifier selectedComponentID;

        private Dictionary<RecordIdentifier, RetailItemAssembly> itemAssemblies;
        private Dictionary<RecordIdentifier, Dictionary<RecordIdentifier, RetailItemAssemblyComponent>> itemAssemblyComponents;

        private WeakReference owner;

        public NewRetailItemAssembliesPage(TabControl owner)
        {
            InitializeComponent();
            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            itemAssemblies = new Dictionary<RecordIdentifier, RetailItemAssembly>();
            itemAssemblyComponents = new Dictionary<RecordIdentifier, Dictionary<RecordIdentifier, RetailItemAssemblyComponent>>();

            lvAssemblies.SetSortColumn(1, true);
            lvComponents.SetSortColumn(1, true);

            lvAssemblies.ContextMenuStrip = new ContextMenuStrip();
            lvAssemblies.ContextMenuStrip.Opening += lvAssemblies_Opening;

            lvComponents.ContextMenuStrip = new ContextMenuStrip();
            lvComponents.ContextMenuStrip.Opening += lvComponents_Opening;

            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new NewRetailItemAssembliesPage((TabControl)sender);
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            retailItem = (RetailItem)internalContext;

            btnsEditAddRemoveAssemblies.EditButtonEnabled = btnsEditAddRemoveAssemblies.RemoveButtonEnabled =
            btnsAddEditRemoveComponents.AddButtonEnabled = btnsAddEditRemoveComponents.EditButtonEnabled =
            btnsAddEditRemoveComponents.RemoveButtonEnabled = false;

            LoadList();
            LoadComponentList();

        }

        private void LoadList()
        {
            RecordIdentifier selectedID = selectedAssemblyID;
            lvAssemblies.ClearRows();
            selectedAssemblyID = selectedID;

            var assemblies = itemAssemblies.Values.ToList();
            
            foreach (var itemAssembly in PluginOperations.SortList(assemblies, (RetailItemAssemblySort)lvAssemblies.Columns.IndexOf(lvAssemblies.SortColumn), lvAssemblies.SortedAscending))
            {
                Row row = new Row();
                row.AddText(itemAssembly.Text);
                row.AddText(itemAssembly.StoreName == "" ? Properties.Resources.All : itemAssembly.StoreName);
                row.AddText(itemAssembly.StartingDate.ToShortDateString());
                row.AddText(itemAssembly.TotalCost.FormatWithLimits(priceLimiter));
                row.AddText(itemAssembly.GetDisplayPrice().FormatWithLimits(priceLimiter));
                row.AddText(itemAssembly.Margin.FormatWithLimits(priceLimiter));
                row.AddText(itemAssembly.GetDisplayWithComponentsString());
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

            if (lvAssemblies.Selection.Count == 1)
            {
                List<RetailItemAssemblyComponent> assemblyComponents = itemAssemblyComponents[selectedAssemblyID].Values.ToList();

                foreach (var itemAssemblyComponent in PluginOperations.SortList(assemblyComponents, (RetailItemAssemblyComponentSort)lvComponents.Columns.IndexOf(lvComponents.SortColumn), lvComponents.SortedAscending))
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

        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnClose()
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }

        public void RequiredFieldsAreValid(FieldValidationArguments args)
        {
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveSecondaryRecords()
        {
            foreach (var itemAssembly in itemAssemblies.Values.ToList())
            {
                itemAssembly.ItemID = retailItem.ID;
                Providers.RetailItemAssemblyData.Save(PluginEntry.DataModel, itemAssembly);
            }

            foreach (var assemblyComponentList in itemAssemblyComponents.Values.ToList())
            { 
                foreach (var itemAssemblyComponent in assemblyComponentList.Values.ToList())
                {
                    Providers.RetailItemAssemblyComponentData.Save(PluginEntry.DataModel, itemAssemblyComponent);
                }
            }
        }

        public void SaveUserInterface()
        {

        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            RequiredInputValidate?.Invoke(this, EventArgs.Empty);
        }

        private void lvAssemblies_SelectionChanged(object sender, EventArgs e)
        {
            if (lvAssemblies.Selection.Count == 1)
            {
                selectedAssemblyID = ((RetailItemAssembly)lvAssemblies.Selection[0].Tag).ID;
                btnsEditAddRemoveAssemblies.EditButtonEnabled = 
                btnsEditAddRemoveAssemblies.RemoveButtonEnabled = 
                btnsAddEditRemoveComponents.AddButtonEnabled = true;

            }
            else
            {
                btnsEditAddRemoveAssemblies.EditButtonEnabled = 
                btnsEditAddRemoveAssemblies.RemoveButtonEnabled = 
                btnsAddEditRemoveComponents.AddButtonEnabled = 
                btnsAddEditRemoveComponents.EditButtonEnabled = 
                btnsAddEditRemoveComponents.RemoveButtonEnabled = false;

                selectedAssemblyID = null;
            }

            LoadComponentList();
        }

        private void lvComponents_SelectionChanged(object sender, EventArgs e)
        {
            selectedComponentID = (lvComponents.Selection.Count == 1) ? ((RetailItemAssemblyComponent)lvComponents.Selection[0].Tag).ID : null;

            btnsAddEditRemoveComponents.AddButtonEnabled = true;
            btnsAddEditRemoveComponents.EditButtonEnabled = 
            btnsAddEditRemoveComponents.RemoveButtonEnabled = (lvComponents.Selection.Count == 1);
        }

        private void CopyComponents(RetailItemAssembly fromAssembly, RecordIdentifier toAssemblyID)
        {
            List<RetailItemAssemblyComponent> fromComponentsList = itemAssemblies.ContainsKey(fromAssembly.ID)
                ? itemAssemblyComponents[fromAssembly.ID].Values.ToList()
                : Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, fromAssembly.ID);

            foreach (RetailItemAssemblyComponent component in fromComponentsList)
            {
                RetailItemAssemblyComponent copiedComponent = new RetailItemAssemblyComponent();
                copiedComponent.ID = Guid.NewGuid();
                copiedComponent.AssemblyID = toAssemblyID;
                copiedComponent.ItemID = component.ItemID;
                copiedComponent.ItemName = component.ItemName;
                copiedComponent.UnitID = component.UnitID;
                copiedComponent.UnitName = component.UnitName;
                copiedComponent.Quantity = component.Quantity;
                copiedComponent.CostPerUnit = component.CostPerUnit;
                itemAssemblyComponents[toAssemblyID].Add(copiedComponent.ID, copiedComponent);
            }
        }

        private void btnsEditAddRemoveAssemblies_AddButtonClicked(object sender, EventArgs e)
        {
            bool messageHandled;
            bool done = false;
            bool createAnother = false;

            object itemData = ((TabControl)this.owner.Target).SendViewPageMessage(this, "GetItemSalesUnitAndPrice", null, out messageHandled);
            ValueTuple<decimal, DataEntity> data = (ValueTuple<decimal, DataEntity>)itemData;
            decimal defaultPrice = data.Item1;
            DataEntity defaultSalesUnit = data.Item2;

            if(RecordIdentifier.IsEmptyOrNull(defaultSalesUnit.ID))
            {
                MessageDialog.Show(Properties.Resources.SalesUnitMustBeSelected);
                return;
            }

            while (!done)
            {
                List<RetailItemAssembly> assembliesToCopy = itemAssemblies.Values.ToList();
                using (NewEditAssemblyDialog dlg = new Dialogs.NewEditAssemblyDialog(
                    retailItem.ID, 
                    false, 
                    assembliesToCopy, 
                    createAnother, 
                    defaultPrice, 
                    defaultSalesUnit
                    ))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        RetailItemAssembly assembly = dlg.Assembly;
                        assembly.ID = Guid.NewGuid();
                        itemAssemblies.Add(assembly.ID, assembly);
                        itemAssemblyComponents.Add(assembly.ID, new Dictionary<RecordIdentifier, RetailItemAssemblyComponent>());
                        if (dlg.CopyFromAssembly != null)
                        {
                            CopyComponents(dlg.CopyFromAssembly, assembly.ID);
                        }

                        if (itemAssemblies.Count == 1)
                        {
                            ((TabControl)this.owner.Target).SendViewPageMessage(this, "RetailItemIsAssemblyItem", null, out messageHandled);
                        }

                        selectedAssemblyID = dlg.Assembly.ID;
                        selectedComponentID = null;
                        LoadList();
                        LoadComponentList();

                        createAnother = dlg.CreateAnother;
                        done = !createAnother;
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
        }

        private void btnsEditAddRemoveAssemblies_EditButtonClicked(object sender, EventArgs e)
        {
            if (lvAssemblies.Selection.Count == 1)
            {
                object itemData = ((TabControl)this.owner.Target).SendViewPageMessage(this, "GetItemSalesUnitAndPrice", null, out bool handled);

                ValueTuple<decimal, DataEntity> data = (ValueTuple<decimal, DataEntity>)itemData;
                DataEntity defaultSalesUnit = data.Item2;

                if (RecordIdentifier.IsEmptyOrNull(defaultSalesUnit.ID))
                {
                    MessageDialog.Show(Properties.Resources.SalesUnitMustBeSelected);
                    return;
                }

                using (NewEditAssemblyDialog dlg = new NewEditAssemblyDialog((RetailItemAssembly)lvAssemblies.Selection[0].Tag, false, defaultSalesUnit))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        RetailItemAssembly assembly = dlg.Assembly;
                        itemAssemblies[assembly.ID] = assembly;
                        LoadList();
                        LoadComponentList();
                    }
                }
            }
        }

        private void btnsEditAddRemoveAssemblies_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvAssemblies.Selection.Count == 1)
            {
                itemAssemblies.Remove(selectedAssemblyID);
                itemAssemblyComponents.Remove(selectedAssemblyID);

                if (itemAssemblies.Count == 0)
                {
                    ((TabControl)this.owner.Target).SendViewPageMessage(this, "RetailItemIsItem", null, out bool handled);
                }

                LoadList();
                LoadComponentList();
            }

        }

        private void btnsAddEditRemoveComponents_AddButtonClicked(object sender, EventArgs e)
        {
            bool done = false;

            while (!done)
            {
                RetailItemAssembly selectedAssembly = (RetailItemAssembly)lvAssemblies.Selection[0].Tag;
                using (NewEditAssemblyComponentDialog dlg = new NewEditAssemblyComponentDialog(
                    selectedAssemblyID,
                    itemAssemblyComponents[selectedAssemblyID].Values.ToList(),
                    false,
                    null,
                    selectedAssembly.StoreID
                ))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        RetailItemAssemblyComponent assemblyComponent = dlg.AssemblyComponent;
                        assemblyComponent.ID = Guid.NewGuid();
                        itemAssemblyComponents[selectedAssemblyID].Add(assemblyComponent.ID, assemblyComponent);
                        UpdateTotalAssemblyCostAndSalePrice(selectedAssemblyID);
                        LoadComponentList();
                        LoadList();

                        done = !dlg.CreateAnother;
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
        }

        private void btnsAddEditRemoveComponents_EditButtonClicked(object sender, EventArgs e)
        {
            RetailItemAssembly selectedAssembly = (RetailItemAssembly)lvAssemblies.Selection[0].Tag;
            using (NewEditAssemblyComponentDialog dlg = new NewEditAssemblyComponentDialog(
                itemAssemblyComponents[selectedAssemblyID][selectedComponentID],
                itemAssemblyComponents[selectedAssemblyID].Values.ToList(),
                false,
                null,
                selectedAssembly.StoreID
            ))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    RetailItemAssemblyComponent assemblyComponent = dlg.AssemblyComponent;
                    itemAssemblyComponents[selectedAssemblyID][selectedComponentID] = assemblyComponent;
                    UpdateTotalAssemblyCostAndSalePrice(selectedAssemblyID);
                    LoadComponentList();
                    LoadList();
                }
            }
        }

       private void btnsAddEditRemoveComponents_RemoveButtonClicked(object sender, EventArgs e)
        {
            itemAssemblyComponents[selectedAssemblyID].Remove(selectedComponentID);
            UpdateTotalAssemblyCostAndSalePrice(selectedAssemblyID);
            selectedComponentID = null;
            LoadComponentList();
            LoadList();

        }

        private void UpdateTotalAssemblyCostAndSalePrice(RecordIdentifier assemblyID)
        {
            decimal cost = 0m;
            decimal salePrice = 0m;
            foreach (var assemblyComponent in itemAssemblyComponents[assemblyID].Values.ToList())
            {
                cost += assemblyComponent.GetTotalCost();

                if(!assemblyComponent.SalesPriceRetrieved)
                {
                    decimal unitFactor = 1;
                    RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, assemblyComponent.ItemID);

                    if(item.IsAssemblyItem)
                    {
                        RetailItemAssembly activeAssembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(PluginEntry.DataModel, item.ID, itemAssemblies[assemblyID].StoreID);

                        if(activeAssembly == null)
                        {
                            assemblyComponent.SalesPrice = item.SalesPriceIncludingTax;
                            unitFactor = Providers.UnitConversionData.GetUnitOfMeasureConversionFactor(PluginEntry.DataModel, item.SalesUnitID, assemblyComponent.UnitID, item.ID);
                        }
                        else
                        {
                            assemblyComponent.SalesPrice = activeAssembly.CalculatePriceFromComponents ? activeAssembly.TotalSalesPrice : activeAssembly.Price;
                        }
                    }
                    else
                    {
                        assemblyComponent.SalesPrice = item.SalesPriceIncludingTax;
                        unitFactor = Providers.UnitConversionData.GetUnitOfMeasureConversionFactor(PluginEntry.DataModel, item.SalesUnitID, assemblyComponent.UnitID, item.ID);
                    }

                    assemblyComponent.SalesPrice = assemblyComponent.SalesPrice * unitFactor;
                    assemblyComponent.SalesPriceRetrieved = true;
                }

                salePrice += assemblyComponent.GetTotalSalesPrice();
            }

            var assembly = itemAssemblies[assemblyID];
            assembly.TotalCost = cost;
            assembly.TotalSalesPrice = salePrice;
            assembly.Margin = PluginOperations.CalculateProfitMargin(assembly.Price, assembly.TotalCost);
        }

        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            switch (message)
            {
                case "CreateAnother":
                    itemAssemblyComponents.Clear();
                    itemAssemblies.Clear();
                    return null;
            }

            return null;
        }

        private void lvAssemblies_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemoveAssemblies.EditButtonEnabled)
            {
                btnsEditAddRemoveAssemblies_EditButtonClicked(sender, args);
            }
        }

        private void lvComponents_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsAddEditRemoveComponents.EditButtonEnabled)
            {
                btnsAddEditRemoveComponents_EditButtonClicked(sender, args);
            }
        }

        void lvAssemblies_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvAssemblies.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemoveAssemblies_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemoveAssemblies.EditButtonEnabled,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsEditAddRemoveAssemblies_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsEditAddRemoveAssemblies.AddButtonEnabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemoveAssemblies_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemoveAssemblies.RemoveButtonEnabled
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
                    btnsAddEditRemoveComponents_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsAddEditRemoveComponents.EditButtonEnabled,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsAddEditRemoveComponents_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsAddEditRemoveComponents.AddButtonEnabled
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsAddEditRemoveComponents_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsAddEditRemoveComponents.RemoveButtonEnabled
            };
            menu.Items.Add(item);

            e.Cancel = menu.Items.Count == 0;
        }
    }
}
