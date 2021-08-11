using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.LSCommerce.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.LSCommerce.Views
{
    public partial class LicensesView : ViewBase
    {
        public LicensesView()
        {
            InitializeComponent();

            HeaderText = Resources.LSCommerceLicenses;

            btnsEditRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.TerminalEdit);

            lvLSCommerceLicenses.ContextMenuStrip = new ContextMenuStrip();
            lvLSCommerceLicenses.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.TerminalEdit);
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

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "LSCommerce license":
                    LoadItems();
                    break;
            }
        }

        private void LoadItems()
        {
            List<OmniLicense> omniLicenses = null;

            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            try
            {
                omniLicenses = service.GetOmniLicenses(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true, !PluginEntry.DataModel.IsHeadOffice ? PluginEntry.DataModel.CurrentStoreID : null);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                return;
            }

            lvLSCommerceLicenses.ClearRows();

            foreach (var license in omniLicenses)
            {
                Row row = new Row();
                row.AddText((string)license.AppID);
                row.AddText((string)license.DeviceID);
                row.AddText((string)license.TerminalID);
                row.AddText((string)license.StoreID);
                row.AddText((string)license.Licensekey);

                row.Tag = license;

                lvLSCommerceLicenses.AddRow(row);
            }

            lvLSCommerceLicenses.AutoSizeColumns();    
        }

        private void lvLSCommerceLicenses_SelectionChanged(object sender, EventArgs e)
        {
            bool hasViewPermission = PluginEntry.DataModel.HasPermission(Permission.TerminalView);
            bool hasEditPermission = PluginEntry.DataModel.HasPermission(Permission.TerminalEdit);

            btnsEditRemove.EditButtonEnabled = lvLSCommerceLicenses.Selection.Count > 0 && hasViewPermission;
            btnsEditRemove.RemoveButtonEnabled = lvLSCommerceLicenses.Selection.Count > 0 && hasEditPermission;
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.EditLicence((OmniLicense)lvLSCommerceLicenses.Selection[0].Tag);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteLicense(((OmniLicense)lvLSCommerceLicenses.Selection[0].Tag).ID);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvLSCommerceLicenses.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.EditCmd,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("StoresList", lvLSCommerceLicenses.ContextMenuStrip, lvLSCommerceLicenses);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnClose()
        {
            
        }

        private void lvLSCommerceLicenses_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
