using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    public class CardNumberSerie
    {
        public CardNumberSerie()
        {
            this.CardTypeID = RecordIdentifier.Empty;
            this.CardNumberFrom = "";
            this.CardNumberTo = "";
        }

        public RecordIdentifier CardTypeID { get; set; }

        public string CardNumberFrom { get; set; }

        public string CardNumberTo { get; set; }
    }
}
