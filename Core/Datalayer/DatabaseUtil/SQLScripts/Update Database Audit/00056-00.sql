/*

	Incident No.	: xxx
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Earth
	Date created	: 06.11.2012

	Description		: Remove columns SCREENALIGNMENT, SCREENNUMBER from KMINTERFACEPROFILELog and add them to table RESTAURANTSTATIONLog
	
	
	Tables affected	: KMINTERFACEPROFILELog and RESTAURANTSTATIONLog
						
*/
USE LSPOSNET_Audit

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KMINTERFACEPROFILELog' AND COLUMN_NAME = 'SCREENALIGNMENT')
BEGIN
	ALTER TABLE KMINTERFACEPROFILELog DROP COLUMN SCREENALIGNMENT
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KMINTERFACEPROFILELog' AND COLUMN_NAME = 'SCREENNUMBER')
BEGIN
	ALTER TABLE KMINTERFACEPROFILELog DROP COLUMN SCREENNUMBER
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RESTAURANTSTATIONLog' AND COLUMN_NAME = 'SCREENNUMBER')
BEGIN
	ALTER TABLE RESTAURANTSTATIONLog ADD SCREENNUMBER INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RESTAURANTSTATIONLog' AND COLUMN_NAME = 'SCREENALIGNMENT')
BEGIN
	ALTER TABLE RESTAURANTSTATIONLog ADD SCREENALIGNMENT INT NULL
END
GO