using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Core.Exceptions;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services
{
    public partial class EFTService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">Information about the EFT operation and properties</param>
        /// <param name="posTransaction">The current transaction</param>
        public virtual void GetBatchAmount(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        /// <summary>
        /// Returns the current batch number
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">Information about the EFT operation and properties</param>
        /// <param name="posTransaction">The current transaction</param>
        public virtual void GetCurrentBatchNumber(IConnectionManager entry, IEFTInfo eftInfo, IPosTransaction posTransaction)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }

        /// <summary>
        /// Closes and increments the batch
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="eftInfo">Information about the EFT operation and properties</param>
        /// <param name="posTransaction">The current transaction</param>
        public virtual void IncrementBatchNumber(IConnectionManager entry)
        {
            // Error!  The connection to the EFT service provider has not been implemented
            throw new POSException(50000, new Exception("The connection to an EFT service provider has not been implemented."));
        }
    }
}
