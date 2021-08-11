
/*

	Incident No.	: 9450
	Responsible		: Björn Eiríksson
	Sprint			: SP3
	Date created	: 14.04.2011

	Description		: Added RBOINVENTITEMIMAGELo

	Logic scripts   : Audit Logic
	
	Tables affected	: RBOINVENTITEMIMAGELo
						
*/

USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOINVENTITEMIMAGELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOINVENTITEMIMAGELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ITEMID] [nvarchar] (20) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOINVENTITEMIMAGELog add constraint PK_RBOINVENTITEMIMAGELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOINVENTITEMIMAGELog_AuditUserGUID ON dbo.RBOINVENTITEMIMAGELog (AuditUserGUID) ON [PRIMARY]
END
GO
