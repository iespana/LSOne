
/*

	Incident No.	: 6394
	Responsible		: Hörður Kristjánsson
	Sprint			: LS POS 2010.1 Sprint 2
	Date created	: 03.11.2010

	Description		: Adding the HOSPITALITYTYPE table for the Hospitality plugin for SC

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: 	HospitalityType			- table added
*/


Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[HOSPITALITYTYPE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[HOSPITALITYTYPE](
		[RESTAURANTID] [nchar](10) NOT NULL,
		[SEQUENCE] [int] NOT NULL,
		[GRAPHICALLAYOUT] [image] NULL,
		[DESCRIPTION] [nvarchar](50) NULL,
		[OVERVIEW] [int] NULL,
		[SALESTYPE] [nvarchar](20) NOT NULL,
		[DINEINTABLEREQUIRED] [tinyint] NULL,
		[STARTINGTABLENO] [int] NULL,
		[REQUESTNOOFGUESTS] [tinyint] NULL,
		[STATIONPRINTING] [int] NULL,
		[DINEINTABLEROWS] [int] NULL,
		[DINEINTABLECOLUMNS] [int] NULL,
		[ACCESSTOOTHERRESTAURANT] [nvarchar](10) NULL,
		[POSLOGONMENUID] [nvarchar](10) NULL,
		[ALLOWNEWENTRIES] [tinyint] NULL,
		[TIPSAMTLINE1] [nvarchar](20) NULL,
		[TIPSAMTLINE2] [nvarchar](20) NULL,
		[TIPSTOTALLINE] [nvarchar](20) NULL,
		[STAYINPOSAFTERTRANS] [tinyint] NULL,
		[TIPSINCOMEACC1] [nvarchar](20) NULL,
		[TIPSINCOMEACC2] [nvarchar](20) NULL,
		[NOOFDINEINTABLES] [int] NULL,
		[TABLEBUTTONPOSMENUID] [nvarchar](10) NULL,
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
		[SETTINGSFROMRESTAURANT] [nvarchar](10) NULL,
		[GUESTBUTTONS] [int] NULL,
		[MAXGUESTBUTTONSSHOWN] [int] NULL,
		[MAXGUESTSPERTABLE] [int] NULL,
		[SPLITBILLLOOKUPID] [nvarchar](10) NULL,
		[SELECTGUESTONSPLITTING] [tinyint] NULL,
		[COMBINESPLITLINESACTION] [int] NULL,
		[TRANSFERLINESLOOKUPID] [nvarchar](10) NULL,
		[PRINTTRAININGTRANSACTIONS] [tinyint] NULL,
		[VISUALPROFILEID] [nvarchar](10) NULL,
		[TOPPOSMENUID] [nvarchar](10) NULL,
		[SPLITMAINFUNCPOSMENUID] [nvarchar](10) NULL,
		[SPLITGRID1BOTTPOSMENUID] [nvarchar](10) NULL,
		[SPLITGRID1RIGHTPOSMENUID] [nvarchar](10) NULL,
		[SPLITGRID2RIGHTPOSMENUID] [nvarchar](10) NULL,
		[SPLITCAPTIONPOSMENUID] [nvarchar](10) NULL,
		[SPLITINFOPOSMENUID] [nvarchar](10) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
	 CONSTRAINT [PK_HOSPITALITYTYPE] PRIMARY KEY CLUSTERED 
	(
		[RESTAURANTID] ASC,
		[SEQUENCE] ASC,
		[SALESTYPE] ASC,
		[DATAAREAID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO