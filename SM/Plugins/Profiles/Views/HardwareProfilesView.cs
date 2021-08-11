using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using System.Linq;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class HardwareProfilesView : ViewBase
    {
        private string selectedID = "";
        private List<HardwareProfile> profiles;

        public HardwareProfilesView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = (string)selectedID;
        }

        public HardwareProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close;


            lvHardwareProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvHardwareProfiles.ContextMenuStrip.Opening += lvHardwareProfiles_Opening;

            lvHardwareProfiles.SmallImageList = PluginEntry.Framework.GetImageList();

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.HardwareProfileEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("HardwareProfiles", RecordIdentifier.Empty, Properties.Resources.FunctionalityProfile, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.HardwareProfiles;
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

            HeaderText = Properties.Resources.HardwareProfiles;
            //HeaderIcon = Properties.Resources.Profiles16;

            LoadHardwareProfiles(1, false);

            
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
                case "HardwareProfile":
                    LoadHardwareProfiles(lvHardwareProfiles.SortColumn, lvHardwareProfiles.SortedBackwards);
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id;

            id = PluginOperations.NewHardwareProfile();

            if (id != RecordIdentifier.Empty)
            {
                selectedID = (string)id;
            }
        }

        private void lvHardwareProfiles_SizeChanged(object sender, EventArgs e)
        {
            lvHardwareProfiles.Columns[1].Width = -2;
        }

        private void lvHardwareProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvHardwareProfiles.SelectedItems.Count > 0) ? (string)lvHardwareProfiles.SelectedItems[0].Tag : "";
            bool isProfileInUse = selectedID == string.Empty ? false : profiles.Single(x => x.ID == selectedID).ProfileIsUsed;
            btnsContextButtons.EditButtonEnabled = (lvHardwareProfiles.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.HardwareProfileView);
            btnsContextButtons.RemoveButtonEnabled = (lvHardwareProfiles.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.HardwareProfileEdit) && !isProfileInUse;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowHardwareProfileSheet((string)lvHardwareProfiles.SelectedItems[0].Tag);
        }

        private void lvHardwareProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvHardwareProfiles_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvHardwareProfiles.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("HardwareProfileList", lvHardwareProfiles.ContextMenuStrip, lvHardwareProfiles);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteHardwareProfile(selectedID);
        }

        private void LoadHardwareProfiles(int sortBy, bool backwards)
        {
            ListViewItem item;
            string sort;

            lvHardwareProfiles.Items.Clear();

            sort = (sortBy == 0) ? "Len(PROFILEID) ASC,PROFILEID ASC" : "NAME ASC";

            if (backwards)
            {
                sort = sort.Replace("ASC", "DESC");
            }

            profiles = Providers.HardwareProfileData.GetHardwareProfileList(PluginEntry.DataModel, sort);

            foreach (DataEntity profile in profiles)
            {
                item = new ListViewItem((string)profile.ID);
                item.SubItems.Add(profile.Text);
                item.Tag = (string)profile.ID;
                item.ImageIndex = -1;

                lvHardwareProfiles.Add(item);

                if (selectedID == (string)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvHardwareProfiles.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvHardwareProfiles.SortColumn = sortBy;

            lvHardwareProfiles_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvTransactionServiceProfiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvHardwareProfiles.SortColumn == e.Column)
            {
                lvHardwareProfiles.SortedBackwards = !lvHardwareProfiles.SortedBackwards;
            }
            else
            {
                if (lvHardwareProfiles.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvHardwareProfiles.Columns[lvHardwareProfiles.SortColumn].ImageIndex = 2;
                }
                lvHardwareProfiles.SortedBackwards = false;
            }

            LoadHardwareProfiles(e.Column, lvHardwareProfiles.SortedBackwards);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.HardwareProfileEdit))
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
            lvHardwareProfiles.SmallImageList = null;

            base.OnClose();
        }
       
    }
}
