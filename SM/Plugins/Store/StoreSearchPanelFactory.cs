using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Store
{
    class StoreSearchPanelFactory : ISearchPanelFactory
    {
        private StoreSearchPanel searchPanel = null;

        #region ISearchPanelProvider Members

        public void Add()
        {
            PluginOperations.NewStore();
        }

        public void Delete(SearchListViewItem item)
        {
            if(item == null)
                PluginOperations.DeleteStore(((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]).ID);
            else
                PluginOperations.DeleteStore(item.ID);
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
            canDelete = PluginEntry.DataModel.HasPermission(Permission.StoreEdit) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty);
        }

        public SearchPanelBase GetPanel()
        {
            if (searchPanel == null)
            {
                searchPanel = new StoreSearchPanel(this);
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
            get { return Properties.Resources.Stores; }
        }

        public void ReceiveChangeInformation(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (searchPanel != null)
            {
                if (objectName == "Store")
                {
                    if (changeHint == DataEntityChangeType.Delete)
                    {
                        // We can handle this one locally.
                        foreach (SearchListViewItem item in searchPanel.ItemList.Items)
                        {
                            if (item.Key == "Store" && item.ID == changeIdentifier)
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
            List<StoreListItem> rows;
            SearchListViewItem item;

            if (text == null)
            {
                text = "";
            }

            StoreListSearchFilter filter = new StoreListSearchFilter
            {
                DescriptionOrID = text,
                DescriptionOrIDBeginsWith = false,
                City = text,
                CityBeginsWith = false,
                MaxCount = maxDisplay
            };

            rows = Providers.StoreData.Search(PluginEntry.DataModel, filter);

            foreach (StoreListItem row in rows)
            {
                item = new SearchListViewItem(this, (string)row.ID,"Store");

                item.SubItems.Add((string)row.Text);
                item.ID = row.ID;

                item.ImageIndex = PluginEntry.StoreImageIndex;

                items.Add(item);
            }
        }

        public void DoubleClickedItem(SearchListViewItem item)
        {
            PluginOperations.ShowStore((string)item.ID);
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
