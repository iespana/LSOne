
/*

	Incident No.	: 27037
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS One 2014 - Stratus
	Date created	: 04.11.13

	Description		: Adding pos operation to clear a triggered periodic discount
	
						
*/
USE LSPOSNET

GO

IF NOT EXISTS (SELECT 'x' FROM POSISOPERATIONS WHERE OPERATIONID='305')
Begin
	insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
	values (305, 'Clear periodic discount',	NULL,	NULL,	1,	1,'LSR',0)
End


