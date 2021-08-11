﻿/*
	Incident No.	: ONE-6768
	Responsible		: Adrian Chiorean
	Sprint			: Färgrik
	Date created	: 18.05.2017

	Description		: Increase RECEIPTID columns size to 61 (StoreID 20 + dash '-' 1 + TerminalID 20 + ReceiptID 20)
	Check query		: SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME LIKE '%RECEIPTID%'
*/

USE LSPOSNET_Audit

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'RECEIPTID' AND TABLE_NAME = 'RBOGIFTCARDTRANSACTIONSLog' AND CHARACTER_MAXIMUM_LENGTH = 20)
BEGIN
	ALTER TABLE RBOGIFTCARDTRANSACTIONSLog ALTER COLUMN RECEIPTID NVARCHAR(61) NULL
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'RECEIPTID' AND TABLE_NAME = 'RBOCREDITVOUCHERTRANSACTIONSLog' AND CHARACTER_MAXIMUM_LENGTH = 20)
BEGIN
	ALTER TABLE RBOCREDITVOUCHERTRANSACTIONSLog ALTER COLUMN RECEIPTID NVARCHAR(61) NULL
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'RECEIPTIDNUMBERSEQUENCE' AND TABLE_NAME = 'RBOTERMINALTABLELog' AND CHARACTER_MAXIMUM_LENGTH = 20)
BEGIN
	ALTER TABLE RBOTERMINALTABLELog ALTER COLUMN RECEIPTIDNUMBERSEQUENCE NVARCHAR(61) NULL
END

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'RECEIPTID' AND TABLE_NAME = 'POSISTILLLAYOUTLog' AND CHARACTER_MAXIMUM_LENGTH = 20)
BEGIN
	ALTER TABLE POSISTILLLAYOUTLog ALTER COLUMN RECEIPTID NVARCHAR(61) NULL
END
GO