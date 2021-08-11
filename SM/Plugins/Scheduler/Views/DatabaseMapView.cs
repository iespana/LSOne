using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class DatabaseMapView : ViewBase
    {
        private Guid tableMapId;
        private JscTableMap tableMap;
        private bool dataIsModified;
        private FieldMapDialog fieldMapDialog = new FieldMapDialog();

        public DatabaseMapView(Guid tableMapId)
            : this()
        {
            this.tableMapId = tableMapId;

            lvFieldMap.ContextMenuStrip = new ContextMenuStrip();
            lvFieldMap.ContextMenuStrip.Opening += new CancelEventHandler(lvFieldMap_Opening);

        }

        private DatabaseMapView()
        {
            InitializeComponent();
            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
        }


        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.DatabaseMap;
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


            this.ReadOnly = !PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseMapEdit);

            tableMap = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableMap(PluginEntry.DataModel, tableMapId);

            HeaderText = Properties.Resources.DatabaseMap;

            tbDbFrom.Text = tableMap.FromJscTableDesign.JscDatabaseDesign.Description;
            tbTableFrom.Text = tableMap.FromJscTableDesign.TableName;
            tbDbTo.Text = tableMap.ToJscTableDesign.JscDatabaseDesign.Description;
            tbTableTo.Text = tableMap.ToJscTableDesign.TableName;

            PopulateFieldMap();
            UpdateActions();

            dataIsModified = false;
        }




        protected override bool DataIsModified()
        {
            return dataIsModified;
        }

        protected override bool SaveData()
        {
            if (!ValidateFieldMap())
            {
                string text = Properties.Resources.DatabaseMapToFieldMsg;
                string caption = Properties.Resources.SaveDataHeader;
                MessageBox.Show(PluginEntry.Framework.MainWindow, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(PluginEntry.DataModel, tableMap);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "TableMap", tableMap.ID, null);

            return true;
        }

        private bool ValidateFieldMap()
        {
            foreach (var fieldMap in tableMap.JscFieldMaps)
            {
                if (fieldMap.ToJscFieldDesign == null)
                {
                    return false;
                }
            }

            return true;
        }



        private void PopulateFieldMap()
        {
            lvFieldMap.ClearRows();
            foreach (var fieldMap in tableMap.JscFieldMaps)
            {
                AddFieldMapToList(fieldMap);
            }
            lvFieldMap.AutoSizeColumns();
            lvFieldMap.Sort();
        }

        private void AddFieldMapToList(JscFieldMap fieldMap)
        {
            var row = new Row();
            FieldMapToRow(fieldMap, row);
            lvFieldMap.AddRow(row);
        }


        private void FieldMapToRow(JscFieldMap fieldMap, Row row)
        {
            row.Clear();
            string toField;
            if (fieldMap.ToJscFieldDesign != null)
            {
                toField = fieldMap.ToJscFieldDesign.FieldName;
            }
            else
            {
                toField = "";
                row.BackColor = Color.Red;
            }

            row.AddText(fieldMap.FromJscFieldDesign.FieldName);
            row.AddText(toField);
            row.AddText(Utils.Utils.EnumResourceString(Properties.Resources.ResourceManager, fieldMap.ConversionMethod));
            row.AddText(fieldMap.ConversionValue);
            row.Tag = fieldMap;
        }



        private void UpdateActions()
        {
            bool hasPermission = PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseMapEdit);
            int selectedCount = lvFieldMap.Selection.Count;

            btnsEditAddRemove.AddButtonEnabled = hasPermission;
            btnsEditAddRemove.EditButtonEnabled = hasPermission && selectedCount == 1;
            btnsEditAddRemove.RemoveButtonEnabled = hasPermission && selectedCount > 0;
        }


        private void btnEditFieldMap_Click(object sender, EventArgs e)
        {
            Row firstSelectedRow = lvFieldMap.Row(lvFieldMap.Selection.FirstSelectedRow);
            JscFieldMap fieldMap = (JscFieldMap)firstSelectedRow.Tag;
            if (fieldMapDialog.ShowDialog(this, fieldMap, (Guid)tableMap.FromTable,(Guid) tableMap.ToTable) == DialogResult.OK)
            {
                FieldMapToRow(fieldMapDialog.FieldMap, firstSelectedRow);
                lvFieldMap.AutoSizeColumns();
                dataIsModified = true;
            }
        }


        private void btnAddFieldMap_Click(object sender, EventArgs e)
        {
            if (fieldMapDialog.ShowDialog(this, null, (Guid)tableMap.FromTable, (Guid)tableMap.ToTable) == DialogResult.OK)
            {
                JscFieldMap fieldMap = fieldMapDialog.FieldMap;
                fieldMap.JscTableMap = tableMap;
                AddFieldMapToList(fieldMap);
                tableMap.JscFieldMaps.Add(fieldMap);
                lvFieldMap.AutoSizeColumns();
                dataIsModified = true;
            }
        }

        private void btnRemoveFieldMap_Click(object sender, EventArgs e)
        {
            string msg;
            if (lvFieldMap.Selection.Count == 1)
            {
                msg = Properties.Resources.FieldMapRemoveMsg;
            }
            else if (lvFieldMap.Selection.Count > 1)
            {
                msg = string.Format(Properties.Resources.FieldMapRemoveManyMsg, lvFieldMap.Selection.Count);
            }
            else
            {
                return;
            }

            if (QuestionDialog.Show(msg, Properties.Resources.FieldMapRemoveHeader) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                // First collect all selected items before we start removing from the list view
                List<Row> itemsToRemove = new List<Row>();
                for (int i = 0; i < lvFieldMap.Selection.Count; i++)
                {
                    itemsToRemove.Add(lvFieldMap.Selection[i]);
                }

                // Remove the collected items
                foreach (var item in itemsToRemove)
                {
                    RemoveFieldMap((JscFieldMap)item.Tag, item);
                }
                lvFieldMap.AutoSizeColumns();
            }
        }



        private void RemoveFieldMap(JscFieldMap fieldMap, Row row = null)
        {
            tableMap.JscFieldMaps.Remove(fieldMap);
            DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().DeleteFieldMap(PluginEntry.DataModel, fieldMap);
            if (row != null)
            {
                lvFieldMap.RemoveRow(lvFieldMap.Rows.IndexOf(row));
            }
            dataIsModified = true;
        }

        private void RemoveAll()
        {
            if (lvFieldMap.RowCount == 0)
                return;

            Cursor.Current = Cursors.WaitCursor;

            foreach (Row row in lvFieldMap.Rows)
            {
                RemoveFieldMap((JscFieldMap)row.Tag);
            }
            lvFieldMap.ClearRows();
        }


        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseMapEdit))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.FieldMapAddAllFields, new ContextbarClickEventHandler(AddAllFieldsHandler)), 300);
                    arguments.Add(new ContextBarItem(Properties.Resources.FieldMapUpdateFields, new ContextbarClickEventHandler(UpdateFieldsHandler)), 300);
                }
            }
            base.OnSetupContextBarItems(arguments);
        }


        private void AddAllFieldsHandler(object sender, ContextBarClickEventArguments args)
        {
            if (lvFieldMap.RowCount > 0)
            {
                var result = MessageBox.Show(PluginEntry.Framework.MainWindow, Properties.Resources.FieldMapOverwriteFieldsMsg, Properties.Resources.FieldMapAddAllFields, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
                if (result == DialogResult.Yes)
                {
                    RemoveAll();
                }
            }
            AddAllFields();
            PopulateFieldMap();
            dataIsModified = true;
        }


        private void UpdateFieldsHandler(object sender, ContextBarClickEventArguments args)
        {
            UpdateFields();
            PopulateFieldMap();
            dataIsModified = true;
        }

        private void AddAllFields()
        {
            Dictionary<string, JscFieldDesign> toFieldDesigns = GetFieldDesignDictionary((Guid)tableMap.ToTable);

            foreach (var fieldDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesignsOrderedByFieldName(PluginEntry.DataModel, tableMap.FromTable).ToList())
            {
                AddAutoFieldMap(toFieldDesigns, fieldDesign);
            }
        }



        private void UpdateFields()
        {
            Dictionary<string, JscFieldDesign> toFieldDesigns = GetFieldDesignDictionary((Guid) tableMap.ToTable);
            var fieldMapsByFromFieldDictionary = GetFieldMapsByFromFieldDictionary();

            foreach (var fieldDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesignsOrderedByFieldName(PluginEntry.DataModel, tableMap.FromTable))
            {
                // Check if this field exisis in our list
                List<JscFieldMap> items;
                if (!fieldMapsByFromFieldDictionary.TryGetValue((Guid) fieldDesign.ID, out items))
                {
                    // This field is not in our list, add it.
                    AddAutoFieldMap(toFieldDesigns, fieldDesign);
                }
                else
                {
                    // The field exists, we remove it from our dictionary so we know at the end
                    // which ones have been removed from the design
                    fieldMapsByFromFieldDictionary.Remove((Guid)fieldDesign.ID);
                }
            }

            // Leftovers: remove from our list all remaining fields
            var remainingFieldMapLists = fieldMapsByFromFieldDictionary.Values.ToArray();
            foreach (var fieldMapList in remainingFieldMapLists)
            {
                foreach (var fieldMapToDelete in fieldMapList)
                {
                    tableMap.JscFieldMaps.Remove(fieldMapToDelete);
                    DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().DeleteFieldMap(PluginEntry.DataModel, fieldMapToDelete);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "FieldMap", fieldMapToDelete.ID, null);
                }
            }
        }

        private void AddAutoFieldMap(Dictionary<string, JscFieldDesign> toFieldDesigns, JscFieldDesign fieldDesign)
        {
            JscFieldMap fieldMap = new JscFieldMap();
            fieldMap.FromJscFieldDesign = fieldDesign;
            JscFieldDesign toFieldDesign;
            if (toFieldDesigns.TryGetValue(fieldDesign.FieldName, out toFieldDesign))
            {
                fieldMap.ToJscFieldDesign = toFieldDesign;
            }
            else
            {
                fieldMap.ToJscFieldDesign = null;
            }
            fieldMap.ConversionMethod = ConversionMethod.None;
            fieldMap.JscTableMap = tableMap;
            tableMap.JscFieldMaps.Add(fieldMap);
        }

        private Dictionary<Guid, List<JscFieldMap>> GetFieldMapsByFromFieldDictionary()
        {
            Dictionary<Guid, List<JscFieldMap>> dictionary = new Dictionary<Guid, List<JscFieldMap>>();

            foreach (Row row in lvFieldMap.Rows)
            {
                JscFieldMap fieldMap = (JscFieldMap)row.Tag;
                List<JscFieldMap> items;
                if (!dictionary.TryGetValue((Guid) fieldMap.FromField, out items))
                {
                    items = new List<JscFieldMap>();
                    dictionary.Add((Guid)fieldMap.FromField, items);
                }
                items.Add(fieldMap);
            }

            return dictionary;
        }

        private Dictionary<string, JscFieldDesign> GetFieldDesignDictionary(Guid tableDesignId)
        {
            Dictionary<string, JscFieldDesign> dictionary = new Dictionary<string, JscFieldDesign>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var fieldDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesigns(PluginEntry.DataModel, tableDesignId))
            {
                dictionary.Add(fieldDesign.FieldName, fieldDesign);
            }
            return dictionary;
        }



        private void lvFieldMap_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvFieldMap.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEditFieldMap_Click));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAddFieldMap_Click));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemoveFieldMap_Click));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DatabaseMapViewMenu", lvFieldMap.ContextMenuStrip, lvFieldMap);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvFieldMap_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void lvFieldMap_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnEditFieldMap_Click(this, EventArgs.Empty);
            }
        }



    }
}
