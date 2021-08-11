/*
	Incident No.	: ONE-6984
	Responsible		: Adrian Chiorean
	Sprint			: Äpplarö
	Date created	: 21.06.2017

	Description		: Add CUSTOMERLog audit table
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[CUSTOMERLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[CUSTOMERLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [MASTERID] [uniqueidentifier] NOT NULL,
    [ACCOUNTNUM] [nvarchar] (61) NOT NULL,
    [NAME] [nvarchar] (200) NULL,
    [INVOICEACCOUNT] [nvarchar] (61) NULL,
    [FIRSTNAME] [nvarchar] (60) NULL,
    [MIDDLENAME] [nvarchar] (60) NULL,
    [LASTNAME] [nvarchar] (60) NULL,
    [NAMEPREFIX] [nvarchar] (8) NULL,
    [NAMESUFFIX] [nvarchar] (8) NULL,
    [CURRENCY] [nvarchar] (3) NULL,
    [LANGUAGEID] [nvarchar] (5) NULL,
    [TAXGROUP] [nvarchar] (20) NULL,
    [PRICEGROUP] [nvarchar] (20) NULL,
    [LINEDISC] [nvarchar] (20) NULL,
    [MULTILINEDISC] [nvarchar] (20) NULL,
    [ENDDISC] [nvarchar] (20) NULL,
    [CREDITMAX] [numeric] (28,12) NULL,
    [ORGID] [nvarchar] (20) NULL,
    [BLOCKED] [int] NULL,
    [NONCHARGABLEACCOUNT] [tinyint] NULL,
    [INCLTAX] [tinyint] NULL,
    [PHONE] [nvarchar] (80) NULL,
    [CELLULARPHONE] [nvarchar] (80) NULL,
    [NAMEALIAS] [nvarchar] (80) NULL,
    [CUSTGROUP] [nvarchar] (20) NULL,
    [VATNUM] [nvarchar] (20) NULL,
    [EMAIL] [nvarchar] (80) NULL,
    [URL] [nvarchar] (255) NULL,
    [TAXOFFICE] [nvarchar] (20) NULL,
    [USEPURCHREQUEST] [tinyint] NULL,
    [LOCALLYSAVED] [tinyint] NULL,
    [GENDER] [int] NULL,
    [DATEOFBIRTH] [datetime] NULL,
    [RECEIPTOPTION] [int] NULL,
    [RECEIPTEMAIL] [nvarchar] (80) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.CUSTOMERLog add constraint PK_CUSTOMERLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_CUSTOMERLog_AuditUserGUID ON dbo.CUSTOMERLog (AuditUserGUID) ON [PRIMARY]
END
GO