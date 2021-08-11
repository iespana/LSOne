
/*

	Incident No.	: 19166
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET Milkyway\Earth
	Date created	: 9.10.2012

	Description		: Creates the audit table for style profiles

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosStyleProfileLog
						
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSSTYLEPROFILELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSSTYLEPROFILELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [NAME] [nvarchar] (60) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.POSSTYLEPROFILELog add constraint PK_POSSTYLEPROFILELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSSTYLEPROFILELog_AuditUserGUID ON dbo.POSSTYLEPROFILELog (AuditUserGUID) ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSSTYLEPROFILELINELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSSTYLEPROFILELINELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [PROFILEID] [nvarchar] (20) NOT NULL,
    [MENUID] [nvarchar] (20) NULL,
    [CONTEXTID] [nvarchar] (20) NULL,
    [STYLEID] [nvarchar] (20) NULL,
    [SYSTEM] [tinyint] NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.POSSTYLEPROFILELINELog add constraint PK_POSSTYLEPROFILELINELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSSTYLEPROFILELINELog_AuditUserGUID ON dbo.POSSTYLEPROFILELINELog (AuditUserGUID) ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSCONTEXTLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSCONTEXTLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [NAME] [nvarchar] (60) NULL,
    [MENUREQUIRED] [tinyint] NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.POSCONTEXTLog add constraint PK_POSCONTEXTLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSCONTEXTLog_AuditUserGUID ON dbo.POSCONTEXTLog (AuditUserGUID) ON [PRIMARY]
END
GO


