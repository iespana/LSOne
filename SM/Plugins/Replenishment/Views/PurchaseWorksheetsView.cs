using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Replenishment.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Replenishment.Views
{
    public partial class PurchaseWorksheetsView : ViewBase
    {
        RecordIdentifier selectedWorksheetId;
        

        public PurchaseWorksheetsView(RecordIdentifier selectedWorksheetId)
            :this()
        {
            this.selectedWorksheetId = selectedWorksheetId;
        }

        public PurchaseWorksheetsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            lvPurchaseWorksheets.ContextMenuStrip = new ContextMenuStrip();
            lvPurchaseWorksheets.ContextMenuStrip.Opening += lvPurchaseWorksheets_RightClick;
            btnEdit.Enabled = false;
        }

        protected override string LogicalContextName
        {
            get
            {
                return HeaderText;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PurchaseWorksheet")
            {          
                selectedWorksheetId = changeAction == DataEntityChangeType.Delete ? RecordIdentifier.Empty : changeIdentifier;
                LoadPurchaseWorksheets();
            }

            if (objectName == "InventoryTemplate" && changeAction == DataEntityChangeType.Delete)
            {
                selectedWorksheetId = changeAction == DataEntityChangeType.Delete ? RecordIdentifier.Empty : changeIdentifier;
                LoadPurchaseWorksheets();
            }

        }

        protected override void LoadData(bool isRevert)
        {
            LoadPurchaseWorksheets();
        }

        private void LoadPurchaseWorksheets()
        {
            try
            {


                List<PurchaseWorksheet> worksheets =
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .GetInventoryWorksheetList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                            false);


                if (!PluginEntry.DataModel.IsHeadOffice)
                {
                    worksheets =
                        worksheets.Where(worksheet => worksheet.StoreId == PluginEntry.DataModel.CurrentStoreID)
                            .ToList();
                }

                RecordIdentifier selectedID = selectedWorksheetId;

                lvPurchaseWorksheets.ClearRows();
                string storeDescription;

                foreach (PurchaseWorksheet worksheet in worksheets)
                {
                    var inventoryTemplateListItem =
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .GetInventoryTemplateListItem(PluginEntry.DataModel,
                                PluginOperations.GetSiteServiceProfile(), worksheet.InventoryTemplateID, false);


                    if (inventoryTemplateListItem == null)
                    {
                        continue;
                    }
                    DataEntity store = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, worksheet.StoreId);
                    storeDescription = store != null ? store.Text : "";

                    var row = new Row();
                    row.AddText(inventoryTemplateListItem.Text);
                    row.AddText(storeDescription);
                    row.AddText(worksheet.NumberOfLinesInWorksheet.ToString());

                    row.Tag = worksheet.ID;
                    lvPurchaseWorksheets.AddRow(row);

                    if (worksheet.ID == selectedID)
                    {
                        lvPurchaseWorksheets.Selection.Set(lvPurchaseWorksheets.RowCount - 1);
                    }
                }
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

            lvPurchaseWorksheets.AutoSizeColumns();
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (lvPurchaseWorksheets.Selection.FirstSelectedRow >= 0)
            {
                var selectedPurchaseWorksheetId = (RecordIdentifier) lvPurchaseWorksheets.Row(lvPurchaseWorksheets.Selection.FirstSelectedRow).Tag;
                PluginOperations.ShowPurchaseWorksheet(selectedPurchaseWorksheetId);
            }
        }

        private void lvPurchaseWorksheets_SelectionChanged(object sender, System.EventArgs e)
        {
            int rowsSelected = lvPurchaseWorksheets.Selection.Count;
            btnEdit.Enabled = rowsSelected > 0;
        }

        private void lvPurchaseWorksheets_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvPurchaseWorksheets.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                   Resources.Edit,
                   100,
                   btnEdit_Click)
            {
                Enabled = btnEdit.Enabled,
                Image = ContextButtons.GetEditButtonImage(),
                Default = true
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PurchaseWorksheetsList", lvPurchaseWorksheets.ContextMenuStrip, lvPurchaseWorksheets);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvPurchaseWorksheets_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnEdit.Enabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }
    }
}
