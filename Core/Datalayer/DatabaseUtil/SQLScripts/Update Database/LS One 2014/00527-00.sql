
/*
	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson	
	Sprint			: LS One 2014
	Date created	: 9.04.2014

	Description		: Added new fields to track original store and receipt ID of sold items
						
*/
USE LSPOSNET
GO

IF Not EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'JscSchedulerSubLog' )
BEGIN
CREATE TABLE [dbo].[JscSchedulerSubLog](
	[Id] [uniqueidentifier] NOT NULL,
	[SchedulerLogId] [uniqueidentifier] NOT NULL,
	[SubJob] [uniqueidentifier] NOT NULL,
	[ReplicationCounterStart] [int] NOT NULL,
	[ReplicationCounterEnd] [int] NULL,
	[RunAsNormal] [bit] NULL,
	[Location] [uniqueidentifier] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
 CONSTRAINT [JscSchedulerSubLog_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) 


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A generic message logging table.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JscSchedulerSubLog'

END;

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscSchedulerLog' AND COLUMN_NAME = 'IsError') 
BEGIN
	ALTER TABLE JscSchedulerLog ADD IsError BIT not null DEFAULT 0
END;



