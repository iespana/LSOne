using LSOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces.Constants;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;

namespace LSOne.Services
{
    public partial class TenderService : ITenderService
    {
        private ISettings settings = null;
        public TenderService()
        {
            ErrorText = "";
        }

        public virtual IErrorLog ErrorLog
        {
            set
            {
          
            }
        }

        public void Init(IConnectionManager entry)
        {
            
        }

        protected void GetSettings(IConnectionManager dataModel)
        {
            if (settings == null)
            {
                settings = (ISettings)dataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            }            
        }

        public virtual string GetTenderDetails(IConnectionManager dataModel, ITenderLineItem tenderLine)
        {
            //If the tender line is a change back line then display the description as a detail as the "Tender name" variable will
            //display a "Change back" text
            if (tenderLine.TypeOfTender != TenderTypeEnum.DepositTender && tenderLine.Amount < 0)
            {                
                return tenderLine.Description;
            }           

            else if (tenderLine.TypeOfTender == TenderTypeEnum.TenderLine && tenderLine.ForeignCurrencyAmount != 0 && tenderLine.ForeignCurrencyAmount != tenderLine.CompanyCurrencyAmount)
            {
                return Interfaces.Services.RoundingService(dataModel).RoundString(dataModel, tenderLine.ForeignCurrencyAmount, tenderLine.CurrencyCode, false, CacheType.CacheTypeTransactionLifeTime) + " " + tenderLine.CurrencyCode;
            }
            
            else if (tenderLine.TypeOfTender == TenderTypeEnum.CardTender)
            {
                string cardExpiryDate = "";
                if (((CardTenderLineItem)tenderLine).ExpiryDate.Length == 4)
                {
                    cardExpiryDate = ((CardTenderLineItem)tenderLine).ExpiryDate.Insert(2, "/");
                }

                if (!string.IsNullOrEmpty(cardExpiryDate))
                {
                    return Properties.Resources.ExpiryDate + ": " + cardExpiryDate;
                }

                return "";
            }

            else if (tenderLine.TypeOfTender == TenderTypeEnum.CustomerTender)
            {
                return ((CustomerTenderLineItem)tenderLine).CustomerId;
            }

            else if (tenderLine.TypeOfTender == TenderTypeEnum.GiftCertificateTender)
            {
                return ((GiftCertificateTenderLineItem)tenderLine).SerialNumber;
            }

            else if (tenderLine.TypeOfTender == TenderTypeEnum.CreditMemoTender)
            {
                return ((CreditMemoTenderLineItem)tenderLine).SerialNumber;
            }

            else if (tenderLine.TypeOfTender == TenderTypeEnum.LoyaltyTender)
            {                
                return Interfaces.Services.RoundingService(dataModel).RoundForReceipt(dataModel, ((LoyaltyTenderLineItem)tenderLine).Points * -1, 0)
                       + " " + Properties.Resources.Points;                
            }

            return "";
            
        }
    }
}
