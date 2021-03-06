/*
	Incident No.	: ONE-6541
	Responsible		: Adrian Chiorean
	Sprint			: Lack
	Date created	: 20.04.2017

	Description		: Add JOURNALID to RBOTRANSACTIONTABLE
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERREQUIREDONRETURN' AND TABLE_NAME = 'POSFUNCTIONALITYPROFILE')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD [CUSTOMERREQUIREDONRETURN] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSFUNCTIONALITYPROFILE', 'CUSTOMERREQUIREDONRETURN', 'True if a customer is required when a transaction has returned items'
END
GO