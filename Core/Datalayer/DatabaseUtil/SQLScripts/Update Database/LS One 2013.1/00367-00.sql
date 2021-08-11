/*

	Responsible		: Gudbjorn Einarsson
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 14.06.2013

	Description		: Remove unit from inventory template table
*/
USE LSPOSNET
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTORYTEMPLATE' AND COLUMN_NAME = 'UNIT')
BEGIN
	ALTER TABLE INVENTORYTEMPLATE DROP COLUMN  UNIT
END
GO