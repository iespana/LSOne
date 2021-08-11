﻿IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOTERMINALGROUPLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOTERMINALGROUPLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    [DESCRIPTION] [nvarchar] (250) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOTERMINALGROUPLog add constraint PK_RBOTERMINALGROUPLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOTERMINALGROUPLog_AuditUserGUID ON dbo.RBOTERMINALGROUPLog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOTERMINALGROUPCONNECTIONLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOTERMINALGROUPCONNECTIONLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [TERMINALGROUPID] [uniqueidentifier] NOT NULL,
    [TERMINALID] [nvarchar] (20) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOTERMINALGROUPCONNECTIONLog add constraint PK_RBOTERMINALGROUPCONNECTIONLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOTERMINALGROUPCONNECTIONLog_AuditUserGUID ON dbo.RBOTERMINALGROUPCONNECTIONLog (AuditUserGUID) ON [PRIMARY]
END
GO

