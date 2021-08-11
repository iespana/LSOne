
/*

	Incident No.	: 15498
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012\Embla
	Date created	: 22.02.2012

	Description		: Adding fields to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISOPERATIONS - new field LookupType added
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISOPERATIONS' AND COLUMN_NAME='LOOKUPTYPE')
Begin
	Alter table POSISOPERATIONS Add LOOKUPTYPE int Null DEFAULT(0)
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSISOPERATIONS' AND COLUMN_NAME='LOOKUPTYPE')
Begin
	update POSISOPERATIONS set LOOKUPTYPE = 0 where LOOKUPTYPE is null
END
GO