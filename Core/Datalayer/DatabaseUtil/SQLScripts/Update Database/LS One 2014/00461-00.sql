
/*

	Incident No.	: 26966
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS One 2014 - Stratus
	Date created	: 04.11.13

	Description		: Adding "triggered automatic/manual" settings to periodic discounts
	
						
*/
USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSPERIODICDISCOUNT' AND COLUMN_NAME = 'TRIGGERED')
BEGIN
	ALTER TABLE POSPERIODICDISCOUNT ADD TRIGGERED INT
END



