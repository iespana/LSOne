using System;
using System.Windows.Forms;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface IDDService : IService
    {
        void RunJobUnsecure(Guid jobid, string jobText, Control control);
        JobValidationResult RunJob(JscJob job, Control control, bool forceNormal = false);
        JobValidationResult RunJob(JscJob job, Control control, bool forceNormal, JscSchedulerLog log, JscLocation location);

        void ReadDesign(Guid locationId, bool readTablesAndFields, bool updateExistingDesign, string newDescription, RecordIdentifier designID, JscLocation locationItem);

        /// <summary>
        /// Run the post transaction DD job configured in the functionality profile
        /// </summary>
        /// <param name="transaction">Current concluded transaction</param>
        void RunPostTransactionJob(IPosTransaction transaction);
    }
}
