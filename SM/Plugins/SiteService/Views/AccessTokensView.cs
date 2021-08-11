using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.DataLayer.BusinessObjects.Profiles;
using System.Linq;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using LSOne.ViewPlugins.SiteService.Properties;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.SiteService.Views
{
    public partial class AccessTokensView : ViewBase
    {
        private AccessToken selectedToken = null;

        public AccessTokensView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvIntegrationFrameworkTokens.ContextMenuStrip = new ContextMenuStrip();
            lvIntegrationFrameworkTokens.ContextMenuStrip.Opening += lvTransactionServiceProfiles_Opening;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuthenticationTokens);

            HeaderText = Resources.AccessTokensHeader;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("IntegrationFrameworkAccessTokens", RecordIdentifier.Empty, Properties.Resources.AccessTokensHeader, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.AccessTokensHeader;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadItems();
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "IntegrationFrameworkTokens":
                    LoadItems();
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewIntegrationFrameworkToken();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.EditIntegrationFrameworkToken((AccessToken)lvIntegrationFrameworkTokens.Row(lvIntegrationFrameworkTokens.Selection.FirstSelectedRow).Tag);
        }

        private void lvTransactionServiceProfiles_SelectionChanged(object sender, EventArgs e)
        {
            selectedToken = (lvIntegrationFrameworkTokens.Selection.Count > 0) ? (AccessToken)lvIntegrationFrameworkTokens.Selection[0].Tag : null;
            btnsContextButtons.EditButtonEnabled = (lvIntegrationFrameworkTokens.Selection.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuthenticationTokens);
            btnsContextButtons.RemoveButtonEnabled = (lvIntegrationFrameworkTokens.Selection.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuthenticationTokens);
        }

        private void lvTransactionServiceProfiles_DoubleClick_1(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvTransactionServiceProfiles_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvIntegrationFrameworkTokens.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            if(selectedToken != null && selectedToken.Active)
            {
                item = new ExtendedMenuItem(
                    Properties.Resources.Revoke,
                    400,
                    RevokeToken);
                item.Enabled = btnsContextButtons.EditButtonEnabled;
                menu.Items.Add(item);
            }

            PluginEntry.Framework.ContextMenuNotify("IntegrationFrameworkAccessTokensList", lvIntegrationFrameworkTokens.ContextMenuStrip, lvIntegrationFrameworkTokens);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void RevokeToken(object sender, EventArgs e)
        {
            if(QuestionDialog.Show(Properties.Resources.RevokeTokenQuestion, Properties.Resources.RevokeToken) == DialogResult.Yes)
            {
                Providers.AccessTokenData.RevokeAccessToken(PluginEntry.DataModel, selectedToken.SenderDNS);
                LoadItems();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteIntegrationFrameworkToken(selectedToken.SenderDNS);
        }

        private void LoadItems()
        {
            lvIntegrationFrameworkTokens.ClearRows();

            List<AccessToken> tokens = Providers.AccessTokenData.GetIFTokenList(PluginEntry.DataModel);

            Row row;
            String status;
            foreach (AccessToken token in tokens)
            {
                row = new Row();
                status = token.Active ? Resources.Active : Resources.Inactive;
                row.AddText(token.Description);
                row.AddText(token.SenderDNS);
                row.AddText((string)token.UserLogin + " - " + token.UserName);
                row.AddText(token.StoreName);
                row.AddText(status);

                row.Tag = token;

                lvIntegrationFrameworkTokens.AddRow(row);

                if (selectedToken != null && selectedToken.SenderDNS == ((AccessToken)row.Tag).SenderDNS)
                {
                    lvIntegrationFrameworkTokens.Selection.Set(lvIntegrationFrameworkTokens.RowCount - 1);
                }
            }

            lvIntegrationFrameworkTokens.AutoSizeColumns();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), btnAdd_Click), 10);
                }
            }
            else if (arguments.CategoryKey == GetType() + ".Related")
            {
                PluginEntry.Framework.FindImplementor(this, "CanInsertDefaultData", arguments);
            }
        }
    }
}
