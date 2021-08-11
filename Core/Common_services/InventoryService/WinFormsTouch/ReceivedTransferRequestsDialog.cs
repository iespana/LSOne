using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{

    /// <summary>
    /// See list of incoming transfer requests.
    /// </summary>
    public partial class ReceivedTransferRequestsDialog : TouchBaseForm
    {
        /// <summary>
        /// Incoming transfer requests dialog buttons
        /// </summary>
        public enum Buttons
        {
            /// <summary>
            /// Search transfer requests
            /// </summary>
            Search,
            /// <summary>
            /// Clear transfer request search
            /// </summary>
            ClearSearch,
            /// <summary>
            /// Create transfer order from transfer request
            /// </summary>
            CreateTransferOrder,
            /// <summary>
            /// Reject transfer request
            /// </summary>
            Reject,
            /// <summary>
            /// Cancel search
            /// </summary>
            Cancel
        }

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private IPosTransaction currentTransaction;
        private OperationInfo operationInfo;
        private List<InventoryTransferRequest> transferRequestList;
        private InventoryTransferRequest selectedRequest;
        private InventoryTransferFilterExtended searchCriteria;


        /// <summary>
        /// Initialize window
        /// </summary>
        /// <param name="entry"></param>
        public ReceivedTransferRequestsDialog(IConnectionManager entry, IPosTransaction transaction, OperationInfo operationInfo)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            currentTransaction = transaction;
            this.operationInfo = operationInfo;
            searchCriteria = new InventoryTransferFilterExtended();
            searchCriteria.TransferFilterType = InventoryTransferType.Incoming;

            panel.ButtonHeight = 50;
            banner.BannerText = Resources.ReceivedTransferRequests;
        }

        private void Search()
        {
            SearchInventoryTransfer search = new SearchInventoryTransfer(dlgEntry, StoreTransferTypeEnum.Request, searchCriteria);
            if (search.ShowDialog() == DialogResult.OK)
            {
                searchCriteria = search.SearchCriteria;
                searchCriteria.SendingStoreID = dlgEntry.CurrentStoreID;
                searchCriteria.TransferFilterType = InventoryTransferType.Incoming;
                panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), true);

                transferRequestList = GetInventoryTransferRequests(searchCriteria);
                PopulateListView(transferRequestList);
            }
        }

        private void Reject()
        {
            if (selectedRequest != null)
            {
                // Ask user if they are sure they want to reject
                if (Interfaces.Services.DialogService(dlgEntry).ShowMessage(
                    Resources.RejectRequestQuestion, Resources.RejectRequest, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Exception exception = new Exception();
                    DeleteTransferResult result = DeleteTransferResult.Success;
                    try
                    {
                        IInventoryService inventoryService = (IInventoryService)dlgEntry.Service(ServiceType.InventoryService);
                        Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => result = inventoryService.DeleteTransferRequest(dlgEntry, selectedRequest.ID, true, dlgSettings.SiteServiceProfile),
                            Resources.SendInventoryTransferOrder, Resources.ThisMayTakeAMoment, out exception);

                        if (result == DeleteTransferResult.Success)
                        {
                            lvTransferRequests.RemoveRow(lvTransferRequests.Selection.FirstSelectedRow);
                            lvTransferRequests.Selection.Clear();
                            selectedRequest = null;
                        }
                        else
                        {
                            string errorMessage = GetRejectRequestErrorMessage(result);
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(errorMessage, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        }
                    }
                    catch(Exception e)
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                }
            }
        }

        private void panel_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((int)args.Tag)
            {
                case (int)Buttons.Search: { Search(); break; }
                case (int)Buttons.ClearSearch:
                    {
                        searchCriteria = new InventoryTransferFilterExtended();
                        LvTransferRequests_Load(null, null); 
                        panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), false);
                        break;
                    }
                case (int)Buttons.Reject: { Reject(); break; }
                case (int)Buttons.CreateTransferOrder: { CreateTransferOrder(); break; }
                case (int)Buttons.Cancel: { Close(); break; }
            }
        }

        private void CreateTransferOrder()
        {
            if(selectedRequest != null && Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.CreateTransferOrderFromRequestQuestion, Resources.CreateTransferOrder, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Exception ex = null;

                try
                {
                    InventoryTransferRequest request = Interfaces.Services.InventoryService(dlgEntry).GetInventoryTransferRequest(dlgEntry, dlgSettings.SiteServiceProfile, selectedRequest.ID, false);

                    if(request == null || request.InventoryTransferOrderCreated || request.Rejected)
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.UnableToCreateTransferOrder, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        LvTransferRequests_Load(this, EventArgs.Empty);
                        return;
                    }

                    CreateTransferOrderResult result = CreateTransferOrderResult.ErrorCreatingTransferOrder;
                    Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => result = Interfaces.Services.InventoryService(dlgEntry).CreateTransferOrdersFromRequests(dlgEntry, new List<RecordIdentifier> { selectedRequest.ID }, dlgEntry.CurrentStoreID, dlgSettings.SiteServiceProfile),
                                Resources.CreateTransferOrder, Resources.ThisMayTakeAMoment, out ex);

                    if (ex != null)
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }

                    if (result == CreateTransferOrderResult.Success)
                    {
                        request = Interfaces.Services.InventoryService(dlgEntry).GetInventoryTransferRequest(dlgEntry, dlgSettings.SiteServiceProfile, selectedRequest.ID, false);
                        InventoryTransferOrder order = Interfaces.Services.InventoryService(dlgEntry).GetInventoryTransferOrder(dlgEntry, dlgSettings.SiteServiceProfile, request.InventoryTransferOrderId, true);

                        if (order != null)
                        {
                            using (StoreTransferDialog dlg = new StoreTransferDialog(dlgEntry, new StoreTransferWrapper(order, InventoryTransferType.Outgoing), currentTransaction, operationInfo))
                            {
                                dlg.ShowDialog();
                                LvTransferRequests_Load(this, EventArgs.Empty);
                            }
                        }
                    }
                    else
                    {
                        ShowCreateResultMessage(result);
                        LvTransferRequests_Load(this, EventArgs.Empty);
                    }
                }
                catch(Exception e)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
        }

        private void PopulateListView(List<InventoryTransferRequest> requests)
        {
            lvTransferRequests.ClearRows();
            Row row;
            foreach (InventoryTransferRequest request in requests)
            {
                row = new Row();
                row.AddText(request.ID.StringValue);
                row.AddText(request.Text);
                row.AddText(request.SendingStoreName);
                row.AddText(request.SentDate.ToShortDateString());
                row.AddText(request.ExpectedDelivery.ToShortDateString());

                row.Tag = request.ID;
                lvTransferRequests.AddRow(row);
            }
            lvTransferRequests.AutoSizeColumns();
        }

        private void PopulateInfoBox()
        {
            lblTransferID.Text = selectedRequest.ID.StringValue;
            lblDescription.Text = selectedRequest.Text;
            lblStore.Text = selectedRequest.SendingStoreName;
            lblDate.Text = selectedRequest.CreationDate.ToShortDateString();
            lblDueDate.Text = selectedRequest.ExpectedDelivery.ToShortDateString();
        }

        private void PopulateItemListView()
        {
            lvItemsList.ClearRows();
            List<InventoryTransferRequestLine> requestLines = new List<InventoryTransferRequestLine>();

            try
            {
                Exception ex = null;
                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => requestLines = Interfaces.Services.SiteServiceService(dlgEntry).GetRequestLinesForInventoryTransferAdvanced(dlgEntry, dlgSettings.SiteServiceProfile, selectedRequest.ID, out int x, new InventoryTransferFilterExtended { RowTo = int.MaxValue }, true), "", Resources.ThisMayTakeAMoment, out ex);

                if (ex != null)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
            catch(Exception e)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

            DecimalLimit quantityLimit;
            Row row;
            foreach (InventoryTransferRequestLine requestLine in requestLines)
            {
                quantityLimit = Providers.UnitData.GetNumberLimitForUnit(dlgEntry, Providers.UnitData.GetIdFromDescription(dlgEntry, requestLine.UnitName), CacheType.CacheTypeApplicationLifeTime);

                row = new Row();
                row.AddText(requestLine.ItemName);
                row.AddText(requestLine.QuantityRequested.FormatWithLimits(quantityLimit));
                row.AddText(requestLine.UnitName);
                row.Tag = requestLine.ID;
                lvItemsList.AddRow(row);
            }
            lvItemsList.AutoSizeColumns(true);
        }

        private void ClearRequestInfo()
        {
            lvItemsList.ClearRows();
            lblTransferID.Text = string.Empty;
            lblDescription.Text = string.Empty;
            lblStore.Text = string.Empty;
            lblDate.Text = string.Empty;
            lblDueDate.Text = string.Empty;
        }

        private List<InventoryTransferRequest> GetInventoryTransferRequests(InventoryTransferFilterExtended filter)
        {
            List<InventoryTransferRequest> requests = new List<InventoryTransferRequest>();

            try
            {
                Exception ex = null;
                Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => requests = Interfaces.Services.SiteServiceService(dlgEntry).SearchInventoryTransferRequestsExtended(dlgEntry, dlgSettings.SiteServiceProfile, filter, true), "", Resources.ThisMayTakeAMoment, out ex);

                if (ex != null)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }
            }
            catch(Exception e)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }

            return requests;
        }

        private void LvTransferRequests_Load(object sender, EventArgs e)
        {
            InventoryTransferFilterExtended filter = new InventoryTransferFilterExtended
            {
                TransferFilterType = InventoryTransferType.Incoming,
                SendingStoreID = dlgEntry.CurrentStoreID
            };
            transferRequestList = GetInventoryTransferRequests(filter);
            PopulateListView(transferRequestList);
        }

        private void SelectRequest(InventoryTransferRequest transferRequest)
        {
            if(!transferRequest.FetchedByReceivingStore)
            {
                try
                {
                    transferRequest.FetchedByReceivingStore = true;
                    Interfaces.Services.InventoryService(dlgEntry).SaveInventoryTransferRequest(dlgEntry, dlgSettings.SiteServiceProfile, transferRequest, true);
                }
                catch
                {
                    transferRequest.FetchedByReceivingStore = false;
                }
            }

            selectedRequest = transferRequest;
            PopulateInfoBox();
            PopulateItemListView();
        }

        private void ReceivedTransferRequestsDialog_Load(object sender, EventArgs e)
        {
            panel.AddButton(Resources.Search, Buttons.Search, Conversion.ToStr((int)Buttons.Search));
            panel.AddButton(Resources.ClearSearch, Buttons.ClearSearch, Conversion.ToStr((int)Buttons.ClearSearch));
            panel.AddButton(Resources.Reject, Buttons.Reject, Conversion.ToStr((int)Buttons.Reject), TouchButtonType.Normal, DockEnum.DockEnd);
            panel.AddButton(Resources.CreateOrder, Buttons.CreateTransferOrder, Conversion.ToStr((int)Buttons.CreateTransferOrder), TouchButtonType.OK, DockEnum.DockEnd);
            panel.AddButton(Resources.Close, Buttons.Cancel, Conversion.ToStr((int)Buttons.Cancel), TouchButtonType.Cancel, DockEnum.DockEnd);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), false);

            //Set the size of the form the same as the main form
            this.Width = dlgSettings.MainFormInfo.MainWindowWidth;
            this.Height = dlgSettings.MainFormInfo.MainWindowHeight;
            this.Top = dlgSettings.MainFormInfo.MainWindowTop;
            this.Left = dlgSettings.MainFormInfo.MainWindowLeft;

            //Make sure the item list view scales with screen to 30% and order list view fills the rest
            int totalWidthItemsAndRequests = Width - panel.Width - 30; // Margins: 10 + 5 + 5 + 10 = 30 px
            lvItemsList.Width = pnlTransferInfo.Width = 3 * (totalWidthItemsAndRequests / 10);
            upDownButton.Width = lvItemsList.Width - 1;
            lvTransferRequests.Width = totalWidthItemsAndRequests - lvItemsList.Width;
            lvTransferRequests.Location = new Point(lvItemsList.Width + 15, lvTransferRequests.Location.Y);

            lvTransferRequests.AutoSizeColumns(true);
            lvItemsList.AutoSizeColumns(true);

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Reject), lvTransferRequests.Selection.Count > 0);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.CreateTransferOrder), lvTransferRequests.Selection.Count > 0);

            ClearRequestInfo();
        }

        private void LvTransferRequests_SelectionChanged(object sender, EventArgs e)
        {
            if (transferRequestList.Count > 0 && lvTransferRequests.Selection.FirstSelectedRow >= 0)
            {
                SelectRequest(transferRequestList[lvTransferRequests.Selection.FirstSelectedRow]);
            }
            else
            {
                selectedRequest = null;
                ClearRequestInfo();
            }

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Reject), selectedRequest != null);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.CreateTransferOrder), selectedRequest != null);
        }

        private string GetRejectRequestErrorMessage(DeleteTransferResult result)
        {
            switch (result)
            {
                case DeleteTransferResult.NotFound:
                    return Resources.UnableToRejectTransferRequest + " " + Resources.TransferRequestNotFound;
                case DeleteTransferResult.Received:
                    return Resources.UnableToRejectTransferRequest + " " + Resources.ReceivedTransferRequestCannotBeDeleted;
                case DeleteTransferResult.ErrorDeletingTransfer:
                    return Resources.UnableToRejectTransferRequest + " " + Resources.ErrorRejectingTransferRequest;
                default:
                    return Resources.UnableToRejectTransferRequest;
            }
        }

        private void ShowCreateResultMessage(CreateTransferOrderResult result)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case CreateTransferOrderResult.OrderNotFound:
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.TransferOrderNotFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case CreateTransferOrderResult.RequestNotFound:
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.TransferRequestNotFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case CreateTransferOrderResult.HeaderInformationInsufficient:
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.UnableToCreateTransferOrder + CRLF + Resources.InformationSendingReceivingStoreMissing, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case CreateTransferOrderResult.ErrorCreatingTransferOrder:
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.UnableToCreateTransferOrder, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case CreateTransferOrderResult.TemplateNotFound:
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.UnableToCreateTransferOrder + CRLF + Resources.StoreTransferTemplateNotFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
            }
        }

        private void pnlTransferInfo_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(p, 0, 0, pnlTransferInfo.Width - 2, pnlTransferInfo.Height - 1);
            p.Dispose();
        }

        private void upDownButton_DownButtonClick(object sender, MouseEventArgs e)
        {
            lvItemsList.MoveSelectionDown();
        }

        private void upDownButton_UpButtonClick(object sender, MouseEventArgs e)
        {
            lvItemsList.MoveSelectionUp();
        }
    }
}