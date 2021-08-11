
/*

	Incident No.	: 
	Responsible		: Guðbjörn Einarsson
	Sprint			: 2012 - Sprint 1
	Date created	: 29.06.2011
	
	Description		: Add log tables for the following tables
						- RBOINVENTLINKEDITEM

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- RBOINVENTLINKEDITEMLog - Created
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOINVENTLINKEDITEMLog]') AND TYPE IN ('U'))
BEGIN
	create table [dbo].RBOINVENTLINKEDITEMLog(
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[ITEMID] [nvarchar](20) NOT NULL,
		[UNIT] [nvarchar](20) NOT NULL,
		[LINKEDITEMID] [nvarchar](20) NOT NULL,
		[QTY] [numeric](28, 12) NULL,
		[BLOCKED] [tinyint] NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		Deleted bit NULL)

		alter table dbo.RBOINVENTLINKEDITEMLog add constraint PK_RBOINVENTLINKEDITEMLog
		primary key clustered (AuditID) on [PRIMARY]
		
		create nonclustered index IX_RBOINVENTLINKEDITEMLog_AuditUserGUID  
		on dbo.RBOINVENTLINKEDITEMLog (AuditUserGUID) on [PRIMARY]		
END
GO
