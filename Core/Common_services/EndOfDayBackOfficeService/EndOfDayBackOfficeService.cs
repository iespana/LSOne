using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class EndOfDayBackOfficeService : IEndOfDayBackOfficeService
    {
        public IErrorLog ErrorLog
        {
            set
            {
            }
        }

        public void Init(IConnectionManager entry)
        {
        }

        public void CalculateStatement(
            IConnectionManager entry, 
            RecordIdentifier statementID, 
            RecordIdentifier storeID, 
            DateTime startTime, 
            DateTime endTime)
        {
            List<StatementLine> statementLines = new List<StatementLine>();

            // Retrieve some settings
            Store store = Providers.StoreData.Get(entry, (string)storeID);
            StatementGroupingMethod statementMethod = StatementGroupingMethod.POSTerminal; // Currently we do not support other statement methods in the POS
            TenderDeclarationCalculation tenderCalculationMethod = store.TenderDeclarationCalculation;

            // Get all the transactions within the period that are not part of another statement
            List<StatementTransaction> transactions = Providers.StatementInfoData.GetTransactionsWithoutStatementIDs(entry, startTime, endTime, storeID);
            transactions = transactions.Where(w => !string.IsNullOrWhiteSpace(w.TerminalID)).ToList();

            MarkStatementTransactions(entry, statementID, storeID, startTime, endTime);

            // Gets a list of all TenderID,CurrencyID pairs that the transaction list has
            List<String[]> tenderCurrencyList = (from t in transactions
                                                 group t by new { t.TenderTypeID, t.CurrencyCode } into g
                                                 select new String[] { g.Key.TenderTypeID, g.Key.CurrencyCode }).ToList();

            int lineNumber = 0;

            // Filter the transactions by TenderID and CurrencyID. 
            foreach (var item in tenderCurrencyList)
            {
                List<StatementTransaction> tcSublist = (from t in transactions
                                                        where t.TenderTypeID == item[0] & t.CurrencyCode == item[1]
                                                        select t).ToList();

                StatementLine statementLine;

                // For each sublist, filter the transactions by how the Store wants the statement lines grouped
                switch (statementMethod)
                {
                    case StatementGroupingMethod.Staff:
                        List<string> staffList = (from t in tcSublist
                                                  select t.StaffID).Distinct().ToList();
                        foreach (var staffID in staffList)
                        {
                            // All the items in this list have the same tenderID, currencyID and staffID
                            List<StatementTransaction> tcsSublist = (from t in tcSublist
                                                                     where t.StaffID == staffID
                                                                     select t).ToList();

                            statementLine = CreateStatementLineFromUniformTransactions(entry, tcsSublist, statementMethod, storeID, tenderCalculationMethod);
                            statementLine.StatementID = (string)statementID;
                            statementLine.StaffID = staffID;
                            statementLine.LineNumber = (++lineNumber).ToString();
                            statementLines.Add(statementLine);
                        }

                        break;
                    case StatementGroupingMethod.POSTerminal:
                        List<string> terminalList = (from t in tcSublist
                                                     orderby  t.TransactionNumber
                                                     select t.TerminalID).Distinct().ToList();
                        foreach (var terminalID in terminalList)
                        {
                            // Only include terminals in statement that have the include setting
                            var terminal = Providers.TerminalData.Get(entry, terminalID, storeID);
                            if (!terminal.IncludeTerminalInStatement) continue;

                            // All the items in this list have the same tenderID, currencyID and terminalID
                            List<StatementTransaction> tctSublist = (from t in tcSublist
                                                                     where t.TerminalID == terminalID
                                                                     orderby t.TransactionNumber
                                                                     select t).ToList();

                            statementLine = CreateStatementLineFromUniformTransactions(entry, tctSublist, statementMethod, storeID, tenderCalculationMethod);
                            statementLine.StatementID = (string)statementID;
                            statementLine.TerminalID = terminalID;
                            statementLine.LineNumber = (++lineNumber).ToString();

                            // Check the posting legality of the statement line
                            if (terminal.AllowTerminalStatementPosting == AllowTerminalStatementPostingEnum.IfLastTransactionIsEod
                                && !AllowEodBecauseOfMissingEodMarker(entry, storeID, terminalID, statementID))
                            {
                                statementLine.StatementStatus = AllowEODEnums.DisallowEodMarkMissingOnTerminal;
                            }
                            else if (!AllowEodBecauseOfSuspendedTransactions(entry, storeID, terminalID, startTime,endTime))
                            {
                                statementLine.StatementStatus = AllowEODEnums.DisallowSuspendedTransaction;
                            }

                            statementLines.Add(statementLine);
                        }
                        break;
                    case StatementGroupingMethod.Total:
                        // All the items in this list have the same tenderID, currencyID
                        statementLine = CreateStatementLineFromUniformTransactions(entry, tcSublist, statementMethod, storeID, tenderCalculationMethod);
                        statementLine.StatementID = (string)statementID;
                        statementLine.LineNumber = (++lineNumber).ToString();
                        statementLines.Add(statementLine);
                        break;
                    default:
                        break;
                }
            }

            // Safe the StatementLines
            foreach (var statementLine in statementLines)
            {
                Providers.StatementLineData.Save(entry, statementLine);
            }

            // Update the Statement
            StatementInfo statement = new StatementInfo { ID = (string)statementID, Calculated = true, CalculatedTime = DateTime.Now };
            Providers.StatementInfoData.UpdateCalculatedInfo(entry, statement);
        }

        public List<AllowEODEnums> AllowedToPostStatement(IConnectionManager entry, StatementInfo statement)
        {
            List<AllowEODEnums> listOfFlags = new List<AllowEODEnums>();
            // Counting incorrect
            if (!CoutingRulesUpheldForStatement(entry, statement)) listOfFlags.Add(AllowEODEnums.DisallowCountingIncorrect);

            // Suspended transactions
            if (statement.statementLines.Any(x => x.StatementStatus == AllowEODEnums.DisallowSuspendedTransaction))
            {
                listOfFlags.Add(AllowEODEnums.DisallowSuspendedTransaction);
            }
            else
            {
                // When checking for suspended transactions it is not enough to check the statement lines because the suspended transaction might be the only
                // transaction of that terminal and thus that terminal would not have a statement line. So we must also check if there are any terminals 
                // with suspended transactions that wish to warn the user.
                List<SuspendedTransaction> suspendedTransactionsForAllTerminals =
                    Providers.SuspendedTransactionData.SuspendedTransactionsForStoreAndTerminal(
                        entry, statement.StoreID, RecordIdentifier.Empty, statement.StartingTime, statement.EndingTime);

                if (SuspendedTransactionsShouldWarnUser(entry, statement.StoreID, suspendedTransactionsForAllTerminals))
                {
                    listOfFlags.Add(AllowEODEnums.DisallowSuspendedTransaction);
                }
            }

            // EOD marker missing
            foreach (var statementLine in statement.statementLines)
            {
                var terminal = Providers.TerminalData.Get(entry, statementLine.TerminalID, statement.StoreID);
                if (terminal.AllowTerminalStatementPosting == AllowTerminalStatementPostingEnum.IfLastTransactionIsEod)
                {
                    if (statementLine.StatementStatus == AllowEODEnums.DisallowEodMarkMissingOnTerminal)
                    {
                        listOfFlags.Add(AllowEODEnums.DisallowEodMarkMissingOnTerminal);
                    }
                }
            }

            // No rule is found that disallows posting a statement
            if (!listOfFlags.Any())
            {
                listOfFlags.Add(AllowEODEnums.Allowed);
            }
            return listOfFlags;
        }

        public void PostStatement(
            IConnectionManager entry, 
            RecordIdentifier statementID, 
            RecordIdentifier storeID, 
            DateTime postingDate)
        {
            Providers.InventoryTransactionData.PostStatementToInventory(entry, statementID, storeID, postingDate);

            // Update the Statement
            StatementInfo statement = new StatementInfo { ID = (string)statementID, Posted = true, PostingDate = Date.Now };
            Providers.StatementInfoData.UpdatePostedInfo(entry, statement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">Which store we are working with</param>
        /// <param name="tenderCalculationMethod">Which method we are using to sum upp tender declarations</param>
        /// <param name="transactions">A list of statement transactions, all having the same TenderID and CurrencyID and depending on the statementMethod, maybe having the
        /// same staffID or terminalID</param>
        /// <param name="statementMethod">[Staff] means all the transactions have the same staffID, [POSTerminal] means that all the transactions have the same terminalID,
        /// [Total] means only the TenderID's and CurrencyID's of the transactions are the same.
        /// </param>
        /// <returns></returns>
        private static StatementLine CreateStatementLineFromUniformTransactions(IConnectionManager entry,
                                                                        List<StatementTransaction> transactions,
                                                                        StatementGroupingMethod statementMethod,
                                                                        RecordIdentifier storeID,
                                                                        TenderDeclarationCalculation tenderCalculationMethod)
        {
            StatementLine statementLine = new StatementLine();

            if (transactions.Count < 1)
            {
                return statementLine;
            }

            // This can be done because all the transactions have the same tender type and currency
            statementLine.TenderID = transactions[0].TenderTypeID;
            statementLine.CurrencyCode = transactions[0].CurrencyCode;

            switch (statementMethod)
            {
                case StatementGroupingMethod.Staff:
                    statementLine.StaffID = transactions[0].StaffID;
                    break;
                case StatementGroupingMethod.POSTerminal:
                    statementLine.TerminalID = transactions[0].TerminalID;
                    break;
                case StatementGroupingMethod.Total:
                    // Nothing
                    break;
                default:
                    break;
            }

            //statementLine.CountingRequired = CountingRequiredForTender(entry, transactions[0].TenderID, storeID);

            statementLine.TransactionAmount = GetTransactionAmountFromTransactions(transactions);
            statementLine.BankedAmount = GetBankedAmountFromTransactions(transactions);
            statementLine.SafeAmount = GetSafeAmountFromTransactions(transactions);
            statementLine.CountedAmount = GetCountedAmountFromTransactions(entry, transactions, storeID, tenderCalculationMethod);
            statementLine.Difference = statementLine.TransactionAmount +
                                        statementLine.BankedAmount +  //Banked amount is a negative number and needs to be retracted from the Transaction amount
                                        statementLine.SafeAmount -    //Safe amount is a negative number and needs to be retracted from the Transaction amount
                                        statementLine.CountedAmount;

            return statementLine;
        }

        private static decimal GetAmountFromTenderDeclarations(List<StatementTransaction> tenderDeclarationTransactions, TenderDeclarationCalculation tenderCalculationMethod)
        {
            decimal tenderAmount;
            switch (tenderCalculationMethod)
            {
                case TenderDeclarationCalculation.Last:
                    DateTime maxDate = (from t in tenderDeclarationTransactions
                                        select t.TransactionTime).Max();

                    tenderAmount = (from t in tenderDeclarationTransactions
                                    where t.TransactionTime == maxDate
                                    select t.Amount).FirstOrDefault();
                    break;
                case TenderDeclarationCalculation.Sum:
                    tenderAmount = (from t in tenderDeclarationTransactions
                                    select t.Amount).Sum();
                    break;
                default:
                    tenderAmount = 0;
                    break;
            }
            return tenderAmount;
        }

        private static decimal GetSafeAmountFromTransactions(List<StatementTransaction> transactions)
        {
            // Just get the sum of the amounts for transactions which are 'safe transactions'
            decimal safeAmount = (from t in transactions
                                  where t.TransactionType == 17 || t.TransactionType == 20
                                  select t.Amount).Sum();

            return safeAmount;

        }

        private static decimal GetBankedAmountFromTransactions(List<StatementTransaction> transactions)
        {
            // Just get the sum of the amounts for transactions which are 'banked transactions'
            decimal bankedAmount = (from t in transactions
                                    where t.TransactionType == 16 || t.TransactionType == 21
                                    select t.Amount).Sum();



            return bankedAmount;
        }

        private static decimal GetTransactionAmountFromTransactions(List<StatementTransaction> transactions)
        {
            // Just get the sum of the amounts for transactions which are 'payment transactions'
            decimal paymentAmount = (from t in transactions
                                     where (t.TransactionType >= 2 && t.TransactionType <= 6) || t.TransactionType == 22
                                     select t.Amount).Sum();

            return paymentAmount;
        }

        // NOTE: Tender declarations are grouped by terminals, not by f.x. staff members.
        private static decimal GetCountedAmountFromTransactions(IConnectionManager entry,
                                                        List<StatementTransaction> transactions,
                                                        RecordIdentifier storeID,
                                                        TenderDeclarationCalculation tenderCalculationMethod)
        {
            RecordIdentifier tenderTypeID = transactions[0].TenderTypeID;

            // Check if this tender actually needs to be counted
            if (!Providers.StorePaymentMethodData.CountingRequiredForTender(entry, tenderTypeID, storeID))
            {
                // Return the sum of the other transactions
                decimal safeAmount = GetSafeAmountFromTransactions(transactions);
                decimal bankedAmount = GetBankedAmountFromTransactions(transactions);
                decimal transactionAmount = GetTransactionAmountFromTransactions(transactions);

                return transactionAmount - safeAmount - bankedAmount;
            }

            decimal tenderAmount = 0;

            List<StatementTransaction> POSTenderDeclarationTransactions =
                (from t in transactions
                 where t.TransactionType == 7 && !t.IsSCTenderDeclaration
                 select t).ToList();

            if (POSTenderDeclarationTransactions.Count > 0)
            {
                tenderAmount += GetAmountFromTenderDeclarations(POSTenderDeclarationTransactions, tenderCalculationMethod);
            }

            List<StatementTransaction> SCTenderDeclarationTransactions =
                (from t in transactions
                 where t.TransactionType == 7 && t.IsSCTenderDeclaration
                 select t).ToList();

            if (SCTenderDeclarationTransactions.Count > 0)
            {
                tenderAmount += GetAmountFromTenderDeclarations(SCTenderDeclarationTransactions, tenderCalculationMethod);
            }

            return tenderAmount;
        }

        private void MarkStatementTransactions(IConnectionManager entry,
            RecordIdentifier statementID,
            RecordIdentifier storeID,
            DateTime startTime,
            DateTime endTime)
        {
            // Mark the transactions. 
            Providers.StatementInfoData.MarkStatementTransactions(entry, startTime, endTime, storeID, statementID);

            // Mark the Site Manager Tender declaration seperately
            var storeControllerTenderDeclarationTransactions = Providers.TenderDeclarationData.GetAllTenderDeclarationsWithoutStatementIDForAPeriod(entry, storeID, startTime, endTime);

            foreach (var transaction in storeControllerTenderDeclarationTransactions)
            {
                transaction.StatementID = (string)statementID;
                Providers.TenderDeclarationData.Save(entry, transaction);
            }
        }

        private bool CoutingRulesUpheldForStatement(IConnectionManager entry, StatementInfo statement)
        {
            Store storeInfo = Providers.StoreData.Get(entry, (string)statement.StoreID);

            decimal totalAllowedDifference = storeInfo.MaximumPostingDifference;
            decimal lineAllowedDifference = storeInfo.MaximumTransactionDifference;

            decimal totalDifference = (from lines in statement.statementLines
                                       select Math.Abs(lines.Difference)).Sum();

            // Perform checks to see if any of the rules are broken and return false if so
            if (totalAllowedDifference != 0 && totalDifference > totalAllowedDifference) return false;

            if (lineAllowedDifference != 0)
            {
                foreach (StatementLine line in statement.statementLines)
                {
                    if (Math.Abs(line.Difference) > lineAllowedDifference) return false;
                }
            }

            // No rules are broken, return true
            return true;
        }

        private bool AllowEodBecauseOfSuspendedTransactions(
            IConnectionManager entry,
            RecordIdentifier storeID,
            RecordIdentifier terminalID,
            DateTime fromDate,
            DateTime toDate)
        {
            // Get all suspended transactions for the store
            var suspendedTransactions =
                Providers.SuspendedTransactionData.SuspendedTransactionsForStoreAndTerminal(entry, storeID, terminalID, fromDate, toDate);

            if (suspendedTransactions.Count == 0) return true;

            return !SuspendedTransactionsShouldWarnUser(entry, storeID, suspendedTransactions);
        }

        private bool AllowEodBecauseOfMissingEodMarker(
            IConnectionManager entry,
            RecordIdentifier storeID,
            RecordIdentifier terminalID,
            RecordIdentifier statementID)
        {
            var transactionsInStatement = Providers.TransactionData.GetTransactionsForStatementID(entry, statementID);
            var transactionsForStoreAndTerminalInStatement = (
                from x in transactionsInStatement
                where x.TerminalID == terminalID && x.StoreID == storeID
                orderby x.EndDateTime 
                select x);

            // We do not care about theses types of transactions
            var relevantTransactions =
                from x in transactionsForStoreAndTerminalInStatement
                where x.Type != TypeOfTransaction.LogOn && x.Type != TypeOfTransaction.LogOff
                      && x.Type != TypeOfTransaction.EndOfShift && x.Type != TypeOfTransaction.OpenDrawer
                select x;

            if (relevantTransactions.Any())
            {
                return relevantTransactions.Last().Type == TypeOfTransaction.EndOfDay;
            }

            return true;
        }

        private bool SuspendedTransactionsShouldWarnUser(
            IConnectionManager entry,
            RecordIdentifier storeID, 
            List<SuspendedTransaction> suspendedTransactions)
        {
            bool storeWarnWhenPosting = Providers.StoreData.WarnOnStatementPostingIfSuspendedTransExists(entry, storeID);
            Dictionary<RecordIdentifier, SuspendedTransactionsStatementPostingEnum> terminalSettings = new Dictionary<RecordIdentifier, SuspendedTransactionsStatementPostingEnum>();

            foreach (var suspendedTransaction in suspendedTransactions)
            {
                switch (suspendedTransaction.AllowStatementPosting)
                {
                    case SuspendedTransactionsStatementPostingEnum.Yes:
                        return true;
                    case SuspendedTransactionsStatementPostingEnum.StoreDefault:
                        if (storeWarnWhenPosting) return true;
                        break;
                    case SuspendedTransactionsStatementPostingEnum.TerminalDefault:
                        if (!terminalSettings.ContainsKey(new RecordIdentifier(suspendedTransaction.TerminalID, suspendedTransaction.StoreID)))
                        {
                            terminalSettings.Add(new RecordIdentifier(suspendedTransaction.TerminalID, suspendedTransaction.StoreID), Providers.TerminalData.TerminalAllowsEOD(entry, suspendedTransaction.TerminalID, suspendedTransaction.StoreID));
                        }

                        switch (terminalSettings[new RecordIdentifier(suspendedTransaction.TerminalID, suspendedTransaction.StoreID)])
                        {
                            case SuspendedTransactionsStatementPostingEnum.StoreDefault:
                                if (storeWarnWhenPosting) return true;
                                break;
                            case SuspendedTransactionsStatementPostingEnum.Yes:
                                return true;
                        }

                        break;
                }
            }

            return false;
        }
    }
}