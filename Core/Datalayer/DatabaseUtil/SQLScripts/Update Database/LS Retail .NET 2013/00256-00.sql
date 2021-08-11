/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Mars
	Date created	: 19.11.2012

	Description		: Remove table KMSETTINGS

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: KMSETTINGS
						
*/

USE LSPOSNET

GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('KMSETTINGS'))
BEGIN
	Drop table KMSETTINGS
END
GO

