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
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Views
{
    /// <summary>
    /// Displays a list of all the <see cref="PosContext" />s available in the system.
    /// </summary>
    public partial class ContextsView : ViewBase
    {
        private RecordIdentifier selectedId;
        private bool backwardsSort;
        private int selectedIndex;
        private int sortBy;
        private List<PosContext> contextList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextsView" /> class.
        /// </summary>
        /// <param name="selectedId">The selected id.</param>
        public ContextsView(RecordIdentifier selectedId)
            :base()
        {
            this.selectedId = selectedId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextsView" /> class.
        /// </summary>
        public ContextsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.Contexts;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ContextEdit);

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ContextEdit);
            btnsEditAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ContextEdit);
            btnsEditAddRemove.EditButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ContextEdit);

            lvContexts.ContextMenuStrip = new ContextMenuStrip();
            lvContexts.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            backwardsSort = false;
            selectedIndex = -1;
            sortBy = 0;
            contextList = new List<PosContext>();
        }

        /// <summary>
        /// Sets the top context bar header. This context bar header can include Save, Close, Delete and Revert but you can also add to it.
        /// </summary>
        /// <value>The text to be displayed on the header of the Context box</value>
        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Contexts;
            }
        }

        /// <summary>
        /// Overload this to return the ID of the current view. This should return RecordIdentifier.Empty for single instance views. For multi instance
        /// view return logical context identifier, f.x. for a store that would be the storeId
        /// </summary>
        /// <value>The ID.</value>
        /// <returns>The logical context ID if applicable</returns>
        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> descriptors)
        {
            descriptors.Add(new AuditDescriptor("Context", 0, Properties.Resources.Context, false));            
        }

        /// <summary>
        /// View should overload to listen to change broadcasts (NotifyDataChanged())
        /// </summary>
        /// <param name="changeAction">Enum that tells you the type of change</param>
        /// <param name="objectName">Tells you what changed , f.x. "Store"</param>
        /// <param name="changeIdentifier">The ID of the changed object</param>
        /// <param name="param">Extra information</param>
        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PosContext" && (changeAction == DataEntityChangeType.Add || changeAction == DataEntityChangeType.MultiDelete || changeAction == DataEntityChangeType.Delete || changeAction == DataEntityChangeType.Edit))
            {
                selectedId = changeIdentifier;
                LoadObjects(backwardsSort, sortBy);
            }
        }

        /// <summary>
        /// Loads and displays the data to be available in the view. The function is called by the framework when the view is being loaded.
        /// </summary>
        /// <param name="isRevert">Tells if we are loading from a revert call</param>
        /// <remarks>Overload this function in each view to load the relevant data</remarks>
        protected override void LoadData(bool isRevert)
        {
            LoadObjects(backwardsSort, sortBy);
        }

        private void LoadObjects(bool backwards, int sortBy)
        {            
            lvContexts.ClearRows();

            string sort = "";

            string[] columns = new string[] { "NAME", "MENUREQUIRED" };

            if (sortBy < columns.Length)
            {
                sort = columns[sortBy] + (backwards ? " DESC" : " ASC");
            }             

            contextList.Clear();
            contextList = Providers.PosContextData.GetList(PluginEntry.DataModel, sort);

            int i = 0;

            foreach (PosContext context in contextList)
            {
                #region Create and add row

                var row = new Row();
                row.AddText(context.Text);
                row.AddCell(new CheckBoxCell(context.MenuRequired, false));
                row.AddCell(new CheckBoxCell(context.UsedInStyleProfile, false));
                row.Tag = context.ID;
                lvContexts.AddRow(row);

                #endregion

                if (selectedId == context.ID)
                {
                    selectedIndex = i;
                }
                i++;                                
            }

            Row selectedRow = null;

            if (selectedId != null)
            {                
                selectedRow = lvContexts.Rows.FirstOrDefault(f => (RecordIdentifier)f.Tag == selectedId);
                if (selectedRow == null) //Will happen after a delete action and when first line added
                {
                    //If the last line was deleted - move the index up to the current last line
                    selectedIndex = selectedIndex >= lvContexts.Rows.Count() ? lvContexts.Rows.Count()-1 : selectedIndex;
                    selectedRow = lvContexts.Row(selectedIndex);
                }
            }

            lvContexts.Selection.Set(selectedIndex);
            lvContexts.SetSortColumn(sortBy, !backwards);
            lvContexts_RowClick(this, new RowEventArgs(selectedIndex, selectedRow, null));
        }
        

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            if (selectedId == null)
                return;

            PluginOperations.EditPosContext(selectedId);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewPosContext();         
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (selectedId != null)
            {
                PluginOperations.DeletePosContext(GetSelectedIDs());
            }
        }

        private List<RecordIdentifier> GetSelectedIDs()
        {
            var IDs = new List<RecordIdentifier>();
            
            for (int i = 0; i < lvContexts.Selection.Count; i++)
            {                
                var selectedID = (RecordIdentifier)lvContexts.Selection[i].Tag;
                var selected = contextList.FirstOrDefault(f => f.ID == selectedID);
                if (selected != null)
                {
                    if (selected.UsedInStyleProfile)
                    {
                        MessageDialog.Show(Properties.Resources.ContextUsedInProfileCannotBeDeleted);
                        return new List<RecordIdentifier>();                
                    }
                    
                    IDs.Add(selectedID);                    
                }
            }            
            

            return IDs;
        }

        // This function is run when the user right clicks the list
        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvContexts.ContextMenuStrip;
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

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvContexts_RowDoubleClick(object sender, RowEventArgs args)
        {
            // If the edit button is enabled when run the edit button operation. No need to change anything here
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvContexts_RowClick(object sender, RowEventArgs args)
        {
            if (args.Row != null)
            {
                selectedId = (lvContexts.Selection.Count > 0) ? (RecordIdentifier)args.Row.Tag : "";
            }

            selectedIndex = args.RowNumber;

            btnsEditAddRemove.EditButtonEnabled = (lvContexts.Selection.Count == 1) && PluginEntry.DataModel.HasPermission(Permission.ContextEdit);
            btnsEditAddRemove.RemoveButtonEnabled = (lvContexts.Selection.Count >= 1) && PluginEntry.DataModel.HasPermission(Permission.ContextEdit);
        }

        private void lvContexts_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (sortBy == args.ColumnNumber)
            {
                backwardsSort = !backwardsSort;
            }
            else
            {
                backwardsSort = false;
            }

            sortBy = args.ColumnNumber;
            LoadObjects(backwardsSort, sortBy);            
        }
    }
}
