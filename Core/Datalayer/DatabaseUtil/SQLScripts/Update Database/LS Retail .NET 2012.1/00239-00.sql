/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Mercury
	Date created	: 16.09.2012

	Description		: Add bump operation to POSISOPERATION
	
	
	Tables affected	: POSISOPERATION
						
*/
USE LSPOSNET
GO
  
IF NOT EXISTS (SELECT 'x' FROM POSISOPERATIONS WHERE OPERATIONID='1305')
Begin
	insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
	values (1305, 'Bump order',	NULL,	NULL,	1,	1,'LSR',0)
End
