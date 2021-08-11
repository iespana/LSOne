﻿/*
	Incident No.	: ONE-8702
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Kunivas
	Date created	: 28.06.2018

	Description		: Add a new configuration for periodic discount cache
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SALESPARAMETERS' AND COLUMN_NAME = 'CLEARPERIODICDISCOUNTCACHE')
BEGIN
	ALTER TABLE SALESPARAMETERS ADD CLEARPERIODICDISCOUNTCACHE INT NULL;

	EXECUTE spDB_SetFieldDescription_1_0 'SALESPARAMETERS', 'CLEARPERIODICDISCOUNTCACHE', 'Configuration for when the periodic discount cache should be cleared in the POS';
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'SALESPARAMETERS' AND COLUMN_NAME = 'CLEARPERIODICDISCOUNTCACHEMINUTES')
BEGIN
	ALTER TABLE SALESPARAMETERS ADD CLEARPERIODICDISCOUNTCACHEMINUTES INT NULL;

	EXECUTE spDB_SetFieldDescription_1_0 'SALESPARAMETERS', 'CLEARPERIODICDISCOUNTCACHEMINUTES', 'How much time should pass before the periodic discount cache is cleared';
END

GO