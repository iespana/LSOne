/*
	Incident No.	: ONE-6359
	Responsible		: Hörður Kristjánsson
	Sprint			: Färgrik
	Date created	: 24.05.2017

	Description		: Expanded the length of ExCode to make sure both store- and termial IDs will fit
*/

USE LSPOSNET

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscLocations' AND COLUMN_NAME = 'ExCode' AND CHARACTER_MAXIMUM_LENGTH < 50)
BEGIN
	ALTER TABLE JscLocations
	ALTER COLUMN ExCode NVARCHAR(50)
END
GO