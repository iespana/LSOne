/*

	Incident No.	: 
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Washington DC

	Description		: Adding new customer order settings tables
	
						
*/


USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[CUSTOMERORDERSETTINGSLog]') AND TYPE IN ('U'))
BEGIN
CREATE TABLE [dbo].[CUSTOMERORDERSETTINGSLog](
    AuditID int NOT NULL IDENTITY (1, 1),
    AuditUserGUID uniqueidentifier NOT NULL,
    AuditUserLogin nvarchar(32) NOT NULL,
    AuditDate datetime NOT NULL,
    [ORDERTYPE] [uniqueidentifier] NOT NULL,
    [ACCEPTSDEPOSITS] [tinyint] NOT NULL,
    [MINIMUMDEPOSITS] [numeric] (28,12) NOT NULL,
    [SOURCE] [tinyint] NOT NULL,
    [DELIVERY] [tinyint] NOT NULL,    
    [EXPIRATIONTIMEVALUE] [int] NOT NULL,
    [EXPIRATIONTIMEUNIT] [int] NOT NULL,
    [NUMBERSERIES] [nvarchar] (20) NOT NULL,
    Deleted bit NULL)

    alter table dbo.CUSTOMERORDERSETTINGSLog add constraint PK_CUSTOMERORDERSETTINGSLog
    primary key clustered (AuditID) on [PRIMARY]

    create nonclustered index IX_CUSTOMERORDERSETTINGSLog_AuditUserGUID ON dbo.CUSTOMERORDERSETTINGSLog (AuditUserGUID) ON [PRIMARY]
END
GO