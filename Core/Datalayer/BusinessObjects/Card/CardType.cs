using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Card
{
    public class CardType : DataEntity
    {
        public CardTypesEnum CardTypes { get; set; }

        public CardType()
            : base()
        {
            CardTypes = CardTypesEnum.Unknown;
        }
    }
}
