/*
	Incident No.	: ONE-7269
	Responsible		: Adrian Chiorean
	Sprint			: Stunsig
	Date created	: 30.06.2017

	Description		: Increase user settings value size
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'USERSETTINGS' AND COLUMN_NAME = 'Value')
BEGIN
	ALTER TABLE USERSETTINGS ALTER COLUMN [Value] NVARCHAR(max) NOT NULL
END
GO