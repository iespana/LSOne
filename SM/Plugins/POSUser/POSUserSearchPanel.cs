using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.POSUser
{
    public partial class POSUserSearchPanel : SearchPanelBase
    {
        private string lastSearch = "";
        private ISearchPanelFactory provider;

        public POSUserSearchPanel(ISearchPanelFactory provider)
            : this()
        {
            this.provider = provider;
        }


        public POSUserSearchPanel()
        {
            InitializeComponent();

            lvPOSUsers.SmallImageList = PluginEntry.Framework.GetImageList();

            lvPOSUsers.ContextMenuStrip = new ContextMenuStrip();
            lvPOSUsers.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            lvPOSUsers.ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);

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

            menu = lvPOSUsers.ContextMenuStrip;

            menu.Items.Clear();
            
            /* We can optionally add our own items right here but we do not do it
             * because there are other places where we are displaying almost the same menu*/

            PluginEntry.Framework.SuspendSearchBarClosing();

            PluginEntry.Framework.ContextMenuNotify("POSUserSearchList", lvPOSUsers.ContextMenuStrip, lvPOSUsers);

            e.Cancel = (menu.Items.Count == 0);
        }

       

     

        private void lvUserSearchResults_Resize(object sender, EventArgs e)
        {
            lvPOSUsers.SuspendLayout();
            lvPOSUsers.BeginUpdate();

            // This seems very stupid but it solves a .NET Listview layout bug when going back from maximized
            lvPOSUsers.Columns[lvPOSUsers.Columns.Count - 1].Width = 10;
            lvPOSUsers.Columns[lvPOSUsers.Columns.Count - 1].Width = -2;
            //lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width = lvStoreSearchResults.Columns[lvStoreSearchResults.Columns.Count - 1].Width - 20;

            lvPOSUsers.EndUpdate();
            lvPOSUsers.ResumeLayout();
        }

        public override void Search(string searchText)
        {
            List<LSOne.DataLayer.BusinessObjects.UserManagement.POSUser> users;
            SearchListViewItem item;

            if (searchText == null)
            {
                searchText = "";
            }
            lastSearch = searchText;

            users = Providers.POSUserData.Search(PluginEntry.DataModel, searchText, searchText);

            lvPOSUsers.Items.Clear();

            foreach (LSOne.DataLayer.BusinessObjects.UserManagement.POSUser user in users)
            {
                item = new SearchListViewItem(provider, (string)user.ID, "POSUser");
                item.SubItems.Add(user.Text);
                //item.SubItems.Add((string)user.StoreID + " - " + user.StoreDescription);
                item.ID = user.ID;

                item.ImageIndex = PluginEntry.POSUserImageIndex;



                lvPOSUsers.Add(item);
            }

            OnSelectionChanged(false, false, false);
            
        }

        public void SearchAgain()
        {
            Search(lastSearch);
        }


        

        public ListView ItemList
        {
            get
            {
                return lvPOSUsers;
            }
        }

        public override bool CanAdd
        {
            get
            {
                return PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.SecurityCreateNewUsers);
            }
        }

        private void lvPOSUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lvPOSUsers.SelectedItems.Count > 0;

            OnSelectionChanged(hasSelection, hasSelection && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.SecurityDeleteUser), false);
        }
    }
}
