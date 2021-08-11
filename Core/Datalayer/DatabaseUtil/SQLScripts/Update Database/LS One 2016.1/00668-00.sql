
/*

	Incident No.	: ONE-4773
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Skálmöld

	Description		: Adding a column for reference
	
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTTRANS' AND COLUMN_NAME = 'REFERENCE')
BEGIN
	ALTER TABLE dbo.INVENTTRANS ADD REFERENCE NVARCHAR(20) NOT NULL DEFAULT ''
END
GO

