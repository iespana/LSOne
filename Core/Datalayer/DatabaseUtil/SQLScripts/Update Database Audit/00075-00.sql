/*
	Incident No.	: 
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 07.06.2013

	Description		: Move data from customer table to customer address table
					  Drop columns from customer table
*/

USE LSPOSNET_Audit
GO

IF NOT EXISTS (SELECT * FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID('[DBO].[CUSTOMERADDRESSLog]') AND TYPE IN ('U'))
BEGIN
	CREATE TABLE [dbo].[CUSTOMERADDRESSLog](
		AuditID int NOT NULL IDENTITY (1, 1),
		AuditUserGUID uniqueidentifier NOT NULL,
		AuditUserLogin nvarchar(32) NOT NULL,
		AuditDate datetime NOT NULL,
		[ACCOUNTNUM] [nvarchar](20) NOT NULL,
		[ADDRESS] [nvarchar](250) NULL,
		[COUNTRY] [nvarchar](10) NULL,
		[ZIPCODE] [nvarchar](10) NULL,
		[STATE] [nvarchar](10) NULL,
		[COUNTY] [nvarchar](10) NULL,
		[CITY] [nvarchar](60) NULL,
		[STREET] [nvarchar](250) NULL,
		[DATAAREAID] [nvarchar](4) NOT NULL,
		[TAXGROUP] [nvarchar](10) NULL,
		[ADDRESSTYPE] [int] NOT NULL,
		[ADDRESSFORMAT] [int] NULL,
		[PHONE] [nvarchar](20) NULL,
		[CELLULARPHONE] [nvarchar](20) NULL,
		[EMAIL] [nvarchar](80) NULL,
		[Deleted] [bit] NOT NULL)

	alter table dbo.CUSTOMERADDRESSLog add constraint PK_CUSTOMERADDRESSLog
	primary key clustered (AuditID) on [PRIMARY]

	create nonclustered index IXCUSTOMERADDRESSLog_AuditUserGUID ON dbo.CUSTOMERADDRESSLog (AuditUserGUID) ON [PRIMARY]
END
GO

-- move records to new audit log
IF EXISTS(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'CUSTTABLELog' AND COLUMN_NAME = 'STREET')
BEGIN
   -- if columns are missing, then the following will not work unless wrapped in sp_executesql
   exec sp_executesql N'INSERT into dbo.[CUSTOMERADDRESSLog](AuditUserGUID,AuditUserLogin,AuditDate,DATAAREAID,ACCOUNTNUM,
		ADDRESSTYPE,STREET,ADDRESS,ZIPCODE,CITY,STATE,COUNTY,COUNTRY,Deleted)
		SELECT AuditUserGUID,AuditUserLogin,AuditDate,DATAAREAID,ACCOUNTNUM,0,STREET,ADDRESS,ZIPCODE,CITY,STATE,COUNTY,COUNTRY,Deleted
		FROM dbo.[CUSTTABLELog] WHERE [DATAAREAID] = ''LSR'''
END
GO

-- drop columns from old log
IF EXISTS(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'CUSTTABLELog' AND COLUMN_NAME = 'STREET')
BEGIN
	ALTER TABLE CUSTTABLELog DROP COLUMN [STREET]
	ALTER TABLE CUSTTABLELog DROP COLUMN [ADDRESS]	
	ALTER TABLE CUSTTABLELog DROP COLUMN [ZIPCODE]
	ALTER TABLE CUSTTABLELog DROP COLUMN [CITY]
	ALTER TABLE CUSTTABLELog DROP COLUMN [STATE]
	ALTER TABLE CUSTTABLELog DROP COLUMN [COUNTY]
	ALTER TABLE CUSTTABLELog DROP COLUMN [COUNTRY]
END
