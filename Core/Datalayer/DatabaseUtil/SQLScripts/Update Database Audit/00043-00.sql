
/*

	Incident No.	: 16591
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012/Hel
	Date created	: 27.04.2012
	
	Description		: alter table INVENTTABLEMODULELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : INVENTTABLEMODULELog - Added DEFAULTPROFIT column
						
*/


USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOINVENTTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[RBOINVENTTABLELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ITEMID] [nvarchar] (20) NOT NULL,
    [ITEMTYPE] [int] NULL,
    [ITEMGROUP] [nvarchar] (20) NULL,
    [ITEMDEPARTMENT] [nvarchar] (20) NULL,
    [ITEMFAMILY] [nvarchar] (20) NULL,
    [UNITPRICEINCLUDINGTAX] [numeric] (28,12) NULL,
    [COSTCALCULATIONONPOS] [int] NULL,
    [NOINVENTPOSTING] [tinyint] NULL,
    [ZEROPRICEVALID] [tinyint] NULL,
    [QTYBECOMESNEGATIVE] [tinyint] NULL,
    [NODISCOUNTALLOWED] [tinyint] NULL,
    [KEYINGINPRICE] [int] NULL,
    [SCALEITEM] [tinyint] NULL,
    [KEYINGINQTY] [int] NULL,
    [DATEBLOCKED] [datetime] NULL,
    [DATETOBEBLOCKED] [datetime] NULL,
    [BLOCKEDONPOS] [tinyint] NULL,
    [DISPENSEPRINTINGDISABLED] [tinyint] NULL,
    [DISPENSEPRINTERGROUPID] [nvarchar] (20) NULL,
    [BASECOMPARISONUNITCODE] [nvarchar] (20) NULL,
    [BARCODESETUPID] [nvarchar] (20) NULL,
    [PRINTVARIANTSSHELFLABELS] [tinyint] NULL,
    [COLORGROUP] [nvarchar] (20) NULL,
    [SIZEGROUP] [nvarchar] (20) NULL,
    [USEEANSTANDARDBARCODE] [tinyint] NULL,
    [STYLEGROUP] [nvarchar] (20) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    [FUELITEM] [tinyint] NULL,
    [GRADEID] [nvarchar] (20) NULL,
    [MUSTKEYINCOMMENT] [tinyint] NULL,
    [DATETOACTIVATEITEM] [datetime] NULL,
    [BUSINESSGROUP] [nvarchar] (20) NULL,
    [DIVISIONGROUP] [nvarchar] (20) NULL,
    [DEFAULTPROFIT] [numeric] (28,12) NULL,
    Deleted bit NULL)

    alter table dbo.RBOINVENTTABLELog add constraint PK_RBOINVENTTABLELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_RBOINVENTTABLELog_AuditUserGUID ON dbo.RBOINVENTTABLELog (AuditUserGUID) ON [PRIMARY]
END
GO
