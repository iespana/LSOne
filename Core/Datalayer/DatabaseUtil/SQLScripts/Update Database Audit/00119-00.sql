/*
	Incident No.	: ONE-6888
	Responsible		: Adrian Chiorean
	Sprint			: Pax
	Date created	: 06.06.2017

	Description		: Add USERPROFILE audit table
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[POSUSERPROFILELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[POSUSERPROFILELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [PROFILEID] [nvarchar] (20) NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    [DESCRIPTION] [nvarchar] (60) NOT NULL,
    [MAXLINEDISCOUNTAMOUNT] [numeric] (28,12) NOT NULL,
    [MAXDISCOUNTPCT] [numeric] (28,12) NOT NULL,
    [MAXTOTALDISCOUNTAMOUNT] [numeric] (28,12) NOT NULL,
    [MAXTOTALDISCOUNTPCT] [numeric] (28,12) NOT NULL,
    [MAXLINERETURNAMOUNT] [numeric] (28,12) NOT NULL,
    [MAXTOTALRETURNAMOUNT] [numeric] (28,12) NOT NULL,
    [STOREID] [nvarchar] (20) NULL,
    [VISUALPROFILE] [nvarchar] (20) NULL,
    [LAYOUTID] [nvarchar] (20) NULL,
    [KEYBOARDCODE] [nvarchar] (20) NULL,
    [LAYOUTNAME] [nvarchar] (20) NULL,
    [OPERATORCULTURE] [nvarchar] (20) NULL,
    Deleted bit NULL)

    alter table dbo.POSUSERPROFILELog add constraint PK_POSUSERPROFILELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_POSUSERPROFILELog_AuditUserGUID ON dbo.POSUSERPROFILELog (AuditUserGUID) ON [PRIMARY]
END
GO