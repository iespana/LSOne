using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System.Collections.Generic;
using LSOne.Controls.Rows;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class DepartmentNotInDivisionSearchDialog : DialogBase
    {
        private List<RetailDepartment> departments;

        private IConnectionManager connection;
        private IApplicationCallbacks callbacks;

        private const int maxNumberOfRecords = 500;
        private string divisionId;

        public DepartmentNotInDivisionSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks, RecordIdentifier groupId)
            :this(connection, callbacks)
        {
            this.divisionId = (string)groupId;

            Search();
        }

        public DepartmentNotInDivisionSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks)
            :this()
        {
            this.connection = connection;
            this.callbacks = callbacks;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        public DepartmentNotInDivisionSearchDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            lvDepartments.SetSortColumn(0, true);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return callbacks;
        }

        public List<RetailDepartment> GetDepartments
        {
            get { return departments; }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SetSelectedDepartments();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SetSelectedDepartments()
        {
            departments = new List<RetailDepartment>();

            for(int i = 0; i < lvDepartments.Selection.Count; i++)
            {
                departments.Add((RetailDepartment)lvDepartments.Selection[i].Tag);
            }
        }

        private void Search()
        {
            lvDepartments.ClearRows();

            string searchText = tbSearch.Text;

            var items = Providers.RetailDepartmentData.DepartmentsNotInRetailDivision(
                connection, 
                searchText, 
                maxNumberOfRecords + 1 ,
                divisionId);

            // We try to get maxNumberOfRecords + 1 in order to see if there are more items
            if (items.Count > maxNumberOfRecords)
            {
                // Remove the last item
                items.RemoveAt(items.Count - 1);

                lblToManyError.Visible = true;
                errorProvider1.SetIconAlignment(lblToManyError, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError(lblToManyError, lblToManyError.Text);
            }

            foreach (var item in items)
            {
                Row row = new Row();
                row.AddText(item.ID.ToString());
                row.AddText(item.Text);
                row.AddText(item.RetailDivisionName);
                row.Tag = item;

                lvDepartments.AddRow(row);
            }

            lvDepartments.AutoSizeColumns();
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    if (btnSearch.Enabled)
            //    {
            //        btnSearch_Click(this, EventArgs.Empty);
            //    }
            //    e.Handled = true;
            //}

            lvDepartments_SelectionChanged(this, EventArgs.Empty);
        }

        private void lvDepartments_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (lvDepartments.Selection.Count > 0)
            {
                btnSelect_Click(this, EventArgs.Empty);
            }
        }

        private void lvDepartments_SelectionChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = (lvDepartments.Selection.Count > 0);
        }
    }
}
