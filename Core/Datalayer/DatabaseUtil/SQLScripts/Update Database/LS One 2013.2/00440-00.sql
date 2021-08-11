
/*

	Incident No.	: N/A
	Responsible		: Höður Sigurdór Heiðarsson
	Sprint			: LS One 2013.2\July, August, September
	Date created	: 29.10.2013

	Description		: Resize the Text column in POSISINFO table
					  This is a rerun of script 00382-00 which had build action as content and not embedded resource
						
*/

USE LSPOSNET

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'POSISINFO' AND COLUMN_NAME = 'TEXT' AND CHARACTER_MAXIMUM_LENGTH < 200)
BEGIN
	ALTER TABLE POSISINFO ALTER COLUMN TEXT NVARCHAR(200) NULL
END




