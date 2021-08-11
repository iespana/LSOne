
/*

	Incident No.	: 12062
	Responsible		: Óskar Bjarnason
	Sprint			: Freyr
	Date created	: 13.10.2011
	
	Description		: Add log tables for the following tables
						- POSISSUSPENSIONTRANSACTION, POSISSUSPENSIONTRANSACTIONADDINFO

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : POSISSUSPENDEDTRANSACTIONSLog - Created
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSISSUSPENDEDTRANSACTIONSLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSISSUSPENDEDTRANSACTIONSLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [TRANSACTIONID] [varchar] (40) NOT NULL,
    [BYTELENGTH] [int] NULL,
    [TRANSDATE] [datetime] NOT NULL,
    [STAFF] [nvarchar] (20) NOT NULL,
    [NETAMOUNT] [numeric] (28,12) NOT NULL,
    [STOREID] [nvarchar] (20) NOT NULL,
    [TERMINALID] [nvarchar] (20) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    [RECALLEDBY] [nvarchar] (20) NULL,
    [DESCRIPTION] [nvarchar] (60) NULL,
    [ALLOWEOD] [int] NULL,
    [ACTIVE] [tinyint] NULL,
    [SUSPENSIONTYPEID] [nvarchar] (40) NULL,
    Deleted bit NULL)

    alter table dbo.POSISSUSPENDEDTRANSACTIONSLog add constraint PK_POSISSUSPENDEDTRANSACTIONSLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSISSUSPENDEDTRANSACTIONSLog_AuditUserGUID ON dbo.POSISSUSPENDEDTRANSACTIONSLog (AuditUserGUID) ON [PRIMARY]
END
GO


----------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSISSUSPENDTRANSADDINFOLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSISSUSPENDTRANSADDINFOLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    [TRANSACTIONID] [nvarchar] (40) NOT NULL,
    [PROMPT] [nvarchar] (60) NULL,
    [FIELDORDER] [int] NOT NULL,
    [INFOTYPE] [int] NOT NULL,
    [INFOTYPESELECTION] [nvarchar] (20) NOT NULL,
    [TEXTRESULT1] [nvarchar] (255) NULL,
    [TEXTRESULT2] [nvarchar] (255) NULL,
    [TEXTRESULT3] [nvarchar] (60) NULL,
    [TEXTRESULT4] [nvarchar] (10) NULL,
    [TEXTRESULT5] [nvarchar] (30) NULL,
    [TEXTRESULT6] [nvarchar] (20) NULL,
    [DATERESULT] [datetime] NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.POSISSUSPENDTRANSADDINFOLog add constraint PK_POSISSUSPENDTRANSADDINFOLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSISSUSPENDTRANSADDINFOLog_AuditUserGUID ON dbo.POSISSUSPENDTRANSADDINFOLog (AuditUserGUID) ON [PRIMARY]
END
GO

