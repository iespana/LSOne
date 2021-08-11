/*

	Incident No.	: N/A
	Responsible		: Hörður Kristjánsson
	Sprint			: DotNetPM\LS Retail .NET 2011 SP3
	Date created	: 19.04.2011

	Description		: Deleting a duplicate Manage Retail Departments permission

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: PERMISSIONS	-	Data removed
						
*/

USE LSPOSNET

GO

IF EXISTS(SELECT * FROM PERMISSIONS WHERE GUID = '2D325910-AC36-11DF-94E2-0800200C9A66')
begin
	DELETE FROM PERMISSIONS WHERE GUID = '2D325910-AC36-11DF-94E2-0800200C9A66'
end


GO