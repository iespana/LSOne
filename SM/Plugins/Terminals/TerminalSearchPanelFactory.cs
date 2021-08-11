using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Terminals
{
    class TerminalSearchPanelFactory : ISearchPanelFactory
    {
        private TerminalSearchPanel searchPanel = null;

        #region ISearchPanelProvider Members

        public void GetItemAllowedOperations(SearchListViewItem item, ref bool canEdit, ref bool canDelete, ref bool canRun)
        {
            canEdit = true;
            canDelete = PluginEntry.DataModel.HasPermission(Permission.TerminalEdit) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == item.ID.SecondaryID);
        }

        public void Add()
        {
            PluginOperations.NewTerminal(this,EventArgs.Empty);
        }

        public void Delete(SearchListViewItem item)
        {
            if (item == null)
                PluginOperations.DeleteTerminal(((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]).ID);
            else
                PluginOperations.DeleteTerminal(item.ID);
        }

        public void Edit(SearchListViewItem item)
        {
            if (item == null)
                DoubleClickedItem((SearchListViewItem)searchPanel.ItemList.SelectedItems[0]);
            else
                DoubleClickedItem(item);
        }

        public SearchPanelBase GetPanel()
        {
            if (searchPanel == null)
            {
                searchPanel = new TerminalSearchPanel(this);
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
            get { return Properties.Resources.Terminals; }
        }

        public void ReceiveChangeInformation(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (searchPanel != null)
            {
                if (objectName == "Terminal")
                {
                    if (changeHint == DataEntityChangeType.Delete)
                    {
                        // We can handle this one locally.
                        foreach (SearchListViewItem item in searchPanel.ItemList.Items)
                        {
                            if (item.Key == "Terminal" && item.ID.PrimaryID == changeIdentifier)
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
            List<TerminalListItem> rows;
            SearchListViewItem item;

            if (text == null)
            {
                text = "";
            }

            rows = Providers.TerminalData.Search(PluginEntry.DataModel, text, text, maxDisplay);

            foreach (TerminalListItem row in rows)
            {
                item = new SearchListViewItem(this, (string)row.ID, "Terminal");

                item.SubItems.Add(row.Text);
                item.ID = new RecordIdentifier(row.ID,row.StoreID);

        
                item.ImageIndex = PluginEntry.TerminalImageIndex;


                items.Add(item);
            }
        }

        public void DoubleClickedItem(SearchListViewItem item)
        {            
            PluginOperations.ShowTerminal((string)item.ID[0], (string)item.ID[1]);
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

