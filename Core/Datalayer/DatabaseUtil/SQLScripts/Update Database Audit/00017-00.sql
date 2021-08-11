/*

	Incident No.	: 8585
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET v 2011 - Sprint 5
	Date created	: 04.2.2011
	
	Description		: Add audit tables for a few hospitality tables

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	DININGTABLELAYOUTSCREENLog	- Table added
						POSCOLORLog					- Table added
						RESTAURANTDININGTABLELog	- Table added
						RESTAURANTMENUTYPELog		- Table added
						STATIONPRINTINGROUTESLog	- Table added
						STATIONSELECTIONLog			- Table added
							
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[DININGTABLELAYOUTSCREENLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[DININGTABLELAYOUTSCREENLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [RESTAURANTID] [nvarchar] (20) NOT NULL,
    [SEQUENCE] [int] NOT NULL,
    [SALESTYPE] [nvarchar] (20) NOT NULL,
    [LAYOUTID] [nvarchar] (20) NOT NULL,
    [SCREENNO] [int] NOT NULL,
    [SCREENDESCRIPTION] [nvarchar] (30) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.DININGTABLELAYOUTSCREENLog add constraint PK_DININGTABLELAYOUTSCREENLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_DININGTABLELAYOUTSCREENLog_AuditUserGUID ON dbo.DININGTABLELAYOUTSCREENLog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSCOLORLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSCOLORLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [COLORCODE] [nvarchar] (20) NOT NULL,
    [DESCRIPTION] [nvarchar] (30) NULL,
    [COLOR] [int] NULL,
    [BOLD] [tinyint] NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.POSCOLORLog add constraint PK_POSCOLORLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSCOLORLog_AuditUserGUID ON dbo.POSCOLORLog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RESTAURANTDININGTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RESTAURANTDININGTABLELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [RESTAURANTID] [nvarchar] (20) NOT NULL,
    [SALESTYPE] [nvarchar] (20) NOT NULL,
    [DINEINTABLENO] [int] NOT NULL,
    [DESCRIPTION] [nvarchar] (30) NULL,
    [TYPE] [int] NULL,
    [NONSMOKING] [tinyint] NULL,
    [NOOFGUESTS] [int] NULL,
    [JOINEDTABLE] [int] NULL,
    [X1POSITION] [int] NULL,
    [X2POSITION] [int] NULL,
    [Y1POSITION] [int] NULL,
    [Y2POSITION] [int] NULL,
    [LINKEDTODINEINTABLE] [int] NULL,
    [DININGTABLESJOINED] [tinyint] NULL,
    [SEQUENCE] [int] NOT NULL,
    [AVAILABILITY] [int] NULL,
    [DININGTABLELAYOUTID] [nvarchar] (20) NOT NULL,
    [LAYOUTSCREENNO] [int] NULL,
    [DESCRIPTIONONBUTTON] [nvarchar] (30) NULL,
    [SHAPE] [int] NULL,
    [X1POSITIONDESIGN] [int] NULL,
    [X2POSITIONDESIGN] [int] NULL,
    [Y1POSITIONDESIGN] [int] NULL,
    [Y2POSITIONDESIGN] [int] NULL,
    [LAYOUTSCREENNODESIGN] [int] NULL,
    [JOINEDTABLEDESIGN] [int] NULL,
    [DININGTABLESJOINEDDESIGN] [tinyint] NULL,
    [DELETEINOTHERLAYOUTS] [tinyint] NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RESTAURANTDININGTABLELog add constraint PK_RESTAURANTDININGTABLELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RESTAURANTDININGTABLELog_AuditUserGUID ON dbo.RESTAURANTDININGTABLELog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RESTAURANTMENUTYPELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RESTAURANTMENUTYPELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [RESTAURANTID] [nvarchar] (20) NOT NULL,
    [MENUORDER] [int] NOT NULL,
    [DESCRIPTION] [nvarchar] (30) NULL,
    [CODEONPOS] [nvarchar] (1) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RESTAURANTMENUTYPELog add constraint PK_RESTAURANTMENUTYPELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RESTAURANTMENUTYPELog_AuditUserGUID ON dbo.RESTAURANTMENUTYPELog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[STATIONPRINTINGROUTESLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[STATIONPRINTINGROUTESLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [ROUTEDESCRIPTION] [nvarchar] (30) NULL,
    [RESTAURANTID] [nvarchar] (20) NOT NULL,
    [PASSWORD] [nvarchar] (20) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.STATIONPRINTINGROUTESLog add constraint PK_STATIONPRINTINGROUTESLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_STATIONPRINTINGROUTESLog_AuditUserGUID ON dbo.STATIONPRINTINGROUTESLog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[STATIONSELECTIONLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[STATIONSELECTIONLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [TYPE] [int] NOT NULL,
    [CODE] [nvarchar] (20) NOT NULL,
    [STATIONID] [nvarchar] (20) NOT NULL,
    [SALESTYPE] [nvarchar] (20) NOT NULL,
    [RESTAURANTID] [nvarchar] (20) NOT NULL,
    [ROUTEID] [nvarchar] (20) NOT NULL,
    [DATAAREAID] [nvarchar] (20) NOT NULL,
    Deleted bit NULL)

    alter table dbo.STATIONSELECTIONLog add constraint PK_STATIONSELECTIONLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_STATIONSELECTIONLog_AuditUserGUID ON dbo.STATIONSELECTIONLog (AuditUserGUID) ON [PRIMARY]
END
GO