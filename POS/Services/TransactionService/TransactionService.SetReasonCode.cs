using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.Services
{
    public partial class TransactionService
    {
		public void SetReasonCode(IConnectionManager entry, ISession session, IPosTransaction transaction)
        {
            ISettings settings = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));
            // View the return dialog
            if (settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
            {
                LinkedList<ReturnedItemReason> returnedItems = new LinkedList<ReturnedItemReason>();
                DialogResult result = Services.Interfaces.Services.DialogService(entry).ShowReturnItemsDialog(entry, (RetailTransaction)transaction.Clone(), ReturnTransactionDialogBehaviourEnum.SetReasonCode, false, "", ref returnedItems);

                if (result == DialogResult.OK)
                {
                    ISaleLineItem itemToUpdate;

                    foreach (ReturnedItemReason item in returnedItems)
                    {
                        itemToUpdate = ((RetailTransaction)transaction).GetItem(item.LineNum);
                        itemToUpdate.ReasonCode = item.ReasonCode;
                    }
                }
            }
        }
    }
}
