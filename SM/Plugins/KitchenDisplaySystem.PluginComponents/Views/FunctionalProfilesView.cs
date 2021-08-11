using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class FunctionalProfilesView : ViewBase
    {
        RecordIdentifier selectedProfileId;

        public FunctionalProfilesView(RecordIdentifier selectedProfileId)
            :this()
        {
            this.selectedProfileId = selectedProfileId;
        }

        public FunctionalProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);

            lvProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvProfiles.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.FunctionalProfiles;
                
            }
        }


        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("KITCHENDISPLAYFUNCTIONALPROFILESLog", RecordIdentifier.Empty, Properties.Resources.FunctionalProfiles, false));
          

        }


        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenDisplayFunctionalProfile":
                    LoadProfiles();
                    break;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadProfiles();
        }

        private void LoadProfiles()
        {
            ListViewItem listItem;
            lvProfiles.Items.Clear();
            List<KitchenDisplayFunctionalProfile> profiles = Providers.KitchenDisplayFunctionalProfileData.GetList(PluginEntry.DataModel);

            foreach (var profile in profiles)
            {
                listItem = new ListViewItem(profile.Text);

                listItem.Tag = profile.ID;
                lvProfiles.Add(listItem);

                if (selectedProfileId == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvProfiles_SelectedIndexChanged(this, EventArgs.Empty);
            lvProfiles.BestFitColumns();
        }

        private void lvProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {            
            var permission = PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles);

            btnsEditAddRemove.EditButtonEnabled = lvProfiles.SelectedItems.Count == 1 && permission;
            btnsEditAddRemove.RemoveButtonEnabled = lvProfiles.SelectedItems.Count > 0 && permission;
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier selectedId = (RecordIdentifier)lvProfiles.SelectedItems[0].Tag;
            PluginOperationsHelper.ShowFunctionalProfileView(selectedId);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.NewFunctionalProfileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginOperationsHelper.ShowFunctionalProfileView(dlg.Id);
            }          
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvProfiles.SelectedItems.Count == 1)
            {
                PluginOperationsHelper.DeleteFunctionalProfile((RecordIdentifier)lvProfiles.SelectedItems[0].Tag);
            }
            else
            {
                PluginOperationsHelper.DeleteFunctionalProfiles(lvProfiles.GetSelectedIDs());
            }
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvProfiles.ContextMenuStrip;
            menu.Items.Clear();

            // Each item is a line in the right click menu. 
            // Usually there is not much that needs to be changed here

            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemove_EditButtonClicked);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true; // The default item has a bold font
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsEditAddRemove_AddButtonClicked);

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;
            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvDataObjects_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
