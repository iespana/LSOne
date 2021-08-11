namespace LSOne.DataLayer.BusinessObjects.TaxFree
{
    public class TaxRefundRange : DataEntity
    {
        public decimal ValueFrom { get; set; }
        public decimal ValueTo { get; set; }
        public decimal TaxValue { get; set; }
        public decimal TaxRefund { get; set; }
        public decimal TaxRefundPercentage { get; set; }

        public new object Clone()
        {
            var item = new TaxRefundRange();
            Populate(item);
            return item;
        }

        protected virtual void Populate(TaxRefundRange item)
        {
            base.Populate(item);
            item.ValueFrom = ValueFrom;
            item.ValueTo = ValueTo;
            item.TaxValue = TaxValue;
            item.TaxRefund = TaxRefund;
            item.TaxRefundPercentage = TaxRefundPercentage;
        }
    }
}
