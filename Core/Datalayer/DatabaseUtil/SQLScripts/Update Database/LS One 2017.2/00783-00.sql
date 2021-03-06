/*
	Incident No.	: ONE-7183
	Responsible		: Adrian Chiorean
	Sprint			: Vardagen
	Date created	: 13.07.2017

	Description		: Add new columns to POSMENUHEADER
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSMENUHEADER' AND COLUMN_NAME = 'DEVICETYPE')
BEGIN
	ALTER TABLE POSMENUHEADER ADD DEVICETYPE INT NOT NULL DEFAULT 0
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSMENUHEADER' AND COLUMN_NAME = 'MAINMENU')
BEGIN
	ALTER TABLE POSMENUHEADER ADD MAINMENU BIT NOT NULL DEFAULT 0
END
GO