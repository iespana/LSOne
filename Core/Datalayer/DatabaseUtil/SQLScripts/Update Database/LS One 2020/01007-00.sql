﻿/*
	Incident No.	: ONE-8879
	Responsible		: Ovidiu Caba
	Sprint			: Shiba Inu 
	Date created	: 05.03.2020

	Description		: Add column KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT to POSFUNCTIONALITYPROFILE
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT' AND TABLE_NAME = 'POSFUNCTIONALITYPROFILE')
BEGIN
	ALTER TABLE POSFUNCTIONALITYPROFILE ADD [KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT] BIT NOT NULL DEFAULT(0)
	EXECUTE spDB_SetFieldDescription_1_0 'POSFUNCTIONALITYPROFILE', 'KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT', 'True if we want the receipt dialog to remain open in POS after printing a receipt'
END
GO