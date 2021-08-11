
/*

	Incident No.	: 17099
	Responsible		: Erna Guðrún Sigurðardóttir
	Sprint			: LS Retail .NET Milky Way/Sprint 2 Mercury
	Date created	: 18.06.2012
	
	Description		: Add table RBOINVENTTABLELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : RBOINVENTTABLELog - Added
						
*/


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOINVENTTRANSLATIONSLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOINVENTTRANSLATIONSLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    [ITEMID] [nvarchar] (20) NOT NULL,
    [CULTURENAME] [nvarchar] (20) NOT NULL,
    [DESCRIPTION] [nvarchar] (250) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOINVENTTRANSLATIONSLog add constraint PK_RBOINVENTTRANSLATIONSLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOINVENTTRANSLATIONSLog_AuditUserGUID ON dbo.RBOINVENTTRANSLATIONSLog (AuditUserGUID) ON [PRIMARY]
END
GO

