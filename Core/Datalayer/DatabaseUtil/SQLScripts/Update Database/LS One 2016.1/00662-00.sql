
/*

	Incident No.	: ONE-4307
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Stuðmenn

	Description		: Adding column for description 
	
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PURCHASEORDERS' AND COLUMN_NAME = 'DESCRIPTION')
Begin
	ALTER TABLE dbo.PURCHASEORDERS ADD [DESCRIPTION] nvarchar(60) NULL
END
GO

