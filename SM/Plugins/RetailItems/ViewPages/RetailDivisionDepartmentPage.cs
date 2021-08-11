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
using LSOne.ViewPlugins.RetailItems.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using System.Linq;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.RetailItems.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    public partial class RetailDivisionDepartmentPage : UserControl, ITabView
    {
        private RecordIdentifier selectedID = "";
        private List<DataEntity> items;
        private RetailDivision retailDivision;
        private WeakReference owner;

        public RetailDivisionDepartmentPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public RetailDivisionDepartmentPage()
            : base()
        {
            InitializeComponent();

            lvDepList.ContextMenuStrip = new ContextMenuStrip();
            lvDepList.ContextMenuStrip.Opening += lvDepList_Opening;

            depDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            depDataScroll.Reset();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.RetailDivisionDepartmentPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RetailDivisions", 0, Properties.Resources.RetailGroups, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            selectedID = context;
            retailDivision = (RetailDivision)internalContext;
            LoadLines();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RetailDepartment":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadLines();
                    break;
            }
        }

        private void LoadLines()
        {
            lvDepList.Items.Clear();

            items = Providers.RetailDepartmentData.Search(PluginEntry.DataModel,
                                                "",
                                                retailDivision.ID,
                                                depDataScroll.StartRecord,
                                                depDataScroll.EndRecord + 1,
                                                false,
                                                RetailDepartment.SortEnum.Description);

            depDataScroll.RefreshState(items);

            foreach (var item in items)
            {
                var listItem = new ListViewItem((string)item.ID);

                listItem.SubItems.Add(item.Text);

                listItem.Tag = item.ID;
                listItem.ImageIndex = -1;

                lvDepList.Add(listItem);
            }

            lvDepList_SelectedIndexChanged(this, EventArgs.Empty);

            lvDepList.BestFitColumns();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        void lvDepList_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvDepList.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here

            ExtendedMenuItem item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    btnAddDepartment_Click)
                {
                    Enabled = btnsContextButtonsItems.AddButtonEnabled,
                    Image = ContextButtons.GetAddButtonImage()
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btnRemoveDepartment_Click)
                {
                    Enabled = btnsContextButtonsItems.RemoveButtonEnabled,
                    Image = ContextButtons.GetRemoveButtonImage()
                };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DepartmentsInDivisionList", lvDepList.ContextMenuStrip, lvDepList);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvDepList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtonsItems.RemoveButtonEnabled = btnsContextButtonsItems.EditButtonEnabled =
                (lvDepList.SelectedItems.Count > 0) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void btnRemoveDepartment_Click(object sender, EventArgs e)
        {
            Providers.RetailDepartmentData.AddOrRemoveRetailDepartmentFromRetailDivision(PluginEntry.DataModel, (RecordIdentifier)lvDepList.SelectedItems[0].Tag, null);
            LoadLines();
        }

        private void btnAddDepartment_Click(object sender, EventArgs e)
        {

            string divisionId = (string)selectedID;
            var dlg = new DepartmentNotInDivisionSearchDialog(PluginEntry.DataModel, PluginEntry.Framework, divisionId);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                List<RetailDepartment> departments = dlg.GetDepartments;

                if(departments.Any(x => x.RetailDivisionID.StringValue != ""))
                {
                    if (QuestionDialog.Show(Resources.RetailDepartmentAssignedToDivision + "\n" +
                                            Resources.DoYouWantToContinue) == DialogResult.Yes)
                    {
                        Providers.RetailDepartmentData.AddOrRemoveRetailDepartmentsFromRetailDivision(
                            PluginEntry.DataModel, departments.Select(x => x.ID).ToList(), divisionId);
                    }
                }
                else
                {
                    Providers.RetailDepartmentData.AddOrRemoveRetailDepartmentsFromRetailDivision(
                        PluginEntry.DataModel, departments.Select(x => x.ID).ToList(), divisionId);
                }
                
                LoadLines();
            }
        }

        public void OnClose()
        {
            lvDepList.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }
    }
}
