
/*

	Incident No.	: 9480
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012/Embla
	Date created	: 17.02.2012
	
	Description		: Added table RBODISCOUNTOFFERLINELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	- RBODISCOUNTOFFERLINELog - added
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBODISCOUNTOFFERLINELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBODISCOUNTOFFERLINELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [OFFERID] [nvarchar] (20) NOT NULL,
    [STATUS] [int] NULL,
    [ITEMRELATION] [nvarchar] (20) NULL,
    [NAME] [nvarchar] (60) NULL,
    [STANDARDPRICEINCLTAX_DEL] [numeric] (28,12) NULL,
    [STANDARDPRICE_DEL] [numeric] (28,12) NULL,
    [DISCPCT] [numeric] (28,12) NULL,
    [DISCAMOUNT] [numeric] (28,12) NULL,
    [OFFERPRICE] [numeric] (28,12) NULL,
    [OFFERPRICEINCLTAX] [numeric] (28,12) NULL,
    [UNIT] [nvarchar] (20) NULL,
    [TYPE] [int] NULL,
    [CURRENCY_DEL] [nvarchar] (3) NULL,
    [DISCONPOS_DEL] [tinyint] NULL,
    [DISCAMOUNTINCLTAX] [numeric] (28,12) NULL,
    [MODIFIEDDATE] [datetime] NULL,
    [MODIFIEDTIME] [int] NULL,
    [MODIFIEDBY] [nvarchar] (5) NULL,
    [MODIFIEDTRANSACTIONID] [int] NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    [ID] [uniqueidentifier] NOT NULL,
    Deleted bit NULL)

    alter table dbo.RBODISCOUNTOFFERLINELog add constraint PK_RBODISCOUNTOFFERLINELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBODISCOUNTOFFERLINELog_AuditUserGUID ON dbo.RBODISCOUNTOFFERLINELog (AuditUserGUID) ON [PRIMARY]
END
GO
