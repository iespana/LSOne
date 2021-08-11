using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Panels;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class StoreTransferDialog : TouchBaseForm
    {
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private IPosTransaction currentTransaction;
        private OperationInfo operationInfo;
        private StoreTransferWrapper transferWrapper;

        private TransferHeaderPanel headerPanel;
        private TransferItemsPanel itemsPanel;
        private Control activePanel;

        public RecordIdentifier TransferID => transferWrapper.ID;

        public StoreTransferDialog(IConnectionManager entry, StoreTransferWrapper transferWrapper, IPosTransaction transaction, OperationInfo operationInfo)
        {
            InitializeComponent();
            currentTransaction = transaction;
            this.operationInfo = operationInfo;
            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            this.transferWrapper = transferWrapper;
        }

        public void SetHeaderInfo()
        {
            StringBuilder sb = new StringBuilder();
            string seperator = "";
            switch (transferWrapper.TransferType)
            {
                case StoreTransferTypeEnum.Order:
                    switch (transferWrapper.TransferDirection)
                    {
                        case InventoryTransferType.Outgoing:
                            sb.Append(Properties.Resources.SendTransferOrder);
                            seperator = Properties.Resources.To;
                            break;
                        case InventoryTransferType.Incoming:
                            sb.Append(Properties.Resources.ReceiveTransferOrder);
                            seperator = Properties.Resources.From;
                            break;
                    }
                    break;
                case StoreTransferTypeEnum.Request:
                    sb.Append(Properties.Resources.RequestTransfer);
                    seperator = Properties.Resources.From;
                    break;
                default:
                    break;
            }

            if(!transferWrapper.IsNewTransfer)
            {
                sb.Append(" " + transferWrapper.ID);
            }

            string store = "";

            if (transferWrapper.IsNewTransfer)
            {
                store = headerPanel.GetHeaderStoreName();
            }
            else
            {
                store = transferWrapper.TransferType == StoreTransferTypeEnum.Order 
                    ? transferWrapper.TransferDirection == InventoryTransferType.Outgoing ? transferWrapper.TransferOrder.ReceivingStoreName :transferWrapper.TransferOrder.SendingStoreName
                    : transferWrapper.TransferRequest.ReceivingStoreName;
            }

            if(!string.IsNullOrWhiteSpace(store))
            {
                sb.Append(" " + seperator + " " + store);
            }

            td_Banner.BannerText = sb.ToString();
        }

        private void StoreTransferDialog_Load(object sender, EventArgs e)
        {
            if(transferWrapper.IsNewTransfer)
            {
                ShowHeaderPanel();
            }
            else
            {
                ShowItemsPanel();
            }

            SetHeaderInfo();
        }

        private void SetPanel(Control control)
        {
            control.Size = new Size(pnlControls.Width, pnlControls.Height);
            pnlControls.Controls.Clear();
            pnlControls.Controls.Add(control);
            activePanel = control;
            control.Select();
            control.Invalidate();
        }

        internal void ShowHeaderPanel()
        {
            if (activePanel == null || activePanel != headerPanel)
            {
                if (headerPanel == null)
                {
                    headerPanel = new TransferHeaderPanel(dlgEntry, transferWrapper);
                    headerPanel.Location = new Point(0, 0);
                }

                SetPanel(headerPanel);
            }
        }

        internal void ShowItemsPanel()
        {
            if (activePanel == null || activePanel != itemsPanel)
            {
                if (itemsPanel == null)
                {
                    if (!transferWrapper.IsNewTransfer)
                    {
                        itemsPanel = new TransferItemsPanel(dlgEntry, transferWrapper, currentTransaction, operationInfo);
                    }
                    else
                    {
                        //Cannot initialize item panel without a valid transfer object
                        return;
                    }

                    itemsPanel.Location = new Point(0, 0);
                }

                SetPanel(itemsPanel);
            }
        }

        private void StoreTransferDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            itemsPanel?.TransferItemsPanel_FormClosed(sender, e);
        }
    }
}
