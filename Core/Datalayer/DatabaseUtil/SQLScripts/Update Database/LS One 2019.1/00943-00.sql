/*
	Incident No.	: ONE-10563
	Responsible		: Adrian Chiorean
	Sprint			: Bellatrix
	Date created	: 01.10.2019

	Description		: Add ISLOCALCURRENCY column to 
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTENDERTYPETABLE' AND COLUMN_NAME = 'ISLOCALCURRENCY')
BEGIN
	ALTER TABLE RBOTENDERTYPETABLE ADD ISLOCALCURRENCY BIT NOT NULL DEFAULT 0
	EXECUTE spDB_SetFieldDescription_1_0 'RBOTENDERTYPETABLE', 'ISLOCALCURRENCY', 'Specifies if the payment type is the local currency (default payment type). Only one payment type can be set as default and it is not allowed to have any limitations.';
END
GO