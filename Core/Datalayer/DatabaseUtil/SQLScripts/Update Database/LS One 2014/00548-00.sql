
/*

	Incident No.	: ONE-397
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Gina
	Date created	: 30.08.2014

	Description		: Change column from being an enum to bool
	
						
*/

USE LSPOSNET
GO

ALTER TABLE dbo.POSTRANSACTIONSERVICEPROFILE ALTER COLUMN CENTRALRETURNLOOKUP TINYINT NULL
GO
