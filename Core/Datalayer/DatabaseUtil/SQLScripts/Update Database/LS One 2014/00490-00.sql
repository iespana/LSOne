/*
	Incident No.	: N/A
	Responsible		: Birgir Kristmannsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 13.01.2014

	Description		: Fixed column type
	
						
*/
USE LSPOSNET
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSFORMTYPE' AND COLUMN_NAME = 'SYSTEMTYPE') 
BEGIN
  ALTER TABLE POSFORMTYPE ALTER COLUMN SYSTEMTYPE integer
END
GO