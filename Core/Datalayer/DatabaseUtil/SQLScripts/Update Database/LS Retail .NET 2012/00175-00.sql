/*

	Incident No.	: 16150
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012\Embla
	Date created	: 30.03.2012

	Description		: Altering row in table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISOPERATIONS - Changed operationID 600
						
*/

USE LSPOSNET

GO


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISOPERATIONS' AND COLUMN_NAME='USEROPERATION')

delete from POSISOPERATIONS 
where OPERATIONID = '600'

insert into POSISOPERATIONS ( OPERATIONID, OPERATIONNAME, PERMISSIONID, PERMISSIONID2, CHECKUSERACCESS, USEROPERATION,DATAAREAID,LOOKUPTYPE)
values (600, 'Customer',	NULL,	NULL,	1,	0,'LSR',0)


GO
