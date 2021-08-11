using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    /// <summary>
    /// This service determines what kind of cards is being swiped into the POS.
    /// In the current releases this should only be used for gift cards/loyalty card and such. NOT credit cards
    /// </summary>
    public partial class CardService : ICardService
    {
        /// <summary>
        /// Access to the error log functionality
        /// </summary>
        public virtual IErrorLog ErrorLog
        {
            set
            {
            }
        }

        /// <summary>
        /// Initializes the barcode service and sets the database connection for the service
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        public void Init(IConnectionManager entry)
        {
          
        }



        #region ICard Members

        /// <summary>
        /// Finds a specific card from the information in the card swiped
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardInfo">The information about the card that is being swiped</param>
        /// <returns>Information about the card that is being swiped</returns>
        public virtual CardInfo GetCardType(IConnectionManager entry, CardInfo cardInfo)
        {

            List<CardInfo> allCardTypes = GetAllCardTypes(entry);

            CardInfo correctCardType = FindCorrectCardType(cardInfo, allCardTypes);
            if (correctCardType != null)
            {

                correctCardType.CardNumber = cardInfo.CardNumber;
                correctCardType.Track2 = cardInfo.Track2;
                correctCardType.CardEntryType = cardInfo.CardEntryType;

                //cardInfo = correctCardType;
                return correctCardType;
            }

            return cardInfo;
        }

        /// <summary>
        /// Checks if entered cardnumber is of a valid length.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardNumber">The card number as a string.</param>
        /// <returns>Returns true if a card number is of a valid length, else false.</returns>
        public virtual bool IsCardLengthValid(IConnectionManager entry, string cardNumber)
        {            
            return true;
        }

        /// <summary>
        /// Checks if entered expiration date is valid or or not.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="expirationDate">The expirationdate as mmyy</param>
        /// <returns>Returns true if expiration date is valid, else false.</returns>
        public virtual bool IsExpiryDateValid(IConnectionManager entry, string expirationDate)
        {
            bool valueOK = true;
            if (expirationDate.Length == 4)
            {
                int month = Convert.ToInt16(expirationDate.Substring(0, 2));
                int year = Convert.ToInt16(expirationDate.Substring(2, 2));
                int currYear = DateTime.Now.Year - 2000;
                int currMonth = DateTime.Now.Month;

                // Validating the month integrity
                valueOK = ((valueOK) && (month > 0));
                valueOK = ((valueOK) && (month <= 12));
                if (valueOK)
                {
                    // Validating the card date
                    valueOK = ((valueOK) && (year > currYear));
                    if (!valueOK)
                        valueOK = ((year == currYear) && (month >= currMonth));
                }
            }
            else
                valueOK = false;
            return valueOK;
        }
        #endregion
        
        /// <summary>
        /// Figure out which card was swiped into the POS
        /// </summary>
        /// <param name="swipedCard">Information about the card that was swiped</param>
        /// <param name="cardTypes">The card types that are available</param>
        /// <returns></returns>
        protected virtual CardInfo FindCorrectCardType(CardInfo swipedCard, List<CardInfo> cardTypes)
        {
            CardInfo result = null;

            Int64 binTo = 0;
            Int64 binFrom = 0;
            int binLength = 0;
            Int64 cardSubString = 0;

            foreach (CardInfo cardInfo in cardTypes)
            {
                if (cardInfo.BinFrom == "" || cardInfo.BinTo == "")
                {
                    continue;
                }
                Int64.TryParse(cardInfo.BinTo, out binTo);
                Int64.TryParse(cardInfo.BinFrom, out binFrom);
                binLength = Convert.ToInt32(cardInfo.BinLength);

                if (!string.IsNullOrEmpty(swipedCard.Track2))
                {
                    if (swipedCard.Track2.Length >= binLength)
                    {
                        Int64.TryParse(swipedCard.Track2.Substring(0, binLength), out cardSubString);
                    }                        
                }
                else
                {
                    swipedCard.CardNumber = swipedCard.CardNumber.Replace("-", "");
                    if (swipedCard.CardNumber.Length >= binLength)
                    {
                        Int64.TryParse(swipedCard.CardNumber.Substring(0, binLength), out cardSubString);
                    }
                    else
                    {
                        if (swipedCard.CardNumber.Trim() != "")
                        {
                            Int64.TryParse(swipedCard.CardNumber, out cardSubString);
                        }
                        else
                        {
                            cardSubString = 0;
                        }
                    }
                }


                if ((binFrom <= cardSubString) && (cardSubString <= binTo))
                {
                    // The card number is within this bin serie.
                    result = cardInfo;
                    break;
                }

            }

            return result;
        }

        /// <summary>
        /// Returns a list of all card types available
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        public virtual List<CardInfo> GetAllCardTypes(IConnectionManager entry)
        {
            return Providers.CardInfoData.GetAll(entry); 
        }
          
    }
}
