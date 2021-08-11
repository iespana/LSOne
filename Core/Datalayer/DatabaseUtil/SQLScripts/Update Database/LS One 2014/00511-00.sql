﻿
/*
	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 07.02.2014

	Description		: Adding returns settings to POSFUNCTIONALITYPROFILE
						
*/
USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'RETURNSPRINTEDTWICE')
BEGIN
	ALTER TABLE RBOSTORETABLE ADD RETURNSPRINTEDTWICE TINYINT NULL
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOSTORETABLE' AND COLUMN_NAME = 'TENDERRECEIPTSAREREPRINTED')
BEGIN
	ALTER TABLE RBOSTORETABLE ADD TENDERRECEIPTSAREREPRINTED TINYINT NULL
END
GO


