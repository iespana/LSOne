using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface ILabelService : IService
    {
        /// <summary>
        /// The last error that occurred while printing
        /// </summary>
        string LastErrorMessage { get; }

        /// <summary>
        /// Gets a list of all available macros for the specified data entity
        /// </summary>
        /// <returns>A list of available macros, or null if none</returns>
        List<string> GetAvailableMacros<T>() where T : IDataEntity;

        /// <summary>
        /// Print labels for the specific entities using the specific template
        /// </summary>
        /// <param name="entry">Connection to database</param>
        /// <param name="requests">A list of label print requests</param>
        /// <returns>True if printed, false otherwise</returns>
        bool Print(IConnectionManager entry, List<LabelPrintRequest> requests);

        /// <summary>
        /// Add the lael print request to the print queue
        /// </summary>
        /// <param name="entry">Connection to database</param>
        /// <param name="batch">Name of batch</param>
        /// <param name="requests">A list of label print requests</param>
        void AddToPrintQueue(IConnectionManager entry, string batch, List<LabelPrintRequest> requests);

        /// <summary>
        /// Print all unprinted labels from the batch
        /// </summary>
        /// <param name="entry">Connection to database</param>
        /// <param name="batch">The id of the batch, or empty to print all unprinted labels</param>
        /// <returns>True if printed, false otherwise</returns>
        bool PrintFromQueue(IConnectionManager entry, string batch);
    }

    public class LabelPrintRequest
    {
        public string Printer { get; set; }
        public int NumberOfLabels { get; set; }
        public LabelTemplate Template { get; set; }
        public IDataEntity Entity { get; set; }
    }
}
