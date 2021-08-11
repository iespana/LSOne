/*
	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 13.01.2014

	Description		: Adding new Job Fields
	
						
*/

GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'JscJobs' )
BEGIN
	IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobs' AND COLUMN_NAME = 'JobTypeCode') 
	BEGIN
	  ALTER TABLE JscJobs ADD JobTypeCode VARCHAR(30) NULL 
	END;
	

	IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobs' AND COLUMN_NAME = 'UseCurrentLocation') 
	BEGIN
	  ALTER TABLE JscJobs ADD UseCurrentLocation BIT NULL 
	END;

	IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobs' AND COLUMN_NAME = 'PluginPath') 
	BEGIN
		ALTER TABLE JSCJOBS	ADD PluginPath VARCHAR(250) NULL;
	END;
	IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobs' AND COLUMN_NAME = 'PluginArguments') 
	BEGIN
		ALTER TABLE JSCJOBS	ADD PluginArguments VARCHAR (MAX) NULL;
	END;
END;
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A generic message logging table.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JscSchedulerSubLog'

END;
GO
