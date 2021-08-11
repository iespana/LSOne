﻿
/*

	Incident No.	: 9450
	Responsible		: Hörður Kristjánsson
	Sprint			: SP3
	Date created	: 06.04.2011

	Description		: Added DESCRIPTION to the STATIONSELECTIONLog

	Logic scripts   : Audit Logig
	
	Tables affected	: STATIONSELECTIONLog
						
*/

USE LSPOSNET_Audit

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'STATIONSELECTIONLog' AND COLUMN_NAME = 'DESCRIPTION')
BEGIN
	ALTER TABLE STATIONSELECTIONLog ADD DESCRIPTION [nvarchar](60) NULL
END

GO