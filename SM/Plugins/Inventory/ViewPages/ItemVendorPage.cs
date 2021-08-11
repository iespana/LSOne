using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.Controls;
using System.Drawing;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Services.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    internal partial class ItemVendorPage : UserControl, ITabViewV2, IMultiEditTabExtension
    {
        RetailItem item;

        bool defaultVendorChanged;
        bool suspendBackwardMessage;

        private const int SAVEDCOLUMN = 3;
        private SiteServiceProfile siteServiceProfile;

        IInventoryService service;
        private class ExtendedCellWithFlags : ExtendedCell
        {
            public bool Saved;
            public bool AddAction;
        }

        public ItemVendorPage()
        {
            suspendBackwardMessage = false;
            InitializeComponent();

            lblLocalDatabase.Text = "";
            if (PluginOperations.TestSiteService(false))
            {
                siteServiceProfile = PluginOperations.GetSiteServiceProfile();
                
            }

            defaultVendorChanged = false;

            btnsAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.VendorEdit) ;

            btnsAddRemove.RemoveButtonEnabled = btnsAddRemove.AddButtonEnabled ;

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);

            lvItems_SelectionChanged(this, EventArgs.Empty);
        }

        public static ITabView CreateInstance(object sender,  TabControl.Tab tab)
        {
            return new ViewPages.ItemVendorPage();
        }

        #region ITabPanel Members

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            if (internalContext != null)
            {
                item = (RetailItem)internalContext;

                if (item.ID == RecordIdentifier.Empty)
                {
                    // We are in multi edit mode
                    btnsAddRemove.RemoveButtonEnabled = true;

                    lvItems.Columns.RemoveAt(lvItems.Columns.Count - 1);
                    lvItems.Columns.RemoveAt(lvItems.Columns.Count - 1);

                    lvItems.Columns.Add(new LSOne.Controls.Columns.Column() { HeaderText = Properties.Resources.Action, Clickable = false, AutoSize = true, Sizable = false });
                    lvItems.Columns.Add(new LSOne.Controls.Columns.Column() { HeaderText = "", Clickable = false, AutoSize = true, Sizable = false, MinimumWidth = 20 });

                    // In multiedit we cannot sort since that would distory the time order of the transactions.
                    lvItems.Columns[0].Clickable = false;
                    lvItems.Columns[0].InternalSort = false;

                    lvItems.Columns[1].Clickable = false;
                    lvItems.Columns[1].InternalSort = false;

                    lvItems.Columns[2].Clickable = false;
                    lvItems.Columns[2].InternalSort = false;

                    lvItems.AutoSizeColumns();

                    lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRoundedColorMerge;

                    
                }
            }
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;
            LoadItems();
        }

        private void AddItem(VendorItem vendorForItem, DecimalLimit decimalLimiter, bool isRemoveAction)
        {
            Row row;

            string lastOrderDateDescription = (vendorForItem.LastOrderDate != Date.Empty) ? vendorForItem.LastOrderDate.ToShortDateString() : "";

            row = new Row();

            row.AddCell(new ExtendedCell(vendorForItem.VendorDescription) { ImagePlaceHolderWidth = 16 });
            row.AddText(vendorForItem.UnitDescription);
            row.AddCell(new NumericCell(vendorForItem.DefaultPurchasePrice.FormatWithLimits(decimalLimiter), vendorForItem.DefaultPurchasePrice));
            if (item.ID == RecordIdentifier.Empty)
            {
                row.BackColor = ColorPalette.MultiEditHighlight;

                if (isRemoveAction)
                {
                    row.AddCell(new ExtendedCellWithFlags() { Image = ContextButtons.GetRemoveButtonImage(), Saved = false, AddAction = false, Text = Properties.Resources.Delete });
                }
                else
                {
                    row.AddCell(new ExtendedCellWithFlags() { Image = ContextButtons.GetAddButtonImage(), Saved = false, AddAction = true, Text = Properties.Resources.Add });
                }

                row.AddCell(new IconButtonCell(new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete),Properties.Resources.RemoveAction),IconButtonCell.IconButtonIconAlignmentEnum.Left | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter));
            }
            else
            {
                if (lastOrderDateDescription != "")
                {
                    row.AddCell(new NumericCell(vendorForItem.LastItemPrice.FormatWithLimits(decimalLimiter), vendorForItem.LastItemPrice));
                }
                else
                {
                    row.AddText("");
                }

                row.AddCell(new DateCell(vendorForItem.LastOrderDate.ToShortDateString(), vendorForItem.LastOrderDate));
            }

            row.Tag = vendorForItem;


            if (vendorForItem.VendorID == item.DefaultVendorID)
            {
                ((ExtendedCell)row[0]).Image = Properties.Resources.Default16;
            }

            lvItems.AddRow(row);
        }

        private void LoadItems()
        {            
            List<VendorItem> vendorsForItem;
            errorProvider1.Clear();
            lblLocalDatabase.Text = "";

            lvItems.ClearRows();

            DecimalLimit decimalLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            try
            {
                vendorsForItem = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendorsForItem(
                    PluginEntry.DataModel, 
                    siteServiceProfile,
                    item.ID, 
                    VendorItemSorting.Description,
                    false, 
                    true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
           
               
            foreach (VendorItem vendorForItem in vendorsForItem)
            {
                AddItem(vendorForItem, decimalLimiter, false);
            }

            lvItems.Sort();
            lvItems.AutoSizeColumns();

            lvItems_SelectionChanged(this, EventArgs.Empty);

            //if (useLocalDatabase)
            //{
            //    lblLocalDatabase.Text = Resources.VendorInfoFromLocalDatabaseCannotBeEdited;
            //    errorProvider1.SetError(lblLocalDatabase, Resources.PressF1ForFurtherInformation);
            //}

        }

        public bool DataIsModified()
        {
            if (defaultVendorChanged)
            {
                item.Dirty = true;
                return true;
            }

            return false;

        }

        public bool SaveData()
        {
            defaultVendorChanged = false;

            return true;
        }

        // TODO
        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (!suspendBackwardMessage)
            {
                if (objectName == "VendorItem" && changeIdentifier == item.ID)
                {
                    LoadItems();
                }
            }
        }

        public void OnClose()
        {
            
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            
            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(AddButtonClicked));

            item.Enabled = btnsAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);
            item = new ExtendedMenuItem(
                  Resources.Edit,
                  100,
                  btnsAddRemove_EditButtonClicked);

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);
            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(RemoveButtonClicked));

            item.Enabled = btnsAddRemove.RemoveButtonEnabled;
            item.Image = ContextButtons.GetRemoveButtonImage();
            menu.Items.Add(item);


            if (this.item.ID == RecordIdentifier.Empty)
            {
                item = new ExtendedMenuItem(
                    Properties.Resources.RemoveAction,
                    330, new EventHandler(RemoveAction));

                item.Image = PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete);

                item.Enabled = lvItems.Selection.Count > 0 && (!((ExtendedCellWithFlags)lvItems.Row(lvItems.Selection.FirstSelectedRow)[SAVEDCOLUMN]).Saved);

                menu.Items.Add(item);
            }

            item = new ExtendedMenuItem(
                    Properties.Resources.ViewVendor,
                    350,
                    new EventHandler(ViewVendor));

            item.Enabled = btnViewVendor.Enabled;

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    btnSetAsDefault.Text,
                    500,
                    new EventHandler(btnSetAsDefault_Click));

            item.Enabled = btnSetAsDefault.Enabled;
            //item.Image = ContextButtons.GetRemoveButtonImage();
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("VendorsForItem", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void RemoveAction(object sender, EventArgs args)
        {
            lvItems.RemoveRow(lvItems.Selection.FirstSelectedRow);
        }

        private void AddAction(bool isRemoveAction, VendorItem vendorItem = null)
        {
            suspendBackwardMessage = true;

            ItemVendorDialog dlg;
            if (vendorItem != null)
            {

                dlg = new ItemVendorDialog(item.ID, vendorItem, item.ID == RecordIdentifier.Empty);
            }
            else
            {
                dlg = new ItemVendorDialog(item.ID, item.ID == RecordIdentifier.Empty);
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DecimalLimit decimalLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

                if (item.ID != RecordIdentifier.Empty)
                {
                    // if we are not in multiedit and the row is the first row added then we set default vendor
                    if (lvItems.RowCount == 0)
                    {
                        item.DefaultVendorID = dlg.VendorItem.VendorID;
                        item.Dirty = true;

                        try
                        {
                            if(service == null)
                            {
                                service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                            }

                            service.SetItemsDefaultVendor(PluginEntry.DataModel, siteServiceProfile, item.ID, item.DefaultVendorID, true);
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        }
                    }

                    LoadItems();

                }
                else
                {
                    AddItem(dlg.VendorItem, decimalLimiter, isRemoveAction);
                }

                lvItems.Sort();
                lvItems.AutoSizeColumns();
            }

            suspendBackwardMessage = false;

        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            AddAction(false);
        }

        private void RemoveButtonClicked(object sender, EventArgs e)
        {

            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            if (item.ID == RecordIdentifier.Empty)
            {
                // Multi edit
                AddAction(true);
            }
            else
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteVendorFromItemQuestion,
                    Properties.Resources.DeleteVendorFromItem) == DialogResult.Yes)
                {
                    Row selectedRow = lvItems.Row(lvItems.Selection.FirstSelectedRow);

                    try
                    {
                        service.DeleteVendorItem(PluginEntry.DataModel,siteServiceProfile, ((VendorItem)selectedRow.Tag).ID, false);                 

                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        return;
                    }

                    lvItems.RemoveRow(lvItems.Selection.FirstSelectedRow);

                    if (((VendorItem)selectedRow.Tag).VendorID == item.DefaultVendorID)
                    {
                        item.DefaultVendorID = "";
                        item.Dirty = true;

                        if (lvItems.RowCount > 0)
                        {
                            selectedRow = lvItems.Row(0);

                            item.DefaultVendorID = ((VendorItem)selectedRow.Tag).VendorID;

                            ((ExtendedCell)selectedRow[0]).Image = Resources.Default16;
                        }

                        try
                        {
                            service.SetItemsDefaultVendor(PluginEntry.DataModel, siteServiceProfile, item.ID, item.DefaultVendorID, true);
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                        }
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(sender, DataEntityChangeType.Delete, "VendorsForItem", item.ID, null);
                }
            }
        }

        private void btnSetAsDefault_Click(object sender, EventArgs e)
        {
            Row selectedRow = lvItems.Row(lvItems.Selection.FirstSelectedRow);

            if (item.ID != RecordIdentifier.Empty)
            {
                try
                {
                    service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    service.SetItemsDefaultVendor(PluginEntry.DataModel, siteServiceProfile, item.ID,
                        ((VendorItem) selectedRow.Tag).VendorID, true);

                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    return;
                }
                // Single edit
                item.DefaultVendorID = ((VendorItem) selectedRow.Tag).VendorID;
                item.Dirty = true;
            }

            foreach (Row row in lvItems.Rows)
            {
                ((ExtendedCell) row[0]).Image = null;
            }

            ((ExtendedCell) selectedRow[0]).Image = Properties.Resources.Default16;

            lvItems.Invalidate();
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
           
            btnsAddRemove.RemoveButtonEnabled = ((lvItems.Selection.Count > 0) && btnsAddRemove.AddButtonEnabled) || (item != null && item.ID == RecordIdentifier.Empty);
            btnViewVendor.Enabled = btnsAddRemove.RemoveButtonEnabled;
            if (item == null || item.ID == RecordIdentifier.Empty)
            {
                btnSetAsDefault.Enabled = (lvItems.Selection.Count > 0) && btnsAddRemove.AddButtonEnabled && item.DefaultVendorID != ((VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).VendorID && ((ExtendedCellWithFlags)lvItems.Row(lvItems.Selection.FirstSelectedRow)[SAVEDCOLUMN]).AddAction;
            }
            else
            {
                btnSetAsDefault.Enabled = (lvItems.Selection.Count > 0) && btnsAddRemove.AddButtonEnabled && item.DefaultVendorID != ((VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).VendorID;
            }
            btnsAddRemove.EditButtonEnabled = btnsAddRemove.AddButtonEnabled && lvItems.Selection.Count == 1;
        }


        private void ViewVendor(object sender, EventArgs e)
        {
            Row selectedRow = lvItems.Row(lvItems.Selection.FirstSelectedRow);

            RecordIdentifier vendorID = ((VendorItem)selectedRow.Tag).VendorID;
            PluginOperations.ShowVendor(vendorID);
        }

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            foreach(Row row in lvItems.Rows)
            {
               if(!((ExtendedCellWithFlags)row[SAVEDCOLUMN]).Saved)
               {
                    return true;
               }
            }

            return false;
        }

        public void MultiEditSaveSecondaryRecords(IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            // If we got header item then we just ignore it and move on
            if ((dataEntity as RetailItemMultiEdit).ItemType == DataLayer.BusinessObjects.Enums.ItemTypeEnum.MasterItem)
            {
                return;
            }

            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            bool hasCheckedDefault = false; // We only really need to check for default once per item, not once per item per vendor so we have this to control that.
            VendorItem vendorItem;
            RecordIdentifier salesUnitID = Providers.RetailItemData.GetItemUnitID(threadedConnection, primaryRecordID.PrimaryID, RetailItem.UnitTypeEnum.Sales);

            foreach (Row row in lvItems.Rows)
            {
                if (!((ExtendedCellWithFlags)row[SAVEDCOLUMN]).Saved)
                {
                    vendorItem = ((VendorItem)row.Tag);
                    vendorItem.ID = RecordIdentifier.Empty;
                    vendorItem.UnitID = salesUnitID;

                    if (((ExtendedCellWithFlags) row[SAVEDCOLUMN]).AddAction)
                    {
                        try
                        {

                            if (
                                !service.VendorItemExists(threadedConnection, siteServiceProfile, vendorItem.VendorID,
                                    primaryRecordID.SecondaryID,
                                    //"", 
                                    salesUnitID, "", false))
                            {
                                vendorItem.RetailItemID = primaryRecordID.SecondaryID;

                                vendorItem.ID = service.SaveVendorItem(threadedConnection, siteServiceProfile, vendorItem, false);

                                if (((ExtendedCell) row[0]).Image != null)
                                {
                                    Providers.RetailItemData.SetItemsDefaultVendor(threadedConnection,
                                        primaryRecordID.PrimaryID, vendorItem.VendorID);
                                    service.SetItemsDefaultVendor(PluginEntry.DataModel, siteServiceProfile,
                                        primaryRecordID.PrimaryID, vendorItem.VendorID, false);
                                }
                                else
                                {
                                    // Check if this is the first item for the vendor, if so he should be the items default vendor
                                    if (!hasCheckedDefault)
                                    {
                                        hasCheckedDefault = true;
                                        if (
                                            !service.ItemHasDefaultVendor(threadedConnection, siteServiceProfile,
                                                primaryRecordID.PrimaryID, false))
                                        {
                                            service.SetItemsDefaultVendor(threadedConnection, siteServiceProfile,
                                                primaryRecordID, vendorItem.VendorID, true);
                                            Providers.RetailItemData.SetItemsDefaultVendor(threadedConnection,
                                                primaryRecordID.PrimaryID, vendorItem.VendorID);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                            return;
                        }

                    }
                    else
                    {
                        try
                        {
                            service.DeleteByRetailItemID(threadedConnection, siteServiceProfile,
                                primaryRecordID.SecondaryID,
                                vendorItem.VendorID, vendorItem.UnitID, true);
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                            return;
                        }
                        // Delete action

                        //Providers.VendorItemData.DeleteByRetailItemID(threadedConnection, primaryRecordID.SecondaryID,
                        //    vendorItem.VendorID, vendorItem.UnitID);
                    }

                }
            }
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {
            foreach (Row row in lvItems.Rows)
            {
                if (!((ExtendedCellWithFlags)row[SAVEDCOLUMN]).Saved)
                {
                    ((ExtendedCellWithFlags)row[SAVEDCOLUMN]).Saved = true;
                    row.BackColor = Color.Empty;
                }

                row[SAVEDCOLUMN + 1] = new Cell("");
            }

            lvItems.Invalidate();
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            if (control == lvItems)
            {
                if (lvItems.Selection.Count > 0)
                {
                    lvItems.RemoveRow(lvItems.Selection.FirstSelectedRow);
                }

                lvItems.Invalidate();
            }

            
        }

        private void lvItems_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            lvItems.RemoveRow(args.RowNumber);
        }

        private void btnsAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            Row selectedRow = lvItems.Row(lvItems.Selection.FirstSelectedRow);

            AddAction(false, ((VendorItem)selectedRow.Tag));
        }

        private void lvItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsAddRemove.EditButtonEnabled)
            {
                Row selectedRow = lvItems.Row(lvItems.Selection.FirstSelectedRow);

                AddAction(false, ((VendorItem)selectedRow.Tag));
            }
        }
    }
}
