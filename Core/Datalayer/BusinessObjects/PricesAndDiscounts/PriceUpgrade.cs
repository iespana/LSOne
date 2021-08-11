namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class PriceUpgrade : TradeAgreementEntry        
    {

        public PriceUpgrade()
            : base()
        {
            Upgrade = false;
        }

        public bool Upgrade { get; set; }
    
    }
}
