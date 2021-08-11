
/*

	Incident No.	: 
	Responsible		: Guðbjörn Einarsson	
	Sprint			: LS Retail .NET Milky Way/Sprint 2 Mercury
	Date created	: 30.07.2012
	
	Description		: Add KMINTERFACEPROFILELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : KMINTERFACEPROFILELog - Added
						
*/


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KMINTERFACEPROFILELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].KMINTERFACEPROFILELog(
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar](20) NOT NULL,
	[ORDERPANEWIDTH] [numeric](24, 6) NULL,
	[ORDERPANEHEIGHT] [numeric](24, 6) NULL,
	[ORDERPANEXPOS] [numeric](24, 6) NULL,
	[ORDERPANEYPOS] [numeric](24, 6) NULL,
	[FUNCTIONBARPANEWIDTH] [numeric](24, 6) NULL,
	[FUNCTIONBARPANEHEIGHT] [numeric](24, 6) NULL,
	[FUNCTIONBARPANEXPOS] [numeric](24, 6) NULL,
	[FUNCTIONBARPANEYPOS] [numeric](24, 6) NULL,
	[FUNCTIONBARMENUID] [nvarchar](20) NULL,
	[FUNCTIONBARPANEVISIBLE] [tinyint] NULL,
	[ZOOMPANEWIDTH] [numeric](24, 6) NULL,
	[ZOOMPANEHEIGHT] [numeric](24, 6) NULL,
	[ZOOMPANEXPOS] [numeric](24, 6) NULL,
	[ZOOMPANEYPOS] [numeric](24, 6) NULL,
	[ZOOMPANEVISIBLE] [tinyint] NULL,
	[ITEMQUEUEPANEWIDTH] [numeric](24, 6) NULL,
	[ITEMQUEUEPANEHEIGHT] [numeric](24, 6) NULL,
	[ITEMQUEUEPANEXPOS] [numeric](24, 6) NULL,
	[ITEMQUEUEPANEYPOS] [numeric](24, 6) NULL,
	[ITEMQUEUEPANEVISIBLE] [tinyint] NULL,
	[ORDERTIMEDISPLAY] [int] NULL,
	[FLOWPATH] [int] NULL,
	[TIMERINTERVAL] [int] NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[BUMPOPERATION] [int] NULL,
	[SHOWBUMPED] [int] NULL,
	[ORDERSTYLEID] [nvarchar](20) NULL,
	[ORDERPANESTYLEID] [nvarchar](20) NULL,
	[BUTTONPANESTYLEID] [nvarchar](20) NULL,
	[ZOOMPANESTYLEID] [nvarchar](20) NULL,
	[ITEMQUEUEPANESTYLEID] [nvarchar](20) NULL,
    Deleted bit NULL)

    alter table dbo.KMINTERFACEPROFILELog add constraint PK_KMINTERFACEPROFILELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_KMINTERFACEPROFILELog_AuditUserGUID ON dbo.KMINTERFACEPROFILELog (AuditUserGUID) ON [PRIMARY]
END
GO

