using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class NewPOFromWorksheet : UserControl, IWizardPage
    {
        WizardBase parent;
        private InventoryTypeAction inventoryTypeAction;
        
        public NewPOFromWorksheet(WizardBase parent, InventoryTypeAction inventoryTypeAction)
        {
            InitializeComponent();

            

            this.parent = parent;
            this.inventoryTypeAction = inventoryTypeAction;
        }

        public RecordIdentifier SelectedPurchaseWorksheetId { get { return (RecordIdentifier)lvPurchaseWorksheets.Row(lvPurchaseWorksheets.Selection.FirstSelectedRow).Tag; } }

        private void LoadItems()
        {
            try
            {
                

                List<PurchaseWorksheet> worksheets = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                    .GetInventoryWorksheetList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), false);
                //Providers.PurchaseWorksheetData.GetList(PluginEntry.DataModel);

                if (!PluginEntry.DataModel.IsHeadOffice)
                {
                    worksheets =
                        worksheets.Where(worksheet => worksheet.StoreId == PluginEntry.DataModel.CurrentStoreID)
                            .ToList();
                }

                lvPurchaseWorksheets.ClearRows();
                string storeDescription;

                foreach (PurchaseWorksheet worksheet in worksheets)
                {
                    var inventoryTemplateListItem =
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .GetInventoryTemplateListItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                worksheet.InventoryTemplateID, false);

                    //Providers.InventoryTemplateData.GetListItem(PluginEntry.DataModel,
                    //    worksheet.InventoryTemplateID);

                    if (inventoryTemplateListItem == null)
                    {
                        continue;
                    }
                    DataEntity store = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, worksheet.StoreId);
                    storeDescription = store != null ? store.Text : "";

                    var row = new Row();
                    row.AddText(inventoryTemplateListItem.Text);
                    row.AddText(storeDescription);
                    //row.AddText(worksheet.NumberOfLinesInWorksheet.ToString());

                    row.Tag = worksheet.ID;
                    lvPurchaseWorksheets.AddRow(row);
                }

                lvPurchaseWorksheets.AutoSizeColumns();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private void CheckState()
        {
            parent.NextEnabled = lvPurchaseWorksheets.Selection.Count >= 1;
        }

        #region IWizardPage Members

        public void Display()
        {
            LoadItems();
            CheckState();
        }

        public IWizardPage RequestNextPage()
        {
            throw new NotImplementedException();
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            canUseFromForwardStack = false;
            return true;
        }

        public bool HasFinish
        {
            get { return true; }
        }

        public bool HasForward
        {
            get { return false; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void ResetControls()
        {

        }

        #endregion

        private void lvPurchaseWorksheets_SelectionChanged(object sender, EventArgs e)
        {
            CheckState();
        }
    }
}
