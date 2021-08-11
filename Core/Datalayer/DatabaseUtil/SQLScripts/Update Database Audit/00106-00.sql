USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[IMPORTPROFILELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[IMPORTPROFILELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [MASTERID] [uniqueidentifier] NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [DESCRIPTION] [nvarchar] (60) NULL,
    [IMPORTTYPE] [int] NOT NULL,
    [DEFAULT] [tinyint] NOT NULL,
    Deleted bit NULL)

    alter table dbo.IMPORTPROFILELog add constraint PK_IMPORTPROFILELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_IMPORTPROFILELog_AuditUserGUID ON dbo.IMPORTPROFILELog (AuditUserGUID) ON [PRIMARY]
END
GO


USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[IMPORTPROFILELINESLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[IMPORTPROFILELINESLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [MASTERID] [uniqueidentifier] NOT NULL,
    [IMPORTPROFILEMASTERID] [uniqueidentifier] NOT NULL,
    [FIELD] [int] NOT NULL,
    [FIELDTYPE] [int] NOT NULL,
    [SEQUENCE] [int] NOT NULL,
    Deleted bit NULL)

    alter table dbo.IMPORTPROFILELINESLog add constraint PK_IMPORTPROFILELINESLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_IMPORTPROFILELINESLog_AuditUserGUID ON dbo.IMPORTPROFILELINESLog (AuditUserGUID) ON [PRIMARY]
END
GO