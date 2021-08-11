
/*

	Incident No.	: 14097
	Responsible		: Guðbjörn Einarsson
	Sprint			: LS Retail .NET 2012\Höður
	Date created	: 20.03.2012

	Description		: Adding field to table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: RBOSTORETABLE - Added field StorePriceSetting and remove field PRICEINCLUDESALESTAX
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBOSTORETABLE' AND COLUMN_NAME='StorePriceSetting')
Begin
	alter table RBOSTORETABLE add StorePriceSetting int NULL Default(0)
End
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='RBOSTORETABLE' AND COLUMN_NAME='PRICEINCLUDESALESTAX')
Begin
	alter table RBOSTORETABLE drop column PRICEINCLUDESALESTAX
End
GO