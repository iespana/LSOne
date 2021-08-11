using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    internal partial class ItemViewGeneralPage : UserControl, ITabViewV2, IMultiEditTabExtension, IMessageTabExtension
    {
        RecordIdentifier lastSelectedDimension;
        RetailItem item;
        WeakReference owner;
        WeakReference taxGroupEditor;
        WeakReference changeUnitsEditor;
        WeakReference unitsEditor;
        bool lockEvents;
        Dictionary<string, object> viewStateData;

        public ItemViewGeneralPage(TabControl owner)
            : this()
        {
            viewStateData = owner.ViewStateData;
            this.owner = new WeakReference(owner);
        }

        public ItemViewGeneralPage()
        {
            IPlugin plugin;

            lastSelectedDimension = "";


            InitializeComponent();

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewItemSalesTaxGroups", null);
            taxGroupEditor = plugin != null ? new WeakReference(plugin) : null;
            btnEditItemSalesTaxGroup.Visible = (taxGroupEditor != null);

            // Sales unit hot jump
            // If the former FindeImplementor returns null then the user does not have the Inventory Plugin
            // which means the user is not using our inventory feature.  
            plugin = PluginEntry.Framework.FindImplementor(this, "CanChangeItemsUnits", null);
            changeUnitsEditor = plugin != null ? new WeakReference(plugin) : null;
            if (changeUnitsEditor != null) // User has the Inventory plugin
            {
                cmbSalesUnit.Enabled = false;
                btnEditSalesUnits.Visible = true;
            }
            else // User does not have the inventory plugin. We can simply change the Sales unit
            {
                plugin = PluginEntry.Framework.FindImplementor(this, "CanEditUnits", null);
                unitsEditor = plugin != null ? new WeakReference(plugin) : null;
                btnEditSalesUnits.Visible = (unitsEditor != null);
            }

            var itemTypes = new object[]
                {
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Item),
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Service),
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.MasterItem),
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.AssemblyItem)
                };
            cmbItemType.Items.AddRange(itemTypes);

            lockEvents = false;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.ItemViewGeneralPage((TabControl)sender);
        }

        bool MainRecordDirty()
        {
            if ((cmbRetailDivision.SelectedData.ID != item.RetailDivisionMasterID)) return true;
            if ((cmbRetailGroup.SelectedData.ID != item.RetailGroupMasterID)) return true;
            if ((cmbRetailDepartment.SelectedData.ID != item.RetailDepartmentMasterID)) return true;

            return false;
        }

        #region ITabPanel Members

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            cmbRetailDepartment.Enabled = false;
            cmbRetailDivision.Enabled = false;
            cmbItemType.Enabled = false;

            if (internalContext != null)
            {
                item = (RetailItem)internalContext;

                if (item.ID == RecordIdentifier.Empty)
                {
                    lblSalesUnit.Visible = false;
                    cmbSalesUnit.Visible = false;
                    btnEditSalesUnits.Visible = false;

                    cmbSalesUnit.SetSelectionToNoChange(false);
                    cmbItemSalesTaxGroup.SetSelectionToNoChange(false);

                    cmbRetailDepartment.SetSelectionToNoChange(false);
                    cmbRetailGroup.SetSelectionToNoChange(false);
                    cmbRetailDivision.SetSelectionToNoChange(false);
                }
            }
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            viewStateData["ItemSalesTaxGroup"] = new RecordIdentifier("");

            item = (RetailItem)internalContext;

            cmbSalesUnit.SelectedData = new DataEntity(item.SalesUnitID, item.SalesUnitName);
            cmbItemSalesTaxGroup.SelectedData = new DataEntity(item.SalesTaxItemGroupID, item.SalesTaxItemGroupName);

            cmbRetailDepartment.SelectedData = new DataEntity(item.RetailDepartmentMasterID, item.RetailDepartmentName);
            cmbRetailGroup.SelectedData = new DataEntity(item.RetailGroupMasterID, item.RetailGroupName);
            cmbRetailDivision.SelectedData = new DataEntity(item.RetailDivisionMasterID, item.RetailDivisionName);

            viewStateData["ItemSalesTaxGroup"] = cmbItemSalesTaxGroup.SelectedData.ID;

            var defaultBarCode = Providers.BarCodeData.GetBarcodeWithShowForItem(PluginEntry.DataModel, item.ID);

            tbDefaultBarcode.Text = (defaultBarCode != null) ? defaultBarCode : "";

            cmbItemType.SelectedItem = item.ItemTypeDescription;

            //For master items or variant items item type cannot be changed.
            btnEditItemType.Enabled = (
                item != null &&
                item.ItemType != ItemTypeEnum.MasterItem &&
                item.ItemType != ItemTypeEnum.AssemblyItem &&
                string.IsNullOrWhiteSpace(item.VariantName)
            );
        }

        public bool DataIsModified()
        {
            if (MainRecordDirty())
            {
                item.Dirty = true;
            }

            if (cmbSalesUnit.SelectedData.ID != item.SalesUnitID ||
                cmbItemSalesTaxGroup.SelectedData.ID != item.SalesTaxItemGroupID)
            {
                item.Dirty = true;
            }

            return item.Dirty;
        }

        public bool SaveData()
        {
            if (item.Dirty)
            {
                item.RetailDepartmentMasterID = cmbRetailDepartment.SelectedData.ID;
                item.RetailGroupMasterID = cmbRetailGroup.SelectedData.ID;
                item.RetailDivisionMasterID = cmbRetailDivision.SelectedData.ID;

                item.SalesUnitID = cmbSalesUnit.SelectedData.ID;
                item.SalesTaxItemGroupID = cmbItemSalesTaxGroup.SelectedData.ID;

                item.ItemType = ItemTypeHelper.StringToItemType((string)cmbItemType.SelectedItem);
            }

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RetailItem", item.MasterID, Properties.Resources.RetailItem, true));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if ((changeHint & DataEntityChangeType.VariableChanged) == DataEntityChangeType.VariableChanged)
            {
                switch (objectName)
                {
                    case "RetailItem":
                        if (changeIdentifier == item.ID && param is RetailGroup)
                        {
                            RetailGroup retailGroup = (RetailGroup)param;
                            cmbItemSalesTaxGroup.SelectedData = new DataEntity(retailGroup.ItemSalesTaxGroupId, retailGroup.ItemSalesTaxGroupName);
                            cmbRetailDepartment.SelectedData = new DataEntity(retailGroup.RetailDepartmentMasterID, retailGroup.RetailDepartmentName);

                            PropertyBagChange();
                        }
                        break;
                }
            }
            if (changeHint == DataEntityChangeType.Edit && objectName == "ItemSalesUnit" && changeIdentifier == item.ID)
            {
                RetailItem changedItem = (RetailItem)param;
                cmbSalesUnit.SelectedData = new DataEntity(changedItem.SalesUnitID, changedItem.SalesUnitName);
            }

            if (changeHint == DataEntityChangeType.Edit && objectName == "BarCodeEdit")
            {
                LoadData(false, item.ID, item);
            }

            if (changeHint == DataEntityChangeType.Edit && objectName == "ItemType" && changeIdentifier == item.ID)
            {
                ItemTypeEnum newType = (ItemTypeEnum)(int)(RecordIdentifier)param;
                cmbItemType.SelectedItem = ItemTypeHelper.ItemTypeToString(newType);
                item.ItemType = newType;
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void DataFormater(object sender, DropDownFormatDataArgs e)
        {
        }



        private void PropertyBagChange()
        {
            if (item.ID != RecordIdentifier.Empty) // TODO: Investigate this one - Multiedit
            {
                viewStateData["ItemSalesTaxGroup"] = cmbItemSalesTaxGroup.SelectedData != null ? cmbItemSalesTaxGroup.SelectedData.ID : new RecordIdentifier("");
            }
        }


        private void RequestClear(object sender, EventArgs e)
        {
            RecordIdentifier oldID = ((DualDataComboBox)sender).SelectedData.ID;

            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");

            if (sender == cmbRetailGroup && item.ID == RecordIdentifier.Empty)
            {
                cmbRetailDivision.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                cmbRetailDepartment.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                cmbRetailDivision.TriggerSelectedDataChangedEvent();
                cmbRetailDepartment.TriggerSelectedDataChangedEvent();
                cmbRetailGroup.TriggerSelectedDataChangedEvent();
            }

        }

        private void cmbItemSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbItemSalesTaxGroup.SetData(
                Providers.ItemSalesTaxGroupData.GetListWithTaxCodes(PluginEntry.DataModel),
                null);
        }

        private void btnEditItemSalesTaxGroup_Click(object sender, EventArgs e)
        {
            if (taxGroupEditor.IsAlive)
            {
                ((IPlugin)taxGroupEditor.Target).Message(this, "ViewItemSalesTaxGroups", cmbItemSalesTaxGroup.SelectedData.ID);
            }
        }


        private void cmbItemSalesTaxGroup_SelectedDataChanged(object sender, EventArgs e)
        {
            if (lockEvents)
            {
                return;
            }

            if (QuestionDialog.Show(Properties.Resources.ChangeTaxGroupQuestion, Properties.Resources.ChangeTaxGroup, Properties.Resources.ChangeTaxGroupDetail) == DialogResult.No)
            {
                lockEvents = true; // Prevent loop-back
                if (item.ID == RecordIdentifier.Empty)
                {
                    // Multi edit
                    ((ViewBase)((Control)owner.Target).Parent.Parent).MultiEditRevertField(cmbItemSalesTaxGroup);
                }
                else
                {
                    cmbItemSalesTaxGroup.SelectedData = new DataEntity(item.SalesTaxItemGroupID, item.SalesTaxItemGroupName);
                }
                lockEvents = false;

            }
            else
            {
                if (item.SalesTaxItemGroupID != cmbItemSalesTaxGroup.SelectedData.ID)
                {
                    item.SalesTaxItemGroupID = cmbItemSalesTaxGroup.SelectedData.ID;
                    item.SalesTaxItemGroupName = cmbItemSalesTaxGroup.SelectedData.Text;
                    item.Dirty = true;
                }

                viewStateData["ItemSalesTaxGroup"] = cmbItemSalesTaxGroup.SelectedData.ID;
                viewStateData["ItemSalesTaxGroupChanged"] = true;

                if (owner.IsAlive)
                {
                    ((TabControl)owner.Target).BroadcastChangeInformation(DataEntityChangeType.TabMessage, "", RecordIdentifier.Empty, null);
                }
            }
        }

        private void btnEditSalesUnits_Click(object sender, EventArgs e)
        {
            // This editor being alive means we invoke the ItemUnitDialog which can edit inventory and sales unit
            if (changeUnitsEditor != null && changeUnitsEditor.IsAlive)
            {
                ((IPlugin)changeUnitsEditor.Target).Message(this, "ChangeItemsUnits", item);
                item = Providers.RetailItemData.Get(PluginEntry.DataModel, item.ID);
                DataEntity salesUnit = new DataEntity(item.SalesUnitID, item.SalesUnitName);
                cmbSalesUnit.SelectedData = salesUnit;
            }
            // This editor being alive means we edit the systems units
            else if (unitsEditor != null && unitsEditor.IsAlive)
            {
                ((IPlugin)unitsEditor.Target).Message(this, "EditUnits", cmbSalesUnit.SelectedData.ID);
            }
        }

        private void cmbInventoryUnit_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData.ID != "")
            {
                e.TextToDisplay = (string)((DualDataComboBox)sender).SelectedData.ID;
            }
            else
            {
                e.TextToDisplay = "";
            }
        }

        private void cmbRetailDepartment_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbRetailDepartment.SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailDepartmentsMasterID, textInitallyHighlighted);
        }

        private void cmbRetailGroup_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            if (((DataEntity)cmbRetailGroup.SelectedData).ID == DualDataComboBox.NoChangeID)
            {
                initialSearchText = "";
                e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, "", SearchTypeEnum.RetailGroupsMasterID, "", false);
            }
            else
            {
                if (e.DisplayText != "")
                {
                    initialSearchText = e.DisplayText;
                    textInitallyHighlighted = false;
                }
                else
                {
                    initialSearchText = "";
                    textInitallyHighlighted = true;
                }
                e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailGroupsMasterID, "", textInitallyHighlighted);
            }
        }

        private void btnEditRetailDepartment_Click(object sender, EventArgs e)
        {
            if (cmbRetailDepartment.SelectedData != null)
            {
                PluginOperations.ShowRetailDepartmentListView(cmbRetailDepartment.SelectedData.ID);
            }
            else
            {
                PluginOperations.ShowRetailDepartmentListView(this, EventArgs.Empty);
            }
        }

        private void btnEditRetailGroup_Click(object sender, EventArgs e)
        {
            if (cmbRetailGroup.SelectedData != null)
            {
                PluginOperations.ShowRetailGroupListView(cmbRetailGroup.SelectedData.ID);
            }
            else
            {
                PluginOperations.ShowRetailGroupListView(this, EventArgs.Empty);
            }
        }

        private void cmbRetailDepartment_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbRetailDepartment.SelectedData.ID == DualDataComboBox.NoChangeID)
            {
                // Sittuation that can happen in multiedit
                cmbRetailDivision.SetSelectionToNoChange();
                return;
            }

            // Remove the selected retail group if it is not in the newly selected retail department
            if (item.ID != RecordIdentifier.Empty)
            {
                if (!Providers.RetailGroupData.RetailGroupInDepartment(PluginEntry.DataModel, cmbRetailGroup.SelectedData.ID, cmbRetailDepartment.SelectedData.ID))
                {
                    cmbRetailGroup.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                }
            }

            var retailDepartment = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, cmbRetailDepartment.SelectedData.ID);
            if (retailDepartment != null && cmbRetailDivision.SelectedDataID != retailDepartment.RetailDivisionMasterID)
            {
                cmbRetailDivision.SelectedData = new DataEntity(retailDepartment.RetailDivisionMasterID, retailDepartment.RetailDivisionName);
                cmbRetailDivision.TriggerSelectedDataChangedEvent();
            }
            else if (retailDepartment == null)
            {
                if (cmbRetailDepartment.SelectedData.ID == DualDataComboBox.NoChangeID)
                {
                    cmbRetailDivision.SetSelectionToNoChange();
                }
                else
                {
                    cmbRetailDivision.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                    cmbRetailDivision.TriggerSelectedDataChangedEvent();
                }
            }

        }

        private void cmbRetailGroup_SelectedDataChanged(object sender, EventArgs e)
        {
            RetailGroup retailGroup = null;

            if (cmbRetailGroup.SelectedData.ID != "" && cmbRetailGroup.SelectedData.ID != RecordIdentifier.Empty && cmbRetailGroup.SelectedData.ID != DualDataComboBox.NoChangeID)
            {
                retailGroup = Providers.RetailGroupData.Get(PluginEntry.DataModel, cmbRetailGroup.SelectedData.ID);
            }


            if (retailGroup != null)
            {
                if (retailGroup.RetailDepartmentMasterID != null)
                {
                    cmbRetailDepartment.SelectedData = new DataEntity(retailGroup.RetailDepartmentMasterID, retailGroup.RetailDepartmentName);
                    cmbRetailDepartment.TriggerSelectedDataChangedEvent();
                }
                else
                {
                    cmbRetailDepartment.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                    cmbRetailDepartment.TriggerSelectedDataChangedEvent();
                }

            }
            else if (retailGroup == null)
            {
                if (cmbRetailGroup.SelectedData.ID == DualDataComboBox.NoChangeID)
                {
                    cmbRetailDepartment.SetSelectionToNoChange();
                }
                else
                {
                    cmbRetailDepartment.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                    cmbRetailDepartment.TriggerSelectedDataChangedEvent();
                }
            }

        }

        private void cmbRetailDivision_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbRetailDivision.SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailDivisionsMasterID,
                cmbRetailDivision.SelectedData.ID, textInitallyHighlighted);
        }

        private void btnEditRetailDivision_Click(object sender, EventArgs e)
        {
            if (cmbRetailDivision.SelectedData != null)
            {
                PluginOperations.ShowRetailDivisionListView(cmbRetailDivision.SelectedData.ID);
            }
            else
            {
                PluginOperations.ShowRetailDivisionListView(this, EventArgs.Empty);
            }
        }

        private void cmbRetailDivision_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbRetailDivision.SelectedData.ID == DualDataComboBox.NoChangeID)
            {
                // Sittuation that can happen in multiedit
                return;
            }
        }

        private void cmbRetailGroup_RequestNoChange(object sender, EventArgs e)
        {
            cmbRetailDepartment.SetSelectionToNoChange(true);
            cmbRetailDivision.SetSelectionToNoChange(true);
        }

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            RetailItemMultiEdit itemObject = (RetailItemMultiEdit)dataEntity;

            if (changedControlHashes.Contains(cmbRetailGroup.GetHashCode()))
            {
                itemObject.RetailGroupMasterID = cmbRetailGroup.SelectedData.ID;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.RetailGroupMasterID;
            }

            if (changedControlHashes.Contains(cmbItemSalesTaxGroup.GetHashCode()))
            {
                itemObject.HasValidSalesTaxItemGroupID = true; // Tell the controller and other tabs that we have a new tax group that should be user rather than the one in the database for each record
                itemObject.MustRecalculatePrices = true; // Signal to the controller that we must reclaculate price with tax for each record.
                itemObject.SalesTaxItemGroupID = cmbItemSalesTaxGroup.SelectedData.ID;
                itemObject.FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesTaxItemGroupID;
            }
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            return false;
        }

        public void MultiEditSaveSecondaryRecords(DataLayer.GenericConnector.Interfaces.IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {

        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {

        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {

        }

        private void btnEditItemType_Click(object sender, EventArgs e)
        {
            if (item != null)
            {
                PluginOperations.EditItemType(item.ID, ItemTypeHelper.StringToItemType((string)cmbItemType.SelectedItem));
            }
        }

        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            switch (message)
            {
                case "GetItemSalesUnit":
                    handled = true;
                    return new DataEntity(cmbSalesUnit.SelectedDataID, cmbSalesUnit.Text);

            }
            return null;
        }
    }
}
