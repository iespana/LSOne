﻿/*
	Incident No.	: ONE-10042
	Responsible		: Adrian Chiorean
	Sprint			: Saiph
	Date created	: 07.06.2019

	Description		: Add column DISPLAYVOIDEDPAYMENTS to POSFUNCTIONALITYPROFILE
*/

USE LSPOSNET_Audit

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILELog' AND COLUMN_NAME = 'DISPLAYVOIDEDPAYMENTS')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILELog ADD DISPLAYVOIDEDPAYMENTS BIT NULL
END
GO