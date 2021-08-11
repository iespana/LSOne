
/*

	Incident No.	: 6331
	Responsible		: BjÃ¶rn EirÃ­ksson
	Sprint			: LS Retail .NET v 2010 - Sprint 3
	Date created	: 18.11.2010
	
	Description		: Add audit tables for Vendors and Contacts

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	RBOSTATEMENTTABLELog
						RBOSTATEMENTLINELog 	
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[CONTACTTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[CONTACTTABLELog](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[AuditUserGUID] [uniqueidentifier] NOT NULL,
	[AuditUserLogin] [nvarchar](32) NOT NULL,
	[AuditDate] [datetime] NOT NULL,
	[CONTACTID] [nvarchar](20) NOT NULL,
	[OWNERID] [nvarchar](20) NOT NULL,
	[OWNERTYPE] [int] NOT NULL,
	[CONTACTTYPE] [int] NOT NULL,
	[COMPANYNAME] [nvarchar](60) NOT NULL,
	[FirstName] [nvarchar](31) NOT NULL,
	[MiddleName] [nvarchar](15) NOT NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[NamePrefix] [nvarchar](8) NOT NULL,
	[NameSuffix] [nvarchar](8) NOT NULL,
	[ADDRESS] [nvarchar](250) NULL,
	[STREET] [nvarchar](250) NULL,
	[ZIPCODE] [nvarchar](10) NULL,
	[CITY] [nvarchar](60) NULL,
	[COUNTY] [nvarchar](10) NULL,
	[STATE] [nvarchar](10) NULL,
	[COUNTRY] [nvarchar](10) NULL,
	[PHONE] [nvarchar](20) NULL,
	[PHONE2] [nvarchar](20) NULL,
	[FAX] [nvarchar](20) NULL,
	[EMAIL] [nvarchar](80) NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[Deleted] [bit] NULL)
	
	alter table dbo.CONTACTTABLELog add constraint PK_CONTACTTABLELog
	primary key clustered (AuditID) on [PRIMARY]

	create nonclustered index IX_CONTACTTABLELog_AuditUserGUID ON dbo.CONTACTTABLELog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[VENDTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[VENDTABLELog](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[AuditUserGUID] [uniqueidentifier] NOT NULL,
	[AuditUserLogin] [nvarchar](32) NOT NULL,
	[AuditDate] [datetime] NOT NULL,
	[ACCOUNTNUM] [nvarchar](20) NOT NULL,
	[NAME] [nvarchar](60) NOT NULL,
	[ADDRESS] [nvarchar](250) NULL,
	[STREET] [nvarchar](250) NULL,
	[ZIPCODE] [nvarchar](10) NULL,
	[CITY] [nvarchar](60) NULL,
	[COUNTY] [nvarchar](10) NULL,
	[STATE] [nvarchar](10) NULL,
	[COUNTRY] [nvarchar](10) NULL,
	[PHONE] [nvarchar](20) NULL,
	[EMAIL] [nvarchar](80) NULL,
	[FAX] [nvarchar](20) NULL,
	[DEFAULTCONTACTID] [nvarchar](20) NULL,
	[LANGUAGEID] [nvarchar](7) NULL,
	[CURRENCY] [nvarchar](3) NOT NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[Deleted] [bit] NULL)
	
	alter table dbo.VENDTABLELog add constraint PK_VENDTABLELog
	primary key clustered (AuditID) on [PRIMARY]

	create nonclustered index IX_VENDTABLELog_AuditUserGUID ON dbo.VENDTABLELog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[INVENTJOURNALTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[INVENTJOURNALTABLELog](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[AuditUserGUID] [uniqueidentifier] NOT NULL,
	[AuditUserLogin] [nvarchar](32) NOT NULL,
	[AuditDate] [datetime] NOT NULL,

	[JOURNALID] [nvarchar](20) NOT NULL,
	[DESCRIPTION] [nvarchar](60) NOT NULL,
	[POSTED] [int] NOT NULL,
	[POSTEDDATETIME] [datetime] NOT NULL,
	[JOURNALTYPE] [int] NOT NULL,
	[JOURNALNAMEID] [nvarchar](10) NOT NULL,
	[EMPLID] [uniqueidentifier] NOT NULL,
	[POSTEDUSERID] [nvarchar](5) NOT NULL,
	[NUMOFLINES] [int] NOT NULL,
	[DELETEPOSTEDLINES] [int] NOT NULL,
	[CREATEDDATETIME] [datetime] NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[Deleted] [bit] NULL)
	
	alter table dbo.INVENTJOURNALTABLELog add constraint PK_INVENTJOURNALTABLELog
	primary key clustered (AuditID) on [PRIMARY]

	create nonclustered index IX_INVENTJOURNALTABLELog_AuditUserGUID ON dbo.INVENTJOURNALTABLELog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[INVENTTRANSREASONLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[INVENTTRANSREASONLog](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[AuditUserGUID] [uniqueidentifier] NOT NULL,
	[AuditUserLogin] [nvarchar](32) NOT NULL,
	[AuditDate] [datetime] NOT NULL,
	[REASONID] [nvarchar](20) NOT NULL,
	[REASONTEXT] [nvarchar](60) NOT NULL,
	[DATAAREAID] [nvarchar](4) NOT NULL,
	[Deleted] [bit] NULL)
	
	alter table dbo.INVENTTRANSREASONLog add constraint PK_INVENTTRANSREASONLog
	primary key clustered (AuditID) on [PRIMARY]

	create nonclustered index IX_INVENTTRANSREASONLog_AuditUserGUID ON dbo.INVENTTRANSREASONLog (AuditUserGUID) ON [PRIMARY]
END
GO