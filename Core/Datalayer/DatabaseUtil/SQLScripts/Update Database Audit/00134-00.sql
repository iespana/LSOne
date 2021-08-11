﻿/*
	Incident No.	: ONE-8518
	Responsible		: Adrian Chiorean
	Sprint			: Alzir
	Date created	: 12.03.2019

	Description		: Add column SALESPERSONPROMPT to POSFUNCTIONALITYPROFILELog
*/

USE LSPOSNET_Audit

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILELog' AND COLUMN_NAME = 'SALESPERSONPROMPT')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILELog ADD SALESPERSONPROMPT INT NULL
END
GO