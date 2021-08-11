/*

	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 24.9.2013

	Description		: Added a new column for staff login
*/

USE LSPOSNET

GO

if not exists (select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'POSFUNCTIONALITYPROFILE' and COLUMN_NAME = 'CLEARUSERBETWEENLOGINS')
begin
	alter table POSFUNCTIONALITYPROFILE add CLEARUSERBETWEENLOGINS smallint
end
