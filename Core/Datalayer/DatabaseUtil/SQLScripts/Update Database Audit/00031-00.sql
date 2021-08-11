
/*

	Incident No.	: 
	Responsible		: Óskar Bjarnason
	Sprint			: Freyr
	Date created	: 30.09.2011
	
	Description		: Add log tables for the following tables
						- POSISSUSPENSIONTYPE, POSISSUSPENSIONADDINFO

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- POSISSUSPENSIONTYPELog,POSISSUSPENSIONADDINFOLog - Created
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSISSUSPENSIONTYPELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSISSUSPENSIONTYPELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (40) NOT NULL,
    [DESCRIPTION] [nvarchar] (60) NULL,
    [ALLOWEOD] [int] NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.POSISSUSPENSIONTYPELog add constraint PK_POSISSUSPENSIONTYPELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSISSUSPENSIONTYPELog_AuditUserGUID ON dbo.POSISSUSPENSIONTYPELog (AuditUserGUID) ON [PRIMARY]
END

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSISSUSPENSIONADDINFOLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSISSUSPENSIONADDINFOLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (40) NOT NULL,
    [SUSPENSIONTYPEID] [nvarchar] (40) NOT NULL,
    [PROMPT] [nvarchar] (60) NULL,
    [FIELDORDER] [int] NULL,
    [INFOTYPE] [int] NULL,
    [INFOTYPESELECTION] [nvarchar] (20) NOT NULL,
    [REQUIRED] [tinyint] NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.POSISSUSPENSIONADDINFOLog add constraint PK_POSISSUSPENSIONADDINFOLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSISSUSPENSIONADDINFOLog_AuditUserGUID ON dbo.POSISSUSPENSIONADDINFOLog (AuditUserGUID) ON [PRIMARY]
END
GO

