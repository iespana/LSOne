namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum ReasonActionEnum
    {
        MainInventory = 0,
        ParkedInventory = 1,
        StockReservation = 2,
        Adjustment = 3
    }

    public static class ReasonActionHelper
    {
        public static string ReasonActionEnumToString(ReasonActionEnum action)
        {
            switch(action)
            {
                case ReasonActionEnum.MainInventory:
                    return Properties.Resources.ReasonActionMainInventory;
                case ReasonActionEnum.ParkedInventory:
                    return Properties.Resources.ReasonActionParkedInventory;
                case ReasonActionEnum.StockReservation:
                    return Properties.Resources.ReasonActionStockReservation;
                case ReasonActionEnum.Adjustment:
                    return Properties.Resources.ReasonActionAdjustment;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// It returns the equivalent reason code action to the given journal type.
        /// </summary>
        /// <param name="journalType"></param>
        /// <returns></returns>
        public static ReasonActionEnum GetEquivalent(InventoryJournalTypeEnum journalType)
        {
            switch(journalType)
            {
                case InventoryJournalTypeEnum.Adjustment:
                    return ReasonActionEnum.Adjustment;
                case InventoryJournalTypeEnum.Reservation:
                    return ReasonActionEnum.StockReservation;
                case InventoryJournalTypeEnum.Parked:
                    return ReasonActionEnum.ParkedInventory;
                default:
                    throw new System.ArgumentOutOfRangeException("Given journal type has to equivalence to any reason code action");
            }
        }
    }
}