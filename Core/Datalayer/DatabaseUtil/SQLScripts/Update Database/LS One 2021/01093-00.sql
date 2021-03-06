/*
	Incident No.	: ONE-13021
	Responsible		: Celia Blacu
	Sprint			: Renminbi
	Date created	: 14.12.2020

	Description		: Barcode is not saved in the POS button properties dialog
*/

USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSMENULINE' AND COLUMN_NAME = 'PARAMETERITEMID')
BEGIN
	ALTER TABLE POSMENULINE ADD PARAMETERITEMID NVARCHAR(30)
END
GO