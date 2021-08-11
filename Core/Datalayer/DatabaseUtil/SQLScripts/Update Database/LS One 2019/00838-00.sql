﻿/*
	Incident No.	: ONE-8771
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: Kerla
	Date created	: 30.05.2018

	Description		: Adding column TAXEXEMPT to table CUSTOMER
*/

USE LSPOSNET

-- Add the columns

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CUSTOMER' AND COLUMN_NAME = 'TAXEXEMPT')
BEGIN
	ALTER TABLE CUSTOMER ADD TAXEXEMPT INT NULL
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOPARAMETERS' AND COLUMN_NAME = 'TAXEXEMPTTAXGROUP')
BEGIN
	ALTER TABLE RBOPARAMETERS ADD TAXEXEMPTTAXGROUP NVARCHAR(20) NULL
END
GO