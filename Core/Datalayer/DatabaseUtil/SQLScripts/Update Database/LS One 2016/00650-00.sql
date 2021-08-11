
/*

	Incident No.	: ONE-2840
	Responsible		: Indriði
	Sprint			: Copenhagen

	Description		: A table to handle info on backups
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/
USE LSPOSNET
GO

IF Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BACKUPINFO')
BEGIN
	CREATE TABLE [dbo].[BACKUPINFO](
	[ID] [uniqueidentifier] NOT NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL CONSTRAINT [DF_BACKUPINFO_DATAAREAID]  DEFAULT ('LSR'),
	[CREATED] [datetime] NOT NULL DEFAULT (getdate()),
	[FOLDER] [ntext] NOT NULL,
	[BACKUPSTATUS] [smallint] NOT NULL DEFAULT ((1)),
	[AUDITBACKUPSTATUS] [smallint] NOT NULL DEFAULT ((1)),
	[USERID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BACKUPINFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[DATAAREAID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END




