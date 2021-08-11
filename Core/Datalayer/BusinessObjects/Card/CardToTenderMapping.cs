using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Card
{
    public class CardToTenderMapping : DataEntity
    {
        public CardToTenderMapping()
        :base()
        {
            BrokerID = RecordIdentifier.Empty;
        }
        [RecordIdentifierValidation(30)]
        public RecordIdentifier BrokerID { get; set; }
    }
}
