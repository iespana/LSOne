
/*

	Incident No.	: 
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET Milky Way/Sprint 2 Mercury
	Date created	: 15.08.2012
	
	Description		: Add column SCREENALIGNMENT to table KMINTERFACEPROFILELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : KMINTERFACEPROFILELog - Add column SCREENALIGMENT
						
*/


IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILELog') AND NAME='SCREENALIGNMENT')
BEGIN
	ALTER TABLE dbo.KMINTERFACEPROFILELog add SCREENALIGNMENT int null
END
GO

