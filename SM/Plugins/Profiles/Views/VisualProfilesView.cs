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
    public partial class VisualProfilesView : ViewBase
    {
        private string selectedID = "";
        private List<VisualProfile> profiles;

        public VisualProfilesView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID.ToString();
        }

        public VisualProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvVisualProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvVisualProfiles.ContextMenuStrip.Opening += lvVisualProfiles_Opening;

            lvVisualProfiles.SmallImageList = PluginEntry.Framework.GetImageList();

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("VisualProfiles", RecordIdentifier.Empty, Properties.Resources.VisualProfiles, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.VisualProfiles;
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

            HeaderText = Properties.Resources.VisualProfiles;
            //HeaderIcon = Properties.Resources.Profiles16;

            LoadVisualProfiles(1,false);

            
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
                case "VisualProfile":
                    LoadVisualProfiles(lvVisualProfiles.SortColumn, lvVisualProfiles.SortedBackwards);
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id;

            id = PluginOperations.NewVisualProfile();

            if (id != RecordIdentifier.Empty)
            {
                selectedID = (string)id;
            }
        }

        private void lvVisualProfiles_SizeChanged(object sender, EventArgs e)
        {
            lvVisualProfiles.Columns[1].Width = -2;
        }

        private void lvVisualProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvVisualProfiles.SelectedItems.Count > 0) ? (string)lvVisualProfiles.SelectedItems[0].Tag : "";
            bool isProfileInUse = selectedID == string.Empty ? false : profiles.Single(x => x.ID == selectedID).ProfileIsUsed;
            btnsContextButtons.EditButtonEnabled = (lvVisualProfiles.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.VisualProfileView);
            btnsContextButtons.RemoveButtonEnabled = (lvVisualProfiles.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit) && !isProfileInUse;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowVisualProfileSheet(this, (string)lvVisualProfiles.SelectedItems[0].Tag);
        }

        private void lvVisualProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvVisualProfiles_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvVisualProfiles.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("VisualProfileList", lvVisualProfiles.ContextMenuStrip, lvVisualProfiles);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteVisualProfile(selectedID);
        }

        private void LoadVisualProfiles(int sortBy, bool backwards)
        {
            ListViewItem item;
            string sort;

            lvVisualProfiles.Items.Clear();

            sort = (sortBy == 0) ? "Len(PROFILEID) ASC,PROFILEID ASC" : "NAME ASC";

            if (backwards)
            {
                sort = sort.Replace("ASC", "DESC");
            }

            profiles = Providers.VisualProfileData.GetVisualProfileList(PluginEntry.DataModel,sort);

            foreach (DataEntity profile in profiles)
            {
                item = new ListViewItem((string)profile.ID);
                item.SubItems.Add(profile.Text);
                item.Tag = (string)profile.ID;
                item.ImageIndex = -1;

                lvVisualProfiles.Add(item);

                if (selectedID == (string)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvVisualProfiles.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvVisualProfiles.SortColumn = sortBy;

            lvVisualProfiles_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvVisualProfiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvVisualProfiles.SortColumn == e.Column)
            {
                lvVisualProfiles.SortedBackwards = !lvVisualProfiles.SortedBackwards;
            }
            else
            {
                if (lvVisualProfiles.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvVisualProfiles.Columns[lvVisualProfiles.SortColumn].ImageIndex = 2;
                }
                lvVisualProfiles.SortedBackwards = false;
            }

            LoadVisualProfiles(e.Column, lvVisualProfiles.SortedBackwards);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit))
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
            lvVisualProfiles.SmallImageList = null;

            base.OnClose();
        }

      
    }
}
