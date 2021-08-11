using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Sequences
{
    public interface INumberSequenceData : IDataProviderBase<NumberSequence>
    {
        RecordIdentifier GenerateNumberFromSequence(IConnectionManager entry, ISequenceable sequenceProvider);
        List<RecordIdentifier> GenerateNumbersFromSequence(IConnectionManager entry, ISequenceable sequenceProvider, int numOfRecords);
        void ReturnNumberToSequence(IConnectionManager entry, RecordIdentifier sequenceID, RecordIdentifier sequenceNumber);

        /// <summary>
        /// Gets a list of number sequences ordered by description
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <returns>A list of number sequences</returns>
        List<NumberSequence> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a number sequence with the given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="numberSequenceID">The id of the number sequence to get</param>
        /// <returns>The number sequence with the given ID, null if no sequence is found</returns>
        NumberSequence Get(IConnectionManager entry, RecordIdentifier numberSequenceID);

        /// <summary>
        /// Gets all number sequences
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">Enum defining how to sort the results.</param>
        /// <param name="backwardsSort">Whether to sort backwards or not</param>
        /// <returns>A list of all numbersequences found</returns>
        List<NumberSequence> Get(IConnectionManager entry, NumberSequenceSorting sortEnum, bool backwardsSort);

        /// <summary>
        /// Checks if a number sequence with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="numberSequenceID">The ID of the number sequence to check for</param>
        /// <returns>Whether a number sequence with a given ID exists in the database</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier numberSequenceID);

        /// <summary>
        /// Deletes the number sequence with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="numberSequenceID">The ID of the number sequence to delete</param>
        void Delete(IConnectionManager entry, RecordIdentifier numberSequenceID);

        /// <summary>
        /// Saves a number sequence. Doing Insert or update depending on if the record with the given key exists or not.
        /// </summary>
        /// <remarks>Requires the 'Edit number sequences' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="numberSequence">The number sequence to be saved</param>
        void Save(IConnectionManager entry, NumberSequence numberSequence);

        void SetNumberSequenceLowest(IConnectionManager entry, RecordIdentifier numberSequenceID, int? lowest);
    }
}