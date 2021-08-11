using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services
{
    internal class CloneTransactions : ICloneTransactions
    {
        public IPosTransaction CloneTransaction(IConnectionManager dataModel, IPosTransaction transactionToClone)
        {
            IPosTransaction newTx = (IPosTransaction)transactionToClone.Clone();
            ClonePartnerObject(dataModel, newTx);
            CloneEFTExtraInfo(dataModel, newTx);
            CloneEFTTransactionExtraInfo(dataModel, newTx);
            return newTx;
        }

        public void ClonePartnerObject(IConnectionManager dataModel, IPosTransaction posTransaction)
        {
            IRetailTransaction transaction = posTransaction as IRetailTransaction;
            if (transaction != null)
            {
                transaction.PartnerObject = Services.Interfaces.Services.ApplicationService(dataModel).PartnerObject;
                if (transaction.PartnerObject != null && transaction.PartnerXElement != null)
                {
                    transaction.PartnerObject.ToClass(transaction.PartnerXElement);
                }                                
            }
        }        

        public void CloneEFTExtraInfo(IConnectionManager dataModel, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)dataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (settings.HardwareProfile.EftConnected)
            {
               if(posTransaction is IRetailTransaction retailTransaction)
                {
                    foreach (var tenderLine in retailTransaction.TenderLines)
                    {
                        if (tenderLine is ICardTenderLineItem cardTenderLine)
                        {
                            if (cardTenderLine.EFTInfo.EFTExtraInfoXElement == null) continue;
                            cardTenderLine.EFTInfo.EFTExtraInfo = Interfaces.Services.EFTService(dataModel).EFTExtraInfo;
                            cardTenderLine.EFTInfo.EFTExtraInfo?.ToClass(cardTenderLine.EFTInfo.EFTExtraInfoXElement, dataModel.ErrorLogger);
                        }
                    }

                    foreach (var tenderLine in retailTransaction.IOriginalTenderLines)
                    {
                        if (tenderLine is ICardTenderLineItem cardTenderLine)
                        {
                            if (cardTenderLine.EFTInfo.EFTExtraInfoXElement == null) continue;
                            cardTenderLine.EFTInfo.EFTExtraInfo = Interfaces.Services.EFTService(dataModel).EFTExtraInfo;
                            cardTenderLine.EFTInfo.EFTExtraInfo?.ToClass(cardTenderLine.EFTInfo.EFTExtraInfoXElement, dataModel.ErrorLogger);
                        }
                    }
                }
            }
        }

        public void CloneEFTTransactionExtraInfo(IConnectionManager dataModel, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)dataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if(settings.HardwareProfile.EftConnected && posTransaction is IRetailTransaction retailTransaction)
            {
                if(retailTransaction.EFTTransactionExtraInfoXElement != null)
                {
                    retailTransaction.EFTTransactionExtraInfo = Interfaces.Services.EFTService(dataModel).EFTTransactionExtraInfo;
                    retailTransaction.EFTTransactionExtraInfo?.ToClass(retailTransaction.EFTTransactionExtraInfoXElement, dataModel.ErrorLogger);
                }
            }
        }
    }
}
