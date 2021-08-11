/*

	Incident No.	: ONE-5578
	Responsible		: Indriði Ingi Stefánsson	
	Sprint			: Oregano 17.11 - 1.12
	Date created	: 24.11.2016

	Description		: Adding timekeeper table
	
	
	Tables affected	: USERKEPTTIMES
						
*/

USE LSPOSNET
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'USERKEPTTIMES' )
BEGIN
CREATE TABLE [USERKEPTTIMES](
	[GUID] [uniqueidentifier] NOT NULL,
	[USERGUID] [uniqueidentifier] NOT NULL,
	[TIMETOKEEP] [datetime] NOT NULL,
	[TIMETYPE] [smallint] NOT NULL,
	[COMMENT] [varchar](1000) NULL,
	[CLOSESENTRY] [uniqueidentifier] NULL,
	CONSTRAINT [PK_USERKEPTTIMES] PRIMARY KEY CLUSTERED 
	(
		[GUID] ASC
	)
)

END

GO
