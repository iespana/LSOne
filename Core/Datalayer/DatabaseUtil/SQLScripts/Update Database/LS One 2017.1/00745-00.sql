﻿/*
	Incident No.	: ONE-6574
	Responsible		: Adrian Chiorean
	Sprint			: Hemnes
	Date created	: 26.04.2017

	Description		: Add Mandatory customer fields to site service profile
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'ALLOWCUSTOMERMANUALID' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [ALLOWCUSTOMERMANUALID] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'ALLOWCUSTOMERMANUALID', 'True if the user can manually type an ID when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERDEFAULTCREDITLIMIT' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMERDEFAULTCREDITLIMIT] NUMERIC(28,12) NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMERDEFAULTCREDITLIMIT', 'The default credit limit of the customer set when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERNAMEMANDATORY' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMERNAMEMANDATORY] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMERNAMEMANDATORY', 'True if the name of the customer is mandatory when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERSEARCHALIASMANDATORY' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMERSEARCHALIASMANDATORY] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMERSEARCHALIASMANDATORY', 'True if the search alias of the customer is mandatory when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERADDRESSMANDATORY' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMERADDRESSMANDATORY] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMERADDRESSMANDATORY', 'True if the address of the customer is mandatory when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERPHONEMANDATORY' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMERPHONEMANDATORY] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMERPHONEMANDATORY', 'True if the phone of the customer is mandatory when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMEREMAILMANDATORY' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMEREMAILMANDATORY] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMEREMAILMANDATORY', 'True if the email address of the customer is mandatory when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERRECEIPTEMAILMANDATORY' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMERRECEIPTEMAILMANDATORY] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMERRECEIPTEMAILMANDATORY', 'True if the receipt email address of the customer is mandatory when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERGENDERMANDATORY' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMERGENDERMANDATORY] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMERGENDERMANDATORY', 'True if the gender of the customer is mandatory when creating a new customer in the POS'
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'CUSTOMERBIRTHDATEMANDATORY' AND TABLE_NAME = 'POSTRANSACTIONSERVICEPROFILE')
BEGIN
	ALTER TABLE POSTRANSACTIONSERVICEPROFILE ADD [CUSTOMERBIRTHDATEMANDATORY] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSTRANSACTIONSERVICEPROFILE', 'CUSTOMERBIRTHDATEMANDATORY', 'True if the birth date of the customer is mandatory when creating a new customer in the POS'
END
GO