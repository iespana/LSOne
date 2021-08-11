/*
	Incident No.	: ONE-8762
	Responsible		: Hörður Kristjánsson
	Sprint			: Kunivas
	Date created	: 14.08.2018

	Description		: Add "Print grand totals" configuration to functionality profile
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'ZRPTPRINTGRANDTOTALS')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD ZRPTPRINTGRANDTOTALS BIT NULL DEFAULT(1) WITH VALUES;

	EXECUTE spDB_SetFieldDescription_1_0 'POSFUNCTIONALITYPROFILE', 'ZRPTPRINTGRANDTOTALS', 'Configuration to control whether the grand totals section on the X/Z report should be printed or not';
END


GO