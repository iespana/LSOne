/*
	Incident No.	: ONE-8530
	Responsible		: Adrian Chiorean
	Sprint			: Romulans
	Date created	: 18.01.2019

	Description		: Increase VALUE1 and VALUE2 column size to max in table JscSubJobFromTableFilters
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscSubJobFromTableFilters' AND COLUMN_NAME = 'Value1')
BEGIN
IF(COL_LENGTH('JscSubJobFromTableFilters', 'Value1') != -1) -- Length of NVARCHAR(max) is -1
	BEGIN 
		ALTER TABLE [JscSubJobFromTableFilters] ALTER COLUMN [Value1] NVARCHAR(max) NULL
	END
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscSubJobFromTableFilters' AND COLUMN_NAME = 'Value2')
BEGIN
IF(COL_LENGTH('JscSubJobFromTableFilters', 'Value2') != -1) -- Length of NVARCHAR(max) is -1
	BEGIN 
		ALTER TABLE [JscSubJobFromTableFilters] ALTER COLUMN [Value2] NVARCHAR(max) NULL
	END
END
GO