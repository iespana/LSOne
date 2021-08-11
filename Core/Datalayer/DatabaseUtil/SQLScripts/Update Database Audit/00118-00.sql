/*

	Incident No.	: ONE-6907
	Responsible		: Hörður Kristjánsson
	Sprint			: Pax
	Date created	: 01.06.2017

	Description		: Adding missing OMNI columns
	
	
	Tables affected	: POSFUNCTIONALITYPROFILELog
						
*/

USE LSPOSNET_Audit
GO

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILELog' and COLUMN_NAME = 'OMNISUSPENSIONTYPE')
BEGIN	
	ALTER TABLE [dbo].POSFUNCTIONALITYPROFILELog add OMNISUSPENSIONTYPE nvarchar(40) NULL
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILELog' and COLUMN_NAME = 'OMNIITEMIMAGELOOKUPGROUP')
BEGIN	
	ALTER TABLE [dbo].POSFUNCTIONALITYPROFILELog add OMNIITEMIMAGELOOKUPGROUP uniqueidentifier NULL
END
GO