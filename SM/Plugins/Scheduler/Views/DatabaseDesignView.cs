using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSOne.ViewPlugins.Scheduler.ViewPages;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class DatabaseDesignView : ViewBase
    {

        private DatabaseDesignPage databaseDesignPage;
        private LinkedFilterPage linkedFilterPage;
        private TableDesignPage tableDesignPage;
        private IDetailView activePage;
        private NewLinkedTableDialog newLinkedTableDialog = new NewLinkedTableDialog();
        private bool inChangingFilter;
        private ContextBarItem readFieldDesignItem;

        private ContextBarItem readDesignItem;

        public DatabaseDesignView()
        {
            InitializeComponent();
            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            databaseDesignTree.OpeningContextMenuStrip += databaseDesignTree_OpeningContextMenuStrip;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.DatabaseDesign;
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
            HeaderText = LogicalContextName;
            
            if (isRevert)
            {
                if (activePage != null)
                {
                    activePage.Hide();
                    activePage = null;
                }
            }

            ReadOnly = !PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignEdit);

            databaseDesignTree.LoadData();
            UpdateActions();
        }

        private void databaseDesignTree_SelectedItemChanging(object sender, CancelEventArgs e)
        {
            e.Cancel = !CheckForSave();
        }

        private bool CheckForSave()
        {
            bool result = true;

            if (activePage != null)
            {
                if (activePage.DataIsModified())
                {
                    if (!activePage.SaveData())
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        private void databaseDesignTree_SelectedItemChanged(object sender, EventArgs e)
        {
            var newPage = SelectPage(databaseDesignTree.SelectedItem);

            bool isDifferentPage = activePage != newPage;

            if (activePage != null && isDifferentPage)
            {
                activePage.Hide();
            }

            activePage = newPage;
            if (activePage != null && isDifferentPage)
            {
                activePage.Show();
            }

            UpdateActions();
        }

        private void UpdateActions()
        {
            bool hasPermission = PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignEdit);
            var item = databaseDesignTree.SelectedItem;

            btnsAddRemove.AddButtonEnabled =
                hasPermission &&
                item != null &&
                (item.ItemType == DatabaseDesignTree.ItemType.TableDesign || item.ItemType == DatabaseDesignTree.ItemType.LinkedTable);

            btnsAddRemove.RemoveButtonEnabled =
                hasPermission &&
                item != null &&
                item.ItemType == DatabaseDesignTree.ItemType.LinkedTable;
            
            if (IsLoaded)
            {
                // Rebuild the context bar to ensure read table fields is visible only when database table is visible
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
        }


        private IDetailView SelectPage(DatabaseDesignTree.Item item)
        {
            IDetailView result = null;

            switch (item.ItemType)
            {
                case DatabaseDesignTree.ItemType.DatabaseDesign:
                    result = GetDatabaseDesignPage((JscDatabaseDesign)item.Object);
                    break;
                case DatabaseDesignTree.ItemType.TableDesign:
                    result = GetTableDesignPage((JscTableDesign)item.Object);
                    break;
                case DatabaseDesignTree.ItemType.LinkedTable:
                    result = GetLinkedTablePage((JscLinkedTable)item.Object);
                    break;
            }
            
            return result;
        }

        private IDetailView GetDatabaseDesignPage(JscDatabaseDesign databaseDesign)
        {
            if (databaseDesignPage == null)
            {
                databaseDesignPage = new DatabaseDesignPage();
                databaseDesignPage.Visible = false;
                databaseDesignPage.Dock = DockStyle.Fill;
                splMain.Panel2.Controls.Add(databaseDesignPage);
            }
            
            databaseDesignPage.LoadData(false, 0, new DatabaseDesignPage.InternalContext { DatabaseDesign = databaseDesign });

            return databaseDesignPage;
        }


        private IDetailView GetLinkedTablePage(JscLinkedTable linkedTable)
        {
            if (linkedFilterPage == null)
            {
                linkedFilterPage = new LinkedFilterPage();
                linkedFilterPage.Visible = false;
                linkedFilterPage.Dock = DockStyle.Fill;
                splMain.Panel2.Controls.Add(linkedFilterPage);
            }

            linkedFilterPage.LoadData(false, 0, new LinkedFilterPage.InternalContext { LinkedTable = linkedTable });

            return linkedFilterPage;
        }


        private IDetailView GetTableDesignPage(JscTableDesign tableDesign)
        {
            if (tableDesignPage == null)
            {
                tableDesignPage = new TableDesignPage();
                tableDesignPage.Visible = false;
                tableDesignPage.Dock = DockStyle.Fill;
                splMain.Panel2.Controls.Add(tableDesignPage);
            }

            tableDesignPage.LoadData(false, 0, new TableDesignPage.InternalContext {  TableDesign = tableDesign });
            return tableDesignPage;
        }


        protected override bool DataIsModified()
        {
            bool result = false;

            if (activePage != null)
            {
                result = activePage.DataIsModified();
            }

            return result;
        }

        protected override bool SaveData()
        {
            bool result = true;

            if (activePage != null)
            {
                result = activePage.SaveData();
            }

            return result;
        }


        private void databaseDesignTree_OpeningContextMenuStrip(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu = databaseDesignTree.ContextMenuStrip;

            item = new ExtendedMenuItem(
                    Properties.Resources.LinkedTableAdd,
                    200,
                    btnAdd_Click);

            item.Enabled = btnsAddRemove.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.LinkedTableRemove,
                    300,
                    btnRemove_Click);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DatabaseMapListViewMenu", databaseDesignTree.ContextMenuStrip, databaseDesignTree);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            JscTableDesign tableFrom = databaseDesignTree.GetTableDesignOfSelection();
            Debug.Assert(tableFrom != null);

            if (newLinkedTableDialog.ShowDialog(PluginEntry.Framework.MainWindow, tableFrom) == DialogResult.OK)
            {
                JscLinkedTable linkedTable = new JscLinkedTable();
                linkedTable.FromJscTableDesign = tableFrom;
                linkedTable.ToJscTableDesign = newLinkedTableDialog.SelectedTableDesign;
                DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(PluginEntry.DataModel, linkedTable);
                databaseDesignTree.Add(linkedTable);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var selectedItem = databaseDesignTree.SelectedItem;
            if (selectedItem == null || selectedItem.ItemType != DatabaseDesignTree.ItemType.LinkedTable)
                return;

            JscLinkedTable linkedTable = (JscLinkedTable)selectedItem.Object;

            string msg = string.Format(Properties.Resources.LinkedTableRemoveMsg, linkedTable.ToJscTableDesign.TableName);
            if (QuestionDialog.Show(msg, Properties.Resources.LinkedTableRemoveHeader) == DialogResult.Yes)
            {
                linkedTable.FromJscTableDesign = null;
                linkedTable.ToJscTableDesign = null;
                DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().DeleteLinkedTable(PluginEntry.DataModel, linkedTable);
                databaseDesignTree.DeleteItem(selectedItem);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "LinkedTable", linkedTable.ID, null);
            }
        }

        private void chkFilterOnlyLinked_CheckedChanged(object sender, EventArgs e)
        {
            if (!inChangingFilter)
            {
                inChangingFilter = true;
                if (CheckForSave())
                {
                    databaseDesignTree.FilterOnlyLinked = chkFilterOnlyLinked.Checked;
                }
                else
                {
                    chkFilterOnlyLinked.Checked = !chkFilterOnlyLinked.Checked;
                }
                inChangingFilter = false;
            }
        }

        private void chkIncludeDisabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!inChangingFilter)
            {
                inChangingFilter = true;
                if (CheckForSave())
                {
                    databaseDesignTree.FilterIncludeDisabled = chkIncludeDisabled.Checked;
                }
                else
                {
                    chkFilterOnlyLinked.Checked = !chkIncludeDisabled.Checked;
                }
                inChangingFilter = false;
            }
        }


        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".View" && tableDesignPage != null &&
                tableDesignPage.Visible)
            {
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignEdit))
                {
                    readFieldDesignItem = new ContextBarItem(Properties.Resources.FieldDesignRead,
                        new ContextbarClickEventHandler(ReadFieldDesign));
                    arguments.Add(readFieldDesignItem, 300);
                }
                
            }
            if (arguments.CategoryKey == base.GetType().ToString() + ".View" && PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit) && PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignEdit))
                {
                    readDesignItem = new ContextBarItem(Properties.Resources.LocationListViewReadDesign, ReadDesignHandler);
                    arguments.Add(readDesignItem, 300);
                }
            base.OnSetupContextBarItems(arguments);
        }

        private void ReadDesignHandler(object sender, EventArgs e)
        {

            Dialogs.DataDirectorDialog dataDirectorDialog = new Dialogs.DataDirectorDialog();

            // Determine if we allow the user to update or merge new data into existing design or if we
            // simply create a new one.


            // Show a dialog prompting for read options
            bool readTablesAndFields;
            bool updateExisting;
            string newDescription = null;

            JscLocation location = null;
            object tag = null;
            bool updateDesign = databaseDesignPage != null && databaseDesignPage.DatabaseDesign != null;
            if (databaseDesignPage != null && databaseDesignPage.LVLocations.Selection.Count == 1)
            {


                DialogResult result = MessageBox.Show(
                    "Do you want to use the selected Location?",
                    "Use Location", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel)
                {
                    return;
                }
                if (result == DialogResult.Yes)
                {
                    tag =
                        databaseDesignPage.LVLocations.Rows[databaseDesignPage.LVLocations.Selection.GetRowIndex(0)]
                            .Tag;
                }

                if (tag is JscLocation)
                {
                    location = tag as JscLocation;

                }
            }

            if (location == null)
            {
                var locations = DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                    .GetLocations(PluginEntry.DataModel, true)
                    .ToList();
                if (locations.Count > 0)
                {
                    LocationSelectorDialog locationSelector = new LocationSelectorDialog(locations);
                    if (locationSelector.ShowDialog() == DialogResult.OK)
                    {
                        location = locationSelector.LocationItem;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (
                        MessageBox.Show("No loctions present.  Please create a location before reading.  Do you want to create one?", "Create Location",
                            MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        PluginOperations.ShowLocationsListView(null, null);
                        
                    }
                    return;
                }
            }



            using (Dialogs.ReadDesignDialog readDesignDialog = new Dialogs.ReadDesignDialog())
            {

                if (readDesignDialog.ShowDialog(PluginEntry.Framework.MainWindow, updateDesign, string.Empty) ==
                    DialogResult.Cancel)
                {
                    return;
                }
                readTablesAndFields = readDesignDialog.ReadTablesAndFields;
                updateExisting = readDesignDialog.UpdateExistingDatabaseDesign;
                if (!updateExisting)
                {
                    newDescription = readDesignDialog.NewDescription;
                }
            }

            if (readTablesAndFields)
                dataDirectorDialog.ReadTablesAndFieldsDesign(this, location, updateExisting,
                    newDescription, databaseDesignPage != null? databaseDesignPage.DatabaseDesign.ID: null);
            else
                dataDirectorDialog.ReadTablesDesign(this, location, updateExisting, newDescription, databaseDesignPage != null ? databaseDesignPage.DatabaseDesign.ID : null);

            LoadData(false);

        }


        private void ReadFieldDesign(object sender, ContextBarClickEventArguments args)
        {

            TableDesignPage tableDesignPage = activePage as TableDesignPage;
            if (tableDesignPage != null)
            {
                tableDesignPage.ReadFieldDesign();
            }
        }


        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            base.OnDataChanged(changeAction, objectName, changeIdentifier, param);

            if (objectName == "DatabaseDesign" && (changeAction == DataEntityChangeType.Edit || changeAction == DataEntityChangeType.Enable || changeAction == DataEntityChangeType.Disable))
            {
                databaseDesignTree.RefreshDatabaseDesign((Guid)changeIdentifier);
            }
        }
    }
}
