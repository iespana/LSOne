﻿/*
	Incident No.	: ONE-10440
	Responsible		: Adrian Chiorean
	Sprint			: Mimosa
	Date created	: 04.11.2019

	Description		: Add ISEMAILRECEIPT column to RBOTRANSACTIONRECEIPTS
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONRECEIPTS' AND COLUMN_NAME = 'ISEMAILRECEIPT')
BEGIN
	ALTER TABLE RBOTRANSACTIONRECEIPTS ADD ISEMAILRECEIPT BIT NOT NULL DEFAULT 0
	EXECUTE spDB_SetFieldDescription_1_0 'RBOTRANSACTIONRECEIPTS', 'ISEMAILRECEIPT', 'True if this receipt was generated using the email profile and is meant to be sent via email.';
END
GO