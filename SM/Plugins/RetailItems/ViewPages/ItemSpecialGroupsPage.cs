using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.ViewPlugins.RetailItems.HelperClasses;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class ItemSpecialGroupsPage : UserControl, ITabViewV2, IMultiEditTabExtension
    {
        RecordIdentifier itemId;
        WeakReference owner;

        public ItemSpecialGroupsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);

            if (((ViewBase)owner.Parent.Parent).ReadOnly)
            {
                lvSpecialGroups.Enabled = false;
            }

            lvSpecialGroups.ContextMenuStrip = new ContextMenuStrip();
            lvSpecialGroups.ContextMenuStrip.Opening += new CancelEventHandler(lvSpecialGroups_ContextMenuStripOpening);

            btnAdd.Enabled = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups);
        }

        public ItemSpecialGroupsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ItemSpecialGroupsPage((TabControl)sender);
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            bool datachanged = false;

            foreach (Row row in lvSpecialGroups.Rows)
            {
                if(((CheckBoxCell)row[0]).Checked != ((SpecialGroupItem)row.Tag).ItemIsInGroup)
                {
                    ((SpecialGroupItem)row.Tag).Dirty = true;
                    datachanged = true;
                }
            }

            return datachanged;

        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("SpecialGroupsItem", itemId, Properties.Resources.SpecialGroup, false));
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            Row row;

            itemId = ((RetailItem)internalContext).ID;

            if (!PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups))
            {
                lvSpecialGroups.Enabled = false;
            }

            if(itemId == RecordIdentifier.Empty)
            {
                // We are in multi edit mode
                lvSpecialGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRoundedColorMerge;

                List<DataEntity> groups = Providers.SpecialGroupData.GetList(PluginEntry.DataModel);

                foreach (DataEntity group in groups)
                {
                    row = new Row();
                    row.Tag = new HelperClasses.SpecialGroupItemMultiEdit() { GroupId = group.ID, ItemId = "", GroupName = group.Text, ItemIsInGroup = false, CheckState = CheckState.Indeterminate };
                    row.AddCell(new CheckBoxCell(group.Text, false) { ThreeState = true, CheckState = CheckState.Indeterminate });

                    lvSpecialGroups.AddRow(row);
                }
            }
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            Row row;
             
            itemId = ((RetailItem)internalContext).ID;

            List<SpecialGroupItem> groups = Providers.SpecialGroupData.GetItemsGroupInformation(PluginEntry.DataModel, itemId);

            // Very important, always start with clear since Initialize can be called again if the user hits revert.
            lvSpecialGroups.ClearRows();

            foreach (SpecialGroupItem group in groups)
            {
                row = new Row();
                row.Tag = group;
                row.AddCell(new CheckBoxCell(group.GroupName,group.ItemIsInGroup));

                lvSpecialGroups.AddRow(row);
            }

            lvSpecialGroups.AutoSizeColumns();
        }

        public bool SaveData()
        {
            foreach (Row row in lvSpecialGroups.Rows)
            {
                if (((SpecialGroupItem)row.Tag).Dirty)
                {
                    if (((CheckBoxCell)row[0]).Checked)
                    {
                        Providers.SpecialGroupData.AddItemToSpecialGroup(PluginEntry.DataModel, itemId, ((SpecialGroupItem)row.Tag).GroupId);
                    }
                    else
                    {
                        Providers.SpecialGroupData.RemoveItemFromSpecialGroup(PluginEntry.DataModel, itemId, ((SpecialGroupItem)row.Tag).GroupId);
                    }
                }
            }

            return true;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion


        

        void lvSpecialGroups_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            lvSpecialGroups.ContextMenuStrip.Items.Clear();

            if (lvSpecialGroups.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageSpecialGroups))
            {
                if (((CheckBoxCell)lvSpecialGroups.Row(lvSpecialGroups.Selection.FirstSelectedRow)[0]).Checked)
                {
                    lvSpecialGroups.ContextMenuStrip.Items.Add(new ExtendedMenuItem(
                        Properties.Resources.RemoveFromGroup,
                        100,
                        new EventHandler(lvSpecialGroups_ToggleGroup)));
                }
                else
                {
                    lvSpecialGroups.ContextMenuStrip.Items.Add(new ExtendedMenuItem(
                        Properties.Resources.AddToGroup,
                        150,
                        new EventHandler(lvSpecialGroups_ToggleGroup)));
                }

                
            }

            lvSpecialGroups.ContextMenuStrip.Items.Add(new ExtendedMenuItem("-",500));

            ExtendedMenuItem item = new ExtendedMenuItem(Properties.Resources.NewSpecialGroup, ContextButtons.GetAddButtonImage(), 510, new EventHandler(btnAdd_Click));
            item.Enabled = btnAdd.Enabled;

            lvSpecialGroups.ContextMenuStrip.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("SpecialGroupAssignment", lvSpecialGroups.ContextMenuStrip, lvSpecialGroups);

            e.Cancel = false;

        }

        private void lvSpecialGroups_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            if(itemId == RecordIdentifier.Empty)
            {
                Row row = lvSpecialGroups.Row(args.RowNumber);

                if (((CheckBoxCell)args.Cell).CheckState != ((SpecialGroupItemMultiEdit)row.Tag).CheckState)
                {
                    row.BackColor = ColorPalette.MultiEditHighlight;
                }
                else
                {
                    row.BackColor = System.Drawing.Color.Empty;
                }
            }
        }

        private void lvSpecialGroups_ToggleGroup(object sender, EventArgs args)
        {
            if (lvSpecialGroups.Selection.Count > 0)
            {
                Row row = lvSpecialGroups.Row(lvSpecialGroups.Selection.FirstSelectedRow);

                ((CheckBoxCell)row[0]).Checked = !((CheckBoxCell)row[0]).Checked;

                row.BackColor = ((CheckBoxCell)row[0]).CheckState != ((SpecialGroupItemMultiEdit)row.Tag).CheckState ? ColorPalette.MultiEditHighlight : System.Drawing.Color.Empty;

                lvSpecialGroups.Invalidate();
            }
        }

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            foreach(Row row in lvSpecialGroups.Rows)
            {
                if(((HelperClasses.SpecialGroupItemMultiEdit)row.Tag).CheckState != ((CheckBoxCell)row[0]).CheckState)
                {
                    return true;
                }
            }

            return false;
        }

        public void MultiEditSaveSecondaryRecords(IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            HelperClasses.SpecialGroupItemMultiEdit specialGroupItem;

            foreach (Row row in lvSpecialGroups.Rows)
            {
                specialGroupItem = ((HelperClasses.SpecialGroupItemMultiEdit)row.Tag);

                if (specialGroupItem.CheckState != ((CheckBoxCell)row[0]).CheckState)
                {
                    if (((CheckBoxCell)row[0]).Checked)
                    {
                        Providers.SpecialGroupData.AddItemToSpecialGroup(threadedConnection, primaryRecordID.SecondaryID, specialGroupItem.GroupId);
                    }
                    else
                    {
                        Providers.SpecialGroupData.RemoveItemFromSpecialGroup(threadedConnection, primaryRecordID.SecondaryID, specialGroupItem.GroupId);
                    }
                }
            }
        }

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {
            HelperClasses.SpecialGroupItemMultiEdit specialGroupItem;

            foreach (Row row in lvSpecialGroups.Rows)
            {
                specialGroupItem = ((HelperClasses.SpecialGroupItemMultiEdit)row.Tag);

                if (specialGroupItem.CheckState != ((CheckBoxCell)row[0]).CheckState)
                {
                    row.BackColor = System.Drawing.Color.Empty;
                    specialGroupItem.CheckState = ((CheckBoxCell)row[0]).CheckState;
                    ((CheckBoxCell)row[0]).ThreeState = false;
                }
            }

            lvSpecialGroups.Invalidate();
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            if(control == lvSpecialGroups)
            {
                Row row;

                handled = true;

                if(isRevertField)
                {
                    if(lvSpecialGroups.Selection.Count > 0)
                    {
                        row = lvSpecialGroups.Row(lvSpecialGroups.Selection.FirstSelectedRow);

                        ((CheckBoxCell)row[0]).CheckState = ((HelperClasses.SpecialGroupItemMultiEdit)row.Tag).CheckState;
                        row.BackColor = System.Drawing.Color.Empty;
                    }
                }
                else
                {
                    foreach(Row listViewRow in lvSpecialGroups.Rows)
                    {
                        ((CheckBoxCell)listViewRow[0]).CheckState = ((HelperClasses.SpecialGroupItemMultiEdit)listViewRow.Tag).CheckState;
                        listViewRow.BackColor = System.Drawing.Color.Empty;
                    }
                }
            }

            lvSpecialGroups.Invalidate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Row row;

            Dialogs.SpecialGroupDialog dlg = new Dialogs.SpecialGroupDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                row = new Row();

                if (itemId == RecordIdentifier.Empty)
                {
                    // We are in multi edit mode
                    row.Tag = new HelperClasses.SpecialGroupItemMultiEdit() { GroupId = dlg.GetSelectedId(), ItemId = "", GroupName = dlg.GetSelectedText(), ItemIsInGroup = false, CheckState = CheckState.Unchecked };
                    row.AddCell(new CheckBoxCell(dlg.GetSelectedText(), false) { ThreeState = false, CheckState = CheckState.Unchecked }); 
                }
                else
                {
                    SpecialGroupItem item = new SpecialGroupItem();

                    item.Text = dlg.GetSelectedText();
                    item.GroupId = dlg.GetSelectedId();
                    item.ItemId = (string)itemId;
                    item.ItemIsInGroup = false;

                    row.Tag = item;
                    row.AddCell(new CheckBoxCell(item.Text, item.ItemIsInGroup));
                }

                lvSpecialGroups.AddRow(row);
            }
        }

      
    }
}
