/*

	Incident No.	: 16188
	Responsible		: Óskar Bjarnason
	Sprint			: LS Retail .NET 2012\Embla
	Date created	: 11.04.2012

	Description		: Altering row in table

	Logic scripts   : No stored procedures added or changed
	
	Tables affected	: POSISOPERATIONS - Changed operationID 611
						
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME='POSVISUALPROFILE' AND COLUMN_NAME='SHOWCURRENCYSYMBOLONCOLUMNS')
	alter table POSVISUALPROFILE add SHOWCURRENCYSYMBOLONCOLUMNS tinyint NULL
GO
