/*
	Incident No.	: ONE-6548
	Responsible		: Adrian Chiorean
	Sprint			: Hemnes
	Date created	: 08.05.2017

	Description		: Increase name columns size for USERS and RBOSTAFFTABLE
*/

USE LSPOSNET

--------------------------------------------------- USERS
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'USERS' AND COLUMN_NAME = 'FirstName')
BEGIN
	ALTER TABLE USERS ALTER COLUMN [FirstName] NVARCHAR(60) NOT NULL
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'USERS' AND COLUMN_NAME = 'MiddleName')
BEGIN
	ALTER TABLE USERS ALTER COLUMN [MiddleName] NVARCHAR(60) NOT NULL
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'USERS' AND COLUMN_NAME = 'LastName')
BEGIN
	ALTER TABLE USERS ALTER COLUMN [LastName] NVARCHAR(60) NOT NULL
END

--------------------------------------------------- RBOSTAFFTABLE
IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTAFFTABLE' AND COLUMN_NAME = 'FIRSTNAME')
BEGIN
	ALTER TABLE RBOSTAFFTABLE ALTER COLUMN [FIRSTNAME] NVARCHAR(60) NULL
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTAFFTABLE' AND COLUMN_NAME = 'LASTNAME')
BEGIN
	ALTER TABLE RBOSTAFFTABLE ALTER COLUMN [LASTNAME] NVARCHAR(60) NULL
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTAFFTABLE' AND COLUMN_NAME = 'NAME')
BEGIN
	ALTER TABLE RBOSTAFFTABLE ALTER COLUMN [NAME] NVARCHAR(200) NULL
END
GO