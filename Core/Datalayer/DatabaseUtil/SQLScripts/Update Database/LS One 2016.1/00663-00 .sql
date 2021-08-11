
/*

	Incident No.	: ONE-4309
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Todmobil

	Description		: Adding column for tax group
	
						
*/

USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PURCHASEORDERMISCCHARGES' AND COLUMN_NAME = 'TAXGROUP')
Begin
	ALTER TABLE dbo.PURCHASEORDERMISCCHARGES ADD [TAXGROUP] nvarchar(20) NULL
END
GO

