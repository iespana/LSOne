using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class DiscountParameters : DataEntity
    {
        public DiscountParameters()
        {
            PeriodicLine = PeriodicLineEnum.Max;
            PeriodicTotal = PeriodicTotalEnum.Accumulative;
            LineTotal = CustomerLineTotalEnum.Accumulative;
        }


        public PeriodicLineEnum PeriodicLine { get; set; }
        public PeriodicTotalEnum PeriodicTotal { get; set; }
        public CustomerLineTotalEnum LineTotal { get; set; }
    }
}
