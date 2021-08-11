using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class VendorContactsPage : UserControl, ITabView
    {
        bool contactIDChanged;
        RecordIdentifier vendorID;
        Vendor vendor;
        RecordIdentifier selectedContactID;

        public VendorContactsPage()
        {
            ImageList images;

            selectedContactID = RecordIdentifier.Empty;
            contactIDChanged = false;

            InitializeComponent();

            images = PluginEntry.Framework.GetImageList();

            imageList1.Images.Add(images.Images[0]);
            imageList1.Images.Add(images.Images[1]);
            imageList1.Images.Add(images.Images[2]);
            imageList1.Images.Add(Properties.Resources.Default16);

            lvContacts.ContextMenuStrip = new ContextMenuStrip();
            lvContacts.ContextMenuStrip.Opening += new CancelEventHandler(lvContacts_Opening);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.VendorContactsPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.VendorEdit);
        }

        void LoadContacts()
        {
            List<Contact> contacts;

            lvContacts.ClearRows();

            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            try
            {
                contacts = service.GetVendorContactList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorID, true);

                Row row;
                foreach (Contact contact in contacts)
                {
                    row = new Row();
                    string name = PluginEntry.DataModel.Settings.NameFormatter.Format(contact.Name);
                    row.AddText(string.IsNullOrEmpty(name) ? contact.Text : name);
                    row.AddText(contact.CompanyName);
                    row.AddText(contact.Phone);
                    row.AddText(PluginEntry.DataModel.Settings.LocalizationContext.FormatSingleLine(contact.Address, PluginEntry.DataModel.Cache));

                    row.Tag = contact.ID;

                    lvContacts.AddRow(row);

                    if (contact.ID == vendor.DefaultContactID)
                    {
                        row.BackColor = ColorPalette.GreenDark;
                    }
                }
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            lvContacts.AutoSizeColumns();

            lvContacts_SelectionChanged(this, EventArgs.Empty);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            vendorID = context;
            vendor = (Vendor)internalContext;

            LoadContacts();
        }

        public bool DataIsModified()
        {
            if (contactIDChanged)
            {
                vendor.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            contactIDChanged = false;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Contacts", new RecordIdentifier(vendor.ID,(int)ContactRelationTypeEnum.Vendor), Properties.Resources.Contacts, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeHint == DataEntityChangeType.Edit && objectName == "VendorDefaultContact")
            {
                Vendor currentVendorInfo = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendor.ID, true);
                vendor.DefaultContactID = currentVendorInfo.DefaultContactID;
            }

            if (changeHint == DataEntityChangeType.Add && objectName == "VendorContact")
            {
                LoadContacts();
            }
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewVendorContact(vendorID);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            if (lvContacts.Selection.Count == 0)
            {
                return;
            }

            PluginOperations.EditVendorContact((RecordIdentifier)lvContacts.Selection[0].Tag);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                lvContacts.Selection.Count == 1 ? Properties.Resources.DeleteContactQuestion : Properties.Resources.DeleteContactsQuestion, 
                lvContacts.Selection.Count == 1 ? Properties.Resources.DeleteContact : Properties.Resources.DeleteContacts) == DialogResult.Yes)
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                List<RecordIdentifier> contactsToDelete = new List<RecordIdentifier>();
                for (int i = 0; i < lvContacts.Selection.Count; i++)
                {
                    contactsToDelete.Add((RecordIdentifier)lvContacts.Selection[i].Tag);
                }

                try
                {
                    service.DeleteVendorContact(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), contactsToDelete, true);
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }

                LoadContacts();
            }
        }

        private void btnSetAsDefault_Click(object sender, EventArgs e)
        {
            // Set the contact to default if its our first contact
            vendor.DefaultContactID = (RecordIdentifier)lvContacts.Selection[0].Tag;
            contactIDChanged = true;

            LoadContacts();
        }

        void lvContacts_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvContacts.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                //Image = Properties.Resources.EditImage,
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    btnSetAsDefault.Text,
                    500,
                    new EventHandler(btnSetAsDefault_Click));

            item.Enabled = btnSetAsDefault.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("VendorContactList", lvContacts.ContextMenuStrip, lvContacts);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvContacts_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = (lvContacts.Selection.Count == 1);
            btnsEditAddRemove.RemoveButtonEnabled = (lvContacts.Selection.Count > 0) && btnsEditAddRemove.AddButtonEnabled;
            btnSetAsDefault.Enabled = (lvContacts.Selection.Count == 1) && btnsEditAddRemove.AddButtonEnabled && vendor.DefaultContactID != (RecordIdentifier)lvContacts.Selection[0].Tag;
        }

        private void lvContacts_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (lvContacts.Selection.Count > 0 && btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
