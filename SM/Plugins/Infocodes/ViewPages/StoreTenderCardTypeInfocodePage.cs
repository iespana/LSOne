using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    public partial class StoreTenderCardTypeInfocodePage : UserControl, ITabView
    {
        
        private RecordIdentifier selectedID;
        private RecordIdentifier refRelationID;
        private RefTableEnum refTable;

        public RecordIdentifier CardtypeID;

        StoreCardType cardType ;
        
        public StoreTenderCardTypeInfocodePage()
        {
            InitializeComponent();

            refTable = RefTableEnum.Tender;
            refRelationID = RecordIdentifier.Empty;

            lvInfocodesImages.Images.Add(Properties.Resources.InfoCodes16);
            lvInfocodes.SmallImageList = lvInfocodesImages;

            lvInfocodes.ContextMenuStrip = new ContextMenuStrip();
            lvInfocodes.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            btnsAddRemove.AddButtonEnabled = btnsAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);
            
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvInfocodes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   100,
                   new EventHandler(btnsAddRemove_AddButtonClicked));

            item.Enabled = btnsAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    200,
                    new EventHandler(btnsAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InfocodesList", lvInfocodes.ContextMenuStrip, lvInfocodes);

            e.Cancel = (menu.Items.Count == 0);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreTenderCardTypeInfocodePage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            cardType = (StoreCardType) internalContext;
            //var paymentMethod = (StorePaymentMethod)internalContext;
            //isCardPayment = paymentMethod.PosOperation.PrimaryID == 201;
            // Store tender type
            refTable = RefTableEnum.CreditCard;

            // The context is set as:
            // 0 = card type ID
            // 1 = store id
            // 2 = tender type id

            refRelationID = new RecordIdentifier(context[1], // Store ID
                            new RecordIdentifier(context[2], // Tender Type ID
                                                 context[0]));

            LoadItems();
            btnsAddRemove.AddButtonEnabled = btnsAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit)
                && cardType.CardTypeID != RecordIdentifier.Empty;
        }

        public bool DataIsModified()
        {                        
            return false;
        }

        public bool SaveData()
        {            
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //throw new NotImplementedException();
        }
        public static void TabMessage(TabControl sender, TabControl.Tab tab, int hint, object data)
        {
            //CardtypeID = (RecordIdentifier) data;
            //tab.Visible = true;
            sender.Invalidate();
        }
        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "InfocodeSpecific":
                    LoadItems();
                    break;
                case "CardTypeSelected":
                    btnsAddRemove.AddButtonEnabled = btnsAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit)
               && cardType.CardTypeID != RecordIdentifier.Empty;
                    refRelationID[1].SecondaryID = cardType.CardTypeID;
                    LoadItems();
                    break;
            }
        }
       
        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvInfocodes.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void LoadItems()
        {
            lvInfocodes.Items.Clear();
            lvInfocodes.Groups.Clear();

            List<InfocodeSpecific> items;

            items = Providers.InfocodeSpecificData.GetListForRefRelation(PluginEntry.DataModel, refRelationID, UsageCategoriesEnum.None, refTable, InfocodeSpecificSorting.InfocodeDescription, false);

            ListViewItem listItem;
            ListViewGroup grpInfocodes = new ListViewGroup(Properties.Resources.Infocodes);
            ListViewGroup grpGroups = new ListViewGroup(Properties.Resources.Groups);

          
            lvInfocodes.Groups.Add(grpInfocodes);
            lvInfocodes.Groups.Add(grpGroups);

            foreach (InfocodeSpecific infocodeSpecific in items)
            {
                listItem = new ListViewItem(infocodeSpecific.InfocodeDescription);
                listItem.Tag = infocodeSpecific.ID;
                listItem.ImageIndex = 0;

                if (infocodeSpecific.InfocodeInputType == InputTypesEnum.Group)
                {
                    listItem.Group = grpGroups;
                }
                else
                {
                    listItem.Group = grpInfocodes;
                }

                lvInfocodes.Items.Add(listItem);
            }

            lvInfocodes_SelectedIndexChanged(this, EventArgs.Empty);
        }
     

        private void lvInfocodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsAddRemove.RemoveButtonEnabled = lvInfocodes.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);
            selectedID = lvInfocodes.SelectedItems.Count > 0 ? (RecordIdentifier)lvInfocodes.SelectedItems[0].Tag : RecordIdentifier.Empty;
        }

        private void btnsAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            var addedInfocode = PluginOperations.NewInfocodeSpecific(refRelationID[0], refRelationID[1],
                cardType.CardTypeID, refTable, UsageCategoriesEnum.None, RecordIdentifier.Empty, true);
            if (addedInfocode
                != null)
            {
                LoadItems();
            }
        }

        private void btnsAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (PluginOperations.DeleteInfocodeSpecific(selectedID))
            {
                LoadItems();
            }
        }
    }
}
