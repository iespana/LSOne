﻿
/*

	Incident No.	: N/A
	Responsible		: Marý B Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 12.7.2013

	Description		: Adds a new field to PosHardwareprofile
	
						
*/

USE LSPOSNET

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSHARDWAREPROFILE' AND COLUMN_NAME = 'DISPLAYBINARYCONVERSION')
BEGIN
	ALTER TABLE POSHARDWAREPROFILE ADD DISPLAYBINARYCONVERSION TINYINT NULL
END
GO


