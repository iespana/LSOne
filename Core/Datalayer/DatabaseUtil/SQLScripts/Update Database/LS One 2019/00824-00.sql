/*
	Incident No.	: ONE-7028
	Responsible		: Adrian Chiorean
	Sprint			: Eket
	Date created	: 06.11.2017

	Description		: Add denomination description to RBOSTORECASHDECLARATIONTABLE table
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'DENOMINATIONDESCRIPTION' AND TABLE_NAME = 'RBOSTORECASHDECLARATIONTABLE')
BEGIN
	ALTER TABLE RBOSTORECASHDECLARATIONTABLE ADD DENOMINATIONDESCRIPTION nvarchar(10) NULL
END
GO