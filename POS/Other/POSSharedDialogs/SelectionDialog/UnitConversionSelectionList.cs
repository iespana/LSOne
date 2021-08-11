using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Units;
using System.Collections.Generic;

namespace LSOne.Controls.Dialogs.SelectionDialog
{
    /// <summary>
    /// Unit conversion selection list for the <see cref="SelectionDialog"/>
    /// </summary>
    public class UnitConversionSelectionList : ISelectionListView
    {
        private List<UnitConversion> data;
        private UnitConversion selectedItem;

        public UnitConversionSelectionList(List<UnitConversion> data, UnitConversion selectedItem = null)
        {
            this.data = data;
            this.selectedItem = selectedItem;
        }

        public void LoadData(ListView listView, string filter = "")
        {
            listView.ClearRows();

            List<UnitConversion> data = filter == "" ? this.data : this.data.FindAll(f => f.ToUnitName.ToLower().Contains(filter.ToLower()));

            Row row;
            foreach (UnitConversion unit in data)
            {
                row = new Row();
                row.AddText(unit.ToUnitName);
                row.AddText(Services.Interfaces.Services.RoundingService(DLLEntry.DataModel).RoundString(DLLEntry.DataModel, unit.UnitPrice, DLLEntry.Settings.Store.Currency, true));
                row.Tag = unit;
                listView.AddRow(row);

                if(selectedItem != null && selectedItem.ID == unit.ID)
                {
                    listView.Selection.Set(listView.RowCount - 1);
                    listView.ScrollRowIntoView(listView.RowCount - 1);
                }
            }
        }

        public void SetupListViewHeader(ListView listView)
        {
            listView.Columns.Clear();
            listView.Columns.Add(new Columns.Column(Properties.Resources.Name, 350, 10, 0, 50, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.UnitPrice, 350, 10, 0, 50, true));
        }
    }
}
