
/*

	Incident No.	: 
	Responsible		: Björn eiríksson
	Sprint			: Baldur
	Date created	: 14.07.2011
	
	Description		: Add log tables for the following tables
						- RBOGIFTCARDTABLE, RBOGIFTCARDTRANSACTIONS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- RBOGIFTCARDTABLELog,RBOGIFTCARDTRANSACTIONSLog - Created
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOGIFTCARDTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOGIFTCARDTABLELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [GIFTCARDID] [nvarchar] (20) NOT NULL,
    [BALANCE] [numeric] (28,12) NULL,
    [CURRENCY] [nvarchar] (20) NOT NULL,
    [ACTIVE] [tinyint] NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOGIFTCARDTABLELog add constraint PK_RBOGIFTCARDTABLELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOGIFTCARDTABLELog_AuditUserGUID ON dbo.RBOGIFTCARDTABLELog (AuditUserGUID) ON [PRIMARY]
END

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOGIFTCARDTRANSACTIONSLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOGIFTCARDTRANSACTIONSLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [GIFTCARDID] [nvarchar] (20) NOT NULL,
    [GIFTCARDLINEID] [uniqueidentifier] NOT NULL,
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
    [DATAAREAID] [nvarchar] (40) NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBOGIFTCARDTRANSACTIONSLog add constraint PK_RBOGIFTCARDTRANSACTIONSLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOGIFTCARDTRANSACTIONSLog_AuditUserGUID ON dbo.RBOGIFTCARDTRANSACTIONSLog (AuditUserGUID) ON [PRIMARY]
END
GO

