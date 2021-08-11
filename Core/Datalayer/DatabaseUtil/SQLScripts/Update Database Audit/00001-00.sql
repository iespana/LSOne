
/*

	Incident No.	: 6050
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET v 2010 - Sprint 2
	Date created	: 22.10.2010
	
	Description		: Added log table for SALESTYPE to the audit database

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	SALESTYPELog - Table added
						
*/


USE LSPOSNET_Audit

GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[SALESTYPELog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].SALESTYPELog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[CODE] [nvarchar](20) NOT NULL,
		[DESCRIPTION] [nvarchar](30) NULL,
		[REQUESTSALESPERSON] [tinyint] NULL,
		[REQUESTDEPOSITPERC] [int] NULL,
		[REQUESTCHARGEACCOUNT] [tinyint] NULL,
		[PURCHASINGCODE] [nvarchar](10) NULL,
		[DEFAULTORDERLIMIT] [numeric](28, 12) NULL,
		[LIMITSETTING] [int] NULL,
		[REQUESTCONFIRMATION] [tinyint] NULL,
		[REQUESTDESCRIPTION] [tinyint] NULL,
		[NEWGLOBALDIMENSION2] [nvarchar](20) NULL,
		[SUSPENDPRINTING] [int] NULL,
		[SUSPENDTYPE] [int] NULL,
		[PREPAYMENTACCOUNTNO] [nvarchar](20) NULL,
		[MINIMUMDEPOSIT] [numeric](28, 12) NULL,
		[PRINTITEMLINESONPOSSLIP] [tinyint] NULL,
		[VOIDEDPREPAYMENTACCOUNTNO] [nvarchar](20) NULL,
		[DAYSOPENTRANSEXIST] [int] NULL,
		[TAXGROUPID] [nvarchar](10) NULL,
		[PRICEGROUP] [nvarchar](10) NULL,
		[TRANSDELETEREMINDER] [int] NULL,
		[LOCATIONCODE] [nvarchar](10) NULL,
		[PAYMENTISPREPAYMENT] [tinyint] NULL,
		[CALCPRICEFROMVATPRICE] [tinyint] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		Deleted bit NULL)


		alter table dbo.SALESTYPELog add constraint PK_SALESTYPELog
		primary key clustered (AuditID) on [PRIMARY]
		

		create nonclustered index IX_SALESTYPELog_AuditUserGUID  
		on dbo.SALESTYPELog (AuditUserGUID) on [PRIMARY]		
END
GO
