/*
	Incident No.	: ONE-10586
	Responsible		: Hera Hjaltadóttir
	Sprint			: Bellatrix
	Date created	: 19.09.2019

	Description		: Add column INVENTORYTRANSFERREQUESTLINEID to INVENTORYTRANSFERORDERLINE
*/
USE LSPOSNET

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTORYTRANSFERORDERLINE' AND COLUMN_NAME = 'INVENTORYTRANSFERREQUESTLINEID')
BEGIN
	ALTER TABLE INVENTORYTRANSFERORDERLINE ADD INVENTORYTRANSFERREQUESTLINEID UNIQUEIDENTIFIER NULL
	EXECUTE spDB_SetFieldDescription_1_0 'INVENTORYTRANSFERORDERLINE', 'INVENTORYTRANSFERREQUESTLINEID', 'The related transfer request line ID';
END