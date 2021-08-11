/*

	Incident No.	: 
	Responsible		: Hörður Kristjánsson
	Sprint			: Kuala Lumpur

	Description		: Adding new retail item tables
	
						
*/

/*
	
	NB!!

	**** When a script is added to any folder the "Build Action" on the script needs to be set to "Embedded resource" in properties (F4) ****

	If this is not done the DatabaseUtility will not find the script and will therefore not run it

*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DIMENSIONATTRIBUTELog')
BEGIN
CREATE TABLE [dbo].[DIMENSIONATTRIBUTELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    [DIMENSIONID] [uniqueidentifier] NOT NULL,
    [DESCRIPTION] [nvarchar] (30) NULL,
    [CODE] [nvarchar] (20) NULL,
    [SEQUENCE] [int] NULL,
    Deleted bit NULL)

    alter table dbo.DIMENSIONATTRIBUTELog add constraint PK_DIMENSIONATTRIBUTELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_DIMENSIONATTRIBUTELog_AuditUserGUID ON dbo.DIMENSIONATTRIBUTELog (AuditUserGUID) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'DIMENSIONTEMPLATELog')
BEGIN
CREATE TABLE [dbo].[DIMENSIONTEMPLATELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    [DESCRIPTION] [nvarchar] (30) NOT NULL,
    Deleted bit NULL)

    alter table dbo.DIMENSIONTEMPLATELog add constraint PK_DIMENSIONTEMPLATELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_DIMENSIONTEMPLATELog_AuditUserGUID ON dbo.DIMENSIONTEMPLATELog (AuditUserGUID) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILITEMLog')
BEGIN
CREATE TABLE [dbo].[RETAILITEMLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
	[MASTERID] [uniqueidentifier] NOT NULL,
	[ITEMID] [nvarchar](20) NOT NULL,
	[HEADERITEMID] [uniqueidentifier] NULL,
	[ITEMNAME] [nvarchar](60) NOT NULL,
	[VARIANTNAME] [nvarchar](60) NOT NULL,
	[ITEMTYPE] [tinyint] NOT NULL,
	[DEFAULTVENDORID] [nvarchar](20) NOT NULL,
	[NAMEALIAS] [nvarchar](60) NOT NULL,
	[EXTENDEDDESCRIPTION] [nvarchar](max) NULL,
	[RETAILGROUPMASTERID] [uniqueidentifier]  NULL,
	[RETAILDEPARTMENTMASTERID] [uniqueidentifier] NULL,
	[RETAILDIVISIONMASTERID] [uniqueidentifier] NULL,
	[ZEROPRICEVALID] [bit] NOT NULL,
	[QTYBECOMESNEGATIVE] [bit] NOT NULL,
	[NODISCOUNTALLOWED] [bit] NOT NULL,
	[KEYINPRICE] [tinyint] NOT NULL,
	[SCALEITEM] [bit] NOT NULL,
	[KEYINQTY] [tinyint] NOT NULL, 
	[BLOCKEDONPOS] [bit] NOT NULL,
	[BARCODESETUPID] [nvarchar](20) NOT NULL,
	[PRINTVARIANTSSHELFLABELS] [bit] NOT NULL,
	[FUELITEM] [bit] NOT NULL,
	[GRADEID] [nvarchar](20) NOT NULL,
	[MUSTKEYINCOMMENT] [bit] NOT NULL,
	[DATETOBEBLOCKED] [datetime] NOT NULL,
	[DATETOACTIVATEITEM] [datetime] NOT NULL,
	[PROFITMARGIN] [numeric](28, 12) NOT NULL,
	[VALIDATIONPERIODID] [nvarchar](20) NOT NULL,
	[MUSTSELECTUOM] [bit] NOT NULL,
	[INVENTORYUNITID] [nvarchar](20) NOT NULL,
	[PURCHASEUNITID] [nvarchar](20) NOT NULL,
	[SALESUNITID] [nvarchar](20) NOT NULL,
	[PURCHASEPRICE] [numeric](28, 12) NOT NULL,
	[PURCHASEPRICEINCLTAX] [numeric](28, 12) NOT NULL,
	[PURCHASEPRICEUNIT] [numeric](28, 12) NOT NULL,
	[PURCHASEMARKUP] [numeric](28, 12) NOT NULL,
	[SALESPRICE] [numeric](28, 12) NOT NULL,
	[SALESPRICEINCLTAX] [numeric](28, 12) NOT NULL,
	[SALESPRICEUNIT] [numeric](28, 12) NOT NULL,
	[SALESMARKUP] [numeric](28, 12) NOT NULL,
	[SALESLINEDISC] [nvarchar](20) NOT NULL,
	[SALESMULTILINEDISC] [nvarchar](20) NOT NULL,
	[SALESALLOWTOTALDISCOUNT] [bit] NOT NULL,
	[SALESTAXITEMGROUPID] [nvarchar](20) NOT NULL,
	[DELETED] [bit] NOT NULL)

    alter table dbo.RETAILITEMLog add constraint PK_RETAILITEMLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RETAILITEMLog_AuditUserGUID ON dbo.RETAILITEMLog (AuditUserGUID) ON [PRIMARY]
END
GO



if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILITEMDIMENSIONLog')
begin
	CREATE TABLE [dbo].[RETAILITEMDIMENSIONLog](
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[ID] [uniqueidentifier] NOT NULL,
		[RETAILITEMID] [uniqueidentifier] NOT NULL,
		[DESCRIPTION] [nvarchar](30) NOT NULL,
		[SEQUENCE] [int] NOT NULL,
		[DELETED] [bit] NOT NULL)
		 alter table dbo.[RETAILITEMDIMENSIONLog] add constraint PK_RETAILITEMDIMENSIONLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RETAILITEMDIMENSIONLog_AuditUserGUID ON dbo.RETAILITEMDIMENSIONLog (AuditUserGUID) ON [PRIMARY]
end

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILITEMDIMENSIONATTRIBUTELog')
begin
	CREATE TABLE [dbo].[RETAILITEMDIMENSIONATTRIBUTELog](
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[RETAILITEMID] [uniqueidentifier] NOT NULL,
		[DIMENSIONATTRIBUTEID] [uniqueidentifier] NOT NULL,
		[DELETED] [bit] NOT NULL
	) 
	 alter table dbo.[RETAILITEMDIMENSIONATTRIBUTELog] add constraint PK_RETAILITEMDIMENSIONATTRIBUTELog
    primary key clustered (AuditID) on [PRIMARY]
	
    create nonclustered index IX_RETAILITEMDIMENSIONATTRIBUTELog_AuditUserGUID ON dbo.RETAILITEMDIMENSIONATTRIBUTELog (AuditUserGUID) ON [PRIMARY]
end
GO
if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'PERIODICDISCOUNTLog')
begin
CREATE TABLE [dbo].[PERIODICDISCOUNTLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[MASTERID] [uniqueidentifier] NOT NULL ,
	[OFFERID] [nvarchar](20) NOT NULL,
	[DESCRIPTION] [nvarchar](60) NOT NULL,
	[STATUS] [tinyint] NULL,
	[PDTYPE] [int] NULL,
	[PRIORITY] [int] NULL,
	[DISCVALIDPERIODID] [nvarchar](20) NULL,
	[DISCOUNTTYPE] [int] NULL,
	[NOOFLINESTOTRIGGER] [int] NULL,
	[DEALPRICEVALUE] [numeric](28, 12) NULL ,
	[DISCOUNTPCTVALUE] [numeric](28, 12) NULL ,
	[DISCOUNTAMOUNTVALUE] [numeric](28, 12) NULL ,
	[NOOFLEASTEXPITEMS] [int] NULL,
	[PRICEGROUP] [nvarchar](20) NULL,
	[ACCOUNTCODE] [int] NULL,
	[ACCOUNTRELATION] [nvarchar](20) NULL,
	[TRIGGERED] [int] NULL,
		[DELETED] [bit] NOT NULL) 
 alter table dbo.[PERIODICDISCOUNTLog] add constraint PK_PERIODICDISCOUNTLog
    primary key clustered (AuditID) on [PRIMARY]
	
    create nonclustered index IX_PERIODICDISCOUNTLog_AuditUserGUID ON dbo.PERIODICDISCOUNTLog (AuditUserGUID) ON [PRIMARY]
end

GO

GO
if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'PERIODICDISCOUNTLINELog')
begin
CREATE TABLE [dbo].[PERIODICDISCOUNTLINELog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[OFFERID] [nvarchar](20) NOT NULL,
	[OFFERMASTERID] [uniqueidentifier] NOT NULL,
	[LINEID] [int] NOT NULL,
	[PRODUCTTYPE] [int] NULL,
	[TARGETID] [nvarchar](30) NOT NULL,
	[TARGETMASTERID] [uniqueidentifier] NOT NULL,
	[DEALPRICEORDISCPCT] [numeric](28, 12) NULL ,
	[LINEGROUP] [nvarchar](60) NOT NULL,
	[DISCTYPE] [int] NULL ,
	[POSPERIODICDISCOUNTLINEGUID] [uniqueidentifier] NOT NULL,
	[DISCPCT] [numeric](28, 12) NULL DEFAULT ((0)),
	[DISCAMOUNT] [numeric](28, 12) NULL DEFAULT ((0)),
	[DISCAMOUNTINCLTAX] [numeric](28, 12) NULL DEFAULT ((0)),
	[OFFERPRICE] [numeric](28, 12) NULL DEFAULT ((0)),
	[OFFERPRICEINCLTAX] [numeric](28, 12) NULL DEFAULT ((0)),
		[DELETED] [bit] NOT NULL ) 
 alter table dbo.[PERIODICDISCOUNTLINELog] add constraint PK_PERIODICDISCOUNTLINELog
    primary key clustered (AuditID) on [PRIMARY]
	
    create nonclustered index IX_PERIODICDISCOUNTLINELog_AuditUserGUID ON dbo.PERIODICDISCOUNTLINELog (AuditUserGUID) ON [PRIMARY]
end


GO

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILDIVISIONLog')
begin
CREATE TABLE [dbo].[RETAILDIVISIONLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[DIVISIONID] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NULL,
	[MASTERID] [uniqueidentifier] NOT NULL ,
		[DELETED] [bit] NOT NULL)

	 alter table dbo.RETAILDIVISIONLog add constraint PK_RETAILDIVISIONLog
    primary key clustered (AuditID) on [PRIMARY]
	
    create nonclustered index IX_RETAILDIVISIONLog_AuditUserGUID ON dbo.RETAILDIVISIONLog (AuditUserGUID) ON [PRIMARY]
end

GO

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILDEPARTMENTLog')
begin
CREATE TABLE [dbo].[RETAILDEPARTMENTLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[DEPARTMENTID] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NULL ,
	[NAMEALIAS] [nvarchar](20) NULL ,
	[DIVISIONID] [nvarchar](20) NULL,
	[DIVISIONMASTERID] [uniqueidentifier] NULL,
	[MASTERID] [uniqueidentifier] NOT NULL ,
		[DELETED] [bit] NOT NULL)
 alter table dbo.RETAILDEPARTMENTLog add constraint PK_RETAILDEPARTMENTLog
    primary key clustered (AuditID) on [PRIMARY]
	
    create nonclustered index IX_RETAILDEPARTMENTLog_AuditUserGUID ON dbo.RETAILDEPARTMENTLog (AuditUserGUID) ON [PRIMARY]
end

GO

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'RETAILGROUPLog')
begin
CREATE TABLE [dbo].[RETAILGROUPLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[MASTERID] [uniqueidentifier] NOT NULL ,
	[GROUPID] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NULL,
	[NAMEALIAS] [nvarchar](20) NULL,
	[DEPARTMENTID] [nvarchar](20) NULL,
	[SIZEGROUPID] [nvarchar](20) NULL,
	[COLORGROUPID] [nvarchar](20) NULL,
	[STYLEGROUPID] [nvarchar](20) NULL,
	[FSHREPLENISHMENTRULEID] [nvarchar](20) NULL,
	[ITEMGROUPID] [nvarchar](20) NULL,
	[INVENTDIMGROUPID] [nvarchar](20) NULL,
	[SALESTAXITEMGROUP] [nvarchar](20) NULL,
	[DEFAULTPROFIT] [numeric](28, 12) NULL,
	[POSPERIODICID] [nvarchar](20) NULL,
	[DIVISIONMASTERID] [uniqueidentifier] NULL,
	[DEPARTMENTMASTERID] [uniqueidentifier] NULL,
		[DELETED] [bit] NOT NULL)
alter table dbo.RETAILGROUPLog add constraint PK_RETAILGROUPLog
    primary key clustered (AuditID) on [PRIMARY]
	
    create nonclustered index IX_RETAILGROUPLog_AuditUserGUID ON dbo.RETAILGROUPLog (AuditUserGUID) ON [PRIMARY]
end

GO

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SPECIALGROUPLog')
begin
CREATE TABLE [dbo].[SPECIALGROUPLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[GROUPID] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NULL,
	[MASTERID] [uniqueidentifier] NOT NULL ,
		[DELETED] [bit] NOT NULL)
alter table dbo.SPECIALGROUPLog add constraint PK_SPECIALGROUPLog
    primary key clustered (AuditID) on [PRIMARY]
	
    create nonclustered index IX_SPECIALGROUPLog_AuditUserGUID ON dbo.SPECIALGROUPLog (AuditUserGUID) ON [PRIMARY]
end

GO

if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'SPECIALGROUPITEMSLog')
begin
CREATE TABLE [dbo].[SPECIALGROUPITEMSLog](
	AuditID int NOT NULL IDENTITY (1, 1),
	AuditUserGUID uniqueidentifier NOT NULL,
	AuditUserLogin nvarchar(32) NOT NULL,
	AuditDate datetime NOT NULL,
	[GROUPID] [nvarchar](20) NOT NULL,
	[ITEMID] [nvarchar](20) NOT NULL,
	[MEMBERMASTERID] [uniqueidentifier] NOT NULL,
	[GROUPMASTERID] [uniqueidentifier] NOT NULL,
		[DELETED] [bit] NOT NULL)
alter table dbo.SPECIALGROUPITEMSLog add constraint PK_SPECIALGROUPITEMSLog
    primary key clustered (AuditID) on [PRIMARY]
	
    create nonclustered index IX_SPECIALGROUPITEMSLog_AuditUserGUID ON dbo.SPECIALGROUPITEMSLog (AuditUserGUID) ON [PRIMARY]
end

GO
