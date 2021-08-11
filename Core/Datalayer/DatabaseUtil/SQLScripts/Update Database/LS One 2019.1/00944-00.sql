﻿/*
	Incident No.	: ONE-10440
	Responsible		: Adrian Chiorean
	Sprint			: Maia
	Date created	: 03.10.2019

	Description		: Add AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY column to INVENTORYTEMPLATE. Mark BATCHSIZE and AUTOPOPULATEITEMS as obsolete
*/

USE LSPOSNET

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTORYTEMPLATE' AND COLUMN_NAME = 'BATCHSIZE')
BEGIN
	EXECUTE spDB_SetFieldDescription_1_0 'INVENTORYTEMPLATE', 'BATCHSIZE', 'Obsolete - ONE-10440';
END
GO

CREATE TABLE #tempVariables
(
	COLUMNADDED BIT NOT NULL
)
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTORYTEMPLATE' AND COLUMN_NAME = 'AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY')
BEGIN
	ALTER TABLE INVENTORYTEMPLATE ADD AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY BIT NOT NULL DEFAULT 0
	EXECUTE spDB_SetFieldDescription_1_0 'INVENTORYTEMPLATE', 'AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY', 'If true, sending a transfer order will automatically set the receiving quantity equal to the sending quantity.';
	INSERT INTO #tempVariables VALUES (1)
END
GO

IF ((SELECT TOP 1 COLUMNADDED FROM #tempVariables) = 1)
BEGIN
	UPDATE INVENTORYTEMPLATE SET AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY = 1 WHERE AUTOPOPULATEITEMS = 2
	EXECUTE spDB_SetFieldDescription_1_0 'INVENTORYTEMPLATE', 'AUTOPOPULATEITEMS', 'Obsolete - ONE-10440';
END
GO

DROP TABLE #tempVariables
GO