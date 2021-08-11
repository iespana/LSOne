using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Enums;
using LSOne.Controls.EventArguments;
using LSOne.Controls.OperationButtons;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.POS.Processes.EventArguments;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Processes.Enums;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.Services.SplitBill
{
    public partial class SplitBillDialog : TouchBaseForm
    {

        private decimal SizeFactor;
        public SplitInfo SplitBillInfo { get; set; }
        
        private OperationButtons opGrid1;
        private OperationButtons opSplitLines;
        private OperationButtons opWindow;
        private OperationButtons opGuestOperations;
        private SplitAction Action;
        private HospitalityType activeHospitalityType;
        private DiningTable currentlySelectedTable;
        private IRetailTransaction orgFromTransaction;
        private IRetailTransaction orgToTransaction;

        private ReceiptItems itTable;
        private ReceiptItems itSplit;

        private string FooterTable;
        private string FooterSplit;

        private string lookUpID;

        private IConnectionManager dataModel;
        private ISettings settings;

        public HospitalityResult OperationResult { get; set; }

        #region Constructors

        public SplitBillDialog(IConnectionManager entry,
                            decimal SizeFactor, 
                            IRetailTransaction FromTransaction, 
                            IRetailTransaction ToTransaction, 
                            int FromTableId, 
                            int ToTableId, 
                            SplitAction Action, 
                            string lookUpID, 
                            HospitalityType activeHospitalityType,
                            DiningTable currentlySelectedTable)
        {
            dataModel = entry;

            try
            {
                OperationResult = new HospitalityResult();
                this.activeHospitalityType = activeHospitalityType;

                opGrid1 = null;
                opGuestOperations = null;
                opSplitLines = null;
                opWindow = null;

                this.Action = Action;
                this.SizeFactor = SizeFactor;

                this.lookUpID = lookUpID;

                SplitBillInfo = new SplitInfo(entry);

                orgFromTransaction = FromTransaction == null ? null : (RetailTransaction)FromTransaction.Clone();
                orgToTransaction = ToTransaction == null ? null : (RetailTransaction)ToTransaction.Clone();
                if (Action == SplitAction.SplitBill)
                {
                    // We need to get both transactions from the database
                    RecordIdentifier id = new RecordIdentifier(currentlySelectedTable.Transaction.TransactionId,
                                                                currentlySelectedTable.Details.StoreID,
                                                                currentlySelectedTable.Details.TerminalID,
                                                                currentlySelectedTable.Details.TableID,
                                                                -1,
                                                                Services.Interfaces.Services.HospitalityService(entry).GetActiveHospSalesType(entry),
                                                                ((RetailTransaction)currentlySelectedTable.Transaction).SplitID);

                    List<HospitalityTransaction> transactions = TransactionProviders.HospitalityTransactionData.GetList(entry, id);

                    if (((RetailTransaction)(currentlySelectedTable.Transaction)).IsTableTransaction)
                    {
                        FromTransaction = currentlySelectedTable.Transaction as RetailTransaction;

                        if (transactions != null && transactions.Count > 1)
                        {
                            ToTransaction = transactions[1].Transaction;
                        }
                    }
                    else
                    {
                        if (transactions != null && transactions.Count > 1)
                        {
                            // Case 1: we are opening split bill from the POS, and we have both table and guest transaction available       
                            FromTransaction = transactions[0].Transaction;
                            ToTransaction = currentlySelectedTable.Transaction as RetailTransaction;
                        }
                        else if (transactions != null && transactions.Count == 1 && !transactions[0].Transaction.IsTableTransaction)
                        {
                            // Case 2: The table transaction has been paid but the guest transaction is left.
                            //         The user is opening SplitBill from the POS after opening the table from the table view
                            FromTransaction = currentlySelectedTable.Transaction as RetailTransaction;
                            ToTransaction = null;
                        }
                    }
                }

                if (FromTransaction == null)
                {
                    FromTransaction = new RetailTransaction((string)entry.CurrentStoreID, settings.Store.Currency, settings.TaxIncludedInPrice);
                    Interfaces.Services.TransactionService(entry).LoadTransactionStatus(entry, FromTransaction, false, true);
                    SplitBillInfo.TableTransaction = FromTransaction;
                }
                else
                {
                    SplitBillInfo.TableTransaction = FromTransaction;
                }

                if (ToTransaction == null)
                {
                    ToTransaction = (RetailTransaction)SplitBillInfo.TableTransaction.Clone();

                    ToTransaction.SaleItems.Clear();
                    SplitBillInfo.SplitTransaction = ToTransaction;
                }
                else
                {
                    SplitBillInfo.SplitTransaction = ToTransaction;
                }

                SplitBillInfo.TableTransaction.Hospitality.TableInformation.TableID = FromTableId;
                SplitBillInfo.SplitTransaction.Hospitality.TableInformation.TableID = ToTableId;

                InitialiseTransactionLines(SplitBillInfo, Action);

                if (this.SizeFactor > 1M)
                {
                    this.SizeFactor = 1M;
                }

                InitializeComponent();

                SetButtonLayout(Action);
                InitReceiptControls();

                settings = (ISettings)dataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                this.itSplit.SaleTotalTxt = FooterSplit;
                this.itTable.SaleTotalTxt = FooterTable;

                this.itSplit.ButtonsVisible = false;
                this.itTable.ButtonsVisible = false;

                this.itTable.DisplayTotalWithoutTax(settings.Store.DisplayBalanceWithTax);
                this.itSplit.DisplayTotalWithoutTax(settings.Store.DisplayBalanceWithTax);

                //Set the size of the form the same as the main form
                this.Width = (int)(settings.MainFormInfo.MainWindowWidth * SizeFactor);
                this.Height = (int)(settings.MainFormInfo.MainWindowHeight * SizeFactor);

                // Since we change the size manually, we also need to reposition the form

                this.currentlySelectedTable = currentlySelectedTable;
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        private void InitReceiptControls()
        {
            ReceiptControlFactory receiptControlFactory = new ReceiptControlFactory();
            itTable = receiptControlFactory.CreateReceiptItemsControl();
            itTable.Name = "itTable";

            itSplit = receiptControlFactory.CreateReceiptItemsControl();
            itSplit.Name = "itSplit";

            itTable.Margin = new Padding(3, 10, 3, 2);
            itSplit.Margin = new Padding(3, 10, 3, 2);
            tlpForm.Controls.Add(itTable, 1, 0);
            tlpForm.Controls.Add(itSplit, 3, 0);

            itTable.Dock = DockStyle.Fill;
            itTable.ReceiptItemDoubleClick += new ReceiptItems.ReceiptItemsDoubleClickEventHandler(itTable_ReceiptItemDoubleClick);
            itTable.ReceiptItemClick += new ReceiptItems.ReceiptItemsClickEventHandler(itTable_ReceiptItemClick);

            itSplit.Dock = DockStyle.Fill;
            itSplit.ReceiptItemDoubleClick += new ReceiptItems.ReceiptItemsDoubleClickEventHandler(itSplit_ReceiptItemDoubleClick);
            itSplit.ReceiptItemClick += new ReceiptItems.ReceiptItemsClickEventHandler(itSplit_ReceiptItemClick);
        }

        /// <summary>
        /// Constructor that sets the form size and all buttons
        /// </summary>
        /// <param name="sizeFactor">The size of the dialog in relation to the main POS form</param>
        public SplitBillDialog(IConnectionManager entry, decimal SizeFactor, IRetailTransaction TableTransaction, SplitAction Action, string lookUpID, HospitalityType activeHospitalityType, DiningTable currentlySelectedTable)
            : this(entry, SizeFactor, TableTransaction, null, TableTransaction.Hospitality.TableInformation.TableID, TableTransaction.Hospitality.TableInformation.TableID, Action, lookUpID, activeHospitalityType, currentlySelectedTable)
        {
            
        }
        
        private void SplitBillDialog_Load(object sender, EventArgs e)
        {
            try
            {
                itSplit.SetMode(ReceiptItemsViewMode.ItemsSelect);
                itTable.SetMode(ReceiptItemsViewMode.ItemsSelect);

                UpdateReceiptItemData(true, true);
                
                UpdateAmountsLabel();


                // This form should be centered to the parent window, which is the POS main form.
                // For some reason the center position is located outide the main form, so we center it manually
                if (settings.POSApp.POSMainWindow != null)
                {
                    Form mainForm = (Form)settings.POSApp.POSMainWindow;

                    int deltaWidth = mainForm.Width - Width;
                    int deltaHeight = mainForm.Height - Height;

                    int newLocationX = deltaWidth / 2 + mainForm.Location.X;
                    int newLocationY = deltaHeight / 2 + mainForm.Location.Y;
                    
                    Point newLocation = new Point(newLocationX, newLocationY);

                    Location = newLocation;
                }
                
            }
            catch (Exception ex)
            {
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        private void SplitBillDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            itTable.ReceiptItemDoubleClick -= itTable_ReceiptItemDoubleClick;
            itTable.ReceiptItemClick -= itTable_ReceiptItemClick;

            itSplit.ReceiptItemDoubleClick -= itSplit_ReceiptItemDoubleClick;
            itSplit.ReceiptItemClick -= itSplit_ReceiptItemClick;
        }

        #endregion

        #region Update Receipt Items

        private void UpdateReceiptItemData(bool updateTable, bool updateGuest)
        {
            Operations op = new Operations(SplitBillInfo);

            IRoundingService rounding = (IRoundingService)dataModel.Service(ServiceType.RoundingService);

            SplitBillInfo.CalculateTotalAmts();
            
            if (updateTable && SplitBillInfo.TableTransaction != null)
            {                                
                this.itTable.DisplayRTItems(SplitBillInfo.TableTransaction, new LinkedList<ISaleLineItem>(SplitBillInfo.TmpPosTransLine));
                
                this.itTable.SaleTotalAmount = rounding.RoundForDisplay(dataModel, 
                                                    settings.Store.DisplayBalanceWithTax ? SplitBillInfo.TableTotalAmtInclTax : SplitBillInfo.TableTotalAmt, 
                                                    false, true, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);

                if (SplitBillInfo.FocusTableLineId.Count > 0)
                {
                    op.SelectSpecificLine(SplitBillInfo.FocusTableLineId, this.itTable, ButtonAppliesTo.Table);                    
                }
            }

            if (updateGuest && SplitBillInfo.SplitTransaction != null)
            {
                this.itSplit.DisplayRTItems(SplitBillInfo.SplitTransaction, new LinkedList<ISaleLineItem>(SplitBillInfo.TmpSplitTransLine));
                this.itSplit.SaleTotalAmount = rounding.RoundForDisplay(dataModel, 
                                                    settings.Store.DisplayBalanceWithTax ? SplitBillInfo.SplitTotalAmtInclTax : SplitBillInfo.SplitTotalAmt,
                                                    false, true, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);

                if (SplitBillInfo.FocusSplitLineId.Count > 0)
                {
                    op.SelectSpecificLine(SplitBillInfo.FocusSplitLineId, this.itSplit, ButtonAppliesTo.Guest);
                }
            }            
        }

        #endregion

        #region Initialise transactions

        private void InitialiseTransactionLines(SplitInfo SplitBillInfo, SplitAction Action)
        {
            try
            {
                //Make sure that all items in the table transaction are unmarked
                SplitBillInfo.UnMarkSelectedItems(null, true, ButtonAppliesTo.Table);
                SplitBillInfo.UnMarkSelectedItems(null, true, ButtonAppliesTo.Guest);
                                
                SplitBillInfo.ExcludedLinesLastLineNo = 0;
                SplitBillInfo.TmpPosTransLine.Clear();
                SplitBillInfo.TmpSplitTransLine.Clear();

                //Retrieve the footer texts from the Hospitality Type
                RetrieveFooterTexts(Action);

                //If the dialog is in TransferTable mode then do specific configuration for that and exit
                if (Action == SplitAction.TransferLines)
                {
                    InitialiseTransferInformation(SplitBillInfo);
                    return;
                }

                int LineId = 0;

                //Figure out the last Line id of items that area voided in the transaction
                LinkedList<SaleLineItem> SaleItemsFiltered = SplitBillInfo.SetSaleItemFilter(f => f.Voided == true, ButtonAppliesTo.Table, FilterAppliesTo.RetailTransaction);
                if (SaleItemsFiltered.Count > 0)
                {
                    LineId = ((SaleLineItem)(object)SaleItemsFiltered.Last()).LineId;
                    SplitBillInfo.ExcludedLinesLastLineNo = LineId;
                }                

                //Figure out the last line if of items that are of type income/expense or items
                SaleItemsFiltered = SplitBillInfo.SetSaleItemFilter(f => (f.ItemClassType == SalesTransaction.ItemClassTypeEnum.IncomeExpenseItem || f.ItemClassType == SalesTransaction.ItemClassTypeEnum.SaleLineItem), ButtonAppliesTo.Table, FilterAppliesTo.RetailTransaction);
                if (SaleItemsFiltered.Count > 0)
                {
                    LineId = ((SaleLineItem)(object)SaleItemsFiltered.Last()).LineId;
                    if (SplitBillInfo.ExcludedLinesLastLineNo < LineId)
                    {
                        SplitBillInfo.ExcludedLinesLastLineNo = LineId;
                    }
                }

                // Since there are only two guests available, we set the correct cover numbers
                foreach (var saleLineItem in SplitBillInfo.TableTransaction.SaleItems)
                {
                    saleLineItem.CoverNo = 0;
                }

                foreach (var saleLineItem in SplitBillInfo.SplitTransaction.SaleItems)
                {
                    saleLineItem.CoverNo = 2;
                }

                SetTableAndGuestTransactions(SplitBillInfo, SplitBillInfo.TableTransaction.SaleItems);
                SetTableAndGuestTransactions(SplitBillInfo, SplitBillInfo.SplitTransaction.SaleItems);

                //Set the next Split Group value
                SplitBillInfo.SplitGroupNo += 1;
                
            }
            catch (Exception ex)
            {
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        private void InitialiseTransferInformation(SplitInfo SplitBillInfo)
        {
            try
            {
                SplitBillInfo.ExcludedLinesLastLineNo = 0;
                SplitBillInfo.ClearTempInfo();

                foreach (SaleLineItem FromPosTransLine in SplitBillInfo.TableTransaction.SaleItems.Where(w => !w.Voided && (w.ItemClassType == SalesTransaction.ItemClassTypeEnum.IncomeExpenseItem || w.ItemClassType == SalesTransaction.ItemClassTypeEnum.SaleLineItem)))
                {
                    SaleLineItem addedLine = SplitBillInfo.AddTmpTransLine(FromPosTransLine, TmpPOSTransLineType.TmpPosTransLine);
                    addedLine.SplitMarked = false;
                    addedLine.SplitGroup = 0;                    
                }

                foreach (SaleLineItem ToPosTransLine in SplitBillInfo.SplitTransaction.SaleItems.Where(w => !w.Voided && (w.ItemClassType == SalesTransaction.ItemClassTypeEnum.IncomeExpenseItem || w.ItemClassType == SalesTransaction.ItemClassTypeEnum.SaleLineItem)))
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
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        private void SetTableAndGuestTransactions(SplitInfo SplitBillInfo, LinkedList<ISaleLineItem> SaleItemsFiltered)
        {
            try
            {
                bool LineExcluded;                
                foreach (SaleLineItem sli in SaleItemsFiltered.Where(w =>w.ItemClassType == SalesTransaction.ItemClassTypeEnum.IncomeExpenseItem || w.ItemClassType == SalesTransaction.ItemClassTypeEnum.SaleLineItem))
                {
                    //Find the highest SplitGroupNo
                    if (SplitBillInfo.SplitGroupNo < sli.SplitGroup)
                    {
                        SplitBillInfo.SplitGroupNo = sli.SplitGroup;
                    }

                    LineExcluded = false;

                    if (sli.CoverNo == 0 || sli.CoverNo == 1)
                    {
                        sli.SplitMarked = false;
                        sli.SplitLineId = sli.LineId;
                        SplitBillInfo.AddTmpTransLine(sli, TmpPOSTransLineType.TmpPosTransLine);
                    }
                    else
                    {
                        sli.SplitMarked = false;
                        sli.SplitLineId = sli.LineId;
                        SplitBillInfo.AddTmpTransLine(sli, TmpPOSTransLineType.TmpSplitTransLine);
                    }

                    if (LineExcluded) //TODO: statement will only take affect when LS POS supports Deal lines
                    {
                        if (SplitBillInfo.ExcludedLinesLastLineNo < sli.LineId)
                        {
                            SplitBillInfo.ExcludedLinesLastLineNo = sli.LineId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        private void RetrieveFooterTexts(SplitAction Action)
        {
            if (Action == SplitAction.SplitBill)
            {
                FooterSplit = "Guest %1 Total Amt";
                FooterTable = "Guest %1 Total Amt";
            }
        }        

        private void SetButtonLayout(SplitAction Action)
        {
            try
            {
                PosLookup menus = Providers.PosLookupData.Get(dataModel, lookUpID);

                opGrid1 = new OperationButtons(dataModel, tlpGrid1, OperationButtonClick);
                opGrid1.SetOperationButtons(menus.DynamicMenu2ID.ToString());

                opSplitLines = new OperationButtons(dataModel, tlpSplitLines, OperationButtonClick);
                opSplitLines.SetOperationButtons(menus.Grid1MenuID.ToString());

                opWindow = new OperationButtons(dataModel, tlpWindow, OperationButtonClick);
                opWindow.SetOperationButtons(menus.DynamicMenuID.ToString());

                opGuestOperations = new OperationButtons(dataModel, tlpGuestLines, OperationButtonClick);
                opGuestOperations.SetOperationButtons(menus.Grid2MenuID.ToString());
            }
            catch (Exception ex)
            {
                dataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        #endregion

        #region Operations Event Handler

        void OperationButtonClick(object sender, IConnectionManager entry, OperationBtnArgs e)        
        {
            try
            {
                Operations op = new Operations(SplitBillInfo);
                bool updateReceiptItems = true;
                bool updateTableReceipt = false;
                bool updateGuestReceipt = false;

                switch ((HospitalityOperations)(int)e.OperationButtonInfo.Operation)
                {
                    #region Mark operations
                    case HospitalityOperations.MarkAll: 
                       
                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {                            
                            op.MarkAllItems(entry, itTable, e.AppliesTo);
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            op.MarkAllItems(entry, itSplit, e.AppliesTo);                         
                        }                        
                        updateReceiptItems = false;
                        break;

                    case HospitalityOperations.MarkSplit:

                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {
                            int SelectedLineId = itTable.SelectedLine.SelectedLineId;
                            op.MarkSplitGroup(entry, itTable, e.AppliesTo, SelectedLineId);                            

                            itTable.GridViewItems.MakeRowVisible(itTable.ReturnSelectedItems()[0] - 1, false);
                            itTable.GridViewItems.MakeRowVisible(itTable.ReturnSelectedItems().Last() - 1, false);
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            int SelectedLineId = itSplit.SelectedLine.SelectedLineId;
                            op.MarkSplitGroup(entry, itSplit, e.AppliesTo, SelectedLineId);
                        }
                        updateReceiptItems = false;
                        break;

                    case HospitalityOperations.UnmarkAll:
                        op.UnMarkAll(entry,e.AppliesTo);                        
                        if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            if (itSplit.ReturnSelectedItems().Count > 0)
                            {
                                updateGuestReceipt = true;
                            }

                            SplitBillInfo.FocusSplitLineId.Clear();
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Table)
                        {                            
                            if (itTable.ReturnSelectedItems().Count > 0)
                            {
                                updateTableReceipt = true;
                            }

                            SplitBillInfo.FocusTableLineId.Clear();
                        }

                        updateReceiptItems = false;
                        break;

                    case HospitalityOperations.MarkGuest:

                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.OperationNotImplemented); //Operation has not been implemented
                        break;

                    #endregion

                    #region Select Lines operations
                    case HospitalityOperations.FirstLine:
                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {
                            op.FirstLine(itTable, e);
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            op.FirstLine(itSplit, e);
                        }
                        updateReceiptItems = false;
                        break;
                    case HospitalityOperations.LastLine:
                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {
                            op.LastLine(itTable, e);
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            op.LastLine(itSplit, e);
                        }
                        updateReceiptItems = false;
                        break;
                    case HospitalityOperations.LineDown:
                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {
                            op.LineDown(entry, itTable, e);
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            op.LineDown(entry, itSplit, e);
                        }
                        updateReceiptItems = false;
                        break;
                    case HospitalityOperations.LineUp:
                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {
                            op.LineUp(entry, itTable, e);
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            op.LineUp(entry, itSplit, e);
                        }
                        updateReceiptItems = false;
                        break;
                    case HospitalityOperations.PageDown:
                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {
                            op.PageDown(itTable, e.AppliesTo);
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            op.PageDown(itSplit, e.AppliesTo);
                        }
                        updateReceiptItems = false;
                        break;
                    case HospitalityOperations.PageUp:
                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {
                            op.PageUp(itTable, e.AppliesTo);
                        }
                        else if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            op.PageUp(itSplit, e.AppliesTo);
                        }
                        updateReceiptItems = false;
                        break;
                    #endregion

                    #region Window operations - One operation Not Yet Implemented

                    case HospitalityOperations.Cancel:
                        if (Action == SplitAction.SplitBill)
                        {
                            //If split bill is cancelled then use the transactions as they were when the operation was started
                            SplitBillInfo.TableTransaction = orgFromTransaction == null ? null : (RetailTransaction)orgFromTransaction.Clone();
                            SplitBillInfo.SplitTransaction = orgToTransaction == null ? null : (RetailTransaction)orgToTransaction.Clone();

                            currentlySelectedTable.Transaction = orgFromTransaction == null ? null : (RetailTransaction)orgFromTransaction.Clone();
                        }

                        DialogResult = DialogResult.Cancel;
                        OperationResult.OperationResult = HospitalityOperationResult.Cancel;
                        updateReceiptItems = false;
                        Close();                        
                        break;

                    case HospitalityOperations.OK:

                        if (Action == SplitAction.SplitBill)
                        {
                            Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.OperationNotImplemented); //Operation has not been implemented
                            return;
                        }

                        RunSplitBill.SaveSplit(entry, Action, SplitBillInfo);
                        DialogResult = DialogResult.OK;
                        OperationResult.OperationResult = HospitalityOperationResult.SaveAndExit;
                        updateReceiptItems = false;
                        this.Close();
                        break;

                    #endregion

                    #region Split item operations
                    case HospitalityOperations.Split:

                        if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.OperationCannotApplyToGuestGrid); //Operation cannot apply to guest grid
                        }
                        else
                        {                            
                            op.SplitLines(entry, e.OperationButtonInfo.Parameter, itTable, activeHospitalityType.MaxNumberOfSplits);
                            RunSplitBill.UpdateTransactions(entry, SplitAction.SplitBill, SplitBillInfo);
                            SplitBillInfo.CalculateTotalAmts();
                        }

                        break;

                    case HospitalityOperations.MoveItem:

                        op.MoveItems(entry, e.OperationButtonInfo.Parameter == "BACK");
                        RunSplitBill.UpdateTransactions(entry, SplitAction.SplitBill, SplitBillInfo);
                        break;              
      
                    #endregion

                    #region Payment

                    case HospitalityOperations.PayGuest:
                        
                        RunSplitBill.UpdateTransactions(entry, SplitAction.SplitBill, SplitBillInfo);
                            OperationResult.Guest = e.AppliesTo == ButtonAppliesTo.Guest
                                                        ? SplitBillInfo.CurrentGuest
                                                        : 0;
                        
                        if (e.AppliesTo == ButtonAppliesTo.Guest)
                        {
                            if (SplitBillInfo.TmpSplitTransLine.Count == 0)
                            {
                                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoItemsSelected);
                                return;
                            }

                            // Check if we moved all remaining items from table to guest, and move all payment lines so they don't get lost
                            if(SplitBillInfo.TableTransaction.SaleItems.Count == 0 && SplitBillInfo.TableTransaction.TenderLines.Count > 0)
                            {
                                foreach(var tenderLine in SplitBillInfo.TableTransaction.TenderLines.Where(p => !SplitBillInfo.SplitTransaction.TenderLines.Exists(x => x.ID == p.ID)))
                                {                                    
                                    SplitBillInfo.SplitTransaction.Add(tenderLine);
                                }

                                SplitBillInfo.TableTransaction.TenderLines.Clear();
                            }

                            // If all remaining items on the table are voided, include them in the split transaction to be payed.
                            if (SplitBillInfo.TableTransaction.SaleItems.All(i => i.Voided))
                            {
                                SplitBillInfo.TmpPosTransLine.ForEach(l => l.SplitMarked = true);

                                op.MoveItems(entry, false);
                                RunSplitBill.UpdateTransactions(entry, SplitAction.SplitBill, SplitBillInfo);
                            }

                            SplitBillInfo.SplitTransaction.IsTableTransaction = false;
                            RunSplitBill.SaveSplitTransaction(entry, ButtonAppliesTo.Table, 0, SplitBillInfo);

                            RunSplitBill.SaveSplitTransaction(entry, e.AppliesTo, SplitBillInfo.CurrentGuest, SplitBillInfo);
                            SplitBillInfo.CurrentGuest = 2;
                        }

                        if (e.AppliesTo == ButtonAppliesTo.Table)
                        {
                            if (SplitBillInfo.TmpPosTransLine.Count == 0)
                            {
                                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoItemsSelected);
                                return;
                            }
                            
                            // Check if we moved all remaining items from guest to table, and move all payment lines so they don't get lost
                            if(SplitBillInfo.SplitTransaction.SaleItems.Count == 0 && SplitBillInfo.SplitTransaction.TenderLines.Count > 0)
                            {
                                foreach(var tenderLine in SplitBillInfo.SplitTransaction.TenderLines.Where(p => !SplitBillInfo.TableTransaction.TenderLines.Exists(x => x.ID == p.ID)))
                                {
                                    SplitBillInfo.TableTransaction.Add(tenderLine);
                                }

                                SplitBillInfo.SplitTransaction.TenderLines.Clear();
                            }

                            SplitBillInfo.SplitTransaction.IsTableTransaction = false;
                            SplitBillInfo.TableTransaction.IsTableTransaction = true;
                            RunSplitBill.SaveSplitTransaction(entry, ButtonAppliesTo.Guest, SplitBillInfo.CurrentGuest, SplitBillInfo);

                            RunSplitBill.SaveSplitTransaction(entry, e.AppliesTo, 0, SplitBillInfo);
                            SplitBillInfo.CurrentGuest = 0;
                        }
                        
                        if (OperationResult.Guest > -1)
                        {
                            OperationResult.OperationResult = HospitalityOperationResult.Pay;
                            DialogResult = DialogResult.OK;
                            this.Close();                            
                        }
                        updateReceiptItems = false;
                        break;

                    #endregion

                    #region Print Split - Not Yet Implemented

                    case HospitalityOperations.PrintSplit:
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.OperationNotImplemented); //Operation has not been implemented
                        updateReceiptItems = false;
                        break;

                    #endregion

                    #region Guest operations - Not Yet Implemented

                    case HospitalityOperations.Guest:
                    case HospitalityOperations.NextGuest:
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.OperationNotImplemented); //Operation has not been implemented
                        updateReceiptItems = false;
                        break;

                    #endregion

                    #region Menu operations - Not Yet Implemented

                    case HospitalityOperations.Submenu:
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.OperationNotImplemented); //Operation has not been implemented
                        updateReceiptItems = false;
                        break;

                    #endregion

                    #region Transfer table
                    case HospitalityOperations.TransferLine:

                        op.TransferLine(entry, e.OperationButtonInfo.Parameter, SplitBillInfo.GetLastLineNo(SplitAction.TransferLines));
                        break;

                    case HospitalityOperations.TransferTable:

                        op.TransferTable(entry, e.OperationButtonInfo.Parameter, entry.Connection.DataAreaId);
                        if (e.OperationButtonInfo.Parameter.ToUpper() == "OK")
                        {                            
                            DialogResult = DialogResult.OK;
                            OperationResult.OperationResult = HospitalityOperationResult.SaveAndExit;
                            updateReceiptItems = false;
                            Close();
                            break;
                        }
                        break;

                    #endregion

                    case HospitalityOperations.None:
                        updateReceiptItems = false;
                        break;

                    default:
                        Interfaces.Services.DialogService(dataModel).ShowMessage(Properties.Resources.OperationNotImplemented); //Operation has not been implemented
                        updateReceiptItems = false;
                        break;
                }

                UpdateReceiptItemData(updateReceiptItems || updateTableReceipt, updateReceiptItems || updateGuestReceipt);
            }
            
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        #endregion

        #region Receipt Item Event Handlers

        private void itTable_ReceiptItemDoubleClick(object sender, ReceiptItemClickArgs e)
        {
            Operations op = new Operations(SplitBillInfo);
            op.MarkSplitGroup(dataModel, itTable, ButtonAppliesTo.Table, e.SelectedLine.SelectedLineId);
        }

        private void itTable_ReceiptItemClick(object sender, ReceiptItemClickArgs e)
        {
            ReceiptItemClick(e.SelectedLine.SelectedLineId, ButtonAppliesTo.Table, e.Action);
        }

        private void itSplit_ReceiptItemDoubleClick(object sender, ReceiptItemClickArgs e)
        {
            Operations op = new Operations(SplitBillInfo);
            op.MarkSplitGroup(dataModel, itSplit, ButtonAppliesTo.Guest, e.SelectedLine.SelectedLineId);
        }

        private void itSplit_ReceiptItemClick(object sender, ReceiptItemClickArgs e)
        {   
            ReceiptItemClick(e.SelectedLine.SelectedLineId, ButtonAppliesTo.Guest, e.Action);
        }

        private void ReceiptItemClick(int LineId, ButtonAppliesTo AppliesTo, ClickAction Action)
        {
            List<int> selected = new List<int>();
            selected.Add(LineId);

            Operations op = new Operations(SplitBillInfo);
            if (AppliesTo == ButtonAppliesTo.Table)
            {
                op.SelectLinkedItems(selected, LineId, Action, AppliesTo, itTable);
            }
            else if (AppliesTo == ButtonAppliesTo.Guest)
            {
                op.SelectLinkedItems(selected, LineId, Action, AppliesTo, itSplit);
            }
        }        

        #endregion   
     
        private void UpdateAmountsLabel()
        {
            if (Action == SplitAction.SplitBill)
            {
                UpdateReceiptItemData(true, true);
                SplitBillInfo.CalculateTotalAmts();

                decimal totalWithTax = SplitBillInfo.TableTotalAmtInclTax + SplitBillInfo.SplitTotalAmtInclTax;
                decimal totalWithoutTax = SplitBillInfo.TableTotalAmt + SplitBillInfo.SplitTotalAmt;

                touchDialogBanner.BannerText = Properties.Resources.SplitBill + ": " +
                    Properties.Resources.TableWithDescription.Replace("%1", SplitBillInfo.TableTransaction.Hospitality.TableInformation.TableID.ToString());

                string amt = Interfaces.Services.RoundingService(dataModel).RoundForDisplay(
                    dataModel,
                    settings.Store.DisplayBalanceWithTax ? totalWithTax : totalWithoutTax,
                    true, false,
                    settings.Store.Currency,
                    CacheType.CacheTypeTransactionLifeTime);

                lblTableBalanceValue.Text = amt;
            }
            else if (Action == SplitAction.TransferLines)
            {
                string fromAmount = Interfaces.Services.RoundingService(dataModel).RoundForDisplay(
                    dataModel,
                    settings.Store.DisplayBalanceWithTax ? SplitBillInfo.TableTotalAmtInclTax : SplitBillInfo.TableTotalAmt,
                    true, false,
                    settings.Store.Currency,
                    CacheType.CacheTypeTransactionLifeTime);

                string toAmount = Interfaces.Services.RoundingService(dataModel).RoundForDisplay(
                    dataModel,
                    settings.Store.DisplayBalanceWithTax ? SplitBillInfo.SplitTotalAmtInclTax : SplitBillInfo.SplitTotalAmt,
                    false, false,
                    settings.Store.Currency,
                    CacheType.CacheTypeTransactionLifeTime);

                string text = Properties.Resources.TransferFromTable;
                text = text.Replace("%1", SplitBillInfo.TableTransaction.Hospitality.TableInformation.TableID.ToString());
                text = text.Replace("%2", fromAmount);
                text = text.Replace("%3", SplitBillInfo.SplitTransaction.Hospitality.TableInformation.TableID.ToString());
                text = text.Replace("%4", toAmount);

                touchDialogBanner.BannerText = text;

                lblTableBalance.Visible = lblTableBalanceValue.Visible = false;
            }
        }
    }
}
