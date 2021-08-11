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

namespace LSOne.ViewPlugins.SiteService.Views
{
    public partial class SiteServiceProfilesView : ViewBase
    {
        private string selectedID = "";
        private List<SiteServiceProfile> profiles;

        public SiteServiceProfilesView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID.ToString();
        }

        public SiteServiceProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvTransactionServiceProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvTransactionServiceProfiles.ContextMenuStrip.Opening += lvTransactionServiceProfiles_Opening;

            lvTransactionServiceProfiles.SmallImageList = PluginEntry.Framework.GetImageList();

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("TransactionServiceProfiles", RecordIdentifier.Empty, Properties.Resources.StoreServerProfiles, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.StoreServerProfiles;
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

            HeaderText = Properties.Resources.StoreServerProfiles;
            //HeaderIcon = Properties.Resources.Profiles16;

            LoadTransactionServiceProfiles(1, false);

            
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
                case "TransactionServiceProfile":
                    LoadTransactionServiceProfiles(lvTransactionServiceProfiles.SortColumn, lvTransactionServiceProfiles.SortedBackwards);
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id;

            id = PluginOperations.NewTransactionServicesProfile();

            if (id != RecordIdentifier.Empty)
            {
                selectedID = (string)id;
            }
        }

        private void lvTransactionServiceProfiles_SizeChanged(object sender, EventArgs e)
        {
            lvTransactionServiceProfiles.Columns[1].Width = -2;
        }

        private void lvTransactionServiceProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvTransactionServiceProfiles.SelectedItems.Count > 0) ? (string)lvTransactionServiceProfiles.SelectedItems[0].Tag : "";
            bool isProfileInUse = selectedID == string.Empty ? false : profiles.Single(x => x.ID == selectedID).ProfileIsUsed;
            btnsContextButtons.EditButtonEnabled = (lvTransactionServiceProfiles.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileView);
            btnsContextButtons.RemoveButtonEnabled = (lvTransactionServiceProfiles.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit) && !isProfileInUse;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowTransactionServiceProfileSheet(this, (string)lvTransactionServiceProfiles.SelectedItems[0].Tag);
        }

        private void lvTransactionServiceProfiles_DoubleClick(object sender, EventArgs e)
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

            menu = lvTransactionServiceProfiles.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("TransactionServiceProfileList", lvTransactionServiceProfiles.ContextMenuStrip, lvTransactionServiceProfiles);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteTransactionServiceProfile(selectedID);
        }

        private void LoadTransactionServiceProfiles(int sortBy, bool backwards)
        {
            ListViewItem item;
            string sort;

            lvTransactionServiceProfiles.Items.Clear();

            sort = (sortBy == 0) ? "Len(PROFILEID) ASC,PROFILEID ASC" : "NAME ASC";

            if (backwards)
            {
                sort = sort.Replace("ASC", "DESC");
            }
            
            profiles = Providers.SiteServiceProfileData.GetSiteServiceProfileList(PluginEntry.DataModel, sort);

            foreach (DataEntity profile in profiles)
            {
                item = new ListViewItem((string)profile.ID);
                item.SubItems.Add(profile.Text);
                item.Tag = (string)profile.ID;
                item.ImageIndex = -1;

                lvTransactionServiceProfiles.Add(item);

                if (selectedID == (string)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvTransactionServiceProfiles.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvTransactionServiceProfiles.SortColumn = sortBy;

            lvTransactionServiceProfiles_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvTransactionServiceProfiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvTransactionServiceProfiles.SortColumn == e.Column)
            {
                lvTransactionServiceProfiles.SortedBackwards = !lvTransactionServiceProfiles.SortedBackwards;
            }
            else
            {
                if (lvTransactionServiceProfiles.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvTransactionServiceProfiles.Columns[lvTransactionServiceProfiles.SortColumn].ImageIndex = 2;
                }
                lvTransactionServiceProfiles.SortedBackwards = false;
            }

            LoadTransactionServiceProfiles(e.Column, lvTransactionServiceProfiles.SortedBackwards);
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

        protected override void OnClose()
        {
            lvTransactionServiceProfiles.SmallImageList = null;

            base.OnClose();
        }

       
    }
}
