using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.ViewPlugins.Inventory.Dialogs.WizardPages;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using System.Linq;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class StoreTransferWizard : WizardBase
    {
        private StoreTransferTypeEnum transferType;
        private StoreTransferActionWrapper transferAction;        

        public StoreTransferWizard(IConnectionManager connection, StoreTransferTypeEnum transferType) : base(connection)
        {
            InitializeComponent();

            this.transferType = transferType;

            transferAction = new StoreTransferActionWrapper
            {
                Action = StoreTransferActionEnum.CreateNew
            };

            switch (transferType)
            {
                case StoreTransferTypeEnum.Order:
                    StoreTransferOrderWizardButtons orderPage = new StoreTransferOrderWizardButtons(this, transferAction);
                    orderPage.RequestFinish += Page_RequestFinish;
                    AddPage(orderPage);
                    break;
                case StoreTransferTypeEnum.Request:
                    StoreTransferRequestWizardButtons requestPage = new StoreTransferRequestWizardButtons(this, transferAction);
                    requestPage.RequestFinish += Page_RequestFinish;
                    AddPage(requestPage);

                    this.Text = Properties.Resources.NewTransferRequest;
                    break;
                default:
                    break;
            }            
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void Page_RequestFinish(object sender, EventArgs e)
        {
            Finish();
        }

        protected override void OnFinish(List<IWizardPage> pages, ref bool cancelAction)
        {
            switch (transferType)
            {
                case StoreTransferTypeEnum.Order:
                    OnTransferOrderFinish(pages, ref cancelAction);
                    break;
                case StoreTransferTypeEnum.Request:
                    OnTransferRequestFinish(pages, ref cancelAction);
                    break;
                default:
                    break;
            }
        }

        private void OnTransferRequestFinish(List<IWizardPage> pages, ref bool cancelAction)
        {
            switch (transferAction.Action)
            {
                case StoreTransferActionEnum.Manage:
                    PluginOperations.ShowStoreTransfersView(transferType);
                    break;
                case StoreTransferActionEnum.CreateNew:
                    try
                    {
                        NewStoreTransfer newTransferResult = (NewStoreTransfer)pages[1];

                        InventoryTransferRequest result = null;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage,
                                                             () => result = PluginOperations.CreateTransferRequestHeader(
                                                                                                   newTransferResult.SendingStoreID,
                                                                                                   newTransferResult.ReceivingStoreID,
                                                                                                   newTransferResult.Description,
                                                                                                   newTransferResult.ExpectedDelivery,
                                                                                                   true));
                        dlg.ShowDialog();


                        if (result != null)
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", result.ID, result);
                            PluginOperations.ShowStoreTransfersView(transferType, result.ID);
                        }                        
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                case StoreTransferActionEnum.GenerateFromOrder:
                    try
                    {
                        NewStoreTransfer newTransferFromOrderResult = (NewStoreTransfer)pages[2];

                        RecordIdentifier result = null;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage,
                                                             () => result = PluginOperations.CreateTransferRequestFromOrder(
                                                                                                   newTransferFromOrderResult.ExistingStoreTransferID,
                                                                                                   newTransferFromOrderResult.SendingStoreID,
                                                                                                   newTransferFromOrderResult.ReceivingStoreID,
                                                                                                   newTransferFromOrderResult.Description,
                                                                                                   newTransferFromOrderResult.ExpectedDelivery));
                        dlg.ShowDialog();


                        if (result != null)
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", result, null);
                            PluginOperations.ShowStoreTransfersView(transferType, result);
                        }                        
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                case StoreTransferActionEnum.GenerateFromRequest:
                    try
                    {
                        NewStoreTransfer newTransferFromRequestResult = (NewStoreTransfer)pages[2];

                        RecordIdentifier result = null;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage,
                                                             () => result = PluginOperations.CopyTransferRequest(                                                                                                   
                                                                                                   newTransferFromRequestResult.ExistingStoreTransferID,
                                                                                                   newTransferFromRequestResult.SendingStoreID,
                                                                                                   newTransferFromRequestResult.ReceivingStoreID,
                                                                                                   newTransferFromRequestResult.Description,
                                                                                                   newTransferFromRequestResult.ExpectedDelivery));
                        dlg.ShowDialog();


                        if (result != null)
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", result, null);
                            PluginOperations.ShowStoreTransfersView(transferType, result);
                        }                        
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                case StoreTransferActionEnum.GenerateFromFilter:
                    try
                    {
                        NewStoreTransfer newTransferFromFilterResult = (NewStoreTransfer)pages[2];
                        newTransferFromFilterResult.Filter.LimitRows = false;
                        newTransferFromFilterResult.Filter.LimitToFirst50Rows = false;

                        RecordIdentifier result = null;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage,
                                                             () => result = PluginOperations.CreateTransferRequestFromFilter(                                                                                                   
                                                                                                   newTransferFromFilterResult.SendingStoreID,
                                                                                                   newTransferFromFilterResult.ReceivingStoreID,
                                                                                                   newTransferFromFilterResult.Description,
                                                                                                   newTransferFromFilterResult.ExpectedDelivery,
                                                                                                   newTransferFromFilterResult.Filter));
                        dlg.ShowDialog();


                        if (result != null)
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", result, null);
                            PluginOperations.ShowStoreTransfersView(transferType, result);
                        }                        
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                case StoreTransferActionEnum.GenerateFromTemplate:
                    try
                    {
                        NewStoreTransfer newTransferFromTemplateResult = (NewStoreTransfer)pages[2];

                        InventoryTransferRequest headerRequest = new InventoryTransferRequest()
                        {
                            SendingStoreId = newTransferFromTemplateResult.SendingStoreID,
                            ReceivingStoreId = newTransferFromTemplateResult.ReceivingStoreID,
                            Text = newTransferFromTemplateResult.Description,
                            ExpectedDelivery = newTransferFromTemplateResult.ExpectedDelivery
                        };

                        RecordIdentifier result = null;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage,
                                                             () => result = PluginOperations.CreateTransferRequestFromTemplate(headerRequest, newTransferFromTemplateResult.Template));
                        dlg.ShowDialog();


                        if (result != null)
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", result, null);
                            PluginOperations.ShowStoreTransfersView(transferType, result);
                        }
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnTransferOrderFinish(List<IWizardPage> pages, ref bool cancelAction)
        {
            switch (transferAction.Action)
            {
                case StoreTransferActionEnum.Manage:
                    PluginOperations.ShowStoreTransfersView(transferType);
                    break;
                case StoreTransferActionEnum.CreateNew:
                    try
                    {
                        NewStoreTransfer newTransferResult = (NewStoreTransfer)pages[1];

                        InventoryTransferOrder result = null;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage,
                                                             () => result = PluginOperations.CreateTransferOrderHeader(
                                                                                                   newTransferResult.SendingStoreID,
                                                                                                   newTransferResult.ReceivingStoreID,
                                                                                                   newTransferResult.Description,
                                                                                                   newTransferResult.ExpectedDelivery, 
                                                                                                   true));
                        dlg.ShowDialog();

                        if (result != null)
                        {
                            NotifyAndDisplay(result.ID);
                        }                        
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                case StoreTransferActionEnum.GenerateFromOrder:
                    try
                    {
                        NewStoreTransfer newTransferFromOrderResult = (NewStoreTransfer)pages[2];

                        RecordIdentifier result = RecordIdentifier.Empty;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage, 
                                                             () => result = PluginOperations.CopyTransferOrder(newTransferFromOrderResult.ExistingStoreTransferID,
                                                                                                   newTransferFromOrderResult.SendingStoreID,
                                                                                                   newTransferFromOrderResult.ReceivingStoreID,
                                                                                                   newTransferFromOrderResult.Description,
                                                                                                   newTransferFromOrderResult.ExpectedDelivery));
                        dlg.ShowDialog();

                        NotifyAndDisplay(result);                                         
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                case StoreTransferActionEnum.GenerateFromRequest:
                    try
                    { 
                        NewStoreTransfer newTransferFromRequestResult = (NewStoreTransfer)pages[2];

                        RecordIdentifier result = RecordIdentifier.Empty;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage, 
                                                              () => result = PluginOperations.CreateTransferOrderFromRequest(newTransferFromRequestResult.ExistingStoreTransferID,
                                                                                                   newTransferFromRequestResult.SendingStoreID,
                                                                                                   newTransferFromRequestResult.ReceivingStoreID,
                                                                                                   newTransferFromRequestResult.Description,
                                                                                                   newTransferFromRequestResult.ExpectedDelivery));
                        dlg.ShowDialog();

                        NotifyAndDisplay(result);                        
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                case StoreTransferActionEnum.GenerateFromFilter:
                    try
                    {
                        NewStoreTransfer newTransferFromFilterResult = (NewStoreTransfer)pages[2];
                        newTransferFromFilterResult.Filter.LimitRows = false;
                        newTransferFromFilterResult.Filter.LimitToFirst50Rows = false;

                        RecordIdentifier result = RecordIdentifier.Empty;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage, 
                                                              () => result = PluginOperations.CreateTransferOrderFromFilter(
                                                                                                   newTransferFromFilterResult.SendingStoreID,
                                                                                                   newTransferFromFilterResult.ReceivingStoreID,
                                                                                                   newTransferFromFilterResult.Description,
                                                                                                   newTransferFromFilterResult.ExpectedDelivery,
                                                                                                   newTransferFromFilterResult.Filter));
                        dlg.ShowDialog();

                        NotifyAndDisplay(result);                        
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                case StoreTransferActionEnum.GenerateFromTemplate:
                    try
                    {
                        NewStoreTransfer newTransferFromTemplateResult = (NewStoreTransfer)pages[2];

                        InventoryTransferOrder headerOrder = new InventoryTransferOrder()
                        {
                            SendingStoreId = newTransferFromTemplateResult.SendingStoreID,
                            ReceivingStoreId = newTransferFromTemplateResult.ReceivingStoreID,
                            Text = newTransferFromTemplateResult.Description,
                            ExpectedDelivery = newTransferFromTemplateResult.ExpectedDelivery,
                            TemplateID = newTransferFromTemplateResult.Template.TemplateID
                        };

                        RecordIdentifier result = null;
                        SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage,
                                                             () => result = PluginOperations.CreateTransferOrderFromTemplate(headerOrder, newTransferFromTemplateResult.Template));
                        dlg.ShowDialog();


                        NotifyAndDisplay(result);
                    }
                    catch
                    {
                        MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                    }
                    break;
                default:
                    break;
            }
        }

        private void NotifyAndDisplay(RecordIdentifier result, bool notifyData = true, bool showTransfersView = true)
        {
            if (result == null || result == RecordIdentifier.Empty)
            {
                return;
            }

            if (notifyData)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", result, null);                
            }

            if (showTransfersView)
            {
                PluginOperations.ShowStoreTransfersView(transferType, result);
            }
        }
    }
}
