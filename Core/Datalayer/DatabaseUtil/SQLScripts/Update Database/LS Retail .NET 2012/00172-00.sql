
/*

	Incident No.	: 15037
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2012\Hel
	Date created	: 20.03.2012

	Description		: Update lookup parameters on two operations

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISOPERATIONS - updated data in the LOOKUPTYPE field
						
*/

USE LSPOSNET
GO

-- Update operation Sales Person to be available in button setup
update POSISOPERATIONS set USEROPERATION = 1 where OPERATIONID = 502 -- Sales Person

GO
