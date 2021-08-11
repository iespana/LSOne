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

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class FunctionalityProfilesView : ViewBase
    {
        private string selectedID = "";
        private List<FunctionalityProfile> profiles;

        public FunctionalityProfilesView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = (string)selectedID;
        }

        public FunctionalityProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            lvFunctionalityProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvFunctionalityProfiles.ContextMenuStrip.Opening += lvFunctionalityProfiles_Opening;

            lvFunctionalityProfiles.SmallImageList = PluginEntry.Framework.GetImageList();

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("FunctionalityProfiles", RecordIdentifier.Empty, Properties.Resources.FunctionalityProfile, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.FunctionalityProfiles;
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

            HeaderText = Properties.Resources.FunctionalityProfiles;
            //HeaderIcon = Properties.Resources.Profiles16;

            LoadFunctionalityProfiles(1,false);
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
                case "FunctionalityProfile":
                    LoadFunctionalityProfiles(lvFunctionalityProfiles.SortColumn, lvFunctionalityProfiles.SortedBackwards);
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id;

            id = PluginOperations.NewFunctionalityProfile();

            if (id != RecordIdentifier.Empty)
            {
                selectedID = (string)id;
            }
        }

        private void lvFunctionalityProfiles_SizeChanged(object sender, EventArgs e)
        {
            lvFunctionalityProfiles.Columns[1].Width = -2;
        }

        private void lvFunctionalityProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvFunctionalityProfiles.SelectedItems.Count > 0) ? (string)lvFunctionalityProfiles.SelectedItems[0].Tag : "";
            bool isProfileInUse = selectedID == string.Empty ? false : profiles.Single(x => x.ID == selectedID).ProfileIsUsed;
            btnsContextButtons.EditButtonEnabled = (lvFunctionalityProfiles.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileView);
            btnsContextButtons.RemoveButtonEnabled = (lvFunctionalityProfiles.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileEdit) && !isProfileInUse;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowFunctionalityProfileSheet(this, (string)lvFunctionalityProfiles.SelectedItems[0].Tag);
        }

        private void lvFunctionalityProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvFunctionalityProfiles_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvFunctionalityProfiles.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("FunctionalityProfileList", lvFunctionalityProfiles.ContextMenuStrip, lvFunctionalityProfiles);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteFunctionalityProfile(selectedID);
        }

        private void LoadFunctionalityProfiles(int sortBy, bool backwards)
        {
            ListViewItem item;
            string sort;

            lvFunctionalityProfiles.Items.Clear();

            sort = (sortBy == 0) ? "Len(PROFILEID) ASC,PROFILEID ASC" : "NAME ASC";

            if (backwards)
            {
                sort = sort.Replace("ASC", "DESC");
            }

            profiles = Providers.FunctionalityProfileData.GetFunctionalityProfileList(PluginEntry.DataModel, sort);

            foreach (FunctionalityProfile profile in profiles)
            {
                item = new ListViewItem((string)profile.ID);
                item.SubItems.Add(profile.Text);
                item.Tag = (string)profile.ID;
                item.ImageIndex = -1;

                lvFunctionalityProfiles.Add(item);

                if (selectedID == (string)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvFunctionalityProfiles.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvFunctionalityProfiles.SortColumn = sortBy;

            lvFunctionalityProfiles_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvFunctionalityProfiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvFunctionalityProfiles.SortColumn == e.Column)
            {
                lvFunctionalityProfiles.SortedBackwards = !lvFunctionalityProfiles.SortedBackwards;
            }
            else
            {
                if (lvFunctionalityProfiles.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvFunctionalityProfiles.Columns[lvFunctionalityProfiles.SortColumn].ImageIndex = 2;
                }
                lvFunctionalityProfiles.SortedBackwards = false;
            }

            LoadFunctionalityProfiles(e.Column, lvFunctionalityProfiles.SortedBackwards);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileEdit))
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
            lvFunctionalityProfiles.SmallImageList = null;

            base.OnClose();
        }

       
    }
}
