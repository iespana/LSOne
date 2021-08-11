/*
	Incident No.	: N/A
	Responsible		: Helgi Rúnar Gunnarsson
	Sprint			: LS One 2015
	Date created	: 02.02.2015

	Description		: Adding new operation Item sale report
						
*/
USE LSPOSNET
GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 918)
Begin
  insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  values (918, 'Print item sales report',	NULL,	NULL,	1,	1,'LSR', 0)
End
