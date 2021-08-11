using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.Scheduler.DataProviders;
using LSRetail.DD.Common;

namespace LSOne.DataLayer.SqlDDDataProviders
{
    public class JobData : SqlServerDataProvider, IJobData
    {
    

        #region Select Strings

        public string JobSubJobGetString
        {
            get { return @" SELECT a.Id JSJId,
                                Job,
                                SubJob,
                                Sequence,
                                a.Enabled JSJEnabled,
                                jsj.Id,
                                jsj.Description,
                                jsj.TableFrom,
                                jsj.StoredProcName,
                                jsj.TableNameTo,
                                jsj.ReplicationMethod,
                                jsj.WhatToDo,
                                jsj.Enabled,
                                jsj.IncludeFlowFields,
                                jsj.ActionTable,
                                jsj.ActionCounterInterval,
                                jsj.MoveActions,
                                jsj.AlwaysExecute,
                                jsj.NoDistributionFilter,
                                jsj.RepCounterField,
                                jsj.RepCounterInterval,
                                jsj.UpdateRepCounter,
                                jsj.UpdateRepCounterOnEmptyInt,
                                jsj.MarkSentRecordsField,
                                jsj.FilterCodeFilter
                            FROM 
                                JscJobSubjobs a
                                join JscSubJobs jsj on a.subjob = jsj.id

                            "; }
        }

        public string JobTriggerGetString
        {
            get { return @" SELECT  
                            Id,
                            Job,
                            TriggerKind,
                            Enabled,
                            RecurrenceType,
                            Seconds,
                            Minutes,
                            Hours,
                            Months,
                            Years,
                            DaysOfMonth,
                            DaysOfWeek,
                            StartTime,
                            EndTime,
                            Tag
                                FROM JscJobTriggers a 
                       "; }
        }

        private static string SubJobGetString
        {
            get { return @"SELECT 
                            jsj.Id,
                            jsj.Description,
                            jsj.TableFrom,
                            jsj.StoredProcName,
                            jsj.TableNameTo,
                            jsj.ReplicationMethod,
                            jsj.WhatToDo,
                            jsj.Enabled,
                            jsj.IncludeFlowFields,
                            jsj.ActionTable,
                            jsj.ActionCounterInterval,
                            jsj.MoveActions,
                            jsj.AlwaysExecute,
                            jsj.NoDistributionFilter,
                            jsj.RepCounterField,
                            jsj.RepCounterInterval,
                            jsj.UpdateRepCounter,
                            jsj.UpdateRepCounterOnEmptyInt,
                            jsj.MarkSentRecordsField,
                            jsj.FilterCodeFilter
                    FROM JscSubJobs jsj"; }
        }

        private static string BaseJobSubJobGetString
        {
            get { return @"SELECT a.[Id],
                      a.[Job],
                      a.[SubJob],
                      a.[Sequence],
                      a.[Enabled]
                FROM [JscJobSubjobs] a"; }
        }

        private static string BaseGetString
        {
            get { return @"SELECT 
                            Id,
                            Description,
                            JobType,
                            Source,
                            Destination,
                            ErrorHandling,
                            Enabled,
                            SubjobJob, 
                            PluginPath, 
                            PluginArguments,
                            JobTypeCode,
                            UseCurrentLocation,
                            CompressionMode,
                            IsolationLevel
                      FROM JscJobs a"; }
        }

        #endregion

        #region Populators

        private static void PopulateJob(IDataReader dr, JscJob job)
        {
            job.Text = (string) dr["Description"];
            job.Destination = dr["Destination"] == DBNull.Value ? RecordIdentifier.Empty : (Guid) dr["Destination"];
            job.JobType = (JobType) ((Int16) (dr["JobType"]));
            job.Source = dr["Source"] == DBNull.Value ? RecordIdentifier.Empty : (Guid) dr["Source"];
            job.ErrorHandling = (ErrorHandling) ((Int16) dr["ErrorHandling"]);
            job.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            job.SubjobJob = dr["SubjobJob"] == DBNull.Value ? RecordIdentifier.Empty : (Guid) dr["SubjobJob"];
            job.ID = (Guid) dr["ID"];
            job.PluginPath =  dr["PluginPath"] == DBNull.Value ? string.Empty: (string) dr["PluginPath"];
            job.PluginArguments = dr["PluginArguments"] == DBNull.Value ? string.Empty : (string)dr["PluginArguments"];
            job.UseCurrentLocation = dr["UseCurrentLocation"] != DBNull.Value && (Convert.ToInt16(dr["UseCurrentLocation"]) != 0);
            job.JobTypeCode = dr["JobTypeCode"] == DBNull.Value ? string.Empty : (string) dr["JobTypeCode"];
            job.CompressionMode = dr["CompressionMode"] == DBNull.Value ? DDCompressionMode.UseDDSetting : (DDCompressionMode)((int) dr["CompressionMode"]);
            job.IsolationLevel = dr["IsolationLevel"] == DBNull.Value ? DDIsolationLevel.UseDDSetting : (DDIsolationLevel)((int)dr["IsolationLevel"]);
        }

        private static void PopulateJobTrigger(IDataReader dr, JscJobTrigger trigger)
        {
            trigger.DaysOfMonth = (string) dr["DaysOfMonth"];
            trigger.DaysOfWeek = (string) dr["DaysOfWeek"];
            trigger.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            trigger.EndTime = dr["EndTime"] == DBNull.Value ? null : (DateTime?) dr["EndTime"];
            trigger.Hours = (string) dr["Hours"];
            trigger.ID = (Guid) dr["Id"];
            trigger.Job = (Guid) dr["Job"];
            trigger.Minutes = (string) dr["Minutes"];
            trigger.Months = (string) dr["Months"];
            if (dr["RecurrenceType"] != DBNull.Value)
            {
                trigger.RecurrenceType = (RecurrenceType) ((Int16) dr["RecurrenceType"]);
            }
            trigger.Seconds = (string) dr["Seconds"];
            trigger.StartTime = dr["StartTime"] == DBNull.Value ? null : (DateTime?) dr["StartTime"];
            if (dr["Tag"] != DBNull.Value)
            {
                trigger.Tag = (string) dr["Tag"];
            }
            if (dr["TriggerKind"] == DBNull.Value)
            {
                trigger.TriggerKind = (TriggerKind) ((Int16) dr["TriggerKind"]);
            }
            trigger.Years = (string) dr["Years"];
        }

        private static void PopulateDataSelector(IDataReader dr, DataSelector selector)
        {
            selector.GuidId = (Guid) dr["ID"];
            selector.Text = (string) dr["Description"];
        }

        private static void PopulateSubJobFromTableFilter(IDataReader dr, JscSubJobFromTableFilter filter)
        {
            filter.ID = (Guid) dr["ID"];
            filter.SubJob = (Guid) dr["SubJob"];
            filter.Field = (Guid) dr["Field"];
            filter.FilterType = (LookupCode) ((Int16) dr["FilterType"]);
            filter.Value1 = DBNull.Value == dr["Value1"] ? null : (string) dr["Value1"];
            filter.Value2 = DBNull.Value == dr["Value2"] ? null : (string) dr["Value2"];
            filter.ApplyFilter = (LinkedFilterType) ((Int16) dr["ApplyFilter"]);
        }

        private static void PopulateISubJob(IDataReader dr, JscSubJob job)
        {
            job.Description = (string) dr["Description"];
            job.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            job.ID = (Guid) dr["ID"];
            job.IncludeFlowFields = (Convert.ToInt16(dr["IncludeFlowFields"]) != 0);
            if (dr["MarkSentRecordsField"] != DBNull.Value)
            {
                job.MarkSentRecordsField = (Guid) dr["MarkSentRecordsField"];
            }
            job.MoveActions = (Convert.ToInt16(dr["MoveActions"]) != 0);
            job.NoDistributionFilter = (Convert.ToInt16(dr["NoDistributionFilter"]) != 0);
            job.AlwaysExecute = Convert.ToBoolean(dr["AlwaysExecute"]);
            if (dr["RepCounterField"] != DBNull.Value)
            {
                job.RepCounterField = (Guid) dr["RepCounterField"];
            }
            job.RepCounterInterval = dr["RepCounterInterval"] == DBNull.Value ? 0 : (int) dr["RepCounterInterval"];
            job.ReplicationMethod = (ReplicationTypes) ((Int16) dr["ReplicationMethod"]);
            job.StoredProcName = dr["StoredProcName"] == DBNull.Value ? string.Empty : (string) dr["StoredProcName"];
            if (dr["TableFrom"] != DBNull.Value)
            {
                job.TableFrom = (Guid) dr["TableFrom"];
            }
            if (dr["ActionTable"] != DBNull.Value)
            {
                job.ActionTable = (Guid) dr["ActionTable"];
            }
            if (dr["ActionCounterInterval"] != DBNull.Value)
            {
                job.ActionCounterInterval = (int) dr["ActionCounterInterval"];
            }
            job.TableNameTo = dr["TableNameTo"] == DBNull.Value ? string.Empty : (string) dr["TableNameTo"];
            job.UpdateRepCounter = (Convert.ToInt16(dr["UpdateRepCounter"]) != 0);
            job.UpdateRepCounterOnEmptyInt = (Convert.ToInt16(dr["UpdateRepCounter"]) != 0);
            job.WhatToDo = (ModeDef) ((Int16) dr["WhatToDo"]);
            job.FilterCodeFilter = dr["FilterCodeFilter"] == DBNull.Value ? string.Empty : (string)dr["FilterCodeFilter"];
        }

        private static void PopulateSchedulerLog(IDataReader dr, JscSchedulerLog log)
        {
            log.Message = (string) dr["Message"];
            log.ID = (Guid) dr["Id"];
            log.Job = (Guid) dr["Job"];
            log.RegTime = (DateTime) dr["RegTime"];
           
        }

        private static void PopulateSchedulerSubLog(IDataReader dr, JscSchedulerSubLog log)
        {
            log.ID = (Guid)dr["Id"];
            log.JobLog =  (Guid)dr["SchedulerLogId"];
            log.SubJob =  (Guid)dr["SubJob"];
            log.Location =  (Guid)dr["Location"];
            log.ReplicationCounterEnd = (int)dr["ReplicationCounterEnd"];
            log.ReplicationCounterStart = (int)dr["ReplicationCounterStart"];
            log.EndTime = (DateTime) dr["EndTime"];
            log.StartTime = (DateTime) dr["StartTime"];
            log.RunAsNormal = (Convert.ToInt16(dr["RunAsNormal"]) != 0);


        }

        private static void PopulateReplicationListItem(IDataReader dr, ReplicationCounterListItem replicationCounterListItem)
        {
            replicationCounterListItem.JobText = (string) dr["JobText"];
            replicationCounterListItem.LocationText = (string) dr["LocationText"];
            replicationCounterListItem.SubJobText = (string) dr["SubJobText"];

            replicationCounterListItem.JscRepCounter = new JscRepCounter();
            PopulateRepCounters(dr, replicationCounterListItem.JscRepCounter);
        }

        private static void PopulateRepCounters(IDataReader dr, JscRepCounter repCounter)
        {
            repCounter.Job = (Guid) dr["Job"];
            repCounter.ID = (Guid) dr["ID"];
            repCounter.SubJob = (Guid) dr["SubJob"];
            repCounter.Location = (Guid) dr["Location"];
            repCounter.Counter = (int) dr["Counter"];
        }
        private static void PopulateJobSubJobNoObject(IDataReader dr, JscJobSubjob jobSubjob)
        {
            jobSubjob.ID = (Guid)dr["JSJID"];
            jobSubjob.JscJob = null;
            jobSubjob.JscSubJob = null;
            jobSubjob.Sequence = (int)dr["Sequence"];
            jobSubjob.Job = dr["Job"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["Job"];
            jobSubjob.SubJob = dr["SubJob"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["SubJob"];
            jobSubjob.Enabled = (Convert.ToInt16(dr["JSJENABLED"]) != 0);

           

        }
        private static void PopulateJobSubJob(IDataReader dr, JscJobSubjob jobSubjob)
        {
            jobSubjob.ID = (Guid) dr["JSJID"];
            jobSubjob.JscJob = null;
            jobSubjob.JscSubJob = null;
            jobSubjob.Sequence = (int)dr["Sequence"];
            jobSubjob.Job = dr["Job"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["Job"];
            jobSubjob.SubJob = dr["SubJob"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["SubJob"];
            jobSubjob.Enabled = (Convert.ToInt16(dr["JSJENABLED"]) != 0);

            var subjob  = new JscSubJob();
            PopulateISubJob(dr, subjob);
            jobSubjob.JscSubJob = subjob;

        }

        private static void PopulateSubJobListItem(IDataReader dr, SubJobListItem job)
        {
            job.TableFromName = dr["TableName"] == DBNull.Value ? string.Empty : (string) dr["TableName"];
            job.JscSubJob = new JscSubJob();
            PopulateISubJob(dr, job.JscSubJob);
        }

        #endregion

        #region Save And Delete

        public void Save(IConnectionManager entry, JscJob job)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            var statement = new SqlServerStatement("JscJobs");

            bool isNew = false;

            if (job.ID == RecordIdentifier.Empty || job.ID == null)
            {
                isNew = true;
                job.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, job.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("Id", (Guid)job.ID, SqlDbType.UniqueIdentifier);
                if (job.JscJobSubjobs != null)
                {
                    foreach (var jobSubJob in job.JscJobSubjobs)
                    {
                        jobSubJob.Job = job.ID;
                    }
                }
                if (job.JscJobTriggers != null)
                {
                    foreach (var jobTrigger in job.JscJobTriggers)
                    {
                        jobTrigger.Job = job.ID;
                    }
                }

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("Id", (Guid)job.ID, SqlDbType.UniqueIdentifier);

            }
            statement.AddField("Description", job.Text);
            statement.AddField("JobType", (int)job.JobType, SqlDbType.Int);
            statement.AddField("PluginPath", job.PluginPath);
            statement.AddField("PluginArguments", job.PluginArguments);
            statement.AddField("UseCurrentLocation", job.UseCurrentLocation ? 1 : 0, SqlDbType.TinyInt);
            if (job.UseCurrentLocation || job.JscSourceLocation == null)  
            {
                statement.AddField("Source", DBNull.Value, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("Source", (Guid) job.JscSourceLocation.ID, SqlDbType.UniqueIdentifier);
            }

            if (job.JscDestinationLocation != null)
            {
                statement.AddField("Destination", (Guid) job.JscDestinationLocation.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("Destination", DBNull.Value, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("Errorhandling", (int)job.ErrorHandling, SqlDbType.Int);
            statement.AddField("Enabled", job.Enabled ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("JobTypeCode", job.JobTypeCode);                            

            if (job.SubjobJob != null && Exists(entry, job.SubjobJob))
            {
                statement.AddField("SubJobjob", (string) job.SubjobJob);
            }
            else
            {
                statement.AddField("SubJobjob", DBNull.Value, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("CompressionMode", (int)job.CompressionMode, SqlDbType.Int);
            statement.AddField("IsolationLevel", (int)job.IsolationLevel, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
            if (job.JscJobTriggers != null)
            {
                foreach (var jscJobTrigger in job.JscJobTriggers)
                {
                    statement = new SqlServerStatement("JscJobTriggers");

                    if (jscJobTrigger.ID != null && RecordExists(entry, "JscJobTriggers", "Id", jscJobTrigger.ID))
                    {
                        statement.StatementType = StatementType.Update;

                        statement.AddCondition("Id", (Guid)jscJobTrigger.ID, SqlDbType.UniqueIdentifier);
                    }
                    else
                    {
                        statement.StatementType = StatementType.Insert;

                        jscJobTrigger.ID = Guid.NewGuid();
                        statement.AddKey("Id", (Guid)jscJobTrigger.ID, SqlDbType.UniqueIdentifier);
                    }
                    statement.AddField("Job", (Guid)job.ID, SqlDbType.UniqueIdentifier);
                    statement.AddField("TriggerKind", (int)jscJobTrigger.TriggerKind, SqlDbType.Int);
                    statement.AddField("Enabled", jscJobTrigger.Enabled ? 1 : 0, SqlDbType.TinyInt);
                    statement.AddField("RecurrenceType", (int)jscJobTrigger.RecurrenceType, SqlDbType.Int);
                    statement.AddField("Seconds", jscJobTrigger.Seconds);
                    statement.AddField("Minutes", jscJobTrigger.Minutes);
                    statement.AddField("Hours", jscJobTrigger.Hours);
                    statement.AddField("Months", jscJobTrigger.Months);
                    statement.AddField("Years", jscJobTrigger.Years);
                    statement.AddField("DaysOfMonth", jscJobTrigger.DaysOfMonth);
                    statement.AddField("DaysOfWeek", jscJobTrigger.DaysOfWeek);

                    if (jscJobTrigger.StartTime != null)
                        statement.AddField("StartTime", jscJobTrigger.StartTime, SqlDbType.DateTime);
                    else
                        statement.AddField("StartTime", DBNull.Value, SqlDbType.DateTime);
                    if (jscJobTrigger.EndTime != null)
                        statement.AddField("EndTime", jscJobTrigger.EndTime, SqlDbType.DateTime);
                    else
                        statement.AddField("EndTime", DBNull.Value, SqlDbType.DateTime);
                    statement.AddField("tag", jscJobTrigger.Tag);

                    entry.Connection.ExecuteStatement(statement);
                }

            }
            if (job.JscJobSubjobs != null && job.JscJobSubjobs.Count > 0)
            {
                foreach (var jscSubJob in job.JscJobSubjobs)
                {
                    Save(entry, jscSubJob);
                }
            }
        }

        public void Save(IConnectionManager entry, JscSubJobFromTableFilter subJobFromTableFilter)
        {

            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            var statement = new SqlServerStatement("JscSubJobFromTableFilters");

            bool isNew = false;

            if (subJobFromTableFilter.ID == RecordIdentifier.Empty || subJobFromTableFilter.ID == null)
            {
                isNew = true;
                subJobFromTableFilter.ID = Guid.NewGuid();
            }

            if (isNew || !RecordExists(entry, "JscSubJobFromTableFilters", "Id", subJobFromTableFilter.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", (Guid)subJobFromTableFilter.ID, SqlDbType.UniqueIdentifier);

            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Id", (Guid)subJobFromTableFilter.ID, SqlDbType.UniqueIdentifier);

            }
            statement.AddField("SubJob", (Guid)subJobFromTableFilter.SubJob, SqlDbType.UniqueIdentifier);
            statement.AddField("Field", (Guid)subJobFromTableFilter.Field, SqlDbType.UniqueIdentifier);
            statement.AddField("FilterType", subJobFromTableFilter.FilterType, SqlDbType.Int);
            statement.AddField("Value1", subJobFromTableFilter.Value1);
            statement.AddField("Value2", subJobFromTableFilter.Value2);
            statement.AddField("ApplyFilter", subJobFromTableFilter.ApplyFilter, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Save(IConnectionManager entry, JscSubJob subJob)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            var statement = new SqlServerStatement("JscSubJobs");


            bool isNew = false;

            if (subJob.ID == null || subJob.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                subJob.ID = Guid.NewGuid();
            }

            if (isNew || !SubJobExists(entry, subJob.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", (Guid)subJob.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("Id", (Guid)subJob.ID, SqlDbType.UniqueIdentifier);
            }


            statement.AddField("Description", subJob.Description);
            if (subJob.TableFrom != null)
            {
                statement.AddField("TableFrom", (string)subJob.TableFrom);
            }
            if (subJob.StoredProcName != null)
            {
                statement.AddField("StoredProcName", subJob.StoredProcName);
            }
            if (subJob.TableNameTo != null)
            {
                statement.AddField("TableNameTo", subJob.TableNameTo);
            }
            statement.AddField("ReplicationMethod", (int)subJob.ReplicationMethod, SqlDbType.Int);
            statement.AddField("WhatToDo", (int)subJob.WhatToDo, SqlDbType.Int);
            statement.AddField("Enabled", subJob.Enabled ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("IncludeFlowFields", subJob.IncludeFlowFields ? 1 : 0, SqlDbType.TinyInt);
            if (subJob.ActionTable != null)
            {
                statement.AddField("ActionTable", (string)subJob.ActionTable);
            }
            if (subJob.ActionCounterInterval != null)
            {
                statement.AddField("ActionCounterInterval", subJob.ActionCounterInterval, SqlDbType.Int);
            }
            statement.AddField("MoveActions", subJob.MoveActions ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("NoDistributionFilter", subJob.NoDistributionFilter ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("AlwaysExecute", subJob.AlwaysExecute, SqlDbType.Bit);
            if (subJob.RepCounterField != null)
            {
                statement.AddField("RepCounterField", (string)subJob.RepCounterField);
            }
            if (subJob.RepCounterInterval != null)
            {
                statement.AddField("RepCounterInterval", subJob.RepCounterInterval, SqlDbType.Int);
            }
            statement.AddField("UpdateRepCounter", subJob.UpdateRepCounter ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("UpdateRepCounterOnEmptyInt", subJob.UpdateRepCounterOnEmptyInt ? 1 : 0,
                               SqlDbType.TinyInt);
            if (subJob.MarkSentRecordsField != null && !subJob.MarkSentRecordsField.IsEmpty)
            {
                statement.AddField("MarkSentRecordsField", (string)subJob.MarkSentRecordsField);
            }

            statement.AddField("FilterCodeFilter", subJob.FilterCodeFilter);

            IConnectionManagerTransaction transaction = entry.CreateTransaction();

            try
            {


                transaction.Connection.ExecuteStatement(statement);
                if (subJob.JscSubJobFromTableFilters != null)
                {
                    foreach (JscSubJobFromTableFilter jscSubJobFromTableFilter in subJob.JscSubJobFromTableFilters)
                    {
                        Save(transaction, jscSubJobFromTableFilter);
                    }
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();

            }
        }

        public void Save(IConnectionManager entry, JscSchedulerLog schedulerLog)
        {
            var statement = new SqlServerStatement("JscSchedulerLog");

            bool isNew = false;

            if (schedulerLog.ID == RecordIdentifier.Empty || schedulerLog.ID == null)
            {
                isNew = true;
                schedulerLog.ID = Guid.NewGuid();
            }

            if (isNew || !RecordExists(entry, "JscSchedulerLog", "Id", schedulerLog.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("Id", (Guid)schedulerLog.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("Id", (Guid)schedulerLog.ID, SqlDbType.UniqueIdentifier);

            }
            statement.AddField("Job", (Guid)schedulerLog.Job, SqlDbType.UniqueIdentifier);
            statement.AddField("RegTime", schedulerLog.RegTime, SqlDbType.DateTime);
            statement.AddField("Message", schedulerLog.Message);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Save(IConnectionManager entry, JscRepCounter jscRepCounter)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            var statement = new SqlServerStatement("JscRepCounters");

            bool isNew = false;

            if (jscRepCounter.ID == null || jscRepCounter.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                jscRepCounter.ID = Guid.NewGuid();
            }

            if (isNew || !RecordExists(entry, "JscRepCounters", "Id", jscRepCounter.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", (Guid)jscRepCounter.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Id", (Guid)jscRepCounter.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("Job", (Guid)jscRepCounter.Job, SqlDbType.UniqueIdentifier);
            statement.AddField("Subjob", (Guid)jscRepCounter.SubJob, SqlDbType.UniqueIdentifier);
            statement.AddField("Location", (Guid)jscRepCounter.Location, SqlDbType.UniqueIdentifier);
            statement.AddField("Counter", jscRepCounter.Counter, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Save(IConnectionManager entry, JscJobSubjob jobSubjob)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);

            var statement = new SqlServerStatement("JscJobSubJobs");

            bool isNew = false;

            if (jobSubjob.ID == RecordIdentifier.Empty || jobSubjob.ID == null)
            {
                isNew = true;
                jobSubjob.ID = Guid.NewGuid();
            }

            if (isNew || !RecordExists(entry, "JscJobSubJobs", "Id", jobSubjob.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("Id", (Guid)jobSubjob.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("Id", (Guid)jobSubjob.ID, SqlDbType.UniqueIdentifier);

            }
            statement.AddField("Job", (Guid)jobSubjob.Job, SqlDbType.UniqueIdentifier);
            statement.AddField("SubJob", (Guid)jobSubjob.SubJob, SqlDbType.UniqueIdentifier);
            statement.AddField("Sequence", jobSubjob.Sequence, SqlDbType.Int);
            statement.AddField("Enabled", jobSubjob.Enabled ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Delete(IConnectionManager entry, IEnumerable<JscSubJob> subJobs)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            foreach (var subJob in subJobs)
            {
                Delete(entry, subJob);
            }
        }

        private static void Delete(IConnectionManager entry, JscSubJob subJob)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            IConnectionManagerTransaction transaction = entry.CreateTransaction();
            try
            {
                DeleteRecord(transaction, "JscRepCounters", "SubJob", subJob.ID, SchedulerPermissions.JobSubjobEdit);
                DeleteRecord(transaction, "JscSubJobs", "Id", subJob.ID, SchedulerPermissions.JobSubjobEdit);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();
            }
        }

        public void Delete(IConnectionManager entry, IEnumerable<JscJobSubjob> jobSubjobs)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            foreach (var jscJobSubjob in jobSubjobs)
            {

                DeleteRecord(entry, "JscJobSubjobs", "Id", jscJobSubjob.ID, SchedulerPermissions.JobSubjobEdit);
            }
        }

        public void Delete(IConnectionManager entry, IEnumerable<JscRepCounter> countersToRemove)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            foreach (var jscRepCounter in countersToRemove)
            {
                DeleteRecord(entry, "JscRepCounters", "Id", jscRepCounter.ID, SchedulerPermissions.JobSubjobEdit);
            }
        }

        public void Delete(IConnectionManager entry, JscJobTrigger trigger)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            DeleteRecord(entry, "JscJobTriggers", "Id", trigger.ID, SchedulerPermissions.JobSubjobEdit);
        }

        public void Delete(IConnectionManager entry, IEnumerable<JscJob> jobs)
        {
            foreach (var job in jobs)
            {
                Delete(entry, job);
            }
        }

        public void Delete(IConnectionManager entry, JscJob job)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
            IConnectionManagerTransaction transaction = entry.CreateTransaction();
            try
            {
                ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);
                if (job.JscJobTriggers != null)
                {
                    foreach (var jscJobTrigger in job.JscJobTriggers)
                    {
                        DeleteRecord(transaction, "JscJobTriggers", "Id", jscJobTrigger.ID, SchedulerPermissions.JobSubjobEdit);
                    }
                }
                if (job.JscJobSubjobs != null)
                {
                    foreach (var jscJobSubjob in job.JscJobSubjobs)
                    {
                        DeleteRecord(transaction, "JscJobSubjobs", "Id", jscJobSubjob.ID, SchedulerPermissions.JobSubjobEdit);
                    }
                }
                DeleteRecord(transaction, "JscRepCounters", "Job", job.ID, SchedulerPermissions.JobSubjobEdit);
                DeleteRecord(transaction, "JscSchedulerLog", "Job", job.ID, SchedulerPermissions.JobSubjobEdit);

                DeleteRecord(transaction, "JscJobs", "Id", job.ID, SchedulerPermissions.JobSubjobEdit);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();
            }
        }

        #endregion

        public JscJob GetJob(IConnectionManager entry, RecordIdentifier jobID, bool populateItems = true)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            JscJob result;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseGetString +
                    " where a.id = @jobID";

                MakeParam(cmd, "jobID", (Guid) jobID);

                var records = Execute<JscJob>(entry, cmd, CommandType.Text, PopulateJob);

                result = (records.Count > 0) ? records[0] : null;
            }
            if (result != null && populateItems)
            {
                using (var cmd = entry.Connection.CreateCommand())
                {

                    cmd.CommandText =
                        JobTriggerGetString +
                        @" 
                        where a.Job = @jobId";

                    MakeParam(cmd, "jobId", (Guid) jobID);

                    var JobTriggers = Execute<JscJobTrigger>(entry, cmd, CommandType.Text, PopulateJobTrigger);

                    result.JscJobTriggers = JobTriggers;
                }
                result.JscJobSubjobs = GetJobSubJobs(entry, result);
                if (result.Source != null && !result.Source.IsEmpty)
                {
                    result.JscSourceLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(entry, result.Source);

                }
                if (result.Destination != null && !result.Destination.IsEmpty)
                {
                    result.JscDestinationLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(entry, result.Destination);

                }
            }

            return result;
        }

        public IEnumerable<JscJob> GetJobsSimple(IConnectionManager entry, bool includeDisabled)
        {
            IEnumerable<JscJob> result;

            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseGetString;


                if (!includeDisabled)
                {
                    cmd.CommandText += " Where a.Enabled = 1";
                }

                result = Execute<JscJob>(entry, cmd, CommandType.Text, PopulateJob);

            }
           
            return result;

        }


        public IEnumerable<JscJob> GetJobs(IConnectionManager entry, bool includeDisabled)
        {
            IEnumerable<JscJob> result;

            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseGetString;  

                      
                if (!includeDisabled)
                {
                    cmd.CommandText += " Where a.Enabled = 1";
                }

                result = Execute<JscJob>(entry, cmd, CommandType.Text, PopulateJob);

            }
            if (result != null)
            {
                foreach (var jscJob in result)
                {
                    using (var cmd = entry.Connection.CreateCommand())
                    {

                        cmd.CommandText =
                            JobTriggerGetString +
                            @" 
                            where a.Job = @jobId";

                        MakeParam(cmd, "jobId", (Guid)jscJob.ID);

                        var JobTriggers = Execute<JscJobTrigger>(entry, cmd, CommandType.Text, PopulateJobTrigger);

                        jscJob.JscJobTriggers = JobTriggers;
                    }
                    if (jscJob.Source != null && !jscJob.Source.IsEmpty)
                    {
                        jscJob.JscSourceLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(entry, jscJob.Source);
                    }
                    if (jscJob.Destination != null && !jscJob.Destination.IsEmpty)
                    {
                        jscJob.JscDestinationLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(entry, jscJob.Destination);

                    }
                }
            }
            return result;

        }

        public IEnumerable<JscJob> GetJobsPopulated(IConnectionManager entry, bool includeDisabled)
        {
            IEnumerable<JscJob> result;

            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseGetString;


                if (!includeDisabled)
                {
                    cmd.CommandText += " Where a.Enabled = 1";
                }

                result = Execute<JscJob>(entry, cmd, CommandType.Text, PopulateJob);
            }
            if (result != null )
            {
                foreach (var jscJob in result)
                {
                    using (var cmd = entry.Connection.CreateCommand())
                    {

                        cmd.CommandText =
                            JobTriggerGetString +
                            @" 
                            where a.Job = @jobId";

                        MakeParam(cmd, "jobId", (Guid)jscJob.ID);

                        var JobTriggers = Execute<JscJobTrigger>(entry, cmd, CommandType.Text, PopulateJobTrigger);

                        jscJob.JscJobTriggers = JobTriggers;
                    }
                    jscJob.JscJobSubjobs = GetJobSubJobs(entry, jscJob);
                    if (jscJob.Source != null && !jscJob.Source.IsEmpty)
                    {
                        jscJob.JscSourceLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(entry, jscJob.Source);
                    }
                    if (jscJob.Destination != null && !jscJob.Destination.IsEmpty)
                    {
                        jscJob.JscDestinationLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(entry, jscJob.Destination);

                    }
                }
            }
            return result;

        }

        public DataSelector[] GetJobSelectorList(IConnectionManager entry)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);

            var jobs =
                from job in GetJobs(entry, true)
                where job.Enabled
                orderby job.Text
                select new
                    {
                        job.ID,
                        Text = job.Text
                    };

            return jobs.Select(job =>
                               new DataSelector {GuidId = job.ID, Text = job.Text}).ToArray();
        }

        public JscSubJob GetSubJob(IConnectionManager entry, RecordIdentifier id)
        {

            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            JscSubJob result;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = SubJobGetString + @"                         
                                  where jsj.Id = @subJobID";

                MakeParam(cmd, "subJobID", (Guid) id);

                var records = Execute<JscSubJob>(entry, cmd, CommandType.Text, PopulateISubJob);
                if (records.Count > 0)
                {
                    if (records[0].TableFrom != null)
                    {
                        records[0].JscTableFrom = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesign(entry, records[0].TableFrom);
                    }
                    if (records[0].ActionTable != null)
                    {
                        records[0].JscActionTable = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesign(entry, records[0].ActionTable);
                    }
                    records[0].JscSubJobFromTableFilters = GetSubJobFromTableFiltersList(entry, records[0]);
                }
                result = (records.Count > 0) ? records[0] : null;
            }


            return result;
        }

        public IEnumerable<JscSubJob> GetSubJobs(IConnectionManager entry)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = SubJobGetString;

                return Execute<JscSubJob>(entry, cmd, CommandType.Text, PopulateISubJob);
            }
        }

        public IEnumerable<SubJobListItem> GetSubJobListItems(IConnectionManager entry, bool includeDisabled)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @" 
                    SELECT  
                        jsj.Id,
                        Description,
                        TableFrom,
                        StoredProcName,
                        TableNameTo,
                        ReplicationMethod,
                        WhatToDo,
                        jsj.Enabled,
                        IncludeFlowFields,
                        ActionTable,
                        ActionCounterInterval,
                        MoveActions,
                        AlwaysExecute,
                        NoDistributionFilter,
                        RepCounterField,
                        RepCounterInterval,
                        UpdateRepCounter,
                        UpdateRepCounterOnEmptyInt,
                        MarkSentRecordsField,
                        jtd.Id,
                        DatabaseDesign,
                        TableName,
                        jtd.Enabled,
                        jsj.FilterCodeFilter
                      FROM JscSubJobs jsj
                      LEFT JOIN JscTableDesigns jtd ON jsj.TableFrom = jtd.Id ";

                if (!includeDisabled)
                {
                    cmd.CommandText += " WHERE jsj.Enabled = 1";
                }

                return Execute<SubJobListItem>(entry, cmd, CommandType.Text, PopulateSubJobListItem);
            }

        }

        public List<JscSubJobFromTableFilter> GetSubJobFromTableFiltersList(IConnectionManager entry, JscSubJob subjob)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);


            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT 
	                                Id,
	                                SubJob,
	                                Field,
	                                FilterType,
	                                Value1,
	                                Value2,
	                                ApplyFilter
                                  FROM JscSubJobFromTableFilters
                                    where SubJob = @subJobID";

                MakeParam(cmd, "subJobID", (Guid) subjob.ID);

                var records = Execute<JscSubJobFromTableFilter>(entry, cmd, CommandType.Text,
                                                                PopulateSubJobFromTableFilter);

                foreach (var jscSubJobFromTableFilter in records)
                {
                    if (jscSubJobFromTableFilter.SubJob != null && !jscSubJobFromTableFilter.SubJob.IsEmpty)
                    {
                        jscSubJobFromTableFilter.JscSubJob = subjob;
                    }
                    if (jscSubJobFromTableFilter.Field != null && !jscSubJobFromTableFilter.Field.IsEmpty)
                    {
                        jscSubJobFromTableFilter.JscField = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesign(entry,
                                                                                      jscSubJobFromTableFilter.Field);
                    }
                }

                return records;
            }
        }

        public DataSelector[] GetSubJobSelectorList(IConnectionManager entry)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT  
                                    jsj.Id,
                                    Description
                                  FROM JscSubJobs jsj
                                  join JscTableDesigns jtd on jsj.TableFrom = jtd.Id        
                                   WHERE jsj.Enabled = 1";

                return Execute<DataSelector>(entry, cmd, CommandType.Text, PopulateDataSelector).ToArray();
            }
        }

        public JscJobSubjob GetJobSubJob(IConnectionManager entry, RecordIdentifier jobSubjobId)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            JscJobSubjob result;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = JobSubJobGetString +
                    @" 
                    where a.id = @jobSubjobId";

                MakeParam(cmd, "jobSubjobId", (Guid) jobSubjobId);

                var records = Execute<JscJobSubjob>(entry, cmd, CommandType.Text, PopulateJobSubJob);

                result = (records.Count > 0) ? records[0] : null;
            }

            return result;
        }

        public List<JscSubJob> GetJobSubjobValues(IConnectionManager entry, RecordIdentifier jobID)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            List<JscSubJob> records = new List<JscSubJob>();

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    SubJobGetString +
                    @" 
                    where jsj.id in 
                    (
	                    select jsub.SubJob
	                    from JscJobSubjobs jsub
	                    where jsub.job = @jobId
                    )";

                MakeParam(cmd, "jobId", (Guid)jobID);

                records = Execute<JscSubJob>(entry, cmd, CommandType.Text, PopulateISubJob);
                
                return records;
            }
        }

        public int GetMaxReplicationCounterValue(IConnectionManager entry, string tableName, string replicationCounterColumnName)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT MAX({replicationCounterColumnName}) FROM {tableName}";

                object result = entry.Connection.ExecuteScalar(cmd);

                return result == null || result is DBNull ? 0 : (int)result;
            }
        }

        public List<JscJobSubjob> GetJobSubJobs(IConnectionManager entry, JscJob job)
        {

            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    JobSubJobGetString +
                    @" 
                    where a.Job = @jobId";

                MakeParam(cmd, "jobId", (Guid) job.ID);

                var records = Execute<JscJobSubjob>(entry, cmd, CommandType.Text, PopulateJobSubJob);
                if (records != null)
                {
                    foreach (var jscJobSubjob in records)
                    {
                        if (jscJobSubjob.Job != null)
                        {
                            jscJobSubjob.JscJob = job;
                        }
                       
                    }
                }

                return records;
            }

        }

        /// <summary>
        /// Returns the latest Sequence used in any JobSubJob for the supplied job
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public int GetMaxJobSubjobSequence(IConnectionManager entry, RecordIdentifier jobID)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @" SELECT 
                                        max( Sequence)
                                    FROM JscJobSubjobs where Job = @jobID ";

                MakeParam(cmd, "jobID", (Guid) jobID, SqlDbType.UniqueIdentifier);

                return entry.Connection.ExecuteScalar(cmd) == DBNull.Value
                           ? 0
                           : (int) entry.Connection.ExecuteScalar(cmd);
            }
        }

        public IEnumerable<JscSubJob> GetSubJobCandidates(IConnectionManager entry, RecordIdentifier fromTableDatabaseDesignId, RecordIdentifier jobId)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = SubJobGetString +
                                  @"
                                    left join JscTableDesigns jtd on jsj.TableFrom = jtd.Id
                                Where 
                                    jsj.ID not in (select jsj.Id 
                                            FROM JscSubJobs jsj
                                            join JscJobSubjobs jsjs on jsj.Id = jsjs.SubJob 
                                            where jsjs.job = @jobID) 
                                    AND ";

                if (fromTableDatabaseDesignId != null)
                {
                    cmd.CommandText +=
                        "(jsj.ReplicationMethod = 2 OR jtd.DatabaseDesign = @fromDatabaseDesign)";

                    MakeParam(cmd, "fromDatabaseDesign", (Guid) fromTableDatabaseDesignId);

                }
                else
                {
                    cmd.CommandText += "jsj.ReplicationMethod = 2";
                }

                MakeParam(cmd, "jobID", (Guid) jobId);

                var records = Execute<JscSubJob>(entry, cmd, CommandType.Text, PopulateISubJob);
                return records;
            }
        }

        public IEnumerable<JscSchedulerLog> GetJobLog(IConnectionManager entry, DateTime fromDate, DateTime toDate, RecordIdentifier jobId)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            var effectiveJobId = jobId ?? Guid.Empty;
            toDate = toDate.AddDays(1);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT Id,
                                        Job,
                                        RegTime,
                                        Message
                                FROM JscSchedulerLog SL
                                WHERE 
                                    RegTime >= @fromDate
                                    AND 
                                    RegTime < @toDate ";
                if (effectiveJobId != Guid.Empty)
                {
                    cmd.CommandText += " AND job = @effectiveJobId ";
                    MakeParam(cmd, "effectiveJobId", (Guid) effectiveJobId);
                }
                cmd.CommandText += "Order By RegTime desc";

                MakeParam(cmd, "fromDate", fromDate, SqlDbType.DateTime);
                MakeParam(cmd, "toDate", toDate, SqlDbType.DateTime);


                return Execute<JscSchedulerLog>(entry, cmd, CommandType.Text, PopulateSchedulerLog);

            }
        }

        public IEnumerable<JscSchedulerSubLog> GetSubJobLog(IConnectionManager entry, RecordIdentifier  jobLogId)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT
                                        sl.Id,
                                        sl.SchedulerLogId,
                                        sl.SubJob,
                                        sl.ReplicationCounterStart,
                                        sl.ReplicationCounterEnd,
                                        sl.RunAsNormal,
                                        sl.Location,
                                        sl.StartTime,
                                        sl.EndTime
                                        FROM 
                                            JscSchedulerSubLog sl
											join [dbo].[JscSchedulerLog] l on l.Id = sl.SchedulerLogId
											left join [dbo].[JscJobSubjobs] jsj on jsj.Job = l.Job and sl.SubJob = jsj.SubJob
                                        WHERE 
                                            SchedulerLogId = @logID
										ORDER BY ISNULL(jsj.Sequence, 1000000)";
                MakeParam(cmd, "logID", (Guid)jobLogId);


                return Execute<JscSchedulerSubLog>(entry, cmd, CommandType.Text, PopulateSchedulerSubLog);

            }
        }
        
        public IEnumerable<JscRepCounter> GetReplicationCounters(IConnectionManager entry, RecordIdentifier subJobId)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT rc.Id,
		                                rc.Job,
		                                rc.SubJob,
		                                rc.Location,
		                                rc.Counter
                                  FROM JscRepCounters rc 
                                    WHERE 
                                rc.Subjob = @subJobID ";

                MakeParam(cmd, "subJobID", (Guid) subJobId);

                var records =  Execute<JscRepCounter>(entry, cmd, CommandType.Text, PopulateRepCounters);
                foreach (var jscRepCounter in records)
                {
                    if (jscRepCounter.Job != null && !jscRepCounter.Job.IsEmpty)
                    {
                        jscRepCounter.JscJob = GetJob(entry, jscRepCounter.Job);
                    }
                    if (jscRepCounter.SubJob != null && !jscRepCounter.SubJob.IsEmpty)
                    {
                        jscRepCounter.JscSubJob = GetSubJob(entry, jscRepCounter.SubJob);
                    }
                    if (jscRepCounter.Location != null && !jscRepCounter.Location.IsEmpty)
                    {
                        jscRepCounter.JscLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(entry,jscRepCounter.Location);
                    }
                }

                return records;
            }
        }

        public IEnumerable<ReplicationCounterListItem> GetReplicationCounterListItems(IConnectionManager entry,ReplicationCountersFilter repCounterFilter)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT rc.Id,
		                                rc.Job,
		                                rc.SubJob,
		                                rc.Location,
		                                rc.Counter, 
		                                j.Description JobText, 
		                                sj.Description SubJobText, 
		                                L.Name LocationText
                                  FROM JscRepCounters rc 
                                  join jscJobs j on rc.Job = j.id
                                  join JscSubJobs sj on rc.SubJob = sj.Id
                                  join jsclocations l on rc.Location = l.id";
                bool first = true;

                if (repCounterFilter.JobId.HasValue)
                {

                    cmd.CommandText += " WHERE ";

                    cmd.CommandText += " rc.Job = @jobID ";
                    MakeParam(cmd, "jobID", (Guid) repCounterFilter.JobId, SqlDbType.UniqueIdentifier);
                    first = false;
                }
                if (repCounterFilter.SubJobId.HasValue)
                {
                    if (first)
                    {
                        cmd.CommandText += " WHERE ";
                    }
                    else
                    {
                        cmd.CommandText += " AND ";
                    }
                    cmd.CommandText += " rc.Subjob = @subJobID ";
                    MakeParam(cmd, "subJobID", (Guid) repCounterFilter.SubJobId, SqlDbType.UniqueIdentifier);
                    first = false;
                }
                if (repCounterFilter.LocationId.HasValue)
                {
                    if (first)
                    {
                        cmd.CommandText += " WHERE ";
                    }
                    else
                    {
                        cmd.CommandText += " AND ";
                    }
                    cmd.CommandText += " rc.Location  = @locationId ";
                    MakeParam(cmd, "locationId", (Guid) repCounterFilter.LocationId, SqlDbType.UniqueIdentifier);
                }

                return Execute<ReplicationCounterListItem>(entry, cmd, CommandType.Text, PopulateReplicationListItem);
            }
        }

        private static IEnumerable<JscJobSubjob> GetEffectiveSubjobs(IConnectionManager entry,JscJob job)
        {
            JscJob effectiveJob;
            if (job.JscSubjobJob == null && job.SubjobJob.IsEmpty )
            {
                effectiveJob = job;
            }
            else
            {
                if (job.JscSubjobJob == null)
                {
                    job.JscSubjobJob = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(entry, job.SubjobJob);
                }
                effectiveJob = job.JscSubjobJob;
            }

            return effectiveJob.JscJobSubjobs;
        }

        private static void GetSingleSourceDatabaseDesign(IConnectionManager entry, out Guid databaseDesignId, out JobValidationResult validationResult, JscJob job)
        {
            databaseDesignId = Guid.Empty;
            Guid[] databaseDesigns;

            validationResult = GetSourceDatabaseDesigns(out databaseDesigns, entry, job);
            if (validationResult != null)
            {
                return;
            }

            // Rule: source locations must all have the same design
            if (databaseDesigns.Length > 1)
            {
                validationResult = new JobValidationResult
                    {
                        Message = JobValidationMessage.SourceLocationsHaveMultipleDBDesigns
                    };
                return;
            }
            if (databaseDesigns.Length == 0|| databaseDesigns[0]  == Guid.Empty)
            {
                validationResult = new JobValidationResult
                    {
                        Message = JobValidationMessage.SourceLocationHasNoDBDesign,
                        Param0 = job.JscSourceLocation.Text
                    };
                return;
            }
            databaseDesignId = databaseDesigns[0];
        }

        private static IEnumerable<JscLocation> GetLocationAndMemberTree(JscLocation location, IConnectionManager entry)
        {
            List<JscLocation> locations = new List<JscLocation>();
            locations.Add(location);
            locations.AddRange(DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetMemberTree(entry, location));

            return locations;
        }

        private static JobValidationResult GetSourceDatabaseDesigns(out Guid[] databaseDesigns,IConnectionManager entry, JscJob job)
        {
            databaseDesigns = null;
            HashSet<Guid> databaseIds = new HashSet<Guid>();

            foreach (var location in GetLocationAndMemberTree(job.JscSourceLocation, entry))
            {
                if (!location.DBServerIsUsed)
                    continue;

                if (location.DatabaseDesign != null)
                {
                    databaseIds.Add((Guid) location.DatabaseDesign);
                }
                else
                {
                    // Rule: All source locations must have a database design
                    return new JobValidationResult
                        {
                            Message = JobValidationMessage.SourceLocationHasNoDBDesign,
                            Param0 = location.Text
                        };
                }
            }


            databaseDesigns = databaseIds.ToArray();

            return null;
        }

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="jobID">The unique ID of the job to check on</param>
        public bool Exists(IConnectionManager entry, RecordIdentifier jobID)
        {
            return RecordExists(entry, "JscJobs", "Id", jobID);
        }

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="subJobID">The unique ID of the subjob to check on</param>
        public bool SubJobExists(IConnectionManager entry, RecordIdentifier subJobID)
        {
            return RecordExists(entry, "JscSubJobs", "Id", subJobID);
        }

        /// <summary>
        /// Returns true if the job has any subjobs.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public bool IsJobUsedForSubjobs(IConnectionManager entry, RecordIdentifier jobId)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            return GetJobs(entry, true).Any(j => j.SubjobJob == jobId);
        }

        /// <summary>
        /// Returns true if any JobSubJobs use the supplied Subjob
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="subJobID"></param>
        /// <returns></returns>
        public bool IsSubjobUsedInJobs(IConnectionManager entry, RecordIdentifier subJobID)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseJobSubJobGetString +

                    @" 
                        join jscJobs j on a.Job = j.id
                    where a.SubJob = @subJobID";

                MakeParam(cmd, "subJobID", (Guid) subJobID);

                var records = Execute<JscJobSubjob>(entry, cmd, CommandType.Text, PopulateJobSubJobNoObject);

                return records.Count > 0;
            }
        }

        /// <summary>
        /// Runs a series of validations for the job Returning OK if no errors, a result indicating 
        /// the source of the failure otherwise.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        public JobValidationResult ValidateForRun(IConnectionManager entry, JscJob job)
        {
            if (job.JobType == JobType.ExternalPlugin)
            {
                return new JobValidationResult { Message = JobValidationMessage.OK };
            }
            if (string.IsNullOrEmpty(job.Text))
            {
                return new JobValidationResult { Message = JobValidationMessage.DescriptionMissing };
            }
            if (!job.Enabled)
            {
                return new JobValidationResult { Message = JobValidationMessage.JobIsNotEnabled };
            }
            if (job.Source == null || job.Source.IsEmpty)
            {
                return new JobValidationResult { Message = JobValidationMessage.SourceLocationMissing };
            }
            if (job.Destination == null || job.Destination.IsEmpty)
            {
                return new JobValidationResult { Message = JobValidationMessage.DestinationLocationMissing };
            }
            if (job.UseCurrentLocation)
            {
                return new JobValidationResult { Message = JobValidationMessage.OK };
            }
            if ((job.JscSourceLocation == null || !job.JscSourceLocation.Enabled))
            {
                return new JobValidationResult { Message = JobValidationMessage.SourceLocationIsNotEnabled };
            }
            if (job.JscSourceLocation.MemberLocations == null)
            {
                job.JscSourceLocation.MemberLocations = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetAllMemberships(entry, job.JscSourceLocation.ID, true);
            }
            if (job.JscSourceLocation.IsGroup)
            {
                foreach (JscLocationMember locm in job.JscSourceLocation.MemberLocations)
                {
                    if ((locm.Member == null) || (locm.Member.Enabled && string.IsNullOrWhiteSpace(locm.Member.DDHost)))
                    {
                        return new JobValidationResult { Message = JobValidationMessage.SourceLocationHasNoDataDirector };
                    }

                    JobValidationResult res = ValidateDatabaseDesigns(entry, job);
                    if (res != null)
                        return res;
                }
                return new JobValidationResult { Message = JobValidationMessage.OK };
            }
            if (string.IsNullOrWhiteSpace(job.JscSourceLocation.DDHost))
            {
                return new JobValidationResult { Message = JobValidationMessage.SourceLocationHasNoDataDirector };
            }

            JobValidationResult result = ValidateDatabaseDesigns(entry, job);
            if (job.JscJobSubjobs != null)
            {
                foreach (var subjob in job.JscJobSubjobs )
                {
                    if ((subjob.JscSubJob.ReplicationMethod == ReplicationTypes.Action|| subjob.JscSubJob.ReplicationMethod == ReplicationTypes.Procedure )
                        &&
                        subjob.JscSubJob.ActionTable == null)
                    {
                        result =
                            new JobValidationResult
                            {
                                Message = JobValidationMessage.MissingSubJobActionTable
                            };
                    }
                }
            }
            if (result == null)
            {
                result = new JobValidationResult { Message = JobValidationMessage.OK };
            }
            
            return result;
        }

        /// <summary>
        /// Runs a series of test validating design aspects of the Job. Returns null if no errors.
        /// A message indicating failure source otherwise.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        private static JobValidationResult ValidateDatabaseDesigns(IConnectionManager entry, JscJob job)
        {
            JobValidationResult result;

            // Make sure all source locations have a single database ID
            Guid sourceDatabaseDesign;
            GetSingleSourceDatabaseDesign(entry, out sourceDatabaseDesign, out result, job);
            if (result != null)
            {
                return result;
            }

            // Make sure that all subjobs refer to the same database as the source location
            result = VerifyUsedDatabaseDesigns(entry,sourceDatabaseDesign, job);
            if (result != null)
            {
                return result;
            }

            return VerifyDestinationDatabaseDesigns(entry, sourceDatabaseDesign, job);
        }

        /// <summary>
        /// Returns null if all is clear otherwise returns a validation message
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="sourceDatabaseDesign"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        private static JobValidationResult VerifyUsedDatabaseDesigns(IConnectionManager entry, Guid sourceDatabaseDesign, JscJob job)
        {
            var subjobs = GetEffectiveSubjobs(entry,job);

            bool isEmpty = true;

            foreach (var jobSubjob in subjobs)
            {
                if (!jobSubjob.Enabled)
                    continue;

                isEmpty = false;
                var subJob = jobSubjob.JscSubJob;

                bool match =
                    (subJob.JscTableFrom == null || subJob.JscTableFrom.DatabaseDesign == sourceDatabaseDesign) &&
                    (subJob.JscActionTable == null || subJob.JscActionTable.DatabaseDesign == sourceDatabaseDesign) &&
                    (subJob.JscRepCounterField == null ||
                     subJob.JscRepCounterField.JscTableDesign.DatabaseDesign == sourceDatabaseDesign) &&
                    (subJob.JscMarkSentRecordsField == null ||
                     subJob.JscMarkSentRecordsField.JscTableDesign.DatabaseDesign == sourceDatabaseDesign);

                if (!match)
                {
                    return new JobValidationResult {Message = JobValidationMessage.SubjobDBMismatch};
                }
            }

            if (isEmpty)
            {
                return new JobValidationResult {Message = JobValidationMessage.NoSubjobs};
            }

            return null;
        }

        /// <summary>
        /// Make sure that all destination locations either have the same database design as
        /// the source locations or if not, that there exists a database map between them.
        /// </summary>
        private static JobValidationResult VerifyDestinationDatabaseDesigns(IConnectionManager entry, Guid sourceDatabaseDesign, JscJob job)
        {
            
            foreach (
                var destinationLocation in GetLocationAndMemberTree(job.JscDestinationLocation, entry))
            {
                if (!destinationLocation.DBServerIsUsed)
                    continue;

                if ((destinationLocation.DatabaseDesign != null && !destinationLocation.DatabaseDesign.IsEmpty))
                    continue;

                RecordIdentifier destinationDatabaseDesign = destinationLocation.DatabaseDesign;
                if (destinationDatabaseDesign == sourceDatabaseDesign)
                    continue;

                TableMapCache tableMapCache = new TableMapCache( entry, sourceDatabaseDesign,
                                                                (Guid) destinationDatabaseDesign);

                // For each subjob in the job, make sure that the referenced table and/or field has a map
                foreach (var jobSubJob in GetEffectiveSubjobs(entry,job))
                {
                    var subJob = jobSubJob.JscSubJob;
                    if (subJob.TableFrom != null && !tableMapCache.Exists(subJob.TableFrom))
                    {
                        return new JobValidationResult
                            {
                                Message = JobValidationMessage.DestinationLocationTableMapMissing,
                                Param0 = destinationLocation.Text,
                                Param1 = subJob.Description
                            };
                    }
                }
            }

            return null;
        }

        public IEnumerable<JscSubJob> SearchSubJobsForJob(IConnectionManager entry, string searchString,
                                               int rowFrom, int rowTo, bool beginsWith, RecordIdentifier fromTableDatabaseDesignId, RecordIdentifier jobID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";

                cmd.CommandText =
                    @"SELECT * FROM
                            (SELECT
                            jsj.Id,
                            jsj.Description,
                            jsj.TableFrom,
                            jsj.StoredProcName,
                            jsj.TableNameTo,
                            jsj.ReplicationMethod,
                            jsj.WhatToDo,
                            jsj.Enabled,
                            jsj.IncludeFlowFields,
                            jsj.ActionTable,
                            jsj.ActionCounterInterval,
                            jsj.MoveActions,
                            jsj.AlwaysExecute,
                            jsj.NoDistributionFilter,
                            jsj.RepCounterField,
                            jsj.RepCounterInterval,
                            jsj.UpdateRepCounter,
                            jsj.UpdateRepCounterOnEmptyInt,
                            jsj.MarkSentRecordsField,
                            jsj.FilterCodeFilter,
                        ROW_NUMBER() OVER(ORDER BY jsj.Description) AS ROW
                        FROM JscSubJobs jsj
                        LEFT JOIN JscTableDesigns jtd on jsj.TableFrom = jtd.Id
                                WHERE
                                    jsj.ID not in (select jsj.Id 
                                            FROM JscSubJobs jsj
                                            join JscJobSubjobs jsjs on jsj.Id = jsjs.SubJob 
                                            where jsjs.job = @jobID) 
                                    AND (jsj.Description Like @SEARCHSTRING) 
                                    AND ";


                
                if (fromTableDatabaseDesignId != null)
                {
                    cmd.CommandText +=
                        "(jsj.ReplicationMethod = 2 OR jtd.DatabaseDesign = @fromDatabaseDesign)";

                    MakeParam(cmd, "fromDatabaseDesign", (Guid)fromTableDatabaseDesignId);

                }
                else
                {
                    cmd.CommandText += "jsj.ReplicationMethod = 2";
                }

                cmd.CommandText += @") S
                                    WHERE S.ROW BETWEEN " + rowFrom + " AND " + rowTo;

                MakeParam(cmd, "SEARCHSTRING", modifiedSearchString);
                MakeParam(cmd, "jobID", (Guid)jobID);
                return Execute<JscSubJob>(entry, cmd, CommandType.Text, PopulateISubJob);
            }
        }
    }
}