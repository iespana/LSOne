/*

	Incident No.	: ONE-6575
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: Hemnes - (26.4-9.5)
	Date created	: 27.04.2017

	Description		: Adding a new operation
	
	
	Tables affected	: POSISOPERATIONS
						
*/


USE LSPOSNET
GO

  IF NOT EXISTS (SELECT 'x' FROM POSISOPERATIONS WHERE OPERATIONID='618')
Begin
	insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE,AUDIT)
	values (618, 'Edit customer',	NULL,	NULL,	1,	1,'LSR',0,1)
End
GO