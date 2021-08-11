/*
	Incident No.	: ONE-10059 Prepare receipt examples for certification
	Responsible		: Andrei Sonka
	Sprint			: Capella
	Date created	: 23.05.2019

	Description		: Added Print fiscal slip option to POS operations
    Remarks			: Also related to ONE-10062 Print informational slip from POS
*/

USE LSPOSNET

-- add POS operation for Print fiscal info slip
IF NOT EXISTS (SELECT 1 FROM OPERATIONS WHERE ID = '920')
BEGIN
    INSERT INTO dbo.OPERATIONS(MASTERID, ID, DESCRIPTION, TYPE, LOOKUPTYPE, AUDIT)
	VALUES  (NEWID(), '920', 'Print fiscal info slip', 2, 0, NULL)
END
ELSE
BEGIN
	PRINT '[OPERATIONS.Print fiscal info slip] was already inserted'
END
GO