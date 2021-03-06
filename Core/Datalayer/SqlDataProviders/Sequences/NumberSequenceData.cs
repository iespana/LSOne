using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using Permission = LSOne.DataLayer.GenericConnector.Permission;

namespace LSOne.DataLayer.SqlDataProviders.Sequences
{
    public class NumberSequenceData : SqlServerDataProviderBase, INumberSequenceData
    {
        // Needs to be static since this class can be generated by a factory which means that multiple threads
        // could be getting numbersequences in parallel with different instances of NumberSeuquenceData
        private static object threadLock = new object();

        private static string BaseSql(string defaultNextRec, string conditions)
        {
            return string.Format(@"select N.NUMBERSEQUENCE, 
                        ISNULL(N.TXT,'') as TXT, 
                        ISNULL(N.LOWEST,0) as LOWEST, 
                        ISNULL(N.HIGHEST,0) as HIGHEST, 
                        ISNULL(V.NEXTREC,{0}) as NEXTREC, 
                        ISNULL(N.FORMAT,'') as FORMAT, 
                        N.EMBEDSTOREID, 
                        N.EMBEDTERMINALID, 
                        ISNULL(N.CANBEDELETED, 0) as CANBEDELETED, 
                        ISNULL(N.WRAPAROUND, 0) as WRAPAROUND 
                        from NUMBERSEQUENCETABLE N
                        left outer join NUMBERSEQUENCEVALUE V on N.NUMBERSEQUENCE=V.NUMBERSEQUENCE AND N.DATAAREAID=V.DATAAREAID AND N.STOREID=V.STOREID
                        where N.DATAAREAID =  @dataAreaId {1} and N.STOREID = 'HO'",
                defaultNextRec, conditions);
        }
        private static string ResolveSort(NumberSequenceSorting sort, bool backwards)
        {
            string sortString = "";

            switch (sort)
            {
                case NumberSequenceSorting.ID:
                    sortString = "NUMBERSEQUENCE ASC";
                    break;
                case NumberSequenceSorting.Description:
                    sortString = "TXT ASC";
                    break;
                case NumberSequenceSorting.Highest:
                    sortString = "HIGHEST ASC";
                    break;
            }

            if (backwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }
            return sortString;
        }

        private NumberSequence GetSequenceRecord(IConnectionManager entry, RecordIdentifier sequenceID)
        {
            var sequence = Get(entry, sequenceID);

            if (sequence == null)
                return null;

            if (sequence.NextRecord > sequence.Highest)
            {
                sequence.Format += "#";
                sequence.Highest = sequence.Highest * 10;

                InternalSave(entry, sequence);
            }
            return sequence;
        }

        public virtual RecordIdentifier GenerateNumberFromSequence(IConnectionManager entry, ISequenceable sequenceProvider)
        {
            lock (threadLock)
            {
                RecordIdentifier sequenceID = sequenceProvider.SequenceID;

                // 1. Fetch the number sequence record
                // ----------------------------------------------------------------------------------
                NumberSequence sequence = GetSequenceRecord(entry, sequenceID);
                if (sequence == null)
                {
                    return RecordIdentifier.Empty;
                }

                // 2 Generate the new ID
                // ----------------------------------------------------------------------------------
                string newID = "";
                while (newID == "")
                {
                    newID = GenerateNumberFromSequence(sequence.Format, sequence.NextRecord);
                    if (entry.CurrentTerminalID != RecordIdentifier.Empty && sequence.EmbedTerminalID)
                    {
                        newID = entry.CurrentTerminalID + newID;
                    }

                    if (entry.CurrentStoreID != RecordIdentifier.Empty && sequence.EmbedStoreID)
                    {
                        newID = entry.CurrentStoreID + "-" + newID;
                    }

                    var existed = sequenceProvider.SequenceExists(entry, newID);
                    if (!existed)
                        continue;

                    sequence.NextRecord++;

                    if (sequence.NextRecord > sequence.Highest)
                    {
                        return RecordIdentifier.Empty;
                    }

                    newID = "";
                }

                // 4 Increment the Rec counter in the database
                // ----------------------------------------------------------------------------------
                sequence.NextRecord++;

                SaveValue(entry, (string) sequenceID, sequence.NextRecord);

                return newID;
            }
        }

        public virtual List<RecordIdentifier> GenerateNumbersFromSequence(IConnectionManager entry, ISequenceable sequenceProvider, int numOfRecords)
        {
            lock(threadLock)
            {
                RecordIdentifier sequenceID = sequenceProvider.SequenceID;

                // 1. Fetch the number sequence record
                // ----------------------------------------------------------------------------------
                NumberSequence sequence = GetSequenceRecord(entry, sequenceID);
                if (sequence == null)
                {
                    return null;
                }

                // 2 Generate the new ID
                // ----------------------------------------------------------------------------------
                string newID = "";
                List<RecordIdentifier> generatedIdentifiers = new List<RecordIdentifier>();

                string fullFormat = sequence.Format;
                if (entry.CurrentTerminalID != RecordIdentifier.Empty && sequence.EmbedTerminalID)
                {
                    fullFormat = entry.CurrentTerminalID + fullFormat;
                }

                if (entry.CurrentStoreID != RecordIdentifier.Empty && sequence.EmbedStoreID)
                {
                    fullFormat = entry.CurrentStoreID + "-" + fullFormat;
                }

                List<RecordIdentifier> existingIdentifier = sequenceProvider.GetExistingSequences(entry, fullFormat, sequence.NextRecord, numOfRecords);

                for(int i = 0; i < numOfRecords; i++)
                {
                    while (newID == "")
                    {
                        newID = GenerateNumberFromSequence(sequence.Format, sequence.NextRecord);
                        if (entry.CurrentTerminalID != RecordIdentifier.Empty && sequence.EmbedTerminalID)
                        {
                            newID = entry.CurrentTerminalID + newID;
                        }

                        if (entry.CurrentStoreID != RecordIdentifier.Empty && sequence.EmbedStoreID)
                        {
                            newID = entry.CurrentStoreID + "-" + newID;
                        }

                        var existed = existingIdentifier.Contains(newID);

                        sequence.NextRecord++;

                        if (sequence.NextRecord > sequence.Highest)
                        {
                            sequence = GetSequenceRecord(entry, sequenceID); //Increase max
                        }

                        if (!existed)
                        {
                            generatedIdentifiers.Add(newID);
                            continue;
                        }

                        newID = "";
                    }

                    newID = "";
                }

                // 4 Increment the Rec counter in the database
                // ----------------------------------------------------------------------------------
                SaveValue(entry, (string)sequenceID, sequence.NextRecord);

                return generatedIdentifiers;
            }
        }

        public virtual void ReturnNumberToSequence(IConnectionManager entry, RecordIdentifier sequenceID, RecordIdentifier sequenceNumber)
        {
            lock (threadLock)
            {
                string number = "";

                // 1. Fetch the number sequence record
                // ----------------------------------------------------------------------------------
                NumberSequence sequence = GetSequenceRecord(entry, sequenceID);
                if (sequence == null || String.IsNullOrEmpty((string) sequenceNumber))
                {
                    return;
                }

                // Check for embedded store ID and add the store ID so that the format matches the
                // actual sequencenumber that is being passed in
                string format = sequence.Format;

                if (sequence.EmbedTerminalID)
                {
                    format = entry.CurrentTerminalID + format;
                }

                if (sequence.EmbedStoreID)
                {
                    format = entry.CurrentStoreID + "-" + format;
                }

                for (int i = 0; i < ((string) sequenceNumber).Length; i++)
                {
                    if (format[i] == '#')
                    {
                        number += ((string) sequenceNumber)[i];
                    }
                }

                SaveValue(entry, (string) sequenceID, Convert.ToInt32(number));
            }
        }

        /// <summary>
        /// Gets a list of number sequences ordered by description
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <returns>A list of number sequences</returns>
        public virtual List<NumberSequence> GetList(IConnectionManager entry)
        {
            return GetList<NumberSequence>(entry, "NUMBERSEQUENCETABLE", "TXT", "NUMBERSEQUENCE", "TXT");
        }

        private static void PopulateNumberSequence(IDataReader dr, NumberSequence numberSequence)
        {
            numberSequence.ID = (string)dr["NUMBERSEQUENCE"];
            numberSequence.Text = (string)dr["TXT"];
            //numberSequence.Lowest = (int)dr["LOWEST"];
            numberSequence.Highest = (int)dr["HIGHEST"];
            numberSequence.NextRecord = (int)dr["NEXTREC"];
            numberSequence.Format = (string)dr["FORMAT"];
            numberSequence.EmbedStoreID = ((byte)dr["EMBEDSTOREID"] == 1);
            numberSequence.EmbedTerminalID = !DBNull.Value.Equals(dr["EMBEDTERMINALID"]) && ((byte)dr["EMBEDTERMINALID"] == 1);
            numberSequence.CanBeDeleted = ((byte)dr["CANBEDELETED"] == 1);
            //numberSequence.WrapAround = AsBool(dr["WRAPAROUND"]);
        }

        /// <summary>
        /// Gets a number sequence with the given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="numberSequenceID">The id of the number sequence to get</param>
        /// <returns>The number sequence with the given ID, null if no sequence is found</returns>
        public virtual NumberSequence Get(IConnectionManager entry, RecordIdentifier numberSequenceID)
        {
            ValidateSecurity(entry);

            List<NumberSequence> results = null;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql("-1", "and N.NUMBERSEQUENCE = @numberSequenceID");

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "numberSequenceID", (string)numberSequenceID);

                results = Execute<NumberSequence>(entry, cmd, CommandType.Text, PopulateNumberSequence);
            }

            // Do this outside of the using - so we only one active connection 
            if (results.Count > 0)
            {
                // Create a value row if one didn't exist
                if (results[0].NextRecord == -1)
                {
                    //if (results[0].Lowest > 0)
                    //    results[0].NextRecord = results[0].Lowest;
                    //else
                        results[0].NextRecord = 0;

                    SaveValue(entry, (string)numberSequenceID, results[0].NextRecord);
                }

                return results[0];
            }

            return null;
        }

        /// <summary>
        /// Gets all number sequences
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">Enum defining how to sort the results.</param>
        /// <param name="backwardsSort">Whether to sort backwards or not</param>
        /// <returns>A list of all numbersequences found</returns>
        public virtual List<NumberSequence> Get(IConnectionManager entry, NumberSequenceSorting sortEnum, bool backwardsSort)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql("0", "") + " order by " + ResolveSort(sortEnum, backwardsSort);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<NumberSequence>(entry, cmd, CommandType.Text, PopulateNumberSequence);
            }
        }

        /// <summary>
        /// Checks if a number sequence with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="numberSequenceID">The ID of the number sequence to check for</param>
        /// <returns>Whether a number sequence with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier numberSequenceID)
        {
            return RecordExists(entry, "NUMBERSEQUENCETABLE", new string[] { "NUMBERSEQUENCE", "STOREID" }, new RecordIdentifier(numberSequenceID, "HO"));
        }

        /// <summary>
        /// Checks if a number sequence value with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="numberSequenceID">The ID of the number sequence to check for</param>
        /// <returns>Whether a number sequence with a given ID exists in the database</returns>
        public virtual bool ExistsValue(IConnectionManager entry, RecordIdentifier numberSequenceID)
        {
            return RecordExists(entry, "NUMBERSEQUENCEVALUE", new string[] { "NUMBERSEQUENCE", "STOREID" }, new RecordIdentifier(numberSequenceID, "HO"));
        }

        /// <summary>
        /// Deletes the number sequence with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="numberSequenceID">The ID of the number sequence to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier numberSequenceID)
        {
            try
            {
                DeleteRecord(entry, "NUMBERSEQUENCEVALUE", "NUMBERSEQUENCE", numberSequenceID, Permission.AdministrationEditNumberSequences);
            }
            catch { }
            DeleteRecord(entry, "NUMBERSEQUENCETABLE", "NUMBERSEQUENCE", numberSequenceID, Permission.AdministrationEditNumberSequences);                            
        }

        /// <summary>
        /// Saves a number sequence. Doing Insert or update depending on if the record with the given key exists or not.
        /// </summary>
        /// <remarks>Requires the 'Edit number sequences' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="numberSequence">The number sequence to be saved</param>
        public virtual void Save(IConnectionManager entry, NumberSequence numberSequence)
        {
            ValidateSecurity(entry, Permission.AdministrationEditNumberSequences);

            InternalSave(entry, numberSequence);
        }

        private void InternalSave(IConnectionManager entry, NumberSequence numberSequence)
        {
            var statement = entry.Connection.CreateStatement("NUMBERSEQUENCETABLE");

            if (!Exists(entry, numberSequence.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("NUMBERSEQUENCE", (string)numberSequence.ID);                                
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("NUMBERSEQUENCE", (string)numberSequence.ID);
            }

            statement.AddField("TXT", numberSequence.Text);
            statement.AddField("HIGHEST", numberSequence.Highest, SqlDbType.Int);
            statement.AddField("FORMAT", numberSequence.Format);
            statement.AddField("CANBEDELETED", numberSequence.CanBeDeleted ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("EMBEDSTOREID", numberSequence.EmbedStoreID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("EMBEDTERMINALID", numberSequence.EmbedTerminalID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("STOREID", (string)numberSequence.StoreID);

            entry.Connection.ExecuteStatement(statement);

            SaveValue(entry, (string) numberSequence.ID, numberSequence.NextRecord);
        }

        private static string GenerateNumberFromSequence(string format, int nextNumber)
        {
            int digitCount = 0;
            int handled = 0;
            string finalResult = "";

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == '#')
                {
                    digitCount++;
                }
            }

            string result = nextNumber.ToString();
            int nextNumberLength = result.Length;

            for (int i = 0; i < (digitCount - nextNumberLength); i++)
            {
                result = "0" + result;
            }

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == '#')
                {
                    finalResult += result[handled];
                    handled++;
                }
                else
                {
                    finalResult += format[i];
                }
            }

            return finalResult;
        }

        private void SaveValue(IConnectionManager entry, string sequenceID, int value)
        {
            bool exists = ExistsValue(entry, sequenceID);

            var statement = entry.Connection.CreateStatement("NUMBERSEQUENCEVALUE", StatementType.Update, false);
            if (!exists)
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("NUMBERSEQUENCE", (string)sequenceID);
                statement.AddKey("STOREID", "HO");
            }
            else
            {
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("NUMBERSEQUENCE", (string) sequenceID);
                statement.AddCondition("STOREID", "HO");
            }
            statement.AddField("NEXTREC", value, SqlDbType.Int);
            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SetNumberSequenceLowest(IConnectionManager entry, RecordIdentifier numberSequenceID, int? lowest)
        {
            var statement = entry.Connection.CreateStatement("NUMBERSEQUENCEVALUE", StatementType.Update, false);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("NUMBERSEQUENCE", (string)numberSequenceID);
            if (lowest != null)
            {
                statement.AddField("NEXTREC", (int)lowest, SqlDbType.Int);
            }
            entry.Connection.ExecuteStatement(statement);
        }
    }
}
