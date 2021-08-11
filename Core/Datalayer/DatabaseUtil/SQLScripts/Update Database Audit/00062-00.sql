
/*

	Incident No.	: 
	Responsible		: Óskar Bjarnason
	Sprint			: 
	Date created	: 10.12.2012
		
	Description		: Remove table KMINTERFACEPROFILELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : KMINTERFACEPROFILELog
						
*/


USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KITCHENDISPLAYVISUALPROFILELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[KITCHENDISPLAYVISUALPROFILELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    [NAME] [nvarchar] (100) NOT NULL,
    [ORDERPANEWIDTH] [numeric] (24,6) NOT NULL,
    [ORDERPANEHEIGHT] [numeric] (24,6) NOT NULL,
    [ORDERPANEX] [numeric] (24,6) NOT NULL,
    [ORDERPANEY] [numeric] (24,6) NOT NULL,
    [ORDERPANEVISIBLE] [tinyint] NOT NULL,
    [BUTTONPANEWIDTH] [numeric] (24,6) NOT NULL,
    [BUTTONPANEHEIGHT] [numeric] (24,6) NOT NULL,
    [BUTTONPANEX] [numeric] (24,6) NOT NULL,
    [BUTTONPANEY] [numeric] (24,6) NOT NULL,
    [BUTTONPANEVISIBLE] [tinyint] NOT NULL,
    [FLOWPATH] [int] NOT NULL,
    [TOPLEFT] [int] NOT NULL,
    [TOPCENTER] [int] NOT NULL,
    [TOPRIGHT] [int] NOT NULL,
    [BOTTOMLEFT] [int] NOT NULL,
    [BOTTOMRIGHT] [int] NOT NULL,
    [BOTTOMCENTER] [int] NOT NULL,
    [NUMBEROFCOLUMNS] [int] NOT NULL,
    [NUMBEROFROWS] [int] NOT NULL,
    [TIMEFORMAT] [nvarchar] (10) NULL,
    [ITEMMODIFIERINCREASEPREFIX] [nvarchar] (30) NULL,
    [ITEMMODIFIERDECREASEPREFIX] [nvarchar] (30) NULL,
    [ITEMMODIFIERNORMALPREFIX] [nvarchar] (30) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    [SHOWDEALS] [tinyint] NULL,
    [BORDERCOLOR] [int] NULL,
    Deleted bit NULL)

    alter table dbo.KITCHENDISPLAYVISUALPROFILELog add constraint PK_KITCHENDISPLAYVISUALPROFILELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_KITCHENDISPLAYVISUALPROFILELog_AuditUserGUID ON dbo.KITCHENDISPLAYVISUALPROFILELog (AuditUserGUID) ON [PRIMARY]
END
GO


GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KITCHENDISPLAYSTYLEPROFILELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[KITCHENDISPLAYSTYLEPROFILELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    [NAME] [nvarchar] (100) NOT NULL,
    [ORDERSTYLEID] [nvarchar] (20) NULL,
    [ORDERPANESTYLEID] [nvarchar] (20) NULL,
    [BUTTONPANESTYLEID] [nvarchar] (20) NULL,
    [ITEMDEFAULTSTYLEID] [nvarchar] (20) NULL,
    [BUMPEDSTYLEID] [nvarchar] (20) NULL,
    [STARTEDSTYLEID] [nvarchar] (20) NULL,
    [MODIFIEDITEMSTYLEID] [nvarchar] (20) NULL,
    [NORMALITEMMODIFIERSTYLEID] [nvarchar] (20) NULL,
    [INCREASEITEMMODIFIERSTYLEID] [nvarchar] (20) NULL,
    [DECREASEITEMMODIFIERSTYLEID] [nvarchar] (20) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.KITCHENDISPLAYSTYLEPROFILELog add constraint PK_KITCHENDISPLAYSTYLEPROFILELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_KITCHENDISPLAYSTYLEPROFILELog_AuditUserGUID ON dbo.KITCHENDISPLAYSTYLEPROFILELog (AuditUserGUID) ON [PRIMARY]
END
GO
