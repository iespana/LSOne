/*
	Incident No.	: ONE-4278
	Responsible		: Adrian Chiorean
	Sprint			: Eket
	Date created	: 02.11.2017

	Description		: Add transaction subtype to RboTransactionTable
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'SUBTYPE' AND TABLE_NAME = 'RBOTRANSACTIONTABLE')
BEGIN
	ALTER TABLE RBOTRANSACTIONTABLE ADD SUBTYPE INT NOT NULL DEFAULT 0
END
GO