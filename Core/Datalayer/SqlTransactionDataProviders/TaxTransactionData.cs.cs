using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class TaxTransactionData : SqlServerDataProviderBase, ITaxTransactionData
    {
        public virtual void Insert(IConnectionManager entry, ISaleLineItem saleLineItem, TaxItem taxItem, int counter)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONTAXTRANS", StatementType.Insert, false);

            bool transHeader = false;
            int lineNum = -1;

            if (saleLineItem == null)
            {
                transHeader = true;
            }
            else
            {
                lineNum = saleLineItem.LineId;
            }

            statement.AddKey("TRANSACTIONID", saleLineItem.Transaction.TransactionId);
            statement.AddKey("LINENUM", (decimal)lineNum, SqlDbType.Decimal);
            statement.AddKey("TAXLINENUM", (decimal)counter, SqlDbType.Decimal);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("TRANSHEADER", transHeader ? (byte)1 : (byte)0, SqlDbType.TinyInt);
            statement.AddKey("STORE", (string)saleLineItem.Transaction.StoreId);
            statement.AddKey("TERMINAL", (string)saleLineItem.Transaction.TerminalId);

            statement.AddField("TAXGROUP", (string)taxItem.ItemTaxGroup);
            statement.AddField("TAXCODE", (string)taxItem.TaxCode);
            statement.AddField("TAXPCT", taxItem.Percentage, SqlDbType.Decimal);
            statement.AddField("TAXAMOUNT", taxItem.Amount, SqlDbType.Decimal);
            statement.AddField("TAXROUNDOFF", taxItem.TaxRoundOff, SqlDbType.Decimal);
            statement.AddField("TAXROUNDOFFTYPE", taxItem.TaxRoundOffType, SqlDbType.Int);
            statement.AddField("PRICEWITHTAX", taxItem.PriceWithTax, SqlDbType.Decimal);
            statement.AddField("TAXCODERECEIPTDISPLAY", taxItem.TaxCodeDisplay);
            statement.AddField("TAXGROUPRECEIPTDISPLAY", taxItem.ItemTaxGroupDisplay);
            statement.AddField("TAXEXEMPT", taxItem.TaxExempt ? (byte)1 : (byte)0, SqlDbType.TinyInt);
            statement.AddField("TAXEXEMPTIONCODE", taxItem.TaxExemptionCode);

            entry.Connection.ExecuteStatement(statement);
        }

        private static void PopulateTaxItem(IConnectionManager entry, IDataReader dr, TaxItem item, ref decimal taxRatePct)
        {
            item.ItemTaxGroup = (string)dr["TAXGROUP"];
            item.TaxCode = (string)dr["TAXCODE"];
            item.Percentage = (decimal)dr["TAXPCT"];
            item.Amount = (decimal)dr["TAXAMOUNT"];
            item.TaxRoundOff = (decimal)dr["TAXROUNDOFF"];
            item.TaxRoundOffType = (int)dr["TAXROUNDOFFTYPE"];
            item.PriceWithTax = -(decimal)dr["NETAMOUNTINCLTAX"];
            item.TaxCodeDisplay = (string)dr["TAXCODERECEIPTDISPLAY"];
            item.ItemTaxGroupDisplay = (string)dr["TAXGROUPRECEIPTDISPLAY"];
            item.TaxExempt = (byte)dr["TAXEXEMPT"] == 1;
            item.TaxExemptionCode = (string)dr["TAXEXEMPTIONCODE"];

            taxRatePct += item.Percentage;
        }

        /// <summary>
        /// Get all tax items connected to a transaction id for a given line number
        /// </summary>
        /// <param name="entry">Entry into the database</param>     
        /// <param name="transactionId">The transaction id on the tax items </param> 
        /// <param name="lineNum">The line number to be fetched</param>  
        /// <param name="trans">The current transaction - information regarding store and terminal used</param>
        /// <param name="sumOfTaxRatePercent">Returns sum of all tax rates</param>
        /// <returns></returns>   
        public virtual List<TaxItem> GetTaxItems(IConnectionManager entry, RecordIdentifier transactionId, int lineNum, IRetailTransaction trans, out decimal sumOfTaxRatePercent)
        {
            sumOfTaxRatePercent = 0M;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select 
                                     tt.TAXGROUP
                                    ,tt.TAXCODE
                                    ,tt.TAXPCT
                                    ,tt.TAXAMOUNT
                                    ,tt.TAXROUNDOFF
                                    ,tt.TAXROUNDOFFTYPE
                                    ,tt.TAXCODERECEIPTDISPLAY
                                    ,tt.TAXGROUPRECEIPTDISPLAY
                                    ,ISNULL(tt.TAXEXEMPT,0) as TAXEXEMPT
                                    ,ISNULL(tt.TAXEXEMPTIONCODE,'') as TAXEXEMPTIONCODE
                                    ,st.NETAMOUNTINCLTAX 
                                    from RBOTRANSACTIONTAXTRANS tt
                                    Join RBOTRANSACTIONSALESTRANS st on st.TRANSACTIONID = tt.TRANSACTIONID and st.DATAAREAID = tt.DATAAREAID 
                                        and st.STORE = tt.STORE and st.TERMINALID = tt.TERMINAL and st.LINENUM = tt.LINENUM
                                    where tt.STORE = @storeID and tt.TERMINAL = @terminalID and tt.TRANSACTIONID = @transactionId 
                                    and tt.LINENUM = @lineNum and tt.DATAAREAID = @dataAreaID 
                                    order by tt.TAXLINENUM";


                MakeParam(cmd, "storeID", trans.StoreId);
                MakeParam(cmd, "terminalID", trans.TerminalId);
                MakeParam(cmd, "transactionId", (string)transactionId);
                MakeParam(cmd, "lineNum", (decimal)lineNum, SqlDbType.Decimal);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<TaxItem, decimal>(entry, cmd, CommandType.Text, ref sumOfTaxRatePercent, PopulateTaxItem);
            }
        }

        public virtual void PopulateTaxLinesForTransaction(RetailTransaction trans)
        {
            var taxItemsToAddToTrans = new List<TaxItem>();
            foreach (var item in trans.ISaleItems.Where(p => !p.Voided))
            {
                foreach (var taxItem in item.ITaxLines)
                {
                    bool taxCodeAlreadyIncluded = taxItemsToAddToTrans.Select(x => x.TaxCode).Contains(taxItem.TaxCode);
                    if (!taxCodeAlreadyIncluded)
                    {
                        taxItemsToAddToTrans.Add(taxItem);
                    }
                    else
                    {
                        var taxItemInList = taxItemsToAddToTrans.First(x => x.TaxCode == taxItem.TaxCode);
                        taxItemInList.PriceWithTax += taxItem.PriceWithTax;
                        taxItemInList.Amount += taxItem.Amount;
                    }
                }
            }

            trans.TaxLines = taxItemsToAddToTrans;
        }
    }
}
