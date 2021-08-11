
/*

	Incident No.	: 10049
	Responsible		: Guðbjörn Einarsson
	Sprint			: SP3
	Date created	: 09.05.2011

	Description		: Added PURCHASEORDERMISCCHARGESLog

	Logic scripts   : Audit Logic
	
	Tables affected	: PURCHASEORDERMISCCHARGESLog
						
*/

USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[PURCHASEORDERMISCCHARGESLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[PURCHASEORDERMISCCHARGESLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
	[PURCHASEORDERID] [nvarchar](20) NOT NULL,
	[LINENUMBER] [nvarchar](20) NOT NULL,
	[TYPE] [int] NULL,
	[REASON] [nvarchar](60) NULL,
	[AMOUNT] [decimal](24, 6) NULL,
	[TAXAMOUNT] [decimal](24, 6) NULL,
    [DATAAREAID] [nvarchar] (4) NOT NULL,
    Deleted bit NULL)

    alter table dbo.PURCHASEORDERMISCCHARGESLog add constraint PK_PURCHASEORDERMISCCHARGESLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_PURCHASEORDERMISCCHARGESLog_AuditUserGUID ON dbo.PURCHASEORDERMISCCHARGESLog (AuditUserGUID) ON [PRIMARY]
END
GO
