﻿/*

	Incident No.	: N/A
	Responsible		: Sigfus Johannesson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 10.9.2013

	Description		: Added a new column to the license table for licensefiles.
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISLICENSE' AND COLUMN_NAME = 'LICENSEFILE')
BEGIN
	ALTER TABLE POSISLICENSE ADD LICENSEFILE NVARCHAR(MAX)
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISLICENSE' AND COLUMN_NAME = 'MACHINEID')
BEGIN
	ALTER TABLE POSISLICENSE ADD MACHINEID NVARCHAR(1024)
END

GO
