using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.DataLayer.DDBusinessObjects
{

    /// <summary>
    /// Sets the compression mode to be used when running this job
    /// </summary>
    public enum DDCompressionMode
    {
        /// <summary>
        /// Means that the job should not override the value set in the DD Configuration Tool
        /// </summary>
        UseDDSetting,
        None,
        ZipStream,
        ZipFile,
        SevenZip
    }

    public enum DDIsolationLevel
    {
        /// <summary>
        /// Means that the job should not override the value set in the DD Configuration Tool
        /// </summary>
        UseDDSetting = 0,

        Unspecified = 1,
        //
        // Summary:
        //     The pending changes from more highly isolated transactions cannot be overwritten.
        Chaos = 2,
        //
        // Summary:
        //     A dirty read is possible, meaning that no shared locks are issued and no exclusive
        //     locks are honored.
        ReadUncommitted = 3,
        //
        // Summary:
        //     Shared locks are held while the data is being read to avoid dirty reads, but
        //     the data can be changed before the end of the transaction, resulting in non-repeatable
        //     reads or phantom data.
        ReadCommitted = 4,
        //
        // Summary:
        //     Locks are placed on all data that is used in a query, preventing other users
        //     from updating the data. Prevents non-repeatable reads but phantom rows are still
        //     possible.
        RepeatableRead = 5,
        //
        // Summary:
        //     A range lock is placed on the System.Data.DataSet, preventing other users from
        //     updating or inserting rows into the dataset until the transaction is complete.
        Serializable = 6,
        //
        // Summary:
        //     Reduces blocking by storing a version of data that one application can read while
        //     another is modifying the same data. Indicates that from one transaction you cannot
        //     see changes made in other transactions, even if you requery.
        Snapshot = 7
    }

    public class JscJob : DataEntity
    {
        private JscJob jscSubjobJob;
        private JscLocation jscDestinationLocation;
        private JscLocation jscSourceLocation;

        public JobType JobType { get; set; }
        public RecordIdentifier Source { get; set; }
        public RecordIdentifier Destination { get; set; }
        public ErrorHandling ErrorHandling { get; set; }
        public bool Enabled { get; set; }
        public RecordIdentifier SubjobJob { get; set; }
        public List<JscJobTrigger> JscJobTriggers { get; set; }
        public List<JscJobSubjob> JscJobSubjobs { get; set; }
        public string PluginPath { get; set; }
        public string PluginArguments { get; set; }
        public string JobTypeCode { get; set; }
        public bool UseCurrentLocation { get; set; }

        public JscLocation JscSourceLocation
        {
            get { return jscSourceLocation; }
            set
            {
                jscSourceLocation = value;

                Source = value == null ? Source : value.ID;
            }
        }

        public JscLocation JscDestinationLocation
        {
            get { return jscDestinationLocation; }
            set
            {
                jscDestinationLocation = value;

                Destination = value == null ? Destination : value.ID;
            }
        }

        public JscJob JscSubjobJob
        {
            get { return jscSubjobJob; }
            set
            {
                jscSubjobJob = value;

                SubjobJob = value == null ? SubjobJob : value.ID;
            }

        }

        public DDCompressionMode CompressionMode { get; set; }

        public DDIsolationLevel IsolationLevel { get; set; }
    }
}
