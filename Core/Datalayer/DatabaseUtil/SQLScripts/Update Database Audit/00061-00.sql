
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


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KITCHENDISPLAYSTATIONSLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[KITCHENDISPLAYSTATIONSLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [NAME] [nvarchar] (100) NOT NULL,
    [SCREENNUMBER] [int] NOT NULL,
    [SCREENALIGNMENT] [int] NOT NULL,
    [KITCHENDISPLAYFUNCTIONALPROFILEID] [uniqueidentifier] NOT NULL,
    [KITCHENDISPLAYSTYLEPROFILEID] [uniqueidentifier] NOT NULL,
    [KITCHENDISPLAYVISUALPROFILEID] [uniqueidentifier] NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.KITCHENDISPLAYSTATIONSLog add constraint PK_KITCHENDISPLAYSTATIONSLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_KITCHENDISPLAYSTATIONSLog_AuditUserGUID ON dbo.KITCHENDISPLAYSTATIONSLog (AuditUserGUID) ON [PRIMARY]
END
GO

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KITCHENDISPLAYITEMCONNECTIONLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[KITCHENDISPLAYITEMCONNECTIONLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    [STATIONID] [nvarchar] (20) NOT NULL,
    [TYPE] [int] NOT NULL,
    [CONNECTIONVALUE] [nvarchar] (20) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.KITCHENDISPLAYITEMCONNECTIONLog add constraint PK_KITCHENDISPLAYITEMCONNECTIONLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_KITCHENDISPLAYITEMCONNECTIONLog_AuditUserGUID ON dbo.KITCHENDISPLAYITEMCONNECTIONLog (AuditUserGUID) ON [PRIMARY]
END
GO
