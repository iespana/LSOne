/*
	Incident No.	: ONE-9217
	Responsible		: Adrian Chiorean
	Sprint			: Denobulans
	Date created	: 17.10.2018

	Description		: Increase LINENUM column size in INVENTJOURNALTRANSLog table
*/

USE LSPOSNET_Audit

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTJOURNALTRANSLog' AND COLUMN_NAME = 'LINENUM')
BEGIN
	IF(COL_LENGTH('INVENTJOURNALTRANSLog', 'LINENUM') < 120) -- We want to update the column to be NVARCHAR(60) which has an equivalent length in bytes of 120
	BEGIN 
		ALTER TABLE [INVENTJOURNALTRANSLog] ALTER COLUMN [LINENUM] NVARCHAR(60) NOT NULL
	END
END
GO