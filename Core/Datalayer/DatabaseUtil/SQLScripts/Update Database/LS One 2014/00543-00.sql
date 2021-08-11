
/*

	Incident No.	: 
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: 
	Date created	: 24.07.14

	Description		: Adding timestamp to gift cards, used when they are issued on POS
	
						
*/
USE LSPOSNET
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOGIFTCARDTABLE' AND COLUMN_NAME = 'DATECREATED')
BEGIN
	ALTER TABLE RBOGIFTCARDTABLE ADD DATECREATED DATETIME NULL
END



