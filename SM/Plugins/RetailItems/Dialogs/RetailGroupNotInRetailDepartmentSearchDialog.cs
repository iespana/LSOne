using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class RetailGroupNotInRetailDepartmentSearchDialog : DialogBase
    {

        RecordIdentifier retailDepartmentID;
        List<RetailGroup> retailGroups;

        IConnectionManager connection;
        IApplicationCallbacks callbacks = null;

        public RetailGroupNotInRetailDepartmentSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks, RecordIdentifier retailDepartmentID)
            :this(connection, callbacks)
        {
            this.retailDepartmentID = retailDepartmentID;
            Search(this, EventArgs.Empty);
        }


        public RetailGroupNotInRetailDepartmentSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks)
            :this()
        {
            this.connection = connection;
            this.callbacks = callbacks;
            
        }

        public RetailGroupNotInRetailDepartmentSearchDialog()
        {
            InitializeComponent();
            
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return callbacks;
        }

        public List<RetailGroup> RetailGroups
        {
            get { return retailGroups; }
        }

      
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SetSelectedRetailGroups();
        
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SetSelectedRetailGroups()
        {
            retailGroups = new List<RetailGroup>();

            for(int i = 0; i < lvGroups.Selection.Count; i++)
            {
                retailGroups.Add((RetailGroup)lvGroups.Selection[i].Tag);
            }
        }

        private void Search(object sender, EventArgs e)
        {
            lvGroups.ClearRows();
            RetailGroupSorting?[] columns = { RetailGroupSorting.RetailGroupId, RetailGroupSorting.RetailGroupName, RetailGroupSorting.RetailDepartmentName};

            if (lvGroups.SortColumn == null)
            {
                lvGroups.SetSortColumn(lvGroups.Columns[1], true);
            }

            int sortColumnIndex = lvGroups.Columns.IndexOf(lvGroups.SortColumn);

           
            string searchText = tbSearch.Text;

            List<RetailGroup> retailGroups = Providers.RetailGroupData.GetRetailGroupsNotInRetailDepartment(
                connection,
                retailDepartmentID,
                searchText,
                 (RetailGroupSorting)((int)columns[sortColumnIndex]),
                !lvGroups.SortedAscending);

            foreach (var retailGroup in retailGroups)
            {
              
                Row row = new Row();
                

                row.AddText((string)retailGroup.ID);
                row.AddText(retailGroup.Text);
                row.AddText(retailGroup.RetailDepartmentName);
                row.Tag = retailGroup;

                lvGroups.AddRow(row);
            }


            lvGroups_SelectionChanged(this, EventArgs.Empty);

            lvGroups.AutoSizeColumns(true);
        }

   
        private void lvGroups_SelectionChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = (lvGroups.Selection.Count > 0);
        }

        private void lvGroups_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (lvGroups.Selection.Count > 0)
            {
                btnSelect_Click(this, EventArgs.Empty);
            }
        }

        private void lvGroups_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvGroups.SortColumn == args.Column)
            {
                lvGroups.SetSortColumn(args.Column, !lvGroups.SortedAscending);
            }
            else
            {
                lvGroups.SetSortColumn(args.Column, true);
            }
            Search(this, EventArgs.Empty);
        }
    }
}
