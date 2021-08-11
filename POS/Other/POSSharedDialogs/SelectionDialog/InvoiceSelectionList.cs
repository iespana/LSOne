using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Invoice;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.Controls.Dialogs.SelectionDialog
{
    /// <summary>
    /// Invoice selection list for the <see cref="SelectionDialog"/>
    /// </summary>
    public class InvoiceSelectionList : ISelectionListView
    {
        private List<Invoice> data;
        private Invoice selectedItem;

        public InvoiceSelectionList(List<Invoice> data, Invoice selectedItem = null)
        {
            this.data = data;
            this.selectedItem = selectedItem;
        }

        public void LoadData(ListView listView, string filter = "")
        {
            listView.ClearRows();
            Currency currency;
            DecimalLimit limit;

            List<Invoice> data = filter == "" ? this.data : this.data.FindAll(f => f.ID.StringValue.ToLower().Contains(filter.ToLower()));

            Row row;
            foreach (Invoice Invoice in data)
            {
                row = new Row();
                currency = Providers.CurrencyData.Get(DLLEntry.DataModel, Invoice.Currency, CacheType.CacheTypeTransactionLifeTime);
                limit = new DecimalLimit(currency.RoundOffSales.DigitsBeforeFirstSignificantDigit());
                row.AddText((string)Invoice.ID);
                row.AddText(Invoice.Created.ToString(DLLEntry.Settings.CultureInfo.DateTimeFormat));
                row.AddText((currency.CurrencyPrefix +  " " + Invoice.Paid.FormatWithLimits(limit) + " " + currency.CurrencySuffix).Trim());
                row.AddText((currency.CurrencyPrefix + " " + Invoice.Total.FormatWithLimits(limit) + " " + currency.CurrencySuffix).Trim());
                row.AddText((currency.CurrencyPrefix + " " + Invoice.Balance.FormatWithLimits(limit) + " " + currency.CurrencySuffix).Trim());
                row.Tag = Invoice;
                listView.AddRow(row);

                if(selectedItem != null && selectedItem.ID == Invoice.ID)
                {
                    listView.Selection.Set(listView.RowCount - 1);
                    listView.ScrollRowIntoView(listView.RowCount - 1);
                }
            }
        }

        public void SetupListViewHeader(ListView listView)
        {
            listView.Columns.Clear();
            listView.Columns.Add(new Columns.Column(Properties.Resources.SalesInvoice, 140, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.Created, 140, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.Paid, 140, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.Total, 140, 10, 0, 21, true));
            listView.Columns.Add(new Columns.Column(Properties.Resources.Balance, 140, 10, 0, 21, true));
        }
    }
}
