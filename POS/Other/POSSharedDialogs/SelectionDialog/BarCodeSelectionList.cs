using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.Controls.Dialogs.SelectionDialog
{
    /// <summary>
    /// BarCode selection list for the <see cref="SelectionDialog"/>
    /// </summary>
    public class BarCodeSelectionList : ISelectionListView
    {
        private List<BarCode> data;
        private BarCode selectedItem;
        private bool loadItems;

        public BarCodeSelectionList(List<BarCode> data, BarCode selectedItem = null, bool loadItems = false)
        {
            this.data = data;
            this.selectedItem = selectedItem;
            this.loadItems = loadItems;
        }

        public void LoadData(ListView listView, string filter = "")
        {
            listView.ClearRows();

            List<BarCode> data = filter == "" ? this.data : this.data.FindAll(f => f.Text.ToLower().Contains(filter.ToLower()));

            Row row;
            foreach (BarCode barcode in data)
            {
                row = new Row();
                if (loadItems)
                {
                    row.AddText(barcode.ItemID.StringValue);
                    row.AddText(barcode.ItemName);
                    row.Tag = barcode;
                }
                else
                {
                    row.AddText(barcode.ItemBarCode.StringValue);
                    row.AddText(DecimalExtensions.FormatWithLimits(barcode.Quantity, Providers.UnitData.GetNumberLimitForUnit(DLLEntry.DataModel, barcode.UnitID)));
                    row.AddText(barcode.UnitDescription);
                    row.Tag = barcode;
                }
                listView.AddRow(row);

                if (selectedItem != null && selectedItem.ID == barcode.ID)
                {
                    listView.Selection.Set(listView.RowCount - 1);
                    listView.ScrollRowIntoView(listView.RowCount - 1);
                }
            }
        }

        public void SetupListViewHeader(ListView listView)
        {
            listView.Columns.Clear();

            if (this.loadItems)
            {
                listView.Columns.Add(new Columns.Column(Properties.Resources.Item, 350, 10, 0, 50, true));
                listView.Columns.Add(new Columns.Column(Properties.Resources.Description, 100, 10, 0, 20, true) { DefaultHorizontalAlignment = Columns.Column.HorizontalAlignmentEnum.Right });
                
            }
            else
            {
                listView.Columns.Add(new Columns.Column(Properties.Resources.BarcodeHeader, 350, 10, 0, 50, true));
                listView.Columns.Add(new Columns.Column(Properties.Resources.QuantityHeader, 100, 10, 0, 20, true) { DefaultHorizontalAlignment = Columns.Column.HorizontalAlignmentEnum.Right });
                listView.Columns.Add(new Columns.Column(Properties.Resources.Unit, 250, 10, 0, 30, true));
            }
        }
    }
}
