
/*

	Incident No.	: 
	Responsible		: Björn Eiríksson
	Sprint			: Sif
	Date created	: 15.09.2011
	
	Description		: Add log tables for the following tables
						- RBOCREDITVOUCHERTABLE, RBOGIFTCARDTRANSACTIONS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- RBOCREDITVOUCHERTABLELog,RBOGIFTCARDTRANSACTIONSLog - Created
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOCREDITVOUCHERTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOCREDITVOUCHERTABLELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [VOUCHERID] [nvarchar] (20) NOT NULL,
    [BALANCE] [numeric] (28,12) NULL,
    [CURRENCY] [nvarchar] (20) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOCREDITVOUCHERTABLELog add constraint PK_RBOCREDITVOUCHERTABLELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOCREDITVOUCHERTABLELog_AuditUserGUID ON dbo.RBOCREDITVOUCHERTABLELog (AuditUserGUID) ON [PRIMARY]
END

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOCREDITVOUCHERTRANSACTIONSLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOCREDITVOUCHERTRANSACTIONSLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [VOUCHERID] [nvarchar] (20) NOT NULL,
    [VOUCHERLINEID] [uniqueidentifier] NOT NULL,
    [STOREID] [nvarchar] (20) NULL,
    [TERMINALID] [nvarchar] (20) NULL,
    [TRANSACTIONNUMBER] [nvarchar] (20) NULL,
    [RECEIPTID] [nvarchar] (20) NULL,
    [STAFFID] [nvarchar] (20) NULL,
    [USERID] [uniqueidentifier] NULL,
    [TRANSACTIONDATE] [date] NULL,
    [TRANSACTIONTIME] [time] NULL,
    [AMOUNT] [numeric] (28,12) NULL,
    [OPERATION] [int] NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOCREDITVOUCHERTRANSACTIONSLog add constraint PK_RBOCREDITVOUCHERTRANSACTIONSLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOCREDITVOUCHERTRANSACTIONSLog_AuditUserGUID ON dbo.RBOCREDITVOUCHERTRANSACTIONSLog (AuditUserGUID) ON [PRIMARY]
END
GO

