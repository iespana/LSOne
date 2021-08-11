
/*
	Incident No.	: N/A
	Responsible		: Gudbjorn Einarsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 25.02.2014

	Description		: Adding station letter field to kitchen display station table
					  Adding operation Scan QR to POS
						
*/
USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'KITCHENDISPLAYSTATION' AND COLUMN_NAME = 'STATIONLETTER')
BEGIN
	ALTER TABLE KITCHENDISPLAYSTATION ADD STATIONLETTER nvarchar(10) null
END

GO

