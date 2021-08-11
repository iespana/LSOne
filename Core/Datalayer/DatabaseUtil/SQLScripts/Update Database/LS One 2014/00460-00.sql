
/*

	Incident No.	: None has to do with scheduler plugin refactor
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: 2014
	Date created	: 18.11.2013

	Description		: DATAAREAID must be added to most tables using scheduler
	
						
*/
USE LSPOSNET

IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscJobs')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobs' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscJobs] 
	drop column DATAAREAID; 
END

end
Else 
Begin
CREATE TABLE [dbo].[JscJobs]
(
	[Id] UniqueIdentifier NOT NULL,
	[Description] NVarChar(60) NOT NULL,
	[JobType] SmallInt NOT NULL,
	[Source] UniqueIdentifier,
	[Destination] UniqueIdentifier,
	[ErrorHandling] SmallInt NOT NULL,
	[Enabled] Bit NOT NULL,
	[SubjobJob] UniqueIdentifier
);

exec spDB_SetTableDescription_1_0 'JscJobs','Scheduler jobs. A job is a unit of data replication. A job is created and configured in the Scheduler user interface. When it is run the Scheduler back-end fetches all data needed by the job from the job''s source location(s). The job and the data is then submitted to the Data Director for routing and updating on the job''s destination locations.';
exec spDB_SetFieldDescription_1_0 'JscJobs', 'Description', 'A short description.';
exec spDB_SetFieldDescription_1_0 'JscJobs', 'JobType', 'The type of this job.';
exec spDB_SetFieldDescription_1_0 'JscJobs', 'Source', 'The location from which the data for this job should be taken.';
exec spDB_SetFieldDescription_1_0 'JscJobs', 'Destination', 'The location that should receive the data.';
exec spDB_SetFieldDescription_1_0 'JscJobs', 'ErrorHandling', 'Currently not used.';
exec spDB_SetFieldDescription_1_0 'JscJobs', 'Enabled', 'Enabled for common use.';
exec spDB_SetFieldDescription_1_0 'JscJobs', 'SubjobJob', 'If set, this job will only use sub-jobs from SubjobJob.';

ALTER TABLE [dbo].[JscJobs] ADD CONSTRAINT [JscJobs_PK] PRIMARY KEY CLUSTERED
([Id]);

End
GO
IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscJobTriggers')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobTriggers' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscJobTriggers] 
	drop column DATAAREAID; 
END
end
else 
begin 
CREATE TABLE [dbo].[JscJobTriggers]
(
	[Id] UniqueIdentifier NOT NULL,
	[Job] UniqueIdentifier NOT NULL,
	[TriggerKind] SmallInt NOT NULL,
	[Enabled] Bit NOT NULL,
	[RecurrenceType] SmallInt NOT NULL,
	[Seconds] NVarChar(170),
	[Minutes] NVarChar(170),
	[Hours] NVarChar(170),
	[Months] NVarChar(170),
	[Years] NVarChar(170),
	[DaysOfMonth] NVarChar(170),
	[DaysOfWeek] NVarChar(170),
	[StartTime] DateTime,
	[EndTime] DateTime,
	[Tag] NVarChar(170)
);

exec spDB_SetTableDescription_1_0 'JscJobTriggers','Triggers of jobs. A trigger is a way to automatically start a job.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'TriggerKind', 'What kind of trigger this is. Currently only a value of 1 (timed schedule) is supported.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'Enabled', 'Enabled for common use.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'RecurrenceType', 'TimedSchedule triggers: The kind of recurrance used by this trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'Seconds', 'A cron string for the seconds part of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'Minutes', 'A cron string for the minutes part of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'Hours', 'A cron string for the hours part of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'Months', 'A cron string for the months part of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'Years', 'A cron string for the years part of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'DaysOfMonth', 'A cron string for the days-of-month part of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'DaysOfWeek', 'A cron string for the days-of-week part of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'StartTime', 'The start time of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'EndTime', 'The end time of the trigger.';
exec spDB_SetFieldDescription_1_0 'JscJobTriggers', 'Tag', 'An arbitrary string that can be used to associate the trigger with some external object.';

ALTER TABLE [dbo].[JscJobTriggers] ADD CONSTRAINT [JscJobTriggers_PK] PRIMARY KEY CLUSTERED
([Id]);

end
GO
IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscJobSubjobs')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobSubjobs' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscJobSubjobs] 
	drop column DATAAREAID; 
END
end
else
begin
CREATE TABLE [dbo].[JscJobSubjobs]
(
	[Id] UniqueIdentifier NOT NULL,
	[Job] UniqueIdentifier NOT NULL,
	[SubJob] UniqueIdentifier NOT NULL,
	[Sequence] Int NOT NULL,
	[Enabled] Bit NOT NULL
);

exec spDB_SetTableDescription_1_0 'JscJobSubjobs','An implement of the many-to-many relationship between tables JscJobs and JscSubJobs.';
exec spDB_SetFieldDescription_1_0 'JscJobSubjobs', 'Sequence', 'The sequence of this sub-job when running the job.';
exec spDB_SetFieldDescription_1_0 'JscJobSubjobs', 'Enabled', 'Enabled for common use.';

ALTER TABLE [dbo].[JscJobSubjobs] ADD CONSTRAINT [JscJobSubjobs_PK] PRIMARY KEY CLUSTERED
([Id]); 
end 
GO
IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscSubJobs')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscSubJobs' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscSubJobs] 
	drop column DATAAREAID; 
END
end
else 
begin 
CREATE TABLE [dbo].[JscSubJobs]
(
	[Id] UniqueIdentifier NOT NULL,
	[Description] NVarChar(60) NOT NULL,
	[TableFrom] UniqueIdentifier,
	[StoredProcName] NVarChar(250),
	[TableNameTo] NVarChar(250),
	[ReplicationMethod] SmallInt NOT NULL,
	[WhatToDo] SmallInt NOT NULL,
	[Enabled] Bit NOT NULL,
	[IncludeFlowFields] Bit NOT NULL,
	[ActionTable] UniqueIdentifier,
	[ActionCounterInterval] Int,
	[MoveActions] Bit NOT NULL,
	[NoDistributionFilter] Bit NOT NULL,
	[RepCounterField] UniqueIdentifier,
	[RepCounterInterval] Int,
	[UpdateRepCounter] Bit NOT NULL,
	[UpdateRepCounterOnEmptyInt] Bit NOT NULL,
	[MarkSentRecordsField] UniqueIdentifier
);

exec spDB_SetTableDescription_1_0 'JscSubJobs','A sub-job specifies a single type of data, most commonly a data table, and how the data in that table should be replicated.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'Description', 'A short description.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'TableFrom', 'The design of the table from where data should be fetched. Empty if StoredProcName is used.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'StoredProcName', 'The name of the stored procedure that should be run to fetch data. Empty if TableName is used.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'TableNameTo', 'The name of the table where to store the data.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'ReplicationMethod', 'Indicates how data should be fetched.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'WhatToDo', 'Modifier definition, tells what to do with the data.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'Enabled', 'Enabled for common use.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'IncludeFlowFields', 'Microsoft NAV specific: Indicates whether to include flow fields when replicating.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'ActionTable', 'The design of the table to use when ReplicationMethod is set to Action.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'ActionCounterInterval', 'Currently not used.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'MoveActions', 'For ReplicationMethod = Action: Move actions as well as data.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'NoDistributionFilter', 'Currently not used.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'RepCounterField', 'The design of the field to use when using replication counters and the ReplicationMethod is set to Normal.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'RepCounterInterval', 'Currently not used.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'UpdateRepCounter', 'Currently not used.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'UpdateRepCounterOnEmptyInt', 'Currently not used.';
exec spDB_SetFieldDescription_1_0 'JscSubJobs', 'MarkSentRecordsField', 'Currently not used.';

ALTER TABLE [dbo].[JscSubJobs] ADD CONSTRAINT [JscSubJobs_PK] PRIMARY KEY CLUSTERED
([Id]);

end 
GO
 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscLocations')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscLocations' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscLocations] 
	drop column DATAAREAID; 
END
end
else 
begin 

CREATE TABLE [dbo].[JscLocations]
(
	[Id] UniqueIdentifier NOT NULL,
	[Name] NVarChar(60) NOT NULL,
	[EXDATAAREAID] NVarChar(4),
	[ExCode] NVarChar(20),
	[DatabaseDesign] UniqueIdentifier,
	[LocationKind] SmallInt NOT NULL,
	[DefaultOwner] UniqueIdentifier,
	[DDHost] NVarChar(50),
	[DDPort] NVarChar(20),
	[DDNetMode] SmallInt NOT NULL,
	[Enabled] Bit NOT NULL,
	[Company] NVarChar(30),
	[UserId] NVarChar(30),
	[Password] NVarChar(30),
	[DBServerIsUsed] Bit NOT NULL,
	[DBServerHost] NVarChar(50),
	[DBPathName] NVarChar(200),
	[DBDriverType] UniqueIdentifier,
	[DBConnectionString] NVarChar(250),
	[SystemTag] NVarChar(50)
);

exec spDB_SetTableDescription_1_0 'JscLocations','Distribution locations are sources and destinations for data transfers.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'Name', 'The name and/or short description.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'EXDATAAREAID', 'Data area ID of external object, if any';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'ExCode', 'Code of external object, if any';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DatabaseDesign', 'The database design used at the location.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'LocationKind', 'The kind of location this represents.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DefaultOwner', 'Currently not used.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DDHost', 'Network host name or IP address of the machine running the Data Director for this location.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DDPort', 'IP port number of the Data Director in this location.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DDNetMode', 'The network mode to use when communicating with the Data Director.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'Enabled', 'Enabled for common use.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'Company', 'Part of connection string: Company name (Dynamics NAV).';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'UserId', 'Part of connection string: User ID.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'Password', 'Part of connection string: User password.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DBServerIsUsed', 'Set if this location has a database server, clear if not.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DBServerHost', 'Part of connection string: Network host name or IP address of the machine running the database server.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DBPathName', 'Part of connection string: Name or path of the database.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DBDriverType', 'Type of database and database driver at this location';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'DBConnectionString', 'The connection string to use when connecting to the database at this location. This connection string is passed to the Data Director.';
exec spDB_SetFieldDescription_1_0 'JscLocations', 'SystemTag', 'Tag used internally by the Scheduler for special locations, such as the ''All Stores'' location. Not available for editing in the UI.';

ALTER TABLE [dbo].[JscLocations] ADD CONSTRAINT [JscLocations_PK] PRIMARY KEY CLUSTERED
([Id]);

end 

GO

 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscTableMap')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscTableMap' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscTableMap ] 
	drop column DATAAREAID; 
END
end
else 
begin 

CREATE TABLE [dbo].[JscTableMap]
(
	[Id] UniqueIdentifier NOT NULL,
	[FromTable] UniqueIdentifier NOT NULL,
	[ToTable] UniqueIdentifier NOT NULL,
	[Description] NVarChar(60)
);

exec spDB_SetTableDescription_1_0 'JscTableMap','A table map exists to make it possible to replicate data from one table design into a table with another design. Usually these two tables are in two different locations and databases.';
exec spDB_SetFieldDescription_1_0 'JscTableMap', 'FromTable', 'The table design where data will be fetched from.';
exec spDB_SetFieldDescription_1_0 'JscTableMap', 'ToTable', 'The table design where data will be stored.';
exec spDB_SetFieldDescription_1_0 'JscTableMap', 'Description', 'A short description.';

ALTER TABLE [dbo].[JscTableMap] ADD CONSTRAINT [JscTableMap_PK] PRIMARY KEY CLUSTERED
([Id]);

end
GO

 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscFieldMap')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscFieldMap' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscFieldMap] 
	drop column DATAAREAID; 
END
end
else 
begin 
CREATE TABLE [dbo].[JscFieldMap]
(
	[Id] UniqueIdentifier NOT NULL,
	[TableMap] UniqueIdentifier NOT NULL,
	[FromField] UniqueIdentifier NOT NULL,
	[ToField] UniqueIdentifier NOT NULL,
	[ConversionMethod] SmallInt NOT NULL,
	[ConversionValue] NVarChar(250)
);

exec spDB_SetTableDescription_1_0 'JscFieldMap','A field map details how to map a field from one table to another, or how to populate the destination field with a constant value.';
exec spDB_SetFieldDescription_1_0 'JscFieldMap', 'FromField', 'Field to take data from.';
exec spDB_SetFieldDescription_1_0 'JscFieldMap', 'ToField', 'Field to put data to.';
exec spDB_SetFieldDescription_1_0 'JscFieldMap', 'ConversionMethod', 'How to convert the data.';
exec spDB_SetFieldDescription_1_0 'JscFieldMap', 'ConversionValue', 'Optional parameter or value for the conversion of the data.';

ALTER TABLE [dbo].[JscFieldMap] ADD CONSTRAINT [JscFieldMap_PK] PRIMARY KEY CLUSTERED
([Id]);

end
GO

 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscSchedulerLog')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscSchedulerLog' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscSchedulerLog] 
	drop column DATAAREAID; 
END
end
else
begin 
CREATE TABLE [dbo].[JscSchedulerLog]
(
	[Id] UniqueIdentifier NOT NULL,
	[Job] UniqueIdentifier NOT NULL,
	[RegTime] DateTime NOT NULL,
	[Message] NVarChar(MAX)
);

exec spDB_SetTableDescription_1_0 'JscSchedulerLog','A generic message logging table.';

ALTER TABLE [dbo].[JscSchedulerLog] ADD CONSTRAINT [JscSchedulerLog_PK] PRIMARY KEY CLUSTERED
([Id]);

CREATE INDEX [JscSchedulerLog_IX_RegTime] ON [dbo].[JscSchedulerLog]
([RegTime]);

CREATE INDEX [JscSchedulerLog_IX_Job_RegTime] ON [dbo].[JscSchedulerLog]
([Job], [RegTime]);
end
GO

 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscDatabaseDesigns')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscDatabaseDesigns' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscDatabaseDesigns] 
	drop column DATAAREAID; 
END
end
else 
begin 
CREATE TABLE [dbo].[JscDatabaseDesigns]
(
	[Id] UniqueIdentifier NOT NULL,
	[Description] NVarChar(60) NOT NULL,
	[CodePage] Int,
	[Enabled] Bit NOT NULL
);

exec spDB_SetTableDescription_1_0 'JscDatabaseDesigns','A database design and the associated tables stores metadata information required to fetch data from and store data in the database of a location.';
exec spDB_SetFieldDescription_1_0 'JscDatabaseDesigns', 'Description', 'A short description.';
exec spDB_SetFieldDescription_1_0 'JscDatabaseDesigns', 'CodePage', 'The code page used by the database using this database design.';
exec spDB_SetFieldDescription_1_0 'JscDatabaseDesigns', 'Enabled', 'Enabled for common use.';

ALTER TABLE [dbo].[JscDatabaseDesigns] ADD CONSTRAINT [JscDatabaseDesigns_PK] PRIMARY KEY CLUSTERED
([Id]);
end

GO

 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscTableDesigns')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscTableDesigns' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscTableDesigns] 
	drop column DATAAREAID; 
END
end
else 
begin 
CREATE TABLE [dbo].[JscTableDesigns]
(
	[Id] UniqueIdentifier NOT NULL,
	[DatabaseDesign] UniqueIdentifier NOT NULL,
	[TableName] NVarChar(250) NOT NULL,
	[Enabled] Bit NOT NULL
);

exec spDB_SetTableDescription_1_0 'JscTableDesigns','A table design.';
exec spDB_SetFieldDescription_1_0 'JscTableDesigns', 'DatabaseDesign', 'The database design to which this table design belongs.';
exec spDB_SetFieldDescription_1_0 'JscTableDesigns', 'TableName', 'The name of the database table.';
exec spDB_SetFieldDescription_1_0 'JscTableDesigns', 'Enabled', 'Enabled for common use.';

ALTER TABLE [dbo].[JscTableDesigns] ADD CONSTRAINT [JscTableDesigns_PK] PRIMARY KEY CLUSTERED
([Id]);

CREATE UNIQUE INDEX [JscTableDesigns_IX_DatabaseDesign_TableName] ON [dbo].[JscTableDesigns]
([DatabaseDesign], [TableName]);

end

GO

 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscFieldDesigns')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscFieldDesigns' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscFieldDesigns]
	drop column DATAAREAID; 
END
end
else 
begin 
CREATE TABLE [dbo].[JscFieldDesigns]
(
	[Id] UniqueIdentifier NOT NULL,
	[TableDesign] UniqueIdentifier NOT NULL,
	[FieldName] NVarChar(60) NOT NULL,
	[Sequence] Int NOT NULL,
	[DataType] SmallInt NOT NULL,
	[Length] Int,
	[FieldClass] SmallInt NOT NULL,
	[FieldOption] NVarChar(250),
	[Enabled] Bit NOT NULL
);

exec spDB_SetTableDescription_1_0 'JscFieldDesigns','A field design describes a field of a database table.';
exec spDB_SetFieldDescription_1_0 'JscFieldDesigns', 'TableDesign', 'The database table design to which this field design belongs.';
exec spDB_SetFieldDescription_1_0 'JscFieldDesigns', 'FieldName', 'The name of the field.';
exec spDB_SetFieldDescription_1_0 'JscFieldDesigns', 'Sequence', 'The sequence number of the field within the table.';
exec spDB_SetFieldDescription_1_0 'JscFieldDesigns', 'DataType', 'The data type of the field.';
exec spDB_SetFieldDescription_1_0 'JscFieldDesigns', 'Length', 'The length of the field, if applicable.';
exec spDB_SetFieldDescription_1_0 'JscFieldDesigns', 'FieldClass', 'Dynamics NAV specific: The class of the field.';
exec spDB_SetFieldDescription_1_0 'JscFieldDesigns', 'FieldOption', 'Dynamics NAV specific: The field option of the field.';
exec spDB_SetFieldDescription_1_0 'JscFieldDesigns', 'Enabled', 'Enabled for common use.';

ALTER TABLE [dbo].[JscFieldDesigns] ADD CONSTRAINT [JscFieldDesigns_PK] PRIMARY KEY CLUSTERED
([Id]);

CREATE UNIQUE INDEX [JscFieldDesigns_IX_TableDesign_FieldName] ON [dbo].[JscFieldDesigns]
([TableDesign], [FieldName]);
end

GO

 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscSubJobFromTableFilters')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscSubJobFromTableFilters' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[JscSubJobFromTableFilters] 
	drop column DATAAREAID; 
END
end
else 
begin
CREATE TABLE [dbo].[JscSubJobFromTableFilters]
(
	[Id] UniqueIdentifier NOT NULL,
	[SubJob] UniqueIdentifier NOT NULL,
	[Field] UniqueIdentifier NOT NULL,
	[FilterType] SmallInt NOT NULL,
	[Value1] NVarChar(250),
	[Value2] NVarChar(250),
	[ApplyFilter] SmallInt NOT NULL
);

exec spDB_SetTableDescription_1_0 'JscSubJobFromTableFilters','Currently not used.';

ALTER TABLE [dbo].[JscSubJobFromTableFilters] ADD CONSTRAINT [JscSubJobFromTableFilters_PK] PRIMARY KEY CLUSTERED
([Id]);

end 

GO
 IF Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DDMonitorTriggers')
Begin
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DDMonitorTriggers' AND COLUMN_NAME = 'DATAAREAID')
BEGIN
	ALTER TABLE [dbo].[DDMonitorTriggers] 
	drop column DATAAREAID; 
END
end
GO


IF NOT Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscRepCounters')
Begin
CREATE TABLE [dbo].[JscRepCounters]
(
	[Id] UniqueIdentifier NOT NULL,
	[Job] UniqueIdentifier NOT NULL,
	[SubJob] UniqueIdentifier NOT NULL,
	[Location] UniqueIdentifier NOT NULL,
	[Counter] Int NOT NULL
);

exec spDB_SetTableDescription_1_0 'JscRepCounters','Replication counters. A replication counter is stored for a particular job, sub-job and location.';
exec spDB_SetFieldDescription_1_0 'JscRepCounters', 'Job', 'The job using the counter.';
exec spDB_SetFieldDescription_1_0 'JscRepCounters', 'SubJob', 'The sub-job using the counter.';
exec spDB_SetFieldDescription_1_0 'JscRepCounters', 'Location', 'The location where the counter is used.';
exec spDB_SetFieldDescription_1_0 'JscRepCounters', 'Counter', 'The counter value.';

ALTER TABLE [dbo].[JscRepCounters] ADD CONSTRAINT [JscRepCounters_PK] PRIMARY KEY CLUSTERED
([Id]);

CREATE UNIQUE INDEX [JscRepCounters_IX_Job_SubJob_Location] ON [dbo].[JscRepCounters]
([Job], [SubJob], [Location]);

end
GO


IF NOT Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscLocationMembers')
begin 
CREATE TABLE [dbo].[JscLocationMembers]
(
	[OwnerLocation] UniqueIdentifier NOT NULL,
	[MemberLocation] UniqueIdentifier NOT NULL,
	[Sequence] Int NOT NULL
);

exec spDB_SetTableDescription_1_0 'JscLocationMembers','Implements the many-to-many relationship between JscLocations and itself. An ''owner'' location can have multiple ''member'' locations. A location can be the member of many owner locations.';
exec spDB_SetFieldDescription_1_0 'JscLocationMembers', 'OwnerLocation', 'The owner or parent location.';
exec spDB_SetFieldDescription_1_0 'JscLocationMembers', 'MemberLocation', 'The member or child location.';
exec spDB_SetFieldDescription_1_0 'JscLocationMembers', 'Sequence', 'The UI display sequence of this member.';

ALTER TABLE [dbo].[JscLocationMembers] ADD CONSTRAINT [JscLocationMembers_PK] PRIMARY KEY CLUSTERED
([OwnerLocation], [MemberLocation]);

end

Go 


IF NOT Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscDriverTypes')
begin 

CREATE TABLE [dbo].[JscDriverTypes]
(
	[Id] UniqueIdentifier NOT NULL,
	[Name] NVarChar(60) NOT NULL,
	[DatabaseServerType] SmallInt NOT NULL,
	[DatabaseParams] NVarChar(50),
	[ConnectionStringFormat] NVarChar(250),
	[EnabledFields] NVarChar(250)
);

exec spDB_SetTableDescription_1_0 'JscDriverTypes','Database driver types. A database driver type is used to format the connection string for a location.';
exec spDB_SetFieldDescription_1_0 'JscDriverTypes', 'Name', 'A system-wide unique name of the driver type.';
exec spDB_SetFieldDescription_1_0 'JscDriverTypes', 'DatabaseServerType', 'The type of database server used by this driver type.';
exec spDB_SetFieldDescription_1_0 'JscDriverTypes', 'DatabaseParams', 'Database server/driver specific parameters used in the connection string.';
exec spDB_SetFieldDescription_1_0 'JscDriverTypes', 'ConnectionStringFormat', 'The format of the connection string.';
exec spDB_SetFieldDescription_1_0 'JscDriverTypes', 'EnabledFields', 'A list of the fields is JscLocations supported by this driver type.';

ALTER TABLE [dbo].[JscDriverTypes] ADD CONSTRAINT [JscDriverTypes_PK] PRIMARY KEY CLUSTERED
([Id]);
end

go

IF NOT Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscLinkedTables')
begin 

CREATE TABLE [dbo].[JscLinkedTables]
(
	[Id] UniqueIdentifier NOT NULL,
	[FromTable] UniqueIdentifier NOT NULL,
	[ToTable] UniqueIdentifier NOT NULL
);

exec spDB_SetTableDescription_1_0 'JscLinkedTables','A link or relationship between two tables.';

ALTER TABLE [dbo].[JscLinkedTables] ADD CONSTRAINT [JscLinkedTables_PK] PRIMARY KEY CLUSTERED
([Id]);
end


go



IF NOT Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscLinkedFilters')
begin 


CREATE TABLE [dbo].[JscLinkedFilters]
(
	[Id] UniqueIdentifier NOT NULL,
	[LinkedTable] UniqueIdentifier NOT NULL,
	[LinkedField] UniqueIdentifier NOT NULL,
	[LinkType] SmallInt NOT NULL,
	[ToField] UniqueIdentifier,
	[Filter] NVarChar(250)
);

exec spDB_SetTableDescription_1_0 'JscLinkedFilters','The field/filter used in a link between two tables.';
exec spDB_SetFieldDescription_1_0 'JscLinkedFilters', 'LinkedTable', 'The link to which this filter belongs.';
exec spDB_SetFieldDescription_1_0 'JscLinkedFilters', 'LinkedField', 'The field in the FromTable to use when linking.';
exec spDB_SetFieldDescription_1_0 'JscLinkedFilters', 'LinkType', 'The type of link to use.';
exec spDB_SetFieldDescription_1_0 'JscLinkedFilters', 'ToField', 'The field in the ToTable to use when linking.';
exec spDB_SetFieldDescription_1_0 'JscLinkedFilters', 'Filter', 'An optional filter value to use based on the link type in use.';

ALTER TABLE [dbo].[JscLinkedFilters] ADD CONSTRAINT [JscLinkedFilters_PK] PRIMARY KEY CLUSTERED
([Id]);

end


go

IF NOT Exists(Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JscInfos')
begin 

CREATE TABLE [dbo].[JscInfos]
(
	[Id] UniqueIdentifier NOT NULL,
	[Name] NVarChar(60) NOT NULL,
	[Xml] Xml
);

exec spDB_SetTableDescription_1_0 'JscInfos','A generic information table. Each entry is identified by a name and resolves to an optional XML value.';

ALTER TABLE [dbo].[JscInfos] ADD CONSTRAINT [JscInfos_PK] PRIMARY KEY CLUSTERED
([Id]);

CREATE UNIQUE INDEX [JscInfos_IX_Name] ON [dbo].[JscInfos]
([Name]);

end


go


if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobs_FK_JscLocations_Source')
ALTER TABLE [dbo].[JscJobs] WITH CHECK ADD CONSTRAINT [JscJobs_FK_JscLocations_Source] FOREIGN KEY
([Source]) REFERENCES [dbo].[JscLocations] ([Id]);

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobs_FK_JscLocations_Destination')
ALTER TABLE [dbo].[JscJobs] WITH CHECK ADD CONSTRAINT [JscJobs_FK_JscLocations_Destination] FOREIGN KEY
([Destination]) REFERENCES [dbo].[JscLocations] ([Id]);

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobs_FK_JscJobs_SubjobJob')
ALTER TABLE [dbo].[JscJobs] WITH CHECK ADD CONSTRAINT [JscJobs_FK_JscJobs_SubjobJob] FOREIGN KEY
([SubjobJob]) REFERENCES [dbo].[JscJobs] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscJobTriggers] WITH CHECK ADD CONSTRAINT [JscJobTriggers_FK_JscJobs_Job] FOREIGN KEY
([Job]) REFERENCES [dbo].[JscJobs] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscJobSubjobs] WITH CHECK ADD CONSTRAINT [JscJobSubjobs_FK_JscJobs_Job] FOREIGN KEY
([Job]) REFERENCES [dbo].[JscJobs] ([Id]);

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscJobSubjobs] WITH CHECK ADD CONSTRAINT [JscJobSubjobs_FK_JscSubJobs_SubJob] FOREIGN KEY
([SubJob]) REFERENCES [dbo].[JscSubJobs] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscSubJobs] WITH CHECK ADD CONSTRAINT [JscSubJobs_FK_JscTableDesigns_TableFrom] FOREIGN KEY
([TableFrom]) REFERENCES [dbo].[JscTableDesigns] ([Id]);

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscSubJobs] WITH CHECK ADD CONSTRAINT [JscSubJobs_FK_JscTableDesigns_ActionTable] FOREIGN KEY
([ActionTable]) REFERENCES [dbo].[JscTableDesigns] ([Id]);

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscSubJobs] WITH CHECK ADD CONSTRAINT [JscSubJobs_FK_JscFieldDesigns_RepCounterField] FOREIGN KEY
([RepCounterField]) REFERENCES [dbo].[JscFieldDesigns] ([Id]);

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscSubJobs] WITH CHECK ADD CONSTRAINT [JscSubJobs_FK_JscFieldDesigns_MarkSentRecordsField] FOREIGN KEY
([MarkSentRecordsField]) REFERENCES [dbo].[JscFieldDesigns] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscSubJobFromTableFilters] WITH CHECK ADD CONSTRAINT [JscSubJobFromTableFilters_FK_JscSubJobs_SubJob] FOREIGN KEY
([SubJob]) REFERENCES [dbo].[JscSubJobs] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscSubJobFromTableFilters] WITH CHECK ADD CONSTRAINT [JscSubJobFromTableFilters_FK_JscFieldDesigns_Field] FOREIGN KEY
([Field]) REFERENCES [dbo].[JscFieldDesigns] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscRepCounters] WITH CHECK ADD CONSTRAINT [JscRepCounters_FK_JscJobs_Job] FOREIGN KEY
([Job]) REFERENCES [dbo].[JscJobs] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscRepCounters] WITH CHECK ADD CONSTRAINT [JscRepCounters_FK_JscSubJobs_SubJob] FOREIGN KEY
([SubJob]) REFERENCES [dbo].[JscSubJobs] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscRepCounters] WITH CHECK ADD CONSTRAINT [JscRepCounters_FK_JscLocations_Location] FOREIGN KEY
([Location]) REFERENCES [dbo].[JscLocations] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLocations] WITH CHECK ADD CONSTRAINT [JscLocations_FK_JscDatabaseDesigns_DatabaseDesign] FOREIGN KEY
([DatabaseDesign]) REFERENCES [dbo].[JscDatabaseDesigns] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLocations] WITH CHECK ADD CONSTRAINT [JscLocations_FK_JscLocations_DefaultOwner] FOREIGN KEY
([DefaultOwner]) REFERENCES [dbo].[JscLocations] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLocations] WITH CHECK ADD CONSTRAINT [JscLocations_FK_JscDriverTypes_DBDriverType] FOREIGN KEY
([DBDriverType]) REFERENCES [dbo].[JscDriverTypes] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLocationMembers] WITH CHECK ADD CONSTRAINT [JscLocationMembers_FK_JscLocations_OwnerLocation] FOREIGN KEY
([OwnerLocation]) REFERENCES [dbo].[JscLocations] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLocationMembers] WITH CHECK ADD CONSTRAINT [JscLocationMembers_FK_JscLocations_MemberLocation] FOREIGN KEY
([MemberLocation]) REFERENCES [dbo].[JscLocations] ([Id]);
GO

GO

GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscTableDesigns] WITH CHECK ADD CONSTRAINT [JscTableDesigns_FK_JscDatabaseDesigns_DatabaseDesign] FOREIGN KEY
([DatabaseDesign]) REFERENCES [dbo].[JscDatabaseDesigns] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscFieldDesigns] WITH CHECK ADD CONSTRAINT [JscFieldDesigns_FK_JscTableDesigns_TableDesign] FOREIGN KEY
([TableDesign]) REFERENCES [dbo].[JscTableDesigns] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLinkedTables] WITH CHECK ADD CONSTRAINT [JscLinkedTables_FK_JscTableDesigns_FromTable] FOREIGN KEY
([FromTable]) REFERENCES [dbo].[JscTableDesigns] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLinkedTables] WITH CHECK ADD CONSTRAINT [JscLinkedTables_FK_JscTableDesigns_ToTable] FOREIGN KEY
([ToTable]) REFERENCES [dbo].[JscTableDesigns] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLinkedFilters] WITH CHECK ADD CONSTRAINT [JscLinkedFilters_FK_JscLinkedTables_LinkedTable] FOREIGN KEY
([LinkedTable]) REFERENCES [dbo].[JscLinkedTables] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLinkedFilters] WITH CHECK ADD CONSTRAINT [JscLinkedFilters_FK_JscFieldDesigns_LinkedField] FOREIGN KEY
([LinkedField]) REFERENCES [dbo].[JscFieldDesigns] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscLinkedFilters] WITH CHECK ADD CONSTRAINT [JscLinkedFilters_FK_JscFieldDesigns_ToField] FOREIGN KEY
([ToField]) REFERENCES [dbo].[JscFieldDesigns] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscTableMap] WITH CHECK ADD CONSTRAINT [JscTableMap_FK_JscTableDesigns_FromTable] FOREIGN KEY
([FromTable]) REFERENCES [dbo].[JscTableDesigns] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscTableMap] WITH CHECK ADD CONSTRAINT [JscTableMap_FK_JscTableDesigns_ToTable] FOREIGN KEY
([ToTable]) REFERENCES [dbo].[JscTableDesigns] ([Id]);
GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscFieldMap] WITH CHECK ADD CONSTRAINT [JscFieldMap_FK_JscTableMap_TableMap] FOREIGN KEY
([TableMap]) REFERENCES [dbo].[JscTableMap] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscFieldMap] WITH CHECK ADD CONSTRAINT [JscFieldMap_FK_JscFieldDesigns_FromField] FOREIGN KEY
([FromField]) REFERENCES [dbo].[JscFieldDesigns] ([Id]);
if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscFieldMap] WITH CHECK ADD CONSTRAINT [JscFieldMap_FK_JscFieldDesigns_ToField] FOREIGN KEY
([ToField]) REFERENCES [dbo].[JscFieldDesigns] ([Id]);
GO

GO

if not exists (Select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = 'JscJobTriggers_FK_JscJobs_Job')
ALTER TABLE [dbo].[JscSchedulerLog] WITH CHECK ADD CONSTRAINT [JscSchedulerLog_FK_JscJobs_Job] FOREIGN KEY
([Job]) REFERENCES [dbo].[JscJobs] ([Id]);
GO

if not exists(select * from [dbo].[JscDriverTypes]  where Id = '97290f9c-6c5b-41ef-a5b8-c35690e831cf')
INSERT INTO [dbo].[JscDriverTypes] ([Id], [Name], [DatabaseServerType], [DatabaseParams], [ConnectionStringFormat], [EnabledFields])
VALUES('97290f9c-6c5b-41ef-a5b8-c35690e831cf', 'NAV SQL 6.01', 0, 'ndbcs@601', 'id={Code};company={Company};server={DBServerHost};dbname={DBPathName};nt=tcp;user={UserId};passwd={Password};|{DatabaseServerType}|{DatabaseParams}','Company,UserId,Password,DBServerHost,DBPathName,NetType');

if not exists(select * from [dbo].[JscDriverTypes]  where Id = 'f3b1cd7c-1e8d-452e-b0f6-53e240e6c176')
INSERT INTO [dbo].[JscDriverTypes] ([Id], [Name], [DatabaseServerType], [DatabaseParams], [ConnectionStringFormat], [EnabledFields])
VALUES('f3b1cd7c-1e8d-452e-b0f6-53e240e6c176', 'NAV Native 6.01', 0, 'ndbcn@601', 'id={Code};company={Company};server={DBServerHost};nt=tcp;user={UserId};passwd={Password};|{DatabaseServerType}|{DatabaseParams}','Company,UserId,Password,DBServerHost,DBPathName,NetType');

if not exists(select * from [dbo].[JscDriverTypes]  where Id = 'c3ebc991-357e-4799-ab3a-8f1691bb0bd1')
INSERT INTO [dbo].[JscDriverTypes] ([Id], [Name], [DatabaseServerType], [DatabaseParams], [ConnectionStringFormat], [EnabledFields])
VALUES('c3ebc991-357e-4799-ab3a-8f1691bb0bd1', 'MS SQL', 2, NULL, 'Data Source={DBServerHost};Initial Catalog={DBPathName};User ID={UserId};Password={Password};|{DatabaseServerType}|{DatabaseParams}','UserId,Password,DBServerHost,DBPathName');

if not exists(select * from [dbo].[JscDriverTypes]  where Id = '869e6ce1-600b-4a8a-a2a8-79473cd49bb2')
INSERT INTO [dbo].[JscDriverTypes] ([Id], [Name], [DatabaseServerType], [DatabaseParams], [ConnectionStringFormat], [EnabledFields])
VALUES('869e6ce1-600b-4a8a-a2a8-79473cd49bb2', 'MS SQL (Windows authentication)', 2, NULL, 'Data Source={DBServerHost};Initial Catalog={DBPathName};Integrated Security=SSPI;|{DatabaseServerType}|{DatabaseParams}','DBServerHost,DBPathName');

if not exists(select * from [dbo].[JscDriverTypes]  where Id = '67bc214e-293a-4ea7-99f7-76f612172b07')
INSERT INTO [dbo].[JscDriverTypes] ([Id], [Name], [DatabaseServerType], [DatabaseParams], [ConnectionStringFormat], [EnabledFields])
VALUES('67bc214e-293a-4ea7-99f7-76f612172b07', 'OLE DB (Windows authentication)', 6, NULL, 'Provider=SQLOLEDB;Data Source={DBServerHost};Initial Catalog={DBPathName};Integrated Security=SSPI;|{DatabaseServerType}|{DatabaseParams}','DBServerHost,DBPathName');

if not exists(select * from [dbo].[JscDriverTypes]  where Id = '2c844cff-886d-414c-8d1f-081e291a07c0')
INSERT INTO [dbo].[JscDriverTypes] ([Id], [Name], [DatabaseServerType], [DatabaseParams], [ConnectionStringFormat], [EnabledFields])
VALUES('2c844cff-886d-414c-8d1f-081e291a07c0', 'OLE DB', 6, NULL, 'Provider=SQLOLEDB;Data Source={DBServerHost};Initial Catalog={DBPathName};User ID={UserId};Password={Password};|{DatabaseServerType}|{DatabaseParams}','UserId,Password,DBServerHost,DBPathName');

UPDATE [dbo].[JscDriverTypes] 
SET
	[ConnectionStringFormat] = 'Data Source={DBServerHost};Initial Catalog={DBPathName};User ID={UserId};Password={Password};Network Library={ConnectionType};|{DatabaseServerType}|{DatabaseParams}',
	[EnabledFields] = 'UserId,Password,DBServerHost,DBPathName,ConnectionType'
WHERE
	[Id] = 'c3ebc991-357e-4799-ab3a-8f1691bb0bd1';



UPDATE [dbo].[JscDriverTypes] 
SET
	[ConnectionStringFormat] = 'Data Source={DBServerHost};Initial Catalog={DBPathName};Integrated Security=SSPI;Network Library={ConnectionType};|{DatabaseServerType}|{DatabaseParams}',
	[EnabledFields] = 'DBServerHost,DBPathName,ConnectionType'
WHERE
	[Id] = '869e6ce1-600b-4a8a-a2a8-79473cd49bb2';

exec spDB_SetTableDescription_1_0 'JscSubJobFromTableFilters','From-table filters of a subjob. Used to filter rows when selecting source data for a subjob.';

DECLARE @dataAreaID NVarChar(10)
DECLARE @permissionGroupGuid UniqueIdentifier

SET @dataAreaID = 'LSR'
SET @permissionGroupGuid = '{7ccab506-b497-403e-99e3-7141655e5bfc}'

IF NOT EXISTS (SELECT * FROM [PERMISSIONGROUP] WHERE [GUID] = @permissionGroupGuid)
BEGIN
    INSERT INTO [PERMISSIONGROUP] ([GUID],[Name],[DATAAREAID]) 
        VALUES (@permissionGroupGuid,'Replication',@dataAreaID)
END



GO

