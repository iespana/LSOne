﻿
/*
	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: LS One 2014
	Date created	: 24.03.2014

	Description		: Added new fields to track original store and receipt ID of sold items
						
*/
USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONSALESTRANS' AND COLUMN_NAME = 'RETURNRECEIPTID')
BEGIN
	ALTER TABLE RBOTRANSACTIONSALESTRANS ADD RETURNRECEIPTID nvarchar(20) NULL
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONSALESTRANS' AND COLUMN_NAME = 'RETURNSTOREID')
BEGIN
	ALTER TABLE RBOTRANSACTIONSALESTRANS ADD RETURNSTOREID nvarchar(20) NULL
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTRANSACTIONSALESTRANS' AND COLUMN_NAME = 'RETURNTERMINALID')
BEGIN
	ALTER TABLE RBOTRANSACTIONSALESTRANS ADD RETURNTERMINALID nvarchar(20) NULL
END
