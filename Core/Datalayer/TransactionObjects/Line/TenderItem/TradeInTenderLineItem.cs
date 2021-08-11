using System;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    /// <summary>
    /// A class handling payments with trade in items.
    /// </summary>
    [Serializable]
    public class TradeInTenderLineItem : TenderLineItem
    {
        public override object Clone()
        {
            TradeInTenderLineItem item = new TradeInTenderLineItem();
            Populate(item);
            return item;
        }
    }
}
