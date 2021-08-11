﻿
/*

	Incident No.	: 
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 27.02.2014
*/

USE LSPOSNET_Audit
GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KITCHENDISPLAYSTATIONSLog' AND COLUMN_NAME = 'SCREENALIGNMENT')
BEGIN
	ALTER TABLE KITCHENDISPLAYSTATIONSLog DROP COLUMN SCREENALIGNMENT
END

