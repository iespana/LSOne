/*

	Incident No.	: 
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013\Sprint 2
	Date created	: 27.8.2013

	Description		: Adding new operation "Run POS plugin"
	
	Tables affected	: POSISOPERATIONS
						
*/
USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 917)
Begin
  insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  values (917, 'Execute POS plugin',	NULL,	NULL,	1,	1,'LSR',10)
End