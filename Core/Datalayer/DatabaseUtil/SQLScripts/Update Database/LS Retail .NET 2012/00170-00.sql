
/*

	Incident No.	: 15921
	Responsible		: Hörður Kristjánsson
	Sprint			: LS Retail .NET 2012\Höður
	Date created	: 15.03.2012

	Description		: Adding field to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSMENULINE - Added field ImagePosition
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='POSMENULINE' AND COLUMN_NAME='IMAGEPOSITION')
Begin
	alter table POSMENULINE add IMAGEPOSITION int NULL Default(0)
End
GO