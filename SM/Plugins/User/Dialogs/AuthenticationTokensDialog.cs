using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.User.Properties;

namespace LSOne.ViewPlugins.User.Dialogs
{
    public partial class AuthenticationTokensDialog : DialogBase
    {
        public UserGroup NewGroup { get; private set; }
        private RecordIdentifier userID;

        public AuthenticationTokensDialog()
            : base()
        {
            InitializeComponent();
            userID = RecordIdentifier.Empty;

            lvTokens.ContextMenuStrip = new ContextMenuStrip();
            lvTokens.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        public AuthenticationTokensDialog(RecordIdentifier userID)
            : this()
        {
            this.userID = userID;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (userID != RecordIdentifier.Empty)
            {
                var tokens = Providers.AuthenticationTokenData.GetTokensForUser(PluginEntry.DataModel, userID);

                foreach (AuthenticationToken token in tokens)
                {
                    AddTokenRow(token);
                }

                lvTokens.ApplyRelativeColumnSize();
            }
        }
        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AddTokenRow(AuthenticationToken token)
        {
            Row row = new Row();
            var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete);

            row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, token.Text, true));
            row.Tag = token.ID;

            lvTokens.AddRow(row);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            NewAuthenticationTokenDialog dlg = new NewAuthenticationTokenDialog(userID, lvTokens.RowCount);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                AddTokenRow(dlg.Token);

                lvTokens.ApplyRelativeColumnSize();
            }
        }

        private void lvTokens_SelectionChanged(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.RemoveButtonEnabled != (lvTokens.Selection.Count > 0))
            {
                btnsEditAddRemove.RemoveButtonEnabled = (lvTokens.Selection.Count > 0);
            }
        }

        private bool DeleteToken(int rowIndex)
        {
            if (QuestionDialog.Show(
                Resources.DeleteAuthenticationTokenQuestion,
                Resources.DeleteAuthenticationToken) == DialogResult.Yes)
            {
                Providers.AuthenticationTokenData.Delete(PluginEntry.DataModel, ((RecordIdentifier)lvTokens.Row(lvTokens.Selection.FirstSelectedRow).Tag).PrimaryID);

                return true;
            }

            return false;
        }

       

        private void lvTokens_CellAction(object sender, CellEventArgs args)
        {
            if (DeleteToken(args.RowNumber))
            {
                lvTokens.RemoveRow(args.RowNumber);
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvTokens.Selection.Count > 1)
            {
                if (QuestionDialog.Show(
                    Resources.DeleteAuthenticationTokensQuestion,
                    Resources.DeleteAuthenticationTokens) == DialogResult.Yes)
                {
                    while (lvTokens.Selection.Count > 0)
                    {
                        Providers.AuthenticationTokenData.Delete(PluginEntry.DataModel, ((RecordIdentifier)lvTokens.Row(lvTokens.Selection.FirstSelectedRow).Tag).PrimaryID);

                        lvTokens.RemoveRow(lvTokens.Selection.FirstSelectedRow);
                    }
                }
            }
            else if (lvTokens.Selection.Count == 1)
            {
                if (DeleteToken(lvTokens.Selection.FirstSelectedRow))
                {
                    lvTokens.RemoveRow(lvTokens.Selection.FirstSelectedRow);
                }
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvTokens.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   btnsEditAddRemove_AddButtonClicked);

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }
    }
}
