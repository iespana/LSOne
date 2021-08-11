
/*

	Incident No.	: 21646
	Responsible		: Oskar Bjarnason	
	Sprint			: Neptune
	Date created	: 11.2.2013

	Description		: Change operations to not be user operations anymore

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PosMenuHeader
						
*/

USE LSPOSNET

 if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSMENUHEADER' and COLUMN_NAME = 'DEFAULTOPERATION')
 begin
 alter table POSMENUHEADER add DEFAULTOPERATION int NULL

 end




