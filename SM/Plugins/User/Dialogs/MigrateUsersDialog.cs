
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.User.Properties;
using ListView = LSOne.Controls.ListView;

namespace LSOne.ViewPlugins.User.Dialogs
{
    public partial class MigrateUsersDialog : DialogBase
    {
        List<UserGroup> userGroups;

        public MigrateUsersDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            var users = Providers.UserMigrationCommands.GetNonPosUserUsers(PluginEntry.DataModel);

            foreach (var user in users)
            {
                var row = new Row();
                string name = "";
                if (user.Name.Prefix != "")
                {
                    name += user.Name.Prefix + " ";
                }
                if (user.Name.First != "")
                {
                    name += user.Name.First + " ";
                }
                if (user.Name.Middle != "")
                {
                    name += user.Name.Middle + " ";
                }
                if (user.Name.Last != "")
                {
                    name += user.Name.Last + " ";
                }
                if (user.Name.Suffix != "")
                {
                    name += user.Name.Suffix;
                }
                row.AddCell(new CheckBoxCell(name, true));

                var item = new ToolStripMenuItem(Properties.Resources.None) {Tag = Guid.Empty};

                var cell = new DropDownCell(item.Text) {Tag = item.Tag};
                row.AddCell(cell);

                row.Tag = user;
                listView1.AddRow(row);
            }

            listView1.AutoSizeColumns();

            var posUsers = Providers.UserMigrationCommands.GetNonUserPosUsers(PluginEntry.DataModel);
            foreach (var user in posUsers)
            {
                Row row = new Row();
                row.AddCell(new CheckBoxCell(user.ID + " - " + user.Text, true));

                row.Tag = user;
                listView2.AddRow(row);
            }

            listView2.AutoSizeColumns();

            btnMigrate.Enabled = isChecked(listView1) || (isChecked(listView2) && (cmbManagers.SelectedData != null) && (cmbNonManagers.SelectedData != null));

            if (listView2.RowCount > 0)
            {
                errorProvider1.SetError(cmbManagers, Resources.NeedsToBeSelected);
                errorProvider2.SetError(cmbNonManagers, Resources.NeedsToBeSelected);
            }

            if (listView1.RowCount == 0 && listView2.RowCount == 0)
            {
                MessageDialog.Show(Resources.NothingToMigrate, MessageBoxButtons.OK);
                Close();
            }

            base.OnLoad(e);
        }

        private static bool isChecked(ListView listView)
        {
            bool result = false;
            foreach (var row in listView.Rows)
            {
                if (((CheckBoxCell)row[0]).Checked)
                {
                    result = true;
                }
            }
            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbManagers_RequestData(object sender, EventArgs e)
        {
            if (userGroups == null)
            {
                userGroups = Providers.UserGroupData.AllGroups(PluginEntry.DataModel);
            }

            cmbManagers.SetData(userGroups, null);
        }

        private void cmbNonManagers_RequestData(object sender, EventArgs e)
        {
            if (userGroups == null)
            {
                userGroups = Providers.UserGroupData.AllGroups(PluginEntry.DataModel);
            }

            cmbNonManagers.SetData(userGroups, null);
        }

        private void listView1_CellDropDown(object sender, CellDropDownEventArgs args)
        {
            var item = new ToolStripMenuItem(Properties.Resources.None) {Tag = Guid.Empty};

            args.Items.Add(item);
            if (userGroups == null)
            {
                userGroups = Providers.UserGroupData.AllGroups(PluginEntry.DataModel);
            }

            var user = (LSOne.DataLayer.BusinessObjects.UserManagement.User)listView1.Row(args.RowNumber).Tag;
            var userGroup = Providers.UserGroupData.GetGroupsForUser(PluginEntry.DataModel, user.ID);

            foreach (var group in userGroups)
            {
                item = new ToolStripMenuItem(group.Name);
                item.Tag = group.ID;
                if (userAlreadyInGroup(group, userGroup))
                {
                    item.Enabled = false;
                }
                args.Items.Add(item);
            }
        }

        private bool userAlreadyInGroup(UserGroup group, List<UserGroup> groups)
        {
            foreach (var userGroup in groups)
            {
                if (group.ID == userGroup.ID)
                {
                    return true;
                }
            }
            return false;
        }

        private void listView1_CellAction(object sender, CellEventArgs args)
        {
            if (args.Cell is DropDownCell)
            {
                listView1.AutoSizeColumns();
            }
            if (args.Cell is CheckBoxCell)
            {
                btnMigrate.Enabled = isChecked(listView1) || (isChecked(listView2) && (cmbManagers.SelectedData != null) && (cmbNonManagers.SelectedData != null));
            }
        }

        private void listView2_CellAction(object sender, CellEventArgs args)
        {
            if (args.Cell is CheckBoxCell)
            {
                if (!isChecked(listView2))
                {
                    errorProvider1.Clear();
                    errorProvider2.Clear();
                }
                else
                {
                    errorProvider1.SetError(cmbManagers, Resources.NeedsToBeSelected);
                    errorProvider2.SetError(cmbNonManagers, Resources.NeedsToBeSelected);
                }
                btnMigrate.Enabled = isChecked(listView1) || (isChecked(listView2) && (cmbManagers.SelectedData != null) && (cmbNonManagers.SelectedData != null));
            }
        }

        private void cmbManagers_SelectedDataChanged(object sender, EventArgs e)
        {
            btnMigrate.Enabled = isChecked(listView1) || (isChecked(listView2) && (cmbManagers.SelectedData != null) && (cmbNonManagers.SelectedData != null));
            errorProvider1.Clear();
        }

        private void cmbNonManagers_SelectedDataChanged(object sender, EventArgs e)
        {
            btnMigrate.Enabled = isChecked(listView1) || (isChecked(listView2) && (cmbManagers.SelectedData != null) && (cmbNonManagers.SelectedData != null));
            errorProvider2.Clear();
        }

        private void btnMigrate_Click(object sender, EventArgs e)
        {
            bool userMigrated = false;
            bool posUserMigrated = false;
            bool usersExist = (listView1.RowCount > 0);
            bool posUsersExist = (listView2.RowCount > 0);
            foreach (var row in listView1.Rows)
            {
                if (((CheckBoxCell)row[0]).Checked)
                {
                    userMigrated = true;
                    var posUser = new POSUser();
                    var user = (LSOne.DataLayer.BusinessObjects.UserManagement.User)row.Tag;

                    string name = "";
                    if (user.Name.Prefix != "")
                    {
                        name += user.Name.Prefix + " ";
                    }
                    if (user.Name.First != "")
                    {
                        name += user.Name.First + " ";
                    }
                    if (user.Name.Middle != "")
                    {
                        name += user.Name.Middle + " ";
                    }
                    if (user.Name.Last != "")
                    {
                        name += user.Name.Last + " ";
                    }
                    if (user.Name.Suffix != "")
                    {
                        name += user.Name.Suffix;
                    }

                    posUser.Text = name;
                    posUser.NameOnReceipt = user.Name.Last;

                    posUser.ID = user.StaffID;

                    posUser.NeedsPasswordChange = true;
                    if ((Guid)((DropDownCell)row[1]).Tag != Guid.Empty)
                    {
                        Providers.UserGroupData.AddUser(PluginEntry.DataModel, (Guid)user.ID, (Guid)((DropDownCell)row[1]).Tag);
                    }
                    Providers.POSUserData.Save(PluginEntry.DataModel, posUser);
                }
            }

            if (cmbManagers.SelectedData != null && cmbNonManagers.SelectedData != null)
            {
                foreach (var row in listView2.Rows)
                {
                    if (((CheckBoxCell)row[0]).Checked)
                    {
                        posUserMigrated = true;
                        var posUser = (POSUser)row.Tag;
                        var name = getNameFromString(posUser.Text);
                        var groupID = new List<Guid>();

                        if (posUser.ManagerPrivileges)
                        {
                            groupID.Add((Guid)((UserGroup)cmbManagers.SelectedData).ID);
                        }
                        else
                        {
                            groupID.Add((Guid)((UserGroup)cmbNonManagers.SelectedData).ID);
                        }

                        Providers.UserData.New(PluginEntry.DataModel,
                                               (string)posUser.ID,
                                               posUser.Password,
                                               name,
                                               groupID,
                                               false,
                                               false,
                                               "");
                    }
                }
            }

            string message = "";
            if (!usersExist)
            {
                message += Resources.NoUsersExist;
            }
            else if (userMigrated)
            {
                message += Resources.UserMigrated;
            }
            else
            {
                message += Resources.UserNotMigrated;
            }
            message += "\n\n";
            if (!posUsersExist)
            {
                message += Resources.NoPOSUsersExist;
            }
            else if (posUserMigrated)
            {
                message += Resources.POSUsersMigrated;
            }
            else
            {
                message += Resources.POSUsersNotMigrated;
            }
            MessageDialog.Show(message, MessageBoxButtons.OK);
            Close();
        }

        private Name getNameFromString(string text)
        {
            Name name = new Name();
            string[] names = text.Split(' ');

            switch (names.Length)
            {
                case 1:
                    name.First = names[0];
                    break;
                case 2:
                    name.First = names[0];
                    name.Last = names[1];
                    break;
                case 3:
                    name.First = names[0];
                    name.Middle = names[1];
                    name.Last = names[2];
                    break;
                case 4:
                    name.First = names[0];
                    name.Middle = names[1] + " " + names[2];
                    name.Last = names[3];
                    break;
                case 5:
                    name.First = text;
                    break;
            }

            return name;
        }
    }
}
