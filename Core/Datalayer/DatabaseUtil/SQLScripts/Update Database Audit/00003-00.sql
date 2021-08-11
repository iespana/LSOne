
/*

	Incident No.	: 6602
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET v 2010 - Sprint 3
	Date created	: 15.11.2010
	
	Description		: Add audit tables for Statements

	Logic scripts   : No stored procedures added or changed
	
	Tables affected:	RBOSTATEMENTTABLELog
						RBOSTATEMENTLINELog 	
						
*/


USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSTATEMENTTABLELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].RBOSTATEMENTTABLELog(
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[AuditUserGUID] [uniqueidentifier] NOT NULL,
	[AuditUserLogin] [nvarchar](32) NOT NULL,
	[AuditDate] [datetime] NOT NULL,
	[STATEMENTID] [int] NOT NULL,
	[STOREID] [varchar](60) NULL,
	[CALCULATEDTIME] [datetime] NULL,
	[POSTINGDATE] [date] NULL,
	[PERIODSTARTINGTIME] [datetime] NULL,
	[PERIODENDINGTIME] [datetime] NULL,
	[POSTED] [tinyint] NULL,
	[CALCULATED] [tinyint] NULL,
	[DATAAREAID] [varchar](4) NOT NULL,
	[Deleted] [bit] NULL)

	alter table dbo.RBOSTATEMENTTABLELog add constraint PK_RBOSTATEMENTTABLELog
	primary key clustered (AuditID) on [PRIMARY]

	create nonclustered index IX_RBOSTATEMENTTABLELog_AuditUserGUID ON dbo.RBOSTATEMENTTABLELog (AuditUserGUID) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[RBOSTATEMENTLINELog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].RBOSTATEMENTLINELog(
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[AuditUserGUID] [uniqueidentifier] NOT NULL,
	[AuditUserLogin] [nvarchar](32) NOT NULL,
	[AuditDate] [datetime] NOT NULL,
	[STATEMENTID] [int] NOT NULL,
	[LINENUMBER] [int] NOT NULL,
	[STAFFID] [varchar](10) NULL,
	[TERMINALID] [varchar](10) NULL,
	[CURRENCYCODE] [varchar](10) NULL,
	[TENDERID] [varchar](60) NULL,
	[TRANSACTIONAMOUNT] [decimal](24, 6) NULL,
	[BANKEDAMOUNT] [decimal](24, 6) NULL,
	[SAFEAMOUNT] [decimal](24, 6) NULL,
	[COUNTEDAMOUNT] [decimal](24, 6) NULL,
	[DIFFERENCE] [decimal](24, 6) NULL,
	[DATAAREAID] [varchar](4) NOT NULL,
	[Deleted] [bit] NULL)

	alter table dbo.RBOSTATEMENTLINELog add constraint PK_RBOSTATEMENTLINELog
	primary key clustered (AuditID) on [PRIMARY]

	create nonclustered index IX_RBOSTATEMENTLINELog_AuditUserGUID ON dbo.RBOSTATEMENTLINELog (AuditUserGUID) ON [PRIMARY]
END
GO