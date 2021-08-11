
/*

	Incident No.	: 
	Responsible		: Gudbjorn Einarsson
	Sprint			: 
	Date created	: 05.12.2012
	
	Description		: Remove ROUTEID from table STATIONSELECTIONLog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : STATIONSELECTIONLog
						
*/


IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('STATIONSELECTIONLog') AND NAME='ROUTEID')
BEGIN
	ALTER TABLE dbo.STATIONSELECTIONLog drop column ROUTEID
END
GO

