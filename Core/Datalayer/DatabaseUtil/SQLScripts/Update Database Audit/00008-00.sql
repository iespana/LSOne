
/*

	Incident No.	: 
	Responsible		: GuÃ°bjÃ¶rn Einarsson
	Sprint			: LS Retail .NET v 2010 - Sprint 3
	Date created	:13.12.2010
	
	Description		: Add log tables for the following tables
						- GOODSRECEIVING
						- GOODSRECEIVINGLINE
						- PURCHASEORDERS
						- PURCHASEORDERLINE
						- RBOSTATEMENTTABLE
						- RBOSTATEMENTLINE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- GOODSRECEIVINGLog
						- GOODSRECEIVINGLINELog
						- PURCHASEORDERSLog
						- PURCHASEORDERLINELog
						- RBOSTATEMENTTABLELog
						- RBOSTATEMENTLINELog
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[GOODSRECEIVINGLog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].GOODSRECEIVINGLog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[GOODSRECEIVINGID] [nvarchar](20) NOT NULL,
		[PURCHASEORDERID] [nvarchar](20) NOT NULL,
		[STATUS] [int] NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.GOODSRECEIVINGLog add constraint PK_GOODSRECEIVINGLog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_GOODSRECEIVINGLog_AuditUserGUID  
		on dbo.GOODSRECEIVINGLog (AuditUserGUID) on [PRIMARY]		
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[GOODSRECEIVINGLINELog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].GOODSRECEIVINGLINELog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[GOODSRECEIVINGID] [nvarchar](20) NOT NULL,
		[PURCHASEORDERLINENUMBER] [nvarchar](20) NOT NULL,
		[LINENUMBER] [nvarchar](20) NOT NULL,
		[RECEIVEDQUANTITY] [decimal](24, 6) NOT NULL,
		[RECEIVEDDATE] [datetime] NOT NULL,
		[POSTED] [tinyint] NOT NULL,
		[STOREID] [nvarchar](20) NULL,
		DATAAREAID nvarchar(4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.GOODSRECEIVINGLINELog add constraint PK_GOODSRECEIVINGLINELog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_GOODSRECEIVINGLINELog_AuditUserGUID  
		on dbo.GOODSRECEIVINGLINELog (AuditUserGUID) on [PRIMARY]		
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[PURCHASEORDERSLog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].PURCHASEORDERSLog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[PURCHASEORDERID] [nvarchar](20) NOT NULL,
		[VENDORID] [nvarchar](20) NOT NULL,
		[CONTACTID] [nvarchar](20) NULL,
		[PURCHASESTATUS] [int] NOT NULL,
		[DELIVERYDATE] [datetime] NOT NULL,
		[CURRENCYCODE] [nvarchar](20) NOT NULL,
		[CREATEDDATE] [datetime] NOT NULL,
		[ORDERER] [nvarchar](20) NOT NULL,
		[DELIVERYNAME] [nvarchar](250) NOT NULL,
		[DELIVERYADDRESS] [nvarchar](250) NOT NULL,
		[DELIVERYSTREET] [nvarchar](250) NOT NULL,
		[DELIVERYZIPCODE] [nvarchar](250) NOT NULL,
		[DELIVERYCITY] [nvarchar](250) NOT NULL,
		[DELIVERYCOUNTY] [nvarchar](250) NOT NULL,
		[DELIVERYSTATE] [nvarchar](250) NOT NULL,
		[DELIVERYCOUNTRY] [nvarchar](250) NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.PURCHASEORDERSLog add constraint PK_PURCHASEORDERSLog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_PURCHASEORDERSLog_AuditUserGUID  
		on dbo.PURCHASEORDERSLog (AuditUserGUID) on [PRIMARY]		
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[PURCHASEORDERLINELog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].PURCHASEORDERLINELog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[PURCHASEORDERID] [nvarchar](20) NOT NULL,
		[LINENUMBER] [nvarchar](20) NOT NULL,
		[RETAILITEMID] [nvarchar](20) NOT NULL,
		[VENDORITEMID] [nvarchar](40) NOT NULL,
		[VARIANTID] [nvarchar](20) NULL,
		[UNITID] [nvarchar](20) NOT NULL,
		[QUANTITY] [decimal](24, 6) NOT NULL,
		[PRICE] [decimal](24, 6) NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.PURCHASEORDERLINELog add constraint PK_PURCHASEORDERLINELog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_PURCHASEORDERLINELog_AuditUserGUID  
		on dbo.PURCHASEORDERLINELog (AuditUserGUID) on [PRIMARY]		
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSTATEMENTTABLELog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].RBOSTATEMENTTABLELog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[STATEMENTID] [nvarchar](20) NOT NULL,
		[STOREID] [nvarchar](60) NOT NULL,
		[CALCULATEDTIME] [datetime] NULL,
		[POSTINGDATE] [date] NOT NULL,
		[PERIODSTARTINGTIME] [datetime] NOT NULL,
		[PERIODENDINGTIME] [datetime] NOT NULL,
		[POSTED] [tinyint] NOT NULL,
		[CALCULATED] [tinyint] NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.RBOSTATEMENTTABLELog add constraint PK_RBOSTATEMENTTABLELog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_RBOSTATEMENTTABLELog_AuditUserGUID  
		on dbo.RBOSTATEMENTTABLELog (AuditUserGUID) on [PRIMARY]		
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSTATEMENTLINELog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].RBOSTATEMENTLINELog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[STATEMENTID] [nvarchar](20) NOT NULL,
		[LINENUMBER] [nvarchar](20) NOT NULL,
		[STAFFID] [nvarchar](10) NULL,
		[TERMINALID] [nvarchar](10) NULL,
		[CURRENCYCODE] [nvarchar](10) NOT NULL,
		[TENDERID] [nvarchar](60) NOT NULL,
		[TRANSACTIONAMOUNT] [decimal](24, 6) NOT NULL,
		[BANKEDAMOUNT] [decimal](24, 6) NOT NULL,
		[SAFEAMOUNT] [decimal](24, 6) NOT NULL,
		[COUNTEDAMOUNT] [decimal](24, 6) NOT NULL,
		[DIFFERENCE] [decimal](24, 6) NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.RBOSTATEMENTLINELog add constraint PK_RBOSTATEMENTLINELog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_RBOSTATEMENTLINELog_AuditUserGUID  
		on dbo.RBOSTATEMENTLINELog (AuditUserGUID) on [PRIMARY]		
END
GO