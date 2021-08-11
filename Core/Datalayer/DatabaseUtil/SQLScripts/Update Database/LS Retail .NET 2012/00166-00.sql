
/*

	Incident No.	: 15507
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012\Embla
	Date created	: 07.03.2012

	Description		: Altering row in table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISOPERATIONS - Changed operationID 612 
						
*/

USE LSPOSNET

GO


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISOPERATIONS' AND COLUMN_NAME='USEROPERATION')

delete from POSISOPERATIONS 
where OPERATIONID = '612'

insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
values (612, 'Customer Add',	NULL,	NULL,	1,	0,'LSR',0)


GO
