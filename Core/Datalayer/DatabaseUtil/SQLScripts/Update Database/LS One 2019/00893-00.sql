/*
	Incident No.	: ONE-10168
	Responsible		: Adrian Chiorean
	Sprint			: Saiph
	Date created	: 30.05.2019

	Description		: Add field description to REGNUM and COREGNUM in company info
*/

USE LSPOSNET

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'COMPANYINFO' AND COLUMN_NAME = 'REGNUM')
BEGIN
	EXECUTE spDB_SetFieldDescription_1_0 'COMPANYINFO', 'REGNUM', 'Region specific registration number';

	IF(COL_LENGTH('COMPANYINFO', 'REGNUM') < 60) -- We want to update the column to be NVARCHAR(30) which has an equivalent length in bytes of 60
	BEGIN 
		ALTER TABLE COMPANYINFO ALTER COLUMN REGNUM NVARCHAR(30) NOT NULL
	END
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'COMPANYINFO' AND COLUMN_NAME = 'COREGNUM')
BEGIN
	EXECUTE spDB_SetFieldDescription_1_0 'COMPANYINFO', 'COREGNUM', 'Company registration number';

	IF(COL_LENGTH('COMPANYINFO', 'COREGNUM') < 60) -- We want to update the column to be NVARCHAR(30) which has an equivalent length in bytes of 60
	BEGIN 
		ALTER TABLE COMPANYINFO ALTER COLUMN COREGNUM NVARCHAR(30) NOT NULL
	END
END
GO