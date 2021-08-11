using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Enums;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Processes.Enums;
using LSOne.Services.Interfaces.Constants;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services.SplitBill
{
    /// <summary>
    /// A class for all Hospitality operations
    /// </summary>
    public class Operations
    {
        /// <summary>
        /// Gets the Split Bill info
        /// </summary>
        /// <value>The split bill info.</value>
        public SplitInfo SplitBillInfo { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Operations" /> class.
        /// </summary>
        /// <param name="SplitBillInfo">The split bill info.</param>
        /// <exception cref="System.Exception">Split Bill and Transfer operations cannot be started without an initialized SplitBillInfo object</exception>
        public Operations(SplitInfo SplitBillInfo)
        {
            if (SplitBillInfo == null)
            {
                throw new InvalidOperationException("Split Bill and Transfer operations cannot be started without an initialized SplitBillInfo object");
            }

            this.SplitBillInfo = SplitBillInfo;
        }

        #endregion

        #region Unsplit All Lines

        /// <summary>
        /// Unsplit all lines.
        /// </summary>
        public void UnSplitAllLines(IConnectionManager entry)
        {
            try
            {
                bool CombineRequested = false;
                bool RequestConfirm = false;

                //Set all items in the Table transaction to SplitMarked = true                
                SplitBillInfo.MarkSelectedItems(SplitBillInfo.TmpPosTransLine);

                UnSplitLines(entry);

                List<SaleLineItem> WorkTmpPosTransLine = new List<SaleLineItem>();

                foreach (SaleLineItem tmpSplitPosTrans in SplitBillInfo.TmpSplitTransLine)
                {
                    SplitBillInfo.AddItemToList(tmpSplitPosTrans, WorkTmpPosTransLine);                    
                }

                int LineNo = 0;
                foreach (SaleLineItem WorkTmpPosLine in WorkTmpPosTransLine)
                {
                    CombineRequested = MoveLineTo(entry, WorkTmpPosLine, ref RequestConfirm, ref CombineRequested, ref LineNo, true);
                    RequestConfirm = false;

                    SaleLineItem tmpSplitPosTransLine = SplitBillInfo.TmpSplitTransLine.FirstOrDefault(f => f.LineId == WorkTmpPosLine.LineId);
                    if (tmpSplitPosTransLine != null)
                    {
                        SplitBillInfo.DeleteTmpPosTransLine(tmpSplitPosTransLine, TmpPOSTransLineType.TmpSplitTransLine);
                    }
                }

                SplitBillInfo.UnMarkSelectedItems(null, true, ButtonAppliesTo.Table);
                SplitBillInfo.FocusTableLineId.Clear();
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Moves lines between guest or table section of the Split bill
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="PosTrLineIn">The line selected</param>
        /// <param name="RequestConfirm">if set to <c>true</c> then the user needs to confirm the move</param>
        /// <param name="CombineRequested">if set to <c>true</c> then the user needs to confirm the combination</param>
        /// <param name="GoToLine"></param>
        /// <param name="MoveToMain">if set to <c>true</c> then the line is being moved to the table section of the Split bill</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        private bool MoveLineTo(IConnectionManager entry, SaleLineItem PosTrLineIn, ref bool RequestConfirm, ref bool CombineRequested, ref int GoToLine, bool MoveToMain)
        {
            bool Combine = false;
            SaleLineItem tmpTransLine;
            ButtonAppliesTo AppliesTo;

            //Set the filter - when moving from split to main an additional condition is needed
            Func<SaleLineItem, bool> Filter; 
            if (MoveToMain)
            {
                AppliesTo = ButtonAppliesTo.Table;

                Filter = f => f.SplitOriginLineNo == PosTrLineIn.SplitOriginLineNo && f.ItemId == PosTrLineIn.ItemId;
            }
            else
            {
                AppliesTo = ButtonAppliesTo.Guest;

                //When moving from the split trans to the main we need to make sure we have lines for the correct guest
                Filter = f => f.SplitOriginLineNo == PosTrLineIn.SplitOriginLineNo && f.ItemId == PosTrLineIn.ItemId && f.CoverNo == SplitBillInfo.CurrentGuest;
            }

            if (PosTrLineIn.SplitOriginLineNo != 0)
            {
                tmpTransLine = SplitBillInfo.ApplicableTmpPosLines(AppliesTo).FirstOrDefault(Filter);
                if (tmpTransLine != null)
                {                    
                    if (RequestConfirm)
                    {
                        RequestConfirm = false;
                        string msg;
                        string guestStr = "1";
                        if (!MoveToMain) { guestStr = SplitBillInfo.CurrentGuest.ToString(); }

                        //If more than one line is in the split trans 
                        if (SplitBillInfo.ApplicableTmpPosLines(AppliesTo).Count > 1)
                        {
                            //Do you want to combine these lines with the existing split parts of guest %1
                            msg = Properties.Resources.CombineLinesQuestion;
                            msg = msg.Replace("%1", guestStr);
                        }
                        else
                        {                            
                            //Do you want to combine %1 with the existing split part of guest %2
                            msg = Properties.Resources.CombineExistingSplitPartQuestion;
                            msg = msg.Replace("%1", tmpTransLine.Description);
                            msg = msg.Replace("%2", guestStr);
                        }
                        DialogResult dlgResult = Interfaces.Services.DialogService(entry).ShowMessage(msg, MessageBoxButtons.YesNo, MessageDialogType.Attention);
                        if (dlgResult == DialogResult.Yes)
                        {
                            Combine = true;
                            CombineRequested = true;
                        }
                    }
                    else
                    {
                        Combine = CombineRequested;
                    }
                }
            }

            if (Combine)
            {
                tmpTransLine = SplitBillInfo.ApplicableTmpPosLines(AppliesTo).FirstOrDefault(Filter);
                if (tmpTransLine != null)
                {
                    int CombineLineNo = tmpTransLine.LineId;
                    int OrgSplitLineNo = tmpTransLine.SplitOriginLineNo;
                    int NewSplitLineNo = OrgSplitLineNo;
                    int GroupMarking = tmpTransLine.SplitGroup;

                    if (!SplitExists(entry, tmpTransLine.LineId, PosTrLineIn))
                    {
                        NewSplitLineNo = 0;
                        GroupMarking = 0;
                    }
                    tmpTransLine = SplitBillInfo.ApplicableTmpPosLines(AppliesTo).FirstOrDefault(f => f.LineId == CombineLineNo);
                    if (tmpTransLine != null)
                    {
                        decimal OldQty = tmpTransLine.Quantity;
                        decimal OldUnitQty = tmpTransLine.UnitQuantity;
                        decimal OldQtyDiscounted = tmpTransLine.QuantityDiscounted;
                        decimal NewAmt = PosTrLineIn.NetAmountWithTax + tmpTransLine.NetAmountWithTax;

                        tmpTransLine.Quantity = OldQty + PosTrLineIn.Quantity;
                        tmpTransLine.UnitQuantity = OldUnitQty + PosTrLineIn.UnitQuantity;
                        tmpTransLine.QuantityDiscounted = OldQtyDiscounted + PosTrLineIn.QuantityDiscounted;
                        tmpTransLine.SplitOriginLineNo = NewSplitLineNo;
                        tmpTransLine.SplitGroup = GroupMarking;
                        if (OrgPriceValid(tmpTransLine))
                        {
                            tmpTransLine.NoPriceCalculation = false;
                            tmpTransLine.PriceWithTax = tmpTransLine.OriginalPriceWithTax;
                            tmpTransLine.OriginalPriceWithTax = 0M;
                        }

                        ITaxService tax = Interfaces.Services.TaxService(entry);

                        tax.CalculateTax(entry, SplitBillInfo.TableTransaction, tmpTransLine);

                        ICalculationService calculation = Services.Interfaces.Services.CalculationService(entry);

                        calculation.CalculateLine(entry, tmpTransLine, SplitBillInfo.TableTransaction, false);

                        decimal AmtDiff = NewAmt - tmpTransLine.NetAmountWithTax;

                        if (AmtDiff != 0 && !LineIsDealItemLine(tmpTransLine))
                        {
                            decimal OrgPrice = tmpTransLine.PriceWithTax;
                            tmpTransLine.PriceWithTax = tmpTransLine.PriceWithTax + (AmtDiff / tmpTransLine.Quantity);
                            tmpTransLine.NoPriceCalculation = false;

                            tax.CalculateTax(entry,SplitBillInfo.TableTransaction, tmpTransLine);
                            calculation.CalculateLine(entry, tmpTransLine, SplitBillInfo.TableTransaction, false);

                            tmpTransLine.OriginalPriceWithTax = OrgPrice;
                        }

                        GoToLine = tmpTransLine.LineId;                        
                        return true;
                    }
                }
            }
            else
            {
                if (MoveToMain)
                {
                    tmpTransLine = SplitBillInfo.AddTmpTransLine(PosTrLineIn, TmpPOSTransLineType.TmpPosTransLine);
                }
                else
                {
                    tmpTransLine = SplitBillInfo.AddTmpTransLine(PosTrLineIn, TmpPOSTransLineType.TmpSplitTransLine);
                }
                
                tmpTransLine.CoverNo = 0;
                if (!MoveToMain)                
                {
                    tmpTransLine.CoverNo = SplitBillInfo.CurrentGuest;
                }

                tmpTransLine.SplitMarked = false;

                GoToLine = tmpTransLine.LineId;
                
                return false;
            }

            return false;
        }

        /// <summary>
        /// Returns true if a split exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="CombineLineNo">The combine line no.</param>
        /// <param name="CombinePosTrLine">The line to combine</param>
        /// <returns><c>true</c> if the split exists, <c>false</c> otherwise</returns>
        private bool SplitExists(IConnectionManager entry, int CombineLineNo, SaleLineItem CombinePosTrLine)
        {
            try
            {
                SaleLineItem tmpSplitTransLine = SplitBillInfo.TmpSplitTransLine.FirstOrDefault(f => f.SplitOriginLineNo == CombinePosTrLine.SplitOriginLineNo
                                                                                                  && f.ItemId == CombinePosTrLine.ItemId
                                                                                                  && (f.LineId != CombineLineNo || f.LineId != CombinePosTrLine.LineId));
                //If nothing is found
                if (tmpSplitTransLine == null)
                {
                    tmpSplitTransLine = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.SplitOriginLineNo == CombinePosTrLine.SplitOriginLineNo
                                                                                       && f.ItemId == CombinePosTrLine.ItemId
                                                                                       && (f.LineId != CombineLineNo || f.LineId != CombinePosTrLine.LineId));
                    if (tmpSplitTransLine == null)
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        #endregion

        #region Unsplit Line

        /// <summary>
        /// Unsplit all lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <exception cref="System.Exception"></exception>
        public void UnSplitLines(IConnectionManager entry)
        {
            try
            {
                List<SaleLineItem> UnSplitSaleItems = new List<SaleLineItem>();            

                foreach (SaleLineItem sli in SplitBillInfo.TmpPosTransLine.Where(sli => sli.SplitMarked && sli.SplitOriginLineNo != 0))
                {
                    SplitBillInfo.AddItemToList(sli, UnSplitSaleItems);                    
                }

                foreach (SaleLineItem unsplitSli in UnSplitSaleItems)
                {
                    //If the line exists in the table transaction
                    SaleLineItem tmpSli = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == unsplitSli.LineId);
                    if (tmpSli != null)
                    {
                        UnsplitSingleLine(entry, unsplitSli);
                    }
                }

                //UpdateAllDeals(false); //TODO: Add for Deal functionality                
            }
            catch (InvalidOperationException opEx)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), opEx);
                throw new Exception(opEx.Message, opEx);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Unsplits a single line.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="PosTrLineIn">The pos tr line in.</param>
        private void UnsplitSingleLine(IConnectionManager entry, SaleLineItem PosTrLineIn)
        {
            try
            {
                List<SaleLineItem> UnSplitSaleItems = new List<SaleLineItem>();

                int OrgSplitLineNo = PosTrLineIn.SplitOriginLineNo;
                SaleLineItem SelTmpPosTransLine = null;
                if (OrgSplitLineNo != PosTrLineIn.LineId)
                {
                    SaleLineItem tmpPosTransLine = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == OrgSplitLineNo);
                    if (tmpPosTransLine == null)
                    {
                        SaleLineItem tmpSplitTransLine = SplitBillInfo.TmpSplitTransLine.FirstOrDefault(f => f.LineId == OrgSplitLineNo);
                        if (tmpSplitTransLine != null)
                        {
                            if (MoveSplitLineToMain(entry, tmpSplitTransLine))
                            {
                                tmpPosTransLine = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == OrgSplitLineNo);
                                if (tmpPosTransLine != null)
                                {
                                    SelTmpPosTransLine = SplitBillInfo.AddItemToList(tmpPosTransLine, UnSplitSaleItems);                                    
                                }
                            }
                        }
                        else
                        {
                            SelTmpPosTransLine = SplitBillInfo.AddItemToList(PosTrLineIn, UnSplitSaleItems);
                        }
                    }
                    else
                    {
                        SelTmpPosTransLine = SplitBillInfo.AddItemToList(tmpPosTransLine, UnSplitSaleItems);
                    }
                }
                else
                {
                    SelTmpPosTransLine = SplitBillInfo.AddItemToList(PosTrLineIn, UnSplitSaleItems);
                }

                decimal SplitQty = 0M;
                decimal SplitAmt = 0M;
                decimal SplitQtyDisc = 0M;

                List<SaleLineItem> deleteLines = new List<SaleLineItem>();
                foreach (SaleLineItem tmpPosTransLine in SplitBillInfo.TmpPosTransLine.Where(w => w.SplitOriginLineNo == OrgSplitLineNo && w.LineId != SelTmpPosTransLine.LineId))                
                {
                    SplitQty += tmpPosTransLine.Quantity;
                    SplitAmt += tmpPosTransLine.NetAmountWithTax;
                    SplitQtyDisc += tmpPosTransLine.QuantityDiscounted;                    
                    deleteLines.Add(tmpPosTransLine);                    
                }                                   
                SplitBillInfo.DeleteTmpPosTransLine(deleteLines, TmpPOSTransLineType.TmpPosTransLine);
                deleteLines.Clear();
                                
                foreach (SaleLineItem tmpSplitTransLine in SplitBillInfo.TmpSplitTransLine.Where(w => w.SplitOriginLineNo == OrgSplitLineNo))
                {
                    SplitQty += tmpSplitTransLine.Quantity;
                    SplitAmt += tmpSplitTransLine.NetAmountWithTax;
                    SplitQtyDisc += tmpSplitTransLine.QuantityDiscounted;                    
                    deleteLines.Add(tmpSplitTransLine);
                }
                SplitBillInfo.DeleteTmpPosTransLine(deleteLines, TmpPOSTransLineType.TmpSplitTransLine);
                deleteLines.Clear();

                decimal OldQty = SelTmpPosTransLine.Quantity;
                decimal OldQtyDiscounted = SelTmpPosTransLine.QuantityDiscounted;
                SaleLineItem tablePosTransLine = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == SelTmpPosTransLine.LineId);
                if (tablePosTransLine != null)
                {
                    SplitAmt += tablePosTransLine.NetAmountWithTax;
                    tablePosTransLine.Quantity = OldQty + SplitQty;                    
                    tablePosTransLine.QuantityDiscounted = OldQtyDiscounted + SplitQtyDisc;
                    tablePosTransLine.SplitOriginLineNo = 0;

                    if (OrgPriceValid(tablePosTransLine))
                    {
                        tablePosTransLine.NoPriceCalculation = false;
                        tablePosTransLine.PriceWithTax = tablePosTransLine.OriginalPriceWithTax;                        
                        tablePosTransLine.OriginalPriceWithTax = 0M;
                    }

                    ITaxService tax = Interfaces.Services.TaxService(entry);

                    tax.CalculateTax(entry, SplitBillInfo.TableTransaction, tablePosTransLine);
                    
                    ICalculationService calculation = Services.Interfaces.Services.CalculationService(entry);
                    
                    calculation.CalculateLine(entry, tablePosTransLine, SplitBillInfo.TableTransaction, false);

                    decimal AmtDiff = SplitAmt - tablePosTransLine.NetAmountWithTax;

                    if (AmtDiff != 0 && !LineIsDealItemLine(tablePosTransLine))
                    {
                        decimal OrgPrice = tablePosTransLine.PriceWithTax;
                        tablePosTransLine.PriceWithTax = tablePosTransLine.PriceWithTax + (AmtDiff / tablePosTransLine.Quantity);
                        tablePosTransLine.NoPriceCalculation = true;

                        tax.CalculateTax(entry, SplitBillInfo.TableTransaction, tablePosTransLine);
                        calculation.CalculateLine(entry, tablePosTransLine, SplitBillInfo.TableTransaction, false);

                        tablePosTransLine.OriginalPriceWithTax = OrgPrice;
                    }

                    tablePosTransLine.SplitMarked = false;
                    tablePosTransLine.SplitGroup = 0;                    
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Returns true if the original price is valid
        /// </summary>
        /// <param name="PosTrLineIn">The line being checked</param>
        /// <returns><c>true</c> if original price is valid, <c>false</c> otherwise</returns>
        private bool OrgPriceValid(SaleLineItem PosTrLineIn)
        {
            if (PosTrLineIn == null)
                return false;

            if (PosTrLineIn.OriginalPriceWithTax != 0 && PosTrLineIn.OriginalPriceWithTax != PosTrLineIn.PriceWithTax)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Moves the split line to table section of Split Bill
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="PosTrLineIn">The selected line</param>
        /// <returns><c>true</c> if split is done, <c>false</c> otherwise</returns>
        private bool MoveSplitLineToMain(IConnectionManager entry, SaleLineItem PosTrLineIn)
        {
            try
            {
                SaleLineItem tmpSplitTransLine = SplitBillInfo.TmpSplitTransLine.FirstOrDefault(f => f.LineId == PosTrLineIn.LineId);
                if (tmpSplitTransLine != null)
                {
                    SaleLineItem splitLine = SplitBillInfo.AddTmpTransLine(tmpSplitTransLine, TmpPOSTransLineType.TmpPosTransLine);
                    splitLine.CoverNo = 0;
                    splitLine.SplitMarked = false;                    
                    SplitBillInfo.DeleteTmpPosTransLine(tmpSplitTransLine, TmpPOSTransLineType.TmpSplitTransLine);
                    return true;
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }

            return false;
        }

        #endregion

        #region Split Line

        /// <summary>
        /// Operation that splits lines. Can take various parameters that define how the split is done
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="Parameter">The parameter</param>
        /// <param name="ReceiptItems">The Receipt item control</param>
        /// <param name="MaxSplitNo">The maximum splits allowed</param>
        public void SplitLines(IConnectionManager entry, string Parameter, ReceiptItems ReceiptItems, int MaxSplitNo)
        {
            try
            {
                //bool RunDirectly = true; //TODO: when Guest functionality has been added - this should be changed to be default false

                List<int> selectedItems = ReceiptItems.ReturnSelectedItems();
                if (selectedItems.Count == 0 && Parameter != "UNSPLITALL")
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoItemsSelected); //No items selected
                    return;
                }                

                SplitBillMode functionMode = SplitBillMode.SplitDivide;
                int splitParam = 0;
                decimal splitParamAmt = 0M;
                decimal splitParamAmtPercent = 0M;                

                SplitBillInfo.ClearSplitGroupInfo();                

                switch (Parameter)
                {
                    case "/2":
                    case "/3":
                    case "/4":
                    case "/?":
                        splitParam = EvaluateParam(entry, Parameter);
                        break;
                    case "/?ALL":
                        splitParam = EvaluateParam(entry, Parameter);
                        functionMode = SplitBillMode.SplitAll;
                        //RunDirectly = true;
                        break;
                    case "/ALL":
                        //If max split is set to 0 then make the split function ask for the number of splits
                        if (MaxSplitNo == 0)
                        {
                            Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoSplitsAllowed, MessageBoxButtons.OK, MessageDialogType.Generic);
                            return;
                        }
                        else
                        {
                            splitParam = EvaluateParam(entry, "/?ALL");
                            if (splitParam == 0)
                            {
                                return;
                            }
                            //splitParam = 0; //TODO: when guest functionality has been added - splitParam should get the value of the guests on the table
                        }
                        //if (splitParam == 0)
                        //{
                        //    //Are you sure you want to split evenly into %1 parts?
                        //    string msg = LSRetail.POS.POSSharedCore.ApplicationLocalizer.Language.Translate(66003);
                        //    msg = msg.Replace("%1", MaxSplitNo.ToString());

                        //    DialogResult result = Services.DialogService(entry).ShowMessage(msg, MessageBoxButtons.YesNo, MessageDialogType.Attention);
                        //    if (result == DialogResult.No)
                        //    {
                        //        return;
                        //    }
                        //    splitParam = MaxSplitNo;                            
                        //}

                        functionMode = SplitBillMode.SplitAll;
                        //RunDirectly = true;

                        break;
                    case "-1":
                        splitParam = 1;
                        functionMode = SplitBillMode.SplitMinus;
                        break;
                    case "-?":
                        splitParam = EvaluateParam(entry, Parameter);
                        functionMode = SplitBillMode.SplitMinus;
                        break;
                    case "AMT":
                        decimal totalSelAmt = SelectedItemsTotalAmt(entry, selectedItems);
                        if (totalSelAmt <= 0)
                        {
                            return;
                        }
                        splitParamAmt = EvaluateParam(entry, Parameter, totalSelAmt);
                        splitParamAmtPercent = splitParamAmt / totalSelAmt;
                        splitParam = 2;
                        functionMode = SplitBillMode.SplitAmount;
                        break;
                    case "UNSPLIT":
                        functionMode = SplitBillMode.UnsplitLine;
                        //RunDirectly = true;
                        break;
                    case "UNSPLITALL":
                        DialogResult res = Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.UnsplitAllLinesQuestion, MessageBoxButtons.YesNo, MessageDialogType.Attention); //Are you sure you want to unsplit all lines and move all lines to guest 1?
                        if (res == DialogResult.Yes)
                        {
                            UnSplitAllLines(entry);
                        }
                        return;
                    default:
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ParameterNotImplemented); //Parameter not implemented
                        return;
                }

                if (splitParamAmt == 0M && functionMode == SplitBillMode.SplitAmount)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.SplitOperationIsNotValid, MessageBoxButtons.OK, MessageDialogType.Generic); //Split operation is not valid
                    return;
                }

                if ((splitParam == 0 || splitParam == 1) && (functionMode != SplitBillMode.SplitMinus && functionMode != SplitBillMode.UnsplitLine && functionMode != SplitBillMode.SplitAmount))
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.SplitOperationIsNotValid, MessageBoxButtons.OK, MessageDialogType.Generic); //Split operation is not valid
                    return;
                }

                if (MaxSplitNo != 0 && splitParam > MaxSplitNo)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TooManySplits.Replace("#1", Conversion.ToStr(splitParam)).Replace("#2", Conversion.ToStr(MaxSplitNo)), MessageBoxButtons.OK, MessageDialogType.Generic);
                    return;
                }

                {
                    int transLastLineNo = SplitBillInfo.GetLastLineNo(SplitAction.SplitBill);
                    switch (functionMode)
                    {
                        case SplitBillMode.SplitDivide:
                        case SplitBillMode.SplitAll:
                        case SplitBillMode.SplitMinus:
                            SplitLines(entry, functionMode, splitParam, transLastLineNo, 0, 0);
                            break;
                        case SplitBillMode.SplitAmount:
                            SplitLines(entry, functionMode, splitParam, transLastLineNo, splitParamAmt, splitParamAmtPercent);
                            break;
                        case SplitBillMode.UnsplitLine:
                            UnSplitLines(entry);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }
                
        private void SplitLines(IConnectionManager entry, SplitBillMode FunctionMode, int SplitCount, int TransLastLineNo, decimal PaidAmount, decimal SplitAmountPercentage)
        {
            try
            {                
                int LastUpdLineNo;
                bool CreateSplitLines;
                int SlotNo;
                int ItemCounter = 0;
                int MinusQty = 0;

                List<SaleLineItem> NewTmpPosTransLine = new List<SaleLineItem>();
                List<SaleLineItem> SelTmpPosTransLine = new List<SaleLineItem>();
                List<SaleLineItem> RelTmpPosTransLine = new List<SaleLineItem>();

                List<OriginalItemInfo> OrgItemInfo = new List<OriginalItemInfo>();
                List<OriginalItemInfo> TotalItemInfo = new List<OriginalItemInfo>();
                List<LinkedItemInfo> LinkedItemInfo = new List<LinkedItemInfo>();

                SplitBillInfo.ClearTempInfo();
                ITaxService tax = Interfaces.Services.TaxService(entry);

                //Add the marked items to the Selected and the Rel temporary POS lines
                foreach (SaleLineItem sli in SplitBillInfo.TmpPosTransLine.Where(sli => sli.SplitMarked))
                {
                    //Mark the items as split
                    sli.SplitItem = true;

                    //Add the selected lines with it's original information to SelTmpPosTransLine and OrgItemInfo
                    SplitBillInfo.AddItemToList(sli, SelTmpPosTransLine);
                    ItemCounter++;

                    AddOriginalItemInfo(entry, OrgItemInfo, sli.NetAmountWithTax, sli.Quantity, ItemCounter, true); //Amount, Quantity
                    AddLinkedItemInfo(LinkedItemInfo, sli);

                    //Add the selected line to RelTmpPosTransLine to keep track of the original information
                    SplitBillInfo.AddItemToList(sli, RelTmpPosTransLine);
                }
                                
                if (FunctionMode == SplitBillMode.SplitMinus)
                {
                    MinusQty = SplitCount;
                    SplitCount = 2;
                }

                SplitBillInfo.MaxSplitGroup = 0;
                if (FunctionMode == SplitBillMode.SplitDivide)
                {
                    if (SplitCount > 2)
                    {
                        SplitBillInfo.MaxSplitGroup = SplitBillInfo.SplitGroupNo + SplitCount - 1;
                        SplitBillInfo.CurrentSplitGroup = SplitBillInfo.SplitGroupNo + 2;
                    }
                }

                                
                SlotNo = TransLastLineNo - (TransLastLineNo % 10000);
                LastUpdLineNo = 0;

                //Clear the Total Item information - used to identify rounding problems when dividing items
                TotalItemInfo.Clear();     
           
                ICalculationService calculation = Services.Interfaces.Services.CalculationService(entry);

                //Create splitcount - 1 block at a time
                for (int i = 2; i <= SplitCount; i++) 
                {
                    ItemCounter = 0;

                    //if any items were selected
                    if (SelTmpPosTransLine.Count > 0)
                    {                        
                        foreach (SaleLineItem sli in SelTmpPosTransLine)
                        {
                            ItemCounter++;
                            CreateSplitLines = true;
                            if (FunctionMode == SplitBillMode.SplitMinus)
                            {
                                if (sli.Quantity == 0 || sli.Quantity <= MinusQty)
                                {
                                    if (MoveSplitLineToCurr(entry, sli)) 
                                    {
                                        CreateSplitLines = false;
                                        SaleLineItem toDel = RelTmpPosTransLine.FirstOrDefault(f => f.LineId == sli.LineId);
                                        SplitBillInfo.DeleteItemFromList(toDel, RelTmpPosTransLine);                                        
                                    }
                                }
                            }
                            if (FunctionMode == SplitBillMode.SplitAmount)
                            {
                                if (SplitAmountPercentage >= 1M)
                                {
                                    if (MoveSplitLineToCurr(entry, sli))
                                    {
                                        CreateSplitLines = false;
                                        SaleLineItem toDel = RelTmpPosTransLine.FirstOrDefault(f => f.LineId == sli.LineId);
                                        SplitBillInfo.DeleteItemFromList(toDel, RelTmpPosTransLine);                                        
                                    }
                                }
                            }
                            if (CreateSplitLines)
                            {
                                //Split the current line
                                SaleLineItem NewTmpLine = SplitBillInfo.AddItemToList(sli, NewTmpPosTransLine); 
                                if (NewTmpLine != null)
                                {                                    
                                    //create a new line id
                                    TransLastLineNo = CreateNewLineId(TransLastLineNo, NewTmpLine.LineId, ref SlotNo);

                                    AddSplitLinkedItemInfo(LinkedItemInfo, NewTmpLine, TransLastLineNo);
                                    UpdateLinkedItemInfo(LinkedItemInfo, NewTmpLine, TransLastLineNo);

                                    NewTmpLine.LineId = TransLastLineNo;
                                    
                                    if (FunctionMode == SplitBillMode.SplitAll)
                                    {
                                        //NewTmpLine.CoverNo = i; //TODO: put this line back in after guest functionality is added

                                        //TODO: MBS: take out these lines after guest functionality is added
                                        NewTmpLine.CoverNo = 0;
                                        if (i == 2)
                                        {
                                            NewTmpLine.CoverNo = SplitBillInfo.CurrentGuest;
                                        }                                      
                                        //TODO: MBS: end lines to be deleted after guest functionality is added                                        
                                    }
                                    else
                                    {
                                        //Set the cover number on the new split line
                                        if (i == 2)
                                        {
                                            //Set the current cover number on the new split line
                                            NewTmpLine.CoverNo = SplitBillInfo.CurrentGuest;                                            
                                        }
                                        else
                                        {
                                            //all additional lines created set the cover no to 0
                                            NewTmpLine.CoverNo = 0;
                                        }
                                    }

                                    //Set the splitgroup on the new split line
                                    NewTmpLine.SplitGroup = SplitBillInfo.SplitGroupNo + i - 1;
                                    NewTmpLine.SplitMarked = false;                                   

                                    decimal MovedQty = 0M;
                                    decimal MovedAmt = 0M;
                                    decimal DiscountSplit = 0M;

                                    switch (FunctionMode)
                                    {
                                        case SplitBillMode.SplitDivide:
                                        case SplitBillMode.SplitAll:
                                            MovedQty = NewTmpLine.Quantity / SplitCount; //create the qty for the split line
                                            DiscountSplit = NewTmpLine.Quantity / SplitCount;
                                            break;
                                        case SplitBillMode.SplitMinus:
                                            MovedQty = MinusQty;
                                            DiscountSplit = MinusQty;
                                            break;
                                        case SplitBillMode.SplitAmount:
                                            MovedQty = NewTmpLine.Quantity * SplitAmountPercentage;
                                            DiscountSplit = SplitAmountPercentage;
                                            break;
                                    }

                                    //Set the split qty
                                    bool QtyDiscDiff = NewTmpLine.QuantityDiscounted != 0 ? (NewTmpLine.Quantity != NewTmpLine.QuantityDiscounted) : false;
                                    NewTmpLine.Quantity = MovedQty;
                                    NewTmpLine.UnitQuantity = NewTmpLine.Quantity * NewTmpLine.UnitQuantityFactor;

                                    if (QtyDiscDiff)
                                    {
                                        NewTmpLine.QuantityDiscounted -= DiscountSplit;
                                    }
                                    else 
                                    {
                                        if (NewTmpLine.QuantityDiscounted != 0)
                                        {
                                            NewTmpLine.QuantityDiscounted = MovedQty;
                                        }
                                    }
                                    

                                    //Calculate tne new price, discounts and etc based on new qty                                   
                                    calculation.CalculateLine(entry, NewTmpLine, SplitBillInfo.TableTransaction, false);

                                    MovedAmt = NewTmpLine.NetAmountWithTax;
                                    
                                    //Set the amount and qty being moved
                                    AddOriginalItemInfo(entry, TotalItemInfo, MovedAmt, MovedQty, ItemCounter, true);

                                    if (!LineIsDealItemLine(NewTmpLine))
                                    {
                                        LastUpdLineNo = NewTmpLine.LineId;
                                    }

                                    //original line updated
                                    SaleLineItem RelTmpLine = RelTmpPosTransLine.FirstOrDefault(f => f.LineId == sli.LineId);
                                    if (RelTmpLine != null)
                                    {                                             
                                        RelTmpLine.SplitParentLineId = NewTmpLine.LineId;

                                        //Update the quantity of the original line
                                        RelTmpLine.Quantity = RelTmpLine.Quantity - MovedQty;
                                        RelTmpLine.UnitQuantity = RelTmpLine.Quantity * RelTmpLine.UnitQuantityFactor;

                                        if (RelTmpLine.QuantityDiscounted != 0)
                                        {
                                            RelTmpLine.QuantityDiscounted = RelTmpLine.QuantityDiscounted - DiscountSplit;
                                        }

                                        if (RelTmpLine.SplitOriginLineNo == 0)
                                        {
                                            RelTmpLine.SplitOriginLineNo = RelTmpLine.LineId;
                                        }

                                        //parent line updated
                                        if (sli.SplitParentLineId != 0)
                                        {
                                            if (NewTmpLine.SplitParentLineId != sli.LineId)
                                            {
                                                SaleLineItem parent = RelTmpPosTransLine.FirstOrDefault(f => f.SplitParentLineId == sli.SplitParentLineId);
                                                if (parent != null)
                                                {
                                                    NewTmpLine.SplitParentLineId = parent.SplitParentLineId;
                                                }
                                                else
                                                {
                                                    NewTmpLine.SplitParentLineId = TransLastLineNo;
                                                }
                                            }
                                        }

                                        //if (NewTmpLine.DealLine == true)
                                        //{
                                        //TODO: add deal line functionality when available in LS POS
                                        //}

                                        if (sli.SplitOriginLineNo == 0)
                                        {
                                            NewTmpLine.SplitOriginLineNo = sli.LineId;
                                        }
                                        else
                                        {
                                            NewTmpLine.SplitOriginLineNo = sli.SplitOriginLineNo;
                                        }
                                        
                                    }
                                }
                            }                            
                        }
                    }
                }

                decimal TotalPaidAmt = 0M;                

                foreach (SaleLineItem tmpPosLine in NewTmpPosTransLine)
                {
                    if (!LineIsDealItemLine(tmpPosLine))
                    {
                        TotalPaidAmt += tmpPosLine.NetAmountWithTax;
                    }                    

                    UpdateItemsLinkedItemInfo(LinkedItemInfo, tmpPosLine);

                    if (tmpPosLine.CoverNo > 1)
                    {
                        SplitBillInfo.AddTmpTransLine(tmpPosLine, TmpPOSTransLineType.TmpSplitTransLine);
                        tmpPosLine.SplitMarked = true;
                        SplitBillInfo.FocusSplitLineId.Add(tmpPosLine.LineId);                        
                    }
                    else
                    {
                        SplitBillInfo.AddTmpTransLine(tmpPosLine, TmpPOSTransLineType.TmpPosTransLine);                        
                    }
                }                
                
                //update SPLITAMT
                if (FunctionMode == SplitBillMode.SplitAmount && SplitAmountPercentage < 1M)
                {
                    #region Split Amount

                    decimal PaidAmtDiff = PaidAmount - TotalPaidAmt;
                    if (PaidAmtDiff != 0M)
                    {
                        SaleLineItem newTmpPos = NewTmpPosTransLine.FirstOrDefault(f => f.LineId == LastUpdLineNo);
                        if (newTmpPos != null)
                        {
                            AddOriginalItemInfo(entry, TotalItemInfo, newTmpPos.NetAmountWithTax * -1, 0, LastUpdLineNo, true);                            
                            decimal OrgPrice = 0M;

                            SaleLineItem splitItem;
                            if (newTmpPos.CoverNo > 1)
                            {
                                splitItem = SplitBillInfo.TmpSplitTransLine.FirstOrDefault(f => f.LineId == newTmpPos.LineId);
                            }
                            else
                            {
                                splitItem = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == newTmpPos.LineId);
                            }

                            if (splitItem != null)
                            {
                                OrgPrice = splitItem.PriceWithTax;
                                splitItem.PriceWithTax = splitItem.PriceWithTax + (PaidAmtDiff / splitItem.Quantity);
                                splitItem.NoPriceCalculation = true;

                                //Calculate the tax on the new price and calculate all other price related variables
                                tax.CalculateTax(entry,SplitBillInfo.TableTransaction, splitItem);
                                calculation.CalculateLine(entry, splitItem, SplitBillInfo.TableTransaction, false);

                                splitItem.OriginalPriceWithTax = OrgPrice;

                                AddOriginalItemInfo(entry, TotalItemInfo, splitItem.NetAmountWithTax, 0, LastUpdLineNo, true);                                
                            }
                        }
                    }
                    #endregion
                }

                //TODO: Update new deal lines

                // Update split lines
                ItemCounter = 0;                
                
                //Go through
                foreach (SaleLineItem relSplitLine in RelTmpPosTransLine)
                {
                    ItemCounter++;
                    SaleLineItem splitLine = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == relSplitLine.LineId);
                    if (splitLine != null)
                    {
                        decimal PaidAmtDiff = 0M;

                        splitLine.Quantity = relSplitLine.Quantity;
                        splitLine.UnitQuantity = relSplitLine.UnitQuantity;
                        splitLine.QuantityDiscounted = relSplitLine.QuantityDiscounted;

                        //Calculate tne new price, discounts and etc based on new qty                        
                        calculation.CalculateLine(entry, splitLine, SplitBillInfo.TableTransaction, false);

                        //Update the original information
                        AddOriginalItemInfo(entry, TotalItemInfo, splitLine.NetAmountWithTax, 0, ItemCounter, true);

                        if (!LineIsDealItemLine(splitLine))
                        {                   
                            //If this is the last line in RelTmpPosTransLine then all items have been
                            //updated with correct prices and the total sums will be correct
                            if (ItemCounter == RelTmpPosTransLine.Count)
                            {
                                SplitBillInfo.CalculateTotalAmts();
                                decimal SplitTotalAmt = SplitBillInfo.TmpSplitTransLine.Sum(s => s.NetAmountWithTax);
                                decimal TableTotalAmt = SplitBillInfo.TmpPosTransLine.Sum(s => s.NetAmountWithTax);
                                decimal totalAmount = SplitBillInfo.TableTotalAmtInclTax + SplitBillInfo.SplitTotalAmtInclTax;

                                PaidAmtDiff = totalAmount - SplitTotalAmt - TableTotalAmt;                                                                                       
                            }
                        }                        
                        
                        //Get information about the original item
                        OriginalItemInfo orgInfo = OrgItemInfo.FirstOrDefault(f => f.ItemCounter == ItemCounter);
                        //Retrieve the total item information just changed
                        OriginalItemInfo totalInfo = TotalItemInfo.FirstOrDefault(f => f.ItemCounter == ItemCounter); 

                        if (orgInfo == null)
                        {
                            orgInfo = new OriginalItemInfo();
                        }

                        decimal AmtDiff = decimal.Zero;
                        if (Math.Abs((orgInfo.ItemAmt - totalInfo.ItemAmt)) == Math.Abs(PaidAmtDiff))                        
                        {
                            AmtDiff = PaidAmtDiff;
                        }
                        else
                        {
                            AmtDiff = orgInfo.ItemAmt - totalInfo.ItemAmt + PaidAmtDiff;
                        }
                        decimal OrgPrice = 0M;

                        if (AmtDiff != 0M && !LineIsDealItemLine(splitLine))
                        {
                            int counter = 0;
                            while (AmtDiff != decimal.Zero)
                            {
                                counter++;
                                //Round of one of the lines if needed
                                OrgPrice = splitLine.PriceWithTax;
                                splitLine.PriceWithTax = splitLine.PriceWithTax + (AmtDiff /* 2 */);
                                splitLine.NoPriceCalculation = true;

                                tax.CalculateTax(entry, SplitBillInfo.TableTransaction, splitLine);
                                calculation.CalculateLine(entry, splitLine, SplitBillInfo.TableTransaction, false);

                                splitLine.OriginalPriceWithTax = OrgPrice;

                                if (ItemCounter == RelTmpPosTransLine.Count)
                                {
                                    SplitBillInfo.CalculateTotalAmts();
                                    decimal SplitTotalAmt = SplitBillInfo.TmpSplitTransLine.Sum(s => s.NetAmountWithTax);
                                    decimal TableTotalAmt = SplitBillInfo.TmpPosTransLine.Sum(s => s.NetAmountWithTax);
                                    decimal totalAmount = SplitBillInfo.TableTotalAmtInclTax + SplitBillInfo.SplitTotalAmtInclTax;

                                    PaidAmtDiff = totalAmount - SplitTotalAmt - TableTotalAmt;                                    
                                }
                                if (PaidAmtDiff == 0M)
                                {
                                    AmtDiff = decimal.Zero;
                                }
                                else
                                {
                                    if (Math.Abs((orgInfo.ItemAmt - totalInfo.ItemAmt)) == Math.Abs(PaidAmtDiff))
                                    {
                                        AmtDiff = PaidAmtDiff;
                                    }
                                    else
                                    {
                                        AmtDiff = orgInfo.ItemAmt - totalInfo.ItemAmt + PaidAmtDiff;
                                    }
                                }

                                if (Math.Abs(AmtDiff) < 0.00M || counter == 25)
                                {
                                    AmtDiff = decimal.Zero;
                                }                                
                            }
                        }

                        splitLine.SplitOriginLineNo = relSplitLine.SplitOriginLineNo;
                        splitLine.SplitMarked = false;
                        splitLine.SplitGroup = SplitBillInfo.SplitGroupNo;                        
                    }
                }

                //TODO: Deal Header code added here
                

                SplitBillInfo.SplitGroupNo += 1;
                
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        #region Evaluate Params

        /// <summary>
        /// Evaluates the parameter and returns the number of splits to perform
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="parameter">The parameter to check</param>
        /// <returns>The number of splits to perform</returns>
        private int EvaluateParam(IConnectionManager entry, string parameter)
        {
            try
            {
                if (parameter == "/2")
                {
                    return 2;
                }

                if (parameter == "/3")
                {
                    return 3;
                }

                if (parameter == "/4")
                {
                    return 4;
                }

                if (parameter == "/?" || parameter == "-?" || parameter == "/?ALL")
                {
                    decimal input = decimal.Zero;
                    string prompt = Properties.Resources.SplitBy;          
                    
                    //Split by parts does NOT allow decimals
                    DialogResult dlgResult = Interfaces.Services.DialogService(entry).NumpadInput((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication), ref input, prompt, Properties.Resources.Amount, false, DecimalSettingEnum.Quantity);


                    //If nothing was entered then return 0
                    if (dlgResult == DialogResult.Cancel || input == decimal.Zero)
                    {
                        return 0;
                    }

                    //If a 0 was entered then return 0
                    int iInput = Convert.ToInt32(input);
                    if (iInput == 0)
                    {
                        return 0;
                    }

                    return iInput;
                }

                if (parameter == "-1" || parameter == "/ALL")
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ParameterNotImplemented); //Parameter not implemented
                    return 1;
                }

            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }

            return 1;
        }

        /// <summary>
        /// Evaluates the parameter and returns the number of splits
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="parameter">The parameter to check</param>
        /// <param name="totalSelAmt">Total amount being split</param>
        /// <returns>Returns the number of splits</returns>
        private decimal EvaluateParam(IConnectionManager entry, string parameter, decimal totalSelAmt)
        {
            try
            {
                if (parameter == "AMT")
                {
                    decimal input = decimal.Zero;

                    //Split by amount allows decimals
                    DialogResult dlgResult = Interfaces.Services.DialogService(entry).NumpadInput((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication), ref input, Properties.Resources.SplitByAmount, Properties.Resources.Amount, true, DecimalSettingEnum.Prices); //Split by amount?

                    if (dlgResult == DialogResult.Cancel || input == decimal.Zero)
                    {
                        return 0M;
                    }

                    decimal dInput = Convert.ToDecimal(input, System.Threading.Thread.CurrentThread.CurrentUICulture);
                    if (dInput > totalSelAmt)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.AmountSelectedIsLargerThenTotal, MessageBoxButtons.OK, MessageDialogType.Generic); //Amount selected is larger then total amount of items. Items cannot be split
                        return 0M;
                    }

                    return dInput;
                }
                return Convert.ToDecimal(EvaluateParam(entry, parameter));
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Calculates the total amount of the selected items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="selectedItems">A list of the selected items</param>
        /// <returns>The total amount</returns>
        private decimal SelectedItemsTotalAmt(IConnectionManager entry, List<int> selectedItems)
        {
            decimal total = 0M;
            try
            {
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    var sli = SplitBillInfo.TableTransaction.GetItem(selectedItems[i]);
                    total += sli.NetAmountWithTax;
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }

            return total;
        }

        #region Linked Item Info
                
        private void UpdateItemsLinkedItemInfo(List<LinkedItemInfo> LinkedItemInfo, SaleLineItem PosTransLineIn)
        {
            LinkedItemInfo linked;
            if (PosTransLineIn.IsLinkedItem || PosTransLineIn.IsInfoCodeItem)
            {                
                linked = LinkedItemInfo.FirstOrDefault(f => f.NewLineId == PosTransLineIn.LineId);
                if (linked != null)
                {
                    PosTransLineIn.LinkedToLineId = linked.NewLinkedLineId;
                    return;
                }

                linked = LinkedItemInfo.FirstOrDefault(f => f.OrgLineId == PosTransLineIn.LineId);
                if (linked != null)
                {
                    PosTransLineIn.LinkedToLineId = linked.OrgLinkedLineId;
                }       
            }
        }

        private void UpdateLinkedItemInfo(List<LinkedItemInfo> LinkedItemInfo, SaleLineItem PosTransLineIn, int TransLastLineNo)
        {
            //If the item is a header line then find all linked items and update the LinkedLineId
            if (PosTransLineIn.IsReferencedByLinkItems)
            {
                foreach (LinkedItemInfo lii in LinkedItemInfo.Where(w => w.OrgLinkedLineId == PosTransLineIn.LineId && w.IsSplitItem && w.NewLineId == 0))
                {
                    lii.NewLinkedLineId = TransLastLineNo;
                }
            }

            //If the item is a linked item then find the information and update the line id
            if (PosTransLineIn.IsInfoCodeItem || PosTransLineIn.IsLinkedItem)
            {
                LinkedItemInfo linked = LinkedItemInfo.FirstOrDefault(f => f.OrgLineId == PosTransLineIn.LineId && f.IsSplitItem && f.NewLineId == 0);
                if (linked != null)
                {                    
                    linked.NewLineId = TransLastLineNo;
                }
            }
        }

        private void AddLinkedItemInfo(List<LinkedItemInfo> LinkedItemInfo, SaleLineItem PosTransLineIn)
        {
            if (PosTransLineIn.IsInfoCodeItem || PosTransLineIn.IsLinkedItem)
            {
                LinkedItemInfo linked = new LinkedItemInfo();
                linked.OrgLineId = PosTransLineIn.LineId;
                linked.OrgLinkedLineId = PosTransLineIn.LinkedToLineId;
                linked.IsInfocodeItem = PosTransLineIn.IsInfoCodeItem;
                linked.IsLinkedItem = PosTransLineIn.IsLinkedItem;
                LinkedItemInfo.Add(linked);
            }
        }

        private void AddSplitLinkedItemInfo(List<LinkedItemInfo> LinkedItemInfo, SaleLineItem PosTransLineIn, int TransLastLineNo)
        {
            List<LinkedItemInfo> tmp = new List<LinkedItemInfo>();
            if (PosTransLineIn.IsReferencedByLinkItems)
            {
                foreach (LinkedItemInfo lii in LinkedItemInfo.Where(w => w.OrgLinkedLineId == PosTransLineIn.LineId && w.IsSplitItem == false))
                {
                    LinkedItemInfo linked = new LinkedItemInfo();
                    linked.OrgLineId = lii.OrgLineId;
                    linked.OrgLinkedLineId = lii.OrgLinkedLineId;
                    linked.IsInfocodeItem = lii.IsInfocodeItem;
                    linked.IsLinkedItem = lii.IsLinkedItem;
                    linked.NewLinkedLineId = TransLastLineNo;
                    linked.NewLineId = 0;
                    linked.IsSplitItem = true;
                    tmp.Add(linked);                    
                }

                foreach (LinkedItemInfo lii in tmp)
                {
                    LinkedItemInfo.Add(lii);
                }
            }            
        }

        #endregion

        #region Original Item Info
        private void AddOriginalItemInfo(IConnectionManager entry, List<OriginalItemInfo> orgInfoList, decimal Amt, decimal Qty, int Counter, bool AddToPrevious)
        {
            try
            {
                OriginalItemInfo org = orgInfoList.FirstOrDefault(f => f.ItemCounter == Counter);

                if (org != null)
                {
                    if (AddToPrevious)
                    {
                        org.ItemAmt += Amt;
                        org.ItemQty += Qty;
                    }
                    else
                    {
                        org.ItemAmt = Amt;
                        org.ItemQty = Qty;
                    }
                }
                else
                {
                    org = new OriginalItemInfo();
                    org.ItemAmt = Amt;
                    org.ItemQty = Qty;
                    org.ItemCounter = Counter;
                    orgInfoList.Add(org);
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
            }
        }

        #endregion

        #region Unimplemented functions

        private bool LineIsDealItemLine(SaleLineItem PosTrLineIn)
        {
            return false; //TODO: Add functionality when LS POS supports deal lines
        }

        #endregion

        private bool MoveSplitLineToCurr(IConnectionManager entry, SaleLineItem PosTrLineIn)
        {
            try
            {
                SaleLineItem tmpPosTransLine = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == PosTrLineIn.LineId);
                if (tmpPosTransLine != null)                
                {
                    SaleLineItem splitSli = SplitBillInfo.AddTmpTransLine(tmpPosTransLine, TmpPOSTransLineType.TmpSplitTransLine);
                    splitSli.SplitMarked = false;                    
                    splitSli.CoverNo = SplitBillInfo.CurrentGuest;                    

                    SplitBillInfo.TmpPosTransLine.Remove(tmpPosTransLine);
                    return true;
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }

            return false;
        }       

        #endregion

        #region Move Items

        /// <summary>
        /// Moves the items between sections
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="MoveToTable">if set to <c>true</c> move to the table section</param>
        public void MoveItems(IConnectionManager entry, bool MoveToTable)
        {
            try
            {
                bool CombineRequested = false;
                bool RequestConfirm = false;

                SplitBillInfo.ClearTempInfo();

                HospitalityType activeHospitalityType = Providers.HospitalityTypeData.Get(entry, entry.CurrentStoreID, SplitBillInfo.SplitTransaction.Hospitality.ActiveHospitalitySalesType);
                if (activeHospitalityType != null)
                {
                    switch (activeHospitalityType.CombineSplitLinesAction)
                    {
                        case HospitalityType.CombineSplitLinesActionEnum.NeverCombine:
                            CombineRequested = false;
                            RequestConfirm = false;
                            break;
                        case HospitalityType.CombineSplitLinesActionEnum.AlwaysCombine:
                            CombineRequested = true;
                            RequestConfirm = false;
                            break;
                        case HospitalityType.CombineSplitLinesActionEnum.CombineOnConfirmation:
                            CombineRequested = false;
                            RequestConfirm = true;
                            break;
                    }
                }
                               

                List<SaleLineItem> WorkTmpPosTransLine = new List<SaleLineItem>();
                int MovedLineNo = 0;                

                if (MoveToTable)
                {
                    //WordTmpPosTransLine used so that the lower foreach can delete from the TmpPosTransLine list
                    foreach (SaleLineItem tmpSplitTransLine in SplitBillInfo.TmpSplitTransLine.Where(w => w.SplitMarked))
                    {
                        SplitBillInfo.AddItemToList(tmpSplitTransLine, WorkTmpPosTransLine);
                    }
                                        
                    foreach (SaleLineItem workTmpPosTransLine in WorkTmpPosTransLine)
                    {
                        MoveLineTo(entry, workTmpPosTransLine, ref RequestConfirm, ref CombineRequested, ref MovedLineNo, true);                        

                        SaleLineItem tmpSplitTransLine = SplitBillInfo.TmpSplitTransLine.FirstOrDefault(f => f.LineId == workTmpPosTransLine.LineId);
                        SplitBillInfo.DeleteTmpPosTransLine(tmpSplitTransLine, TmpPOSTransLineType.TmpSplitTransLine);

                        SplitBillInfo.FocusTableLineId.Add(MovedLineNo);                        
                    }
                }
                else
                {
                    SplitBillInfo.UnMarkSelectedItems(null, true, ButtonAppliesTo.Guest);
                    SplitBillInfo.FocusSplitLineId.Clear();

                    //WordTmpPosTransLine used so that the lower foreach can delete from the TmpPosTransLine list
                    foreach (SaleLineItem tmpPosTransLine in SplitBillInfo.TmpPosTransLine.Where(w=> w.SplitMarked))
                    {
                        SplitBillInfo.AddItemToList(tmpPosTransLine, WorkTmpPosTransLine);
                    }

                    foreach (SaleLineItem workTmpPosTransLine in WorkTmpPosTransLine)
                    {
                        MoveLineTo(entry, workTmpPosTransLine, ref RequestConfirm, ref CombineRequested, ref MovedLineNo, false);                        

                        SaleLineItem tmpPosTransLine = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == workTmpPosTransLine.LineId);
                        SplitBillInfo.DeleteTmpPosTransLine(tmpPosTransLine, TmpPOSTransLineType.TmpPosTransLine);

                        SplitBillInfo.FocusSplitLineId.Add(MovedLineNo);                        
                    }
                    //UpdateAllDeals(true); //TODO: when deal information added
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        private void MarkNextSplitGroup(IConnectionManager entry)
        {
            try
            {
                if (SplitBillInfo.CurrentSplitGroup > 0 && SplitBillInfo.CurrentSplitGroup == SplitBillInfo.MaxSplitGroup)
                {
                    SplitBillInfo.CurrentSplitGroup = 0;
                    return;
                }

                SplitBillInfo.CurrentSplitGroup += 1;
                                
                foreach (SaleLineItem tmpPosTransLine in SplitBillInfo.TmpPosTransLine.Where(w => w.SplitGroup == SplitBillInfo.CurrentSplitGroup))
                {                   
                    SplitBillInfo.FocusTableLineId.Add(tmpPosTransLine.LineId);
                    tmpPosTransLine.SplitMarked = true;
                }                
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }        

        #endregion
        
        #region Move focus - Select Lines

        /// <summary>
        /// Selects a specific line.
        /// </summary>
        /// <param name="LineIds">The line ids selected</param>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        public void SelectSpecificLine(List<int> LineIds, ReceiptItems ReceiptItems, ButtonAppliesTo AppliesTo)
        {
            //Unselect and unmark all items
            ReceiptItems.UnselectAll();            
            SplitBillInfo.UnMarkSelectedItems(null, true, AppliesTo);
            
            //Select the line id            
            ReceiptItems.SelectSpecificLine(LineIds);            

            MarkAndSelectLinkedItems(LineIds, null, ReceiptItems, true, ClickAction.Select, AppliesTo);                     
        }

        /// <summary>
        /// Selects the first line
        /// </summary>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="e">The event arguments</param>
        public void FirstLine(ReceiptItems ReceiptItems, OperationBtnArgs e)
        {
            //Unselect and unmark all items
            ReceiptItems.UnselectAll();
            SplitBillInfo.UnMarkSelectedItems(null, true, e.AppliesTo);

            //Set the receipt items first line as selected/unselected
            ReceiptItems.MoveToFirstLine();

            //Toggle, Mark and select linked items (if any)
            ToggleMarkAndSelectLinkedItems(ReceiptItems.SelectedLine.SelectedLineId, ReceiptItems, e.AppliesTo);            
        }

        /// <summary>
        /// Selects the last line
        /// </summary>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="e">The event arguments</param>
        public void LastLine(ReceiptItems ReceiptItems, OperationBtnArgs e)
        {
            //Unselect and unmark all items
            ReceiptItems.UnselectAll();
            SplitBillInfo.UnMarkSelectedItems(null, true, e.AppliesTo);
            
            //Move the focus to the last line
            ReceiptItems.MoveToLastLine();

            //Toggle, Mark and select linked items (if any)
            ToggleMarkAndSelectLinkedItems(ReceiptItems.SelectedLine.SelectedLineId, ReceiptItems, e.AppliesTo);            
        }

        /// <summary>
        /// Moves the selection one line up
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ReceiptItems">The receipt item control</param>
        /// <param name="e">The event arguments</param>
        public void LineUp(IConnectionManager entry, ReceiptItems ReceiptItems, OperationBtnArgs e)
        {
            List<int> selectedItems = ReceiptItems.ReturnSelectedItems();
            if (selectedItems.Count == 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoItemsSelected); //No items selected
                return;
            }  

            //Select the previous item
            int SelectedLine = ReceiptItems.SelectPrevious();

            //Unselect and unmark all items
            ReceiptItems.UnselectAll();
            SplitBillInfo.UnMarkSelectedItems(null, true, e.AppliesTo);

            bool Continue = true;

            SaleLineItem first = SplitBillInfo.ApplicableTmpPosLines(e.AppliesTo).FirstOrDefault();
            if (first != null)
            {
                if (first.LineId == SelectedLine && first.SplitMarked)
                {
                    Continue = false;
                }
            }            

            if (Continue)
            {
                //Toggle, Mark and select linked items (if any)
                ToggleMarkAndSelectLinkedItems(SelectedLine, ReceiptItems, e.AppliesTo);
            }
            
        }

        /// <summary>
        /// Moves the selection one line down
        /// </summary>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="e">The event arguments</param>
        public void LineDown(IConnectionManager entry, ReceiptItems ReceiptItems, OperationBtnArgs e)
        {
            List<int> selectedItems = ReceiptItems.ReturnSelectedItems();
            if (selectedItems.Count == 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoItemsSelected); //No items selected
                return;
            }   

            //Select the next line down
            int SelectedLine = ReceiptItems.SelectNext();

            //Unselect and unmark all items
            ReceiptItems.UnselectAll();
            SplitBillInfo.UnMarkSelectedItems(null, true, e.AppliesTo);

            bool Continue = true;

            SaleLineItem last = SplitBillInfo.ApplicableTmpPosLines(e.AppliesTo).LastOrDefault();
            if (last != null)
            {
                if (last.LineId == SelectedLine && last.SplitMarked)
                {
                    Continue = false;
                }
            }

            if (Continue)
            {
                //Toggle, Mark and select linked items (if any)
                ToggleMarkAndSelectLinkedItems(SelectedLine, ReceiptItems, e.AppliesTo);
            }
        }

        /// <summary>
        /// Moves the selection one page down
        /// </summary>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        public void PageDown(ReceiptItems ReceiptItems, ButtonAppliesTo AppliesTo)
        {            
            int SelectedLine = ReceiptItems.PageDown();

            //Toggle, Mark and select linked items (if any)
            ToggleMarkAndSelectLinkedItems(SelectedLine, ReceiptItems, AppliesTo);
        }

        /// <summary>
        /// Moves the selection one page up
        /// </summary>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        public void PageUp(ReceiptItems ReceiptItems, ButtonAppliesTo AppliesTo)
        {
            int SelectedLine = ReceiptItems.PageUp();

            //Toggle, Mark and select linked items (if any)
            ToggleMarkAndSelectLinkedItems(SelectedLine, ReceiptItems, AppliesTo);
        }

        /// <summary>
        /// Toggles the marked and selected linked items.
        /// </summary>
        /// <param name="SelectedLineId">The selected line id.</param>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        private void ToggleMarkAndSelectLinkedItems(int SelectedLineId, ReceiptItems ReceiptItems, ButtonAppliesTo AppliesTo)
        {
            //Get the item that was selected
            SaleLineItem selected = SplitBillInfo.GetItem(SelectedLineId, AppliesTo);

            if (selected == null) 
                return;

            //Toggle the item
            ClickAction Action = ReceiptItems.ToggleSelect();

            List<int> LineIds = new List<int>();
            LineIds.Add(SelectedLineId);
            MarkAndSelectLinkedItems(LineIds, selected, ReceiptItems, (Action == ClickAction.Select), Action, AppliesTo);
        }

        /// <summary>
        /// Marks and selects linked items.
        /// </summary>
        /// <param name="SelectedLineIds">The selected line ids.</param>
        /// <param name="PosTransLineIn">The selected line information</param>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="SplitMarked">if set to <c>true</c> then mark the split.</param>
        /// <param name="Action">The action being done</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        private void MarkAndSelectLinkedItems(List<int> SelectedLineIds, SaleLineItem PosTransLineIn, ReceiptItems ReceiptItems, bool SplitMarked, ClickAction Action, ButtonAppliesTo AppliesTo)
        {            
            foreach (int i in SelectedLineIds)
            {
                if (PosTransLineIn == null)
                {
                    //Get the item that was selected
                    PosTransLineIn = SplitBillInfo.GetItem(i, AppliesTo);
                }

                if (PosTransLineIn != null)
                {
                    PosTransLineIn.SplitMarked = SplitMarked;

                    //If the item either has linked item or is a linked item - then select the entire range
                    if (PosTransLineIn.IsReferencedByLinkItems || PosTransLineIn.IsLinkedItem || PosTransLineIn.IsInfoCodeItem)
                    {
                        SelectLinkedItems(new List<int>(), PosTransLineIn.LineId, Action, AppliesTo, ReceiptItems);                        
                    }

                    PosTransLineIn = null;
                }
            }
        }

        /// <summary>
        /// Selects the linked items.
        /// </summary>
        /// <param name="Selected">The selected line ids</param>
        /// <param name="LineId">The line id.</param>
        /// <param name="Action">The action being done.</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        /// <param name="ReceiptItems">The receipt items controls</param>
        public void SelectLinkedItems(List<int> Selected, int LineId, ClickAction Action, ButtonAppliesTo AppliesTo, ReceiptItems ReceiptItems)
        {
            int LinkedLineId = LineId;
            //Get the item that was selected
            SaleLineItem sli = SplitBillInfo.ApplicableTmpPosLines(AppliesTo).FirstOrDefault(f => f.LineId == LineId);
            if (sli != null)
            {
                //If it is a linked item get the header item
                if (sli.IsInfoCodeItem || sli.IsLinkedItem)
                {
                    LinkedLineId = sli.LinkedToLineId;
                    sli = SplitBillInfo.ApplicableTmpPosLines(AppliesTo).FirstOrDefault(f => f.LineId == LinkedLineId);
                }

                //If the header item exists (which it always should..)
                if (sli != null && sli.IsReferencedByLinkItems)
                {
                    //Add it to the selected list
                    Selected.Add(sli.LineId);
                    //Find all the items linked to the header line and add them to the selected list
                    foreach (SaleLineItem linked in SplitBillInfo.ApplicableTmpPosLines(AppliesTo).Where(w => (w.IsLinkedItem == true || w.IsInfoCodeItem == true) && w.LinkedToLineId == LinkedLineId))
                    {
                        Selected.Add(linked.LineId);
                    }
                }
            }

            //Select/Unselect all the linked items together
            if (Selected != null && Action == ClickAction.Select)
            {
                SplitBillInfo.MarkSelectedItems(Selected, AppliesTo);
            }
            else if (Selected != null && Action == ClickAction.Unselect)
            {
                SplitBillInfo.UnMarkSelectedItems(Selected, false, AppliesTo);
            }

            //If only one line needs to be selected then SelectRange doesn't need to be called
            if (Selected.Count > 1)
            {
                ReceiptItems.SelectRange(Selected, Action);                
            }
        }

        /// <summary>
        /// Marks all items.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        public void MarkAllItems(IConnectionManager entry, ReceiptItems ReceiptItems, ButtonAppliesTo AppliesTo)
        {
            try
            {
                ReceiptItems.SelectAll();
                MarkSelectedItems(entry, ReceiptItems, AppliesTo, null, ClickAction.Select);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Marks the selected items.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        /// <param name="SelectedItems">The selected items.</param>
        /// <param name="Action">The action being done</param>
        private void MarkSelectedItems(IConnectionManager entry, ReceiptItems ReceiptItems, ButtonAppliesTo AppliesTo, List<int> SelectedItems, ClickAction Action)
        {
            try
            {
                if (SelectedItems == null)
                {
                    SelectedItems = ReceiptItems.ReturnSelectedItems();                    
                }

                if (SelectedItems != null && Action == ClickAction.Select)
                {
                    SplitBillInfo.MarkSelectedItems(SelectedItems, AppliesTo);
                }
                else if (SelectedItems != null && Action == ClickAction.Unselect)
                {
                    SplitBillInfo.UnMarkSelectedItems(SelectedItems, false, AppliesTo);
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Marks the split group.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ReceiptItems">The receipt items control</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        /// <param name="SelectedLineId">The selected line id.</param>
        public void MarkSplitGroup(IConnectionManager entry, ReceiptItems ReceiptItems, ButtonAppliesTo AppliesTo, int SelectedLineId)
        {
            try
            {
                List<int> selectedItems = ReceiptItems.ReturnSelectedItems();
                if (selectedItems.Count == 0)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoItemsSelected); //No items selected
                    return;
                }   

                SaleLineItem selectedLine = SplitBillInfo.GetItem(SelectedLineId, AppliesTo);
                if (selectedLine != null)
                {
                    SplitBillInfo.UnMarkSelectedItems(null, true, AppliesTo);

                    int splitGroup = selectedLine.SplitGroup;

                    List<int> SelectedIdx = new List<int>();
                    foreach (SaleLineItem sli in SplitBillInfo.ApplicableTmpPosLines(AppliesTo).Where(sli => sli.SplitGroup == splitGroup))
                    {                        
                        SelectedIdx.Add(sli.LineId);
                    }

                    SplitBillInfo.MarkSelectedItems(SelectedIdx, AppliesTo);
                    ReceiptItems.UnselectAll();
                    ReceiptItems.SelectRange(SelectedIdx, ClickAction.Select);                    
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Un the marks all lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        public void UnMarkAll(IConnectionManager entry, ButtonAppliesTo AppliesTo)
        {
            try
            {
                SplitBillInfo.UnMarkSelectedItems(null, true, AppliesTo);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        #endregion

        #region Save Split

        

        #endregion

        #region Pay Guest

       /* /// <summary>
        /// Pays the guest.
        /// </summary>
        /// <param name="AppliesTo">Which section does the operation apply to</param>
        /// <param name="DataAreaId">The data area id.</param>
        /// <returns>System.Int32.</returns>
        public int PayGuest(ButtonAppliesTo AppliesTo, string DataAreaId)
        {
            try
            {
                if (SplitBillInfo.ApplicableTmpPosLines(AppliesTo).Count == 0)
                {
                    Services.DialogService(entry).ShowMessage(66002); //No items selected
                    return -1;
                }

                UpdateTransactions(SplitAction.SplitBill);

                if (AppliesTo == ButtonAppliesTo.Guest)
                {
                    //Save the table transaction as the guest transaction will be returned to the POS
                    SaveSplitTransaction(ButtonAppliesTo.Table, 0, DataAreaId);

                    //Save the guest transaction and return the hospitality transaction id
                    SaveSplitTransaction(AppliesTo, SplitBillInfo.CurrentGuest, DataAreaId);

                    return SplitBillInfo.CurrentGuest;
                }

                if (AppliesTo == ButtonAppliesTo.Table)
                {
                    //Save the guest transaction as the table transaction will be returned to the POS
                    SaveSplitTransaction(ButtonAppliesTo.Guest, SplitBillInfo.CurrentGuest, DataAreaId);

                    //Save the guest transaction and return the hospitality transaction id
                    SaveSplitTransaction(AppliesTo, 0, DataAreaId);

                    return 0;
                }

                return 0;
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }*/

        #endregion

        #region Transfer

        /// <summary>
        /// Transfers a line between tables
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="Parameter">The parameter that applies to the operation</param>
        /// <param name="TransLastLineNo">The last line number</param>
        public void TransferLine(IConnectionManager entry, string Parameter, int TransLastLineNo)
        {
            try
            {
                if (Parameter == "")
                {
                    TransferLine(entry, TransLastLineNo, true);
                }
                else if (Parameter.ToUpper() == "BACK")
                {
                    TransferLinesBack(entry, true);
                }                
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Transfers all lines from one table to another
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="Parameter">The parameter that applies to the operation</param>
        /// <param name="DataAreaId">The data area id.</param>
        public void TransferTable(IConnectionManager entry, string Parameter, string DataAreaId)
        {
            try
            {
                if (Parameter.ToUpper() == "CANCEL_ALL")
                {
                    CancelAllTransfers(entry);
                }
                else
                {
                    SplitBillInfo.UnMarkSelectedItems(null, true, ButtonAppliesTo.Table);
                    foreach (SaleLineItem tmpTransLine in SplitBillInfo.TmpPosTransLine)
                    {
                        tmpTransLine.SplitMarked = true;
                        TransferLine(entry, SplitBillInfo.GetLastLineNo(SplitAction.TransferLines), false);                        
                        tmpTransLine.SplitMarked = false;                        
                    }

                    SplitBillInfo.TmpPosTransLine.Clear();

                    if (Parameter.ToUpper() == "OK")
                    {
                        RunSplitBill.SaveSplit(entry, SplitAction.TransferLines, SplitBillInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        private void CancelAllTransfers(IConnectionManager entry)
        {
            try
            {
                SplitBillInfo.TmpSplitTransLine.Clear();
                SplitBillInfo.TmpPosTransLine.Clear();

                foreach (SaleLineItem FromPosTransLine in SplitBillInfo.TableTransaction.SaleItems.Where(w => w.Voided == false && (w.ItemClassType == SalesTransaction.ItemClassTypeEnum.IncomeExpenseItem || w.ItemClassType == SalesTransaction.ItemClassTypeEnum.SaleLineItem)))
                {
                    SaleLineItem addedLine = SplitBillInfo.AddTmpTransLine(FromPosTransLine, TmpPOSTransLineType.TmpPosTransLine);
                    addedLine.SplitMarked = false;
                    addedLine.SplitGroup = 0;
                }

                foreach (SaleLineItem ToPosTransLine in SplitBillInfo.SplitTransaction.SaleItems.Where(w => w.Voided == false && (w.ItemClassType == SalesTransaction.ItemClassTypeEnum.IncomeExpenseItem || w.ItemClassType == SalesTransaction.ItemClassTypeEnum.SaleLineItem)))
                {
                    if (ToPosTransLine.CoverNo > SplitBillInfo.LastGuestNo)
                    {
                        SplitBillInfo.LastGuestNo = ToPosTransLine.CoverNo;
                    }

                    SaleLineItem addedLine = SplitBillInfo.AddTmpTransLine(ToPosTransLine, TmpPOSTransLineType.TmpSplitTransLine);
                    addedLine.SplitMarked = false;
                    addedLine.SplitGroup = 0;
                }

                if (SplitBillInfo.LastGuestNo == 0)
                {
                    SplitBillInfo.LastGuestNo = 1;
                }

            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        private void TransferLinesBack(IConnectionManager entry, bool ResetLastGuestNo)
        {
            try
            {
                int TransLastLineNo = 1;
                if (SplitBillInfo.TmpSplitTransLine.Count > 0)
                {
                    //The items are being moved to the table list
                    SaleLineItem sli = SplitBillInfo.TmpPosTransLine.LastOrDefault();
                    if (sli != null)
                    {
                        TransLastLineNo = sli.LineId;
                    }
                }

                SplitBillInfo.UnMarkSelectedItems(null, true, ButtonAppliesTo.Table);
                SplitBillInfo.FocusTableLineId.Clear();

                List<SaleLineItem> deleteLines = new List<SaleLineItem>();

                int FirstLine = 0;
                int SlotNo = CreateNewLineId(TransLastLineNo);

                foreach (SaleLineItem tmpSplitTrans in SplitBillInfo.TmpSplitTransLine.Where(w => w.SplitMarked))
                {
                    SaleLineItem newTransLine = SplitBillInfo.AddTmpTransLine(tmpSplitTrans, TmpPOSTransLineType.TmpPosTransLine);

                    if (newTransLine != null)
                    {
                        if (newTransLine.Description.Contains("*") == false)
                        {
                            newTransLine.Description = "*" + newTransLine.Description;
                        }                        

                        TransLastLineNo = CreateNewLineId(TransLastLineNo, newTransLine.LineId, ref SlotNo);
                        newTransLine.LineId = TransLastLineNo;

                        int Offset = TransLastLineNo - tmpSplitTrans.LineId;

                        if (newTransLine.SplitParentLineId != 0)
                        {
                            newTransLine.SplitParentLineId = newTransLine.SplitParentLineId + Offset;
                        }

                        SplitBillInfo.FocusTableLineId.Add(TransLastLineNo);
                        newTransLine.SplitGroup = SplitBillInfo.CurrentSplitGroup;
                        newTransLine.SplitMarked = true;


                        if (FirstLine == 0)
                        {
                            FirstLine = newTransLine.LineId;
                        }
                    }                    

                    SplitBillInfo.AddItemToList(tmpSplitTrans, deleteLines);
                }                

                foreach (SaleLineItem delLines in deleteLines)
                {
                    SaleLineItem toDel = SplitBillInfo.TmpSplitTransLine.FirstOrDefault(f => f.LineId == delLines.LineId);
                    if (toDel != null)
                    {
                        SplitBillInfo.DeleteTmpPosTransLine(toDel, TmpPOSTransLineType.TmpSplitTransLine);
                    }
                }                
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        private void TransferLine(IConnectionManager entry, int TransLastLineNo, bool DeleteOrgLine)
        {
            try
            {
                List<SaleLineItem> NewTmpPosTransLine = new List<SaleLineItem>();
                List<SaleLineItem> SelTmpPosTransLine = new List<SaleLineItem>();

                SplitBillInfo.UnMarkSelectedItems(null, true, ButtonAppliesTo.Guest);
                SplitBillInfo.FocusSplitLineId.Clear();

                foreach (SaleLineItem tmpPosTransLine in SplitBillInfo.TmpPosTransLine.Where(w => w.SplitMarked))
                {
                    SplitBillInfo.AddItemToList(tmpPosTransLine, SelTmpPosTransLine);
                }
                
                SplitBillInfo.CurrentSplitGroup += 1;

                int SlotNo = CreateNewLineId(TransLastLineNo);
                
                foreach (SaleLineItem selTransLine in SelTmpPosTransLine)
                {
                    SaleLineItem newTransLine = SplitBillInfo.AddItemToList(selTransLine, NewTmpPosTransLine);                   

                    TransLastLineNo = CreateNewLineId(TransLastLineNo, newTransLine.LineId, ref SlotNo);
                    newTransLine.LineId = TransLastLineNo;

                    int Offset = TransLastLineNo - selTransLine.LineId;

                    newTransLine.CoverNo = 2;

                    if (newTransLine.Description.Contains("*") == false)
                    {                        
                        newTransLine.Description = "*" + newTransLine.Description;
                    }
                    

                    if (newTransLine.SplitParentLineId != 0)
                    {
                        newTransLine.SplitParentLineId = newTransLine.SplitParentLineId + Offset;
                    }

                    SplitBillInfo.FocusSplitLineId.Add(TransLastLineNo);                    

                    newTransLine.SplitGroup = SplitBillInfo.CurrentSplitGroup;
                    newTransLine.SplitMarked = true;

                    SplitBillInfo.AddTmpTransLine(newTransLine, TmpPOSTransLineType.TmpSplitTransLine);
                    
                    if (DeleteOrgLine)
                    {
                        SaleLineItem tmpTransLine = SplitBillInfo.TmpPosTransLine.FirstOrDefault(f => f.LineId == selTransLine.LineId);
                        if (tmpTransLine != null)
                        {
                            SplitBillInfo.DeleteTmpPosTransLine(tmpTransLine, TmpPOSTransLineType.TmpPosTransLine);
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Used to initalise the new Line Id when splitting and transferring. 
        /// </summary>
        /// <param name="TransLastLineNo">The original line id that will be used to create the subsequent line ids</param>
        /// <returns></returns>
        private int CreateNewLineId(int TransLastLineNo)
        {
            return TransLastLineNo - (TransLastLineNo % 10000);
        }

        private int CreateNewLineId(int TransLastLineNo, int BaseLineId, ref int SlotNo)
        {
            int ModulusValue = BaseLineId % 10000;
            if ((SlotNo + ModulusValue) <= TransLastLineNo)
            {
                SlotNo += 10000;
                return SlotNo + ModulusValue;
            }

            return SlotNo + ModulusValue;
        }        

        #endregion                                

    }
}
