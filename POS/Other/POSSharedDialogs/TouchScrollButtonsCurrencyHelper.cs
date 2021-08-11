using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Controls.Dialogs
{
    public static class TouchScrollButtonsCurrencyHelper
    {
        public enum ViewOptions
        {
            None = 0,
            HigherAmounts = 1,
            LowerAmounts = 2,
            HeigherAndLowerAmounts = 3,
            ExactAmountOnly = 4
        }

        public static void SetCurrencyBills(this TouchScrollButtonPanel panel, RecordIdentifier currency, decimal? upperLimit = null)
        {
            panel.Clear();
            List<CashDenominator> cashDenominators = Providers.CashDenominatorData.GetBills(DLLEntry.DataModel, currency);
            IRoundingService rounding = Services.Interfaces.Services.RoundingService(DLLEntry.DataModel);
            foreach (CashDenominator cashDenominator in cashDenominators)
            {
                if (upperLimit != null && upperLimit != 0 && cashDenominator.Amount > upperLimit)
                {
                    break;
                }
                panel.AddButton(rounding.RoundString(DLLEntry.DataModel, cashDenominator.Amount, currency, true, CacheType.CacheTypeTransactionLifeTime), cashDenominator, "");
            }
        }

        public static void SetButtonsCurrency(this TouchScrollButtonPanel panel, decimal amount,
                                              StorePaymentMethod tenderInformation, RecordIdentifier currentCurrency, bool sortAsc = true,
                                              ViewOptions? viewOption = null, decimal? lowerLimit = null,
                                              decimal? upperLimit = null)
        {
            panel.Clear();
            IRoundingService rounding = (IRoundingService) DLLEntry.DataModel.Service(ServiceType.RoundingService);

            if (viewOption == null)
            {
                viewOption = tenderInformation.AllowOverTender
                                 ? ViewOptions.HigherAmounts
                                 : ViewOptions.ExactAmountOnly;
            }

            if (viewOption == ViewOptions.None) return;

            if (upperLimit == null)
            {
                upperLimit = tenderInformation.MaximumAmountAllowed;
            }

            if (lowerLimit == null)
            {
                lowerLimit = tenderInformation.MinimumAmountAllowed;
            }

            if (amount < 0)
            {
                panel.AddButton(rounding.RoundString(DLLEntry.DataModel, amount, currentCurrency, true, CacheType.CacheTypeTransactionLifeTime), amount, "");
            }
            else
            {
                bool refAmountInArray = false;

                if (viewOption != ViewOptions.ExactAmountOnly)
                {
                    List<CashDenominator> amounts = PopulateCurrencyDenominators(currentCurrency, viewOption, amount, lowerLimit, upperLimit, ref refAmountInArray, sortAsc);

                    foreach (CashDenominator current in amounts)
                    {
                        panel.AddButton(string.IsNullOrEmpty(current.Denomination) ? rounding.RoundString(DLLEntry.DataModel, current.Amount, currentCurrency, true, CacheType.CacheTypeTransactionLifeTime) : current.Denomination, current.Amount, "");
                    }
                }

                // at least the exact amount is added if no other is found.
                if (!refAmountInArray)
                {
                    if (AmountWithinLimits(amount, lowerLimit, upperLimit))
                    {
                        panel.AddButton(rounding.RoundString(DLLEntry.DataModel, amount, currentCurrency, true, CacheType.CacheTypeTransactionLifeTime), amount, "");
                    }
                    else
                    {
                        panel.AddButton(rounding.RoundString(DLLEntry.DataModel, (decimal)upperLimit, currentCurrency, true, CacheType.CacheTypeTransactionLifeTime), (decimal)upperLimit, "");
                    }
                }
            }
        }

        public static void SetButtonsLoyaltyPoints(this TouchScrollButtonPanel panel, decimal amount, decimal pointMultiplier, StorePaymentMethod tenderInformation, RecordIdentifier currentCurrency,
                                              decimal roundValue, int roundMethod, bool sortAsc = true, ViewOptions? viewOption = null, decimal? lowerLimit = null, decimal? upperLimit = null)
        {
            panel.Clear();

            if (pointMultiplier == decimal.Zero)
            {
                return;
            }

            IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

            if (viewOption == null)
            {
                viewOption = tenderInformation.AllowOverTender ? ViewOptions.HigherAmounts : ViewOptions.ExactAmountOnly;
            }

            if (viewOption == ViewOptions.None) return;

            if (upperLimit == null)
            {
                upperLimit = tenderInformation.MaximumAmountAllowed;
            }

            if (lowerLimit == null)
            {
                lowerLimit = tenderInformation.MinimumAmountAllowed;
            }

            if (amount < 0 && (amount * pointMultiplier) != 0)
            {
                panel.AddButton(rounding.RoundAmount(DLLEntry.DataModel, amount / pointMultiplier, roundValue, (TenderRoundMethod)roundMethod, false, currentCurrency, CacheType.CacheTypeTransactionLifeTime), amount, "");
            }
            else
            {
                bool refAmountInArray = false;

                if (viewOption != ViewOptions.ExactAmountOnly)
                {
                    List<CashDenominator> amounts = PopulateCurrencyDenominators(currentCurrency, viewOption, amount, lowerLimit, upperLimit, ref refAmountInArray, sortAsc);

                    foreach (CashDenominator current in amounts)
                    {
                        decimal pointsValue = rounding.RoundAmount(DLLEntry.DataModel, current.Amount * pointMultiplier, roundValue, (TenderRoundMethod)roundMethod, CacheType.CacheTypeTransactionLifeTime);
                        decimal value = pointsValue / pointMultiplier;

                        string strAmt = pointsValue.ToString("N0");

                        if ((strAmt != "0" && strAmt != "0,00" && strAmt != "0.00") && ((current.Amount * pointMultiplier) >= pointMultiplier))
                        {
                            panel.AddButton(strAmt, value, "");
                        }
                    }
                }

                // at least the exact amount is added if no other is found.
                if (!refAmountInArray)
                {
                    if (AmountWithinLimits(amount, lowerLimit, upperLimit) && (amount / pointMultiplier) != 0)
                    {
                        panel.AddButton(rounding.RoundAmount(DLLEntry.DataModel, amount / pointMultiplier, roundValue, (TenderRoundMethod)roundMethod, false, currentCurrency, CacheType.CacheTypeTransactionLifeTime), amount, "");
                    }
                }
            }
        }

        private static List<CashDenominator> PopulateCurrencyDenominators(RecordIdentifier currentCurrency, ViewOptions? viewOption, decimal amount, decimal? lowerLimit, decimal? upperLimit, ref bool refAmountInArray, bool sortAsc)
        {
            List<CashDenominator> denominators = new List<CashDenominator>();

            refAmountInArray = false;

            if (viewOption != ViewOptions.ExactAmountOnly)
            {
                CurrencyInfo currInfo = Services.Interfaces.Services.CurrencyService(DLLEntry.DataModel).DetailedCurrencyInfo(DLLEntry.DataModel, (string) currentCurrency);
                List<CashDenominator> distinct = currInfo.CurrencyItems.GroupBy(f => f.Amount, (key, group) => group.First()).ToList();

                if (!sortAsc)
                {
                    distinct = distinct.OrderByDescending(o => o.Amount).ToList();
                }

                foreach (CashDenominator current in distinct)
                {
                    if ((current.Amount < amount) &&
                        ((viewOption == ViewOptions.LowerAmounts) || (viewOption == ViewOptions.HeigherAndLowerAmounts)) &&
                        (lowerLimit == 0 || current.Amount >= lowerLimit))
                    {
                        if ((!refAmountInArray) && (current.Amount > amount) &&
                            AmountWithinLimits(amount, lowerLimit, upperLimit))
                        {
                            denominators.Insert(0, new CashDenominator { Amount = amount});
                            refAmountInArray = true;
                        }
                        if (AmountWithinLimits(current.Amount, lowerLimit, upperLimit))
                        {
                            denominators.Add(current);
                            refAmountInArray = refAmountInArray || current.Amount == amount;
                        }

                    }
                    else if ((current.Amount >= amount) &&
                             ((viewOption == ViewOptions.HigherAmounts) ||
                              (viewOption == ViewOptions.HeigherAndLowerAmounts)) &&
                             (upperLimit == 0 || current.Amount <= upperLimit))
                    {
                        if ((!refAmountInArray) && (current.Amount > amount) &&
                            AmountWithinLimits(amount, lowerLimit, upperLimit))
                        {
                            denominators.Insert(0, new CashDenominator { Amount = amount });
                            refAmountInArray = true;
                        }

                        if (AmountWithinLimits(current.Amount, lowerLimit, upperLimit))
                        {
                            denominators.Add(current);
                            refAmountInArray = refAmountInArray || current.Amount == amount;
                        }
                    }
                }

                if (!refAmountInArray)
                {
                    if (AmountWithinLimits(amount, lowerLimit, upperLimit))
                    {
                        denominators.Insert(0, new CashDenominator { Amount = amount });
                        refAmountInArray = true;
                    }
                }
            }

            return denominators;
        }

        private static bool AmountWithinLimits(decimal amount, decimal? lowerLimit, decimal? upperLimit)
        {
            if (amount == decimal.Zero && lowerLimit == decimal.Zero && upperLimit == decimal.Zero)
            {
                return false;
            }

            return ((amount >= lowerLimit) || (lowerLimit == 0))
                   &&
                   ((amount <= upperLimit) || (upperLimit == 0));
        }

        public static void SetButtonsForeignCurrency(this TouchScrollButtonPanel panel, StorePaymentMethod tenderInformation, RecordIdentifier exclutionCurrency)
        {
            panel.Clear();
            List<DataEntity> currencyCodes = Providers.CurrencyData.GetList(DLLEntry.DataModel, exclutionCurrency);

            foreach (DataEntity current in currencyCodes)
            {
                panel.AddButton(current.Text, current, "");
            }
        }
    }
}
