using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Triggers
{
    public class DiscountTriggers : IDiscountTriggers
    {
        #region Constructor - Destructor

        public DiscountTriggers(IConnectionManager entry)
        {
            
            // Get all text through the Translation function in the ApplicationLocalizer
            // TextID's for DiscountTriggers are reserved at 53000 - 53999
        }

        ~DiscountTriggers()
        {

        }

        #endregion

        #region IDiscountTriggers Members

        public void PreLineDiscountAmount(IConnectionManager entry, PreTriggerResults results, IPosTransaction transaction, int LineId)
        {
           
        }

        public void PostLineDiscountAmount(IConnectionManager entry, IPosTransaction transaction)
        {

        }

        public void PreLineDiscountPercent(IConnectionManager entry, PreTriggerResults results, IPosTransaction transaction, int LineId)
        {
            
        }

        public void PostLineDiscountPercent(IConnectionManager entry, IPosTransaction transaction)
        {

        }

        #endregion
    }
}
