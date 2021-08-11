/*

	Incident No.	: ONE-2455
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Muskat

	Description		: New audit table created
	
						
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[PAYMENTLIMITATIONSLog]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[PAYMENTLIMITATIONSLog](
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[MASTERID] [uniqueidentifier] NOT NULL,
		[TENDERID] [nvarchar](20) NOT NULL,
		[RESTRICTIONCODE] [nvarchar](200) NOT NULL,
		[TYPE] [int] NOT NULL,
		[RELATIONMASTERID] [uniqueidentifier] NOT NULL,		
		[INCLUDE] [int] NULL,		
		[Action] nvarchar(10))
	

	alter table dbo.PAYMENTLIMITATIONSLog add constraint PK_PAYMENTLIMITATIONSLog
	primary key clustered (AuditID) on [PRIMARY]

	create nonclustered index IX_PAYMENTLIMITATIONSLog_AuditUserGUID ON dbo.PAYMENTLIMITATIONSLog (AuditUserGUID) ON [PRIMARY]
END
GO
