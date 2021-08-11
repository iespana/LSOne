using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.Controls.Dialogs.SelectionDialog
{
    /// <summary>
    /// Infocode selection list for the <see cref="SelectionDialog"/>
    /// </summary>
    public class InfocodeSelectionList : ISelectionListView
    {
        private List<InfocodeSubcode> data;
        private InfocodeSubcode selectedItem;
        private bool hasPriceColumn;

        public InfocodeSelectionList(List<InfocodeSubcode> data, InfocodeSubcode selectedItem = null)
        {
            this.data = data;
            this.selectedItem = selectedItem;
        }

        public void LoadData(ListView listView, string filter = "")
        {
            listView.ClearRows();

            List<InfocodeSubcode> data = filter == "" ? this.data : this.data.FindAll(f => f.Text.ToLower().Contains(filter.ToLower()));

            Currency currencyInfo = Providers.CurrencyData.Get(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);

            Row row;
            foreach (InfocodeSubcode subcode in data)
            {
                row = new Row();
                row.AddText(subcode.Text);

                if(hasPriceColumn)
                {
                    switch (subcode.PriceType)
                    {
                        case PriceTypes.None:
                            row.AddText("");
                            break;
                        case PriceTypes.FromItem:
                            row.AddText(Properties.Resources.NormalPrice);
                            break;
                        case PriceTypes.Percent:
                            row.AddText(subcode.AmountPercent.FormatWithLimits(DLLEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent)) + " %");
                            break;
                        case PriceTypes.Price:
                            row.AddText(currencyInfo.CurrencyPrefix + " " + subcode.AmountPercent.FormatWithLimits(new DecimalLimit(currencyInfo.RoundOffSales.DigitsBeforeFirstSignificantDigit())) + " " + currencyInfo.CurrencySuffix);
                            break;
                    }
                }

                row.Tag = subcode;
                listView.AddRow(row);

                if(selectedItem != null && selectedItem.ID == subcode.ID)
                {
                    listView.Selection.Set(listView.RowCount - 1);
                    listView.ScrollRowIntoView(listView.RowCount - 1);
                }
            }
        }

        public void SetupListViewHeader(ListView listView)
        {
            listView.Columns.Clear();
            listView.Columns.Add(new Columns.Column(Properties.Resources.Description, 375, 10, 0, 50, true));

            if(data.Any(x => x.PriceType != PriceTypes.None))
            {
                hasPriceColumn = true;
                listView.Columns.Add(new Columns.Column(Properties.Resources.Price, 375, 10, 0, 50, true));
            }
        }
    }
}
