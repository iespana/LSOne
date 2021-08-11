
/*

	Incident No.	: 
	Responsible		: Guðbjörn Einarsson	
	Sprint			: LS Retail .NET Milky Way/Sprint 2 Mercury
	Date created	: 30.07.2012
	
	Description		: Add KMTRANSACTIONPROFILELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : KMTRANSACTIONPROFILELog - Added
						
*/


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[KMTRANSACTIONPROFILELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].KMTRANSACTIONPROFILELog(
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    ID uniqueidentifier NOT NULL,
    NAME nvarchar(20) NOT NULL,
    KITCHENMANAGERSERVER nvarchar(20) NOT NULL,
    KITCHENMANAGERPORT nvarchar(10) NOT NULL,
    DATAAREAID nvarchar(4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.KMTRANSACTIONPROFILELog add constraint PK_KMTRANSACTIONPROFILELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_KMTRANSACTIONPROFILELog_AuditUserGUID ON dbo.KMTRANSACTIONPROFILELog (AuditUserGUID) ON [PRIMARY]
END
GO

