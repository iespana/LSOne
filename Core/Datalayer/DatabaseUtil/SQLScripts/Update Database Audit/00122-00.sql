/*
	Incident No.	: ONE-7185
	Responsible		: Adrian Chiorean
	Sprint			: Vardagen
	Date created	: 13.07.2017

	Description		: Create OPERATIONS log table
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[OPERATIONSLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[OPERATIONSLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [MASTERID] [uniqueidentifier] NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [DESCRIPTION] [nvarchar] (50) NOT NULL,
    [TYPE] [int] NOT NULL,
    [LOOKUPTYPE] [int] NOT NULL,
    [AUDIT] [tinyint] NULL,
    Deleted bit NULL)

    alter table dbo.OPERATIONSLog add constraint PK_OPERATIONSLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_OPERATIONSLog_AuditUserGUID ON dbo.OPERATIONSLog (AuditUserGUID) ON [PRIMARY]
END
GO
