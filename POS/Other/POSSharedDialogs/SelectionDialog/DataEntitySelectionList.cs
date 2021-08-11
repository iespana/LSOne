using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using System.Collections.Generic;

namespace LSOne.Controls.Dialogs.SelectionDialog
{
    /// <summary>
    /// DataEntity selection list for the <see cref="SelectionDialog"/>
    /// </summary>
    public class DataEntitySelectionList : ISelectionListView
    {
        private List<DataEntity> data;
        private DataEntity selectedItem;

        public DataEntitySelectionList(List<DataEntity> data, DataEntity selectedItem = null)
        {
            this.data = data;
            this.selectedItem = selectedItem;
        }

        public void LoadData(ListView listView, string filter = "")
        {
            listView.ClearRows();

            List<DataEntity> data = filter == "" ? this.data : this.data.FindAll(f => f.Text.ToLower().Contains(filter.ToLower()));

            Row row;
            foreach (DataEntity dataEntity in data)
            {
                row = new Row();
                row.AddText(dataEntity.Text);
                row.Tag = dataEntity;
                listView.AddRow(row);

                if(selectedItem != null && selectedItem.ID == dataEntity.ID)
                {
                    listView.Selection.Set(listView.RowCount - 1);
                    listView.ScrollRowIntoView(listView.RowCount - 1);
                }
            }
        }

        public void SetupListViewHeader(ListView listView)
        {
            listView.Columns.Clear();
            listView.Columns.Add(new Columns.Column(Properties.Resources.Selection, 700, 10, 0, 100, true));
        }
    }
}
