
/*

	Incident No.	: 
	Responsible		: Sigfús Jóhannesson
	Sprint			: 
	Date created	: 20.05.14 / 15.07.14  Merged from previous version

	Description		: Adding standard scale units to hardware profile.
	
						
*/
USE LSPOSNET
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOPARAMETERS' AND COLUMN_NAME = 'SCALEGRAMUNIT')
BEGIN
	ALTER TABLE RBOPARAMETERS ADD SCALEGRAMUNIT NVARCHAR(20) NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOPARAMETERS' AND COLUMN_NAME = 'SCALEKILOGRAMUNIT')
BEGIN
	ALTER TABLE RBOPARAMETERS ADD SCALEKILOGRAMUNIT NVARCHAR(20) NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOPARAMETERS' AND COLUMN_NAME = 'SCALEOUNCEUNIT')
BEGIN
	ALTER TABLE RBOPARAMETERS ADD SCALEOUNCEUNIT NVARCHAR(20) NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOPARAMETERS' AND COLUMN_NAME = 'SCALEPOUNDUNIT')
BEGIN
	ALTER TABLE RBOPARAMETERS ADD SCALEPOUNDUNIT NVARCHAR(20) NULL
END



