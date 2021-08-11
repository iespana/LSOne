/*
	Incident No.	: ONE-11041 Create POS operations for "Inventory transfer requests and orders"
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: Mimosa
	Date created	: 31.10.2019

	Description		: Add inventory transfer operations
    Remarks			: Related to the SAP HANA inventory transfer changes
*/

USE LSPOSNET

IF NOT EXISTS (SELECT 1 FROM OPERATIONS WHERE ID = '802')
BEGIN
    INSERT INTO dbo.OPERATIONS(MASTERID, ID, DESCRIPTION, TYPE, LOOKUPTYPE, AUDIT)
	VALUES  (NEWID(), '802', 'Transfer request', 2, 25, 1)
END
ELSE
BEGIN
	PRINT '[OPERATIONS.Transfer request] was already inserted'
END

IF NOT EXISTS (SELECT 1 FROM OPERATIONS WHERE ID = '803')
BEGIN
    INSERT INTO dbo.OPERATIONS(MASTERID, ID, DESCRIPTION, TYPE, LOOKUPTYPE, AUDIT)
	VALUES  (NEWID(), '803', 'Transfer order', 2, 26, 1)
END
ELSE
BEGIN
	PRINT '[OPERATIONS.Transfer order] was already inserted'
END
GO