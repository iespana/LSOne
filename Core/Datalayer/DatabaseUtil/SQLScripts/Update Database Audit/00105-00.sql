/*

	Incident No.	: ONE-4383
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Todmobil

	Description		: Adding Created/Posted date to goods receiving documents
	
						
*/


USE LSPOSNET_Audit
GO

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GOODSRECEIVINGLog' and COLUMN_NAME = 'CREATEDDATE')
BEGIN	
	ALTER TABLE dbo.GOODSRECEIVINGLog ADD CREATEDDATE [DATETIME] NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GOODSRECEIVINGLog' AND COLUMN_NAME = 'POSTEDDATE')
BEGIN
	ALTER TABLE dbo.GOODSRECEIVINGLog ADD POSTEDDATE [DATETIME] NULL
END