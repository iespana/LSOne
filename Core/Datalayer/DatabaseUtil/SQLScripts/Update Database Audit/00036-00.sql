
/*

	Incident No.	: 12822
	Responsible		: Hörður Kristjánsson
	Sprint			: Thor
	Date created	: 17.10.2011
	
	Description		: Add log tables for the following tables
						- RBOMSRCARDTABLE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : RBOMSRCARDTABLELog - Created
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOMSRCARDTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOMSRCARDTABLELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [LINKTYPE] [int] NULL,
    [LINKID] [nvarchar] (20) NULL,
    [CARDNUMBER] [nvarchar] (30) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOMSRCARDTABLELog add constraint PK_RBOMSRCARDTABLELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOMSRCARDTABLELog_AuditUserGUID ON dbo.RBOMSRCARDTABLELog (AuditUserGUID) ON [PRIMARY]
END
GO
