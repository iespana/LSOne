/*
	Incident No.	: N/A
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS One 2014 - Stratus
	Date created	: 09.12.13

	Description		: Added INCLUDEINSTATEMENT to RBOTERMINALTABLE
	
						
*/
USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOTERMINALTABLE' AND COLUMN_NAME = 'INCLUDEINSTATEMENT')
BEGIN
	ALTER TABLE RBOTERMINALTABLE ADD INCLUDEINSTATEMENT bit not null default 1
END