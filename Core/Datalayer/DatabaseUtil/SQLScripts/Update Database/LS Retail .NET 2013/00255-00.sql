/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS Retail .NET 2013\Mars
	Date created	: 16.11.2012

	Description		: Add KEYMAPPING to POSMENULINE

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSMENULINE
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('POSMENULINE') AND NAME='KEYMAPPING')
BEGIN
	Alter table POSMENULINE Add KEYMAPPING int null
END
GO

