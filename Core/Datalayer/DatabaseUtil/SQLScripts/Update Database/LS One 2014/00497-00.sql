/*
	Incident No.	: N/A
	Responsible		: Indriði Ingi Stefánsson
	Sprint			: LS One 2014 - Nimbo
	Date created	: 21.01.2014

	Description		: Adding Current Location Field
	
						
*/
USE LSPOSNET
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RBOPARAMETERS' AND COLUMN_NAME = 'CURRENTLOCATION') 
BEGIN
 ALTER TABLE RBOPARAMETERS ADD CURRENTLOCATION uniqueidentifier null  
END
GO