
/*

	Incident No.	: 15353
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012\Embla
	Date created	: 22.02.2012

	Description		: Adding fields to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSMENUHEADER - new field MenuType added
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSMENUHEADER' AND COLUMN_NAME='MENUTYPE')
Begin
	Alter table POSMENUHEADER Add MENUTYPE int Null DEFAULT(0)
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='POSMENUHEADER' AND COLUMN_NAME='MENUTYPE')
Begin
	update POSMENUHEADER set MENUTYPE = 0 where MENUTYPE is null
END
GO