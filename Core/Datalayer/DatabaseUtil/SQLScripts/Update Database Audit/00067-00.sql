/*
	Description		: Adding audit table for USERTOKENLOGINS
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[USERLOGINTOKENSLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[USERLOGINTOKENSLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [GUID] [uniqueidentifier] NOT NULL,
    [USERGUID] [uniqueidentifier] NOT NULL,
    [DESCRIPTION] [nvarchar] (40) NOT NULL,
    [HASH] [nvarchar] (40) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.USERLOGINTOKENSLog add constraint PK_USERLOGINTOKENSLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_USERLOGINTOKENSLog_AuditUserGUID ON dbo.USERLOGINTOKENSLog (AuditUserGUID) ON [PRIMARY]
END
GO
