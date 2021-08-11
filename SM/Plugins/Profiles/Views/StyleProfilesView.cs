using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Views
{
    /// <summary>
    /// The view that displays all the <see cref="StyleProfile" />s available in the system.
    /// </summary>
    public partial class StyleProfilesView : ViewBase
    {
        /// <summary>
        /// The selected ID
        /// </summary>
        RecordIdentifier selectedID;

        /// <summary>
        /// Constructor for the dialog where a specific ID of a <see cref="StyleProfile" /> is sent to the view
        /// </summary>
        /// <param name="selectedId">A unique ID for a <see cref="StyleProfile" /> that should be selected when the view is opened</param>
        public StyleProfilesView(RecordIdentifier selectedId)
            :base()
        {
            this.selectedID = selectedId;
        }

        /// <summary>
        /// The default constructor for the dialog. All class variables are initialized
        /// </summary>
        public StyleProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.StyleProfiles;            

            lvStylesProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvStylesProfiles.ContextMenuStrip.Opening += ContextMenuStrip_Opening;            

            lvStylesProfiles.SmallImageList = PluginEntry.Framework.GetImageList();
            
            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.StyleProfileEdit);

        }

        /// <summary>
        /// The view's audit descriptors available for the view are defined
        /// </summary>
        /// <param name="contexts">The audit context</param>
        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("StyleProfiles", 0, Properties.Resources.StyleProfile, false));            
        }

        /// <summary>
        /// Overload this to return the ID of the current view. This should return RecordIdentifier.Empty for single instance views. For multi instance
        /// view return logical context identifier, f.x. for a store that would be the storeId
        /// </summary>
        /// <value>The ID</value>
        /// <returns></returns>
        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        /// <summary>
        /// The logical context name of the view. Is displayed in as a header in the Context box
        /// </summary>
        /// <value>The name of the logical context.</value>
        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.StyleProfiles;
            }
        }

        /// <summary>
        /// The event called by the Site Manager framework during a data change broadcast .
        /// </summary>
        /// <param name="changeAction">The data change being done</param>
        /// <param name="objectName">The name of the object being changed</param>
        /// <param name="changeIdentifier">A unique ID of the object being changed</param>
        /// <param name="param">A parameter send with the data change broadcast</param>
        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "StyleProfiles":                    
                    if (changeAction == DataEntityChangeType.Add || changeAction == DataEntityChangeType.Edit || changeAction == DataEntityChangeType.Delete)
                    {
                        selectedID = changeIdentifier;
                        LoadData(false);
                    }
                    break;                
            }
        }

        /// <summary>
        /// Override this to load data from the db into the view
        /// </summary>
        /// <param name="isRevert">Tells if we are loading from a revert call</param>
        protected override void LoadData(bool isRevert)
        {            
            LoadStyleProfiles(0, false);            
        }

        /// <summary>
        /// Loads the style profiles.
        /// </summary>
        /// <param name="sortBy">The index number of the column that is the data should be sorted by</param>
        /// <param name="backwards">if set to <c>true</c> the sorting is backwards.</param>
        private void LoadStyleProfiles(int sortBy, bool backwards)
        {            
            List<DataEntity> profiles;
            ListViewItem item;
            string sort;

            lvStylesProfiles.Items.Clear();

            sort = (sortBy == 0) ? "NAME ASC" : "LEN(NAME) ASC, NAME ASC";

            if (backwards)
            {
                sort = sort.Replace("ASC", "DESC");
            }

            profiles = Providers.StyleProfileData.GetList(PluginEntry.DataModel, sort);

            foreach (DataEntity profile in profiles)
            {
                item = new ListViewItem(profile.Text);
                item.Tag = (RecordIdentifier)profile.ID;
                item.ImageIndex = -1;
                lvStylesProfiles.Add(item);

                if (selectedID == (RecordIdentifier)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvStylesProfiles.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvStylesProfiles.SortColumn = sortBy;
            lvStylesProfiles.BestFitColumns();
            lvStyleProfiles_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvStyleProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvStylesProfiles.SelectedItems.Count > 0) ? (RecordIdentifier)lvStylesProfiles.SelectedItems[0].Tag : "";
            btnsEditAddRemove.EditButtonEnabled = lvStylesProfiles.SelectedItems.Count != 0 && PluginEntry.DataModel.HasPermission(Permission.StyleProfileView); 
            btnsEditAddRemove.RemoveButtonEnabled = lvStylesProfiles.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.StyleProfileEdit);            
        }
                
        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowStyleProfileSheet(this, (RecordIdentifier)lvStylesProfiles.SelectedItems[0].Tag);
        }

        
        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewStyleProfile();           
        }
                
        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {            
            selectedID = (RecordIdentifier)lvStylesProfiles.SelectedItems[0].Tag;
            PluginOperations.DeleteStyleProfile(selectedID);            
        }

        
        /// <summary>
        /// This function is run when the user right clicks the list
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvStylesProfiles.ContextMenuStrip;
            menu.Items.Clear();

            // Each item is a line in the right click menu. 
            // Usually there is not much that needs to be changed here

            var item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnsEditAddRemove_EditButtonClicked);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true; // The default item has a bold font
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("StyleProfileList", lvStylesProfiles.ContextMenuStrip, lvStylesProfiles);

            e.Cancel = (menu.Items.Count == 0);            
        }
        
        private void lvDataObjects_DoubleClick(object sender, EventArgs e)
        {
            // If the edit button is enabled when run the edit button operation. No need to change anything here
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
        
        private void lvStylesProfiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvStylesProfiles.SortColumn == e.Column)
            {
                lvStylesProfiles.SortedBackwards = !lvStylesProfiles.SortedBackwards;
            }
            else
            {
                if (lvStylesProfiles.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvStylesProfiles.Columns[lvStylesProfiles.SortColumn].ImageIndex = 2;
                }
                lvStylesProfiles.SortedBackwards = false;
            }

            LoadStyleProfiles(e.Column, lvStylesProfiles.SortedBackwards);
        }

        /// <summary>
        /// Called when the view is closed
        /// </summary>
        protected override void OnClose()
        {
            lvStylesProfiles.SmallImageList = null;

            base.OnClose();
        }

    }
}
