//using LSRetail.Utilities.String;

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.RetailItems
{
    public partial class RetailItemSearchPanel : SearchPanelBase
    {
        private string lastSearch = "";
        private ISearchPanelFactory provider;

        private int MaxRecords = 500;

        public RetailItemSearchPanel(ISearchPanelFactory provider)
            : this()
        {
            this.provider = provider;
        }


        public RetailItemSearchPanel()
        {
            InitializeComponent();

            lvRetailItemSearchResults.SmallImageList = PluginEntry.Framework.GetImageList();

            lvRetailItemSearchResults.ContextMenuStrip = new ContextMenuStrip();
            lvRetailItemSearchResults.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            lvRetailItemSearchResults.ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);

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

            menu = lvRetailItemSearchResults.ContextMenuStrip;

            menu.Items.Clear();
            
            /* We can optionally add our own items right here but we do not do it
             * because there are other places where we are displaying almost the same menu*/

            PluginEntry.Framework.SuspendSearchBarClosing();
            PluginEntry.Framework.ContextMenuNotify("RetailItemSearchList", lvRetailItemSearchResults.ContextMenuStrip, lvRetailItemSearchResults);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvUserSearchResults_Resize(object sender, EventArgs e)
        {
            lvRetailItemSearchResults.SuspendLayout();
            lvRetailItemSearchResults.BeginUpdate();

            // This seems very stupid but it solves a .NET Listview layout bug when going back from maximized
            lvRetailItemSearchResults.Columns[lvRetailItemSearchResults.Columns.Count - 1].Width = 10;
            lvRetailItemSearchResults.Columns[lvRetailItemSearchResults.Columns.Count - 1].Width = -2;
            //lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width = lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width - 20;

            lvRetailItemSearchResults.EndUpdate();
            lvRetailItemSearchResults.ResumeLayout();
        }

        public override void Search(string searchText)
        {
            SearchListViewItem item;

            if (searchText == null)
            {
                searchText = "";
            }

            RetailItemSearchPanelFactory.rows = Providers.RetailItemData.Search(PluginEntry.DataModel, searchText, 0, MaxRecords, true, SortEnum.Description,false);

            lvRetailItemSearchResults.Items.Clear();

            foreach (SimpleRetailItem row in RetailItemSearchPanelFactory.rows)
            {
                item = new SearchListViewItem(provider, (string)row.ID, "RetailItem");

                item.SubItems.Add((string)row.Text);
                item.ID = row.ID;
                item.MasterID = row.MasterID;

                item.ImageIndex = row.ItemType != ItemTypeEnum.MasterItem ? (row.HeaderItemID == RecordIdentifier.Empty ? PluginEntry.RetailItemImageIndex : PluginEntry.VariantItemImageIndex) : PluginEntry.MasterItemImageIndex;



                lvRetailItemSearchResults.Add(item);
            }

            OnSelectionChanged(false, false, false); // Nothing is selected at this point
            
        }

        public void SearchAgain()
        {
            Search(lastSearch);
        }

        public ListView ItemList
        {
            get
            {
                return lvRetailItemSearchResults;
            }
        }

        private void lvStoreSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lvRetailItemSearchResults.SelectedItems.Count > 0;

            OnSelectionChanged(hasSelection, hasSelection && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit), false);
        }

        public override bool CanAdd
        {
            get
            {
                return PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);
            }
        }

        
    }
}
