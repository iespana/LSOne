using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetail.StoreController;
using LSRetail.StoreController.Controls.SharedControls.Interfaces;
using LSRetail.StoreController.SharedCore;
using LSRetail.StoreController.SharedCore.Enums;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.BusinessObjects.Hospitality;
using LSRetail.StoreController.BusinessObjects.DataEntities.Hospitality;
using LSRetail.StoreController.BusinessObjects.DataEntities;
using LSRetail.StoreController.BusinessObjects;
using LSRetail.StoreController.BusinessObjects.DataEntities.Infocodes;
using LSRetail.StoreController.BusinessObjects.Infocodes;

namespace LSRetail.StoreController.Infocodes.ViewPages
{
    public partial class _RetailItemItemModifiersPage : UserControl, ITabView
    {
        
        private RecordIdentifier selectedID;
        private RecordIdentifier refRelationID;
        private TableRefId refTable;
        private UsageCategories usageCategory;

        public _RetailItemItemModifiersPage()
        {
            InitializeComponent();

            refTable = TableRefId.None;
            refRelationID = RecordIdentifier.Empty;
            usageCategory = UsageCategories.ItemModifier;

            lvInfocodesImages.Images.Add(Properties.Resources.InfoCodes16);
            lvInfocodes.SmallImageList = lvInfocodesImages;

            lvInfocodes.ContextMenuStrip = new ContextMenuStrip();
            lvInfocodes.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            btnsAddRemove.AddButtonEnabled = btnsAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvInfocodes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuitem(
                   Properties.Resources.Add,
                   100,
                   new EventHandler(btnsAddRemove_AddButtonClicked));

            item.Enabled = btnsAddRemove.AddButtonEnabled;
            item.Image = btnsAddRemove.AddButtonImage;

            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Delete,
                    200,
                    new EventHandler(btnsAddRemove_RemoveButtonClicked));

            item.Image = btnsAddRemove.RemoveButtonImage;
            item.Enabled = btnsAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InfocodesList", lvInfocodes.ContextMenuStrip, lvInfocodes);

            e.Cancel = (menu.Items.Count == 0);
        }

        public static ITabView CreateInstance(object sender, LSRetail.StoreController.Controls.TabControl.Tab tab)
        {
            return new ViewPages._RetailItemItemModifiersPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            // Retail Item
            refTable = TableRefId.Item;
            refRelationID = new RecordIdentifier(context,
                            new RecordIdentifier(RecordIdentifier.Empty, RecordIdentifier.Empty)); ;
                
            LoadItems();
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

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "InfocodeSpecific":
                    LoadItems();
                    break;
            }
        }

        #endregion

        private void LoadItems()
        {
            lvInfocodes.Items.Clear();
            lvInfocodes.Groups.Clear();

            List<InfocodeSpecific> items;

            items = Providers.InfocodeSpecificData.GetListForRefRelation(PluginEntry.DataModel, refRelationID, usageCategory);

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

                if (infocodeSpecific.InfocodeInputType == InputTypes.Group)
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
            btnsAddRemove.RemoveButtonEnabled = lvInfocodes.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.InfocodeEdit);
            selectedID = lvInfocodes.SelectedItems.Count > 0 ? (RecordIdentifier)lvInfocodes.SelectedItems[0].Tag : RecordIdentifier.Empty;
        }

        private void btnsAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewInfocodeSpecific(refRelationID, refTable, usageCategory, RecordIdentifier.Empty);
        }

        private void btnsAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteInfocodeSpecific(selectedID);
        }
    }
}
