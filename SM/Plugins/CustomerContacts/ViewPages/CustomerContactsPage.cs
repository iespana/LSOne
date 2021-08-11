using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.CustomerContacts.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerContacts.ViewPages
{
    public partial class CustomerContactsPage : UserControl, ITabView
    {
        bool contactIDChanged;
        RecordIdentifier customerID;
        Customer customer;
        RecordIdentifier selectedContactID;

        public CustomerContactsPage()
        {
            ImageList images;

            selectedContactID = RecordIdentifier.Empty;
            contactIDChanged = false;

            InitializeComponent();

            lvContacts.Columns[0].Tag = ContactSorting.Name;
            lvContacts.Columns[1].Tag = ContactSorting.CompanyName;
            lvContacts.Columns[2].Tag = ContactSorting.Phone;
            lvContacts.Columns[3].Tag = ContactSorting.Address;

            images = PluginEntry.Framework.GetImageList();

            imageList1.Images.Add(images.Images[0]);
            imageList1.Images.Add(images.Images[1]);
            imageList1.Images.Add(images.Images[2]);

            lvContacts.ContextMenuStrip = new ContextMenuStrip();
            lvContacts.ContextMenuStrip.Opening += new CancelEventHandler(lvContacts_Opening);

            lvContacts.SortedBackwards = false;
            lvContacts.SortColumn = 0;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CustomerContactsPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);
        }

        void LoadContacts()
        {
            lvContacts.Items.Clear();

            var contacts = Providers.ContactData.
                GetList(PluginEntry.DataModel, ContactRelationTypeEnum.Customer, customerID,
                (ContactSorting)lvContacts.Columns[lvContacts.SortColumn].Tag, lvContacts.SortedBackwards);

            foreach (Contact contact in contacts)
            {
                var item = new ListViewItem(PluginEntry.DataModel.Settings.NameFormatter.Format(contact.Name));
                item.SubItems.Add(contact.CompanyName);
                item.SubItems.Add(contact.Phone);
                item.SubItems.Add(PluginEntry.DataModel.Settings.LocalizationContext.FormatSingleLine(contact.Address,PluginEntry.DataModel.Cache));
                item.Tag = contact.ID;
                item.ImageIndex = -1;

                lvContacts.Add(item);

                if (selectedContactID == contact.ID)
                {
                    item.Selected = true;
                }
            }

            lvContacts.Columns[lvContacts.SortColumn].ImageIndex = (lvContacts.SortedBackwards ? 1 : 0);

            lvContacts.BestFitColumns();

            lvContacts_SelectedIndexChanged(this, EventArgs.Empty);
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            customerID = context;
            customer = (Customer)internalContext;

            LoadContacts();
        }

        public bool DataIsModified()
        {
            if (contactIDChanged)
            {
                customer.Dirty = true;
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
            contexts.Add(new AuditDescriptor("Contacts", new RecordIdentifier(customer.ID,(int)ContactRelationTypeEnum.Customer), Properties.Resources.Contacts, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvContacts.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier contactID = ContactDialog.NewContact(
                PluginEntry.DataModel,
                PluginEntry.Framework, 
                customerID, 
                ContactRelationTypeEnum.Customer, 
                PluginEntry.Framework.MainWindow);

            if (contactID != RecordIdentifier.Empty)
            {
                LoadContacts();
            }
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            ContactDialog dlg = new ContactDialog(
                PluginEntry.DataModel,
                PluginEntry.Framework,
                (RecordIdentifier)lvContacts.SelectedItems[0].Tag,
                btnsEditAddRemove.AddButtonEnabled);

            if(dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                LoadContacts();
            }
        }

        private void lvContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = (lvContacts.SelectedItems.Count == 1);
            btnsEditAddRemove.RemoveButtonEnabled = (lvContacts.SelectedItems.Count > 0) && btnsEditAddRemove.AddButtonEnabled;
         
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                lvContacts.SelectedItems.Count == 1 ? Properties.Resources.DeleteContactQuestion : Properties.Resources.DeleteContactsQuestion, 
                lvContacts.SelectedItems.Count == 1 ? Properties.Resources.DeleteContact : Properties.Resources.DeleteContacts) == DialogResult.Yes)
            {
                foreach (ListViewItem item in lvContacts.SelectedItems)
                {
                    Providers.ContactData.Delete(PluginEntry.DataModel, (RecordIdentifier)item.Tag, ContactRelationTypeEnum.Customer);
                }

                LoadContacts();
            }
        }

        private void lvContacts_DoubleClick(object sender, EventArgs e)
        {
            if (lvContacts.SelectedItems.Count > 0 && btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }


        void lvContacts_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvContacts.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
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


            PluginEntry.Framework.ContextMenuNotify("CustomerContactList", lvContacts.ContextMenuStrip, lvContacts);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvContacts_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvContacts.SortColumn == e.Column)
            {
                lvContacts.SortedBackwards = !lvContacts.SortedBackwards;
            }
            else
            {
                if (lvContacts.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvContacts.Columns[lvContacts.SortColumn].ImageIndex = 2;

                    lvContacts.SortColumn = e.Column;
                }
                lvContacts.SortedBackwards = false;
            }

            LoadContacts();
        }

    }
}
