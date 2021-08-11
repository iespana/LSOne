
/*

	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: 2011 - Sprint 5
	Date created	: 07.01.2011

	Description		: Add tables for hospitality auditing

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	:	DININGTABLELAYOUTLog	-	table added
						HOSPITALITYTYPELog		-	table added
						POSLOOKUPLog			-	table added
						POSMENUHEADERLog		-	table added
						POSMENULINELog			-   table added
						RESTAURANTSTATIONLog	-	table added
						
*/

USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[DININGTABLELAYOUTLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[DININGTABLELAYOUTLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[RESTAURANTID] [nvarchar](20) NOT NULL,
	[SEQUENCE] [int] NOT NULL,
	[SALESTYPE] [nvarchar](20) NOT NULL,
	[LAYOUTID] [nvarchar](20) NOT NULL,
	[DESCRIPTION] [nvarchar](30) NULL,
	[NOOFSCREENS] [int] NULL,
	[STARTINGTABLENO] [int] NULL,
	[NOOFDININGTABLES] [int] NULL,
	[ENDINGTABLENO] [int] NULL,
	[DININGTABLEROWS] [int] NULL,
	[DININGTABLECOLUMNS] [int] NULL,
	[CURRENTLAYOUT] [tinyint] NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	Deleted bit NULL)	

	alter table dbo.DININGTABLELAYOUTLog add constraint PK_DININGTABLELAYOUTLog
	primary key clustered (AuditID) on [PRIMARY]
	
	create nonclustered index IX_DININGTABLELAYOUTLog_AuditUserGUID ON dbo.DININGTABLELAYOUTLog (AuditUserGUID) ON [PRIMARY]	
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[HOSPITALITYTYPELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[HOSPITALITYTYPELog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[RESTAURANTID] [nvarchar](20) NOT NULL,
	[SEQUENCE] [int] NOT NULL,
	[GRAPHICALLAYOUT] [image] NULL,
	[DESCRIPTION] [nvarchar](50) NULL,
	[OVERVIEW] [int] NULL,
	[SALESTYPE] [nvarchar](20) NOT NULL,
	[DINEINTABLEREQUIRED] [tinyint] NULL,
	[REQUESTNOOFGUESTS] [tinyint] NULL,
	[STATIONPRINTING] [int] NULL,
	[ACCESSTOOTHERRESTAURANT] [nvarchar](20) NULL,
	[POSLOGONMENUID] [nvarchar](20) NULL,
	[ALLOWNEWENTRIES] [tinyint] NULL,
	[TIPSAMTLINE1] [nvarchar](20) NULL,
	[TIPSAMTLINE2] [nvarchar](20) NULL,
	[TIPSTOTALLINE] [nvarchar](20) NULL,
	[STAYINPOSAFTERTRANS] [tinyint] NULL,
	[TIPSINCOMEACC1] [nvarchar](20) NULL,
	[TIPSINCOMEACC2] [nvarchar](20) NULL,
	[NOOFDINEINTABLES] [int] NULL,
	[TABLEBUTTONPOSMENUID] [nvarchar](20) NULL,
	[TABLEBUTTONDESCRIPTION] [int] NULL,
	[TABLEBUTTONSTAFFDESCRIPTION] [int] NULL,
	[STAFFTAKEOVERINTRANS] [int] NULL,
	[MANAGERTAKEOVERINTRANS] [int] NULL,
	[VIEWSALESSTAFF] [tinyint] NULL,
	[VIEWTRANSDATE] [tinyint] NULL,
	[VIEWTRANSTIME] [tinyint] NULL,
	[VIEWDELIVERYADDRESS] [tinyint] NULL,
	[VIEWLISTTOTALS] [tinyint] NULL,
	[ORDERBY] [int] NULL,
	[VIEWRESTAURANT] [tinyint] NULL,
	[VIEWGRID] [tinyint] NULL,
	[VIEWCOUNTDOWN] [tinyint] NULL,
	[VIEWPROGRESSSTATUS] [tinyint] NULL,
	[DIRECTEDITOPERATION] [int] NULL,
	[SETTINGSFROMHOSPTYPE] [nvarchar](20) NULL,
	[SETTINGSFROMSEQUENCE] [int] NULL,
	[SHARINGSALESTYPEFILTER] [nvarchar](250) NULL,
	[SETTINGSFROMRESTAURANT] [nvarchar](20) NULL,
	[GUESTBUTTONS] [int] NULL,
	[MAXGUESTBUTTONSSHOWN] [int] NULL,
	[MAXGUESTSPERTABLE] [int] NULL,
	[SPLITBILLLOOKUPID] [nvarchar](20) NULL,
	[SELECTGUESTONSPLITTING] [tinyint] NULL,
	[COMBINESPLITLINESACTION] [int] NULL,
	[TRANSFERLINESLOOKUPID] [nvarchar](20) NULL,
	[PRINTTRAININGTRANSACTIONS] [tinyint] NULL,
	[LAYOUTID] [nvarchar](20) NULL,
	[TOPPOSMENUID] [nvarchar](20) NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[DININGTABLELAYOUTID] [nvarchar](20) NULL,
	[AUTOMATICJOININGCHECK] [tinyint] NULL,
	Deleted bit NULL)	

	alter table dbo.HOSPITALITYTYPELog add constraint PK_HOSPITALITYTYPELog
	primary key clustered (AuditID) on [PRIMARY]
	
	create nonclustered index IX_HOSPITALITYTYPELog_AuditUserGUID ON dbo.HOSPITALITYTYPELog (AuditUserGUID) ON [PRIMARY]	
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSLOOKUPLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSLOOKUPLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[LOOKUPID] [nvarchar](20) NOT NULL,
	[DESCRIPTION] [nvarchar](50) NULL,
	[DYNAMICMENUID] [nvarchar](20) NULL,
	[DYNAMICMENU2ID] [nvarchar](20) NULL,
	[GRID1MENUID] [nvarchar](20) NULL,
	[GRID2MENUID] [nvarchar](20) NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	Deleted bit NULL)	

	alter table dbo.POSLOOKUPLog add constraint PK_POSLOOKUPLog
	primary key clustered (AuditID) on [PRIMARY]
	
	create nonclustered index IX_POSLOOKUPLog_AuditUserGUID ON dbo.POSLOOKUPLog (AuditUserGUID) ON [PRIMARY]	
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSMENUHEADERLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSMENUHEADERLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[MENUID] [nvarchar](20) NOT NULL,
	[DESCRIPTION] [nvarchar](30) NULL,
	[COLUMNS] [int] NULL,
	[ROWS] [int] NULL,
	[MENUCOLOR] [int] NULL,
	[FONTNAME] [nvarchar](32) NULL,
	[FONTSIZE] [int] NULL,
	[FONTBOLD] [tinyint] NULL,
	[FORECOLOR] [int] NULL,
	[BACKCOLOR] [int] NULL,
	[FONTITALIC] [tinyint] NULL,
	[FONTCHARSET] [int] NULL,
	[USENAVOPERATION] [tinyint] NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[APPLIESTO] [int] NULL,
	Deleted bit NULL)	

	alter table dbo.POSMENUHEADERLog add constraint PK_POSMENUHEADERLog
	primary key clustered (AuditID) on [PRIMARY]
	
	create nonclustered index IX_POSMENUHEADERLog_AuditUserGUID ON dbo.POSMENUHEADERLog (AuditUserGUID) ON [PRIMARY]	
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSMENULINELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSMENULINELog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[MENUID] [nvarchar](20) NOT NULL,
	[SEQUENCE] [nvarchar](20) NOT NULL,
	[KEYNO] [int] NOT NULL,
	[DESCRIPTION] [nvarchar](80) NULL,
	[OPERATION] [int] NULL,
	[PARAMETER] [nvarchar](50) NULL,
	[PARAMETERTYPE] [int] NULL,
	[FONTNAME] [nvarchar](32) NULL,
	[FONTSIZE] [int] NULL,
	[FONTBOLD] [tinyint] NULL,
	[FORECOLOR] [int] NULL,
	[BACKCOLOR] [int] NULL,
	[FONTITALIC] [tinyint] NULL,
	[FONTCHARSET] [int] NULL,
	[DISABLED] [tinyint] NULL,
	[PICTURE] [image] NULL,
	[PICTUREFILE] [nvarchar](200) NULL,
	[HIDEDESCRONPICTURE] [tinyint] NULL,
	[FONTSTRIKETHROUGH] [tinyint] NULL,
	[FONTUNDERLINE] [tinyint] NULL,
	[COLUMNSPAN] [int] NULL,
	[ROWSPAN] [int] NULL,
	[NAVOPERATION] [nvarchar](20) NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[HIDDEN] [tinyint] NULL,
	[SHADEWHENDISABLED] [tinyint] NULL,
	[BACKGROUNDHIDDEN] [tinyint] NULL,
	[TRANSPARENT] [tinyint] NULL,
	[GLYPH] [int] NULL,
	[GLYPH2] [int] NULL,
	[GLYPH3] [int] NULL,
	[GLYPH4] [int] NULL,
	[GLYPHTEXT] [nvarchar](80) NULL,
	[GLYPHTEXT2] [nvarchar](80) NULL,
	[GLYPHTEXT3] [nvarchar](80) NULL,
	[GLYPHTEXT4] [nvarchar](80) NULL,
	[GLYPHTEXTFONT] [nvarchar](32) NULL,
	[GLYPHTEXT2FONT] [nvarchar](32) NULL,
	[GLYPHTEXT3FONT] [nvarchar](32) NULL,
	[GLYPHTEXT4FONT] [nvarchar](32) NULL,
	[GLYPHTEXTFONTSIZE] [int] NULL,
	[GLYPHTEXT2FONTSIZE] [int] NULL,
	[GLYPHTEXT3FONTSIZE] [int] NULL,
	[GLYPHTEXT4FONTSIZE] [int] NULL,
	[GLYPHTEXTFORECOLOR] [int] NULL,
	[GLYPHTEXT2FORECOLOR] [int] NULL,
	[GLYPHTEXT3FORECOLOR] [int] NULL,
	[GLYPHTEXT4FORECOLOR] [int] NULL,
	[GLYPHOFFSET] [int] NULL,
	[GLYPH2OFFSET] [int] NULL,
	[GLYPH3OFFSET] [int] NULL,
	[GLYPH4OFFSET] [int] NULL,
	[BACKCOLOR2] [int] NULL,
	[GRADIENTMODE] [int] NULL,
	[SHAPE] [int] NULL,
	Deleted bit NULL)	

	alter table dbo.POSMENULINELog add constraint PK_POSMENULINELog
	primary key clustered (AuditID) on [PRIMARY]
	
	create nonclustered index IX_POSMENULINELog_AuditUserGUID ON dbo.POSMENULINELog (AuditUserGUID) ON [PRIMARY]	
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RESTAURANTSTATIONLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RESTAURANTSTATIONLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[ID] [nvarchar](20) NOT NULL,
	[STATIONNAME] [nvarchar](30) NULL,
	[WINDOWSPRINTER] [nvarchar](50) NULL,
	[OUTPUTLINES] [int] NULL,
	[CHECKPRINTING] [int] NULL,
	[POSEXTERNALPRINTERID] [nvarchar](20) NULL,
	[PRINTING] [int] NULL,
	[STATIONFILTER] [nvarchar](70) NULL,
	[STATIONCHECKMINUTES] [int] NULL,
	[COMPRESSBOMRECEIPT] [int] NULL,
	[EXCLUDEFROMCOMPRESSION] [nvarchar](100) NULL,
	[STATIONTYPE] [int] NULL,
	[PRINTINGPRIORITY] [int] NULL,
	[ENDTURNSREDAFTERMIN] [int] NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	Deleted bit NULL)	

	alter table dbo.RESTAURANTSTATIONLog add constraint PK_RESTAURANTSTATIONLog
	primary key clustered (AuditID) on [PRIMARY]
	
	create nonclustered index IX_RESTAURANTSTATIONLog_AuditUserGUID ON dbo.RESTAURANTSTATIONLog (AuditUserGUID) ON [PRIMARY]	
END
GO