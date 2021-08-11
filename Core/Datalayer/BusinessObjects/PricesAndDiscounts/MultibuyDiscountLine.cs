using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    /// <summary>
    /// Data entity class for a configuration line on a Multibuy discount
    /// </summary>
    public class MultibuyDiscountLine : DataEntity
    {
        RecordIdentifier offerID;
        RecordIdentifier minQuantity;

        /// <summary>
        /// The default constructor
        /// </summary>
        public MultibuyDiscountLine()
        {
            offerID = "";
            minQuantity = 0.0M;
        }

        /// <summary>
        /// Gets the ID of the entity. In this case the MultibuyDiscountLine has a double primary key, OfferID, MinQuantity
        /// The setter has be suppressed and will throw NotImplementedException if used.
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(offerID, minQuantity);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the OfferID
        /// </summary>
        public RecordIdentifier OfferID
        {
            get { return offerID; }
            set { offerID = value; }
        }

        /// <summary>
        /// Gets or sets the minimum quantity
        /// </summary>
        public RecordIdentifier MinQuantity
        {
            get { return minQuantity; }
            set { minQuantity = value; }
        }

        /// <summary>
        /// Gets or sets the property that represents price or discount depending on type.
        /// </summary>
        public decimal PriceOrDiscountPercent {get; set;}

        public override object Clone()
        {
            var clone = new MultibuyDiscountLine
            {
                OfferID = OfferID,
                MinQuantity = MinQuantity,
                PriceOrDiscountPercent = PriceOrDiscountPercent
            };

            return clone;
        }
    }
}
