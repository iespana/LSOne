/*

	Incident No.	: 8842
	Responsible		: Björn Eiríksson
	Sprint			: LS Retail .NET v 2011sp1 - Sprint 1
	Date created	: 28.2.2011
	
	Description		: Add audit tables for a few hospitality tables

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	RBOCUSTTABLELog	- Table added

							
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOCUSTTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOCUSTTABLELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ACCOUNTNUM] [nvarchar] (20) NOT NULL,
    [OTHERTENDERINFINALIZING] [tinyint] NULL,
    [POSTASSHIPMENT] [tinyint] NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    [USEORDERNUMBERREFERENCE] [tinyint] NULL,
    [RECEIPTOPTION] [int] NULL,
    [RECEIPTEMAIL] [nvarchar] (80) NULL,
    [NONCHARGABLEACCOUNT] [tinyint] NULL,
    [REQUIRESAPPROVAL] [tinyint] NULL,
    Deleted bit NULL)

    alter table dbo.RBOCUSTTABLELog add constraint PK_RBOCUSTTABLELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOCUSTTABLELog_AuditUserGUID ON dbo.RBOCUSTTABLELog (AuditUserGUID) ON [PRIMARY]
END
GO