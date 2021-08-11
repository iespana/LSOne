
/*

	Incident No.	: 
	Responsible		: Gudbjorn Einarsson
	Sprint			: 
	Date created	: 10.12.2012
	
	Description		: Remove table KMINTERFACEPROFILELog

	Logic scripts   : No stored procedures added or changed
	
	Tables affected : KMINTERFACEPROFILELog
						
*/


IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMINTERFACEPROFILELog'))
BEGIN
	Drop table KMINTERFACEPROFILELog
END
GO

