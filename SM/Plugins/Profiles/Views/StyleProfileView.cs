using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Profiles.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Views
{
    /// <summary>
    /// The view that displays the profile lines on the specific <see cref="StyleProfile"/>
    /// </summary>
    public partial class StyleProfileView : ViewBase
    {
        private int selectedIndex;
        private RecordIdentifier styleProfileID;
        private StyleProfile styleProfile = new StyleProfile();
        private RecordIdentifier selectedProfileLineID;
        private int columnSortedBy;
        private bool columnSortedBackwards;
        
        /// <summary>
        /// The constructor used when editing a specific <see cref="StyleProfile"/>
        /// </summary>
        /// <param name="objectId">The unique ID of the style profile</param>
        public StyleProfileView(RecordIdentifier objectId)
            : this()
        {
            styleProfileID = objectId;

            HeaderText = Resources.StyleProfile;
            //HeaderIcon = Resources.Profiles16;
            styleProfile = Providers.StyleProfileData.Get(PluginEntry.DataModel, styleProfileID);

            tbID.Text = (string)styleProfile.ID;
            txtDescription.Text = styleProfile.Text;
            columnSortedBy = 0;
            columnSortedBackwards = false;                      
        }

        
        public StyleProfileView()
        {
            InitializeComponent();

            // These are the attributes that the view should support.
            Attributes = 
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.StyleProfileEdit);
            btnsContextButtons.AddButtonEnabled = !ReadOnly;  
            selectedIndex = -1;            

            lvProfileSettings.ContextMenuStrip = new ContextMenuStrip();
            lvProfileSettings.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

        }        

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.StyleProfile;
            }
        }

        /// <summary>
        /// The current <see cref="StyleProfile"/> being edited
        /// </summary>
        public override RecordIdentifier ID
        {
            get { return styleProfileID; }
        }
        
        protected override void LoadData(bool isRevert)
        {
            LoadStyleProfiles(columnSortedBy, columnSortedBackwards);            
        }

        private void LoadStyleProfiles(int sortBy, bool backwards)
        {
            List<PosStyleProfileLine> profiles;
            string sort = "";

            lvProfileSettings.ClearRows();

            var columns = new string[] { "CONTEXTDESCRIPTION", "MENUDESCRIPTION", "STYLEDESCRIPTION", "SYSTEM" };

            if (sortBy < columns.Length)
            {
                sort = columns[sortBy] + (backwards ? " DESC" : " ASC" );
            }

            profiles = Providers.PosStyleProfileLineData.GetProfileLines(PluginEntry.DataModel, styleProfileID, sort);            
            int i = 0;            

            foreach (PosStyleProfileLine profile in profiles)
            {
                var row = new Row();
                row.AddText(profile.ContextDescription);
                row.AddText(profile.MenuDescription);
                row.AddText(profile.StyleDescription);
                row.AddCell(new CheckBoxCell(profile.System ? Properties.Resources.IsSystemStyle : "", profile.System, false));
                row.Tag = profile.PosStyleProfileLineId;                
                lvProfileSettings.AddRow(row);
                if (selectedProfileLineID == profile.PosStyleProfileLineId)
                {
                    selectedIndex = i;
                }
                i++;
            }            

            Row selectedRow = null;

            if (selectedProfileLineID != null)
            {                
                selectedRow = lvProfileSettings.Rows.FirstOrDefault(f => (RecordIdentifier)f.Tag == selectedProfileLineID);
                if (selectedRow == null) //Will happen after a delete action and when first line added
                {
                    //If the last line was deleted - move the index up to the current last line
                    selectedIndex = selectedIndex >= lvProfileSettings.Rows.Count() ? lvProfileSettings.Rows.Count()-1 : selectedIndex;
                    selectedRow = lvProfileSettings.Row(selectedIndex);
                }
            }

            lvProfileSettings.Selection.Set(selectedIndex);
            lvProfileSettings.SetSortColumn(sortBy, !backwards);
            lvProfileSettings_RowClick(this, new RowEventArgs(selectedIndex, selectedRow, null));
            
        }

        protected override bool DataIsModified()
        {
            if (txtDescription.Text != styleProfile.Text) return true;

            return false;
        }

        protected override bool SaveData()
        {
            styleProfile.Text = txtDescription.Text;
            Providers.StyleProfileData.Save(PluginEntry.DataModel, styleProfile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "StyleProfiles", styleProfileID, null);            

            return true;
        }

        /// <summary>
        /// The view's audit descriptors available for the view are defined
        /// </summary>
        /// <param name="contexts">The audit context</param>
        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("StyleProfileLines", styleProfile.ID, Properties.Resources.StyleProfileLines, false));
            contexts.Add(new AuditDescriptor("Context", 0, Properties.Resources.Context, false));            
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
                case "ContextStyle":
                    if (changeAction == DataEntityChangeType.Delete || changeAction == DataEntityChangeType.Add || changeAction == DataEntityChangeType.Delete)
                    {
                        selectedProfileLineID = changeIdentifier;
                        LoadStyleProfiles(columnSortedBy, columnSortedBackwards);
                    }
                    break;
                case "StyleProfiles":
                    if (changeAction == DataEntityChangeType.Delete && styleProfileID == changeIdentifier)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;                        
            }            
        }        

        protected override void OnDelete()
        {
            PluginOperations.DeleteStyleProfile(styleProfileID);
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier ID = PluginOperations.NewContextStyle(styleProfileID);            
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            if (selectedProfileLineID == null)
                return;

            RecordIdentifier ID = PluginOperations.NewContextStyle(styleProfileID, selectedProfileLineID);           
        }

        private void lvProfileSettings_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = (lvProfileSettings.Selection.Count == 1);
            btnsContextButtons.RemoveButtonEnabled = (lvProfileSettings.Selection.Count >= 1);     
        }

        private void lvProfileSettings_RowClick(object sender, RowEventArgs args)
        {
            if (args.Row != null)
            {
                selectedProfileLineID = (lvProfileSettings.Selection.Count > 0) ? (RecordIdentifier)args.Row.Tag : "";
            }

            selectedIndex = args.RowNumber;

            btnsContextButtons.EditButtonEnabled = (lvProfileSettings.Selection.Count == 1) && PluginEntry.DataModel.HasPermission(Permission.StyleProfileEdit);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (selectedProfileLineID != null)
            {
                PluginOperations.DeleteStyleProfileLine(selectedProfileLineID);
            }
        }

        private void lvProfileSettings_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (columnSortedBy == args.ColumnNumber)
            {
                columnSortedBackwards = !columnSortedBackwards;
            }
            else
            {
                columnSortedBackwards = false;
            }

            columnSortedBy = args.ColumnNumber;
            LoadStyleProfiles(columnSortedBy, columnSortedBackwards);
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvProfileSettings.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnsContextButtons_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsContextButtons_AddButtonClicked);

            item.Enabled = btnsContextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsContextButtons_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ContextStyle", lvProfileSettings.ContextMenuStrip, lvProfileSettings);

            e.Cancel = (menu.Items.Count == 0);

        }
    }
}
