using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.User
{
    public partial class UserSearchPanel : SearchPanelBase
    {
        private string lastSearch;
        private ISearchPanelFactory searchProvider;

        public UserSearchPanel(ISearchPanelFactory searchProvider)
        {
            InitializeComponent();

            lvUserSearchResults.SmallImageList = PluginEntry.Framework.GetImageList();

            lvUserSearchResults.ContextMenuStrip = new ContextMenuStrip();
            lvUserSearchResults.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
            lvUserSearchResults.ContextMenuStrip.Closed += new ToolStripDropDownClosedEventHandler(ContextMenuStrip_Closed);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.searchProvider = searchProvider;
        }

        void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            PluginEntry.Framework.ResumeSearchBarClosing();
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu;

            menu = lvUserSearchResults.ContextMenuStrip;

            menu.Items.Clear();
            
            /* We can optionally add our own items right here but we do not do it
             * because there are other places where we are displaying almost the same menu*/

            PluginEntry.Framework.SuspendSearchBarClosing();

            PluginEntry.Framework.ContextMenuNotify("UserSearchList", lvUserSearchResults.ContextMenuStrip, lvUserSearchResults);

            e.Cancel = (menu.Items.Count == 0);
        }

       

     

        private void lvUserSearchResults_Resize(object sender, EventArgs e)
        {
            lvUserSearchResults.SuspendLayout();
            lvUserSearchResults.BeginUpdate();

            // This seems very stupid but it solves a .NET Listview layout bug when going back from maximized
            lvUserSearchResults.Columns[lvUserSearchResults.Columns.Count - 1].Width = 10;
            lvUserSearchResults.Columns[lvUserSearchResults.Columns.Count - 1].Width = -2;
            //lvUserSearchResults.Columns[lvUserSearchResults.Columns.Count - 1].Width = lvUserSearchResults.Columns[lvUserSearchResults.Columns.Count - 1].Width - 20;

            lvUserSearchResults.EndUpdate();
            lvUserSearchResults.ResumeLayout();
        }

        public void SearchAgain()
        {
            Search(lastSearch);
        }

        public override void Search(string searchText)
        {
            UserListViewItem item;
            string loginName = "";
            Name name;
            List<LSOne.DataLayer.BusinessObjects.UserManagement.User> users;
            IProfileSettings settings = PluginEntry.DataModel.Settings;

            if (searchText == null)
            {
                searchText = "";
            }
            lastSearch = searchText;

            if (!searchText.Contains(" ") && !searchText.Contains(","))
            {
                loginName = searchText;
            }

            string[] fields = searchText.Split(new [] { " ", "," }, StringSplitOptions.None);
            int numberOfFields = fields.Length;
            name = NameParser.ParseName(searchText, PluginEntry.DataModel.Settings.NameFormat == NameFormat.LastNameFirst);

            users = Providers.UserData.FindUsers(PluginEntry.DataModel, name.First, name.Middle, name.Last, name.Suffix, loginName,0);

            lvUserSearchResults.Items.Clear();

            foreach (LSOne.DataLayer.BusinessObjects.UserManagement.User user in users)
            {
                item = new UserListViewItem(searchProvider,
                    user,
                    settings.NameFormatter.Format(user.Name),
                        user.Disabled ? PluginEntry.UserDisabledImageIndex : (user.IsServerUser ? PluginEntry.ServerUserImageIndex : PluginEntry.UserImageIndex));

                if (user.Guid.Equals((Guid)PluginEntry.DataModel.CurrentUser.ID))
                {
                    //item.Font = new Font(item.Font, FontStyle.Bold);
                }
                
                //item.Tag = user.Guid;
                item.SubItems.Add(user.Login);

                lvUserSearchResults.Add(item);

            }
        }


        public ListView ItemList
        {
            get
            {
                return lvUserSearchResults;
            }
        }

        public override bool CanAdd
        {
            get
            {
                return PluginEntry.DataModel.HasPermission(Permission.SecurityCreateNewUsers);
            }
        }

        private void lvUserSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lvUserSearchResults.SelectedItems.Count > 0;
            bool canDelete;
            ListViewItem item;

            canDelete = hasSelection && PluginEntry.DataModel.HasPermission(Permission.SecurityDeleteUser);

            if (hasSelection)
            {
                item = lvUserSearchResults.SelectedItems[0];

                if ((((UserListViewItem)item).ID == PluginEntry.DataModel.CurrentUser.ID))
                {
                    canDelete = false;
                }

                if (((UserListViewItem)item).User.Login == "admin")
                {
                    canDelete = false;
                }
            }

            OnSelectionChanged(hasSelection, canDelete, false);
        }

       
    }
}
