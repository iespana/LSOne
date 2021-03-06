
/*

	Incident No.	: N/A
	Responsible		: Björn Eiríksson
	Sprint			: 2011 - Sprint 5
	Date created	: 12.01.2011

	Description		: Adds indexes to the REPLICATIONACTIONS table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: REPLICATIONACTIONS
						
*/

USE LSPOSNET

GO

if not Exists(SELECT * FROM sys.indexes where object_id = OBJECT_ID('REPLICATIONACTIONS') AND NAME = 'INDEX_DateCreated')
begin
	CREATE INDEX INDEX_DateCreated
	ON dbo.REPLICATIONACTIONS (DateCreated)
	
	CREATE INDEX INDEX_ObjectName
	ON dbo.REPLICATIONACTIONS (ObjectName)
end