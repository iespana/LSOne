/*
	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 13.01.2014

	Description		: Adding new Job Fields
	
						
*/
USE LSPOSNET
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobs' AND COLUMN_NAME = 'JobTypeCode') 
BEGIN
  ALTER TABLE JscJobs ADD JobTypeCode VARCHAR(30) NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JscJobs' AND COLUMN_NAME = 'UseCurrentLocation') 
BEGIN
  ALTER TABLE JscJobs ADD UseCurrentLocation BIT NULL 
END
GO
