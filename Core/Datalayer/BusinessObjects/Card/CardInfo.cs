using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Card
{
    /// <summary>
    /// Contains all information relevant to an card that has either been read with a stripe reader or by entering the values manually.
    /// </summary>
    [Serializable]
    public class CardInfo : DataEntity, ICloneable
    {
        private CardEntryTypesEnum cardEntryType;       // How did we receive the card number?  Manually or by sweeping the card?

        /// <remarks>
        /// Track2 of the magnetic stripe of the card - if the card was swept through a card reader.
        /// </remarks>
        private string track2 = "";
        private string[] trackParts;

        /// <remarks>
        /// The card number if the information was entered manually.
        /// </remarks>
        private string cardNumber = "";
        private bool cardNumberHidden;

        /// <remarks>
        /// The expirydate if the information was entered manually.
        /// </remarks>
        private string expDate = "";

        private CardTypesEnum cardType; 

        private string binFrom = "";
        private string binTo = "";
        private int binLength;

        private bool modulusCheck;
        private bool expDateCheck;

        private bool processLocally;

        private string issuer = "";

        /// <remarks>
        /// Is it allowed to manually enter the card number.
        /// </remarks>
        private bool allowManualInput;
        /// <remarks>
        /// Is it allowed to manually enter the card number.
        /// </remarks>
        private string restrictionCode = "";
        /// <remarks>
        /// Should fleet information be entered.
        /// </remarks>
        private bool enterFleetInfo;

        private decimal cardFee;

        #region Constructor
        /// <summary>
        /// Initial values when the constructor is called.
        /// </summary>
        public CardInfo()
            : base()
        {
            cardNumber = "";
            expDate = "";
            binFrom = "";
            binTo = "";
            TenderTypeId = "";
            issuer = "";
            cardType = CardTypesEnum.Unknown;
        }
        #endregion

        public override object Clone()
        {
            var cardInfo = new CardInfo();
            Populate(cardInfo);
            return cardInfo;
        }

        public void Populate(CardInfo info)
        {
            base.Populate(info);
            info.CardEntryType = CardEntryType;
            info.Track2 = Track2;
            info.TrackParts = TrackParts;
            info.CardNumber = CardNumber;
            info.CardNumberHidden = CardNumberHidden;
            info.ExpDate = ExpDate;
            info.CardType = CardType;
            info.CardName = CardName;
            info.BinFrom = BinFrom;
            info.BinTo = BinTo;
            info.BinLength = BinLength;
            info.ModulusCheck = ModulusCheck;
            info.ExpDateCheck = ExpDateCheck;
            info.ProcessLocally = ProcessLocally;
            info.TenderTypeId = TenderTypeId;
            info.Issuer = Issuer;
            info.AllowManualInput = AllowManualInput;
            info.EnterFleetInfo = EnterFleetInfo;
            info.CardFee = CardFee;
            info.RestrictionCode = RestrictionCode;
        }   

        #region Properties

        /// <summary>
        /// How did we receive the card number?  Manually or by sweeping the card?
        /// </summary>
        public CardEntryTypesEnum CardEntryType
        {
            set { cardEntryType = value; }
            get { return cardEntryType; }
        }

        /// <summary>
        /// Track2 of the magnetic stripe of the card - if the card was swept through a card reader
        /// </summary>
        public string Track2
        {
            get { return track2; }
            set { track2 = value; }
        }

        /// <summary>
        /// TrackParts are the two parts of Track2 that is seperated by the card seperator character
        /// </summary>
        public string[] TrackParts
        {
            get { return trackParts; }
            set { trackParts = value; }
        }

        /// <summary>
        /// The card number if the information was entered manually.  
        /// </summary>
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        /// <summary>
        /// Is the cardnumber supposed to be hidden 
        /// </summary>
        public bool CardNumberHidden
        {
            get { return cardNumberHidden; }
            set { cardNumberHidden = value; }
        }

        /// <summary>
        /// The expirydate if the information was entered manually.  
        /// </summary>
        public string ExpDate
        {
            get { return expDate; }
            set { expDate = value; }
        }

        public CardTypesEnum CardType
        {
            get
            {
                return cardType;
            }
            set
            {
                cardType = value;
            }
        }

        public string CardName
        {
            get
            {
                return Text;
            }
            set
            {
                Text = value;
            }
        }

        public string BinFrom
        {
            get
            {
                return binFrom;
            }
            set
            {
                binFrom = value;
            }
        }

        public string BinTo
        {
            get
            {
                return binTo;
            }
            set
            {
                binTo = value;
            }
        }

        public int BinLength
        {
            get
            {
                return binLength;
            }
            set
            {
                binLength = value;
            }
        }

        public bool ModulusCheck
        {
            get
            {
                return modulusCheck;
            }
            set
            {
                modulusCheck = value;
            }
        }

        public bool ExpDateCheck
        {
            get
            {
                return expDateCheck;
            }
            set
            {
                expDateCheck = value;
            }
        }

        public bool ProcessLocally
        {
            get
            {
                return processLocally;
            }
            set
            {
                processLocally = value;
            }
        }

        public RecordIdentifier TenderTypeId {get; set;}

        public string Issuer
        {
            get
            {
                return issuer;
            }
            set
            {
                issuer = value;
            }
        }

        public bool AllowManualInput
        {
            get
            {
                return allowManualInput;
            }
            set
            {
                allowManualInput = value;
            }
        }

        public bool EnterFleetInfo
        {
            get
            {
                return enterFleetInfo;
            }
            set
            {
                enterFleetInfo = value;
            }
        }

        public decimal CardFee
        {
            get { return cardFee; }
            set { cardFee = value; }
        }

        public string RestrictionCode
        {
            get { return restrictionCode; }
            set { restrictionCode = value; }
        }

        public int TypeOfCardIndex
        {
            get
            {
                int index = 0;

                foreach (CardTypesEnum type in Enum.GetValues(typeof(CardTypesEnum)))
                {
                    if (type == CardType)
                    {
                        return index;
                    }

                    index++;
                }

                return -1;
            }
            set
            {
                CardType = (CardTypesEnum)(Enum.GetValues(typeof(CardTypesEnum)).GetValue(value));
            }
        }
        #endregion
    }
}
