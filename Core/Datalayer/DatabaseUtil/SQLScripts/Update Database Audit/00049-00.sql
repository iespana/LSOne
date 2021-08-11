
/*

	Incident No.	: 
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET Milky Way/Sprint 2 Mercury
	Date created	: 30.07.2012
	
	Description		: Add column KITCHENDISPLAYPROFILEID to table RESTAURANTSTATIONLog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : RESTAURANTSTATIONLog - Add column KITCHENDISPLAYPROFILEID
						
*/


IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('RESTAURANTSTATIONLog') AND NAME='KITCHENDISPLAYPROFILEID')
BEGIN
	ALTER TABLE dbo.RESTAURANTSTATIONLog add KITCHENDISPLAYPROFILEID nvarchar(20) null
END
GO

