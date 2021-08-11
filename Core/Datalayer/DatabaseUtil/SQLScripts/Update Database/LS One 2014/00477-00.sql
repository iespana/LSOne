/*

	Incident No.	: 
	Responsible		: Hörður Kristjánsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 23.12.2013

	Description		: Added field to RBOTERMINALTABLE
*/

USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTERMINALTABLE' AND COLUMN_NAME = 'StatementPosting')
BEGIN
	alter table RBOTERMINALTABLE ADD  StatementPosting INT
END