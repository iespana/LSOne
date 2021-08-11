
/*

	Incident No.	: N/A
	Responsible		: Marý Björk Steingrímsdóttir
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 30.8.2013

	Description		: [Short description]	
	
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM POSISOPERATIONS WHERE OPERATIONID = 133)
Begin
  insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
  values (133, 'Loyalty points discount',	NULL,	NULL,	1,	1, 'LSR', 2)
End




