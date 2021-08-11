/*

	Incident No.	: 20859
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2013\Uranus
	Date created	: 09.01.2012

	Description		: Changing an operation to user operation	
	
	Tables affected	: POSISOPERATIONS
						
*/
USE LSPOSNET

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISOPERATIONS' AND COLUMN_NAME='LOOKUPTYPE')
Begin
  -- Lookup type for this operations should be StorePaymentType
  update POSISOPERATIONS set USEROPERATION = 1, LOOKUPTYPE = 2 where OPERATIONID = 207
End