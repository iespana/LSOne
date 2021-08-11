/* 
        Incident No.: N/A 
        Responsible : Indriði Ingi Stefánsson
        Sprint		: LS One 2013.2\July, August, September
        Date created: 29.10.13 
        Description	: Increase namealias column length for items
*/ 
USE LSPOSNET_Audit 
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTTABLELog' AND COLUMN_NAME = 'NAMEALIAS' AND CHARACTER_MAXIMUM_LENGTH = 60)
And EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTTABLELog' AND COLUMN_NAME = 'NAMEALIAS')
BEGIN
	alter table [dbo].INVENTTABLELog ALTER COLUMN NAMEALIAS NVARCHAR(60)	
END