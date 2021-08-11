using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class CustomerCardsView : ViewBase
    {
        RecordIdentifier selectedID = "";

        public CustomerCardsView(RecordIdentifier cardId)
            :base()
        {
            selectedID = cardId;
        }

        public CustomerCardsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.CustomerCards;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageMsrCardLinks);

            //lvCustomerCards.Columns[0].Tag = DataProviders.StoreManagement.StoreSorting.ID;
            //lvCustomerCards.Columns[1].Tag = DataProviders.StoreManagement.StoreSorting.Name;
            //lvCustomerCards.Columns[2].Tag = DataProviders.StoreManagement.StoreSorting.City;

            //lvCustomerCards.SmallImageList = PluginEntry.Framework.GetImageList();
            lvCustomerCards.ContextMenuStrip = new ContextMenuStrip();
            lvCustomerCards.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageMsrCardLinks);

            lvCustomerCards.SortColumn = 1;
        }        

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("MsrCardLinks", (int)MsrCardLink.LinkTypeEnum.Customer, Properties.Resources.CustomerCards, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CustomerCards;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadItems();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "CustomerCard":
                    if (changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Add)
                    {
                        selectedID = changeIdentifier;
                    }                    

                    LoadItems();
                    break;

                case "MsrCardLink":
                    if (changeHint == DataEntityChangeType.Delete)
                    {
                        if (((MsrCardLink.LinkTypeEnum)param) == MsrCardLink.LinkTypeEnum.Customer)
                        {
                            LoadItems();
                        }
                    }
                    break;
            }

        }

        private void LoadItems()
        {
            List<MsrCardLink> customerCards;
            ListViewItem listItem;

            lvCustomerCards.Items.Clear();

            // Get all customer cards            
            customerCards = Providers.MsrCardLinkData.GetList(PluginEntry.DataModel, MsrCardLink.LinkTypeEnum.Customer);

            foreach (MsrCardLink customerCard in customerCards)
            {
                listItem = new ListViewItem((string)customerCard.ID);
                //listItem.SubItems.Add((string)customerCard.LinkID);                
                listItem.SubItems.Add(customerCard.Text);
                //listItem.ImageIndex = -1;

                listItem.Tag = customerCard.ID;

                lvCustomerCards.Add(listItem);

                if (selectedID == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvStores_SelectedIndexChanged(this, EventArgs.Empty);

            lvCustomerCards.BestFitColumns();
        }

        private void lvStores_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = lvCustomerCards.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageMsrCardLinks);

            // We only want to be able to delete stores from the head office
            btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.EditButtonEnabled;
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {            
            PluginOperations.EditCustomerCard((RecordIdentifier)lvCustomerCards.SelectedItems[0].Tag);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {                     
            PluginOperations.NewCustomerCard();
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {            
            PluginOperations.DeleteMsrCardLink((RecordIdentifier)lvCustomerCards.SelectedItems[0].Tag, MsrCardLink.LinkTypeEnum.Customer);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvCustomerCards.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
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

            PluginEntry.Framework.ContextMenuNotify("CustomerCardsList", lvCustomerCards.ContextMenuStrip, lvCustomerCards);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvStores_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvStores_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvCustomerCards.SortColumn == e.Column)
            {
                lvCustomerCards.SortedBackwards = !lvCustomerCards.SortedBackwards;
            }
            else
            {
                if (lvCustomerCards.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvCustomerCards.Columns[lvCustomerCards.SortColumn].ImageIndex = 2;

                    lvCustomerCards.SortColumn = e.Column;
                }
                lvCustomerCards.SortedBackwards = false;
            }

            LoadItems();
        }

        protected override void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvCustomerCards.SmallImageList = null;

            base.OnClose();
        }
    }
}
