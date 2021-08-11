
/*

	Incident No.	: 
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET Milky Way/Sprint 2 Mercury
	Date created	: 27.06.2012
	
	Description		: Add table STATIONSELECTIONLog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : STATIONSELECTIONLog - Changing column in table
						
*/


IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('STATIONSELECTIONLog') AND NAME='ROUTEID')
BEGIN
	ALTER TABLE dbo.STATIONSELECTIONLog drop column ROUTEID
END
GO


IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('STATIONSELECTIONLog') AND NAME='ROUTEID')
BEGIN
	ALTER TABLE dbo.STATIONSELECTIONLog add ROUTEID nvarchar(20) null
END
GO

