/*
	Incident No.	: ONE-12385
	Responsible		: Helgi Runar Gunnarsson
	Sprint			: Kol 
	Date created	: 10.08.2020

	Description		: Disallow direct sale of an item on the POS
*/

USE LSPOSNET_Audit

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RETAILITEMLog' AND COLUMN_NAME = 'CANBESOLD')
BEGIN
	ALTER TABLE RETAILITEMLog ADD [CANBESOLD] BIT NOT NULL DEFAULT 1
END
GO