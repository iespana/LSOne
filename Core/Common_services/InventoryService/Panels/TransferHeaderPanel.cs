using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Services.Interfaces.Constants;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.WinFormsTouch;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.Controls.SupportClasses;
using System.Drawing;
using LSOne.Services.Interfaces;

namespace LSOne.Services.Panels
{
    public partial class TransferHeaderPanel : UserControl
    {
        private enum POSTransferCreateOption
        {
            New = 0,
            FromTemplate = 1,
            FromRequest = 2,
            FromOrder = 3
        }

        private IConnectionManager entry;
        private ISettings settings;
        private List<DataEntity> createOptions;
        private StoreTransferWrapper transferWrapper;
        private bool expectedDeliveryChanged;

        //Keep list of transfers to access store information
        private List<InventoryTransferOrder> inventoryTransferOrders;
        private List<InventoryTransferRequest> inventoryTransferRequests;

        public TransferHeaderPanel(IConnectionManager entry, StoreTransferWrapper transferWrapper)
        {
            InitializeComponent();

            this.entry = entry;
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            this.transferWrapper = transferWrapper;

            SetStyle(ControlStyles.Selectable, true);
            DoubleBuffered = true;
            InitializeLables();
        }

        private void TransferHeaderPanel_Load(object sender, EventArgs e)
        {
            InitView();
        }

        private void InitView()
        {
            createOptions = new List<DataEntity>();
            if (transferWrapper.IsNewTransfer)
            {
                createOptions.Add(new DataEntity((int)POSTransferCreateOption.New, Properties.Resources.CreateNew));
                createOptions.Add(new DataEntity((int)POSTransferCreateOption.FromTemplate, Properties.Resources.CreateFromTemplate));

                if(transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                {
                    createOptions.Add(new DataEntity((int)POSTransferCreateOption.FromRequest, Properties.Resources.CreateFromOpenRequest));
                    createOptions.Add(new DataEntity((int)POSTransferCreateOption.FromOrder, Properties.Resources.CreateFromOrder));
                }
                else
                {
                    createOptions.Add(new DataEntity((int)POSTransferCreateOption.FromRequest, Properties.Resources.CreateFromRequest));
                }

                cmbCreateOptions.SelectedData = createOptions[0];
            }
            else
            {
                DisableControlsForExistingTransfer();

                tbDescription.Text = transferWrapper.Description;
                dtExpectedDelivery.Value = transferWrapper.ExpectedDelivery;
                cmbStore.SelectedData = new DataEntity(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? transferWrapper.TransferOrder.ReceivingStoreId : transferWrapper.TransferRequest.SendingStoreId,
                                                       transferWrapper.TransferType == StoreTransferTypeEnum.Order ? transferWrapper.TransferOrder.ReceivingStoreName : transferWrapper.TransferRequest.SendingStoreName);
            }
        }

        private void DisableControlsForExistingTransfer()
        {
            lblCreateOptions.Visible = lblCopyFrom.Visible = cmbCopyFrom.Visible = cmbCreateOptions.Visible = false;
            cmbStore.Enabled = false;

            //Move visible control up by half the size of the textbox/label pair to center
            int textBoxHeightWithMargins = (tbDescription.Height + lblDescription.Height) + 11;
            lblDescription.Location = new Point(lblDescription.Location.X, lblDescription.Location.Y - textBoxHeightWithMargins);
            tbDescription.Location = new Point(tbDescription.Location.X, tbDescription.Location.Y - textBoxHeightWithMargins);
            lblStore.Location = new Point(lblStore.Location.X, lblStore.Location.Y - textBoxHeightWithMargins);
            cmbStore.Location = new Point(cmbStore.Location.X, cmbStore.Location.Y - textBoxHeightWithMargins);
            dtExpectedDelivery.Location = new Point(dtExpectedDelivery.Location.X, dtExpectedDelivery.Location.Y - textBoxHeightWithMargins);
            lblDueDate.Location = new Point(lblDueDate.Location.X, lblDueDate.Location.Y - textBoxHeightWithMargins);
        }

        private void InitializeLables()
        {
            switch (transferWrapper.TransferType)
            {
                case StoreTransferTypeEnum.Order:
                    lblStore.Text = Properties.Resources.ToStore;
                    break;
                case StoreTransferTypeEnum.Request:
                    lblStore.Text = Properties.Resources.FromStore;
                    break;
            }
        }

        private void touchKeyboard_ObtainCultureName(object sender, Controls.EventArguments.CultureEventArguments args)
        {
            if (settings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = settings.UserProfile.KeyboardCode;
                args.LayoutName = settings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = settings.Store.KeyboardCode;
                args.LayoutName = settings.Store.KeyboardLayoutName;
            }
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            if(transferWrapper.IsNewTransfer && !RecordIdentifier.IsEmptyOrNull(cmbStore.SelectedDataID))
            {
                if (!expectedDeliveryChanged)
                {
                    Store store = Providers.StoreData.Get(entry, cmbStore.SelectedDataID, CacheType.CacheTypeApplicationLifeTime, UsageIntentEnum.Minimal);
                    dtExpectedDelivery.Value = store.StoreTransferExpectedDeliveryDate();
                }
            }

            SetParentHeaderInfo();
        }

        private void SetParentHeaderInfo()
        {
            GetParentDialog()?.SetHeaderInfo();
        }

        private StoreTransferDialog GetParentDialog()
        {
            return this.Parent.Parent as StoreTransferDialog;
        }

        public string GetHeaderStoreName()
        {
            if(transferWrapper.IsNewTransfer)
            {
                return cmbStore.Text;
            }
            else
            {
                return transferWrapper.TransferType == StoreTransferTypeEnum.Order ? transferWrapper.TransferOrder.ReceivingStoreName : transferWrapper.TransferRequest.ReceivingStoreName;
            }
        }

        public bool Save()
        {
            try
            {
                if(!ValidateFields())
                {
                    return false;
                }

                if (DataChanged())
                {
                    if (transferWrapper.IsNewTransfer)
                    {
                        CreateTransferOrderResult result = CreateTransferOrderResult.ErrorCreatingTransferOrder;
                        RecordIdentifier newID = RecordIdentifier.Empty;
                        RecordIdentifier copyFromID = cmbCopyFrom.SelectedDataID;
                        Exception ex = null;
                        Action action = null;

                        if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                        {
                            transferWrapper.TransferOrder = new InventoryTransferOrder
                            {
                                CreationDate = DateTime.Now,
                                SendingStoreId = entry.CurrentStoreID,
                                ReceivingStoreId = cmbStore.SelectedDataID,
                                ReceivingStoreName = cmbStore.Text,
                                Text = tbDescription.Text.Trim(),
                                ExpectedDelivery = dtExpectedDelivery.Value == Date.Empty.DateTime ? DateTime.Now.AddDays(3) : dtExpectedDelivery.Value,
                                CreatedBy = entry.CurrentStoreID,
                            };

                            switch((int)cmbCreateOptions.SelectedDataID)
                            {
                                case (int)POSTransferCreateOption.New:
                                    action = delegate
                                    {
                                        newID = Interfaces.Services.InventoryService(entry).SaveInventoryTransferOrder(entry, settings.SiteServiceProfile, transferWrapper.TransferOrder, true);
                                        result = CreateTransferOrderResult.Success;
                                    };
                                    break;
                                case (int)POSTransferCreateOption.FromRequest:
                                    action = delegate { result = Interfaces.Services.InventoryService(entry).CreateTransferOrderFromRequest(entry, settings.SiteServiceProfile, copyFromID, transferWrapper.TransferOrder, ref newID, true); };
                                    break;
                                case (int)POSTransferCreateOption.FromOrder:
                                    action = delegate { result = Interfaces.Services.InventoryService(entry).CopyTransferOrder(entry, settings.SiteServiceProfile, copyFromID, transferWrapper.TransferOrder, ref newID, true); };
                                    break;
                                case (int)POSTransferCreateOption.FromTemplate:
                                    action = delegate
                                    {
                                        transferWrapper.TransferOrder.TemplateID = cmbCopyFrom.SelectedDataID;
                                        result = Interfaces.Services.InventoryService(entry).CreateTransferOrderFromTemplate(entry, settings.SiteServiceProfile, transferWrapper.TransferOrder, new TemplateListItem { TemplateID = copyFromID }, ref newID, true);
                                    };
                                    break;
                            }

                            if(action != null)
                            {
                                Interfaces.Services.DialogService(entry).ShowSpinnerDialog(action, "", Properties.Resources.ThisMayTakeAMoment, out ex);
                            }

                            if(ex != null)
                            {
                                Interfaces.Services.DialogService(entry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                return false;
                            }

                            switch (result)
                            {
                                case CreateTransferOrderResult.Success:
                                    transferWrapper.TransferOrder.ID = newID;
                                    DisableControlsForExistingTransfer();
                                    return true;
                                default:
                                    transferWrapper.TransferOrder = null;
                                    ShowCreateResultMessage(result);
                                    break;
                            }
                        }
                        else
                        {
                            transferWrapper.TransferRequest = new InventoryTransferRequest
                            {
                                CreationDate = DateTime.Now,
                                SendingStoreId = entry.CurrentStoreID,
                                ReceivingStoreId = cmbStore.SelectedDataID,
                                ReceivingStoreName = cmbStore.Text,
                                Text = tbDescription.Text.Trim(),
                                ExpectedDelivery = dtExpectedDelivery.Value == Date.Empty.DateTime ? DateTime.Now.AddDays(3) : dtExpectedDelivery.Value,
                                CreatedBy = entry.CurrentStoreID
                            };

                            switch ((int)cmbCreateOptions.SelectedDataID)
                            {
                                case (int)POSTransferCreateOption.New:
                                    action = delegate
                                    {
                                        newID = Interfaces.Services.InventoryService(entry).SaveInventoryTransferRequest(entry, settings.SiteServiceProfile, transferWrapper.TransferRequest, true);
                                        result = CreateTransferOrderResult.Success;
                                    };
                                    break;
                                case (int)POSTransferCreateOption.FromRequest:
                                    action = delegate { result = Interfaces.Services.InventoryService(entry).CopyTransferRequest(entry, settings.SiteServiceProfile, copyFromID, transferWrapper.TransferRequest, ref newID, true); };
                                    break;
                                case (int)POSTransferCreateOption.FromOrder:
                                    action = delegate { result = Interfaces.Services.InventoryService(entry).CreateTransferRequestFromOrder(entry, settings.SiteServiceProfile, copyFromID, transferWrapper.TransferRequest, ref newID, true); };
                                    break;
                                case (int)POSTransferCreateOption.FromTemplate:
                                    action = delegate
                                    {
                                        transferWrapper.TransferRequest.TemplateID = cmbCopyFrom.SelectedDataID;
                                        result = Interfaces.Services.InventoryService(entry).CreateTransferRequestFromTemplate(entry, settings.SiteServiceProfile, transferWrapper.TransferRequest, new TemplateListItem { TemplateID = copyFromID }, ref newID, true);
                                    };
                                    break;
                            }

                            if (action != null)
                            {
                                Interfaces.Services.DialogService(entry).ShowSpinnerDialog(action, "", Properties.Resources.ThisMayTakeAMoment, out ex);
                            }

                            if (ex != null)
                            {
                                Interfaces.Services.DialogService(entry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                return false;
                            }

                            switch (result)
                            {
                                case CreateTransferOrderResult.Success:
                                    transferWrapper.TransferRequest.ID = newID;
                                    return true;
                                default:
                                    transferWrapper.TransferRequest = null;
                                    ShowCreateResultMessage(result);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                        {
                            transferWrapper.TransferOrder.Text = tbDescription.Text;
                            transferWrapper.TransferOrder.ExpectedDelivery = dtExpectedDelivery.Value;
                            Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferOrder(entry, settings.SiteServiceProfile, transferWrapper.TransferOrder, true);
                        }
                        else
                        {
                            transferWrapper.TransferRequest.Text = tbDescription.Text;
                            transferWrapper.TransferRequest.ExpectedDelivery = dtExpectedDelivery.Value;
                            Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferRequest(entry, settings.SiteServiceProfile, transferWrapper.TransferRequest, true);
                        }

                        return true;
                    }

                    return false;
                }

                return true;
            }
            catch(Exception e)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(e is ClientTimeNotSynchronizedException ? e.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return false;
            }
            finally
            {
                if (transferWrapper.TransferRequest != null && RecordIdentifier.IsEmptyOrNull(transferWrapper.TransferRequest.ID))
                {
                    transferWrapper.TransferRequest = null;
                }

                if (transferWrapper.TransferOrder != null && RecordIdentifier.IsEmptyOrNull(transferWrapper.TransferOrder.ID))
                {
                    transferWrapper.TransferOrder = null;
                }
            }
        }

        private void ShowCreateResultMessage(CreateTransferOrderResult result)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case CreateTransferOrderResult.OrderNotFound:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToCreateTransferOrder : Properties.Resources.UnableToCreateTransferRequest
                                       + CRLF + Properties.Resources.TransferOrderToBeCopiedCouldNotBeFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;

                case CreateTransferOrderResult.RequestNotFound:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToCreateTransferOrder : Properties.Resources.UnableToCreateTransferRequest
                                       + CRLF + Properties.Resources.TransferRequestToBeCopiedCouldNotBeFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case CreateTransferOrderResult.HeaderInformationInsufficient:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToCreateTransferOrder : Properties.Resources.UnableToCreateTransferRequest
                                       + CRLF + Properties.Resources.InformationSendingReceivingStoreMissing, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case CreateTransferOrderResult.ErrorCreatingTransferOrder:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToCreateTransferOrder : Properties.Resources.UnableToCreateTransferRequest, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
                case CreateTransferOrderResult.TemplateNotFound:
                    Interfaces.Services.DialogService(entry).ShowMessage(transferWrapper.TransferType == StoreTransferTypeEnum.Order ? Properties.Resources.UnableToCreateTransferOrder : Properties.Resources.UnableToCreateTransferRequest
                                        + CRLF + Properties.Resources.StoreTransferTemplateNotFound, "", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    break;
            }
        }

        private bool DataChanged()
        {
            return transferWrapper.IsNewTransfer || transferWrapper.Description != tbDescription.Text || transferWrapper.ExpectedDelivery != dtExpectedDelivery.Value;
        }

        private bool ValidateFields()
        {
            touchErrorProvider.Clear();
            touchErrorProvider.Visible = false;

            if(string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                touchErrorProvider.ErrorText = Properties.Resources.DescriptionCannotBeEmpty;
                touchErrorProvider.Visible = true;
                return false;
            }

            if (transferWrapper.IsNewTransfer && (int)cmbCreateOptions.SelectedDataID != (int)POSTransferCreateOption.New && RecordIdentifier.IsEmptyOrNull(cmbCopyFrom.SelectedDataID))
            {
                touchErrorProvider.ErrorText = Properties.Resources.SelectionCannotBeEmpty;
                touchErrorProvider.Visible = true;
                return false;
            }

            if(transferWrapper.IsNewTransfer && RecordIdentifier.IsEmptyOrNull(cmbStore.SelectedDataID))
            {
                touchErrorProvider.ErrorText = Properties.Resources.StoreCannotBeEmpty;
                touchErrorProvider.Visible = true;
                return false;
            }

            return true;
        }

        private void cmbCreateOptions_SelectedDataChanged(object sender, EventArgs e)
        {
            cmbCopyFrom.Clear();
            cmbCopyFrom.SelectedData = new DataEntity();
            cmbCopyFrom.Enabled = (int)cmbCreateOptions.SelectedDataID != (int)POSTransferCreateOption.New;
        }

        private void cmbCopyFrom_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            try
            {
                IEnumerable<DataEntity> dataEntities = new List<DataEntity>();

                switch ((int)cmbCreateOptions.SelectedDataID)
                {
                    case (int)POSTransferCreateOption.FromTemplate:
                        dataEntities = Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateListForStore(entry, settings.SiteServiceProfile, entry.CurrentStoreID, new InventoryTemplateListFilter { EntryType = TemplateEntryTypeEnum.TransferStock }, true);
                        break;
                    case (int)POSTransferCreateOption.FromRequest:

                        if (transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                        {
                            InventoryTransferFilterExtended filter = new InventoryTransferFilterExtended();
                            filter.SendingStoreID = entry.CurrentStoreID;
                            filter.TransferFilterType = InventoryTransferType.Incoming;
                            inventoryTransferRequests = Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferRequestsExtended(entry, settings.SiteServiceProfile, filter, true);
                        }
                        else
                        {
                            inventoryTransferRequests = Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferRequests(entry, settings.SiteServiceProfile, new InventoryTransferFilter { ReceivingStoreID = entry.CurrentStoreID }, true);
                        }

                        dataEntities = inventoryTransferRequests;
                        break;
                    case (int)POSTransferCreateOption.FromOrder:
                        inventoryTransferOrders = Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferOrders(entry, settings.SiteServiceProfile, new InventoryTransferFilter { SendingStoreID = entry.CurrentStoreID }, true);
                        dataEntities = inventoryTransferOrders;
                        break;
                }

                RecordIdentifier selectedID = cmbCopyFrom.SelectedDataID ?? RecordIdentifier.Empty;
                DualDataPanel panelToEmbed = new DualDataPanel(
                    selectedID,
                    dataEntities,
                    null,
                    true,
                    cmbCopyFrom.SkipIDColumn,
                    false,
                    50,
                    false);

                panelToEmbed.Width = cmbCopyFrom.Width;
                panelToEmbed.SelectNoneAllowed = true;
                panelToEmbed.Touch = true;
                e.ControlToEmbed = panelToEmbed;
            }
            catch(Exception ex)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Properties.Resources.CouldNotConnectToStoreServer, Properties.Resources.ConnectionFailure, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        private void cmbStore_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            RecordIdentifier selectedID = cmbStore.SelectedDataID ?? RecordIdentifier.Empty;
            DualDataPanel panelToEmbed = new DualDataPanel(
                selectedID,
                Providers.StoreData.GetList(entry).Where(x => x.ID != entry.CurrentStoreID),
                null,
                true,
                cmbStore.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Width = cmbStore.Width;
            panelToEmbed.SelectNoneAllowed = true;
            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private void dtExpectedDelivery_ValueChanged(object sender, EventArgs e)
        {
            expectedDeliveryChanged = true;
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            if(!RecordIdentifier.IsEmptyOrNull(cmbCopyFrom.SelectedDataID))
            {
                if((int)cmbCreateOptions.SelectedDataID == (int)POSTransferCreateOption.FromOrder)
                {
                    InventoryTransferOrder order = inventoryTransferOrders.SingleOrDefault(x => x.ID == cmbCopyFrom.SelectedDataID);

                    if (order != null)
                    {
                        cmbStore.SelectedData = new DataEntity(order.ReceivingStoreId, order.ReceivingStoreName);

                        if (!expectedDeliveryChanged)
                        {
                            Store store = Providers.StoreData.Get(entry, order.ReceivingStoreId, CacheType.CacheTypeApplicationLifeTime, UsageIntentEnum.Minimal);
                            dtExpectedDelivery.Value = store.StoreTransferExpectedDeliveryDate();
                        }
                    }
                }
                else if((int)cmbCreateOptions.SelectedDataID == (int)POSTransferCreateOption.FromRequest)
                {
                    InventoryTransferRequest request = inventoryTransferRequests.SingleOrDefault(x => x.ID == cmbCopyFrom.SelectedDataID);

                    if(request != null)
                    {
                        if(transferWrapper.TransferType == StoreTransferTypeEnum.Order)
                        {
                            cmbStore.SelectedData = new DataEntity(request.SendingStoreId, request.SendingStoreName);

                            if (!expectedDeliveryChanged)
                            {
                                dtExpectedDelivery.Value = request.ExpectedDelivery;
                            }
                        }
                        else
                        {
                            cmbStore.SelectedData = new DataEntity(request.ReceivingStoreId, request.ReceivingStoreName);

                            if (!expectedDeliveryChanged)
                            {
                                Store store = Providers.StoreData.Get(entry, request.ReceivingStoreId, CacheType.CacheTypeApplicationLifeTime, UsageIntentEnum.Minimal);
                                dtExpectedDelivery.Value = store.StoreTransferExpectedDeliveryDate();
                            }
                        }
                        
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            GetParentDialog()?.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(Save())
            {
                StoreTransferDialog parent = GetParentDialog();

                if(parent != null)
                {
                    parent.SetHeaderInfo();
                    parent.ShowItemsPanel();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(Save())
            {
                GetParentDialog()?.Close();
            }
        }

        private void cmbCreateOptions_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                createOptions,
                null,
                true,
                cmbCreateOptions.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }
    }
}
