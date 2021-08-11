/*
	Incident No.	: ONE-7957
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Eket
	Date created	: 16.11.2017	
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[INVENTORYTEMPLATELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[INVENTORYTEMPLATELog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ID] [nvarchar] (20) NOT NULL,
    [NAME] [nvarchar] (100) NOT NULL,
    [CHANGEVENDORINLINE] [bit] NOT NULL,
    [CHANGEUOMINLINE] [bit] NOT NULL,
    [CALCULATESUGGESTEDQUANTITY] [bit] NOT NULL,
    [SETQUANTITYTOSUGGESTEDQUANTITY] [bit] NOT NULL,
    [DISPLAYREORDERPOINT] [bit] NOT NULL,
    [DISPLAYMAXIMUMINVENTORY] [bit] NOT NULL,
    [DISPLAYBARCODE] [bit] NOT NULL,
    [ALLSTORES] [bit] NOT NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    [ADDLINESWITHZEROSUGGESTEDQTY] [bit] NOT NULL,
    [TEMPLATEENTRYTYPE] [int] NOT NULL,
    [UNITSELECTION] [int] NOT NULL,
    [ENTERINGTYPE] [int] NOT NULL,
    [QUANTITYMETHOD] [int] NOT NULL,
    [DEFAULTQUANTITY] [numeric] (28,12) NOT NULL,
    [AREAID] [uniqueidentifier] NULL,
    [DEFAULTSTORE] [nvarchar] (20) NULL,
    [AUTOPOPULATEITEMS] [int] NULL,
    Deleted bit NULL)

    alter table dbo.INVENTORYTEMPLATELog add constraint PK_INVENTORYTEMPLATELog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_INVENTORYTEMPLATELog_AuditUserGUID ON dbo.INVENTORYTEMPLATELog (AuditUserGUID) ON [PRIMARY]
END
GO
