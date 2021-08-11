using LSOne.Controls;
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
    /// See list of outgoing transfer requests.
    /// </summary>
    public partial class RequestTransferDialog : TouchBaseForm
    {
        /// <summary>
        /// Outgoing transfer requests dialog buttons
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
            /// Create new transfer request
            /// </summary>
            Add,
            /// <summary>
            /// Edit transfer request
            /// </summary>
            Edit,
            /// <summary>
            /// Delete transfer request
            /// </summary>
            Delete,
            /// <summary>
            /// Send transfer request
            /// </summary>
            Send,
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
        public RequestTransferDialog(IConnectionManager entry, IPosTransaction transaction, OperationInfo operationInfo)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            currentTransaction = transaction;
            this.operationInfo = operationInfo;
            searchCriteria = new InventoryTransferFilterExtended();
            searchCriteria.TransferFilterType = InventoryTransferType.Outgoing;

            panel.ButtonHeight = 50;
            banner.BannerText = Resources.RequestTransfer;
        }

        private void Delete()
        {
            if (selectedRequest != null)
            {
                // Ask user if they are sure they want to delete
                if (Interfaces.Services.DialogService(dlgEntry).ShowMessage(
                    Resources.DeleteRequestQuestion, Resources.DeleteRequest, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Exception exception = new Exception();
                    DeleteTransferResult result = DeleteTransferResult.Success;
                    try
                    {
                        IInventoryService inventoryService = (IInventoryService)dlgEntry.Service(ServiceType.InventoryService);

                        InventoryTransferRequest request = inventoryService.GetInventoryTransferRequest(dlgEntry, dlgSettings.SiteServiceProfile, selectedRequest.ID, true);

                        if (request.FetchedByReceivingStore)
                        {
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.FetchedRequestCannotBeDeleted, MessageBoxButtons.OK, MessageDialogType.Attention);
                            LvTransferRequests_Load(this, EventArgs.Empty);
                            return;
                        }

                        Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => result = inventoryService.DeleteTransferRequest(dlgEntry, selectedRequest.ID, false, dlgSettings.SiteServiceProfile),
                            Resources.DeleteRequest, Resources.ThisMayTakeAMoment, out exception);
                        if (result == DeleteTransferResult.Success)
                        {
                            lvTransferRequests.RemoveRow(lvTransferRequests.Selection.FirstSelectedRow);
                            lvTransferRequests.Selection.Clear();
                            selectedRequest = null;
                        }
                        else
                        {
                            string errorMessage = GetDeleteRequestErrorMessage(result);
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

        private void Search()
        {
            SearchInventoryTransfer search = new SearchInventoryTransfer(dlgEntry, StoreTransferTypeEnum.Request, searchCriteria);
            if (search.ShowDialog() == DialogResult.OK)
            {
                searchCriteria = search.SearchCriteria;
                searchCriteria.ReceivingStoreID = dlgEntry.CurrentStoreID;
                searchCriteria.TransferFilterType = InventoryTransferType.Outgoing;
                panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), true);

                transferRequestList = GetInventoryTransferRequests(searchCriteria);
                PopulateListView(transferRequestList);
            }
        }

        private void Send()
        {
            if (selectedRequest != null)
            {
                // Ask user if they are sure they want to send
                if (Interfaces.Services.DialogService(dlgEntry).ShowMessage(
                    Resources.SendTransferRequestQuestion, Resources.SendTransferRequest, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Exception exception = new Exception();
                    SendTransferOrderResult result = SendTransferOrderResult.Success;
                    try
                    {
                        IInventoryService inventoryService = (IInventoryService)dlgEntry.Service(ServiceType.InventoryService);

                        InventoryTransferRequest request = inventoryService.GetInventoryTransferRequest(dlgEntry, dlgSettings.SiteServiceProfile, selectedRequest.ID, true);

                        if (request.FetchedByReceivingStore)
                        {
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.FetchedRequestCannotBeSent, MessageBoxButtons.OK, MessageDialogType.Attention);
                            LvTransferRequests_Load(this, EventArgs.Empty);
                            return;
                        }

                        Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => result = inventoryService.SendTransferRequest(dlgEntry, dlgSettings.SiteServiceProfile, selectedRequest.ID, true),
                            Resources.SendTransferRequest, Resources.ThisMayTakeAMoment, out exception);
                        if (result == SendTransferOrderResult.Success)
                        {
                            lvTransferRequests.Row(lvTransferRequests.Selection.FirstSelectedRow)[3].Text = Resources.Requested;
                            lvTransferRequests.AutoSizeColumns(true);
                            lvTransferRequests.Selection.Clear();
                        }
                        else
                        {
                            string errorMessage = GetSendRequestErrorMessage(result);
                            Interfaces.Services.DialogService(dlgEntry).ShowMessage(errorMessage, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        }
                        LvTransferRequests_Load(this, EventArgs.Empty);
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
                case (int)Buttons.Add: { AddTransferRequest(); break; }
                case (int)Buttons.Edit: { EditTransferRequest(); break; }
                case (int)Buttons.Delete: { Delete(); break; }
                case (int)Buttons.Send: { Send(); break; }
                case (int)Buttons.Cancel: { Close(); break; }
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
                row.AddText(request.ReceivingStoreName);
                row.AddText(GetStatus(request));
                row.AddText(request.SentDate.ToShortDateString());
                row.AddText(request.ExpectedDelivery.ToShortDateString());

                row.Tag = request.ID;
                lvTransferRequests.AddRow(row);
            }
            lvTransferRequests.AutoSizeColumns(true);
        }

        private string GetStatus(InventoryTransferRequest request)
        {
            string status = Resources.New;

            if (request.Rejected)
            {
                status = Resources.Rejected; // Note: we don't want to show this request
            }
            else if (request.InventoryTransferOrderCreated) // Note: we don't want to show this request
            {
                status = Resources.Closed;
            }
            else if (request.FetchedByReceivingStore)
            {
                status = Resources.Viewed;
            }
            else if (request.Sent)
            {
                status = Resources.Requested;
            }

            return status;
        }

        private void PopulateInfoBox()
        {
            lblTransferID.Text = selectedRequest.ID.StringValue;
            lblDescription.Text = selectedRequest.Text;
            lblFromStore.Text = selectedRequest.ReceivingStoreName;
            lblStatus.Text = GetStatus(selectedRequest);
            lblSentDate.Text = selectedRequest.CreationDate.ToShortDateString();
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
            lblFromStore.Text = string.Empty;
            lblStatus.Text = string.Empty;
            lblSentDate.Text = string.Empty;
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
                TransferFilterType = InventoryTransferType.Outgoing,
                ReceivingStoreID = dlgEntry.CurrentStoreID
            };
            transferRequestList = GetInventoryTransferRequests(filter);
            PopulateListView(transferRequestList);
            ClearRequestInfo();
        }

        private void SelectRequest(InventoryTransferRequest transferRequest)
        {
            selectedRequest = transferRequest;

            PopulateInfoBox();
            PopulateItemListView();
        }

        private void RequestTransferDialog_Load(object sender, EventArgs e)
        {
            panel.AddButton(Resources.Search, Buttons.Search, Conversion.ToStr((int)Buttons.Search));
            panel.AddButton(Resources.ClearSearch, Buttons.ClearSearch, Conversion.ToStr((int)Buttons.ClearSearch));
            panel.AddButton(Resources.Add, Buttons.Add, Conversion.ToStr((int)Buttons.Add), TouchButtonType.Normal, DockEnum.DockNone, Resources.Plusincircle_16px, ImageAlignment.Left);
            panel.AddButton(Resources.Edit, Buttons.Edit, Conversion.ToStr((int)Buttons.Edit), TouchButtonType.Normal, DockEnum.DockNone, Resources.Edit_16px, ImageAlignment.Left);
            panel.AddButton(Resources.Delete, Buttons.Delete, Conversion.ToStr((int)Buttons.Delete), TouchButtonType.Normal, DockEnum.DockNone, Resources.Clear_16px, ImageAlignment.Left);
            panel.AddButton(Resources.SendRequest, Buttons.Send, Conversion.ToStr((int)Buttons.Send), TouchButtonType.OK, DockEnum.DockEnd);
            panel.AddButton(Resources.Close, Buttons.Cancel, Conversion.ToStr((int)Buttons.Cancel), TouchButtonType.Cancel, DockEnum.DockEnd);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), false);

            //Set the size of the form the same as the main form
            this.Width = dlgSettings.MainFormInfo.MainWindowWidth;
            this.Height = dlgSettings.MainFormInfo.MainWindowHeight;
            this.Top = dlgSettings.MainFormInfo.MainWindowTop;
            this.Left = dlgSettings.MainFormInfo.MainWindowLeft;

            //Make sure the item list view scales with screen to 25% and request list view fills the rest (75% - width of the button panel)

            int totalWidthItemsAndRequests = Width - panel.Width - 30; // Margins: 10 + 5 + 5 + 10 = 30 px
            lvItemsList.Width = pnlTransferInfo.Width = 3 * (totalWidthItemsAndRequests / 10);
            upDownButton.Width = lvItemsList.Width - 1; 
            lvTransferRequests.Width = totalWidthItemsAndRequests - lvItemsList.Width;
            lvTransferRequests.Location = new Point(lvItemsList.Width + 15, lvTransferRequests.Location.Y);

            lvTransferRequests.AutoSizeColumns(true);
            lvItemsList.AutoSizeColumns(true);

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Edit), false);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Delete), false);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Send), false);
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

            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Send), selectedRequest != null && !selectedRequest.FetchedByReceivingStore);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Edit), selectedRequest != null && !selectedRequest.FetchedByReceivingStore);
            panel.SetButtonEnabled(Conversion.ToStr((int)Buttons.Delete), selectedRequest != null && !selectedRequest.FetchedByReceivingStore);
        }

        private string GetDeleteRequestErrorMessage(DeleteTransferResult result)
        {
            switch(result)
            {
                case DeleteTransferResult.NotFound:
                    return Resources.UnableToDeleteTransferRequest + " " + Resources.TransferRequestNotFound;
                case DeleteTransferResult.Sent:
                    return Resources.UnableToDeleteTransferRequest + " " + Resources.SentTransferRequestCannotBeDeleted;
                case DeleteTransferResult.FetchedByReceivingStore:
                    return Resources.UnableToDeleteTransferRequest + " " + Resources.FetchedTransferRequestCannotBeDeleted;
                case DeleteTransferResult.Received:
                    return Resources.UnableToDeleteTransferRequest + " " + Resources.ReceivedTransferRequestCannotBeDeleted;
                case DeleteTransferResult.ErrorDeletingTransfer:
                    return Resources.UnableToDeleteTransferRequest + " " + Resources.ErrorDeletingTransferRequest;
                default:
                    return Resources.UnableToDeleteTransferRequest;
            }
        }

        private string GetSendRequestErrorMessage(SendTransferOrderResult result)
        {
            switch(result)
            {
                case SendTransferOrderResult.ErrorSendingTransferOrder:
                    return Resources.UnableToSendTransferRequest;
                case SendTransferOrderResult.FetchedByReceivingStore:
                    return Resources.TransferRequestHasBeenOpenedByReceivingStore;
                case SendTransferOrderResult.NotFound:
                    return Resources.TransferRequestNotFound;
                case SendTransferOrderResult.TransferAlreadySent:
                    return Resources.UnableToSendTransferRequest + " " + Resources.TransferRequestAlreadySent;
                case SendTransferOrderResult.NoItemsOnTransfer:
                    return Resources.UnableToSendTransferRequest + " " + Resources.TransferRequestHasNoLines;
                case SendTransferOrderResult.UnitConversionError:
                    return Resources.UnableToSendTransferRequest + " " + Resources.MissingUnitConversion;
                case SendTransferOrderResult.TransferOrderIsRejected:
                    return Resources.UnableToSendTransferRequest + " " + Resources.RejectedTransferRequestCantBeSent;
                default:
                    return Resources.UnableToSendTransferRequest;
            }
        }

        private void AddTransferRequest()
        {
            using (StoreTransferDialog dlg = new StoreTransferDialog(dlgEntry, new StoreTransferWrapper(StoreTransferTypeEnum.Request, InventoryTransferType.Outgoing), currentTransaction, operationInfo))
            {
                dlg.ShowDialog();
                LvTransferRequests_Load(this, EventArgs.Empty);
            }
        }

        private void EditTransferRequest()
        {
            if (selectedRequest != null)
            {
                try
                {
                    InventoryTransferRequest request = Interfaces.Services.InventoryService(dlgEntry).GetInventoryTransferRequest(dlgEntry, dlgSettings.SiteServiceProfile, selectedRequest.ID, true);

                    if(request.FetchedByReceivingStore)
                    {
                        Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.FetchedRequestCannotBeEdited, MessageBoxButtons.OK, MessageDialogType.Attention);
                        LvTransferRequests_Load(this, EventArgs.Empty);
                        return;
                    }
                }
                catch(Exception e)
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Resources.CouldNotConnectToSiteService, Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return;
                }

                using (StoreTransferDialog dlg = new StoreTransferDialog(dlgEntry, new StoreTransferWrapper(selectedRequest, InventoryTransferType.Outgoing), currentTransaction, operationInfo))
                {
                    dlg.ShowDialog();
                    LvTransferRequests_Load(this, EventArgs.Empty);
                }
            }
        }

        private void lvTransferRequests_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            EditTransferRequest();
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