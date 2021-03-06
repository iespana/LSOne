/*

	Incident No.	: 
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 16.12.2013

	Description		: Added new fields to transaction table, store table and hardware profile table
*/

USE LSPOSNET
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONTABLE' AND COLUMN_NAME = 'SALESSEQUENCEID')
BEGIN
	ALTER TABLE dbo.RBOTRANSACTIONTABLE ADD SALESSEQUENCEID NVARCHAR(20)	
END
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'BARCODESYMBOLOGY')
BEGIN
	ALTER TABLE dbo.RBOSTORETABLE ADD BARCODESYMBOLOGY int	
END
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSHARDWAREPROFILE' AND COLUMN_NAME = 'PRINTEREXTRALINES')
BEGIN
	ALTER TABLE dbo.POSHARDWAREPROFILE ADD PRINTEREXTRALINES int	
END
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'NUMBERSEQUENCETABLE' AND COLUMN_NAME = 'WRAPAROUND')
BEGIN
	ALTER TABLE dbo.NUMBERSEQUENCETABLE ADD WRAPAROUND tinyint	
END
