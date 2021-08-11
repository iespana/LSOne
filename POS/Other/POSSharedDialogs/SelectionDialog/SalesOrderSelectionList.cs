using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.Controls.Dialogs.SelectionDialog
{
    /// <summary>
    /// Sales order selection list for the <see cref="SelectionDialog"/>
    /// </summary>
    public class SalesOrderSelectionList : ISelectionListView
    {
        private List<SalesOrder> data;
        private SalesOrder selectedItem;

        public SalesOrderSelectionList(List<SalesOrder> data, SalesOrder selectedItem = null)
        {
            this.data = data;
            this.selectedItem = selectedItem;
        }

        public void LoadData(ListView listView, string filter = "")
        {
            listView.ClearRows();
            Currency currency;
            DecimalLimit limit;

            List<SalesOrder> data = filter == "" ? this.data : this.data.FindAll(f => f.ID.StringValue.ToLower().Contains(filter.ToLower()));

            Row row;
            foreach (SalesOrder order in data)
            {
                row = new Row();
                currency = Providers.CurrencyData.Get(DLLEntry.DataModel, order.Currency, CacheType.CacheTypeTransactionLifeTime);
                limit = new DecimalLimit(currency.RoundOffSales.DigitsBeforeFirstSignificantDigit());
                row.AddText((string)order.ID);
                row.AddText(order.Created.ToString(DLLEntry.Settings.CultureInfo.DateTimeFormat));
                row.AddText((currency.CurrencyPrefix +  " " + order.Prepayment.FormatWithLimits(limit) + " " + currency.CurrencySuffix).Trim());
                row.AddText((currency.CurrencyPrefix + " " + order.Prepaid.FormatWithLimits(limit) + " " + currency.CurrencySuffix).Trim());
                row.AddText((currency.CurrencyPrefix + " " + order.Total.FormatWithLimits(limit) + " " + currency.CurrencySuffix).Trim());
                row.AddText((currency.CurrencyPrefix + " " + order.Balance.FormatWithLimits(limit) + " " + currency.CurrencySuffix).Trim());
                row.Tag = order;
                listView.AddRow(row);

                if(selectedItem != null && selectedItem.ID == order.ID)
                {
                    listView.Selection.Set(listView.RowCount - 1);
                    listView.ScrollRowIntoView(listView.RowCount - 1);
                }
            }
        }

        public void SetupListViewHeader(ListView listView)
        {
            listView.Columns.Clear();
            listView.Columns.Add(new Columns.Column(Properties.Resources.SalesOrder, 130, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.Created, 110, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.ForPrepayment, 170, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.Prepaid, 110, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.Total, 100, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.Balance, 110, 10, 0, 21, true));
        }
    }
}
