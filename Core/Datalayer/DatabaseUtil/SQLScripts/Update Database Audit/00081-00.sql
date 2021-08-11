/*

	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 24.9.2013

	Description		: Added a new column for staff login
*/

USE LSPOSNET_Audit

GO

if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSFUNCTIONALITYPROFILELog' and COLUMN_NAME = 'CLEARUSERBETWEENLOGINS')
begin
	alter table POSFUNCTIONALITYPROFILELog add CLEARUSERBETWEENLOGINS smallint
end
