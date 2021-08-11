using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Services.Interfaces.SupportClasses.Price;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services
{
    public partial class TaxService
    {
        /// <summary>
        /// Clears the tax exemption properties of the transaction and recalculates the prices
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        public virtual void ClearTaxExemption(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ClearTransactionTaxExemption, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
            {
                // Clear RetailTransaction
                retailTransaction.TaxExempt = false;
                retailTransaction.TransactionTaxExemptionCode = "";

                Interfaces.Services.PriceService(entry).SetPrice(entry, retailTransaction, CacheType.CacheTypeTransactionLifeTime);

                // Clear SaleItems
                foreach (ISaleLineItem item in retailTransaction.SaleItems)
                {
                    // We should not overwrite existing tax exemption codes that are coming from active payment limitations.
                    // We should also not modify lines that have already been paid for with a limitation since it could affect the current balance.
                    if (item.TaxExempt && !string.IsNullOrEmpty(item.LimitationCode))
                    {
                        continue;
                    }

                    item.TaxExempt = false;
                    item.TaxExemptionCode = "";

                    item.TaxLines.Clear();

                    if (!item.PriceOverridden)
                    {
                        TradeAgreementPriceInfo tradeAgreementPriceInfo = Interfaces.Services.PriceService(entry).GetPrice(
                            entry,
                            item.ItemId,
                            RecordIdentifier.Empty,
                            retailTransaction.Customer.ID,
                            entry.CurrentStoreID,
                            retailTransaction.StoreCurrencyCode,
                            item.OrgUnitOfMeasure,
                            retailTransaction.Hospitality.ActiveHospitalitySalesType,
                            retailTransaction.TaxIncludedInPrice,
                            item.Quantity,
                            CacheType.CacheTypeTransactionLifeTime);

                        if (tradeAgreementPriceInfo.Price != null)
                        {
                            item.Price = (decimal) tradeAgreementPriceInfo.Price;
                            item.PriceID = (string)tradeAgreementPriceInfo.PriceID;
                            item.PriceType = tradeAgreementPriceInfo.PriceType;
                        }
                    }
                }

                Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, retailTransaction, true);
                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, retailTransaction);

                POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.TaxExemptionCleared);  // Tax exemption cleared
            }
        }

        /// <summary>
        /// Asks the user for a tax exemption code and then sets the tax exemption properties of the transaction and recalculates the prices
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        public virtual void SetTaxExemption(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            // Are you sure you want to tax exempt ?
            if (Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TaxExemptTransaction, MessageBoxButtons.YesNo, MessageDialogType.Attention) != DialogResult.Yes)
            {
                return;
            }

            string taxExemptionCode = "";
            int maxLengthOfCode = 100;

            var result = Interfaces.Services.DialogService(entry).KeyboardInput(ref taxExemptionCode, Properties.Resources.EnterTaxExemptionCode, Properties.Resources.TaxExemptionCode, maxLengthOfCode, InputTypeEnum.Normal);

            while (result == DialogResult.OK && string.IsNullOrEmpty(taxExemptionCode))
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CannotEnterEmptyCode, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                result = Interfaces.Services.DialogService(entry).KeyboardInput(ref taxExemptionCode, Properties.Resources.EnterTaxExemptionCode, Properties.Resources.TaxExemptionCode, maxLengthOfCode, InputTypeEnum.Normal);
            }

            if (result == DialogResult.OK)
            {
                retailTransaction.TaxExempt = true;
                retailTransaction.TransactionTaxExemptionCode = taxExemptionCode;

                // Attach the tax exemption code to all retail items, so the Tax service can fetch the tax exemption code
                // from the sales items when processing calculateTax

                foreach (ISaleLineItem item in retailTransaction.SaleItems)
                {
                    // We should not overwrite existing tax exemption codes that are coming from active payment limitations which are tax exempt
                    if(item.TaxExempt && !string.IsNullOrEmpty(item.LimitationCode))
                    {
                        continue;
                    }

                    item.TaxExempt = true;
                    item.TaxExemptionCode = taxExemptionCode;                    
                }

                Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, retailTransaction, true);
                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, retailTransaction);

                POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.TransactionTaxExempted);  // The transaction has been tax exempted
            }
        }


    }
}
