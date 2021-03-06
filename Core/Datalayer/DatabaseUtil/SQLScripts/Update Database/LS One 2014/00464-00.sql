
/*

	Incident No.	: XXX
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS One 2014 - Stratus
	Date created	: 25.11.13

	Description		: Adding an extra column for recalled orders location to KITCHENDISPLAYFUNCTIONALPROFILE
	
						
*/
USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KITCHENDISPLAYFUNCTIONALPROFILE' AND COLUMN_NAME = 'RECALLEDORDERSAPPEAR')
BEGIN
	ALTER TABLE KITCHENDISPLAYFUNCTIONALPROFILE ADD RECALLEDORDERSAPPEAR INT NOT NULL DEFAULT 0
END



