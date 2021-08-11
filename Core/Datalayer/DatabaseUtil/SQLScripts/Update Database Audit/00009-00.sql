
/*

	Incident No.	: 
	Responsible		: GuÃ°bjÃ¶rn Einarsson
	Sprint			: LS Retail .NET v 2010 - Sprint 3
	Date created	:16.12.2010
	
	Description		: Add log tables for the following tables
						- INVENTJOURNALTRANS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- INVENTJOURNALTRANSLog - Created
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[INVENTJOURNALTRANSLog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].INVENTJOURNALTRANSLog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[JOURNALID] [nvarchar](20) NOT NULL,
		[LINENUM] [nvarchar](20) NOT NULL,
		[ORIGIN] [nvarchar](20) NOT NULL,
		[TRANSDATE] [datetime] NULL,
		[VOUCHER] [nvarchar](20) NULL,
		[JOURNALTYPE] [int] NULL,
		[ITEMID] [nvarchar](20) NULL,
		[ADJUSTMENT] [numeric](28, 12) NULL,
		[COSTPRICE] [numeric](28, 12) NULL,
		[PRICEUNIT] [numeric](28, 12) NULL,
		[COSTMARKUP] [numeric](28, 12) NULL,
		[COSTAMOUNT] [numeric](28, 12) NULL,
		[SALESAMOUNT] [numeric](28, 12) NULL,
		[INVENTONHAND] [numeric](28, 12) NULL,
		[COUNTED] [numeric](28, 12) NULL,
		[DIMENSION] [nvarchar](10) NULL,
		[DIMENSION2_] [nvarchar](10) NULL,
		[DIMENSION3_] [nvarchar](10) NULL,
		[REASONREFRECID] [nvarchar](20) NULL,
		[VARIANTID] [nvarchar](20) NULL,
		[POSTED] [int] NULL,
		[POSTEDDATETIME] [datetime] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.INVENTJOURNALTRANSLog add constraint PK_INVENTJOURNALTRANSLog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_INVENTJOURNALTRANSLog_AuditUserGUID  
		on dbo.INVENTJOURNALTRANSLog (AuditUserGUID) on [PRIMARY]		
END
GO
