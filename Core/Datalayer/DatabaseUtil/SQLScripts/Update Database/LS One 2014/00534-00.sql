
/*
	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: LS One 2014
	Date created	: 05.06.2014

	Description		: Adding new operation Authorize Card Quick
						
*/
USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 216)
Begin
  insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  values (216, 'Authorize card quick',	NULL,	NULL,	1,	1,'LSR',2)
End