using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.ViewPlugins.RetailItems
{
    class RetailItemSearchPanelFactory : ISearchPanelFactory
    {
        internal static List<SimpleRetailItem> rows;
        private RetailItemSearchPanel searchPanel = null;

        #region ISearchPanelProvider Members

        public void Add()
        {
            PluginOperations.NewItem();
        }

        public void Delete(SearchListViewItem item)
        {
            if(item == null)
                PluginOperations.DeleteItem(((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]).ID, ((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]).MasterID);
            else
                PluginOperations.DeleteItem(item.ID, item.MasterID);
        }

        public void Edit(SearchListViewItem item)
        {
            if (item == null)
                DoubleClickedItem((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]);
            else
                DoubleClickedItem(item);
        }


        public void GetItemAllowedOperations(SearchListViewItem item, ref bool canEdit, ref bool canDelete, ref bool canRun)
        {
            canEdit = true;
            canDelete = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);
        }

        public SearchPanelBase GetPanel()
        {
            if (searchPanel == null)
            {
                searchPanel = new RetailItemSearchPanel(this);
                searchPanel.ItemList.DoubleClick += new EventHandler(searchPanel_ItemDoubleClick);
            }
            return searchPanel;
        }

        public void Close()
        {
            searchPanel = null;
        }

        public string SearchContextName
        {
            get { return Properties.Resources.RetailItems; }
        }

        public void ReceiveChangeInformation(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (searchPanel != null)
            {
                if (objectName == "RetailItem")
                {
                    if (changeHint == DataEntityChangeType.Delete)
                    {
                        // We can handle this one locally.
                        foreach (SearchListViewItem item in searchPanel.ItemList.Items)
                        {
                            if (item.Key == "RetailItem" && item.ID == changeIdentifier)
                            {
                                searchPanel.ItemList.Items.Remove(item);
                                break;
                            }
                        }
                    }
                    else if (changeHint == DataEntityChangeType.MultiDelete)
                    {
                        foreach (SearchListViewItem item in searchPanel.ItemList.Items)
                        {
                            if (item.Key == "RetailItem" && ((List<RecordIdentifier>)param).Contains(item.ID))
                            {
                                searchPanel.ItemList.Items.Remove(item);
                                break;
                            }
                        }
                    }
                    else
                    {
                        searchPanel.SearchAgain();
                    }
                }
            }
        }

        public void SearchAllHandler(string text, List<SearchListViewItem> items,int maxDisplay)
        {
            SearchListViewItem item;

            if (text == null)
            {
                text = "";
            }

            rows = Providers.RetailItemData.Search(PluginEntry.DataModel, text, 0, maxDisplay,true,SortEnum.Description,false);

            foreach (SimpleRetailItem row in rows)
            {
                item = new SearchListViewItem(this, (string)row.ID,"RetailItem");

                item.SubItems.Add((string)row.Text);
                item.ID = row.ID;
                item.MasterID = row.MasterID;

                switch (row.ItemType)
                {
                    case ItemTypeEnum.Item:
                        item.ImageIndex = row.HeaderItemID == RecordIdentifier.Empty ? PluginEntry.RetailItemImageIndex : PluginEntry.VariantItemImageIndex;
                        break;
                    case ItemTypeEnum.Service:
                        item.ImageIndex = PluginEntry.ServiceItemImageIndex;
                        break;
                    case ItemTypeEnum.MasterItem:
                        item.ImageIndex = PluginEntry.MasterItemImageIndex;
                        break;
                    case ItemTypeEnum.BOM:
                    case ItemTypeEnum.AssemblyItem:
                        item.ImageIndex = PluginEntry.AssemblyItemImageIndex;
                        break;
                }

                items.Add(item);
            }
        }

        public void DoubleClickedItem(SearchListViewItem item)
        {

            PluginOperations.ShowItemSheet((string)item.ID, rows.Cast<IDataEntity>());
        }

        public int ItemCount
        {
            get
            {
                return searchPanel.ItemList.Items.Count;
            }
        }

        #endregion

        void searchPanel_ItemDoubleClick(object sender, EventArgs e)
        {
            if (searchPanel.ItemList.SelectedItems.Count > 0)
            {
                DoubleClickedItem((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]);
            }
        }
    }
}
