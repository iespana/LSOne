/*

	Incident No.	: 
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2013\Merkúr
	Date created	: 25.07.2012

	Description		: Add KMINTERFACEPROFILE table
	
	
	Tables affected	: KMINTERFACEPROFILE
						
*/


Use LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KMINTERFACEPROFILE]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[KMINTERFACEPROFILE](
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
	[OrderStyleId] [nvarchar](20) NULL,
	[OrderPaneStyleId] [nvarchar](20) NULL,
	[ButtonPaneStyleId] [nvarchar](20) NULL,
	[ZoomPaneStyleId] [nvarchar](20) NULL,
	[ItemQueuePaneStyleId] [nvarchar](20) NULL
	)

	Alter table [KMINTERFACEPROFILE] Add CONSTRAINT  PK_KMINTERFACEPROFILE Primary Key (ID,DATAAREAID)
END
GO