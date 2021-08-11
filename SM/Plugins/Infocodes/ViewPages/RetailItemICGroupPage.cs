﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
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
    public partial class RetailItemICGroupPage : UserControl, ITabView
    {
        
        RecordIdentifier selectedID;
        RecordIdentifier refRelationID;
        RefTableEnum refTable;
        UsageCategoriesEnum usageCategory;
        RecordIdentifier extraRetailGroupID; // This is used if we are dealing with an item and would like to get the item's retail group infocodes too

        public RetailItemICGroupPage(UsageCategoriesEnum usageCategory)
        {
            InitializeComponent();

            refTable = RefTableEnum.All;
            refRelationID = RecordIdentifier.Empty;
            this.usageCategory = usageCategory;

            lvInfocodesImages.Images.Add(Properties.Resources.InfoCodes16);
            lvInfocodes.SmallImageList = lvInfocodesImages;

            lvInfocodes.ContextMenuStrip = new ContextMenuStrip();
            lvInfocodes.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            btnsAddRemove.AddButtonEnabled  = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvInfocodes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                   Properties.Resources.Edit,
                   100,
                   new EventHandler(btnsAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsAddRemove.EditButtonEnabled,
                Default = true
            };
            menu.Items.Add(item);

            
            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
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

        public static ITabView CreateCSInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.RetailItemICGroupPage(UsageCategoriesEnum.CrossSelling);
        }

        public static ITabView CreateIMInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.RetailItemICGroupPage(UsageCategoriesEnum.ItemModifier);
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            // Retail Item
            if (internalContext is RetailItem)
            {
                refTable = RefTableEnum.Item;
                refRelationID = new RecordIdentifier(((RetailItem)internalContext).ID,
                                new RecordIdentifier(RecordIdentifier.Empty, RecordIdentifier.Empty));
                
                extraRetailGroupID = ((RetailItem)internalContext).RetailGroupID;

                LoadItems();
            }
            else if (internalContext is RetailGroup)
            {
                refTable = RefTableEnum.RetailGroup;
                refRelationID = new RecordIdentifier(((RetailGroup)internalContext).ID,
                                new RecordIdentifier(RecordIdentifier.Empty, RecordIdentifier.Empty));

                LoadItems();
            }

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
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "InfocodeSpecific":
                    LoadItems();
                    break;
                case "RetailItem":
                    // Our Retail group has been changed and that affects infocodes from it
                    if (refTable == RefTableEnum.Item && changeIdentifier == refRelationID.PrimaryID && param is RetailGroup)
                    {
                        extraRetailGroupID = ((RetailGroup)param).ID;

                        LoadItems();
                    }
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

            items = Providers.InfocodeSpecificData.GetListForRefRelation(PluginEntry.DataModel, refRelationID, usageCategory, refTable, InfocodeSpecificSorting.InfocodeDescription, false);

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

            // If we are dealing with an item, add infocodes from the items retail group also
            if (refTable == RefTableEnum.Item)
            {
                RecordIdentifier extraRefRelationID = new RecordIdentifier(extraRetailGroupID, new RecordIdentifier(RecordIdentifier.Empty, RecordIdentifier.Empty));

                var itemsFromRetailGroup =
                    Providers.InfocodeSpecificData.GetListForRefRelation(PluginEntry.DataModel, extraRefRelationID, usageCategory, RefTableEnum.RetailGroup, InfocodeSpecificSorting.InfocodeDescription, false);
                ListViewGroup grpRetailGroupInfocodes = new ListViewGroup(Properties.Resources.FromRetailGroupInfocodes);
                ListViewGroup grpRetailGroupGroups = new ListViewGroup(Properties.Resources.FromRetailGroupGroups);

                lvInfocodes.Groups.Add(grpRetailGroupInfocodes);
                lvInfocodes.Groups.Add(grpRetailGroupGroups);

                foreach (InfocodeSpecific infocodeSpecific in itemsFromRetailGroup)
                {
                    listItem = new ListViewItem(infocodeSpecific.InfocodeDescription);
                    listItem.Tag = infocodeSpecific.ID;
                    listItem.ImageIndex = 0;

                    if (infocodeSpecific.InfocodeInputType == InputTypesEnum.Group)
                    {
                        listItem.Group = grpRetailGroupGroups;
                    }
                    else
                    {
                        listItem.Group = grpRetailGroupInfocodes;
                    }

                    lvInfocodes.Items.Add(listItem);
                }
            }

            lvInfocodes_SelectedIndexChanged(this, EventArgs.Empty);
        }
     

        private void lvInfocodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsAddRemove.EditButtonEnabled = btnsAddRemove.RemoveButtonEnabled = lvInfocodes.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);
            selectedID = lvInfocodes.SelectedItems.Count > 0 ? (RecordIdentifier)lvInfocodes.SelectedItems[0].Tag : RecordIdentifier.Empty;
        }

        private void btnsAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewInfocodeSpecific(refRelationID, refTable, usageCategory, selectedID, false);
        }
        
        private void btnsAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewInfocodeSpecific(refRelationID, refTable, usageCategory, RecordIdentifier.Empty, true);
        }

        private void btnsAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteInfocodeSpecific(selectedID);
        }

        private void lvInfocodes_DoubleClick(object sender, EventArgs e)
        {
            PluginOperations.NewInfocodeSpecific(refRelationID, refTable, usageCategory, selectedID, false);
        }
    }
}
