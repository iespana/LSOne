﻿/*
	Incident No.	: ONE-10095
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Vega
	Date created	: 16.09.2019

	Description		: Add columns LIMITATIONDISPLAYTYPE and DISPLAYLIMITATIONSTOTALSINPOS to POSFUNCTIONALITYPROFILELog
*/

USE LSPOSNET_Audit

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILELog' AND COLUMN_NAME = 'LIMITATIONDISPLAYTYPE')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILELog ADD LIMITATIONDISPLAYTYPE INT NOT NULL DEFAULT 0 WITH VALUES
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILELog' AND COLUMN_NAME = 'DISPLAYLIMITATIONSTOTALSINPOS')
BEGIN
		ALTER TABLE POSFUNCTIONALITYPROFILELog ADD DISPLAYLIMITATIONSTOTALSINPOS TINYINT NOT NULL DEFAULT 0 WITH VALUES
END
GO