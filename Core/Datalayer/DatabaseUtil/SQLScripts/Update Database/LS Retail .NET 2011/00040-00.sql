
/*

	Incident No.	: N/A
	Responsible		: Guðbjörn Einarsson
	Sprint			: 2011 - Sprint 5
	Date created	: 04.01.2011

	Description		: Change a field in PURCHASEORDERS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PURCHASEORDERS - ORDERER field from nvarchar(20) to UniqueIdentifier
*/

USE LSPOSNET

GO
IF 'nvarchar' IN (SELECT data_type FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'PURCHASEORDERS' AND COLUMN_NAME = 'ORDERER')
BEGIN
	Alter Table PURCHASEORDERS Drop Column ORDERER

	Alter Table PURCHASEORDERS Add ORDERER UniqueIdentifier NULL
	
END