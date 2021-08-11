using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces
{
    public interface IPharmacyService : IService
    {
        /// <summary>
        /// Add a prescription to a retail transactions.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="prescriptionId">The prescription id to add to the transaction. Can f.ex. come from a pharmacy barcode</param>
        /// <param name="retailTransaction">The retail transaction to add the prescription items to.</param>
        /// <param name="operationInfo"></param>
        IRetailTransaction AddPrescription(IConnectionManager entry, string prescriptionId, IRetailTransaction retailTransaction, OperationInfo operationInfo);

        /// <summary>
        /// Cancel the prescription in the transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="prescriptionId">The prescription id to cancel.</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        void CancelPrescription(IConnectionManager entry, string prescriptionId, IRetailTransaction retailTransaction);

        /// <summary>
        /// Cancel all the pharmacy prescriptions in the retail transaction.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction</param>
        void CancelAllPrescriptions(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Mark the prescription as paid.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail transaction.</param>
        /// <returns>Returns true if the confirmation was successful.</returns>
        bool PayPrescription(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Is the prescriptiond id found in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="prescriptionId">The prescription id.</param>
        /// <param name="operationInfo"></param>
        /// <returns>Returns true if a prescription id is found, else false.</returns>
        bool IsPrescriptionFound(IConnectionManager entry, string prescriptionId, OperationInfo operationInfo);

        /// <summary>
        /// Select a prescription from a list of all valid prescriptions.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="prescriptionStatus">The status of prescriptions to select from.</param>
        /// <param name="operationInfo"></param>
        /// <returns>The selected prescription id.</returns>
        string SelectPrescription(IConnectionManager entry, PrescriptionStatus prescriptionStatus, OperationInfo operationInfo);

        /// <summary>
        /// Select a prescription containing a certain string from a list of all valid prescriptions.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="prescriptionStatus"></param>
        /// <param name="rowFilter"></param>
        /// <param name="operationInfo"></param>
        /// <returns></returns>
        string SelectPrescription(IConnectionManager entry, PrescriptionStatus prescriptionStatus, string rowFilter, OperationInfo operationInfo);

        /// <summary>
        /// Returns true if the Site service is needed to conclude a transaction that includes phramacy information
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <returns></returns>
        bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction);
    }
}
