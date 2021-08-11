﻿
/*

	Incident No.	: 12506
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012/Thor
	Date created	: 26.10.2011

	Description		: Renaming and adding a column to suspended transactions

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosisSuspendedTransaction - Rename NETAMOUNT to BALANCE
											    - Added column BALANCEWITHTAX
						
*/

USE LSPOSNET
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISSUSPENDEDTRANSACTIONS' AND COLUMN_NAME = 'NETAMOUNT')
BEGIN
   EXEC sp_rename 'DBO.POSISSUSPENDEDTRANSACTIONS.NETAMOUNT', 'BALANCE', 'COLUMN'
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISSUSPENDEDTRANSACTIONS' AND COLUMN_NAME = 'BALANCEWITHTAX')
BEGIN
	ALTER TABLE dbo.POSISSUSPENDEDTRANSACTIONS ADD BALANCEWITHTAX NUMERIC(28,12) NULL
END
GO

exec spDB_SetFieldDescription_1_0 'POSISSUSPENDEDTRANSACTIONS','BALANCEWITHTAX','The transaction balance including tax'

IF NOT EXISTS (select * from sys.objects where name = 'DF_POSISSUSPENDEDTRANSACTIONS_BALANCEWITHTAX')
BEGIN
	ALTER TABLE dbo.POSISSUSPENDEDTRANSACTIONS ADD CONSTRAINT
		DF_POSISSUSPENDEDTRANSACTIONS_BALANCEWITHTAX DEFAULT 0 FOR BALANCEWITHTAX
END
GO