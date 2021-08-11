
/*

	Incident No.	: 26882
	Responsible		: Marý B. Steingrímsdóttir
	Sprint			: LS One 2014 - Nimbo
	Date created	: 20.12.2013						
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[CUSTGROUPLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[CUSTGROUPLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [NAME] [nvarchar] (60) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    [EXCLUSIVE] [tinyint] NULL,
    [CATEGORY] [nvarchar] (20) NULL,
    [PURCHASEAMOUNT] [numeric] (28,12) NULL,
	[USEPURCHASELIMIT] [tinyint] NULL,
	[PURCHASEPERIOD] [tinyint] NULL,
    Deleted bit NULL)

    alter table dbo.CUSTGROUPLog add constraint PK_CUSTGROUPLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_CUSTGROUPLog_AuditUserGUID ON dbo.CUSTGROUPLog (AuditUserGUID) ON [PRIMARY]
END
GO


IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[CUSTGROUPCATEGORYLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[CUSTGROUPCATEGORYLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [DESCRIPTION] [nvarchar] (60) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.CUSTGROUPCATEGORYLog add constraint PK_CUSTGROUPCATEGORYLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_CUSTGROUPCATEGORYLog_AuditUserGUID ON dbo.CUSTGROUPCATEGORYLog (AuditUserGUID) ON [PRIMARY]
END
GO





