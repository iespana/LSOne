using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.ErrorHandling;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.Services
{
    public partial class PharmacyService : IPharmacyService
    {
        public PharmacyService()
        {

        }

        public void Init(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            DLLEntry.Settings = settings;
#pragma warning restore 0612, 0618
            // You can get pharmacy host and pharmacy port from:
            // settings.HardwareProfile.PharmacyHost
            // settings.HardwareProfile.PharmacyPort
        }

        public virtual IErrorLog ErrorLog
        {
            set
            {

            }
        }

        /// <summary>
        /// Add a prescription to a retail transactions
        /// </summary>
        /// <param name="prescriptionId">The prescription id to cancel</param>
        /// <param name="retailTransaction">The retail transaction to add the prescription items to</param>
        public virtual IRetailTransaction AddPrescription(IConnectionManager entry, string prescriptionId, IRetailTransaction retailTransaction, OperationInfo operationInfo)
        {
            /*
             * NOTE! If the barcode is the first one scanned into an empty sale it is possible that the transaction is of type InternalTransaction 
             * and this code would need to create a Retail transaction and return it. 
             * */

            throw new Exception("This external pharmacy service has not been implemented.");
        }

        /// <summary>
        /// Cancel the prescription in the transaction.
        /// </summary>
        /// <param name="prescriptionId">The prescription id to cancel</param>
        /// <returns>Returns true if the cancelation was successful, else false.</returns>
        public virtual void CancelPrescription(IConnectionManager entry, string prescriptionId, IRetailTransaction retailTransaction)
        {
            throw new Exception("This external pharmacy service has not been implemented.");
        }

        /// <summary>
        /// Cancel all the pharmacy prescriptions in the retail transaction
        /// </summary>
        /// <param name="retailTransaction">The retail transaction</param>
        public virtual void CancelAllPrescriptions(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            throw new Exception("This external pharmacy service has not been implemented.");
        }

        /// <summary>
        /// Mark the prescription as paid.
        /// </summary>
        /// <param name="prescriptionId">The prescription id</param>
        /// <returns>Returns true if the confirmation successful</returns>
        public virtual bool PayPrescription(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            throw new Exception("This external pharmacy service has not been implemented.");
        }

        /// <summary>
        /// Is the prescriptiond id found in the database.
        /// </summary>
        /// <param name="prescriptionId">The prescription id</param>
        /// <returns>Returns true if a prescription id is found, else false</returns>
        public virtual bool IsPrescriptionFound(IConnectionManager entry, string prescriptionId, OperationInfo operationInfo)
        {
            throw new Exception("This external pharmacy service has not been implemented.");
        }

        public virtual string SelectPrescription(IConnectionManager entry, PrescriptionStatus prescriptionStatus, OperationInfo operationInfo)
        {
            throw new Exception("This external pharmacy service has not been implemented.");
        }

        public virtual string SelectPrescription(IConnectionManager entry, PrescriptionStatus prescriptionStatus, string rowFilter, OperationInfo operationInfo)
        {
            throw new Exception("This external pharmacy service has not been implemented.");
        }

        public virtual bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction)
        {
            //ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            return false;
        }

    }
}
