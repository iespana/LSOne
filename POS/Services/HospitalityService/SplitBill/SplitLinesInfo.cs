using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.Controls.Enums;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services.SplitBill
{   

    public class SplitInfo
    {

        private IConnectionManager entry;

        public IRetailTransaction TableTransaction { get; set; }
        public IRetailTransaction SplitTransaction { get; set; }

        public List<SaleLineItem> TmpPosTransLine { get; set; }
        public List<SaleLineItem> TmpSplitTransLine { get; set; } 

        public decimal TableTotalAmt { get; set; }
        public decimal TableTotalAmtInclTax { get; set; }
        public decimal SplitTotalAmt { get; set; }
        public decimal SplitTotalAmtInclTax { get; set; }

        public int ExcludedLinesLastLineNo { get; set; }
        
        public int SplitGroupNo { get; set; }

        public int CurrentGuest { get; set; }
        public int LastGuestNo { get; set; }
        public int CurrentSplitGroup { get; set; }
        public int MaxSplitGroup { get; set; }

        public List<int> FocusTableLineId { get; set; }
        public List<int> FocusSplitLineId { get; set; }

        public SplitInfo(IConnectionManager entry)
        {
            TmpPosTransLine = new List<SaleLineItem>();
            TmpSplitTransLine = new List<SaleLineItem>();     

            TableTransaction = null;
            SplitTransaction = null;            
                        
            TableTotalAmt = 0M;
            TableTotalAmtInclTax = 0M;
            SplitTotalAmt = 0M;
            SplitTotalAmtInclTax = 0M;
            ExcludedLinesLastLineNo = 0;
            SplitGroupNo = 0;
            CurrentGuest = 2;
            LastGuestNo = 0;
            CurrentSplitGroup = 0;
            MaxSplitGroup = 0;

            FocusSplitLineId = new List<int>();
            FocusTableLineId = new List<int>();

            this.entry = entry;
        }

        public void ClearTempInfo()
        {
            try
            {
                FocusSplitLineId.Clear();
                FocusTableLineId.Clear();

                TableTotalAmt = 0M;
                TableTotalAmtInclTax = 0M;
                SplitTotalAmt = 0M;
                SplitTotalAmtInclTax = 0M;
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }
        }

        /// <summary>
        /// Returns the appropriate transaction depending on what the operation is to apply to. 
        /// </summary>
        /// <param name="AppliesTo">Only Table and Guest are allowed. Any other value entered an exception will be thrown</param>
        /// <returns>The transaction that applies to the button that was clicked</returns>
        public IRetailTransaction ApplicableTransaction(ButtonAppliesTo AppliesTo)
        {
            try
            {
                if (AppliesTo == ButtonAppliesTo.Table)
                {
                    return TableTransaction;
                }

                if (AppliesTo == ButtonAppliesTo.Guest)
                {
                    return SplitTransaction;
                }

                throw new Exception("Function SplitLinesInfo.ApplicableTransactions can only accept ButtonAppliesTo.Table and Guest");
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }

            return null;
        }

        public List<int> FocusList(ButtonAppliesTo AppliesTo)
        {
            try
            {
                if (AppliesTo == ButtonAppliesTo.Table)
                {
                    return FocusTableLineId;
                }
                else if (AppliesTo == ButtonAppliesTo.Guest)
                {
                    return FocusSplitLineId;
                }

                throw new Exception("Function SplitLinesInfo.FocusList can only accept ButtonAppliesTo.Table and Guest");
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }

            return null;
        }

        public List<SaleLineItem> ApplicableTmpPosLines(ButtonAppliesTo AppliesTo)
        {
            try
            {
                if (AppliesTo == ButtonAppliesTo.Table)
                {
                    return TmpPosTransLine;
                }
                else if (AppliesTo == ButtonAppliesTo.Guest)
                {
                    return TmpSplitTransLine;
                }

                throw new Exception("Function SplitLinesInfo.ApplicableTmpPosLines can only accept ButtonAppliesTo.Table and Guest");
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }

            return null;
        }

        /// <summary>
        /// Returns a filtered list of Sale line items. Can only apply to the Table and Guest transactions
        /// </summary>
        /// <param name="Filter">A LAMBDA function. An example would be sli => sli.Voided = true && sli.Quantity > 10</param>
        /// <param name="AppliesTo">Only Table and Guest are allowed. Any other value entered an exception will be thrown</param>
        /// <returns>A filtered linked list of SaleLineItems</returns>
        public LinkedList<SaleLineItem> SetSaleItemFilter(Func<ISaleLineItem, bool> Filter, ButtonAppliesTo AppliesTo, FilterAppliesTo ApplieFilterTo)
        {
            LinkedList<SaleLineItem> FilteredList = null;
            try
            {
                FilteredList = new LinkedList<SaleLineItem>();
                if (ApplieFilterTo == FilterAppliesTo.RetailTransaction)
                {
                    foreach (var sli in ApplicableTransaction(AppliesTo).SaleItems.Where(Filter))
                    {
                        FilteredList.AddLast((SaleLineItem)sli);
                    }
                }
                else if (ApplieFilterTo == FilterAppliesTo.TmpPOSLines)
                {
                    foreach (SaleLineItem sli in ApplicableTmpPosLines(AppliesTo).Where(Filter))
                    {
                        FilteredList.AddLast(sli);
                    }
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }
            return FilteredList;
        }

        

        /// <summary>
        /// Deletes a specific item found by the filter from the temporary SaleItem variables
        /// </summary>
        /// <param name="Filter">A LAMBDA function. An example would be sli => sli.Voided = true && sli.Quantity > 10</param>
        /// <param name="TmpType">The temporary SaleItems variable to delete from</param>
        public void DeleteTmpPosTransLine(Func<SaleLineItem, bool> Filter, TmpPOSTransLineType TmpType)
        {
            try
            {
                SaleLineItem toDel = null;

                switch (TmpType)
                {                    
                    case TmpPOSTransLineType.TmpPosTransLine:
                        toDel = TmpPosTransLine.FirstOrDefault(Filter);                        
                        break;
                    case TmpPOSTransLineType.TmpSplitTransLine:
                        toDel = TmpSplitTransLine.FirstOrDefault(Filter);                        
                        break;
                    default:
                        throw new NotImplementedException();
                }

                DeleteTmpPosTransLine(toDel, TmpType);

            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }
        }

        public void DeleteTmpPosTransLine(List<SaleLineItem> toDel, TmpPOSTransLineType TmpType)
        {
            try
            {
                foreach (SaleLineItem toDelete in toDel)
                {
                    DeleteTmpPosTransLine(toDelete, TmpType);
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        public void DeleteTmpPosTransLine(SaleLineItem toDel, TmpPOSTransLineType TmpType)
        {
            try
            {
                if (toDel == null)
                    return;

                switch (TmpType)
                {                    
                    case TmpPOSTransLineType.TmpPosTransLine:                        
                        TmpPosTransLine.Remove(toDel); 
                        break;
                    case TmpPOSTransLineType.TmpSplitTransLine:                        
                        TmpSplitTransLine.Remove(toDel); 
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }
        }

        public void DeleteItemFromList(SaleLineItem toDelete, List<SaleLineItem> ListToDeleteFrom)
        {
            try
            {
                if (toDelete == null)
                {
                    return;
                }

                if (ListToDeleteFrom == null)
                {
                    return;
                }

                ListToDeleteFrom.Remove(toDelete);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        public SaleLineItem AddItemToList(SaleLineItem toAdd, List<SaleLineItem> ListToAddTo)
        {
            try
            {
                if (toAdd == null)
                {
                    return null;
                }

                if (ListToAddTo == null)
                {
                    ListToAddTo = new List<SaleLineItem>();
                }

                SaleLineItem cloneItem = (SaleLineItem)toAdd.Clone();
                ListToAddTo.Add(cloneItem);

                return cloneItem;
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        public SaleLineItem AddTmpTransLine(SaleLineItem toAdd, TmpPOSTransLineType TmpType)
        {            
            try
            {
                if (toAdd != null)
                {
                    SaleLineItem cloneItem = (SaleLineItem)toAdd.Clone();
                    switch (TmpType)
                    {                        
                        case TmpPOSTransLineType.TmpPosTransLine:
                            TmpPosTransLine.Add(cloneItem);
                            break;
                        case TmpPOSTransLineType.TmpSplitTransLine:
                            TmpSplitTransLine.Add(cloneItem);
                            break;
                    }
                    return cloneItem;
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }

            return null;
        }

        private void ApplyLineIds(LinkedList<SaleLineItem> TransLines)
        {
            try
            {
                int LineId = 1;
                foreach (SaleLineItem sli in TransLines)
                {
                    sli.LineId = LineId;
                    LineId++;
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }
        }        

        public void UnMarkSelectedItems(List<int> SelectedLineIds, bool UnMarkAll, ButtonAppliesTo AppliesTo)
        {
            if (UnMarkAll)
            {
                foreach (SaleLineItem sli in ApplicableTmpPosLines(AppliesTo))
                {
                    sli.SplitMarked = false;
                }
            }
            else
            {
                foreach (int sel in SelectedLineIds)
                {
                    SaleLineItem sli = ApplicableTmpPosLines(AppliesTo).FirstOrDefault(line => line.LineId == sel);
                    if (sli != null)
                    {
                        sli.SplitMarked = false;                        
                    }
                }
            }
        }

        public SaleLineItem GetItem(int LineId, ButtonAppliesTo AppliesTo)
        {
            return ApplicableTmpPosLines(AppliesTo).FirstOrDefault(f => f.LineId == LineId);
        }

        public void MarkSelectedItems(List<int> SelectedLineIds, ButtonAppliesTo AppliesTo)
        {
            foreach (int sel in SelectedLineIds)
            {
                SaleLineItem sli = ApplicableTmpPosLines(AppliesTo).FirstOrDefault(line => line.LineId == sel);
                if (sli != null)
                {
                    sli.SplitMarked = true;
                }
            }
        }        

        public void MarkSelectedItems(List<SaleLineItem> SaleItems)
        {
            if (SaleItems == null)
            {
                return;
            }

            foreach (SaleLineItem sli in SaleItems)
            {
                sli.SplitMarked = true;
            }
        }

        public void ClearSplitGroupInfo()
        {
            CurrentSplitGroup = 0;
            MaxSplitGroup = 0;
        }

        public void CalculateTotalAmts()
        {
            TableTotalAmt = 0M;
            TableTotalAmtInclTax = 0M;
            SplitTotalAmt = 0M;
            SplitTotalAmtInclTax = 0M;

            foreach (SaleLineItem sli in TmpPosTransLine)
            {
                TableTotalAmt += sli.NetAmount;
                TableTotalAmtInclTax += sli.NetAmountWithTax;
            }

            foreach (SaleLineItem sli in TmpSplitTransLine)
            {
                SplitTotalAmt += sli.NetAmount;
                SplitTotalAmtInclTax += sli.NetAmountWithTax;
            }
        }

        public int GetLastLineNo(SplitAction Action)
        {
            int MainLastLineNo = 0;
            int RestLastLineNo = 0;
            int LastLineNo = 0;

            try
            {

                if (Action == SplitAction.SplitBill)
                {
                    if (TmpPosTransLine.Count > 0)
                    {
                        SaleLineItem sli = TmpPosTransLine.LastOrDefault();
                        if (sli != null)
                        {
                            MainLastLineNo = sli.LineId;
                        }
                    }

                    if (TmpSplitTransLine.Count > 0)
                    {
                        SaleLineItem sli = TmpSplitTransLine.LastOrDefault();
                        if (sli != null)
                        {
                            RestLastLineNo = sli.LineId;
                        }
                    }
                    LastLineNo = ExcludedLinesLastLineNo;

                    if (MainLastLineNo > LastLineNo)
                    {
                        LastLineNo = MainLastLineNo;
                    }

                    if (RestLastLineNo > LastLineNo)
                    {
                        LastLineNo = RestLastLineNo;
                    }
                }
                else
                {                    
                    SaleLineItem sli = TmpSplitTransLine.LastOrDefault();
                    if (sli != null)
                    {
                        LastLineNo = sli.LineId;
                    }
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }

            return LastLineNo;
        }
    }
}
