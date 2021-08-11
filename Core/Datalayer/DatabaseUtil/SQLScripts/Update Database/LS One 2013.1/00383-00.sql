/*
	Incident No.	: 24678
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: LS One 2013.1\Sprint One
	Date created	: 9.7.2013

	Description		: Add a column to contain the site service profile.
*/

USE LSPOSNET

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOPARAMETERS' AND COLUMN_NAME = 'SITESERVICEPROFILE' )
BEGIN
	ALTER TABLE RBOPARAMETERS ADD  SITESERVICEPROFILE NVARCHAR(60) NULL
END


