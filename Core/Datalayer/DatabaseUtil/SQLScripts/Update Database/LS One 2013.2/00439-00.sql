/* 
        Incident No.: N/A 
        Responsible : Indriði Ingi Stefánsson
        Sprint		: LS One 2013.2\July, August, September
        Date created: 25.10.13 
        Description	: Increase namealias column length for items
*/ 
USE LSPOSNET 
GO 

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTTABLE' AND COLUMN_NAME = 'NAMEALIAS' AND CHARACTER_MAXIMUM_LENGTH = 60)
And  EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'INVENTTABLE' AND COLUMN_NAME = 'NAMEALIAS' )
BEGIN
	alter table [dbo].INVENTTABLE ALTER COLUMN NAMEALIAS NVARCHAR(60)	
END