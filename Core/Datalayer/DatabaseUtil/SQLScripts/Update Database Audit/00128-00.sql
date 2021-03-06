/*
	Incident No.	: ONE-8762
	Responsible		: Hörður Kristjánsson
	Sprint			: Kunivas
	Date created	: 14.08.2018

	Description		: Add "Print grand totals" configuration to functionality profile
*/

USE LSPOSNET_Audit

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILELog' AND COLUMN_NAME = 'ZRPTPRINTGRANDTOTALS')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILELog ADD ZRPTPRINTGRANDTOTALS BIT	
END

GO