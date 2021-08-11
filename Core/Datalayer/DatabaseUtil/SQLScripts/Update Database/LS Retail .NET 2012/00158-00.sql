
/*

	Incident No.	: 14681
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS Retail .NET 2012/Askur
	Date created	: 18.1.2012

	Description		: Add a new operation

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosisOperations - new operation added
						
*/

IF NOT EXISTS (SELECT * FROM [POSISOPERATIONS] WHERE [OPERATIONID] = 1500 and [DATAAREAID] = 'LSR')
INSERT POSISOPERATIONS(OPERATIONID,OPERATIONNAME,CHECKUSERACCESS,USEROPERATION,DATAAREAID) VALUES('1500','Open menu','1','1','LSR')
GO