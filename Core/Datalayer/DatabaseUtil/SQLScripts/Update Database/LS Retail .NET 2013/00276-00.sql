/*

	Incident No.	: 20861
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2013\Uranus
	Date created	: 10.01.2012

	Description		: Adding new operation "Add customer to loyalty card"
	
	Tables affected	: POSISOPERATIONS
						
*/
USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 1101)
Begin
  insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  values (1101, 'Add customer to loyalty card',	NULL,	NULL,	1,	1,'LSR',0)
End