/*

	Incident No.	: N/A
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS One 2013.1\Sprint 2
	Date created	: 26.9.2013

	Description		: Adding db id to POSISINFO table
*/

USE LSPOSNET

GO

if not exists (SELECT [Text] FROM [POSISINFO] WHERE [ID] = 'DATABASEID')
begin
	insert into POSISINFO ([ID], [TEXT]) values ('DATABASEID', cast(NEWID() as nvarchar(50)))
end
