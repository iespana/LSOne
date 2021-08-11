//using LSRetail.Utilities.String;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Terminals
{
    public partial class TerminalSearchPanel : SearchPanelBase
    {
        private string lastSearch = "";
        private ISearchPanelFactory provider;

        public TerminalSearchPanel(ISearchPanelFactory provider)
            : this()
        {
            this.provider = provider;
        }


        public TerminalSearchPanel()
        {
            InitializeComponent();

            lvTerminalSearchResults.SmallImageList = PluginEntry.Framework.GetImageList();

            lvTerminalSearchResults.ContextMenuStrip = new ContextMenuStrip();
            lvTerminalSearchResults.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            lvTerminalSearchResults.ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);

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

            menu = lvTerminalSearchResults.ContextMenuStrip;

            menu.Items.Clear();
            
            /* We can optionally add our own items right here but we do not do it
             * because there are other places where we are displaying almost the same menu*/

            PluginEntry.Framework.SuspendSearchBarClosing();
            PluginEntry.Framework.ContextMenuNotify("TerminalSearchList", lvTerminalSearchResults.ContextMenuStrip, lvTerminalSearchResults);

            e.Cancel = (menu.Items.Count == 0);
        }

       

     

        private void lvUserSearchResults_Resize(object sender, EventArgs e)
        {
            lvTerminalSearchResults.SuspendLayout();
            lvTerminalSearchResults.BeginUpdate();

            // This seems very stupid but it solves a .NET Listview layout bug when going back from maximized
            lvTerminalSearchResults.Columns[lvTerminalSearchResults.Columns.Count - 1].Width = 10;
            lvTerminalSearchResults.Columns[lvTerminalSearchResults.Columns.Count - 1].Width = -2;
            //lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width = lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width - 20;

            lvTerminalSearchResults.EndUpdate();
            lvTerminalSearchResults.ResumeLayout();
        }

        public override void Search(string searchText)
        {
            List<TerminalListItem> rows;
            SearchListViewItem item;

            if (searchText == null)
            {
                searchText = "";
            }
            lastSearch = searchText;

            rows = Providers.TerminalData.Search(PluginEntry.DataModel, searchText, searchText, 0);

            lvTerminalSearchResults.Items.Clear();

            foreach (TerminalListItem row in rows)
            {
                item = new SearchListViewItem(provider, (string)row.ID, "Terminal");

                item.SubItems.Add(row.Text);
                item.SubItems.Add((string)row.StoreID);
                item.SubItems.Add(row.StoreName);
                item.ID = row.ID;

                item.ImageIndex = PluginEntry.TerminalImageIndex;

                lvTerminalSearchResults.Add(item);
            }

            OnSelectionChanged(false,false, false); // False since nothing is selected
            
        }

        public void SearchAgain()
        {
            Search(lastSearch);
        }


        

        public ListView ItemList
        {
            get
            {
                return lvTerminalSearchResults;
            }
        }

        private void lvTerminalSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lvTerminalSearchResults.SelectedItems.Count > 0;
            bool canDelete = hasSelection;

            if (hasSelection)
            {
                canDelete = canDelete && ((PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty) || PluginEntry.DataModel.CurrentStoreID == ((SearchListViewItem)lvTerminalSearchResults.SelectedItems[0]).ID.SecondaryID);
            }

            OnSelectionChanged(hasSelection, canDelete && PluginEntry.DataModel.HasPermission(Permission.StoreEdit), false);
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
