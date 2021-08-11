using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class KitchenServiceProfilesView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public KitchenServiceProfilesView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID;
        }

        public KitchenServiceProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvKitchenServiceProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvKitchenServiceProfiles.ContextMenuStrip.Opening += lvKitchenServiceProfiles_Opening;

            lvKitchenServiceProfiles.SmallImageList = PluginEntry.Framework.GetImageList();

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenServiceProfiles);

            btnsContextButtons.AddButtonEnabled = !ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("KMPROFILES", RecordIdentifier.Empty, Properties.Resources.KitchenServiceProfiles, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.KitchenServiceProfiles;
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
            if (isRevert)
            {
                
            }

            HeaderText = Properties.Resources.KitchenServiceProfiles;
           

            LoadProfiles();

            
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
                case "DefaultData":
                case "KitchenServiceProfile":
                    LoadProfiles();
                    break;
            }
        }

        private void LoadProfiles()
        {
            List<KitchenServiceProfile> profiles;
            ListViewItem item;

            lvKitchenServiceProfiles.Items.Clear();

            profiles = Providers.KitchenDisplayTransactionProfileData.GetList(PluginEntry.DataModel);

            foreach (KitchenServiceProfile profile in profiles)
            {
                item = new ListViewItem(profile.Text) {Tag = profile.ID, ImageIndex = -1};
                item.SubItems.Add(string.Format("{0}:{1}", profile.KitchenServiceAddress, profile.KitchenServicePort));

                lvKitchenServiceProfiles.Add(item);

                if (selectedID == (RecordIdentifier)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvKitchenServiceProfiles.Columns[lvKitchenServiceProfiles.SortColumn].ImageIndex = (lvKitchenServiceProfiles.SortedBackwards ? 1 : 0);
            lvKitchenServiceProfiles.BestFitColumns();
            lvKitchenServiceProfiles_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier newID = PluginOperationsHelper.AddKitchenServiceProfile();

            if (!RecordIdentifier.IsEmptyOrNull(newID))
            {
                selectedID = newID;
                LoadProfiles();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowKitchenServiceProfileView(selectedID);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvKitchenServiceProfiles.SelectedItems.Count == 1)
            {
                PluginOperationsHelper.DeleteKitchenServiceProfile(selectedID);
            }
            else
            {
                PluginOperationsHelper.DeleteKitchenServiceProfiles(lvKitchenServiceProfiles.GetSelectedIDs());
            }
        }

        private void lvKitchenServiceProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var permission = PluginEntry.DataModel.HasPermission(Permission.ManageKitchenServiceProfiles);
            selectedID = (lvKitchenServiceProfiles.SelectedItems.Count > 0) ? (RecordIdentifier)lvKitchenServiceProfiles.SelectedItems[0].Tag : "";

            btnsContextButtons.EditButtonEnabled = (lvKitchenServiceProfiles.SelectedItems.Count == 1) && permission;
            btnsContextButtons.RemoveButtonEnabled = (lvKitchenServiceProfiles.SelectedItems.Count > 0) && permission;
        }

        private void lvKitchenMangerProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvKitchenServiceProfiles_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvKitchenServiceProfiles.ContextMenuStrip;

            menu.Items.Clear();

            var item = ExtendedMenuItem.EditToMenu(btnsContextButtons, menu, btnEdit_Click);
            item.Default = true;

            ExtendedMenuItem.AddToMenu(btnsContextButtons, menu, btnAdd_Click);
            ExtendedMenuItem.RemoveToMenu(btnsContextButtons, menu, btnRemove_Click);

            PluginEntry.Framework.ContextMenuNotify("KitchenServiceProfilesList", lvKitchenServiceProfiles.ContextMenuStrip, lvKitchenServiceProfiles);

            e.Cancel = (menu.Items.Count == 0);
        }
       
        private void lvKitchenServiceProfiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvKitchenServiceProfiles.SortColumn == e.Column)
            {
                lvKitchenServiceProfiles.SortedBackwards = !lvKitchenServiceProfiles.SortedBackwards;
            }
            else
            {
                if (lvKitchenServiceProfiles.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvKitchenServiceProfiles.Columns[lvKitchenServiceProfiles.SortColumn].ImageIndex = 2;
                    lvKitchenServiceProfiles.SortColumn = e.Column;
                }
                lvKitchenServiceProfiles.SortedBackwards = false;
            }

            LoadProfiles();
        }

        protected override void OnClose()
        {
            lvKitchenServiceProfiles.SmallImageList = null;

            base.OnClose();
        }
    }
}
