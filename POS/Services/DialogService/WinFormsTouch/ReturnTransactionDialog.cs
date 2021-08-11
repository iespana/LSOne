using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Services.Interfaces.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.DataLayer.DataProviders;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.POS.Processes.Enums;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.WinFormsTouch
{
    public partial class ReturnTransactionDialog : TouchBaseForm
    {
        private IPosTransaction posTransaction;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private bool displayReasonCodes;
        private List<ReasonCode> reasonCodes;
        private ReturnTransactionDialogBehaviourEnum behaviour;
        private string defaultReasonCodeID;
        private ReceiptItems receipt;
        private const string OKButtonID = "OK";

        public LinkedList<ReturnedItemReason> ReturnedItems { get; private set; }

        private enum Buttons
        {
            SelectLine,
            SelectAll,
            ClearSelection,
            ReasonCodes
        }

        public ReturnTransactionDialog(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            displayReasonCodes = false;

            if (!DesignMode)
            {
                ReturnedItems = new LinkedList<ReturnedItemReason>();
                receipt = new ReceiptControlFactory().CreateReceiptItemsControl(pnlReceipt);
                if (DLLEntry.Settings.FunctionalityProfile.DisplayItemIDInReturnDialog)
                {
                    receipt.DisplayItemID(true);
                }
            }

            receipt.ReceiptItemClick += Receipt_ReceiptItemClick;
        }
        
        public ReturnTransactionDialog(IPosTransaction transaction, IConnectionManager entry, ReturnTransactionDialogBehaviourEnum behaviour, string defaultReasonCodeID)
            : this(entry)
        {
            posTransaction = transaction;
            this.behaviour = behaviour;
            this.defaultReasonCodeID = defaultReasonCodeID;

            touchDialogBanner1.BannerText = behaviour == ReturnTransactionDialogBehaviourEnum.ReturnTransaction
                                ? Properties.Resources.HeaderReturnItems
                                : Properties.Resources.HeaderSetReasonCode;

            receipt.DisplayReasonCode(true);



            //If the store doesn't display balance with tax then the journal doesn't display all the amounts correctly
            //because the user cannot add columns to display the amounts that are missing from the default configuration of the receipt
            //So we display both total columns and the sales balance with tax
            if (!DLLEntry.Settings.Store.DisplayBalanceWithTax)
            {
                receipt.DisplayTotalWithAndWithoutTax(true);
                receipt.DisplaySalesBalanceWithTax(true);
            }

            InitializeButtons(false);
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            receipt.SetMode(ReceiptItemsViewMode.ItemsSelect);
            receipt.LookAndFeel.SkinName = "Light2";
            receipt.GridViewItems.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            if (posTransaction is RetailTransaction)
            {
                if(behaviour == ReturnTransactionDialogBehaviourEnum.ReturnTransaction)
                {
                    receipt.DisplayRTItems((PosTransaction)posTransaction);
                    ReasonCode defaultReasonCode = null;

                    try
                    {
                        if (!string.IsNullOrEmpty(defaultReasonCodeID))
                        {
                            defaultReasonCode = Providers.ReasonCodeData.GetReasonById(dlgEntry, defaultReasonCodeID);

                            if (defaultReasonCode == null)
                            {
                                Action getReasonCodesAction = () =>
                                {
                                    defaultReasonCode = Services.Interfaces.Services.InventoryService(dlgEntry).GetReasonById(dlgEntry, dlgSettings.SiteServiceProfile, defaultReasonCodeID, true);
                                };

                                Exception ex = new Exception();
                                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(getReasonCodesAction, Properties.Resources.PleaseWait, Properties.Resources.GettingReasonCodes, out ex);
                            }
                        }
                    }
                    catch (Exception) { }

                    try
                    {
                        if (defaultReasonCode == null && dlgSettings.Store.ReturnReasonCodeID.StringValue != string.Empty)
                        {
                            defaultReasonCode = Providers.ReasonCodeData.GetReasonById(dlgEntry, dlgSettings.Store.ReturnReasonCodeID);

                            if (defaultReasonCode == null)
                            {
                                Action getReasonCodesAction = () =>
                                {
                                    defaultReasonCode = Services.Interfaces.Services.InventoryService(dlgEntry).GetReasonById(dlgEntry, dlgSettings.SiteServiceProfile, dlgSettings.Store.ReturnReasonCodeID, true);
                                };

                                Exception ex = new Exception();
                                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(getReasonCodesAction, Properties.Resources.PleaseWait, Properties.Resources.GettingReasonCodes, out ex);
                            }
                        }
                    }
                    catch (Exception) { }

                    if (defaultReasonCode == null) //If we get here, we try to get the first reason code with action Main inventory
                    {
                        List<ReasonCode> availableCodes = Providers.ReasonCodeData.GetList(dlgEntry, new List<ReasonActionEnum> { ReasonActionEnum.MainInventory }, true, true);

                        if (availableCodes.Count == 0)
                        {
                            try
                            {
                                Action getReasonCodesAction = () =>
                                {
                                    availableCodes = Services.Interfaces.Services.InventoryService(dlgEntry).GetReasonCodesList(dlgEntry, dlgSettings.SiteServiceProfile, true, new List<ReasonActionEnum> { ReasonActionEnum.MainInventory }, true, true);
                                };

                                Exception ex = new Exception();
                                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(getReasonCodesAction, Properties.Resources.PleaseWait, Properties.Resources.GettingReasonCodes, out ex);
                            }
                            catch (Exception) { }
                        }

                        if (availableCodes.Count > 0)
                        {
                            defaultReasonCode = availableCodes[0];
                        }
                    }

                    if (defaultReasonCode != null)
                    {
                        SetReasonCode(defaultReasonCode, true);
                    }
                }
                else
                {
                    receipt.DisplayRTItems((PosTransaction)posTransaction, 
                        new LinkedList<ISaleLineItem>(((RetailTransaction)posTransaction).SaleItems.Where(x => x.Quantity < 0
                                                                                                            && !x.Voided
                                                                                                            && !x.QtyBecomesNegative
                                                                                                            && x.ItemClassType != SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem
                                                                                                            && x.ItemClassType != SalesTransaction.ItemClassTypeEnum.FuelSalesLineItem
                                                                                                            && x.GetType() != typeof(GiftCertificateItem)
                                                                                                            && x.GetType() != typeof(IncomeExpenseItem)
                                                                                                            && x.Returnable)));
                }

                try
                {
                    reasonCodes = Providers.ReasonCodeData.GetReasonCodesForReturn(dlgEntry);

                    if (!reasonCodes.Any())
                    {
                        Action getReasonCodesAction = () =>
                        {
                            reasonCodes = Services.Interfaces.Services.InventoryService(dlgEntry).GetReasonCodesForReturn(dlgEntry, dlgSettings.SiteServiceProfile, true);
                        };

                        Exception ex = new Exception();
                        Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(getReasonCodesAction, Properties.Resources.PleaseWait, Properties.Resources.GettingReasonCodes, out ex);

                        if(ex != null)
                        {
                            throw ex;
                        }
                    }
                }
                catch(Exception ex)
                {
                    ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Properties.Resources.CouldNotConnectToSiteService, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }

            SetButtonsEnabledState();
        }

        private void touchScrollButtonPanel1_Click(object sender, ScrollButtonEventArguments args)
        {
            if(args.Tag == null)
            {
                return;
            }

            if(args.Tag is Buttons)
            {
                switch((Buttons)args.Tag)
                {
                    case Buttons.SelectLine:
                        
                        receipt.ToggleSelect(this, EventArgs.Empty);                        
                        
                        if(posTransaction is IRetailTransaction)
                        {
                            SetSplitItemSelection(receipt.SelectedLine.SelectedLineId, (bool)receipt.ItemTable.Rows[receipt.SelectedLine.SelectedIndex]["Selected"]);
                        }
                        break;
                    case Buttons.SelectAll:
                        receipt.SelectAll();
                        break;
                    case Buttons.ClearSelection:
                        receipt.Deselect_All(this, EventArgs.Empty);
                        break;
                    case Buttons.ReasonCodes:
                        InitializeButtons(!displayReasonCodes);
                        break;
                }

                SetButtonsEnabledState();
                return;
            }

            if(args.Tag is DataEntity)
            {
                SetReasonCode((DataEntity)args.Tag, false);
                return;
            }

            if(args.Tag is DialogResult)
            {
                if(((DialogResult)args.Tag == DialogResult.OK) && posTransaction is RetailTransaction transaction)
                {
                    int assemblyLineID = -1;
                    List<ISaleLineItem> saleLines = transaction.SaleItems.ToList();

                    for (int i = 0; i < receipt.ItemTable.Rows.Count; i++)
                    {
                        DataRow itemRow = receipt.ItemTable.Rows[i];
                        bool lineSelected = (itemRow["Selected"].ToString() == "True");
                        int lineID = int.Parse(itemRow["LineId"].ToString());
                        ISaleLineItem saleLine = saleLines.Find(line => line.LineId == lineID);

                        if (lineSelected && saleLine.IsAssembly && !saleLine.IsAssemblyComponent)
                        {
                            assemblyLineID = lineID;
                        }

                        if (saleLine.IsAssemblyComponent)
                        {
                            lineSelected = (saleLine.LinkedToLineId != -1) && (saleLine.LinkedToLineId == assemblyLineID);
                        }

                        if (lineSelected || behaviour == ReturnTransactionDialogBehaviourEnum.SetReasonCode)
                        {
                            ReasonCode code = reasonCodes.SingleOrDefault(x => x.ID == itemRow["ReasonCodeID"].ToString());

                            if (code != null || saleLine.IsAssemblyComponent)
                            {
                                ReturnedItems.AddLast(new ReturnedItemReason { LineNum = lineID, ReasonCode = code });
                            }
                            else
                            {
                                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.NoReasonCodeSelected, MessageBoxButtons.OK, MessageDialogType.Generic);
                                ReturnedItems.Clear();
                                return;
                            }
                        }
                    }

                    // Check if any items are selected...
                    if(ReturnedItems.Count == 0)
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.NoItemsAreSelected, MessageBoxButtons.OK, MessageDialogType.Generic);  // No items are selected                
                        return;
                    }
                }

                DialogResult = (DialogResult)args.Tag;
                Close();
                return;
            }
        }

        private void Receipt_ReceiptItemClick(object sender, POS.Processes.EventArguments.ReceiptItemClickArgs e)
        {
            SetSplitItemSelection(e.SelectedLine.SelectedLineId, (bool)receipt.ItemTable.Rows[e.SelectedLine.SelectedIndex]["Selected"]);
            SetButtonsEnabledState();
        }

        private void SetSplitItemSelection(int selectedLineID, bool selected)
        {
            if(!(posTransaction is IRetailTransaction))
            {
                return;
            }

            IRetailTransaction retailTransaction = (IRetailTransaction)posTransaction;
            ISaleLineItem selectedLine = retailTransaction.GetItem(selectedLineID);
            ISaleLineItem item = retailTransaction.GetTopMostLimitationSplitParentItem(selectedLine);

            if(item.LimitationSplitChildLineId < 0)
            {
                return;
            }

            List<int> linesToToggle = new List<int>();                       
            
            while(true)
            {
                if (item.LineId != selectedLineID)
                {
                    linesToToggle.Add(item.LineId);
                }

                // This was the last item in the chain
                if(item.LimitationSplitChildLineId < 0)
                {                    
                    break;
                }

                item = retailTransaction.GetItem(item.LimitationSplitChildLineId);
            }

            if (linesToToggle.Count > 0)
            {
                receipt.SelectRange(linesToToggle, selected ? ClickAction.Select : ClickAction.Unselect);
            }
        }

        /// <summary>
        /// Creates and saves a default system reason code for returns
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="closeConnection"></param>
        /// <returns></returns>
        /// <remarks>Same code is used in "Return Item" operation (POSPRocesses>Operations>ItemSale) and in ReasonCodeSelectDialog</remarks>
        private ReasonCode GetDefaultReturnReasonCode(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            ReasonCode defaultReason = ReasonCode.DefaultReturns();

            if (!Providers.ReasonCodeData.Exists(entry, defaultReason.ID))
            {
                Providers.ReasonCodeData.Save(entry, defaultReason);
            }

            ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
            if (service.TestConnection(entry, entry.SiteServiceAddress, entry.SiteServicePortNumber) == ConnectionEnum.Success)
            {
                service.SaveReasonCode(entry, siteServiceProfile, defaultReason, closeConnection);
            }

            return defaultReason;
        }

        private void SetReasonCode(DataEntity code, bool setAll)
        {
            string selected = "";

            for (int i = 0; i < receipt.ItemTable.Rows.Count; i++)
            {
                DataRow itemRow = receipt.ItemTable.Rows[i];
                selected = itemRow["Selected"].ToString() ?? "0";

                if (setAll || selected == "True")
                {
                    itemRow["ReasonCode"] = (bool)itemRow["IsAssemblyComponent"] ? "" : code.Text;
                    itemRow["ReasonCodeID"] = (bool)itemRow["IsAssemblyComponent"] ? "" : code.ID;
                }
            }
        }

        private void InitializeButtons(bool displayReasonCodes)
        {
            this.displayReasonCodes = displayReasonCodes;

            touchScrollButtonPanel1.Clear();

            if(behaviour == ReturnTransactionDialogBehaviourEnum.ReturnTransaction)
            {
                touchScrollButtonPanel1.AddButton(Properties.Resources.SelectLine, Buttons.SelectLine, "");
            }

            touchScrollButtonPanel1.AddButton(Properties.Resources.SelectAll, Buttons.SelectAll, "");
            touchScrollButtonPanel1.AddButton(Properties.Resources.ClearSelection, Buttons.ClearSelection, Conversion.ToStr((int)Buttons.ClearSelection));
            touchScrollButtonPanel1.AddButton(Properties.Resources.ReasonCodes, Buttons.ReasonCodes, "", TouchButtonType.Action, DockEnum.DockNone, displayReasonCodes ? Properties.Resources.white_line_arrow_up_16 : Properties.Resources.white_line_arrow_down_16, ImageAlignment.Left);

            if(displayReasonCodes)
            {
                try
                {
                    reasonCodes = Providers.ReasonCodeData.GetReasonCodesForReturn(dlgEntry);

                    if (!reasonCodes.Any())
                        reasonCodes = Services.Interfaces.Services.InventoryService(dlgEntry).GetReasonCodesForReturn(dlgEntry, dlgSettings.SiteServiceProfile, true);

                    if (!reasonCodes.Any())
                    {
                        //If no return reason codes, create a default one locally and at HQ
                        var defaultReason = GetDefaultReturnReasonCode(dlgEntry, dlgSettings.SiteServiceProfile, true);
                        reasonCodes = new List<ReasonCode> { defaultReason };
                    }

                    foreach (ReasonCode code in reasonCodes)
                    {
                        touchScrollButtonPanel1.AddButton(code.Text, code, "");
                    }
                }
                catch (Exception e)
                {
                    ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToSiteService, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }

            if(behaviour == ReturnTransactionDialogBehaviourEnum.ReturnTransaction)
            {
                touchScrollButtonPanel1.AddButton(Properties.Resources.ReturnItems, DialogResult.OK, OKButtonID, TouchButtonType.OK, DockEnum.DockEnd);
            }
            else
            {
                touchScrollButtonPanel1.AddButton(Properties.Resources.Set, DialogResult.OK, OKButtonID, TouchButtonType.OK, DockEnum.DockEnd);

                if(dlgSettings.TrainingMode)
                {
                    touchScrollButtonPanel1.SetButtonEnabled(OKButtonID, false);
                }
            }

            touchScrollButtonPanel1.AddButton(Properties.Resources.Cancel, DialogResult.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
        }

        private void SetButtonsEnabledState()
        {
            bool hasSelectedItems = false;

            for (int i = 0; i < receipt.ItemTable.Rows.Count; i++)
            {
                if (receipt.ItemTable.Rows[i]["Selected"].ToString() == "True")
                {
                    hasSelectedItems = true;
                    break;
                }
            }

            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSelection), hasSelectedItems);

            if(behaviour == ReturnTransactionDialogBehaviourEnum.ReturnTransaction || !dlgSettings.TrainingMode)
            {
                touchScrollButtonPanel1.SetButtonEnabled(OKButtonID, hasSelectedItems);
            }
        }
    }
}
