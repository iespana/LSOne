﻿/*
	Incident No.	: ONE-10042
	Responsible		: Adrian Chiorean
	Sprint			: Saiph
	Date created	: 07.06.2019

	Description		: Add column DISPLAYVOIDEDPAYMENTS to POSFUNCTIONALITYPROFILE
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFUNCTIONALITYPROFILE' AND COLUMN_NAME = 'DISPLAYVOIDEDPAYMENTS')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD DISPLAYVOIDEDPAYMENTS BIT NOT NULL DEFAULT 1
END
GO