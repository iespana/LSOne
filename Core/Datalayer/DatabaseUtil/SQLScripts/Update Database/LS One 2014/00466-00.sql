
/*

	Incident No.	: 27186
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2014 - Stratus
	Date created	: 03.12.13

	Description		: Updating Sales person operation
	
						
*/
USE LSPOSNET
GO

UPDATE POSISOPERATIONS
SET OPERATIONNAME = 'Sales person', LOOKUPTYPE = 16
WHERE OPERATIONID = 502
AND DATAAREAID = 'LSR'
GO

UPDATE POSISOPERATIONS
SET OPERATIONNAME = 'Clear sales person'
WHERE OPERATIONID = 121
AND DATAAREAID = 'LSR'
GO


