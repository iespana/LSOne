using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
    /// <summary>
    /// A interface for data providers to implement if they want to support sequenses
    /// </summary>
    public interface ISequenceable
    {
        /// <summary>
        /// Return true if the passed in sequence id exists.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Sequence ID to check for</param>
        /// <returns>True if the ID exists, else false</returns>
        bool SequenceExists(IConnectionManager entry,RecordIdentifier id);

        List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords);

        /// <summary>
        /// In this function the ID of the sequence for the given type should be returned. This would be the ID from the Sequence table
        /// </summary>
        RecordIdentifier SequenceID
        {
            get;
        }
    }
}
