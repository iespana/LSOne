/*
	Incident No.	: ONE-6530
	Responsible		: Hörður Kristjánsson
	Sprint			: Billy
	Date created	: 03.04.2017

	Description		: Added column to POSHARDWAREPROFILE
*/

USE LSPOSNET

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'STATIONPRINTINGHOSTID' AND TABLE_NAME = 'POSHARDWAREPROFILE')
BEGIN
	ALTER TABLE POSHARDWAREPROFILE ADD [STATIONPRINTINGHOSTID] NVARCHAR(20) NULL
END
GO