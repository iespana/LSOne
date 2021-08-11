
/*

	Incident No.	: 6050
	Responsible		: BjÃ¶rn EirÃ­ksson
	Sprint			: LS Retail .NET v 2010 - Sprint 3
	Date created	: 25.10.2010
	
	Description		: Added log table for VENDORITEMS to the audit database

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	VENDORITEMSLog - Table added
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[VENDORITEMSLog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].VENDORITEMSLog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[INTERNALID] nvarchar(20) NOT NULL,
		[VENDORITEMID] nvarchar(30) NOT NULL,
		[RETAILITEMID] nvarchar(20) NOT NULL,
		[VARIANTID] nvarchar(20) NOT NULL,
		[UNITID] nvarchar(20) NOT NULL,
		[VENDORID] nvarchar(20) NOT NULL,
		DATAAREAID nvarchar(4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.VENDORITEMSLog add constraint PK_VENDORITEMSLog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_VENDORITEMSLog_AuditUserGUID  
		on dbo.VENDORITEMSLog (AuditUserGUID) on [PRIMARY]		
END
GO
