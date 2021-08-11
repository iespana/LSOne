using System;
using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DDDataProviders
{
    public class ReplicationCountersFilter
    {
        public Guid? JobId { get; set; }
        public Guid? SubJobId { get; set; }
        public Guid? LocationId { get; set; }
    }

    public class ReplicationCounterListItem
    {
        public JscRepCounter JscRepCounter { get; set; }
        public string JobText { get; set; }
        public string SubJobText { get; set; }
        public string LocationText { get; set; }
    }

    public class SubJobListItem
    {
        public JscSubJob JscSubJob { get; set; }
        public string TableFromName { get; set; }
    }
    public interface IJobData : IDataProviderBase<JscJob>
    {
        string JobSubJobGetString { get; }
        string JobTriggerGetString { get; }
        void Save(IConnectionManager entry, JscJob job);
        void Save(IConnectionManager entry, JscSubJobFromTableFilter subJobFromTableFilter);
        void Save(IConnectionManager entry, JscSubJob subJob);
        void Save(IConnectionManager entry, JscSchedulerLog schedulerLog);
        void Save(IConnectionManager entry, JscRepCounter jscRepCounter);
        void Save(IConnectionManager entry, JscJobSubjob jobSubjob);
        void Delete(IConnectionManager entry, IEnumerable<JscSubJob> subJobs);
        void Delete(IConnectionManager entry, IEnumerable<JscJobSubjob> jobSubjobs);
        void Delete(IConnectionManager entry, IEnumerable<JscRepCounter> countersToRemove);
        void Delete(IConnectionManager entry, JscJobTrigger trigger);
        void Delete(IConnectionManager entry, IEnumerable<JscJob> jobs);
        void Delete(IConnectionManager entry, JscJob job);
        JscJob GetJob(IConnectionManager entry, RecordIdentifier jobID, bool populateItems = true);
        IEnumerable<JscJob> GetJobs(IConnectionManager entry, bool includeDisabled);
        IEnumerable<JscJob> GetJobsPopulated(IConnectionManager entry, bool includeDisabled);
        IEnumerable<JscJob> GetJobsSimple(IConnectionManager entry, bool includeDisabled);
        DataSelector[] GetJobSelectorList(IConnectionManager entry);
        JscSubJob GetSubJob(IConnectionManager entry, RecordIdentifier id);
        IEnumerable<JscSubJob> GetSubJobs(IConnectionManager entry);
        IEnumerable<SubJobListItem> GetSubJobListItems(IConnectionManager entry, bool includeDisabled);
        List<JscSubJobFromTableFilter> GetSubJobFromTableFiltersList(IConnectionManager entry, JscSubJob subjob);
        DataSelector[] GetSubJobSelectorList(IConnectionManager entry);
        JscJobSubjob GetJobSubJob(IConnectionManager entry, RecordIdentifier jobSubjobId);
        List<JscJobSubjob> GetJobSubJobs(IConnectionManager entry, JscJob job);

        /// <summary>
        /// Returns the latest Sequence used in any JobSubJob for the supplied job
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="jobID"></param>
        /// <returns></returns>
        int GetMaxJobSubjobSequence(IConnectionManager entry, RecordIdentifier jobID);

        IEnumerable<JscSubJob> GetSubJobCandidates(IConnectionManager entry, RecordIdentifier fromTableDatabaseDesignId, RecordIdentifier jobId);
        IEnumerable<JscSchedulerLog> GetJobLog(IConnectionManager entry, DateTime fromDate, DateTime toDate, RecordIdentifier jobId);
        IEnumerable<JscSchedulerSubLog> GetSubJobLog(IConnectionManager entry, RecordIdentifier jobLogId);
        IEnumerable<JscRepCounter> GetReplicationCounters(IConnectionManager entry, RecordIdentifier subJobId);
        IEnumerable<ReplicationCounterListItem> GetReplicationCounterListItems(IConnectionManager entry, ReplicationCountersFilter repCounterFilter);

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="jobID">The unique ID of the job to check on</param>
        bool Exists(IConnectionManager entry, RecordIdentifier jobID);

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="subJobID">The unique ID of the subjob to check on</param>
        bool SubJobExists(IConnectionManager entry, RecordIdentifier subJobID);

        /// <summary>
        /// Returns true if the job has any subjobs.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        bool IsJobUsedForSubjobs(IConnectionManager entry, RecordIdentifier jobId);

        /// <summary>
        /// Returns true if any JobSubJobs use the supplied Subjob
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="subJobID"></param>
        /// <returns></returns>
        bool IsSubjobUsedInJobs(IConnectionManager entry, RecordIdentifier subJobID);

        /// <summary>
        /// Runs a series of validations for the job Returning OK if no errors, a result indicating 
        /// the source of the failure otherwise.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        JobValidationResult ValidateForRun(IConnectionManager entry, JscJob job);

        /// <summary>
        /// Searches for SubJobs that match searchString
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="searchString"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="beginsWith"></param>
        /// <param name="fromTableDatabaseDesignId"></param>
        /// <param name="jobID"></param>
        /// <returns></returns>
        IEnumerable<JscSubJob> SearchSubJobsForJob(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, RecordIdentifier fromTableDatabaseDesignId, RecordIdentifier jobID);

        /// <summary>
        /// Gets a list of all SubJobs that belong to the given job
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="jobID">The ID of the job to get the subjobs for</param>
        /// <returns>A list of populated <see cref="JscSubJob"/> objects</returns>
        List<JscSubJob> GetJobSubjobValues(IConnectionManager entry, RecordIdentifier jobID);

        /// <summary>
        /// Gets the maximum value of the replication counter column in the given table.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tableName">The name of the table to get the value from</param>
        /// <param name="replicationCounterColumnName">The name of the column that's used for the replication counter</param>
        /// <returns></returns>
        int GetMaxReplicationCounterValue(IConnectionManager entry, string tableName, string replicationCounterColumnName);
    }
}