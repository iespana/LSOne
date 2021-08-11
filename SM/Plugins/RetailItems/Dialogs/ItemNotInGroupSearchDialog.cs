using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class ItemNotInGroupSearchDialog : DialogBase
    {
        public enum GroupEnum
        {
            RetailGroup,
            SpecialGroup
        }

        List<RecordIdentifier> items;

        IConnectionManager connection;
        IApplicationCallbacks callbacks = null;

        int maxNumberOfRecords = 500;
        string groupId;
        GroupEnum groupEnum;

        public ItemNotInGroupSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks, RecordIdentifier groupId, GroupEnum groupEnum)
            :this(connection, callbacks)
        {
            this.groupId = (string)groupId;
            this.groupEnum = groupEnum;

            if (groupEnum == GroupEnum.SpecialGroup)
            {
                // ONE-7457: Remove the column Current group because it is always empty
                lvItemsList.Columns.Remove(column3);
            }

            Search();
        }

        public ItemNotInGroupSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks)
            :this()
        {
            this.connection = connection;
            this.callbacks = callbacks;
        }

        public ItemNotInGroupSearchDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lvItemsList.SetSortColumn(1, true);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return callbacks;
        }

        public List<RecordIdentifier> GetItems
        {
            get { return items; }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SetSelectedRecords();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SetSelectedRecords()
        {
            items = new List<RecordIdentifier>();

            for(int i = 0; i < lvItemsList.Selection.Count; i++)
            {
                items.Add((RecordIdentifier)lvItemsList.Selection[i].Tag);
            }
        }

        private void Search()
        {
            lvItemsList.ClearRows();
            List<ItemInGroup> items = new List<ItemInGroup>();

            string searchText = tbSearch.Text;

            switch (groupEnum)
            {
                case GroupEnum.RetailGroup:
                    items = Providers.RetailGroupData.ItemsNotInRetailGroup(
                        connection, 
                        searchText, 
                        maxNumberOfRecords + 1 ,
                        groupId);
                    break;
                case GroupEnum.SpecialGroup:
                    items = Providers.SpecialGroupData.ItemsNotInSpecialGroup(
                        connection,
                        groupId,
                        searchText,
                        maxNumberOfRecords + 1);
                    break;
            }

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
                var row = new Row();

                row.AddText((string)item.ID);
                row.AddText(item.Text);
                if (groupEnum != GroupEnum.SpecialGroup)
                {
                    row.AddText(item.Group);
                }
                row.AddText(item.VariantName);

                row.Tag = item.ID;

                lvItemsList.AddRow(row);
            }

            lvItemsList_SelectionChanged(this, EventArgs.Empty);

            lvItemsList.AutoSizeColumns();
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

            lvItemsList_SelectionChanged(this, EventArgs.Empty);
        }

        private void lvItemsList_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (lvItemsList.Selection.Count > 0)
            {
                btnSelect_Click(this, EventArgs.Empty);
            }
        }

        private void lvItemsList_SelectionChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = (lvItemsList.Selection.Count > 0);
        }
    }
}