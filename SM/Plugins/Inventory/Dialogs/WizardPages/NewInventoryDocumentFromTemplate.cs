using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class NewInventoryDocumentFromTemplate : UserControl, IWizardPage
    {
        WizardBase parent;
        private InventoryTypeAction inventoryTypeAction;

        public TemplateListItem SelectedTemplate
        {
            get
            {
                return (TemplateListItem)lvTemplates.Selection[0].Tag;
            }
        }

        public NewInventoryDocumentFromTemplate(WizardBase parent, InventoryTypeAction inventoryTypeAction)
        {
            InitializeComponent();

            this.parent = parent;
            this.inventoryTypeAction = inventoryTypeAction;
        }

        public bool HasFinish
        {
            get
            {
                return inventoryTypeAction.InventoryType.HasValue && inventoryTypeAction.InventoryType.Value == InventoryEnum.StockCounting;
            }
        }

        public bool HasForward
        {
            get
            {
                return inventoryTypeAction.InventoryType.HasValue && inventoryTypeAction.InventoryType.Value != InventoryEnum.StockCounting;
            }
        }

        public Control PanelControl
        {
            get
            {
                return this;
            }
        }

        public void Display()
        {
            LoadItems();
            CheckState();
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            canUseFromForwardStack = false;
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            if(inventoryTypeAction.InventoryType.HasValue)
            {
                if(inventoryTypeAction.InventoryType.Value == InventoryEnum.StoreTransfer)
                {
                    return new NewStoreTransfer(parent, SelectedTemplate, inventoryTypeAction.StoreTransferType.Value);
                }
                else if(inventoryTypeAction.InventoryType.Value == InventoryEnum.PurchaseOrder)
                {
                    return new NewEmptyPO(parent, SelectedTemplate, inventoryTypeAction);
                }
            }

            throw new Exception("Inventory document type is not defined.");
        }

        public void ResetControls()
        {
            
        }

        private void CheckState()
        {
            parent.NextEnabled = lvTemplates.Selection.Count >= 1;
        }

        private void lvTemplates_SelectionChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void LoadItems()
        {
            try
            {
                if (inventoryTypeAction == null || !inventoryTypeAction.InventoryType.HasValue) return;

                TemplateEntryTypeEnum? templateEntryType = null;

                switch (inventoryTypeAction.InventoryType.Value)
                {
                    case InventoryEnum.PurchaseOrder:
                        templateEntryType = TemplateEntryTypeEnum.PurchaseOrder;
                        break;
                    case InventoryEnum.StockCounting:
                        templateEntryType = TemplateEntryTypeEnum.StockCounting;
                        break;
                    case InventoryEnum.StoreTransfer:
                        templateEntryType = TemplateEntryTypeEnum.TransferStock;
                        break;
                    default:
                        break;
                }

                if (!templateEntryType.HasValue) return;

                List<TemplateListItem> templates = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                    .GetInventoryTemplatesByType(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), templateEntryType.Value, true);

                lvTemplates.ClearRows();

                if((inventoryTypeAction.InventoryType.Value == InventoryEnum.StoreTransfer && !PluginEntry.DataModel.IsHeadOffice)
                    || (inventoryTypeAction.InventoryType.Value == InventoryEnum.PurchaseOrder && !PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrdersForAllStores)))
                {
                    templates.RemoveAll(x => x.StoreID != PluginEntry.DataModel.CurrentStoreID);
                }
                
                foreach (TemplateListItem template in templates)
                {
                    Row row = new Row();
                    row.AddText(template.TemplateName);
                    row.AddText(template.StoreName);

                    row.Tag = template;
                    lvTemplates.AddRow(row);
                }

                lvTemplates.AutoSizeColumns();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private void lvTemplates_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if(parent.NextEnabled)
            {
                if (HasForward)
                {
                    parent.Next();
                }
                else
                {
                    parent.Finish();
                }
            }
        }
    }
}
