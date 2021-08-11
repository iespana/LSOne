using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.DataProviders;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.User
{
    public partial class UserProfileUsersPage : UserControl, ITabView
    {
        UserProfile userProfile;

        public UserProfileUsersPage()
        {
            InitializeComponent();

            btnAdd.Enabled = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.SecurityEditUser);

            lvUsers.ContextMenuStrip = new ContextMenuStrip();
            lvUsers.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvUsers.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnsContextButtons_AddButtonClicked));

            item.Enabled = btnAdd.Enabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new UserProfileUsersPage();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            userProfile = (UserProfile)internalContext;
            LoadItems();
        }

        private void LoadItems()
        {
            List<DataLayer.BusinessObjects.UserManagement.User> users = Providers.UserData.GetUsersByUserProfile(PluginEntry.DataModel, userProfile.ID);

            lvUsers.ClearRows();
            IProfileSettings settings = PluginEntry.DataModel.Settings;
            foreach (DataLayer.BusinessObjects.UserManagement.User user in users)
            {
                Row row = new Row();
                row.AddText(settings.NameFormatter.Format(user.Name));
                row.AddText(user.Login);
                row.AddText(user.GroupName);
                row.AddText(user.NameOnReceipt);
                row.AddCell(new CheckBoxCell(!user.Disabled, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.Tag = user;
                lvUsers.AddRow(row);
            }

            lvUsers.AutoSizeColumns(false);
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "UserProfileUser" || objectName == "User")
            {
                LoadItems();
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            List<DataLayer.BusinessObjects.UserManagement.User> excludedUsers = new List<DataLayer.BusinessObjects.UserManagement.User>();

            for (int i = 0; i < lvUsers.Rows.Count; i++)
            {
                excludedUsers.Add((DataLayer.BusinessObjects.UserManagement.User)lvUsers.Rows[i].Tag);
            }

            if(new Dialogs.AddUsersToUserProfileDialog(userProfile).ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }
    }
}
