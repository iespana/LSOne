using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class LinkedItemsDialog : DialogBase
    {
        RetailItem originalItem;
        RetailItem retailItem;
        private RecordIdentifier unitID;
        WeakReference unitAdder;
        LinkedItem linkedItem;
        private List<Unit> convertableUnits;
        private bool originalBlockedState;
        private int originalQuantity;

        public LinkedItemsDialog(RetailItem originalItem)
        {
            InitializeComponent();

            this.originalItem = originalItem;
            
            cmbLinkedItem.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            originalBlockedState = false;
            originalQuantity = 0;
            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanAddUnits", null);
            unitAdder = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddUnit.Visible = (unitAdder != null);
                
        }

        public LinkedItemsDialog(RecordIdentifier linkedItemID, RetailItem originalItem)
            : this(originalItem)
        {
            linkedItem = Providers.LinkedItemData.Get(PluginEntry.DataModel, linkedItemID);
            retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, linkedItemID);
            cmbLinkedItem.SelectedData = new DataEntity(linkedItem.LinkedItemID, linkedItem.LinkedItemDescription);
            cmbVariant.SelectedData = retailItem.HeaderItemID != null ? new DataEntity(linkedItem.LinkedItemID, linkedItem.LinkedItemVariantDescription) : new DataEntity();
            cmbUnit.SelectedData = new DataEntity(linkedItem.LinkedItemUnitID, linkedItem.LinkedItemUnitDescription);
            chkBlocked.Checked = linkedItem.Blocked;
            originalBlockedState = chkBlocked.Checked;
            ntbQuantity.SetValueWithLimit(linkedItem.LinkedItemQuantity,linkedItem.UnitLimiter);
            originalQuantity = (int)linkedItem.LinkedItemQuantity;

            cmbLinkedItem.Enabled = lblLinkedItem.Enabled = false;
            btnOK.Enabled = false;
        }

        public RecordIdentifier LinkedItemID
        {
            get
            {
                return linkedItem.LinkedItemID;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs args)
        {

            if (linkedItem == null)
            {
                linkedItem = new LinkedItem();
                linkedItem.OriginalItemID = originalItem.ID;
                linkedItem.LinkedItemID = retailItem.ID;
                linkedItem.LinkedItemUnitID = cmbUnit.SelectedData.ID;

                if (Providers.LinkedItemData.Exists(PluginEntry.DataModel, linkedItem.ID))
                {
                    errorProvider1.SetError(cmbLinkedItem, Properties.Resources.LinkedItemAlreadyExists);
                    linkedItem = null;
                    return;
                }
                
                if(linkedItem.OriginalItemID == linkedItem.LinkedItemID)
                {
                    errorProvider1.SetError(cmbLinkedItem, Properties.Resources.LinkedItemSameAsOriginalItem);
                    linkedItem = null;
                    return;
                }    
            }

            linkedItem.Blocked = chkBlocked.Checked;
            linkedItem.LinkedItemQuantity = (decimal)ntbQuantity.Value;

            Providers.LinkedItemData.Save(PluginEntry.DataModel, linkedItem);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled()
        {
            errorProvider1.Clear();

            bool enabled = cmbLinkedItem.Text != "" && ntbQuantity.FullPrecisionValue != 0;

            enabled = enabled && (
                (
                    cmbVariant.Enabled
                    && !RecordIdentifier.IsEmptyOrNull(cmbVariant.SelectedData.ID.PrimaryID)
                    && cmbVariant.SelectedData.ID.PrimaryID != ""
                )
                 || (
                    !cmbVariant.Enabled
                    && RecordIdentifier.IsEmptyOrNull(cmbVariant.SelectedData.ID)
                    )
                     || (
                     cmbVariant.SelectedData.ID != "" && cmbLinkedItem.SelectedData.ID != "")
                );
            enabled = enabled && (cmbUnit.Text != "");

            btnOK.Enabled = enabled;
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            // We only want units displayed that are convertable to the linked items inventory unit
            RecordIdentifier linkedItemsInventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbLinkedItem.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);

            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,"",linkedItemsInventoryUnitID),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel)};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                250);

            cmbUnit.SetData(data, pnl);
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            ((IPlugin)unitAdder.Target).Message(this, "AddUnits", null);
        }

        private void cmbLinkedItem_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbLinkedItem.SelectedData.ID != "")
            {
                cmbVariant.Enabled = lblVariant.Enabled = false;
                cmbVariant.SelectedData = new DataEntity();
                retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbLinkedItem.SelectedData.ID);

                if (retailItem.ItemType == ItemTypeEnum.MasterItem)
                {
                    cmbVariant.Enabled = lblVariant.Enabled = true;
                }

                unitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbLinkedItem.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);

                if ((string) unitID != null)
                {
                    convertableUnits = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, cmbLinkedItem.SelectedData.ID, unitID);
                    cmbUnit.SelectedData = convertableUnits.Find(u => u.ID == unitID);
                }
                cmbUnit.Enabled = lblUnit.Enabled = btnAddUnit.Enabled = true;
            }

            CheckEnabled();
        }

        private void cmbLinkedItem_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbLinkedItem.SelectedData).ID;
                textInitallyHighlighted = true;
            }
            
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailItems, textInitallyHighlighted, null, true, false);
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            bool hasUnitConversion = PluginOperations.HasUnitConversion((DataEntity)cmbLinkedItem.SelectedData, cmbUnit.SelectedData.ID);

            if (!hasUnitConversion)
            {
                cmbUnit.SelectedData = new DataEntity("", "");
            }

            CheckEnabled();
        }

        private void cmbVariant_DropDown(object sender, DropDownEventArgs e)
        {
            List<RecordIdentifier> excludedItemIDs = new List<RecordIdentifier>();

            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbVariant.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs, textInitallyHighlighted, true);
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbVariant.SelectedData.ID != "")
            {
                if ((string)unitID != null)
                {
                    convertableUnits = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, cmbVariant.SelectedData.ID, unitID);
                    cmbUnit.SelectedData = convertableUnits[0];
                }

                retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbVariant.SelectedData.ID);
            }
            CheckEnabled();
        }

        private void chkBlocked_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBlocked.Checked != originalBlockedState || ntbQuantity.FullPrecisionValue != originalQuantity)
            {
                CheckEnabled();
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        private void ntbQuantity_TextChanged(object sender, EventArgs e)
        {
            if (chkBlocked.Checked != originalBlockedState || ntbQuantity.FullPrecisionValue != originalQuantity)
            {
                CheckEnabled();
            }
            else
            {
                btnOK.Enabled = false;
            }
        }
    }
}
