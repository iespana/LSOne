//using LSRetail.Utilities.String;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Store
{
    public partial class StoreSearchPanel : SearchPanelBase
    {
        private string lastSearch = "";
        private ISearchPanelFactory provider;

        public StoreSearchPanel(ISearchPanelFactory provider)
            : this()
        {
            this.provider = provider;
        }


        public StoreSearchPanel()
        {
            InitializeComponent();

            lvStoreSearchResults.SmallImageList = PluginEntry.Framework.GetImageList();

            lvStoreSearchResults.ContextMenuStrip = new ContextMenuStrip();
            lvStoreSearchResults.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            lvStoreSearchResults.ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            PluginEntry.Framework.ResumeSearchBarClosing();
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu;

            menu = lvStoreSearchResults.ContextMenuStrip;

            menu.Items.Clear();
            
            /* We can optionally add our own items right here but we do not do it
             * because there are other places where we are displaying almost the same menu*/

            PluginEntry.Framework.SuspendSearchBarClosing();
            PluginEntry.Framework.ContextMenuNotify("StoreSearchList", lvStoreSearchResults.ContextMenuStrip, lvStoreSearchResults);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvUserSearchResults_Resize(object sender, EventArgs e)
        {
            lvStoreSearchResults.SuspendLayout();
            lvStoreSearchResults.BeginUpdate();

            // This seems very stupid but it solves a .NET Listview layout bug when going back from maximized
            lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width = 10;
            lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width = -2;
            //lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width = lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width - 20;

            lvStoreSearchResults.EndUpdate();
            lvStoreSearchResults.ResumeLayout();
        }

        public override void Search(string searchText)
        {
            List<StoreListItem> items;
            SearchListViewItem item;

            if (searchText == null)
            {
                searchText = "";
            }
            lastSearch = searchText;

            StoreListSearchFilter filter = new StoreListSearchFilter
            {
                DescriptionOrID = searchText,
                DescriptionOrIDBeginsWith = false,
                City = searchText,
                CityBeginsWith = false
            };

            items = Providers.StoreData.Search(PluginEntry.DataModel, filter);

            lvStoreSearchResults.Items.Clear();

            foreach (StoreListItem row in items)
            {
                item = new SearchListViewItem(provider, (string)row.ID, "Store");

                item.SubItems.Add((string)row.Text);
                item.SubItems.Add((string)row.City);
                item.ID = row.ID;

                item.ImageIndex = PluginEntry.StoreImageIndex;

                lvStoreSearchResults.Add(item);
            }

            OnSelectionChanged(false,false,false); // Nothing is selected at this point
            
        }

        public void SearchAgain()
        {
            Search(lastSearch);
        }

        public ListView ItemList
        {
            get
            {
                return lvStoreSearchResults;
            }
        }

        private void lvStoreSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lvStoreSearchResults.SelectedItems.Count > 0;

            OnSelectionChanged(hasSelection, hasSelection && PluginEntry.DataModel.HasPermission(Permission.StoreEdit) && (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty), false);
        }

        public override bool CanAdd
        {
            get
            {
                return PluginEntry.DataModel.HasPermission(Permission.StoreEdit);
            }
        }

        
    }
}
