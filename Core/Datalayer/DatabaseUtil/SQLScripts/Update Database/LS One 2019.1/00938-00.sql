/*
	Incident No.	: ONE-10078: Tax exemption per item
	Responsible		: Hörður Kristjánsson
	Sprint			: Canopus
	Date created	: 09.09.2019

	Description		: Add tax exempt column to PAYMENTLIMITATIONS
*/

USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PAYMENTLIMITATIONS' AND COLUMN_NAME = 'TAXEXEMPT')
BEGIN
	ALTER TABLE PAYMENTLIMITATIONS ADD TAXEXEMPT BIT NOT NULL DEFAULT 0
	EXECUTE spDB_SetFieldDescription_1_0 'PAYMENTLIMITATIONS', 'TAXEXEMPT', 'If true then items paid using this limitation should be tax exempt';
END
GO