
/*

	Incident No.	: []
	Responsible		: Guðbjörn Einarsson
	Sprint			: DotNetPM\LS POS 2010.1\Dot Net Stream\Sprint 03\SC Team
	Date created	: 07.12.2010

	Description		: Adding Goods receving tables

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	:	GoodsReceiving		- table created
						GoodsReceivingLine	- table created
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[GOODSRECEIVING]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].GOODSRECEIVING(
	GOODSRECEIVINGID nvarchar(20) NOT NULL,
	PURCHASEORDERID nvarchar(20) NOT NULL,
	STATUS int NOT NULL,
	DATAAREAID nvarchar(4) NOT NULL
	)
	
	alter table dbo.GOODSRECEIVING add constraint PK_GOODSRECEIVING
	primary key clustered (GOODSRECEIVINGID,DATAAREAID) on [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[GOODSRECEIVINGLINE]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].GOODSRECEIVINGLINE(
	GOODSRECEIVINGID nvarchar(20) NOT NULL,
	PURCHASEORDERLINENUMBER nvarchar(20) NOT NULL,
	LINENUMBER nvarchar(20) NOT NULL,
	STOREID nvarchar(20) NOT NULL,
	RECEIVEDQUANTITY decimal(24,6) NOT NULL,
	RECEIVEDDATE datetime NOT NULL,
	POSTED tinyint NOT NULL,
	DATAAREAID nvarchar(4) NOT NULL
	)
	
	alter table dbo.GOODSRECEIVINGLINE add constraint PK_GOODSRECEIVINGLINE
	primary key clustered (GOODSRECEIVINGID,PURCHASEORDERLINENUMBER,LINENUMBER,DATAAREAID) on [PRIMARY]
END
GO


