
/*

	Incident No.	: 
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012
	Date created	: 09.03.2012

	Description		: Removing data from table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosisOperations - Removed operation PayTransaction with id 205
						
*/

USE LSPOSNET

GO

IF EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 205)
BEGIN
    delete from POSISOPERATIONS where OPERATIONID = 205
END

GO