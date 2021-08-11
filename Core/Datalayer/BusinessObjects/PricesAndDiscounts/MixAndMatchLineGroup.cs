#if !MONO
#endif
using System;
using System.Drawing;
using LSOne.Utilities.DataTypes;


namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class MixAndMatchLineGroup : DataEntity
    {
        RecordIdentifier offerID;
        RecordIdentifier lineGroup;

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(offerID, lineGroup);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public RecordIdentifier OfferID
        {
            get { return offerID; }
            set { offerID = value; }
        }
        
        public RecordIdentifier LineGroup
        {
            get { return lineGroup; }
            set { lineGroup = value; }
        }

        public int NumberOfItemsNeeded { get; set; }
#if !MONO
        public Color Color { get; set; }
#endif
    }
}
