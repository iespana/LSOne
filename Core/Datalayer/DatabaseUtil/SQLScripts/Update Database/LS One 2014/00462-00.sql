
/*

	Incident No.	: 26965
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS One 2014 - Stratus
	Date created	: 04.11.13

	Description		: Adding pos operation to trigger a periodic discount manually
	
						
*/
USE LSPOSNET

GO

IF NOT EXISTS (SELECT 'x' FROM POSISOPERATIONS WHERE OPERATIONID='304')
Begin
	insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
	values (304, 'Manually trigger periodic discount',	NULL,	NULL,	1,	1,'LSR',15)
End


