/*

	Incident No.	: 
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 23.12.2013

	Description		: Added field to KitchenDisplayVisualProfile
*/

USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KITCHENDISPLAYVISUALPROFILE' AND COLUMN_NAME = 'SHOWNAME')
BEGIN
	alter table KITCHENDISPLAYVISUALPROFILE ADD  SHOWNAME Bit not null default 1
END